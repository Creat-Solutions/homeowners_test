using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp;
using ME.Admon;


public interface IQuoteCalc
{
    byte[] GetGeneratedPDF();
}

public class QuoteCalculo : IQuoteCalc
{
    private Font Helvetica = FontFactory.GetFont(FontFactory.HELVETICA, 8),
             HelveticaBold = FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD),
             HelveticaBold12Black = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, BaseColor.BLACK),
             HelveticaWhite = FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.WHITE),
             HelveticaSmall = FontFactory.GetFont(FontFactory.HELVETICA, 6),
             HelveticaSmallBold = FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.BOLD);
    private MemoryStream m_stream = null;

    public QuoteCalculo(int quoteid)
    {
        MemoryStream memData = new MemoryStream();
        Document document = new Document(PageSize.LETTER);
        PdfWriter writer = PdfWriter.GetInstance(document, memData);
        document.AddTitle("Cotizacion Calculo " + quoteid);
        document.AddAuthor("Macafee and Edwards");
        document.Open();
        GenerateDocument(document, quoteid);
        document.Close();
        m_stream = memData;
    }

    public byte[] GetGeneratedPDF()
    {
        if (m_stream == null) {
            return null;
        }
        return m_stream.ToArray();
    }

    private PdfPCell GenerateDefaultCell(string data)
    {
        PdfPCell cell = string.IsNullOrEmpty(data) ? new PdfPCell() : new PdfPCell(new Phrase(data, Helvetica));
        cell.BorderWidth = 0;
        cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
        return cell;
    }

    private PdfPCell GenerateDefaultCell()
    {
        return GenerateDefaultCell(string.Empty);
    }

    private string CoveredBoolToString(bool data)
    {
        return data ? "COVERED / CUBIERTO" : "EXCLUDED / EXCLUÍDO";
    }

    private PdfPTable GenerateTableForData()
    {
        PdfPTable table = new PdfPTable(new float[] { 35, 65 });
        MakeDefaultCellForTable(table);
        return table;
    }

    private PdfPCell GenerateLabelForDataCell(string label)
    {
        PdfPCell cell = GenerateDefaultCell(label);
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        return cell;
    }

    private PdfPCell GenerateDataCell(string data)
    {
        PdfPCell cell = GenerateDefaultCell(data);
        return cell;
    }

    private PdfPCell AseguradoViviendaTable(string typeOfClient, string typeOfProperty, string useOfProperty, string distanceSea, string adjoining)
    {
        PdfPCell cell = GenerateDefaultCell();
        cell.AddElement(new Paragraph("DEL ASEGURADO", HelveticaBold));

        PdfPTable table = GenerateTableForData();
        table.AddCell(GenerateLabelForDataCell("Propietario: "));
        table.AddCell(GenerateDataCell(typeOfClient));
        cell.AddElement(table);

        cell.AddElement(new Paragraph("DE LA VIVIENDA", HelveticaBold));
        table = GenerateTableForData();
        table.AddCell(GenerateLabelForDataCell("Tipo de vivienda: "));
        table.AddCell(GenerateDataCell(typeOfProperty));
        table.AddCell(GenerateLabelForDataCell("Uso: "));
        table.AddCell(GenerateDataCell(useOfProperty));
        table.AddCell(GenerateLabelForDataCell("Distancia al mar: "));
        table.AddCell(GenerateDataCell(distanceSea));
        table.AddCell(string.Empty);
        table.AddCell(GenerateDataCell(adjoining));
        cell.AddElement(table);

        return cell;
    }

    private string BoolString(bool data)
    {
        return data.ToString().ToUpper();
    }

    private void GenerateBoolCoveredCell(PdfPTable table, string title, bool data)
    {
        table.AddCell(GenerateLabelForDataCell(title));
        table.AddCell(GenerateDataCell(BoolString(data)));
    }

    private PdfPCell ConstruccionSeguridadRoboTable(string roofType, string wallType, bool guards24, bool centralAlarm, bool localAlarm, bool tvCircuit, bool tvGuard)
    {
        PdfPCell cell = GenerateDefaultCell();
        cell.AddElement(new Paragraph("DE LA CONSTRUCCIÓN", HelveticaBold));

        PdfPTable table = GenerateTableForData();
        table.AddCell(GenerateLabelForDataCell("Construcción techo: "));
        table.AddCell(GenerateDataCell(roofType));
        table.AddCell(GenerateLabelForDataCell("Construcción muro: "));
        table.AddCell(GenerateDataCell(wallType));
        cell.AddElement(table);

        cell.AddElement(new Paragraph("SEGURIDAD ROBO", HelveticaBold));
        table = GenerateTableForData();
        GenerateBoolCoveredCell(table, "Guardias: ", guards24);
        GenerateBoolCoveredCell(table, "Alarma central: ", centralAlarm);
        GenerateBoolCoveredCell(table, "Alarma local: ", localAlarm);
        GenerateBoolCoveredCell(table, "Circuito de TV: ", tvCircuit);
        GenerateBoolCoveredCell(table, "Video portero: ", tvGuard);
        cell.AddElement(table);

        return cell;
    }

    private void MakeDefaultCellForTable(PdfPTable table)
    {
        table.DefaultCell.BorderWidth = 0;
        table.DefaultCell.VerticalAlignment = Element.ALIGN_TOP;
    }

    private PdfPTable PrimasTable(decimal netPremium, decimal fee, decimal tax, decimal rxpf, decimal rxpfP, int currency)
    {
        PdfPTable table = new PdfPTable(3);
        MakeDefaultCellForTable(table);
        table.AddCell("");
        table.AddCell(GenerateDataCell("Prima Neta: "));
        table.AddCell(GenerateLabelForDataCell(netPremium.ToString("C")));

        table.AddCell("");
        table.AddCell(GenerateDataCell("Derechos: "));
        table.AddCell(GenerateLabelForDataCell(ApplicationConstants.Fee(currency, fee).ToString("C")));

        table.AddCell(GenerateDataCell(rxpfP.ToString("P")));
        table.AddCell(GenerateDataCell("RXPF: "));
        table.AddCell(GenerateLabelForDataCell(rxpf.ToString("C")));

        table.AddCell(GenerateDataCell(tax.ToString("P")));
        table.AddCell(GenerateDataCell("Tax: "));
        table.AddCell(GenerateLabelForDataCell(ApplicationConstants.CalculateTax(netPremium, tax, rxpfP, currency, fee).ToString("C")));

        table.AddCell("");
        table.AddCell(GenerateDataCell("TOTAL: "));
        table.AddCell(GenerateLabelForDataCell(ApplicationConstants.CalculateTotal(netPremium, tax, rxpfP, currency, fee).ToString("C")));

        return table;
    }

    private PdfPTable ComisionTable(decimal netPremium, decimal comision)
    {
        decimal comP = comision / netPremium;
        PdfPTable table = new PdfPTable(1);
        MakeDefaultCellForTable(table);
        table.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        table.AddCell(GenerateLabelForDataCell(comision.ToString("C")));
        table.AddCell(GenerateLabelForDataCell(comP.ToString("P")));
        return table;
    }

    private PdfPCell GenerateQuoteDetailHeaderCell(string label)
    {
        PdfPCell header = new PdfPCell(new Phrase(label, HelveticaWhite));
        header.HorizontalAlignment = Element.ALIGN_CENTER;
        header.BorderWidth = 0.1f;
        header.BackgroundColor = new BaseColor(0xB9, 0x09, 0x0B);
        return header;
    }

    private PdfPTable GenerateQuoteDetailHeader()
    {
        PdfPTable table = new PdfPTable(new float[] {7, 22, 17, 7, 7, 7, 7, 7, 5, 7, 7});
        table.WidthPercentage = 100f;
        table.DefaultCell.BorderWidth = 0.1f;
        table.AddCell(GenerateQuoteDetailHeaderCell("Cod"));
        table.AddCell(GenerateQuoteDetailHeaderCell("Cobertura"));
        table.AddCell(GenerateQuoteDetailHeaderCell("Monto"));
        table.AddCell(GenerateQuoteDetailHeaderCell("Tasa"));
        table.AddCell(GenerateQuoteDetailHeaderCell("Rec"));
        table.AddCell(GenerateQuoteDetailHeaderCell("Des"));
        table.AddCell(GenerateQuoteDetailHeaderCell("T. Final"));
        table.AddCell(GenerateQuoteDetailHeaderCell("Prima"));
        table.AddCell(GenerateQuoteDetailHeaderCell("Fac"));
        table.AddCell(GenerateQuoteDetailHeaderCell("%"));
        table.AddCell(GenerateQuoteDetailHeaderCell("Abs"));
        return table;
    }

    private PdfPCell GenerateQuoteDetailDataCell(string data, int alignment)
    {
        PdfPCell cell = new PdfPCell(new Phrase(data, Helvetica));
        cell.BorderWidth = 0.1f;
        cell.HorizontalAlignment = alignment;
        return cell;
    }

    private PdfPCell GenerateQuoteDetailDataCell(string data)
    {
        return GenerateQuoteDetailDataCell(data, Element.ALIGN_LEFT);
    }

    private bool edificios = true;
    private bool contenidos = true;
    private bool perdidaRentas = true;
    private bool gastosExtras = true;
    private bool remocionEscombros = true;
    private bool robo = true;
    private PdfPCell GenerateQuoteDetailSpecialHeader(int codigo)
    {
        PdfPCell cell = null;
        if (codigo > 3000 && edificios) {
            edificios = false;
            cell = GenerateQuoteDetailDataCell("EDIFICIO");
            cell.Colspan = 11;
        } else if (codigo > 3050 && contenidos) {
            contenidos = false;
            cell = GenerateQuoteDetailDataCell("CONTENIDOS");
            cell.Colspan = 11;
        } else if (codigo > 3210 && remocionEscombros) {
            remocionEscombros = false;
            cell = GenerateQuoteDetailDataCell("REMOCIÓN ESCOMBROS");
            cell.Colspan = 11;
        } else if (codigo > 3220 && gastosExtras) {
            gastosExtras = false;
            cell = GenerateQuoteDetailDataCell("GASTOS EXTRA CASA HABITACIÓN");
            cell.Colspan = 11;
        } else if (codigo > 3230 && perdidaRentas) {
            perdidaRentas = false;
            cell = GenerateQuoteDetailDataCell("PÉRDIDA DE RENTAS");
            cell.Colspan = 11;
        } else if (codigo > 3500 && robo) {
            robo = false;
            cell = GenerateQuoteDetailDataCell("ROBO DE MENAJE");
            cell.Colspan = 11;
        }
        return cell;
    }

    private string GenerateQuoteDetailMonto(HouseInsuranceDataContext db, int quoteid, int codigo, decimal amount)
    {
        switch (codigo) {
            case 3910:
            case 3911:
            case 3920:
            case 3930:
                bool covered = db.quotes.Where(m => m.id == quoteid).Select(m => m.familyAssistance).Single();
                return covered ? "COVERED/CUBIERTO" : "EXCLUDED/EXCLUÍDO";
            case 3940:
            case 3941:
                return "EXCLUDED/EXCLUÍDO";
            default:
                return amount > 0M ? amount.ToString("C2") : "EXCLUDED/EXCLUÍDO";
        }
    }

    private PdfPTable GenerateQuoteDetailTable(int quoteid)
    {
        PdfPTable table = GenerateQuoteDetailHeader();
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        var data = db.quoteDetails.Where(m => m.quote == quoteid);
        foreach (quoteDetail detail in data) {
            PdfPCell specialCell = GenerateQuoteDetailSpecialHeader(detail.codigoCobertura);
            if (specialCell != null) {
                table.AddCell(specialCell);
            }
            table.AddCell(GenerateQuoteDetailDataCell(detail.codigoCobertura.ToString()));
            table.AddCell(GenerateQuoteDetailDataCell(detail.descripcionCobertura));
            table.AddCell(GenerateQuoteDetailDataCell(
                GenerateQuoteDetailMonto(
                    db,
                    quoteid,
                    detail.codigoCobertura, 
                    detail.monto.Value
                ),
                Element.ALIGN_RIGHT
            ));
            table.AddCell(GenerateQuoteDetailDataCell(detail.tasa.ToString()));
            table.AddCell(GenerateQuoteDetailDataCell(detail.rec.ToString()));
            table.AddCell(GenerateQuoteDetailDataCell(detail.des.ToString()));
            table.AddCell(GenerateQuoteDetailDataCell(detail.tasaFinal.ToString()));
            table.AddCell(GenerateQuoteDetailDataCell(detail.prima.Value.ToString("C"), Element.ALIGN_RIGHT));
            table.AddCell(GenerateQuoteDetailDataCell(detail.fac.ToString()));
            table.AddCell(GenerateQuoteDetailDataCell(detail.porcentaje.ToString()));
            table.AddCell(GenerateQuoteDetailDataCell(detail.comision.Value.ToString("C"), Element.ALIGN_RIGHT));
        }
        return table;
    }

    private void GenerateDocument(Document doc, int quoteid)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        
        var dquote = db.quotes.Where(m => m.id == quoteid).Single();
        HouseEstate he = dquote.insuredPersonHouse1.HouseEstate;
        var data = db.spList_Calcula(quoteid, he.City.HMP).Single();
        bool HMP = (bool)dquote.HMP;
        bool quake = (bool)dquote.quake;
        int currency = dquote.currency;

        decimal netPremium = data.netPremium ?? 0;
        decimal comision = data.comision ?? 0;
        decimal fee = dquote.fee.Value;
        decimal tax = db.fGet_QuoteTaxPercentage(quoteid).Value;
        PdfPTable table = new PdfPTable(1);
        table.WidthPercentage = 100f;
        PdfPCell title = new PdfPCell(new Phrase("Cotización " + quoteid, HelveticaBold12Black));
        title.HorizontalAlignment = Element.ALIGN_CENTER;
        title.BorderWidth = 0;
        table.AddCell(title);
        doc.Add(table);
        doc.Add(new Paragraph());
        Paragraph p = new Paragraph();
        p.Font = Helvetica;
        p.Add(new Chunk("Nombre: " + data.name + " " + (data.lastName ?? string.Empty) + "\n"));
        p.Add(new Chunk("Descuento: " + dquote.descuento.ToString("P") + "\n"));
        p.Add(new Chunk("Moneda: " + (dquote.currency == ApplicationConstants.PESOS_CURRENCY ? "Pesos" : "Dólares") + "\n"));
        doc.Add(p);

        p = new Paragraph();
        p.Font = Helvetica;
        p.Add(new Chunk("Zona FHM: " + he.City.HMP + "\n"));
        p.Add(new Chunk("Zona Robo: " + he.City.State.TheftZone + "\n"));
        p.Add(new Chunk("Zona TEV: " + he.TEV + "\n"));
        doc.Add(p);
        table = new PdfPTable(new float[] { 50f, 50f });
        table.WidthPercentage = 100;
        MakeDefaultCellForTable(table);
        table.AddCell(AseguradoViviendaTable(
            data.typeOfClient, 
            data.typeOfProperty, 
            data.useOfProperty, 
            data.distanceFromSea,
            (bool)data.adjoiningEstates ? "Con colindantes sin fincar" : "Sin colindantes sin fincar"
        ));
        table.AddCell(ConstruccionSeguridadRoboTable(
            data.roofType,
            data.wallType,
            (bool)data.guards24,
            (bool)data.centralAlarm,
            (bool)data.localAlarm,
            (bool)data.tvCircuit,
            (bool)data.tvGuard
        ));
            
        doc.Add(table);
        doc.Add(new Paragraph());

        table = new PdfPTable(new float[] { 50, 50 });
        table.HorizontalAlignment = Element.ALIGN_LEFT;
        table.WidthPercentage = 50;
        MakeDefaultCellForTable(table);
        table.AddCell(GenerateLabelForDataCell("Earthquake / Terremoto: "));
        table.AddCell(GenerateDataCell(CoveredBoolToString(HMP)));

        table.AddCell(GenerateLabelForDataCell("HMP / FHM: "));
        table.AddCell(GenerateDataCell(CoveredBoolToString(quake)));
        doc.Add(table);

        //tabla principal
        doc.Add(GenerateQuoteDetailTable(quoteid));

        //tabla de totales
        table = new PdfPTable(new float[] { 60, 40 });
        table.WidthPercentage = 40;
        MakeDefaultCellForTable(table);

        PdfPCell cell = GenerateDefaultCell("PRIMA");
        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        table.AddCell(cell);
        cell = GenerateDefaultCell("COMISIÓN");
        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        table.AddCell(cell);

        table.AddCell(PrimasTable(netPremium, fee, tax, dquote.recargoPagoFraccionado, dquote.porcentajeRecargoPagoFraccionado, currency));
        table.AddCell(ComisionTable(netPremium, comision));

        doc.Add(table);
    }
}
