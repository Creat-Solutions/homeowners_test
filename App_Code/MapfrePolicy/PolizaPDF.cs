using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Globalization;
using System.Configuration;
using System.Web.Configuration;
using ME.Admon;

/// <summary>
/// Represents and creates a PDF file for a policy
/// </summary>
public class PolizaPDF
{
    private int m_policyId = 0;
    private string m_policyNo = null;
    private MemoryStream m_stream = null;

    public PolizaPDF(string policyno, bool english)
    {
        m_policyNo = policyno;
        InitInternalData(english, false);
    }

    public PolizaPDF(int policyid, bool english, bool fromQuotes)
    {
        m_policyId = policyid;
        TemplateData data = InitInternalData(english, fromQuotes);
        m_policyNo = data.policyData.policyNo;
    }

    public PolizaPDF(int policyid) :
        this(policyid, false, false)
    {
        
    }

    public PolizaPDF(string policyno) :
        this(policyno, false)
    {
    }

    private TemplateData InitInternalData(bool english, bool fromQuotes)
    {
        TemplateLanguage language = english ? TemplateLanguage.English : TemplateLanguage.Spanish;
        Document pdfDocument = new Document(PageSize.LETTER);
        MemoryStream memStream = new MemoryStream();
        PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDocument, memStream);
        pdfDocument.AddAuthor("Macafee and Edwards");
        pdfDocument.Open();
        TemplateData data = GetData(fromQuotes);
        pdfDocument.AddTitle("Homeowner's policy number: " + data.policyData.policyNo);
        PolizaPDFTemplate firstTemplate = new PolizaPDFTemplate(pdfWriter, TemplateVersion.Long, language, data, fromQuotes);
        if (!english) {
            PolizaPDFTemplate secondTemplate = new PolizaPDFTemplate(pdfWriter, TemplateVersion.Short, language, data, fromQuotes);
        }
        pdfDocument.Close();
        m_stream = memStream;

        return data;
    }

    public void Merge(PolizaPDF otherDoc)
    {
        MemoryStream stream = new MemoryStream(m_stream.ToArray());
        MemoryStream otherStream = new MemoryStream(otherDoc.m_stream.ToArray());
        m_stream = null;
        PdfReader reader = new PdfReader(stream);
        PdfReader otherReader = new PdfReader(otherStream);
        MemoryStream outputStream = new MemoryStream();
        PdfCopyFields copy = new PdfCopyFields(outputStream);
        copy.AddDocument(reader);
        copy.AddDocument(otherReader);
        copy.Close();
        m_stream = outputStream;
    }    

    private TemplateData GetData(bool fromQuotes)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        policy data;
        if (fromQuotes) {
            quote info = db.quotes.Single(m => m.id == m_policyId);
            var datos = db.spList_Cotiza(m_policyId).Single();
            decimal taxP = db.fGet_QuoteTaxPercentage(m_policyId) ?? 0;
            decimal netPremiumQuote = datos.NetPremium ?? 0;
            decimal comision = datos.comision ?? 0;

            UserInformationDB userInfo = AdminGateway.Gateway.GetUserInformation(info.issuedBy);
            
            //esta clase no esta asociada a ningun DataContext

            #region GENERATE_POLICY_NO_CONTEXT
            data = new policy {
                id = m_policyId,
                netPremium = netPremiumQuote,
                policyNo = "COTIZACIÓN",
                policyNoLocal = true,
                accidentalGlassBreakageLimit = info.accidentalGlassBreakageLimit,
                adjoiningEstates = info.adjoiningEstates == null ? false : (bool)info.adjoiningEstates,
                buildingLimit = info.buildingLimit,
                burglaryJewelryLimit = info.burglaryJewelryLimit,
                burglaryLimit = info.burglaryLimit,
                centralAlarm = info.centralAlarm == null ? false : (bool)info.centralAlarm,
                comissionP = comision / netPremiumQuote,
                comissionTotal = comision,
                company = info.company == null ? 0 : (int)info.company,
                contentsLimit = info.contentsLimit,
                CSLLimit = info.CSLLimit,
                days = 0,
                debriLimit = info.debriLimit,
                electronicEquipmentLimit = info.electronicEquipmentLimit,
                extraLimit = info.extraLimit,
                familyAssistance = info.familyAssistance,
                fee = ApplicationConstants.Fee(info.currency, info.fee.Value),
                fireAll = info.fireAll == null ? false : (bool)info.fireAll,
                guards24 = info.guards24 == null ? false : (bool)info.guards24,
                HMP = info.HMP == null ? false : (bool)info.HMP,
                issuedBy = info.issuedBy,
                issueDate = info.issueDate,
                isTest = true,
                localAlarm = info.localAlarm == null ? false : (bool)info.localAlarm,
                lossOfRentsLimit = info.lossOfRentsLimit,
                moneySecurityLimit = info.moneySecurityLimit,
                outdoorDescription = info.outdoorDescription,
                outdoorLimit = info.outdoorLimit,
                otherStructuresLimit = info.otherStructuresLimit,
                otherStructuresDescription = info.otherStructuresDescription,
                personalItemsLimit = info.personalItemsLimit,
                quake = info.quake == null ? false : (bool)info.quake,
                roofType = info.roofType == null ? 0 : (int)info.roofType,
                roofType1 = info.roofType1,
                shutters = info.shutters == null ? false : (bool)info.shutters,
				// 26 de Marzo, 2014 cambio pedido por Felipe Gutierrez para fechas del PDF
                startDate = info.startDate == null ? info.issueDate.AddDays(1) : (DateTime)info.startDate,
                endDate = info.endDate == null ? info.issueDate.AddDays(1).AddMonths(1) : (DateTime)info.endDate,
                taxP = taxP,
                taxTotal = ApplicationConstants.CalculateTax(netPremiumQuote, taxP, info.porcentajeRecargoPagoFraccionado, info.currency, info.fee.Value),
                tvCircuit = info.tvCircuit == null ? false : (bool)info.tvCircuit,
                tvGuard = info.tvGuard == null ? false : (bool)info.tvGuard,
                typeOfClient = info.typeOfClient == null ? 0 : (int)info.typeOfClient,
                typeOfProperty = info.typeOfProperty == null ? 0 : (int)info.typeOfProperty,
                typeOfProperty1 = info.typeOfProperty1,
                useOfProperty = info.useOfProperty == null ? 0 : (int)info.useOfProperty,
                useOfProperty1 = info.useOfProperty1,
                wallType = info.wallType == null ? 0 : (int)info.wallType,
                wallType1 = info.wallType1,
                stories = (int)info.stories,
                underground = (int)info.underground,
                clientType = info.clientType,
                distanceFromSea = info.distanceFromSea == null ? 0 : (int)info.distanceFromSea,
                distanceFromSea1 = info.distanceFromSea1,
                domesticWorkersLiability = info.domesticWorkersLiability,
                insuredPerson = info.insuredPerson == null ? 0 : (int)info.insuredPerson,
                insuredPerson1 = info.insuredPerson1,
                insuredPersonHouse = info.insuredPersonHouse == null ? 0 : (int)info.insuredPersonHouse,
                insuredPersonHouse1 = info.insuredPersonHouse1,
                agentAddress = userInfo.BranchInformation.BranchAddress,
                agentName = userInfo.AgentInformation.AgentName,
                descuento = info.descuento,
                userName = userInfo.UserName,
                currency = info.currency,
                cantidadDePagos = info.cantidadDePagos,
                porcentajeRecargoPagoFraccionado = info.porcentajeRecargoPagoFraccionado,
                recargoPagoFraccionado = info.recargoPagoFraccionado,
                techSupport = info.techSupport,
                additionalNotes = info.additionalNotes
            };
            #endregion
        } else {
            if (m_policyNo == null && m_policyId != 0) {
                data = db.policies.Single(m => m.id == m_policyId);
            } else {
                data = db.policies.Single(m => m.policyNo == m_policyNo);
            }            
        }
        decimal netPremium = data.netPremium;
        DeduciblesCoaseguros dataDeducibles = new DeduciblesCoaseguros(db, data.id, fromQuotes);
        return new TemplateData {
            deductibleData = dataDeducibles,
            netPremium = netPremium,
            policyData = data
        };
    }

    public string GetPolizaNo()
    {
        return m_policyNo;
    }

    public byte[] GetGeneratedPDF()
    {
        if (m_stream != null)
            return m_stream.ToArray();
        return null;
    }
}
