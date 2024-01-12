<%@ WebHandler Language="C#" Class="CotizaPDF" %>

using System;
using System.Web;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp;


public class CotizaPDF : IHttpHandler {    

    private static string DeduciblesHelper(decimal? value)
    {
        if (value == null || value == 0M)
            return "Excluded";
        decimal v = (decimal)value;
        v *= 100M;
        return v.ToString("F") + "%";

    }

    private static string BoolToString(bool value)
    {
        return value ? "TRUE / VERDADERO" : "FALSE / FALSO";
    }

    private static IElement GetSectionTitle(string title)
    {
        Font HelveticaBold12White = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, BaseColor.WHITE);
        PdfPTable headerRow = new PdfPTable(1);
        headerRow.WidthPercentage = 100;
        headerRow.DefaultCell.BorderWidth = 0;
        headerRow.DefaultCell.BackgroundColor = new BaseColor(185, 9, 11);
        headerRow.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
        headerRow.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
        headerRow.AddCell(new Phrase(title, HelveticaBold12White));
        return headerRow;
    }

    public static void CreatePDF(int quote, string filename)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        var cotiza = db.spList_Cotiza(quote).Single();
        var dquote = db.quotes.Where(m => m.id == quote).Single();
        decimal comisionP = (decimal)cotiza.comision / (decimal)cotiza.NetPremium;
        ME.Admon.HouseEstate houseEstate = new ME.Admon.HouseEstate(dquote.insuredPersonHouse1.houseEstate);
        var csegtable = db.spGenerate_DeduciblesCoaseguros(quote, null, houseEstate.TEV, houseEstate.City.HMP).Single();
        BaseColor RED = new BaseColor(0xB9, 0x09, 0x0B);
        Font Helvetica = FontFactory.GetFont(FontFactory.HELVETICA, 10),
             HelveticaRed = FontFactory.GetFont(FontFactory.HELVETICA, 10, RED),
             HelveticaBoldRed = FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD, RED),
             HelveticaBold12Red = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, RED),
             HelveticaBold = FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD),
             HelveticaBold12Black = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, BaseColor.BLACK),
             HelveticaBold12Underlined = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD | Font.UNDERLINE),
             HelveticaSmall = FontFactory.GetFont(FontFactory.HELVETICA, 8),
             HelveticaSmallBold = FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD);
        
        FileStream stream = new FileStream(filename, FileMode.Truncate, FileAccess.Write);
        Document document = new Document(PageSize.LETTER);
        PdfWriter.GetInstance(document, stream);
        document.AddTitle("Homeowners quote: " + quote);
        document.AddAuthor("Macafee and Edwards");
        document.Open();

        PdfPTable table = new PdfPTable(new float[] { 25, 50, 25 });
        table.WidthPercentage = 100;
        table.DefaultCell.BorderWidth = 0;
        table.DefaultCell.VerticalAlignment = Element.ALIGN_TOP;
        Image meLogo = Image.GetInstance(HttpContext.Current.Server.MapPath("~/images/logoME2.jpg"));
        meLogo.ScaleAbsolute(100, 77.87f);
        meLogo.Alignment = Element.ALIGN_CENTER;
        PdfPCell cell = new PdfPCell();
        cell.BorderWidth = 0;
        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        cell.VerticalAlignment = Element.ALIGN_TOP;
        cell.AddElement(meLogo);
        table.AddCell(cell);
        cell = new PdfPCell();
        cell.BorderWidth = 0;
        cell.BorderWidthLeft = 0.1f;
        cell.BorderWidthRight = 0.1f;
        cell.VerticalAlignment = Element.ALIGN_TOP;
        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        Paragraph title = new Paragraph();
        title.Alignment = Element.ALIGN_CENTER;
        title.Add(new Chunk("MacAfee & Edwards, Inc.\n", HelveticaBold12Black));
        title.Add(new Chunk("Mexican Insurance Specialist\n", HelveticaBold));
        title.Add(new Chunk("HOMEOWNERS QUOTE", HelveticaBold12Red));
        cell.AddElement(title);
        table.AddCell(cell);
        table.AddCell(new Paragraph("700 S. Flower Street #1216\nLos Angeles, CA 90017\nP. (213) 629-9777\nF. (213) 629-9779\n", HelveticaSmall));
        document.Add(table);
        document.Add(new Paragraph("\n"));
        
        Chunk p;
        document.Add(GetSectionTitle("- GENERAL INFORMATION -"));
        
        table = new PdfPTable(2);
        table.DefaultCell.BorderWidth = 0;
        cell = new PdfPCell(new Phrase("Named Insured: ", HelveticaBold));
        cell.BorderWidth = 0;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        table.AddCell(cell);

        table.AddCell(new Phrase(cotiza.firstname + " " + cotiza.lastName, HelveticaBoldRed));

        cell = new PdfPCell(new Phrase("Loss payee (Beneficiario preferente): ", HelveticaBold));
        cell.BorderWidth = 0;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        table.AddCell(cell);

        table.AddCell(new Phrase(cotiza.bankName, HelveticaBoldRed));

        cell = new PdfPCell(new Phrase("Loan number (Número de crédito): ", HelveticaBold));
        cell.BorderWidth = 0;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        table.AddCell(cell);

        table.AddCell(new Phrase(cotiza.loanNumber, HelveticaBoldRed));

        cell = new PdfPCell(new Phrase("Address: ", HelveticaBold));
        cell.BorderWidth = 0;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        table.AddCell(cell);

        table.AddCell(new Phrase(cotiza.street + ", " + cotiza.exteriorNo +
                     (cotiza.InteriorNo == null ? "" : "- " + cotiza.InteriorNo), 
                     HelveticaRed));

        table.AddCell(string.Empty);
        
        table.AddCell(new Phrase(houseEstate.Name + ", " + houseEstate.City.Name + ", " + houseEstate.City.State.Name + ", " + houseEstate.ZipCode,
                      HelveticaRed));
        table.AddCell(string.Empty);
        table.AddCell(new Phrase("MEXICO", FontFactory.GetFont(FontFactory.HELVETICA, 10, RED)));

        cell = new PdfPCell(new Phrase("Jurisdiction (Jurisdicción): ", HelveticaBold));
        cell.BorderWidth = 0;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        table.AddCell(cell);

        table.AddCell(new Phrase("Only in the Republic of Mexico (México únicamente)", HelveticaBoldRed));
        document.Add(table);

        p = new Chunk("QUOTE BASE ON THE FOLLOWING PARAMETERS (COTIZACION DE ACUERDO A LOS SIGUIENTES PARÁMETROS):", Helvetica);
        table = new PdfPTable(2);
        table.DefaultCell.BorderWidth = 0;
        cell = new PdfPCell(new Phrase("Type of Property (Tipo de Propiedad): ", HelveticaBold));
        cell.BorderWidth = 0;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        table.AddCell(cell);
        table.AddCell(new Phrase(cotiza.typeOfProperty, HelveticaBoldRed));

        cell = new PdfPCell(new Phrase("Use (Uso): ", HelveticaBold));
        cell.BorderWidth = 0;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        table.AddCell(cell);
        table.AddCell(new Phrase(cotiza.useOfProperty, HelveticaBoldRed));

        cell = new PdfPCell(new Phrase("Beach Front (Distancia al mar): ", HelveticaBold));
        cell.BorderWidth = 0;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        table.AddCell(cell);
        table.AddCell(new Phrase(cotiza.distanceFromSea + " / " + cotiza.distanciaAlMar, HelveticaBoldRed));

        cell = new PdfPCell(new Phrase("Adjacent Structures (Colindantes sin fincar): ", HelveticaBold));
        cell.BorderWidth = 0;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        table.AddCell(cell);
        table.AddCell(new Phrase(BoolToString(cotiza.adjoiningEstates ?? false), HelveticaBoldRed));
        
        document.Add(table);
        
        document.Add(GetSectionTitle("- PROPERTY COVERAGES (BIENES CUBIERTOS) - "));

        p = new Chunk("I. LIMITS (LÍMITES)", HelveticaBold12Underlined);
        document.Add(p);

        table = new PdfPTable(2);
        table.DefaultCell.BorderWidth = 0;

        cell = new PdfPCell(new Phrase("Currency (Moneda): ", HelveticaBold));
        cell.BorderWidth = 0;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        table.AddCell(cell);

        table.AddCell(new Phrase(cotiza.currency == ApplicationConstants.PESOS_CURRENCY ? "PESOS" : "U.S. DOLLARS / DÓLARES", HelveticaBoldRed));
        
        cell = new PdfPCell(new Phrase("Building (Edificio): ", HelveticaBold));
        cell.BorderWidth = 0;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        table.AddCell(cell);

        table.AddCell(new Phrase(cotiza.buildingLimit > 0.0M ? cotiza.buildingLimit.ToString("C") : "EXCLUDED / EXCLUÍDO", HelveticaBoldRed));

        cell = new PdfPCell(new Phrase("Contents (contents): ", HelveticaBold));
        cell.BorderWidth = 0;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        table.AddCell(cell);

        table.AddCell(new Phrase(cotiza.contentsLimit > 0.0M ? cotiza.contentsLimit.ToString("C") : "EXCLUDED / EXCLUÍDO", HelveticaBoldRed));

        cell = new PdfPCell(new Phrase("Outdoor Property (Bienes a la intemperie): ", HelveticaBold));
        cell.BorderWidth = 0;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        table.AddCell(cell);

        table.AddCell(new Phrase(cotiza.outdoorLimit > 0.0M ? cotiza.outdoorLimit.ToString("C") : "EXCLUDED / EXCLUÍDO", HelveticaBoldRed));

        cell = new PdfPCell(new Phrase("Debri Removal (Rem. escombros): ", HelveticaBold));
        cell.BorderWidth = 0;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        table.AddCell(cell);

        table.AddCell(new Phrase(cotiza.debriLimit > 0.0M ? cotiza.debriLimit.ToString("C") : "EXCLUDED / EXCLUÍDO", HelveticaBoldRed));

        cell = new PdfPCell(new Phrase("Extra expenses (Gastos extras): ", HelveticaBold));
        cell.BorderWidth = 0;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        table.AddCell(cell);

        table.AddCell(new Phrase(cotiza.extraLimit > 0.0M ? cotiza.extraLimit.ToString("C") : "EXCLUDED / EXCLUÍDO", HelveticaBoldRed));

        cell = new PdfPCell(new Phrase("Loss Of Rent - 4 months (Perdida de Rentas - 4 meses): ", HelveticaBold));
        cell.BorderWidth = 0;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        table.AddCell(cell);

        table.AddCell(new Phrase(cotiza.lossOfRentsLimit > 0.0M ? cotiza.lossOfRentsLimit.ToString("C") : "EXCLUDED / EXCLUÍDO", HelveticaBoldRed));

        document.Add(table);


        p = new Chunk("Outdoor Property covered (Bienes a la Intemperie cubiertos): Antennas/Satellites, Pools, Awnings, Palapas, Gardens & Decorative Constructions, Streets, Patios, Decks, Roads, Outdoor Furniture, Docks, Buildings lacking roofs, windows or walls, Electric Substations, Sporting Installations, Signs, etc. - This type of Property may be covered by specifically scheduling (with it's respective value) each individual item to be covered.\n", HelveticaSmall);
        document.Add(p);

        p = new Chunk("-IF IT'S NOT SCHEDULED, IT'S NOT COVERED-.\n", HelveticaSmallBold);
        document.Add(p);

        p = new Chunk("II. COVERAGES & DEDUCTIBLES (COBERTURAS Y DEDUCIBLES)", HelveticaBold12Underlined);
        document.Add(p);

        table = new PdfPTable(2);
        table.DefaultCell.BorderWidth = 0;
        
        cell = new PdfPCell(new Phrase("Fire (Incendio): ", Helvetica));
        cell.BorderWidth = 0;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        table.AddCell(cell);

        table.AddCell(new Phrase("All risk Mexican Form (Todo riesgo forma Mexicana)", Helvetica));

        cell = new PdfPCell(new Phrase("Earthquake (Terremoto): ", Helvetica));
        cell.BorderWidth = 0;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        table.AddCell(cell);

        table.AddCell(new Phrase((bool)cotiza.quake ? "Covered/Cubierto" : "Excluded/Excluído", HelveticaBoldRed));

        cell = new PdfPCell(new Phrase("HMP (FHM): ", Helvetica));
        cell.BorderWidth = 0;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        table.AddCell(cell);

        table.AddCell(new Phrase((bool)cotiza.HMP ? "Covered/Cubierto" : "Excluded/Excluído", HelveticaBoldRed));

        document.Add(table);

        p = new Chunk("EARTHQUAKE AND HMP ARE EXCLUDED FOR LOSS OF RENT (TERREMOTO Y FHM ESTAN EXCLUIDOS PARA LA COBERTURA DE PERDIDA DE RENTAS)\n", HelveticaSmallBold);
        document.Add(p);

        p = new Chunk("HMP: (Hydro-Meteorological Phenomenon includes the following perils, Mudslide, hail, frost, hurricane, flood, flood by rain, wave wash/Tidal, waves, hail) FHM (Fenómenos Hidro-Meteorológicos incluye los siguientes riesgos: avalancha de lodo, granizo, helado, huracán, Inundación, Inundación por lluvia, marejada, golpe de mar, Nevada y vientos tempestuosos)\n", HelveticaSmall);
        document.Add(p);

        table = new PdfPTable(3);
        table.DefaultCell.BorderWidth = 0;
        table.AddCell(string.Empty);
        cell = new PdfPCell(new Phrase("Deductible/Deducible"));
        cell.BorderWidth = 0.1f;
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase("Copay/Co-aseguro"));
        cell.BorderWidth = 0.1f;
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase("Fire (Incendio):"));
        cell.BorderWidth = 0.1f;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase("None"));
        cell.BorderWidth = 0.1f;
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase("None"));
        cell.BorderWidth = 0.1f;
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase("Earthquake (Terremoto):"));
        cell.BorderWidth = 0.1f;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase(DeduciblesHelper(csegtable.deducibleterremoto)));
        cell.BorderWidth = 0.1f;
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase(DeduciblesHelper(csegtable.coaseguroTerremoto)));
        cell.BorderWidth = 0.1f;
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase("*HMP (FHM):"));
        cell.BorderWidth = 0.1f;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase(DeduciblesHelper(csegtable.deducibleFHM)));
        cell.BorderWidth = 0.1f;
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase(DeduciblesHelper(csegtable.coaseguroFHM)));
        cell.BorderWidth = 0.1f;
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase("Outdoor Property (Bienes a la intemperie):"));
        cell.BorderWidth = 0.1f;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase(DeduciblesHelper(csegtable.deducibleInterperie)));
        cell.BorderWidth = 0.1f;
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase(DeduciblesHelper(csegtable.coaseguroInterperie)));
        cell.BorderWidth = 0.1f;
        table.AddCell(cell);
        document.Add(table);
        
        p = new Chunk("III. SPECIAL CLAUSES (CLAUSULAS ESPECIALES)", HelveticaBold12Underlined);
        document.Add(p);

        table = new PdfPTable(new float[] { 30, 70 });
        table.DefaultCell.BorderWidth = 0;
        
        cell = new PdfPCell(new Phrase("Valuation (Valuacion): ", Helvetica));
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        cell.BorderWidth = 0;
        cell.VerticalAlignment = Element.ALIGN_TOP;
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase("Replacement Cost for all coverage (Valor de Reposición para todas las coberturas)",
                            HelveticaBoldRed));
        cell.VerticalAlignment = Element.ALIGN_TOP;
        cell.BorderWidth = 0;
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase("Note 1 (Nota 1): ", Helvetica));
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        cell.BorderWidth = 0;
        cell.VerticalAlignment = Element.ALIGN_TOP;
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase("First risk insurance except for the Earthquake coverage in which we need at least 80% of the values. (Seguro a primer riesgo absoluto excepto para la cobertura de Terremoto para la cual necesitamos por los menos el 80% de los valores, la cual cuenta con un coaseguro del 80%).",
                            HelveticaBoldRed));
        cell.VerticalAlignment = Element.ALIGN_TOP;
        cell.BorderWidth = 0;
        table.AddCell(cell);
        
        document.Add(table);

        document.Add(GetSectionTitle("- ADDITIONAL COVERAGES (COBERTURAS ADICIONALES) - "));

        if (cotiza.CSLLimit > 0) {
            p = new Chunk("CIVIL GENERAL LIABILITY (Responsabilidad Civil General)", HelveticaBold);
            document.Add(p);
            table = new PdfPTable(2);
            table.DefaultCell.BorderWidth = 0;
            cell = new PdfPCell(new Phrase("Limit (Límite):", Helvetica));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(cotiza.CSLLimit.ToString("C") + " CSL PD/BI Occ/Agg (L.U.C.).", HelveticaBoldRed));
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Coverage (Cobertura):", Helvetica));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Homeowners Liability (RC Familiar), Wordlwide coverage (Cobertura Mundial).", HelveticaRed));
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Deductible (Deducible):", Helvetica));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("None (Ninguno).", HelveticaRed));
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Major Exclusion (Exclusión):", Helvetica));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Punitive Damages or exemplary damages (Daños Punitivos o ejemplares).", HelveticaBoldRed));
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            document.Add(table);
        }
        if (cotiza.burglaryLimit > 0  || cotiza.burglaryJewelryLimit > 0) {
            p = new Chunk("BURGLARY (Robo con violencia)", HelveticaBold);
            document.Add(p);
            table = new PdfPTable(2);
            table.DefaultCell.BorderWidth = 0;

            if (cotiza.burglaryLimit > 0) {
                cell = new PdfPCell(new Phrase("Limit (Límite):", Helvetica));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.BorderWidth = 0;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(cotiza.burglaryLimit.ToString("C") + "  Item I - General Household Contents & Electronic/Photo/Sport.", HelveticaBoldRed));
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.BorderWidth = 0;
                table.AddCell(cell);
            }

            if (cotiza.burglaryJewelryLimit > 0) {
                cell = new PdfPCell(new Phrase("Limit (Límite):", Helvetica));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.BorderWidth = 0;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(cotiza.burglaryJewelryLimit.ToString("C") + "  Item II - Art, Jewelry, Watches, Furs, Collections, Gold, & Silver items.", HelveticaBoldRed));
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.BorderWidth = 0;
                table.AddCell(cell);
            }

            cell = new PdfPCell(new Phrase("Coverage (Cobertura):", Helvetica));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Theft of contents within premises with violence and/or assault (Robo de contenidos dentro de los predios con violencia y/o asalto).", HelveticaRed));
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Deductible (Deducible):", Helvetica));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("None (Ninguno).", HelveticaRed));
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Major Exclusion (Exclusión):", Helvetica));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Mysterious Disappearance (Robo sin violencia ).", HelveticaBoldRed));
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);
            
            document.Add(table);
        }
        if (cotiza.accidentalGlassBreakageLimit > 0) {
            p = new Chunk("ACCIDENTAL GLASS BREAKAGE (Rotura de Cristales)", HelveticaBold);
            document.Add(p);
            table = new PdfPTable(2);
            table.DefaultCell.BorderWidth = 0;
            cell = new PdfPCell(new Phrase("Limit (Límite):", Helvetica));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(cotiza.accidentalGlassBreakageLimit.ToString("C"), HelveticaBoldRed));
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Coverage (Cobertura):", Helvetica));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Accidental glass breakage (Rotura de cristales).", HelveticaRed));
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Deductible (Deducible):", Helvetica));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("None (Ninguno).", HelveticaRed));
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);
            
            document.Add(table);
        }
        if (cotiza.moneySecurityLimit > 0) {
            p = new Chunk("MONEY AND SECURITIES (Dinero y valores)", HelveticaBold);
            document.Add(p);
            table = new PdfPTable(2);
            table.DefaultCell.BorderWidth = 0;
            cell = new PdfPCell(new Phrase("Limit (Límite):", Helvetica));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(cotiza.moneySecurityLimit.ToString("C"), HelveticaBoldRed));
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Coverage (Cobertura):", Helvetica));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Cash/Valuable Papers (Efectivo/Documentos Valiosos).", HelveticaRed));
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Deductible (Deducible):", Helvetica));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("None (Ninguno).", HelveticaRed));
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Major Exclusion (Exclusión):", Helvetica));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Mysterious Disappearance (Robo sin violencia ).", HelveticaBoldRed));
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);
            
            document.Add(table);
        }
        if (cotiza.electronicEquipmentLimit > 0) {
            p = new Chunk("ELECTRICAL HOUSEHOLD APPLIANCES (Equipo electrodom&eacute;stico)", HelveticaBold);
            document.Add(p);
            table = new PdfPTable(2);
            table.DefaultCell.BorderWidth = 0;
            cell = new PdfPCell(new Phrase("Limit (Límite):", Helvetica));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(cotiza.electronicEquipmentLimit.ToString("C"), HelveticaBoldRed));
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Coverage (Cobertura):", Helvetica));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Inexperience, Carelessness (Impericia, descuido), Direct action of electrical energy (Acción directa de la energía eléctrica).", HelveticaRed));
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Deductible (Deducible):", Helvetica));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Any damage over $150 US dollars (Cualquier daño superior a $150 US dólares).", HelveticaRed));
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Major Exclusion (Exclusión):", Helvetica));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Gradual wear or deterioration. Esthetic defects (Desgaste o deterioro. Defectos estéticos).", HelveticaBoldRed));
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);
            
            document.Add(table);
        }
        if (cotiza.personalItemsLimit > 0) {
            p = new Chunk("PERSONAL ITEMS (Objetos personales)", HelveticaBold);
            document.Add(p);
            table = new PdfPTable(2);
            table.DefaultCell.BorderWidth = 0;
            cell = new PdfPCell(new Phrase("Limit (Límite):", Helvetica));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(cotiza.personalItemsLimit.ToString("C"), HelveticaBoldRed));
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Sublimit (Sublímite):", Helvetica));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("$ 200 per article (por artículo).", HelveticaBoldRed));
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Coverage (Cobertura):", Helvetica));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Portable articles property of the Insured (Artículos portatiles propiedad del asegurado).", HelveticaRed));
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Deductible (Deducible):", Helvetica));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("None (Ninguno).", HelveticaRed));
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Major Exclusion (Exclusión):", Helvetica));
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Mysterious Disappearance (Robo sin violencia ).", HelveticaBoldRed));
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BorderWidth = 0;
            table.AddCell(cell);
            document.Add(table);
        }

        p = new Chunk("FAMILY ASSISTANCE (Asistencia Familiar)", HelveticaBold);
        document.Add(p);
        table = new PdfPTable(2);
        table.DefaultCell.BorderWidth = 0;
        cell = new PdfPCell(new Phrase("Limit (Límite):", Helvetica));
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        cell.BorderWidth = 0;
        cell.VerticalAlignment = Element.ALIGN_TOP;
        table.AddCell(cell);
        string famString = (cotiza.familyAssistance || cotiza.techSupport) ? "COVERED / CUBIERTO" : "EXCLUDED / EXCLUÍDO";
        cell = new PdfPCell(new Phrase(famString, HelveticaBoldRed));
        cell.VerticalAlignment = Element.ALIGN_TOP;
        cell.BorderWidth = 0;
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase("Coverage (Cobertura):", Helvetica));
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        cell.BorderWidth = 0;
        cell.VerticalAlignment = Element.ALIGN_TOP;
        cell.BorderWidth = 0;
        table.AddCell(cell);
        string coverageFA = string.Empty;
        if (cotiza.familyAssistance) {
            coverageFA = "Home assistance (Asistencia en Hogar), Legal Assistance (Asistencia Legal). Medical assistance by phone (MEDITEL). ";
        }
        
        if (cotiza.techSupport) {
            coverageFA += "Tech Support Assistance (Servicio de asistencia informática).";
        }
        cell = new PdfPCell(new Phrase(coverageFA, HelveticaRed));
        cell.VerticalAlignment = Element.ALIGN_TOP;
        cell.BorderWidth = 0;
        table.AddCell(cell);
        document.Add(table);
        
        document.Add(GetSectionTitle("- PREMIUM BREAKDOWN (DESGLOSE DE PRIMAS) - "));
        string protectionHMPQuake = "This quote includes ";
        if ((bool)cotiza.HMP && (bool)cotiza.quake) {
            protectionHMPQuake += "HMP and earthquake protection.";
        } else if ((bool)cotiza.quake) {
            protectionHMPQuake += "earthquake protection.";
        } else if ((bool)cotiza.HMP) {
            protectionHMPQuake += "HMP protection.";
        } else {
            protectionHMPQuake += "no earthquake nor HMP protection.";
        }
        document.Add(new Chunk(protectionHMPQuake, HelveticaBoldRed));
        
        decimal taxP = db.fGet_QuoteTaxPercentage(quote).Value;
        decimal fee = db.quotes.Where(x => x.id == x.id).Select(x => x.fee).Single().Value;
        decimal rxpf = dquote.recargoPagoFraccionado;
        decimal rxpfP = dquote.porcentajeRecargoPagoFraccionado;
        decimal tax = ApplicationConstants.CalculateTax((decimal)cotiza.NetPremium, taxP, rxpfP, cotiza.currency, fee);
        decimal total = ApplicationConstants.CalculateTotal((decimal)cotiza.NetPremium, taxP, rxpfP, cotiza.currency, fee);
        PdfPTable tableCover = new PdfPTable(2);
        tableCover.DefaultCell.BorderWidth = 0;
        tableCover.DefaultCell.VerticalAlignment = Element.ALIGN_TOP;
        
        table = new PdfPTable(2);

        cell = new PdfPCell(new Phrase("Number of Payments (No. de pagos):", HelveticaBold));
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        cell.VerticalAlignment = Element.ALIGN_TOP;
        cell.BorderWidth = 0;
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase(dquote.cantidadDePagos.ToString(), HelveticaBoldRed));
        cell.BorderWidth = 0;
        cell.VerticalAlignment = Element.ALIGN_TOP;
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase("Currency (Moneda):", HelveticaBold));
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        cell.VerticalAlignment = Element.ALIGN_TOP;
        cell.BorderWidth = 0;
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase(cotiza.currency == ApplicationConstants.PESOS_CURRENCY ? "PESOS" : "U.S. DOLLARS / DÓLARES", HelveticaBoldRed));
        cell.BorderWidth = 0;
        cell.VerticalAlignment = Element.ALIGN_TOP;
        table.AddCell(cell);
        
        cell = new PdfPCell(new Phrase("Net Premium (Prima Neta):", HelveticaBold));
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        cell.VerticalAlignment = Element.ALIGN_TOP;
        cell.BorderWidth = 0;
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase(((decimal)cotiza.NetPremium).ToString("C"), HelveticaBoldRed));
        cell.BorderWidth = 0;
        cell.VerticalAlignment = Element.ALIGN_TOP;
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase("Fees (Derechos):", HelveticaBold));
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        cell.VerticalAlignment = Element.ALIGN_TOP;
        cell.BorderWidth = 0;
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase(ApplicationConstants.Fee(cotiza.currency, fee).ToString("C"), HelveticaBoldRed));
        cell.BorderWidth = 0;
        cell.VerticalAlignment = Element.ALIGN_TOP;
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase("RXPF:", HelveticaBold));
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        cell.VerticalAlignment = Element.ALIGN_TOP;
        cell.BorderWidth = 0;
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase(rxpf.ToString("C") + "     " + rxpfP.ToString("P"), HelveticaBoldRed));
        cell.BorderWidth = 0;
        cell.VerticalAlignment = Element.ALIGN_TOP;
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase("Tax (Impuestos):", HelveticaBold));
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        cell.VerticalAlignment = Element.ALIGN_TOP;
        cell.BorderWidth = 0;
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase(tax.ToString("C") + "     " + taxP.ToString("P"), HelveticaBoldRed));
        cell.BorderWidth = 0;
        cell.VerticalAlignment = Element.ALIGN_TOP;
        table.AddCell(cell);

        cell = new PdfPCell(new Phrase("Total Premium (Prima total):", HelveticaBold));
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        cell.VerticalAlignment = Element.ALIGN_TOP;
        cell.BorderWidth = 0;
        table.AddCell(cell);
        cell = new PdfPCell(new Phrase(total.ToString("C"), HelveticaBoldRed));
        cell.BorderWidth = 0;
        cell.VerticalAlignment = Element.ALIGN_TOP;
        table.AddCell(cell);

        tableCover.AddCell(table);
        Paragraph carrierInformation = new Paragraph("Insurance Carrier Information:\nMapfre México SA\nAM Best Rating \" A - \"\nwww.mapfre.com.mx\n\n", HelveticaSmall);
        cell = new PdfPCell();
        cell.BorderWidth = 0;
        cell.VerticalAlignment = Element.ALIGN_TOP;
        cell.AddElement(carrierInformation);
        Image carrierLogo = Image.GetInstance(HttpContext.Current.Server.MapPath("~/images/logoMapfre.png"));
        carrierLogo.ScaleAbsolute(130 * 0.5f, 38 * 0.5f);
        cell.AddElement(carrierLogo);
        tableCover.AddCell(cell);
        
        document.Add(tableCover);

        p = new Chunk("This quotation is based on terms & conditions of a standard Mexican Insurance policy." +
                      "Please be aware that Mexican Insurance policies have different terms & conditions from policies applicable in the USA." +
                      "If you need to review the coverage terms, you can request a sample copy.", Helvetica);
        
        document.Add(p);
        document.Close();
    }

    public void ProcessRequest (HttpContext context) 
    {
        int quoteid = 0;
        string path = Path.GetTempFileName();
        try {
            quoteid = int.Parse(context.Request["quote"]);
            CreatePDF(quoteid, path);
        } catch {
            quoteid = 0;
        }
        if (quoteid == 0) {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Not a valid quote");
            return;
        }
        context.Response.ContentType = "application/pdf";
        context.Response.AddHeader("Content-Disposition", "attachment; filename=Cotiza" + quoteid + ".pdf");
        context.Response.TransmitFile(path);
       
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}