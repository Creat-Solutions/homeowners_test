using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ME.Admon;

public partial class UserControls_Cotiza : System.Web.UI.UserControl, IQuoteData
{
    public int _quoteid = 0;

    public int quoteid
    {
        get
        {
            return _quoteid;
        }
        set
        {
            _quoteid = value;
            LoadData();
        }
    }

    private string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private string getMonth(int month)
    {
        return months[month - 1];
    }

    private string DeduciblesHelper(decimal? value)
    {
        if (value == null || value == 0M)
            return "Excluded";
        decimal v = (decimal)value;
        v *= 100M;
        return v.ToString("F") + "%";

    }

    private rxpfSur GetRXPFSurcharge(HouseInsuranceDataContext db, int company, int currency)
    {
        if (currency != ApplicationConstants.PESOS_CURRENCY && currency != ApplicationConstants.DOLLARS_CURRENCY)
        {
            currency = ApplicationConstants.DOLLARS_CURRENCY;
        }

        return db.rxpfSurs.Single(m => m.month == DateTime.Now.Month && m.company == company && m.currency == currency);
    }

    private Control FindControlRecursive(Control root, string id)
    {
        if (root.ID == id)
        {
            return root;
        }

        foreach (Control c in root.Controls)
        {
            Control t = FindControlRecursive(c, id);
            if (t != null)
            {
                return t;
            }
        }

        return null;
    }

    private void EnableWithoutPayment(Control control)
    {
        if (control != null && typeof(CheckBox) == control.GetType())
        {
            CheckBox chkBox = control as CheckBox;
            chkBox.Enabled = true;
            chkBox.Checked = false;
        }
    }

    private void DisableWithoutPayment(Control control)
    {
        //if we are buying it
        if (control != null && typeof(CheckBox) == control.GetType())
        {
            CheckBox chkBox = control as CheckBox;
            chkBox.Enabled = false;
            chkBox.Checked = true;
        }
    }

    private void LoadData()
    {
        if (quoteid != 0)
        {
            HouseInsuranceDataContext db = new HouseInsuranceDataContext();
            var cotiza = db.spList_Cotiza(quoteid).Single();
            var dquote = db.quotes.Where(m => m.id == quoteid).Single();
            decimal taxPercentage = db.fGet_QuoteTaxPercentage(quoteid).Value;
            HouseEstate houseEstate = dquote.insuredPersonHouse1.HouseEstate;
            var deducibles = db.spGenerate_DeduciblesCoaseguros(quoteid, null, houseEstate.TEV, houseEstate.City.HMP).Single();
            lblName.Text = cotiza.firstname + " " + cotiza.lastName + " " + cotiza.lastName2;
            lblAddInsured.Text = cotiza.additionalInsured;
            lblPayee.Text = cotiza.bankName;
            if (cotiza.startDate != null)
            {
                lblPeriodStart.Text = cotiza.startDate.ToString();
                lblPeriodEnd.Text = cotiza.endDate.ToString();
                lblPeriodDays.Text = (((DateTime)cotiza.endDate - (DateTime)cotiza.startDate).Days + 1).ToString();
            }
            lblLoanNumber.Text = cotiza.loanNumber;
            lblAdjacent.Text = (bool)cotiza.adjoiningEstates ? "TRUE / VERDADERO" : "FALSE / FALSO";
            lblBeachFront.Text = cotiza.distanceFromSea + " / " + cotiza.distanciaAlMar;
            lblAddress1.Text = cotiza.street + ", " + cotiza.exteriorNo + (cotiza.InteriorNo == null ? "" : "- " + cotiza.InteriorNo);

            string address2 = houseEstate.Name + ", " + houseEstate.City.Name + ", " + houseEstate.City.State.Name + ", " + houseEstate.ZipCode;
            lblAddress2.Text = address2;
            lblTypeOfProperty.Text = cotiza.typeOfProperty;
            lblUseOfProperty.Text = cotiza.useOfProperty;
            if (cotiza.buildingLimit > 0)
            {
                lblBuildingMoney.Visible = true;
                lblBuildingLimit.Text = cotiza.buildingLimit.ToString("C").Substring(1);
            }
            else
            {
                lblBuildingLimit.Text = "EXCLUDED / EXCLU&Iacute;DO";
                lblBuildingMoney.Visible = false;
            }
            lblContentsLimit.Text = cotiza.contentsLimit.ToString("C").Substring(1);
            if (cotiza.outdoorLimit > 0)
            {
                lblOutdoorMoney.Visible = true;
                lblOutdoorLimit.Text = cotiza.outdoorLimit.ToString("C").Substring(1);
            }
            else
            {
                lblOutdoorLimit.Text = "EXCLUDED / EXCLU&Iacute;DO";
                lblOutdoorMoney.Visible = false;
            }
            if (cotiza.debriLimit > 0)
            {
                lblDebriMoney.Visible = true;
                lblDebriLimit.Text = cotiza.debriLimit.ToString("C").Substring(1);
            }
            else
            {
                lblDebriLimit.Text = "EXCLUDED / EXCLU&Iacute;DO";
                lblDebriMoney.Visible = false;
            }
            if (cotiza.extraLimit > 0)
            {
                lblExtraMoney.Visible = true;
                lblExtraLimit.Text = cotiza.extraLimit.ToString("C").Substring(1);
            }
            else
            {
                lblExtraMoney.Visible = false;
                lblExtraLimit.Text = "EXCLUDED / EXCLU&Iacute;DO";
            }
            if (cotiza.lossOfRentsLimit > 0)
            {
                lblLossRentMoney.Visible = true;
                lblLossOfRent.Text = cotiza.lossOfRentsLimit.ToString("C").Substring(1);
            }
            else
            {
                lblLossOfRent.Text = "EXCLUDED / EXCLU&Iacute;DO";
                lblLossRentMoney.Visible = false;
            }

            lblHMP.Text = (bool)cotiza.HMP ? "Covered/Cubierto" : "Excluded/Exclu&iacute;do";
            lblOutdoorDeducible.Text = DeduciblesHelper(deducibles.deducibleInterperie);
            lblOutdoorCoaseguro.Text = DeduciblesHelper(deducibles.coaseguroInterperie);
            lblHMPDeducible.Text = DeduciblesHelper(deducibles.deducibleFHM);
            lblHMPCoaseguro.Text = DeduciblesHelper(deducibles.coaseguroFHM);
            lblTEVDeducible.Text = DeduciblesHelper(deducibles.deducibleterremoto);
            lblTEVCoaseguro.Text = DeduciblesHelper(deducibles.coaseguroTerremoto);


            lblEarthquake.Text = (bool)cotiza.quake ? "Covered/Cubierto" : "Excluded/Exclu&iacute;do";
            //chkHMP.Checked = (bool)cotiza.HMP;
            //chkQuake.Checked = (bool)cotiza.quake;

            if (dquote.cantidadDePagos == 12)
            {
                DisableWithoutPayment(FindControlRecursive(Page, "chkCC"));
            }
            else
            {
                EnableWithoutPayment(FindControlRecursive(Page, "chkCC"));
            }
            rxpfSur surcharge = GetRXPFSurcharge(db, cotiza.company.Value, cotiza.currency);
            decimal fee = ApplicationConstants.Fee(cotiza.currency, dquote.fee.Value);

            plhAdditional.Controls.Clear();
            //additional coverages
            if (cotiza.CSLLimit != 0)
            {
                HtmlGenericControl h3 = new HtmlGenericControl("h3");
                h3.InnerHtml = "CIVIL GENERAL LIABILITY (Responsabilidad Civil General):";
                plhAdditional.Controls.Add(h3);
                plhAdditional.Controls.Add(BuildCSL(Math.Round(cotiza.CSLLimit, 2)));
            }

            if (cotiza.burglaryLimit != 0 || cotiza.burglaryJewelryLimit != 0)
            {
                HtmlGenericControl h3 = new HtmlGenericControl("h3");
                h3.InnerHtml = "BURGLARY (Robo con violencia):";
                plhAdditional.Controls.Add(h3);
                plhAdditional.Controls.Add(BuildBurglary(Math.Round(cotiza.burglaryLimit, 2), Math.Round(cotiza.burglaryJewelryLimit, 2)));
            }
            if (cotiza.accidentalGlassBreakageLimit != 0)
            {
                HtmlGenericControl h3 = new HtmlGenericControl("h3");
                h3.InnerHtml = "ACCIDENTAL GLASS BREAKAGE (Rotura de Cristales):";
                plhAdditional.Controls.Add(h3);
                plhAdditional.Controls.Add(BuildGlass(Math.Round(cotiza.accidentalGlassBreakageLimit, 2)));
            }
            if (cotiza.moneySecurityLimit != 0)
            {
                HtmlGenericControl h3 = new HtmlGenericControl("h3");
                h3.InnerHtml = "MONEY AND SECURITIES (Dinero y Valores):";
                plhAdditional.Controls.Add(h3);
                plhAdditional.Controls.Add(BuildMoney(Math.Round(cotiza.moneySecurityLimit, 2)));
            }
            if (cotiza.electronicEquipmentLimit != 0)
            {
                HtmlGenericControl h3 = new HtmlGenericControl("h3");
                h3.InnerHtml = "ELECTRICAL HOUSEHOLD APPLIANCES (Equipo electrodom&eacute;stico):";
                plhAdditional.Controls.Add(h3);
                plhAdditional.Controls.Add(BuildElectric(Math.Round(cotiza.electronicEquipmentLimit, 2)));
            }
            if (cotiza.personalItemsLimit != 0)
            {
                HtmlGenericControl h3 = new HtmlGenericControl("h3");
                h3.InnerHtml = "PERSONAL ITEMS (Objetos Personales):";
                plhAdditional.Controls.Add(h3);
                plhAdditional.Controls.Add(BuildPersonal(Math.Round(cotiza.personalItemsLimit, 2)));
            }
            HtmlGenericControl tit = new HtmlGenericControl("h3");
            tit.InnerHtml = "FAMILY ASSISTANCE (Asistencia Familiar):";
            plhAdditional.Controls.Add(tit);
            plhAdditional.Controls.Add(BuildFamily(cotiza.familyAssistance));
        }
    }

    #region "BUILDTABLES"

    private HtmlTable BuildFamily(bool assistance)
    {
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell cell;
        row = new HtmlTableRow();
        cell = new HtmlTableCell("td");
        cell.Attributes.Add("class", "cAR");
        cell.InnerHtml = "Limit (L&iacute;mite): ";
        row.Cells.Add(cell);

        cell = new HtmlTableCell("td");
        cell.InnerHtml = "<span class=\"cData\">" + (assistance ? "COVERED / CUBIERTO" : "EXCLUDED / EXCLU&Iacute;DO") + "</span>";
        row.Cells.Add(cell);
        table.Rows.Add(row);

        row = new HtmlTableRow();
        cell = new HtmlTableCell("td");
        cell.Attributes.Add("class", "cAR");
        cell.InnerHtml = "Coverage (Cobertura): ";
        row.Cells.Add(cell);

        cell = new HtmlTableCell("td");
        cell.InnerHtml = "<span class=\"cInfo\">Home assistance (Asistencia en Hogar), Legal Assistance (Asistencia Legal)<br />Medical assistance by phone (MEDITEL).</span>";
        row.Cells.Add(cell);
        table.Rows.Add(row);

        return table;
    }

    private HtmlTable BuildPersonal(decimal value)
    {
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell td;

        row = new HtmlTableRow();
        td = new HtmlTableCell("td");
        td.Attributes.Add("class", "cAR");
        td.InnerHtml = "Limit (L&iacute;mite): ";
        row.Cells.Add(td);

        td = new HtmlTableCell("td");
        td.InnerHtml = "<span class=\"cData\">" + value.ToString("C") + "</span>";
        row.Cells.Add(td);
        table.Rows.Add(row);

        row = new HtmlTableRow();
        td = new HtmlTableCell("td");
        td.Attributes.Add("class", "cAR");
        td.InnerHtml = "Sublimit (Subl&iacute;mite): ";
        row.Cells.Add(td);

        td = new HtmlTableCell("td");
        td.InnerHtml = "<span class=\"cData\">$ 200 per article (por art&iacute;culo)</span>";
        row.Cells.Add(td);
        table.Rows.Add(row);

        row = new HtmlTableRow();
        td = new HtmlTableCell("td");
        td.Attributes.Add("class", "cAR");
        td.InnerHtml = "Coverage (Cobertura): ";
        row.Cells.Add(td);

        td = new HtmlTableCell("td");
        td.InnerHtml = "<span class=\"cInfo\">Portable articles property of the Insured (Artículos portatiles propiedad del asegurado).</span>";
        row.Cells.Add(td);
        table.Rows.Add(row);

        row = new HtmlTableRow();
        td = new HtmlTableCell("td");
        td.Attributes.Add("class", "cAR");
        td.InnerHtml = "Deductible (Deducible): ";
        row.Cells.Add(td);

        td = new HtmlTableCell("td");
        td.InnerHtml = "<span class=\"cInfo\">None (Ninguno)</span>";
        row.Cells.Add(td);
        table.Rows.Add(row);

        row = new HtmlTableRow();
        td = new HtmlTableCell("td");
        td.Attributes.Add("class", "cAR");
        td.InnerHtml = "Major Exclusion (Exclusi&oacute;n): ";
        row.Cells.Add(td);

        td = new HtmlTableCell("td");
        td.InnerHtml = "<span class=\"cData\">Mysterious Disappearance (Robo sin violencia ).</span>";
        row.Cells.Add(td);
        table.Rows.Add(row);
        return table;
    }

    private HtmlTable BuildElectric(decimal value)
    {
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell td;

        row = new HtmlTableRow();
        td = new HtmlTableCell("td");
        td.Attributes.Add("class", "cAR");
        td.InnerHtml = "Limit (L&iacute;mite): ";
        row.Cells.Add(td);

        td = new HtmlTableCell("td");
        td.InnerHtml = "<span class=\"cData\">" + value.ToString("C") + "</span>";
        row.Cells.Add(td);
        table.Rows.Add(row);

        row = new HtmlTableRow();
        td = new HtmlTableCell("td");
        td.Attributes.Add("class", "cAR");
        td.InnerHtml = "Coverage (Cobertura): ";
        row.Cells.Add(td);

        td = new HtmlTableCell("td");
        td.InnerHtml = "<span class=\"cInfo\">Inexperience, Carelessness (Impericia, descuido), Direct action of electrical energy <br />(Acci&oacute;n directa de la energ&iacute;a el&eacute;ctrica).</span>";
        row.Cells.Add(td);
        table.Rows.Add(row);

        row = new HtmlTableRow();
        td = new HtmlTableCell("td");
        td.Attributes.Add("class", "cAR");
        td.InnerHtml = "Deductible (Deducible): ";
        row.Cells.Add(td);

        td = new HtmlTableCell("td");
        td.InnerHtml = "<span class=\"cInfo\">Any damage over $150 US dollars (Cualquier da&ntilde;o superior a $150 US dollars).</span>";
        row.Cells.Add(td);
        table.Rows.Add(row);

        row = new HtmlTableRow();
        td = new HtmlTableCell("td");
        td.Attributes.Add("class", "cAR");
        td.InnerHtml = "Major Exclusion (Exclusi&oacute;n): ";
        row.Cells.Add(td);

        td = new HtmlTableCell("td");
        td.InnerHtml = "<span class=\"cData\">Gradual wear or deterioration (Desgaste o deterioro).<br />Esthetic defects (Defectos est&eacute;ticos).</span>";
        row.Cells.Add(td);
        table.Rows.Add(row);
        return table;
    }

    private HtmlTable BuildMoney(decimal value)
    {
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell td;

        row = new HtmlTableRow();
        td = new HtmlTableCell("td");
        td.Attributes.Add("class", "cAR");
        td.InnerHtml = "Limit (L&iacute;mite): ";
        row.Cells.Add(td);

        td = new HtmlTableCell("td");
        td.InnerHtml = "<span class=\"cData\">" + value.ToString("C") + "</span>";
        row.Cells.Add(td);
        table.Rows.Add(row);

        row = new HtmlTableRow();
        td = new HtmlTableCell("td");
        td.Attributes.Add("class", "cAR");
        td.InnerHtml = "Coverage (Cobertura): ";
        row.Cells.Add(td);

        td = new HtmlTableCell("td");
        td.InnerHtml = "<span class=\"cInfo\">Cash/Valuable Papers (Efectivo/Documentos Valiosos )</span>";
        row.Cells.Add(td);
        table.Rows.Add(row);

        row = new HtmlTableRow();
        td = new HtmlTableCell("td");
        td.Attributes.Add("class", "cAR");
        td.InnerHtml = "Deductible (Deducible): ";
        row.Cells.Add(td);

        td = new HtmlTableCell("td");
        td.InnerHtml = "<span class=\"cInfo\">None (Ninguno).</span>";
        row.Cells.Add(td);
        table.Rows.Add(row);

        row = new HtmlTableRow();
        td = new HtmlTableCell("td");
        td.Attributes.Add("class", "cAR");
        td.InnerHtml = "Major Exclusion (Exclusi&oacute;n): ";
        row.Cells.Add(td);

        td = new HtmlTableCell("td");
        td.InnerHtml = "<span class=\"cData\">Mysterious Disappearance (Robo sin violencia ).</span>";
        row.Cells.Add(td);
        table.Rows.Add(row);
        return table;
    }


    private HtmlTable BuildGlass(decimal value)
    {
        HtmlTable table = new HtmlTable();
        HtmlTableRow row = new HtmlTableRow();
        HtmlTableCell td = new HtmlTableCell("td");
        td.Attributes.Add("class", "cAR");
        td.InnerHtml = "Limit (L&iacute;mite): ";
        row.Cells.Add(td);

        td = new HtmlTableCell("td");
        td.InnerHtml = "<span class=\"cData\">" + value.ToString("C") + "</span>";
        row.Cells.Add(td);
        table.Rows.Add(row);

        row = new HtmlTableRow();
        td = new HtmlTableCell("td");
        td.Attributes.Add("class", "cAR");
        td.InnerHtml = "Coverage (Cobertura): ";
        row.Cells.Add(td);

        td = new HtmlTableCell("td");
        td.InnerHtml = "<span class=\"cInfo\">Accidental glass breakage (Rotura de cristales).</span>";
        row.Cells.Add(td);
        table.Rows.Add(row);

        row = new HtmlTableRow();
        td = new HtmlTableCell("td");
        td.Attributes.Add("class", "cAR");
        td.InnerHtml = "Deductible (Deducible): ";
        row.Cells.Add(td);

        td = new HtmlTableCell("td");
        td.InnerHtml = "<span class=\"cInfo\">None (Ninguno).</span>";
        row.Cells.Add(td);
        table.Rows.Add(row);

        return table;
    }

    private HtmlTable BuildBurglary(decimal burglary, decimal jewelry)
    {
        HtmlTable table = new HtmlTable();
        HtmlTableRow row;
        HtmlTableCell td;
        if (burglary != 0)
        {
            row = new HtmlTableRow();
            td = new HtmlTableCell("td");
            td.Attributes.Add("class", "cAR");
            td.InnerHtml = "Limit (L&iacute;mite): ";
            row.Cells.Add(td);

            td = new HtmlTableCell("td");
            td.InnerHtml = "<span class=\"cData\">" + burglary.ToString("C") + "  Item I - General Household Contents &amp; Electronic/Photo/Sport.</span>";
            row.Cells.Add(td);
            table.Rows.Add(row);
        }
        if (jewelry != 0)
        {
            row = new HtmlTableRow();
            td = new HtmlTableCell("td");
            td.Attributes.Add("class", "cAR");
            td.InnerHtml = "Limit (L&iacute;mite): ";
            row.Cells.Add(td);

            td = new HtmlTableCell("td");
            td.InnerHtml = "<span class=\"cData\">" + jewelry.ToString("C") + "  Item II - Art, Jewelry, Watches, Furs, Collections, Gold, &amp; Silver items</span>";
            row.Cells.Add(td);
            table.Rows.Add(row);
        }

        row = new HtmlTableRow();
        td = new HtmlTableCell("td");
        td.Attributes.Add("class", "cAR");
        td.InnerHtml = "Coverage (Cobertura): ";
        row.Cells.Add(td);

        td = new HtmlTableCell("td");
        td.InnerHtml = "<span class=\"cInfo\">Theft of contents within premises with violence and/or assault <br />(Robo de contenidos dentro de los predios con violencia y/o asalto).</span>";
        row.Cells.Add(td);
        table.Rows.Add(row);

        row = new HtmlTableRow();
        td = new HtmlTableCell("td");
        td.Attributes.Add("class", "cAR");
        td.InnerHtml = "Deductible (Deducible): ";
        row.Cells.Add(td);

        td = new HtmlTableCell("td");
        td.InnerHtml = "<span class=\"cInfo\">None (Ninguno)</span>";
        row.Cells.Add(td);
        table.Rows.Add(row);

        row = new HtmlTableRow();
        td = new HtmlTableCell("td");
        td.Attributes.Add("class", "cAR");
        td.InnerHtml = "Major Exclusion (Exclusi&oacute;n): ";
        row.Cells.Add(td);

        td = new HtmlTableCell("td");
        td.InnerHtml = "<span class=\"cData\">Mysterious Disappearance (Robo sin violencia ).</span>";
        row.Cells.Add(td);
        table.Rows.Add(row);
        return table;
    }

    private HtmlTable BuildCSL(decimal value)
    {
        HtmlTable table = new HtmlTable();
        HtmlTableRow row = new HtmlTableRow();
        HtmlTableCell td = new HtmlTableCell("td");
        td.Attributes.Add("class", "cAR");
        td.InnerHtml = "Limit (L&iacute;mite): ";
        row.Cells.Add(td);

        td = new HtmlTableCell("td");
        td.InnerHtml = "<span class=\"cData\">" + value.ToString("C") + " CSL PD/BI Occ/Agg (L.U.C.)</span>";
        row.Cells.Add(td);
        table.Rows.Add(row);

        row = new HtmlTableRow();
        td = new HtmlTableCell("td");
        td.Attributes.Add("class", "cAR");
        td.InnerHtml = "Coverage (Cobertura): ";
        row.Cells.Add(td);

        td = new HtmlTableCell("td");
        td.InnerHtml = "<span class=\"cInfo\">Homeowners Liability (RC Familiar), Worldwide coverage (Cobertura Mundial).<br />Worldwide coverage (Worldwide coverage is only provided if the Insure permanenty resides in Mexico. (La cobertura mundial solo se otorga cuando el asegurado resida legalmente en M&eacute;xico).</span>";
        row.Cells.Add(td);
        table.Rows.Add(row);

        row = new HtmlTableRow();
        td = new HtmlTableCell("td");
        td.Attributes.Add("class", "cAR");
        td.InnerHtml = "Deductible (Deducible): ";
        row.Cells.Add(td);

        td = new HtmlTableCell("td");
        td.InnerHtml = "<span class=\"cInfo\">None (Ninguno).</span>";
        row.Cells.Add(td);
        table.Rows.Add(row);

        row = new HtmlTableRow();
        td = new HtmlTableCell("td");
        td.Attributes.Add("class", "cAR");
        td.InnerHtml = "Major Exclusion (Exclusi&oacute;n): ";
        row.Cells.Add(td);

        td = new HtmlTableCell("td");
        td.InnerHtml = "<span class=\"cData\">Punitive Damages or exemplary damages  (Da&ntilde;os Punitivos o ejemplares)</span>";
        row.Cells.Add(td);
        table.Rows.Add(row);
        return table;
    }
    #endregion
}
