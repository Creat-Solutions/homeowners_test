using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Text;

public class PolicyInvoiceData
{
    public int ApprovalYear { get; set; }
    public string ApprovalNumber { get; set; }
    public string Certificate { get; set; }
    public string CertificateNo { get; set; }
    public string Date { get; set; }
    public string PaymentType { get; set; }
    public string Series { get; set; }
    public string MapfreInternalNumber { get; set; }
    public string OriginalString { get; set; }
    public string Seal { get; set; }
    public string Folio { get; set; }
    public string ReceiptType { get; set; }
    public string FxReceiptType { get; set; }
    public string PaymentMedium { get; set; }
    public string IssuedIn { get; set; }
    public string Registry { get; set; }
    public string BankReferencesXML { get; set; }
    public string MapfreEndoso { get; set; }
    public string ReceiptSeries { get; set; }
    public string MapfreMEAgentName { get; set; }
    public string MapfreMEKey { get; set; }
    public string RawXML { get; set; }
    public int PolicyID { get; set; }
}


public abstract class AbstractElectronicInvoiceParser
{
    private string m_codedData;
    public string RawXMLResponse { get; private set; }
    private bool m_test;

	protected void Init(string fullDoc, string codedResult, bool test)
	{
        RawXMLResponse = fullDoc;
        m_codedData = codedResult;
        m_test = test;
	}

    protected void Init(string fullDoc, string codedResult)
    {
        Init(fullDoc, codedResult, false);
    }

    public void DecodeAndGenerate(int policyID)
    {
        var xmlDoc = DecodeAndParse(m_codedData);
        XNamespace ns = "http://www.fact.com.mx/schema/fx";
        XNamespace addendaNS = "http://www.sat.gob.mx/cfd/3";
        var facturaAddenda = xmlDoc.Descendants(addendaNS + "Addenda").Descendants(ns + "FactDocMX").Single();
        var agentData = facturaAddenda.Descendants(ns + "Mapfre").Descendants(ns + "Agente").Single();
        string internalNo = facturaAddenda.Descendants(ns + "NumeroInterno").Single().Value;
        string originalString = facturaAddenda.Descendants(ns + "CadenaOriginal").Single().Value;
        string fxReceiptType = facturaAddenda.Descendants(ns + "TipoDeComprobante").Single().Value;
        string paymentMedium = facturaAddenda.Descendants(ns + "MedioDePago").Single().Value;
        string registry = facturaAddenda.Descendants(ns + "RegistroComision").Single().Value;
        string issuedIn = facturaAddenda.Descendants(ns + "ExpedidoEn").Single().Value;
        string mapfreEndoso = facturaAddenda.Descendants(ns + "Endoso").Single().Value;

        var asignacion = facturaAddenda.Descendants(ns + "Asignacion").Single();
        int approvalYear = int.Parse(asignacion.Descendants(ns + "AnoDeAprobacion").Single().Value);
        string approvalNo = asignacion.Descendants(ns + "NumeroDeAprobacion").Single().Value;
        string series = asignacion.Descendants(ns + "Serie").First().Value;
        string folio = asignacion.Descendants(ns + "Folio").Single().Value;
        string receiptSeries = string.Empty;
        try {
            receiptSeries = facturaAddenda.Descendants(ns + "SerieRecibo").Single().Value;
        } catch {
            // fepadre
            receiptSeries = string.Empty;
        }
        var bancos = facturaAddenda.Descendants(ns + "ReferenciasBancarias").Single();
        string bankReferences = string.Empty;
        foreach (var banco in bancos.Elements()) {

            bankReferences += "<ReferenciaBancaria><Banco>" + 
                              banco.Element(ns + "Banco").Value +
                              "</Banco>";

            var convenio = banco.Element(ns + "Convenio");
            if (convenio != null) {
                bankReferences += "<Convenio>" + banco.Element(ns + "Convenio").Value + "</Convenio>";
            }

            var refCliente = banco.Element(ns + "RefCliente");
            if (refCliente != null) {
                bankReferences += "<RefCliente>" + banco.Element(ns + "RefCliente").Value + "</RefCliente>";
            }

            bankReferences += "</ReferenciaBancaria>";
                
        }
        
        var datosFE = new PolicyInvoiceData {
            ApprovalYear = approvalYear,
            ApprovalNumber = approvalNo,
            Certificate = (string)xmlDoc.Attribute("certificado"),
            CertificateNo = (string)xmlDoc.Attribute("noCertificado"),
            Date = (string)xmlDoc.Attribute("fecha"),
            PaymentType = (string)xmlDoc.Attribute("formaDePago"),
            Series = series,
            MapfreInternalNumber = internalNo,
            OriginalString = originalString,
            Seal = (string)xmlDoc.Attribute("sello"),
            Folio = folio,
            ReceiptType = (string)xmlDoc.Attribute("tipoDeComprobante"),
            FxReceiptType = fxReceiptType,
            PaymentMedium = paymentMedium,
            IssuedIn = issuedIn,
            Registry = registry,
            // Replace("fx:", "") podria ser mejor con XSLT
            BankReferencesXML = "<ReferenciasBancarias>" +
                bankReferences +
                "</ReferenciasBancarias>",
            MapfreEndoso = mapfreEndoso,
            ReceiptSeries = receiptSeries,
            MapfreMEAgentName = agentData.Element(ns + "Nombre").Value,
            MapfreMEKey = agentData.Element(ns + "Clave").Value,
            RawXML = xmlDoc.ToString(),
            PolicyID = policyID
        };

        SaveToDatabase(datosFE);
        
    }

    
    protected abstract void SaveToDatabase(PolicyInvoiceData data);

    private XElement DecodeAndParse(string codedResult)
    {
        string decoded = Decode(codedResult);
        return XElement.Parse(decoded);
    }

    private string Decode(string codedResult)
    {
        byte[] decodedData = Convert.FromBase64String(codedResult);
        return Encoding.UTF8.GetString(decodedData);
    }
}


public class MapfreElectronicInvoiceParser : AbstractElectronicInvoiceParser
{
    public MapfreElectronicInvoiceParser(string fullDoc, string codedResult, bool test)
    {
        Init(fullDoc, codedResult, test);
    }

    public MapfreElectronicInvoiceParser(string fullDoc, string codedResult)
        : this(fullDoc, codedResult, false)
    {
    }

    protected override void SaveToDatabase(PolicyInvoiceData data)
    {

        var datosFE = new policyFE {
            approvalYear = data.ApprovalYear,
            approvalNo = data.ApprovalNumber,
            certificate = data.Certificate,
            certificateNo = data.CertificateNo,
            date = data.Date,
            paymentType = data.PaymentType,
            series = data.Series,
            mapfreInternalNo = data.MapfreInternalNumber,
            originalString = data.OriginalString,
            seal = data.Seal,
            folio = data.Folio,
            receiptType = data.ReceiptType,
            fxReceiptType = data.FxReceiptType,
            paymentMedium = data.PaymentMedium,
            issuedIn = data.IssuedIn,
            registry = data.Registry,
            bankReferencesXML = data.BankReferencesXML,
            mapfreEndoso = data.MapfreEndoso,
            receiptSeries = data.ReceiptSeries,
            mapfreMEAgentName = data.MapfreMEAgentName,
            mapfreMEKey = data.MapfreMEKey,
            rawXML = data.RawXML,
            policy = data.PolicyID
        };

        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.policyFEs.InsertOnSubmit(datosFE);
        db.SubmitChanges();
    }
}

public class MapfreFEPADREInvoiceParser: AbstractElectronicInvoiceParser
{

    public MapfreFEPADREInvoiceParser(string fullDoc, string codedResult, bool test)
    {
        Init(fullDoc, codedResult, test);
    }

    public MapfreFEPADREInvoiceParser(string fullDoc, string codedResult)
        : this(fullDoc, codedResult, false)
    {

    }

    protected override void SaveToDatabase(PolicyInvoiceData data)
    {

        var datosFE = new policyFEPADRE {
            approvalYear = data.ApprovalYear,
            approvalNo = data.ApprovalNumber,
            certificate = data.Certificate,
            certificateNo = data.CertificateNo,
            date = data.Date,
            paymentType = data.PaymentType,
            series = data.Series,
            mapfreInternalNo = data.MapfreInternalNumber,
            originalString = data.OriginalString,
            seal = data.Seal,
            folio = data.Folio,
            receiptType = data.ReceiptType,
            fxReceiptType = data.FxReceiptType,
            paymentMedium = data.PaymentMedium,
            issuedIn = data.IssuedIn,
            registry = data.Registry,
            bankReferencesXML = data.BankReferencesXML,
            mapfreEndoso = data.MapfreEndoso,
            receiptSeries = data.ReceiptSeries,
            mapfreMEAgentName = data.MapfreMEAgentName,
            mapfreMEKey = data.MapfreMEKey,
            rawXML = data.RawXML,
            policy = data.PolicyID
        };

        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.policyFEPADREs.InsertOnSubmit(datosFE);
        db.SubmitChanges();
    }
}