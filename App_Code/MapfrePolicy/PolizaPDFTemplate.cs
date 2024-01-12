using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ME.Admon;


public enum TemplateVersion
{
    Long,
    Short
}

public enum TemplateLanguage
{
    English,
    Spanish
}

public class DeduciblesCoaseguros
{
    public string DeducibleIntemperie { get; set; }
    public string CoaseguroIntemperie { get; set; }
    public string DeducibleFHM { get; set; }
    public string CoaseguroFHM { get; set; }
    public string DeducibleTerremoto { get; set; }
    public string CoaseguroTerremoto { get; set; }
    public DeduciblesCoaseguros(HouseInsuranceDataContext db, int policy, bool fromQuote)
    {
        int? quoteid = null;
        int? policyid = null;
        HouseEstate he;
        if (fromQuote)
        {
            he = db.quotes.Single(m => m.id == policy).insuredPersonHouse1.HouseEstate;
            quoteid = policy;
        }
        else
        {
            he = db.policies.Single(m => m.id == policy).insuredPersonHouse1.HouseEstate;
            policyid = policy;
        }
        var data = db.spGenerate_DeduciblesCoaseguros(quoteid, policyid, he.TEV, he.City.HMP).Single();
        DeducibleIntemperie = DeducibleHelper(data.deducibleInterperie);
        CoaseguroIntemperie = DeducibleHelper(data.coaseguroInterperie);
        DeducibleFHM = DeducibleHelper(data.deducibleFHM);
        CoaseguroFHM = DeducibleHelper(data.coaseguroFHM);
        DeducibleTerremoto = DeducibleHelper(data.deducibleterremoto);
        CoaseguroTerremoto = DeducibleHelper(data.coaseguroTerremoto);
    }
    private string DeducibleHelper(decimal? value)
    {
        if (value == null)
      {
         return null;
      }

      decimal v = (decimal)value;
      return v == 0.0M ? null : v.ToString("P");
   }
}

public class TemplateData
{
    public policy policyData;
    public decimal netPremium;
    public DeduciblesCoaseguros deductibleData;
}

public class TemplateValues
{
    public const float HORIZMARGIN = 22.0f;
    public const float VERTMARGIN = 25.0f;
    public BaseFont pdfFont = null;
    public float bottom, right, top, left;
    public const float textSize = 8.0f, diffText = 9.0f, boxheight = 35.0f, diffy = 38.0f;
    public float middleboxwidth, middle, middleboxX, middleboxEnd, largeBoxWidth, mediumBoxWidth;
}

/// <summary>
/// Contiene el template para generar el PDF. Simplifica en gran manera la clase PolizaPDF
/// </summary>
public class PolizaPDFTemplate
{
    private PdfTemplate m_template;
    public PdfTemplate UnderTest;
    private bool m_templateFirst;
    private PdfWriter m_writer;
    private PolizaStringFactory m_stringFactory;
    private TemplateData m_data;
    private TemplateValues m_values;
    private bool m_isQuote;

    public PolizaPDFTemplate(PdfWriter writer, TemplateVersion version, TemplateLanguage language, TemplateData data, bool isquote)
    {
        m_templateFirst = version == TemplateVersion.Long;
        m_isQuote = isquote;
        m_stringFactory = language == TemplateLanguage.English
           ? new PolizaStringFactory("~/PolizaEnglish.xml")
           : new PolizaStringFactory("~/PolizaEspanol.xml");
      InitVars(writer);
        m_writer = writer;
        m_data = data;
        PdfContentByte cb = writer.DirectContent;
        m_template = cb.CreateTemplate(PageSize.LETTER.Width, PageSize.LETTER.Height);
        GenerateFormat();
        FillAndGenerate(language);
    }

    private void InitVars(PdfWriter pdfWriter)
    {
        float tbottom = TemplateValues.VERTMARGIN;
        float tright = pdfWriter.DirectContent.PdfDocument.Right +
            pdfWriter.DirectContent.PdfDocument.RightMargin - TemplateValues.HORIZMARGIN;
        float ttop = pdfWriter.DirectContent.PdfDocument.Top +
            pdfWriter.DirectContent.PdfDocument.TopMargin - TemplateValues.VERTMARGIN;
        float tleft = TemplateValues.HORIZMARGIN;
        float tmiddleboxwidth = (tright - tleft) * 0.22f;
        float tmiddle = tleft + (tright - tleft) / 2;
        float tmiddleboxX = tmiddle - tmiddleboxwidth / 2;
        float tmiddleboxEnd = tmiddle + tmiddleboxwidth / 2;
        float tlargeBoxWidth = tmiddleboxX - tleft - 3.0f;
        float tmediumBoxWidth = tlargeBoxWidth / 2 - 1.5f;
        m_values = new TemplateValues
        {
            bottom = tbottom,
            right = tright,
            top = ttop,
            left = tleft,
            middleboxwidth = tmiddleboxwidth,
            middle = tmiddle,
            middleboxX = tmiddleboxX,
            middleboxEnd = tmiddleboxEnd,
            largeBoxWidth = tlargeBoxWidth,
            mediumBoxWidth = tmediumBoxWidth,
            pdfFont = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, false)
        };
    }

    private void FillAndGenerate(TemplateLanguage language)
    {
        FillTemplate(m_writer, m_template, m_data.policyData, m_data.netPremium, m_templateFirst, language);
        if (m_templateFirst)
        {
            PolizaPDFFillerT1.FillPDF(
                m_writer,
                m_template,
                UnderTest,
                m_data.policyData,
                m_data.netPremium,
                m_data.deductibleData,
                m_values,
                m_stringFactory,
                language
            );
        }
        else
        {
            PolizaPDFFillerT2.FillPDF(m_writer, m_template, UnderTest, m_values, m_data, m_stringFactory);
        }
    }

    private void FillTemplate(PdfWriter writer, PdfTemplate template, policy data, decimal netPremium, bool firstPart, TemplateLanguage language)
    {
        float cheight = m_values.top - 10.0f;
        BaseFont pdfFont = m_values.pdfFont;
        cheight -= TemplateValues.diffText;
        string policyNumber = m_isQuote ? m_stringFactory.GetString("QuoteC") : data.policyNo;

      string policyRenewal = data.previousPolicyNo.ToString();

        if (data.previousPolicyNo != 0 && data.previousPolicyNo != null)
         {
            HouseInsuranceDataContext db = new HouseInsuranceDataContext();
            policy policyRenew = db.policies.Single(m => m.id == data.previousPolicyNo);
            policyRenewal = policyRenew.policyNo;
         }

        template.DrawTextAligned(PdfContentByte.ALIGN_LEFT, policyNumber, pdfFont, TemplateValues.textSize, m_values.right - 90.0f, cheight);
        cheight -= 2 * TemplateValues.diffText;
        template.DrawTextAligned(PdfContentByte.ALIGN_LEFT, data.id.ToString(), pdfFont, TemplateValues.textSize, m_values.right - 90.0f, cheight);
        cheight -= 1 * TemplateValues.diffText;
        template.DrawTextAligned(PdfContentByte.ALIGN_LEFT, policyRenewal, pdfFont, TemplateValues.textSize, m_values.right - 90.0f, cheight);
        cheight = m_values.top - 65.0f;
        string name = data.insuredPerson1.firstname
            + (string.IsNullOrEmpty(data.insuredPerson1.lastName) ? string.Empty : " " + data.insuredPerson1.lastName)
            + (string.IsNullOrEmpty(data.insuredPerson1.lastName2) ? string.Empty : " " + data.insuredPerson1.lastName2);
        name = name.ToUpper();
        string rfc = string.Empty;
        if (!string.IsNullOrEmpty(data.insuredPerson1.rfc))
      {
         rfc = data.insuredPerson1.rfc;
      }

      template.DrawText(m_stringFactory.GetString("Insured") + name, pdfFont, TemplateValues.textSize, m_values.left + 20.0f, cheight);
        template.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, "R.F.C.: ", pdfFont, TemplateValues.textSize, m_values.right - 90.0f, cheight);
        template.DrawTextAligned(PdfContentByte.ALIGN_LEFT, rfc, pdfFont, TemplateValues.textSize, m_values.right - 90.0f, cheight);
        cheight -= TemplateValues.diffText;
        string aseguradoAdicional = string.Empty;
        if (!string.IsNullOrEmpty(data.insuredPerson1.additionalInsured))
        {
            aseguradoAdicional = data.insuredPerson1.additionalInsured.ToUpper();
        }
        template.DrawText(m_stringFactory.GetString("AndOr") + aseguradoAdicional, pdfFont, TemplateValues.textSize, m_values.left + 20.0f, cheight);
        template.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, m_stringFactory.GetString("Zip"), pdfFont, TemplateValues.textSize, m_values.right - 90.0f, cheight);
        string direccion = string.Empty, zip = string.Empty;

        insuredPersonHouse hData = data.insuredPersonHouse1;
        const int MAXADDRESSWITHSTATE = 78;
        direccion = hData.street + " " + hData.exteriorNo.Trim() + (string.IsNullOrEmpty(hData.InteriorNo) ? " " : " - " + hData.InteriorNo + " ")
            + hData.HouseEstate.Name + ", " + hData.HouseEstate.City.Name;
        direccion = direccion.Trim();
        float mSize = 0.0f;
        int addressLengthDim = direccion.Length + hData.HouseEstate.City.State.Name.Length + 2 - MAXADDRESSWITHSTATE;
        if (addressLengthDim <= 0)
        {
            direccion += ", " + hData.HouseEstate.City.State.Name;
        }
        else if (addressLengthDim <= 10)
        {
            direccion += ", " + hData.HouseEstate.City.State.Abbreviation;
        }
        else if (addressLengthDim <= 25)
        {
            direccion += ", " + hData.HouseEstate.City.State.Name;
            mSize = 2.0f;
        }
        else if (addressLengthDim <= 46)
        {
            direccion += ", " + hData.HouseEstate.City.State.Name;
            mSize = 3.0f;
        }
        else
        {
            direccion += ", " + hData.HouseEstate.City.State.Name;
            mSize = 4.0f;
        }
        zip = hData.ZipCode;
        direccion = direccion.ToUpper();
        template.DrawTextAligned(PdfContentByte.ALIGN_LEFT, zip, pdfFont, TemplateValues.textSize, m_values.right - 90.0f, cheight);
        cheight -= TemplateValues.diffText;

        template.DrawText(
            m_stringFactory.GetString("Address"),
            pdfFont,
            TemplateValues.textSize,
            m_values.left + 20.0f,
            cheight
        );
        template.DrawText(
            direccion,
            pdfFont,
            TemplateValues.textSize - mSize,
            m_values.left + 72.0f,
            cheight
        );
        template.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, m_stringFactory.GetString("Phone"), pdfFont, TemplateValues.textSize, m_values.right - 90.0f, cheight);
        template.DrawTextAligned(PdfContentByte.ALIGN_LEFT, data.insuredPerson1.phone, pdfFont, TemplateValues.textSize, m_values.right - 90.0f, cheight);

        cheight = m_values.top - 50.0f - TemplateValues.boxheight - 20.0f;
        CultureInfo culture;

        string formatDate = string.Empty;
        if (language == TemplateLanguage.English)
        {
            formatDate = "MMM/d/yyyy";
            culture = new CultureInfo("en-US");
        }
        else
        {
            formatDate = "dd/MM/yyyy";
            culture = new CultureInfo("es-MX");
        }
        string startDateStr = data.startDate.ToString(formatDate, culture);
        string endDateStr = data.endDate.ToString(formatDate, culture);
        string issueDateStr = data.issueDate.ToString(formatDate, culture);

        string inicio = m_stringFactory.GetString("FromInception") + startDateStr;
        string fin = m_stringFactory.GetString("ToInception") + endDateStr;
        template.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, m_stringFactory.GetString("Inception"), pdfFont, TemplateValues.textSize, m_values.left + 53.0f, cheight);
        template.DrawTextAligned(PdfContentByte.ALIGN_LEFT, inicio, pdfFont, TemplateValues.textSize, m_values.left + 53.0f, cheight);
        cheight -= TemplateValues.diffText;
        template.DrawTextAligned(PdfContentByte.ALIGN_LEFT, fin, pdfFont, TemplateValues.textSize, m_values.left + 53.0f, cheight);
        //template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, data.issuedBy.ToString(), pdfFont, TemplateValues.textSize, m_values.left + (m_values.right - m_values.left) / 2, cheight);
        template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, ApplicationConstants.MAPFREMENAME, pdfFont, TemplateValues.textSize, m_values.left + (m_values.right - m_values.left) / 2, cheight);
        cheight -= TemplateValues.diffy;

        float m1 = m_values.left + m_values.mediumBoxWidth / 2, m2 = m1 + m_values.mediumBoxWidth + 3.0f,
           m3 = m_values.middleboxEnd + 3.0f + m_values.mediumBoxWidth / 2, m4 = m3 + m_values.mediumBoxWidth + 3.0f;
        template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, issueDateStr, pdfFont, TemplateValues.textSize, m1, cheight);

        if (data.cantidadDePagos == 12)
        {
            template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, m_stringFactory.GetString("PaymentTypeDataMensual"), pdfFont, TemplateValues.textSize, m2, cheight);
        }
        else if (data.cantidadDePagos == 4)
        {
            template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, m_stringFactory.GetString("PaymentTypeDataTrimestral"), pdfFont, TemplateValues.textSize, m2, cheight);
        }
        else if (data.cantidadDePagos == 2)
        {
            template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, m_stringFactory.GetString("PaymentTypeDataSemestral"), pdfFont, TemplateValues.textSize, m2, cheight);
        }
        else
        {
            /* 1 contado */
            template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, m_stringFactory.GetString("PaymentTypeDataContado"), pdfFont, TemplateValues.textSize, m2, cheight);
        }

        template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, ApplicationConstants.MAPFREMEAGENT.ToString(), pdfFont, TemplateValues.textSize, m4, cheight);
        cheight -= TemplateValues.diffy;
        if (firstPart)
        {
            string rxpfP = (data.porcentajeRecargoPagoFraccionado * 100).ToString("F2");
            decimal total = ApplicationConstants.CalculateTotalWithData(data.netPremium, data.taxTotal, data.recargoPagoFraccionado, data.fee);

            template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, netPremium.ToString("C"), pdfFont, TemplateValues.textSize, m1, cheight);
            template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, data.fee.ToString("C2"), pdfFont, TemplateValues.textSize, m_values.middle, cheight);
            template.DrawText(rxpfP, pdfFont, TemplateValues.textSize, m2 - 45.0f, cheight);
            template.DrawText(data.recargoPagoFraccionado.ToString("C2"), pdfFont, TemplateValues.textSize, m2 + 20.0f, cheight);
            template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, data.taxP.ToString("P"), pdfFont, TemplateValues.textSize, m3 - m_values.mediumBoxWidth / 4, cheight);
            template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, data.taxTotal.ToString("C2"), pdfFont, TemplateValues.textSize, m3 + m_values.mediumBoxWidth / 4, cheight);
            template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, total.ToString("C2"), pdfFont, TemplateValues.textSize, m4, cheight);
        }
    }

    private void DrawWaterMark()
    {
        PdfTemplate under = null;
        if (m_isQuote || m_data.policyData.isTest)
        {
            under = m_writer.DirectContentUnder.CreateTemplate(PageSize.LETTER.Width, PageSize.LETTER.Height);

            float angleOfRotation = (float)(Math.Atan2((double)PageSize.LETTER.Height, (double)PageSize.LETTER.Width) * 180 / Math.PI);
            BaseFont font = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            float fontSize = 80;

            under.BeginText();

            under.SetTextRenderingMode(PdfPatternPainter.TEXT_RENDER_MODE_STROKE);
            under.SetColorStroke(new BaseColor(0.96f, 0, 0));
            under.SetFontAndSize(font, fontSize);

            string watermarkText = string.Empty;
            if (m_isQuote)
            {
                watermarkText = m_stringFactory.GetString("QuoteC");
            }
            else if (m_data.policyData.isTest)
            {
                watermarkText = m_stringFactory.GetString("Test");
            }

            under.ShowTextAligned(PdfContentByte.ALIGN_CENTER, watermarkText, PageSize.LETTER.Width / 2, PageSize.LETTER.Height / 2, angleOfRotation);
            under.EndText();

        }
        UnderTest = under;
    }

    private void DrawTemplate()
    {
        DrawLogo(m_template);
        DrawWaterMark();
        float cheight = m_values.top;
        m_template.DrawCurvedBox(m_values.right - 200.0f, cheight, 200.0f, 50.0f);
        cheight -= TemplateValues.diffy + 15.0f;
        m_template.DrawCurvedBox(m_values.left, cheight, m_values.right - m_values.left, TemplateValues.boxheight);
        cheight -= TemplateValues.diffy;

        m_template.DrawCurvedBox(m_values.left, cheight, m_values.largeBoxWidth, TemplateValues.boxheight);
        m_template.DrawCurvedBox(m_values.middleboxX, cheight, m_values.middleboxwidth, TemplateValues.boxheight);
        m_template.DrawCurvedBox(m_values.middleboxEnd + 3.0f, cheight, m_values.largeBoxWidth, TemplateValues.boxheight);
        cheight -= TemplateValues.diffy;

        m_template.DrawCurvedBox(m_values.left, cheight, m_values.mediumBoxWidth, TemplateValues.boxheight);
        m_template.DrawCurvedBox(m_values.left + m_values.mediumBoxWidth + 3.0f, cheight, m_values.mediumBoxWidth, TemplateValues.boxheight);
        m_template.DrawCurvedBox(m_values.middleboxX, cheight, m_values.middleboxwidth, TemplateValues.boxheight);
        m_template.DrawCurvedBox(m_values.middleboxEnd + 3.0f, cheight, m_values.mediumBoxWidth, TemplateValues.boxheight);
        m_template.DrawCurvedBox(m_values.middleboxEnd + 6.0f + m_values.mediumBoxWidth, cheight, m_values.mediumBoxWidth, TemplateValues.boxheight);
        cheight -= TemplateValues.diffy;
        if (m_templateFirst)
        {
            float separatorX = m_values.mediumBoxWidth * 0.32f;
            m_template.DrawCurvedBox(m_values.left, cheight, m_values.mediumBoxWidth, TemplateValues.boxheight);
            m_template.DrawCurvedBox(m_values.left + m_values.mediumBoxWidth + 3.0f, cheight, m_values.mediumBoxWidth, TemplateValues.boxheight);
            m_template.MoveTo(m_values.left + m_values.mediumBoxWidth + 3.0f + separatorX, cheight - TemplateValues.boxheight);
            m_template.LineTo(m_values.left + m_values.mediumBoxWidth + 3.0f + separatorX, cheight - TemplateValues.boxheight / 2);
            m_template.DrawCurvedBox(m_values.middleboxX, cheight, m_values.middleboxwidth, TemplateValues.boxheight);
            m_template.DrawCurvedBox(m_values.middleboxEnd + 3.0f, cheight, m_values.mediumBoxWidth, TemplateValues.boxheight);
            m_template.DrawCurvedBox(m_values.middleboxEnd + 6.0f + m_values.mediumBoxWidth, cheight, m_values.mediumBoxWidth, TemplateValues.boxheight);
            cheight -= TemplateValues.diffy;
        }

        m_template.DrawCurvedBox(m_values.left, cheight, m_values.right - m_values.left, TemplateValues.boxheight);
        cheight -= TemplateValues.diffy;

        float lastBoxY = m_values.bottom + 17.5f + 3.0f + 60.5f;
        m_template.DrawCurvedBox(m_values.left, cheight, m_values.right - m_values.left, cheight - lastBoxY - 3.0f);
        m_template.DrawCurvedBox(m_values.left, lastBoxY, m_values.right - m_values.left, 60.5f);
        DrawSignature(m_template);
    }

    private IEnumerable<string> WordBoundaryWrap(string line, int max)
    {
        if (line.Length <= max)
        {
            return new string[] { line };
        }

        List<string> lines = new List<string>();
        StringBuilder lineBuilder = new StringBuilder();

        foreach (var word in line.Split(' '))
        {
            if (word.Length + lineBuilder.Length + 1 > max)
            {
                lines.Add(lineBuilder.ToString());
                lineBuilder = new StringBuilder();
            }
            lineBuilder.Append(word);
            lineBuilder.Append(" ");
        }

        if (lineBuilder.Length > 0)
        {
            lines.Add(lineBuilder.ToString());
        }

        return lines;
    }

    private const int AGENT_LINE_MAX = 60;
    private const int SMALL_AGENT_LINE_MAX = 45;
    char[] splitOn = { ' ' };
    List<string> multipleAgentLines = new List<string>();

    private void DrawAgentName(BaseFont pdfFont, float cheight)
    {
        UserInformationDB userInfo = AdminGateway.Gateway.GetUserInformation(m_data.policyData.issuedBy);

        string agentNameLabel = m_stringFactory.GetString("LblAgentName").ToUpper();
        string agentFirstLine = userInfo.AgentInformation.AgentName.ToUpper();
        string agentSecondLine = userInfo.UserName.ToUpper() + " - " + m_data.policyData.issuedBy.ToString();

        multipleAgentLines.Add(agentFirstLine);
        if ((agentFirstLine.Length + agentNameLabel.Length) > AGENT_LINE_MAX || agentSecondLine.Length > AGENT_LINE_MAX)
        {
            cheight += TemplateValues.diffText * 0.7f;
            if ((agentFirstLine.Length + agentNameLabel.Length) > SMALL_AGENT_LINE_MAX)
            {
                string originalText = (agentFirstLine);
                multipleAgentLines = new List<string>();
                multipleAgentLines = SplitToLines(originalText, splitOn, SMALL_AGENT_LINE_MAX);
            }
        }

        m_template.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, agentNameLabel, pdfFont, TemplateValues.textSize - 2, m_values.middleboxEnd + 3.0f + m_values.mediumBoxWidth / 2, cheight);
        foreach (var line in multipleAgentLines)
        {
            m_template.DrawTextAligned(PdfContentByte.ALIGN_LEFT, line, pdfFont, TemplateValues.textSize - 2, m_values.middleboxEnd + 3.0f + m_values.mediumBoxWidth / 2, cheight);
            cheight -= TemplateValues.diffText;
        }

        foreach (string line in WordBoundaryWrap(agentSecondLine, AGENT_LINE_MAX))
        {
            m_template.DrawTextAligned(PdfContentByte.ALIGN_LEFT, line, pdfFont, TemplateValues.textSize - 2, m_values.middleboxEnd + 3.0f + m_values.mediumBoxWidth / 2, cheight);
            cheight -= TemplateValues.diffText;
        }
    }

    private void GenerateFormat()
    {
        DrawTemplate();
        BaseFont pdfFont = m_values.pdfFont;
        float cheight = m_values.top - 10.0f - TemplateValues.diffText;
        m_template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, m_stringFactory.GetString("Title"), pdfFont, 10.0f, m_values.middle, m_values.top - 10.0f);
        m_template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, "\"HOGAR BIEN SEGURO\"", pdfFont, 10.0f, m_values.middle, m_values.top - 20.0f);
        DrawTextAddress(m_template);
        m_template.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, m_stringFactory.GetString("TypeDocument"), pdfFont, TemplateValues.textSize, m_values.right - 90.0f, m_values.top - 10.0f);
        string typeOfDocument;
        if (m_isQuote)
        {
            typeOfDocument = m_stringFactory.GetString("QuoteC");
            m_template.DrawText("____________________", pdfFont, TemplateValues.textSize, m_values.right - 105.0f, 132.0f);
            m_template.DrawText("Insured Signature", pdfFont, TemplateValues.textSize, m_values.right - 100.0f, 120.0f);
        }
        else
        {
            typeOfDocument = m_stringFactory.GetString("PolicyC");
        }
        m_template.DrawTextAligned(PdfContentByte.ALIGN_LEFT, typeOfDocument, pdfFont, TemplateValues.textSize, m_values.right - 90.0f, m_values.top - 10.0f);
        m_template.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, m_stringFactory.GetString("LblPolicyNo"), pdfFont, TemplateValues.textSize, m_values.right - 90.0f, cheight);
        cheight -= TemplateValues.diffText;
        m_template.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, m_stringFactory.GetString("LblEndorsement"), pdfFont, TemplateValues.textSize, m_values.right - 90.0f, cheight);
        m_template.DrawTextAligned(PdfContentByte.ALIGN_LEFT, "0", pdfFont, TemplateValues.textSize, m_values.right - 90.0f, cheight);
        cheight -= TemplateValues.diffText;
        m_template.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, m_stringFactory.GetString("LblReference"), pdfFont, TemplateValues.textSize, m_values.right - 90.0f, cheight);
        cheight -= TemplateValues.diffText;
        m_template.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, m_stringFactory.GetString("LblRenewal"), pdfFont, TemplateValues.textSize, m_values.right - 90.0f, cheight);
        cheight -= TemplateValues.diffText;
        //m_template.DrawTextAligned(PdfContentByte.ALIGN_LEFT, "ERIC", pdfFont, TemplateValues.textSize, m_values.right - 90.0f, cheight);
        cheight = m_values.top - 50.0f - TemplateValues.boxheight - 20.0f;
        m_template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, m_stringFactory.GetString("LblAgentNo"), pdfFont, TemplateValues.textSize, m_values.middle, cheight);


        DrawAgentName(pdfFont, cheight);
        cheight -= TemplateValues.diffy;

        float m1 = m_values.left + m_values.mediumBoxWidth / 2, m2 = m1 + m_values.mediumBoxWidth + 3.0f,
            m3 = m_values.middleboxEnd + 3.0f + m_values.mediumBoxWidth / 2, m4 = m3 + m_values.mediumBoxWidth + 3.0f;
        m_template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, m_stringFactory.GetString("IssueDate"), pdfFont, TemplateValues.textSize, m1, cheight);
        m_template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, m_stringFactory.GetString("PaymentType"), pdfFont, TemplateValues.textSize, m2, cheight);
        m_template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, m_stringFactory.GetString("Currency"), pdfFont, TemplateValues.textSize, m_values.middle, cheight);

        string currency = string.Empty;
        currency = m_data.policyData.currency == 2 ? m_stringFactory.GetString("Dollars") : m_stringFactory.GetString("Pesos");
      m_template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, currency, pdfFont, TemplateValues.textSize, m_values.middle, cheight - TemplateValues.diffText);
        m_template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, m_stringFactory.GetString("PaymentForm"), pdfFont, TemplateValues.textSize, m3, cheight);
        m_template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, m_stringFactory.GetString("PayFormAgent"), pdfFont, TemplateValues.textSize, m3, cheight - TemplateValues.diffText);
        m_template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, m_stringFactory.GetString("PaymentCode"), pdfFont, TemplateValues.textSize, m4, cheight);
        cheight -= TemplateValues.diffy;
        if (m_templateFirst)
        {
            m_template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, m_stringFactory.GetString("NetPremium"), pdfFont, TemplateValues.textSize, m1, cheight);
            m_template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, m_stringFactory.GetString("Surcharge"), pdfFont, TemplateValues.textSize, m2, cheight);
            m_template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, m_stringFactory.GetString("PolicyFee"), pdfFont, TemplateValues.textSize, m_values.middle, cheight);
            m_template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, "%", pdfFont, TemplateValues.textSize, m3 - m_values.mediumBoxWidth / 4, cheight);
            m_template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, m_stringFactory.GetString("Tax"), pdfFont, TemplateValues.textSize, m3 + m_values.mediumBoxWidth / 4, cheight);
            m_template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, m_stringFactory.GetString("TotalPremium"), pdfFont, TemplateValues.textSize, m4, cheight);
            cheight -= TemplateValues.diffy;
        }
        m_template.DrawText(m_stringFactory.GetString("PreTagLine1"), pdfFont, TemplateValues.textSize, m_values.left + 20.0f, cheight);
        m_template.DrawText(m_stringFactory.GetString("PreTagLine2"), pdfFont, TemplateValues.textSize, m_values.left + 20.0f, cheight - TemplateValues.diffText);

        float smallText = TemplateValues.textSize - 2.0f, smallDiff = TemplateValues.diffText - 2.0f;
        //lastBoxY aparece tambien arriba
        float lastBoxY = m_values.bottom + 17.5f + 3.0f + 60.5f;
        cheight = lastBoxY - 10.0f;
        m_template.DrawText(m_stringFactory.GetString("FooterLine1"), pdfFont, smallText, m_values.left + 20.0f, cheight);
        cheight -= smallDiff;
        m_template.DrawText(m_stringFactory.GetString("FooterLine2"), pdfFont, smallText, m_values.left + 20.0f, cheight);
        cheight -= smallDiff;
        m_template.DrawText(m_stringFactory.GetString("FooterLine3"), pdfFont, smallText, m_values.left + 20.0f, cheight);
        cheight -= smallDiff;
        m_template.DrawText(m_stringFactory.GetString("FooterLine4"), pdfFont, smallText, m_values.left + 20.0f, cheight);
        cheight = m_values.bottom + 17.5f + 3.0f + 10.0f;
        m_template.DrawTextAligned(PdfContentByte.ALIGN_CENTER, m_stringFactory.GetString("FooterLine5"), pdfFont, TemplateValues.textSize, m_values.middle, cheight);
    }

    private void DrawLogo(PdfContentByte cb)
    {
        float logox = TemplateValues.HORIZMARGIN;
        float logoy = cb.PdfDocument.Top - 18.0f;
        cb.DrawImage("~/images/LOGOoct06.jpg", logox, logoy, 0.30f);
    }

    private void DrawSignature(PdfContentByte cb)
    {
        float sigx = (m_values.right - m_values.left) * 0.88f;
        float sigy = TemplateValues.VERTMARGIN + 17.5f + 4.0f;
        //DrawImage(cb, "~/images/firmajrdv.jpg", sigx, sigy, 0.14f);
        cb.DrawImage("~/images/FirmaHO.jpg", sigx, sigy, 0.81f);
    }


    private void DrawTextAddress(PdfTemplate cb)
    {
        float cheight = m_values.top - 30.0f;
        BaseFont pdfFont = m_values.pdfFont;
        csCompany mapfreCompany = new csCompany(ApplicationConstants.COMPANY, ApplicationConstants.ADMINDB);
        //cb.DrawText("Boulevard Magnocentro #5, Col. Centro Urbano", pdfFont, 5.0f, m_values.left + 20.0f, cheight);
        cb.DrawText(mapfreCompany.address1, pdfFont, 8.0f, m_values.left + 20.0f, cheight);
        cheight -= 6.0f;
        //cb.DrawText("Huixquilucan, Estado de México C.P. 52670", pdfFont, 5.0f, m_values.left + 20.0f, cheight);
        cb.DrawText(mapfreCompany.address2, pdfFont, 8.0f, m_values.left + 20.0f, cheight);
        cheight -= 6.0f;
        cb.DrawText("Tel.: 01(800) 01 365 24 Interior de la República", pdfFont, 8.0f, m_values.left + 20.0f, cheight);
        cheight -= 6.0f;
        cb.DrawText("Para D.F. Tel. 52-30-71-71 R.F.C. MTE-440316 E54", pdfFont, 8.0f, m_values.left + 20.0f, cheight);
    }

    private List<string> SplitToLines(string originalText, char[] splitOnCharacter, int maxStringLength)
    {
        List<string> sb = new List<string>();
        int index = 0;

        while (originalText.Length > index)
        {
            // get the next substring, else the rest of the string if remainder is shorter than `maxStringLength`
            var splitAt = index + maxStringLength <= originalText.Length ?
                originalText.Substring(index, maxStringLength).LastIndexOfAny(splitOnCharacter) :
                originalText.Length - index;

            // if can't find split location, take `maxStringLength` charachters
            splitAt = (splitAt == -1) ? maxStringLength : splitAt;

            // add result to collection & increment index
            sb.Add(originalText.Substring(index, splitAt).Trim());
            index += splitAt;
        }

        return sb;
    }
}
