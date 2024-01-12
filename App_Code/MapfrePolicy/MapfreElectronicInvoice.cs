using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Globalization;
using System.IO;
using System.Xml.Linq;


public class MapfreElectronicInvoice
{
    private TemplateData m_data;
    private PdfWriter m_writer;
    private PdfTemplate m_template, m_under;
    private float m_bottom, m_top, m_right, m_left, m_center, m_textSize, m_titleSize, m_diffText, m_smallSize, m_boxDiffText;
    private float m_middleLeftSize, m_middleRightSize;
    private BaseColor RED = new BaseColor(0.96f, 0, 0);
    private PolizaStringFactory m_stringFactory;
    private BaseFont m_font, m_boldFont;

    private string m_policyNo = null;
    private MemoryStream m_stream = null;
    private bool m_fepadre = false;


    public MapfreElectronicInvoice(int policyid, bool fepadre)
    {
        Document pdfDocument = new Document(PageSize.LETTER);
        MemoryStream memStream = new MemoryStream();
        m_writer = PdfWriter.GetInstance(pdfDocument, memStream);
        m_data = GetData(policyid);
        if (m_data.policyData.cantidadDePagos == 1)
        {
            fepadre = false;
        }

        m_fepadre = fepadre;
        m_stringFactory = new PolizaStringFactory("~/PolizaEspanol.xml");
        pdfDocument.AddAuthor("Macafee and Edwards");
        pdfDocument.Open();

        pdfDocument.AddTitle("Homeowner's policy number: " + m_data.policyData.policyNo);
        InitVars(m_writer);
        m_under = DrawWaterMark();

        Generate();

        m_policyNo = m_data.policyData.policyNo;
        pdfDocument.Close();
        m_stream = memStream;
    }

    private void Generate()
    {
        m_template = CreateTemplate();
        FillInvoice();
    }

    private TemplateData GetData(int policyid)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        policy data = db.policies.Single(m => m.id == policyid);

        if (string.IsNullOrEmpty(data.insuredPerson1.rfc))
        {
            throw new InvalidOperationException("El asegurado debe tener un RFC registrado");
        }

        decimal netPremium = data.netPremium;
        DeduciblesCoaseguros dataDeducibles = new DeduciblesCoaseguros(db, data.id, false);
        return new TemplateData
        {
            deductibleData = dataDeducibles,
            netPremium = netPremium,
            policyData = data
        };
    }

    private PdfTemplate DrawWaterMark()
    {
        PdfTemplate under = null;
        if (m_data.policyData.isTest)
        {
            under = m_writer.DirectContentUnder.CreateTemplate(PageSize.LETTER.Width, PageSize.LETTER.Height);

            float angleOfRotation = (float)(Math.Atan2((double)PageSize.LETTER.Height, (double)PageSize.LETTER.Width) * 180 / Math.PI);
            BaseFont font = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            float fontSize = 80;

            under.BeginText();

            under.SetTextRenderingMode(PdfPatternPainter.TEXT_RENDER_MODE_STROKE);
            under.SetColorStroke(new BaseColor(0.96f, 0, 0));
            under.SetFontAndSize(font, fontSize);

            string watermarkText = m_stringFactory.GetString("Test");

            under.ShowTextAligned(PdfContentByte.ALIGN_CENTER, watermarkText, PageSize.LETTER.Width / 2, PageSize.LETTER.Height / 2, angleOfRotation);
            under.EndText();

        }
        return under;
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

    private void InitVars(PdfWriter pdfWriter)
    {
        m_bottom = TemplateValues.VERTMARGIN;
        m_right = pdfWriter.DirectContent.PdfDocument.Right +
            pdfWriter.DirectContent.PdfDocument.RightMargin - TemplateValues.HORIZMARGIN;
        m_top = pdfWriter.DirectContent.PdfDocument.Top +
            pdfWriter.DirectContent.PdfDocument.TopMargin - TemplateValues.VERTMARGIN - 30f;
        m_left = TemplateValues.HORIZMARGIN + 2f;
        m_font = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
        m_boldFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);
        m_textSize = 7.0f;
        m_smallSize = 5.5f;
        m_titleSize = 12.0f;
        m_diffText = 10.0f;
        m_boxDiffText = 10.0f;
        m_middleLeftSize = (m_right - m_left) * 0.57f;
        m_middleRightSize = (m_right - m_left) * 0.23f;
        //float tmiddleboxwidth = (tright - tleft) * 0.22f;
        m_center = m_left + (m_right - m_left) / 2;
        //float tmiddleboxX = tmiddle - tmiddleboxwidth / 2;
        //float tmiddleboxEnd = tmiddle + tmiddleboxwidth / 2;
        //float tlargeBoxWidth = tmiddleboxX - tleft - 3.0f;
        //float tmediumBoxWidth = tlargeBoxWidth / 2 - 1.5f;
    }

    private PdfTemplate CreateTemplate()
    {
        PdfContentByte cb = m_writer.DirectContent;
        PdfTemplate template = cb.CreateTemplate(PageSize.LETTER.Width, PageSize.LETTER.Height);
        //todo draw template
        DrawInvoiceLogo(template);
        DrawTextComponents(template);
        DrawBoxes(template);

        return template;
    }

    private void DrawBoxes(PdfContentByte cb)
    {
        float cheight = m_top - m_diffText * 8.5f;
        float boxHeight = m_boxDiffText * 3.5f;
        cb.DrawCurvedBox(m_left, cheight, m_right - m_left, m_boxDiffText * 3.5f);
        cheight -= boxHeight + 3.0f;
        cb.DrawCurvedBox(m_left, cheight, m_middleLeftSize, m_boxDiffText * 9.5f);
        cb.DrawCurvedBox(m_left + m_middleLeftSize + 3.0f, cheight, m_right - m_left - m_middleLeftSize - 3.0f, m_boxDiffText * 24.5f);
        cheight -= m_boxDiffText * 24.5f + 3.0f;
        cb.DrawCurvedBox(m_left, cheight, m_right - m_left - 3.0f - m_middleRightSize, m_boxDiffText * 12.5f);
        cb.DrawCurvedBox(m_right - m_middleRightSize, cheight, m_middleRightSize, m_boxDiffText * 12.5f);
        cheight -= m_boxDiffText * 14;
        cb.DrawBox(m_left, cheight, m_right - m_left, m_boxDiffText * 3.0f);
        cheight -= m_boxDiffText * 4.5f;
        cb.DrawBox(m_left, cheight, m_right - m_left, m_boxDiffText * 4.5f);
        cheight -= m_boxDiffText * 6;
        cb.DrawLine(m_left, cheight, m_right, cheight);

    }

    private PolicyInvoiceData GetDataFromFE()
    {
        if (m_fepadre)
        {
            var fepadre = m_data.policyData.policyFEPADRE;
            return new PolicyInvoiceData
            {
                ApprovalYear = fepadre.approvalYear,
                ApprovalNumber = fepadre.approvalNo,
                BankReferencesXML = fepadre.bankReferencesXML,
                Certificate = fepadre.certificate,
                CertificateNo = fepadre.certificateNo,
                Date = fepadre.date,
                Folio = fepadre.folio,
                FxReceiptType = fepadre.fxReceiptType,
                IssuedIn = fepadre.issuedIn,
                MapfreEndoso = fepadre.mapfreEndoso,
                MapfreInternalNumber = fepadre.mapfreInternalNo,
                MapfreMEAgentName = fepadre.mapfreMEAgentName,
                MapfreMEKey = fepadre.mapfreMEKey,
                OriginalString = fepadre.originalString,
                PaymentMedium = fepadre.paymentMedium,
                PaymentType = fepadre.paymentType,
                PolicyID = fepadre.policy,
                ReceiptSeries = fepadre.receiptSeries,
                ReceiptType = fepadre.receiptType,
                Registry = fepadre.registry,
                Seal = fepadre.seal,
                Series = fepadre.series
            };
        }
        else
        {
            var fe = m_data.policyData.policyFE;
            return new PolicyInvoiceData
            {
                ApprovalYear = fe.approvalYear,
                ApprovalNumber = fe.approvalNo,
                BankReferencesXML = fe.bankReferencesXML,
                Certificate = fe.certificate,
                CertificateNo = fe.certificateNo,
                Date = fe.date,
                Folio = fe.folio,
                FxReceiptType = fe.fxReceiptType,
                IssuedIn = fe.issuedIn,
                MapfreEndoso = fe.mapfreEndoso,
                MapfreInternalNumber = fe.mapfreInternalNo,
                MapfreMEAgentName = fe.mapfreMEAgentName,
                MapfreMEKey = fe.mapfreMEKey,
                OriginalString = fe.originalString,
                PaymentMedium = fe.paymentMedium,
                PaymentType = fe.paymentType,
                PolicyID = fe.policy,
                ReceiptSeries = fe.receiptSeries,
                ReceiptType = fe.receiptType,
                Registry = fe.registry,
                Seal = fe.seal,
                Series = fe.series
            };
        }
    }

    private void DrawTextComponents(PdfContentByte cb)
    {
        float cheight = m_top;
        var dataFE = GetDataFromFE();

        cb.DrawTextAligned(PdfContentByte.ALIGN_CENTER, m_stringFactory.GetString("InvoiceTitle"), m_boldFont, m_titleSize, m_center, cheight);
        cheight -= m_diffText;
        cb.DrawText(m_stringFactory.GetString("YearApproval"), m_boldFont, m_textSize, m_left, cheight);
        cb.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, dataFE.ApprovalYear + "/" + dataFE.ApprovalNumber, m_font, m_textSize, m_left + 170.0f, cheight);
        cheight -= m_diffText;
        cb.DrawText(m_stringFactory.GetString("Series"), m_boldFont, m_textSize, m_left, cheight);
        cb.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, dataFE.Series, m_font, m_textSize, m_left + 170.0f, cheight);
        cheight -= m_diffText;
        cb.DrawText(m_stringFactory.GetString("LblReference"), m_boldFont, m_textSize, m_left, cheight);
        cb.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, dataFE.Folio, m_font, m_textSize, m_left + 170.0f, cheight);
        cheight -= m_diffText;
        cb.DrawText(m_stringFactory.GetString("CertificateNo"), m_boldFont, m_textSize, m_left, cheight);
        cb.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, dataFE.CertificateNo, m_font, m_textSize, m_left + 170.0f, cheight);
        cheight -= m_diffText;
        cb.DrawText(m_stringFactory.GetString("Date"), m_boldFont, m_textSize, m_left, cheight);
        cb.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, dataFE.Date, m_font, m_textSize, m_left + 170.0f, cheight);
        cb.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, m_stringFactory.GetString("MapfreCaps"), m_boldFont, m_smallSize, m_right, cheight);
        cheight -= m_diffText;
        cb.DrawText(m_stringFactory.GetString("PaymentType"), m_boldFont, m_textSize, m_left, cheight);
        cb.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, dataFE.PaymentType, m_font, m_textSize, m_left + 170.0f, cheight);
        cb.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, m_stringFactory.GetString("MapfreAddress1"), m_boldFont, m_smallSize, m_right, cheight);
        cheight -= m_diffText;
        cb.DrawText(m_stringFactory.GetString("PaymentMedium"), m_boldFont, m_textSize, m_left, cheight);
        cb.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, dataFE.PaymentMedium, m_font, m_textSize, m_left + 170.0f, cheight);
        cb.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, m_stringFactory.GetString("MapfreAddress2"), m_boldFont, m_smallSize, m_right, cheight);
        cheight -= m_diffText * 1.5f + m_boxDiffText * 14.0f + 6.0f;

        cb.DrawText("Estimado Asegurado:", m_font, m_textSize, m_left + 30.0f, cheight);

        cheight -= m_diffText;
        cb.DrawText("Si Usted realiza el pago de su póliza con Cheque, por favor debe expedirlo a favor de", m_font, m_textSize, m_left + 30.0f, cheight);
        cheight -= m_diffText;
        cb.DrawText("MAPFRE México, S.A. y deberá anotar con tinta el número de su póliza al dorso del", m_font, m_textSize, m_left + 30.0f, cheight);
        cheight -= m_diffText;
        cb.DrawText("mismo. Si paga con cheque, se entenderá de recibido salvo buen cobro como lo indica el", m_font, m_textSize, m_left + 30.0f, cheight);
        cheight -= m_diffText;
        cb.DrawText("Art. 7o. De la Ley General De Títulos y Operaciones De Crédito.", m_font, m_textSize, m_left + 30.0f, cheight);
        cheight -= m_diffText * 2;

        cb.DrawText("Si Usted realiza el pago de su póliza en las Ventanillas Bancarias o en las Ventanillas de", m_font, m_textSize, m_left + 30.0f, cheight);
        cheight -= m_diffText;
        cb.DrawText("MAPFRE México, S.A., su recibo deberá contener INVARIABLEMENTE EL SELLO y/o", m_font, m_textSize, m_left + 30.0f, cheight);
        cheight -= m_diffText;
        cb.DrawText("COMPROBANTE BANCARIO, de lo contrario es nulo para cualquier reclamación y/o", m_font, m_textSize, m_left + 30.0f, cheight);
        cheight -= m_diffText;
        cb.DrawText("aclaración.", m_font, m_textSize, m_left + 30.0f, cheight);
        cheight -= m_diffText * 2;

        cb.DrawText("Si usted realiza el pago de su prima de seguros en efectivo a través de su agente, le", m_font, m_textSize, m_left + 30.0f, cheight);
        cheight -= m_diffText;
        cb.DrawText("sugerimos solicitar al mismo anote nombre, fecha y firma en este documento y solicítele la", m_font, m_textSize, m_left + 30.0f, cheight);
        cheight -= m_diffText;
        cb.DrawText("ficha de deposito, para respaldar el pago.", m_font, m_textSize, m_left + 30.0f, cheight);
        cheight -= m_diffText;

        cheight = m_top - m_diffText * 8.5f - m_boxDiffText * 29 - 6.0f;
        cb.DrawTextAligned(PdfContentByte.ALIGN_CENTER, "Sello del Banco o Caja", m_font, m_textSize, m_right - m_middleRightSize / 2, cheight);
        WriteBankInformation(dataFE.BankReferencesXML, cb, m_left + 20f, cheight);
        cheight -= m_boxDiffText * 12.5f;

        cb.DrawText("SELLO DIGITAL", m_boldFont, m_textSize, m_left, cheight);
        cheight -= m_boxDiffText * 1.5f;
        string sello = dataFE.Seal;
        const int rowSize = 132;
        int totalRows = 0;
        while (sello.Length > 0)
        {
            int currentRowSize = Math.Min(sello.Length, rowSize);
            string row = sello.Substring(0, currentRowSize);
            cb.DrawText(row, m_font, m_textSize, m_left + 10.0f, cheight);
            cheight -= m_boxDiffText;
            totalRows++;
            sello = sello.Substring(currentRowSize);
        }

        cheight -= m_boxDiffText * (3f - totalRows);
        cb.DrawText("CADENA ORIGINAL", m_boldFont, m_textSize, m_left, cheight);
        cheight -= m_boxDiffText * 1.5f;
        string cadena = dataFE.OriginalString;
        while (cadena.Length > 0)
        {
            int currentRowSize = Math.Min(cadena.Length, rowSize);
            string row = cadena.Substring(0, currentRowSize);
            cb.DrawText(row, m_font, m_textSize, m_left + 10.0f, cheight);
            cheight -= m_boxDiffText;
            cadena = cadena.Substring(currentRowSize);
        }

        cheight = m_bottom + m_diffText * 2.5f;
        cb.DrawText("ESTE DOCUMENTO ES UNA IMPRESION DE UN COMPROBANTE FISCAL DIGITAL", m_font, m_textSize, m_left, cheight);
        cb.DrawTextAligned(
            PdfContentByte.ALIGN_RIGHT,
            (m_stringFactory.GetString("Series") + dataFE.Series).ToUpper(),
            m_font,
            m_textSize,
            m_right,
            cheight
        );
        cheight -= m_diffText;
        cb.DrawText("EMISOR: MAPFRE MÉXICO, S.A.", m_font, m_textSize, m_left, cheight);
        cb.DrawTextAligned(
            PdfContentByte.ALIGN_RIGHT,
            (m_stringFactory.GetString("LblReference") + dataFE.Folio).ToUpper(),
            m_font,
            m_textSize,
            m_right,
            cheight
        );
        cheight -= m_diffText;
        cb.DrawTextAligned(
            PdfContentByte.ALIGN_RIGHT,
            "Página 1 de 1",
            m_font,
            m_smallSize,
            m_right,
            cheight
        );

    }

    private void WriteBankInformation(string encodedBank, PdfContentByte template, float xpos, float ypos)
    {
        PdfPTable table = new PdfPTable(new float[] { 0.2f, 0.2f, 0.6f });
        table.TotalWidth = m_right - m_left - 33.0f - m_middleRightSize;
        XElement banksDoc = XElement.Parse(encodedBank);
        var banks = banksDoc.Descendants("ReferenciaBancaria");
        int numBanks = 0;
        string[] xmlElements = { "Banco", "Convenio", "RefCliente" };
        Font pFont = new Font(m_font, m_textSize);

        foreach (var bank in banks)
        {
            foreach (string element in xmlElements)
            {
                string cellContent = (string)bank.Element(element) ?? string.Empty;
                PdfPCell cell = new PdfPCell(new Phrase(cellContent, pFont));
                cell.Border = 0;
                cell.Padding = 3f;

                table.AddCell(cell);
            }
            numBanks++;
        }
        table.WriteSelectedRows(0, numBanks, xpos, ypos, template);
    }

    private void WritePaymentInformation(policy p, PdfContentByte template, float xpos, float ypos)
    {
        PdfPTable table = new PdfPTable(new float[] { 0.5f, 0.2f, 0.2f, 0.1f });
        table.TotalWidth = m_right - m_left - m_middleLeftSize - 13.0f;
        table.DefaultCell.Border = 0;
        Font pFont = new Font(m_font, m_textSize),
             bFont = new Font(m_boldFont, m_textSize);
        Phrase emptyPhrase = new Phrase(string.Empty, pFont);
        Phrase currencyPhrase = new Phrase(
            p.currency == ApplicationConstants.DOLLARS_CURRENCY ? "USD" : "MXN",
            pFont
        );

        decimal premiumFracc = p.netPremium / p.cantidadDePagos;
        decimal rxpf = premiumFracc * p.porcentajeRecargoPagoFraccionado;
        decimal feeRX = p.fee;

        table.AddCell(emptyPhrase);
        table.AddCell(GetRightAlignedCell(new Phrase("Total", bFont)));
        if (p.cantidadDePagos > 1)
        {
            table.AddCell(GetRightAlignedCell(new Phrase("Parcialidad", bFont)));
        }
        else
        {
            table.AddCell(emptyPhrase);
        }
        table.AddCell(emptyPhrase);

        table.AddCell(new Phrase("Prima neta", pFont));
        table.AddCell(GetRightAlignedCell(new Phrase(p.netPremium.ToString("N2"), pFont)));
        if (p.cantidadDePagos > 1)
        {
            table.AddCell(GetRightAlignedCell(new Phrase(premiumFracc.ToString("N2"), pFont)));
        }
        else
        {
            table.AddCell(emptyPhrase);
        }
        table.AddCell(currencyPhrase);

        table.AddCell(new Phrase("Gasto expedición", pFont));
        table.AddCell(GetRightAlignedCell(new Phrase(p.fee.ToString("N2"), pFont)));
        if (p.cantidadDePagos > 1)
        {
            table.AddCell(GetRightAlignedCell(new Phrase(p.fee.ToString("N2"), pFont)));
        }
        else
        {
            table.AddCell(emptyPhrase);

        }
        table.AddCell(currencyPhrase);

        table.AddCell(new Phrase("Finan. pago fracc.", pFont));
        table.AddCell(GetRightAlignedCell(new Phrase(p.recargoPagoFraccionado.ToString("N2"), pFont)));
        if (p.cantidadDePagos > 1)
        {
            table.AddCell(GetRightAlignedCell(new Phrase(rxpf.ToString("N"), pFont)));
        }
        else
        {
            table.AddCell(emptyPhrase);
        }
        table.AddCell(currencyPhrase);

        table.AddCell(new Phrase("IVA " + p.taxP.ToString("P"), pFont));
        table.AddCell(GetRightAlignedCell(new Phrase(p.taxTotal.ToString("N2"), pFont)));
        decimal tax = (premiumFracc + rxpf + feeRX) * p.taxP;
        if (p.cantidadDePagos > 1)
        {
            table.AddCell(GetRightAlignedCell(new Phrase(tax.ToString("N2"), pFont)));
        }
        else
        {
            table.AddCell(emptyPhrase);
        }
        table.AddCell(currencyPhrase);

        table.AddCell(new Phrase("Total a pagar", pFont));
        decimal total = ApplicationConstants.CalculateTotalWithData(p.netPremium, p.taxTotal, p.recargoPagoFraccionado, p.fee);
        decimal totalRX = ApplicationConstants.CalculateTotalWithData(premiumFracc, tax, rxpf, feeRX);
        PdfPCell cell = GetRightAlignedCell(new Phrase(total.ToString("C"), pFont));
        cell.BorderWidthTop = 1f;
        table.AddCell(cell);
        if (p.cantidadDePagos > 1)
        {
            cell = GetRightAlignedCell(new Phrase(totalRX.ToString("C"), pFont));
        }
        else
        {
            cell = GetRightAlignedCell(new Phrase(string.Empty, pFont));
        }
        cell.BorderWidthTop = 1f;
        table.AddCell(cell);
        table.AddCell(emptyPhrase);

        table.WriteSelectedRows(0, 6, xpos, ypos, template);
    }

    private PdfPCell GetRightAlignedCell(Phrase p)
    {
        PdfPCell cell = new PdfPCell(p);
        cell.Border = 0;
        cell.HorizontalAlignment = PdfContentByte.ALIGN_RIGHT;
        return cell;
    }

    private void DrawInvoiceLogo(PdfContentByte cb)
    {
        cb.DrawImage("~/images/logoMapfreInvoice.png", m_right - 170f, m_top - 35, 0.33f);
    }

    private string GetDateFormat(DateTime dt)
    {
        return dt.ToString("d/MMM/yyyy", CultureInfo.CreateSpecificCulture("es-MX"));
    }

    private string GetEndDate(policy p)
    {
        if (1 == p.cantidadDePagos)
        {
            return GetDateFormat(p.endDate);
        }

        int addMonths = 12 / p.cantidadDePagos;
        return GetDateFormat(p.startDate.AddMonths(addMonths).AddDays(-1));
    }

    private void FillAgentAndDate(policy p, PdfContentByte template, float xpos, float ypos)
    {
        PdfPTable table = new PdfPTable(new float[] { 0.57f, 0.43f });
        table.TotalWidth = m_middleLeftSize - 10;
        table.DefaultCell.Border = 0;
        Font pFont = new Font(m_font, m_textSize),
             bFont = new Font(m_boldFont, m_textSize);
        Phrase emptyPhrase = new Phrase(string.Empty, pFont);
        var dataFE = GetDataFromFE();
        string startDateStr = GetDateFormat(p.startDate);
        string endDateStr = GetEndDate(p);
        string issueDateStr = GetDateFormat(p.issueDate);
        string inicio = m_stringFactory.GetString("FromInception") + startDateStr;
        inicio = inicio.ToLower();
        string fin = m_stringFactory.GetString("ToInception") + endDateStr;

        table.AddCell(emptyPhrase);
        table.AddCell(new Phrase(m_stringFactory.GetString("Endoso") + dataFE.MapfreEndoso, pFont));

        table.AddCell(new Phrase(m_stringFactory.GetString("Inception") + inicio, pFont));
        table.AddCell(new Phrase(fin, pFont));

        table.AddCell(new Phrase(m_stringFactory.GetString("LblAgentName") + dataFE.MapfreMEAgentName, pFont));
        table.AddCell(new Phrase(m_stringFactory.GetString("AgentKey") + dataFE.MapfreMEKey, pFont));

        table.AddCell(emptyPhrase);
        table.AddCell(emptyPhrase);
        if (m_fepadre)
        {
            table.AddCell(new Phrase(m_stringFactory.GetString("SerieRecibo") + "GENERAL 1/1", pFont));
        }
        else
        {
            table.AddCell(new Phrase(m_stringFactory.GetString("SerieRecibo") + dataFE.ReceiptSeries, pFont));
        }
        table.AddCell(new Phrase(m_stringFactory.GetString("FechaExpedicion") + issueDateStr, pFont));

        table.WriteSelectedRows(0, 5, xpos, ypos, template);
    }

    private void FillInvoice()
    {
        //todo fill data 
        PdfContentByte cb = m_writer.DirectContent;
        Document doc = cb.PdfDocument;
        doc.NewPage();
        AddTemplates(cb);

        float cheight = m_top;
        cheight -= m_diffText * 9.5f;
        policy p = m_data.policyData;
        var dataFE = GetDataFromFE();
        string name = p.insuredPerson1.firstname
            + (string.IsNullOrEmpty(p.insuredPerson1.lastName) ? string.Empty : " " + p.insuredPerson1.lastName)
            + (string.IsNullOrEmpty(p.insuredPerson1.lastName2) ? string.Empty : " " + p.insuredPerson1.lastName2);
        name = name.ToUpper();
        string rfc = string.Empty;
        if (!string.IsNullOrEmpty(p.insuredPerson1.rfc))
            rfc = p.insuredPerson1.rfc;

        cb.DrawText(m_stringFactory.GetString("Insured"), m_font, m_textSize, m_left + 10.0f, cheight);
        cb.DrawText(name, m_boldFont, m_textSize, m_left + 50.0f, cheight);
        cb.DrawText("R.F.C.: ", m_font, m_textSize, m_right - 95.0f, cheight);
        cb.DrawText(rfc, m_boldFont, m_textSize, m_right - 70.0f, cheight);
        cheight -= m_boxDiffText;

        string direccion = string.Empty, zip = string.Empty;
        insuredPersonHouse hData = p.insuredPersonHouse1;
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


        cb.DrawText(
            m_stringFactory.GetString("Address"),
            m_font,
            m_textSize,
            m_left + 10.0f,
            cheight
        );
        cb.DrawText(
            direccion,
            m_boldFont,
            m_textSize - mSize,
            m_left + 43.0f,
            cheight
        );
        cb.DrawText(m_stringFactory.GetString("Zip"), m_font, m_textSize, m_right - 95.0f, cheight);
        cb.DrawText(zip, m_boldFont, m_textSize, m_right - 75f, cheight);
        cheight -= m_boxDiffText;
        cb.DrawText(m_stringFactory.GetString("Phone"), m_font, m_textSize, m_right - 95.0f, cheight);
        cb.DrawText(p.insuredPerson1.phone, m_boldFont, m_textSize, m_right - 75.0f, cheight);
        cheight -= m_boxDiffText * 1.5f + 3.0f;
        cb.DrawTextAligned(PdfContentByte.ALIGN_RIGHT, m_stringFactory.GetString("InsuranceOf"), m_font, m_textSize, m_left + 40.0f, cheight);
        cb.DrawTextAligned(PdfContentByte.ALIGN_LEFT, "SEGURO CASA HABITACIÓN \"HOGAR", m_boldFont, m_textSize, m_left + 40.0f, cheight);
        float firstRightBoxSize = m_right - m_left - 3.0f - m_middleLeftSize;
        WritePaymentInformation(p, cb, m_left + m_middleLeftSize + 8.0f, cheight);

        cheight -= m_boxDiffText;
        cb.DrawTextAligned(PdfContentByte.ALIGN_LEFT, "BIEN SEGURO\"", m_boldFont, m_textSize, m_left + 40.0f, cheight);
        cheight -= m_boxDiffText;
        cb.DrawText(m_stringFactory.GetString("LblPolicyNo") + p.policyNo, m_font, m_textSize, m_left + 10.0f, cheight);

        FillAgentAndDate(p, cb, m_left + 10, cheight);

        cheight -= m_boxDiffText * 10;

        // cb.DrawLine(m_left + m_middleLeftSize + 58, cheight + m_boxDiffText * 0.5f, m_right - 7, cheight + m_boxDiffText * 0.5f);

        decimal total = 0;
        if (p.cantidadDePagos > 1)
        {
            decimal feeRX = p.fee;
            decimal premiumRX = p.netPremium / p.cantidadDePagos;
            decimal rxpf = premiumRX * p.porcentajeRecargoPagoFraccionado;
            decimal tax = (feeRX + premiumRX + rxpf) * p.taxP;
            total = ApplicationConstants.CalculateTotalWithData(premiumRX, tax, rxpf, feeRX);
        }
        else
        {
            total = ApplicationConstants.CalculateTotal(p.netPremium, p.taxP, p.porcentajeRecargoPagoFraccionado, p.currency, p.fee);
        }

        cb.DrawText("Importe con letra: ", m_font, m_textSize, m_left + m_middleLeftSize + 8.0f, cheight);
        cheight -= m_boxDiffText;
        string currencyAppend = "M.N.";
        string units = "pesos";
        if (p.currency == ApplicationConstants.DOLLARS_CURRENCY)
        {
            units = "dólares";
            currencyAppend = string.Empty;
        }
        string[] words = ("-" + total.ToSpanishWrittenString(units) + " " + currencyAppend + "-").ToUpper().Split(' ');
        bool lineAdded = false;
        int chars = 0;
        string[] linesTotal = new string[2] { string.Empty, string.Empty };
        foreach (string word in words)
        {
            if (!lineAdded && chars + word.Length < 60)
            {
                linesTotal[0] += word + " ";
                chars += word.Length;
            }
            else
            {
                lineAdded = true;
                linesTotal[1] += word + " ";
            }
        }
        cb.DrawText(linesTotal[0], m_font, m_textSize, m_left + m_middleLeftSize + 8.0f, cheight);
        cheight -= m_boxDiffText;
        cb.DrawText(linesTotal[1], m_font, m_textSize, m_left + m_middleLeftSize + 8.0f, cheight);
        cheight -= m_boxDiffText;
        cb.DrawText("RECIBO DE PAGO DE PRIMAS SERIE " + dataFE.Series.ToUpper() + " No. " + dataFE.MapfreInternalNumber, m_font, m_textSize, m_left + m_middleLeftSize + 8.0f, cheight);
        cheight -= m_boxDiffText;
        cb.DrawText("Expedido en: " + dataFE.IssuedIn.ToUpper(), m_font, m_textSize, m_left + m_middleLeftSize + 8.0f, cheight);
        cheight -= m_boxDiffText;
        cheight -= m_boxDiffText;
        cb.DrawText("Registro: " + dataFE.Registry, m_font, m_textSize, m_left + m_middleLeftSize + 8.0f, cheight);
        cheight -= m_boxDiffText;
        cheight -= m_boxDiffText;
        cheight -= m_boxDiffText;
        cheight -= m_boxDiffText;
        //cb.DrawText("En las tiendas OXXO solo se reciben pagos en efectivo.", m_font, m_textSize, m_left + m_middleLeftSize + 8.0f, cheight);

    }

    private void AddTemplates(PdfContentByte cb)
    {
        cb.AddTemplate(m_template, 0, 0);
        if (m_under != null)
        {
            m_writer.DirectContentUnder.AddTemplate(m_under, 0, 0);
        }
    }
}
