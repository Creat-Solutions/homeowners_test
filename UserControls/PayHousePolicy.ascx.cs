using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ME.Admon;
using ME.Mexicard;
using Protec.Authentication.Membership;

//using Openpay;
//using Openpay.Entities;
//using Openpay.Entities.Request;

public partial class UserControls_PayHousePolicy : System.Web.UI.UserControl, IApplicationStep
{
	 private const string POLICYCHECKER = "PolicyCheckerCache";
	 protected string ZIP_CODE = string.Empty;

	 //protected string PUBLIC_KEY = "pk_91cbc68abaf9448991896094a29cfd77";
	 //protected string SECRET_KEY = "sk_a91f07d3e75f453cb15fd2f61698f33b";
	 //protected string MERCHANT_ID = "m9fsvh2s7mmmf6kk2ikl";
	 //protected string IS_SANDBOX = "true";
	 //protected bool IS_SANDBOX_BOOL = true;
	 //protected string passMX = string.Empty;

	 private class PolicyInternalThreadHolder
	 {
		  public int QuoteId { get; set; }
		  public bool IsTest { get; set; }
		  public DateTime StartDate { get; set; }
		  public DateTime EndDate { get; set; }
		  public decimal NetPremium { get; set; }
		  public clAuthorizeCC CreditCardInformation { get; set; }
		  public int UserID { get; set; }
		  public string Address { get; set; }
		  public string Name { get; set; }
		  public string Phone { get; set; }
		  public double Amount { get; set; }
		  public decimal TaxPercentage { get; set; }
		  public int ClientID { get; set; }
		  public int Currency { get; set; }
		  public string CompleteAddress { get; set; }
		  public decimal Comision { get; set; }
		  public string CountryData { get; set; }
		  public decimal ExchangeRate { get; set; }
		  public decimal RXPFPercentage { get; set; }
		  public decimal RXPF { get; set; }
		  public string HostName { get; set; }
		  public int TotalPayments { get; set; }
		  public decimal Fee { get; set; }
	 }

	 public void LoadStep() { }

	 private void StartPolicyChecker(bool skip)
	 {
		  Session[POLICYCHECKER] = new PolicyCacheData
		  {
				Status = PolicyCacheStatus.Incomplete,
				Data = null,
				SkipPayment = skip
		  };
	 }

	 private void StopPolicyChecker(IHousePolicy policy)
	 {
		  PolicyCacheData data = Session[POLICYCHECKER] as PolicyCacheData;
		  if (data != null)
		  {
				data.Status = PolicyCacheStatus.Complete;
				data.Data = policy;
		  }
	 }

	 private bool m_skipPayment = false;

	 public bool SaveStep()
	 {
		  //Si llega a este paso los datos deben de ser validos (o casi)...
		  //pagar con CC
		  HouseInsuranceDataContext db = new HouseInsuranceDataContext();
		  int quoteid = 0,
				agent = 0,
				userid = 0;

		  UserInformation user = (UserInformation)HttpContext.Current.User.Identity;
		  bool mustPay = AdminGateway.Gateway.MustPayWithCC(user.UserID);

		  try
		  {
				quoteid = (int)Session["quoteidBuy"];
				if (quoteid <= 0)
			{
				return false;
			}

			agent = user.Agent;
				if (agent <= 0)
			{
				return false;
			}

			userid = user.UserID;
				if (userid <= 0)
			{
				return false;
			}
		}
		  catch
		  {
				return false;
		  }

		  var cotiza = db.spList_Cotiza(quoteid).Single();
		  var dquote = db.quotes.Where(m => m.id == quoteid).Single();
		  var fee = dquote.fee.Value;
		  var clientinfo = db.spList_V1_quotePerson(quoteid).Single();
		  string value;

		  /* for now dont let payment occur if pesos is the currency */
		  if (dquote.currency == ApplicationConstants.PESOS_CURRENCY && mustPay)
		  {
				return false;
		  }

		  if (txtCCNumber.Text.IndexOf("xxxx") != -1)
		  {
				value = clientinfo.DecryptCC();
				if (!DoCreditCardValidation(value))
				{
					 valCCNumber.IsValid = false;
					 return false;
				}
		  }
		  else
		  {
				value = Regex.Replace(txtCCNumber.Text, "[^0-9]", string.Empty);
				//save credit card info
				if (rbYes.Checked)
				{
					 clientinfo.EncryptCC(txtCCNumber.Text);
					 db.SubmitChanges();
				}
		  }
		  db.Connection.Close();
		  string paddress,
				pcity,
				pzip,
				pstate,
				countryData;
		  HouseEstate houseEstate = dquote.insuredPersonHouse1.HouseEstate;
		  if (string.IsNullOrEmpty(clientinfo.street))
		  {
				paddress =
					 cotiza.exteriorNo
					 + (
						  string.IsNullOrEmpty(cotiza.InteriorNo)
								? string.Empty
								: " - " + cotiza.InteriorNo
					 )
					 + " "
					 + cotiza.street;

				pcity = houseEstate.City.Name;
				pzip = houseEstate.ZipCode;
				pstate = houseEstate.City.State.Name;
				countryData = "MX";
		  }
		  else
		  {
				paddress = clientinfo.street;
				pcity = clientinfo.city;
				pzip = clientinfo.zipCode;
				pstate =
					 clientinfo.state == null
						  ? clientinfo.stateText
						  : new State(clientinfo.state.Value).Name;
				countryData = "USA";
		  }
		  bool isTest = AdminGateway.Gateway.IsTestUser();
		  clAuthorizeCC cc = null;
		  decimal netPremium = (decimal)cotiza.NetPremium;
		  decimal taxP = db.fGet_QuoteTaxPercentage(quoteid) ?? 0;
		  double amount = (double)
				Math.Round(
					 ApplicationConstants.CalculateTotal(
						  netPremium,
						  taxP,
						  dquote.porcentajeRecargoPagoFraccionado,
						  cotiza.currency,
						  fee
					 ),
					 2
				);
		  if (!m_skipPayment)
		  {
				//if (rbAmericanCC.Checked)
				//{
				cc = new clAuthorizeCC();
				cc.isTest = isTest ? "True" : "False";
				cc.CCcode = txtCardCode.Text.Trim();
				cc.amount = amount;
				cc.CCno = value;
				cc.address = paddress;
				cc.city = pcity;
				cc.country = "USA";
				cc.firstName = cotiza.firstname;
				cc.lastName = cotiza.lastName + " " + cotiza.lastName2;
				cc.zip = pzip;
				cc.state = pstate;
				//no de cotizacion
				cc.orderDescription = quoteid.ToString();
				cc.invoiceNo = quoteid.ToString();
				cc.CCExpDate = ddlExpMonth.SelectedValue + ddlExpYear.SelectedValue;
				if (!cc.AuthorizePayment())
				{
					 valCCNumber.IsValid = false;
					 return false;
				}
				//}
				//else if (rbMexicanCC.Checked)
				//{
				//    string script = string.Format("callCreateToken();");
				//    ScriptManager.RegisterStartupScript(this, this.GetType(), "Script", script, true);

				//    OpenpayAPI openPayAPI = new OpenpayAPI(SECRET_KEY, MERCHANT_ID, !IS_SANDBOX_BOOL);
				//    string token_id = txtTokenId.Text; //"kwmnijantutlicmm2ln3";
				//    string device_id = txtDeviceId.Text; //"46167f4fea6145eeabe3fac76c3a34da";

				//    ChargeRequest request = new ChargeRequest();
				//    request.SourceId = token_id;
				//    request.DeviceSessionId = device_id;
				//    request.Method = "card";
				//    request.Description = quoteid.ToString();

				//    request.Amount = (decimal)amount;
				//    request.Currency = "USD";


				//    Customer customer = new Customer();
				//    customer.Name = cotiza.firstname;
				//    customer.LastName = cotiza.lastName;

				//    customer.Email = clientinfo.email;
				//    customer.Address = new Address();
				//    customer.Address.Line1 = paddress;
				//    customer.Address.City = pcity;
				//    customer.Address.PostalCode = pzip;
				//    customer.Address.CountryCode = countryData;
				//    customer.Address.State = pstate;

				//    //Card card = new Card();
				//    //card.CardNumber = "4111111111111111";
				//    //card.HolderName = "John Doe";
				//    //card.Cvv2 = "123";
				//    //card.ExpirationMonth = "01";
				//    //card.ExpirationYear = "22";
				//    //card.BankCode = "02";

				//    //PayoutRequest request = new PayoutRequest();
				//    //request.Method = "card";
				//    //request.Card = card;
				//    //request.Amount = 500.00m;
				//    //request.Description = "Payout test";


				//    try
				//    {
				//        request.Customer = openPayAPI.CustomerService.Create(customer);
				//        Charge charge = openPayAPI.ChargeService.Create(request);
				//        //Payout payout = openPayAPI.PayoutService.Create(customer.Id, request);
				//    }
				//    catch (Exception ex)
				//    {
				//        valCCNumber.IsValid = false;
				//        return false;
				//        throw;
				//    }

				//}
		  }

		  string caddress =
				cotiza.exteriorNo
				+ (string.IsNullOrEmpty(cotiza.InteriorNo) ? string.Empty : " - " + cotiza.InteriorNo)
				+ " "
				+ cotiza.street
				+ " "
				+ houseEstate.City.Name
				+ ", "
				+ houseEstate.City.State.Name;

		  Thread policyNoThread = new Thread(PolicyNumberBuilder);
		  PolicyInternalThreadHolder data = new PolicyInternalThreadHolder
		  {
				QuoteId = quoteid,
				IsTest = isTest,
				StartDate = (DateTime)cotiza.startDate,
				EndDate = (DateTime)cotiza.endDate,
				NetPremium = (decimal)cotiza.NetPremium,
				CreditCardInformation = cc,
				UserID = cotiza.issuedBy,
				Address = caddress ?? string.Empty,
				Name = cotiza.firstname + " " + cotiza.lastName + " " + cotiza.lastName2,
				Phone = clientinfo.phone ?? string.Empty, //REVISAR
				Amount = amount,
				ClientID = clientinfo.id,
				CompleteAddress = caddress,
				TaxPercentage = taxP,
				Currency = cotiza.currency,
				Comision = cotiza.comision ?? 0,
				CountryData = countryData,
				ExchangeRate = user.ExchangeRate,
				RXPF = dquote.recargoPagoFraccionado,
				RXPFPercentage = dquote.porcentajeRecargoPagoFraccionado,
				TotalPayments = dquote.cantidadDePagos,
				HostName = Request.Url.Host,
				Fee = fee
		  };

		  StartPolicyChecker(m_skipPayment);

		  policyNoThread.Start(data);

		  return true;
	 }

	 public void PolicyNumberBuilder(object threadData)
	 {
		  PolicyInternalThreadHolder data = threadData as PolicyInternalThreadHolder;
		  //generar poliza en base de datos de la aplicacion
		  IHousePolicy policyData = new MapfrePolicy(data.HostName);
	 decimal comPercentage = ApplicationConstants.COMISSION_PERCENTAGE;//Math.Round(data.Comision, 2) / Math.Round(data.NetPremium);

				policyData.ConvertToPolicy(
				data.QuoteId,
				data.IsTest,
				data.NetPremium,
				data.TaxPercentage,
				comPercentage,
				data.ExchangeRate
		  );

		HouseInsuranceDataContext db = new HouseInsuranceDataContext();

		var pol_home = db.policies.Where(p => p.policyNo.ToLower().Trim() == policyData.PolicyNo.ToLower().Trim()).FirstOrDefault();
		  //generar poliza en base de datos administrativa
		  csPolicy dbPolicy = new csPolicy(ApplicationConstants.ADMINDB);
		  dbPolicy.aplicacion = ApplicationConstants.APPLICATION;
		  dbPolicy.policyNo = policyData.PolicyNo;
		  dbPolicy.inception = pol_home.startDate.ToString("MM/dd/yyyy HH:mm:ss");
		  dbPolicy.expiration = pol_home.endDate.ToString("MM/dd/yyyy HH:mm:ss");
		  dbPolicy.company = ApplicationConstants.COMPANY; //OJO HARD CODE TEMPORAL
		  dbPolicy.insured = pol_home.insuredPerson;//data.ClientID;
		  dbPolicy.insuredAddress = data.Address;
		  dbPolicy.insuredName = data.Name;
		  dbPolicy.insuredPhone = data.Phone;
		  dbPolicy.insurer = pol_home.issuedBy;//data.UserID;
		  dbPolicy.issueDate = pol_home.issueDate.ToString("MM/dd/yyyy HH:mm:ss");
		  dbPolicy.policy = policyData.PolicyID;
		  dbPolicy.netPremium = (double)Math.Round(pol_home.netPremium, 2);
		  dbPolicy.mexicanTax = (double)Math.Round(ApplicationConstants.CalculateTax(pol_home.netPremium, data.TaxPercentage, data.RXPFPercentage, data.Currency, data.Fee), 2);
		  if (data.TotalPayments > 1)
		  {
				dbPolicy.RXPF = (double)data.RXPF;
				dbPolicy.RXPFPercentage = (double)data.RXPFPercentage;
				dbPolicy.totalPayments = data.TotalPayments;
		  }
		  dbPolicy.taxPercentage = (double)Math.Round(data.TaxPercentage, 2);
		  dbPolicy.policyFee = (double)
				Math.Round(ApplicationConstants.Fee(data.Currency, data.Fee), 2);
		  dbPolicy.totalComission = 0;
		  dbPolicy.totalPremium = data.Amount;
		  dbPolicy.mgaCom = (double)Math.Round(comPercentage, 2);
		  dbPolicy.insuredCountry = data.CountryData;
		  dbPolicy.currency = data.Currency;
		  dbPolicy.usComission = dbPolicy.totalComission;
		  dbPolicy.policyDescription = dbPolicy.insuredName + " " + data.CompleteAddress;
		  dbPolicy.specialCoverageFee = 0.0;
		  dbPolicy.Save();

		  if (!m_skipPayment)
		  {
				csPayment dbPayment = new csPayment(ApplicationConstants.ADMINDB); //ojo hard code con strConexion.xml
				dbPayment.aplicacion = ApplicationConstants.APPLICATION;
				dbPayment.payDate = DateTime.Now.ToString("MM/dd/yyyy hh-mm-ss");
				dbPayment.policy = dbPolicy.policyNo;
				clAuthorizeCC cc = data.CreditCardInformation;
				dbPayment.netCode = cc.netCode;
				dbPayment.netTransid = cc.netTransid;
				dbPayment.CCExpMonth = int.Parse(cc.CCExpDate.Substring(0, 2));
				dbPayment.CCExpYear = int.Parse(cc.CCExpDate.Substring(2, 4));
				dbPayment.CCNumber = cc.CCno;
				dbPayment.CCType = ddlType.SelectedValue;
				dbPayment.paymentType = "CC";
				dbPayment.payAmount = data.Amount;
				dbPayment.Pay("PAID", "P", "''", 1);
		  }

		  string message = string.Empty;
		  message += "Policy No.: " + dbPolicy.policyNo + "<br />";
		  message += "Start Date: " + dbPolicy.inception + "<br />";
		  message += "End Date: " + dbPolicy.expiration + "<br />";
		  message += "Insured: " + dbPolicy.insuredName + "<br />";
		  message += dbPolicy.insuredAddress + "<br />" + dbPolicy.insuredPhone + "<br />";
		  message += "Issue Date: " + dbPolicy.issueDate + "<br />";
		  message += "Net Premium: " + data.NetPremium.ToString("C") + "<br />";
		  if (data.TotalPayments > 1)
		  {
				message += "RXPDF: " + data.RXPF.ToString("C") + "<br />";
		  }
		  message += "Tax: " + dbPolicy.mexicanTax.ToString("C") + "<br />";
		  message += "Policy Fee: " + dbPolicy.policyFee.ToString("C") + "<br />";
		  message += "Policy Description: " + dbPolicy.policyDescription + "<br />";
		  message +=
				"<br /><br />If you have any questions or concerns please contact us by calling (800) 334-7950."
				+ "<br /><br />Thank you for your business."
				+ "<br />MacAfee & Edwards, Inc"
				+ "<br />Mexican Insurance Specialist";

		  /*
		  MailMessage mail = new MailMessage(ConfigurationSettings.AppSettings["MailFrom"], "manuel@creatsol.com", "Poliza: " + dbPolicy.policyNo, message);
		  if (data.IsTest) {
				mail.Attachments.Add(GenerateAttachment(policyData.XMLString));
		  } else {
				mail.CC.Add("info@mexicard.com");
				mail.Bcc.Add(ConfigurationSettings.AppSettings["MailBCC"]);
		  }
		  mail.IsBodyHtml = true;
		  SmtpClient mailClient = new SmtpClient(ConfigurationSettings.AppSettings["MailServer"]);
		  mailClient.Credentials = CredentialCache.DefaultNetworkCredentials;
		  try {
			  // mailClient.Send(mail);
		  } catch {
		  } */

		  //if (!policyData.PolicyNo.Contains("HO") || !data.IsTest)
		  //    ImportadorASicas.HerramientasCreatsol.RegistraPolizaSICAS(policyData.PolicyNo, ApplicationConstants.view_SICAS);

		  try
		  {
				Session["policyid"] = policyData.PolicyID;
				Session["policyno"] = policyData.PolicyNo;
		  }
		  catch { }

		  StopPolicyChecker(policyData);
	 }

	 private Attachment GenerateAttachment(string text)
	 {
		  string tempFileName = Path.GetTempFileName();
		  FileStream stream = new FileStream(tempFileName, FileMode.Create, FileAccess.Write);
		  StreamWriter writer = new StreamWriter(stream);
		  writer.BaseStream.Seek(0, SeekOrigin.Begin);
		  writer.Write(text);
		  writer.Flush();
		  writer.Close();
		  return new Attachment(tempFileName);
	 }

	 public string Title()
	 {
		  return string.Empty;
	 }

	 private static string AdminCS()
	 {
		  return ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString;
	 }

	 private bool VerifyLuhn(string number)
	 {
		  int sum = 0,
				parity = number.Length % 2;
		  for (int i = 0; i < number.Length; i++)
		  {
				int digit;
				try
				{
					 digit = int.Parse(number.Substring(i, 1));
				}
				catch
				{
					 continue;
				}
				if (i % 2 == parity)
				{
					 digit *= 2;
					 if (digit > 9)
				{
					digit -= 9;
				}
			}
				sum += digit;
		  }
		  return sum % 10 == 0;
	 }

	 private bool DoCreditCardValidation(string value)
	 {
		  int length = value.Length;
		  //verify by type
		  if (length < 13)
		{
			return false;
		}

		if (string.IsNullOrEmpty(txtCardCode.Text))
		{
			return false;
		}

		switch (ddlType.SelectedValue)
		  {
				case "VISA":
					 if ((length != 16 && length != 13) || value[4] != '4')
				{
					return false;
				}

				break;
				case "MC":
					 try
					 {
						  if (
								length != 16
								|| value[0] != '5'
								|| value[1] == '0'
								|| int.Parse(value[1].ToString()) > 5
						  )
					{
						return false;
					}
				}
					 catch
					 {
						  return false;
					 }
					 break;
				case "DISCOVER":
					 if (
						  length != 16
						  || value[0] != '6'
						  || value[1] != '0'
						  || value[2] != '1'
						  || value[3] != '1'
					 )
				{
					return false;
				}

				break;
				case "AMEX":
					 if (length != 15 || value[0] != '3' || (value[1] != '4' && value[1] != '7'))
				{
					return false;
				}

				break;
				default:
					 return false;
		  }
		  //Algoritmo de Luhn
		  return VerifyLuhn(value);
	 }

	 protected void CreditCardValidate(object sender, ServerValidateEventArgs args)
	 {
		  if (args.Value.IndexOf("xxxx") != -1)
		  {
				args.IsValid = !string.IsNullOrEmpty(txtCardCode.Text);
		  }
		  else
		  {
				//trim cc
				string value = Regex.Replace(args.Value, "[^0-9]", string.Empty);
				args.IsValid = DoCreditCardValidation(value);
		  }
	 }

	 private bool CheckSkipCookie()
	 {
		  return Session["DoNotSkipPayment"] == null || !(bool)Session["DoNotSkipPayment"];
	 }

	 protected void Page_Load(object sender, EventArgs e)
	 {
		  int userid = ((UserInformation)HttpContext.Current.User.Identity).UserID;
		  var mustPay = AdminGateway.Gateway.MustPayWithCC(userid);
		  if (!mustPay && CheckSkipCookie())
		  {
				int step = (int)Session["PolicyStep"];
				m_skipPayment = true;
				string redirect = string.Empty;
				if (SaveStep())
				{
					 redirect = "~/Application/PolicyChecker.aspx";
					 Session["PolicyStep"] = step + 1;
				}
				else
				{
					 redirect = Request.Url.AbsolutePath;
					 Session["PolicyStep"] = step - 1;
				}
				Response.Redirect(redirect, true);
		  }
		  else
		{
			m_skipPayment = false;
		}

		ClientScriptManager scriptManager = Page.ClientScript;
		  scriptManager.RegisterOnSubmitStatement(GetType(), "customSubmit", "CheckValidatedPage();");
		  scriptManager.RegisterStartupScript(GetType(), "disableBuyBtn", "DisableBuyBtn();", true);
		  Button btnCancel = (Button)Parent.FindControl("btnCancel");
		  btnCancel.OnClientClick = "DisableValidators();";
		  int quoteid = 0;
		  try
		  {
				quoteid = (int)Session["quoteidBuy"];
				if (quoteid <= 0)
			{
				return;
			}
		}
		  catch
		  {
				return;
		  }
		  int year = DateTime.Today.Year;
		  int tmp;
		  for (int i = 0; i < 8; i++)
		  {
				tmp = year + i;
				ddlExpYear.Items.Insert(i, new ListItem(tmp.ToString(), tmp.ToString()));
		  }
		  SqlConnection dbadmin = new SqlConnection(AdminCS());
		  dbadmin.Open();
		  SqlCommand command = new SqlCommand(
				"SELECT * FROM creditCard ORDER BY description",
				dbadmin
		  );
		  ddlType.DataSource = command.ExecuteReader(CommandBehavior.CloseConnection);
		  ddlType.DataBind();
		  dbadmin.Close();
		  ddlExpMonth.Items.Insert(0, new ListItem("01", "01"));
		  ddlExpMonth.Items.Insert(1, new ListItem("02", "02"));
		  ddlExpMonth.Items.Insert(2, new ListItem("03", "03"));
		  ddlExpMonth.Items.Insert(3, new ListItem("04", "04"));
		  ddlExpMonth.Items.Insert(4, new ListItem("05", "05"));
		  ddlExpMonth.Items.Insert(5, new ListItem("06", "06"));
		  ddlExpMonth.Items.Insert(6, new ListItem("07", "07"));
		  ddlExpMonth.Items.Insert(7, new ListItem("08", "08"));
		  ddlExpMonth.Items.Insert(8, new ListItem("09", "09"));
		  ddlExpMonth.Items.Insert(9, new ListItem("10", "10"));
		  ddlExpMonth.Items.Insert(10, new ListItem("11", "11"));
		  ddlExpMonth.Items.Insert(11, new ListItem("12", "12"));
		  HouseInsuranceDataContext db = new HouseInsuranceDataContext();
		  var info = db.spList_Cotiza(quoteid).Single();
		  var quote = db.quotes.Where(m => m.id == quoteid).Single();
		  decimal rxpfP = quote.porcentajeRecargoPagoFraccionado;
		  var taxP = db.fGet_QuoteTaxPercentage(quoteid).Value;
		  lblClient.Text = info.legalPerson
				? info.firstname
				: info.lastName + " " + info.lastName2 + ", " + info.firstname;
		  string anumber =
				info.exteriorNo
				+ (string.IsNullOrEmpty(info.InteriorNo) ? string.Empty : " - " + info.exteriorNo);
		  HouseEstate houseEstate = quote.insuredPersonHouse1.HouseEstate;
		  lblAddress.Text =
				anumber
				+ " "
				+ info.street
				+ ". "
				+ houseEstate.City.Name
				+ ", "
				+ houseEstate.City.State.Name;
		  lblValue.Text = info.buildingLimit.ToString("C");
		  lblStartDate.Text =
				info.startDate == null ? string.Empty : ((DateTime)info.startDate).ToString();
		  lblEndDate.Text = info.endDate == null ? string.Empty : ((DateTime)info.endDate).ToString();
		  lblCSL.Text = info.CSLLimit == 0 ? "EXCLUDED" : info.CSLLimit.ToString("C");
		  lblTotal.Text = ApplicationConstants
				.CalculateTotal((decimal)info.NetPremium, taxP, rxpfP, info.currency, quote.fee.Value)
				.ToString("C");
	 }

	 //protected Charge createCharge()
	 //{
	 //    string PUBLIC_KEY = "pk_91cbc68abaf9448991896094a29cfd77";
	 //    string SECRET_KEY = "sk_a91f07d3e75f453cb15fd2f61698f33b";
	 //    string MERCHANT_ID = "m9fsvh2s7mmmf6kk2ikl";
	 //    bool IS_SANDBOX_BOOL = true;

	 //    OpenpayAPI openPayAPI = new OpenpayAPI(SECRET_KEY, MERCHANT_ID, !IS_SANDBOX_BOOL);
	 //    string token_id = txtTokenId.Text;
	 //    string device_id = txtDeviceId.Text;

	 //    ChargeRequest request = new ChargeRequest();
	 //    request.SourceId = token_id;
	 //    request.DeviceSessionId = device_id;
	 //    request.Method = "card";
	 //    request.Description = "HM" + "Numero de Poliza"; // Preguntarle a Manuel; número de la cotización?
	 //    Session["numberOfPayments"] = 1;
	 //    request.Amount = decimal.Parse(_wsO.total.ToString(), NumberStyles.Currency);
	 //    request.Currency = "USD";

	 //    //Revisar la informacion del customer
	 //    Customer customer = new Customer();
	 //    customer.Name = txtNameCC.Text;
	 //    customer.LastName = txtNameCC.Text;
	 //    //customer.PhoneNumber = txtPhone.Text;
	 //    customer.Email = txtEmail.Text;
	 //    customer.Address = new Address();
	 //    customer.Address.Line1 = txtStreet.Text.Trim();
	 //    customer.Address.City = ddlCity.SelectedItem.Text;
	 //    customer.Address.PostalCode = txtZipCode.Text;
	 //    customer.Address.Line1 = txtStreet.Text + " " + txtNoExt.Text + " " + txtNoInt.Text;
	 //    customer.Address.CountryCode = "MX";
	 //    customer.Address.State = ddlState.SelectedItem.Text;
	 //    request.Customer = openPayAPI.CustomerService.Create(customer);
	 //    Charge charge = openPayAPI.ChargeService.Create(request);

	 //    //Session["Transaction"] = charge.Authorization;

	 //    return charge;
	 //}
}
