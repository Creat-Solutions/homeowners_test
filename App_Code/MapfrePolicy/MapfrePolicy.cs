//using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using System.Xml;
using System.Xml.Linq;
using ME.Admon;
using NETInteractions_Hayw.MailSettings;

public class MapfrePolicy : IHousePolicy
{
   private bool m_isTest = false;
   private int quoteid = 0;
   private HouseInsuranceDataContext db = null;
   private string m_error = string.Empty;
   private string m_hostname = string.Empty;
   private bool m_mapfreError = false;
   private MapfreElectronicInvoiceParser m_feParser = null;
   private MapfreFEPADREInvoiceParser m_fepadreParser = null;

   public string PolicyNo { get; private set; }
   public int PolicyID { get; private set; }
   public string XMLString { get; private set; }

   private string GetPolicyNoFromWebService(quote data)
   {
      ServicePointManager.SecurityProtocol = (SecurityProtocolType) 3072; //TLS 1.2
      MapfrePolicyXMLGenerator xmlFormat = new MapfrePolicyXMLGenerator(db, data, m_isTest);
      string resultxml = xmlFormat.GetXml();

      XMLString = resultxml;
      string policyno = null;
      string token = GetToken(m_isTest);

      try
      {
         MapfreWebServices webservice = MapfreWebServicesFactory.GetServices(m_isTest);
         XmlNode rootnode = webservice.WM_XMLBuzonesGeneral(resultxml, token);
         XElement root = XElement.Load(new XmlNodeReader(rootnode));

         string result = root.Descendants(XName.Get("param")).Descendants().
               Where(m => m.Name.LocalName == "result").Select(m => m.Value).Single();

         if (result.Equals("true", StringComparison.CurrentCultureIgnoreCase))
         {
            policyno = root.Descendants(XName.Get("param")).Descendants().
                  Where(m => m.Name.LocalName.Equals("POLIZA", StringComparison.CurrentCultureIgnoreCase)).Select(m => m.Value).Single();
            if (m_isTest)
            {
               policyno = "P" + policyno;
               policyTest pol = new policyTest
               {
                  fecha = DateTime.Now,
                  policy_no = policyno
               };
               db.policyTests.InsertOnSubmit(pol);
               db.SubmitChanges();
            }
            //generar datos de factura electronica solo si tiene RFC
            if (!string.IsNullOrEmpty(data.insuredPerson1.rfc))
            {
               try
               {
                  string encodedFE = root.Descendants(XName.Get("param"))
                                                .Descendants()
                                                .Where(m => m.Name.LocalName.Equals("FE", StringComparison.CurrentCultureIgnoreCase))
                                                .Select(m => m.Value)
                                                .Single();
                  m_feParser = new MapfreElectronicInvoiceParser(root.Value, encodedFE);
               } catch (Exception e)
               {
                  SendEmailNoFEData(root.Value, policyno);
               }

               try
               {
                  string encodedFEPADRE = root.Descendants(XName.Get("param"))
                                                            .Descendants()
                                                            .Where(m => m.Name.LocalName.Equals("FEPADRE", StringComparison.CurrentCultureIgnoreCase))
                                                            .Select(m => m.Value)
                                                            .Single();
                  m_fepadreParser = new MapfreFEPADREInvoiceParser(root.Value, encodedFEPADRE);
               } catch (Exception e)
               {
                  SendEmailNoFEData(root.Value, policyno);
               }
            }
         } else
         {
            policyno = null;
            m_error = root.Value;
            m_mapfreError = true;
         }
      } catch (Exception e)
      {
         policyno = null;
         m_error = e.Message;
      }
      return policyno;
   }

   private string GetToken(bool m_isTest)
   {
      string token = null;
      //EJDP: 10/26/2022
      //1.se cambia el parametro para obtener el token al web.config
      //2.se cambia la estructura para obtener el token y evitar error de token vencido
      string app_name = ConfigurationSettings.AppSettings["app_name"];
      try
      {
         ServicePointManager.SecurityProtocol = (SecurityProtocolType) 3072; //TLS 1.2
         MapfreTokenServices tokenService = MapfreTokenServicesFactory.GetServices(m_isTest);
         XmlNode tokenRootNode = tokenService.ObtenToken_ws(app_name);
         XElement tokenRoot = XElement.Load(new XmlNodeReader(tokenRootNode));
         if (tokenRoot.Name.LocalName == "TokenGenerado")
         {
            token = tokenRoot.Value;
         } else
         {
            tokenRootNode = tokenService.GeneraToken_ws(app_name);
            tokenRoot = XElement.Load(new XmlNodeReader(tokenRootNode));
            if (tokenRoot.Name.LocalName == "TokenGenerado")
            {
               token = tokenRoot.Value;
            }
         }
      } catch (Exception e)
      {
         throw e;
      }
      return ValidaToken(m_isTest, token);
   }

   private string ValidaToken(bool m_isTest, string token)
   {
      ServicePointManager.SecurityProtocol = (SecurityProtocolType) 3072; //TLS 1.2
      string app_name = ConfigurationSettings.AppSettings["app_name"];

      try
      {
         MapfreTokenServices tokenService = MapfreTokenServicesFactory.GetServices(m_isTest);

         XmlNode tokenRootNode = tokenService.ValidaToken_ws(token);
         XElement tokenRoot = XElement.Load(new XmlNodeReader(tokenRootNode));
         if (tokenRoot.Name.LocalName == "TokenValido")
         {
            string valido = tokenRoot.Value;
            if (valido == "True")
            {
               return token;
            } else
            {
               tokenRootNode = tokenService.GeneraToken_ws(app_name);
               tokenRoot = XElement.Load(new XmlNodeReader(tokenRootNode));
               if (tokenRoot.Name.LocalName == "TokenGenerado")
               {
                  token = tokenRoot.Value;
               }
            }
         }
      } catch (Exception e)
      {
         throw e;
      }

      return token;
   }

   private void SendEmailNoFEData(string response, string policyno)
   {
      string path = AppDomain.CurrentDomain.BaseDirectory + "\\Log.txt";
      if (!File.Exists(path))
      {
         File.Create(path).Close();
      }

      try
      {
         string[] emails = null;

         try
         {
            csProduct product = new csProduct(ApplicationConstants.APPLICATION, ApplicationConstants.ADMINDB);
            emails = Array.ConvertAll(product.ErrorEmails.Split(','), a => a.Trim());
         } catch
         {
            emails = new string[] { "eric@creatsol.com", "dalitza@creatsol.com" };
         }

         string to = string.Empty;

         to = m_isTest && m_hostname == "localhost"
               ? "eric@creatsol.com"
               : emails.Length > 0 ? String.Join(", ", emails) : "eric@creatsol.com";

         byte[] decoded = System.Text.Encoding.UTF8.GetBytes(response);
         MemoryStream stream = new MemoryStream(decoded);

         //EmailService emailService = new EmailService(to, true);

         //if (m_hostname != "localhost" && !m_isTest)
         //{
         //    foreach (var email in emails)
         //    {
         //        emailService.AddCC(email);
         //    }
         //}

         //emailService.AddAttachment(stream, "Response" + policyno + ".xml", "text/xml; charset=UTF-8");
         //emailService.Subject = "Homeowner's Insurance Application Warning con Factura Electronica (" + policyno + ")";
         //emailService.Body = "La poliza fue generada correctamente con el numero: " + policyno + ", pero los datos de la factura electronica" +
         //                    " no fueron procesados correctamente.";
         //emailService.Send();

         string xmlPath = AppDomain.CurrentDomain.BaseDirectory + "\\Response" + policyno + ".xml";
         File.WriteAllBytes(xmlPath, decoded);

         EmailSettings _mail = new EmailSettings();
         _mail.alias = ConfigurationSettings.AppSettings["mail_alias"];
         _mail.toAddress = new System.Net.Mail.MailAddressCollection();
         _mail.toAddress.Add(to);
         _mail.subject = "Homeowner's Insurance Application Warning con Factura Electronica (" + policyno + ")";
         _mail.attachFilesPath = new List<string> { xmlPath };

         _mail.body = "La poliza fue generada correctamente con el numero: " + policyno + ", pero los datos de la factura electronica" +
               " no fueron procesados correctamente.";
         _mail.bodyHTML = true;
         _mail.CreateMail();

         if (!string.IsNullOrEmpty(_mail.Alert))
         {
            File.AppendAllText(
            path,
            DateTime.Now.ToString() + Environment.NewLine +
                  _mail.Alert
            );
         }

         File.Delete(xmlPath);
      } catch (Exception ex)
      {
         File.AppendAllText(
               path,
               DateTime.Now.ToString() + Environment.NewLine +
                     ex.Message + Environment.NewLine +
                     ex.StackTrace + Environment.NewLine + Environment.NewLine
               );
      }
   }

   private void SendEmailWithXMLString()
   {
      string path = AppDomain.CurrentDomain.BaseDirectory + "\\Log.txt";
      if (!File.Exists(path))
      {
         File.Create(path).Close();
      }
      try
      {
         if (!string.IsNullOrEmpty(m_error))
         {
            File.AppendAllText(path, DateTime.Now.ToString() + Environment.NewLine +
            "Policy: " + PolicyNo + Environment.NewLine +
            "Error: " + m_error);
         }

         string[] emails = null;

         try
         {
            csProduct product = new csProduct(ApplicationConstants.APPLICATION, ApplicationConstants.ADMINDB);
            emails = Array.ConvertAll(product.ErrorEmails.Split(','), a => a.Trim());
         } catch
         {
            emails = new string[] { "eric@creatsol.com", "dalitza@creatsol.com" };
         }

         if (m_isTest && m_hostname == "localhost")
         {
            emails = new string[] { };
         }

         string to = m_mapfreError && !m_isTest && m_hostname != "localhost" ? "Dvillard@mapfre.com.mx" : "eric@creatsol.com";
         List<string> ccList = new List<string>();

         if (m_mapfreError || !string.IsNullOrEmpty(m_error))
         {
            ccList.AddRange(emails.ToList());
         }

         byte[] decoded = System.Text.Encoding.UTF8.GetBytes(XMLString);
         MemoryStream stream = new MemoryStream(decoded);

         //EmailService emailService = new EmailService(to, true);
         //foreach (string cc in ccList)
         //{
         //    emailService.AddCC(cc);
         //}
         //emailService.AddAttachment(stream, "Policy_" + PolicyNo + ".xml", "text/xml; charset=UTF-8");
         //emailService.Subject = "Homeowner's Insurance Application " + (m_isTest ? "Test Error" : "Error") + " XML: " + PolicyNo;
         //emailService.Body = errorRequest + m_error;
         //emailService.Send();

         string xmlPath = AppDomain.CurrentDomain.BaseDirectory + "\\Policy_" + PolicyNo + ".xml";
         File.WriteAllBytes(xmlPath, decoded);

         string errorRequest = string.Empty;
         errorRequest = m_mapfreError && !m_isTest
               ? "A quien corresponda.<br />\n Equipo MAPFRE, ¿Podrían ayudarnos con la razón del error devuelto por su Web Service?<br /> \n<br />\nGracias\n<br />"
               : "Error con el servicio web de MAPFRE al emitir la póliza.";

         string bcc_group = ConfigurationSettings.AppSettings["bcc_group"];

         EmailSettings _mail = string.IsNullOrEmpty(bcc_group) ? new EmailSettings() : new EmailSettings(bcc_group);
         _mail.alias = ConfigurationSettings.AppSettings["mail_alias"];
         _mail.toAddress = new System.Net.Mail.MailAddressCollection();
         _mail.toAddress.Add(to);
         if (ccList.Count > 0)
         {
            _mail.ccAddress = new System.Net.Mail.MailAddressCollection();
            _mail.ccAddress.Add(String.Join(", ", ccList));
         }
         _mail.subject = "Homeowner's Insurance Application " + (m_isTest ? "Test Error" : "Error") + " XML: " + PolicyNo;
         _mail.attachFilesPath = new List<string> { xmlPath };
         _mail.body = errorRequest + m_error;
         _mail.bodyHTML = true;

         if (m_isTest)
         {
            _mail.bccAddress = null;
         }

         _mail.CreateMail();

         if (!string.IsNullOrEmpty(_mail.Alert))
         {
            File.AppendAllText(
            path,
            DateTime.Now.ToString() + Environment.NewLine +
                  _mail.Alert
            );
         }

         File.Delete(xmlPath);
      } catch (Exception ex)
      {
         File.AppendAllText(
               path,
               DateTime.Now.ToString() + Environment.NewLine +
                     ex.Message + Environment.NewLine +
                     ex.StackTrace + Environment.NewLine + Environment.NewLine
               );
      }
   }

   private void ConvertToPolicy(decimal netPremium, decimal taxPercentage, decimal comisionPercentage, decimal exchangeRate)
   {
      PolicyID = 0;
      PolicyNo = null;
      XMLString = null;
      CreateConnection();
      quote cotiza = db.quotes.Where(m => m.id == quoteid).Single();

      PolicyNo = GetPolicyNoFromWebService(cotiza);

      UserInformationDB userInfo = AdminGateway.Gateway.GetUserInformation(cotiza.issuedBy);

      var policyInfo = db.spConvertToPolicy(
            quoteid,
            PolicyNo,
            m_isTest,
            netPremium,
            taxPercentage,
            userInfo.AgentInformation.AgentName,
            userInfo.UserName,
            userInfo.BranchInformation.BranchAddress,
            comisionPercentage,
            ApplicationConstants.Fee(cotiza.currency, cotiza.fee.Value),
            exchangeRate
      ).Single();
      PolicyID = (int) policyInfo.policyId;

      bool errorOnPolicyNo = string.IsNullOrEmpty(PolicyNo);
      if (errorOnPolicyNo)
      {
         PolicyNo = policyInfo.policyNo;
      }
      if (errorOnPolicyNo || m_isTest)
      {
         SendEmailWithXMLString();
      } else
      {
         db.spIns_policyXML(PolicyID, XMLString);
      }

      if (m_feParser != null)
      {
         try
         {
            m_feParser.DecodeAndGenerate(PolicyID);
         } catch
         {
            SendEmailNoFEData(m_feParser.RawXMLResponse, PolicyNo);
         }
      }

      if (m_fepadreParser != null)
      {
         try
         {
            m_fepadreParser.DecodeAndGenerate(PolicyID);
         } catch
         {
         }
      }
   }

   private HouseInsuranceDataContext CreateConnection()
   {
      if (db == null)
      {
         db = new HouseInsuranceDataContext();
      }

      return db;
   }

   private void CloseConnection()
   {
      if (db != null)
      {
         db.Connection.Close();
         db.Dispose();
         db = null;
      }
   }

   public MapfrePolicy(string hostname)
   {
      m_hostname = hostname;
   }

   public bool ConvertToPolicy(int id, bool test, decimal netPremium, decimal taxPercentage, decimal comisionPercentage, decimal exchangeRate)
   {
      quoteid = id;
      m_isTest = test;
      ConvertToPolicy(netPremium, taxPercentage, comisionPercentage, exchangeRate);
      CloseConnection();
      return true;
   }
}
