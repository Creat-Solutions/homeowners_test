using Protec.Authentication.Membership;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;



public partial class UserControls_ApplicationStep6 : System.Web.UI.UserControl, IApplicationStep
{
    private string _title = "Additional Coverages";

    protected void valCSL_ServerValidate(object source, ServerValidateEventArgs args)
    {
        valLimit_ServerValidate(source as CustomValidator, args, 1, false);
    }

    protected void valBurglary_ServerValidate(object source, ServerValidateEventArgs args)
    {
        valLimit_ServerValidate(source as CustomValidator, args, 3, !chkBurglary.Checked);
    }

    protected void valJewelry_ServerValidate(object source, ServerValidateEventArgs args)
    {
        valLimit_ServerValidate(source as CustomValidator, args, 4, !chkJewelry.Checked);
    }

    protected void valMoney_ServerValidate(object source, ServerValidateEventArgs args)
    {
        valLimit_ServerValidate(source as CustomValidator, args, 5, !chkMoney.Checked);
    }

    protected void valGlass_ServerValidate(object source, ServerValidateEventArgs args)
    {
        valLimit_ServerValidate(source as CustomValidator, args, 6, !chkGlass.Checked);
    }

    protected void valElectronic_ServerValidate(object source, ServerValidateEventArgs args)
    {
        valLimit_ServerValidate(source as CustomValidator, args, 7, !chkElectronic.Checked);
    }

    protected void valPersonal_ServerValidate(object source, ServerValidateEventArgs args)
    {
        valLimit_ServerValidate(source as CustomValidator, args, 8, !chkPersonal.Checked);
    }


    private void valLimit_ServerValidate(CustomValidator validator, ServerValidateEventArgs args, int id, bool emptyAllowed)
    {
        decimal value;
        validator.Text = "Invalid value. ";

        if (string.IsNullOrEmpty(args.Value))
        {
            args.IsValid = emptyAllowed;
            if (!emptyAllowed)
            {
                validator.Text = "Required";
            }
            return;
        }

        try
        {
            value = decimal.Parse(args.Value);
        }
        catch
        {
            args.IsValid = false;
            return;
        }
        if (id <= 0)
        {
            args.IsValid = false;
            return;
        }
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        decimal minimum, maximum;
        try
        {
            int quoteid = (int)Session["quoteid"];
            var limit = db.additionalCoverages.Select(m => new { m.id, m.minimum, m.maximum, m.percentage }).Single(m => m.id == id);
            var info = db.quotes.Where(m => m.id == quoteid).Select(m => new { m.contentsLimit, m.currency }).Single();
            decimal content = info.contentsLimit;
            int currency = info.currency;
            minimum = (decimal)limit.minimum;
            maximum = (decimal)limit.maximum;
            if (limit.percentage)
            {
                minimum *= (decimal)content / 100;
                maximum *= (decimal)content / 100;
            }
            else if (currency != 2)
            {
                UserInformation user = (UserInformation)HttpContext.Current.User.Identity;
                decimal tipoCambio = user.ExchangeRate;
                minimum *= tipoCambio;
                maximum *= tipoCambio;

                // max 20M en pesos
                if (validator == valCSL && maximum > 20000000M)
                {
                    maximum = 20000000M;
                }

            }
        }
        catch
        {
            args.IsValid = false;
            return;
        }
        bool valid = value >= minimum && value <= maximum;
        if (!valid)
            validator.Text += "Minimum: " + minimum.ToString("C") + ", Maximum: " + maximum.ToString("C");

        args.IsValid = valid;

    }

    private void LoadLimitName(Label lblLimit, Label lblLimitEs, string name, string nombre)
    {
        lblLimit.Text = name;
        lblLimitEs.Text = nombre;
    }

    private void LoadLimits(HtmlTableCell minimum, HtmlTableCell maximum, decimal min, decimal max)
    {
        minimum.InnerHtml = min.ToString("C");
        maximum.InnerHtml = max.ToString("C");
    }

    private void LoadTable(HouseInsuranceDataContext db, int quoteid)
    {
        var info = db.quotes.Where(m => m.id == quoteid).Select(m => new { m.contentsLimit, m.currency }).Single();
        decimal content = info.contentsLimit;
        int currency = info.currency;

        var coverages = db.additionalCoverages.ToArray();
        int numberCoverages = coverages.Length;
        if (numberCoverages != 10)
            return;
        Label[] lblEnglish = { lblCSL, lblDomestic, lblBurglary, lblJewelry, lblMoney, lblGlass, lblElectronic, lblPersonal, lblFamily, lblTechSupport };
        Label[] lblSpanish = { lblCSLEs, lblDomesticEs, lblRobo, lblJoyas, lblDinero, lblCristales, lblElectronico, lblPersonalEs, lblFamilia, lblAsistenciaInformatica };
        HtmlTableCell[] tdMinimum = { tdCSLMinimum, tdDomesticMinimum, tdBurglaryMinimum, tdJewelryMinimum, tdMoneyMinimum, tdGlassMinimum, tdElectronicMinimum, tdPersonalMinimum, tdFamilyMinimum };
        HtmlTableCell[] tdMaximum = { tdCSLMaximum, tdDomesticMaximum, tdBurglaryMaximum, tdJewelryMaximum, tdMoneyMaximum, tdGlassMaximum, tdElectronicMaximum, tdPersonalMaximum, tdFamilyMaximum };

        bool pesos = currency == 1;
        UserInformation user = (UserInformation)HttpContext.Current.User.Identity;
        decimal tipoCambio = user.ExchangeRate;

        for (int i = 0; i < numberCoverages && i < lblEnglish.Length; i++)
        {
            LoadLimitName(lblEnglish[i], lblSpanish[i], coverages[i].name, coverages[i].nameEs);
            if (coverages[i].minimum == null && coverages[i].maximum == null)
                continue;
            decimal minimum = coverages[i].minimum ?? 0;
            decimal maximum = coverages[i].maximum ?? 0;
            if (coverages[i].percentage)
            {
                minimum *= content / 100;
                maximum *= content / 100;
            }
            else if (pesos)
            {
                minimum *= tipoCambio;
                maximum *= tipoCambio;

                if (i == 0 && maximum > 20000000M)
                {
                    maximum = 20000000M;
                }

            }
            LoadLimits(tdMinimum[i], tdMaximum[i], minimum, maximum);
            Page.ClientScript.RegisterArrayDeclaration("valuesMinimum", minimum.ToString("F2"));
            Page.ClientScript.RegisterArrayDeclaration("valuesMaximum", maximum.ToString("F2"));
        }
    }

    private void LoadLimitValue(TextBox txtLimit, decimal value, CheckBox chkLimit, int? use)
    {
        if (use != null && use != PRIMARY_RESIDENCE)
        {
            chkLimit.Enabled = chkLimit.Checked = txtLimit.Enabled = false;
            txtLimit.Text = string.Empty;
        }
        else if (value == 0)
        {
            chkLimit.Checked = txtLimit.Enabled = false;
            txtLimit.Text = string.Empty;
        }
        else
        {
            chkLimit.Checked = txtLimit.Enabled = true;
            txtLimit.Text = value.ToString("N");
        }
    }

    private void LoadLimitValue(TextBox txtLimit, decimal value, CheckBox chkLimit)
    {
        LoadLimitValue(txtLimit, value, chkLimit, null);
    }

    private const int PRIMARY_RESIDENCE = 1;

    public void LoadStep()
    {
        int key = (int)Session["quoteid"];

        HouseInsuranceDataContext db = new HouseInsuranceDataContext();

        LoadTable(db, key);

        var info = db.quotes.Select(m => new
        {
            m.id,
            m.CSLLimit,
            m.moneySecurityLimit,
            m.accidentalGlassBreakageLimit,
            m.electronicEquipmentLimit,
            m.personalItemsLimit,
            m.domesticWorkersLiability,
            m.burglaryLimit,
            m.burglaryJewelryLimit,
            m.familyAssistance,
            m.limitsEnabled,
            m.useOfProperty,
            m.techSupport
        }).Single(m => m.id == key);



        txtCSL.Text = info.CSLLimit.ToString("N");
        chkDomestic.Checked = info.domesticWorkersLiability;
        LoadLimitValue(txtBurglary, info.burglaryLimit, chkBurglary);
        LoadLimitValue(txtJewelry, info.burglaryJewelryLimit, chkJewelry, info.useOfProperty);
        LoadLimitValue(txtMoney, info.moneySecurityLimit, chkMoney, info.useOfProperty);
        LoadLimitValue(txtGlass, info.accidentalGlassBreakageLimit, chkGlass);
        LoadLimitValue(txtElectronic, info.electronicEquipmentLimit, chkElectronic);
        LoadLimitValue(txtPersonal, info.personalItemsLimit, chkPersonal);
        chkFamily.Checked = info.familyAssistance;
        chkTechSupport.Checked = info.techSupport;

        string boolPrimary = info.useOfProperty == PRIMARY_RESIDENCE ? "true" : "false";
        Page.ClientScript.RegisterStartupScript(GetType(), "primaryResidenceInit", "primaryResidence = " + boolPrimary + ";", true);// +

        lblFamilyCovered.Text = chkFamily.Checked ? "COVERED / CUBIERTO" : "EXCLUDED / EXCLU&Iacute;DO";

        lblTechSupportCovered.Text = chkTechSupport.Checked ? "COVERED / CUBIERTO" : "EXCLUDED / EXCLU&Iacute;DO";
    }

    public bool SaveStep()
    {
        if (Page.IsValid)
        {
            try
            {
                saveData((int)Session["quoteid"]);
            }
            catch
            {
                return false;
            }
            return true;
        }
        return false;
    }

    public string Title()
    {
        return _title;
    }

    private decimal GetLimitData(TextBox txtLimit)
    {
        try
        {
            return string.IsNullOrEmpty(txtLimit.Text) ? 0 : decimal.Parse(txtLimit.Text);
        }
        catch
        {
            return 0;
        }
    }

    private void saveData(int key)
    {
        if (key <= 0)
            throw new Exception("key <= 0");

        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        var quoteInfo = db.quotes.Where(m => m.id == key).Select(m => new { m.useOfProperty, m.insuredPersonHouse1 }).Single();
        int useProperty = (int)quoteInfo.useOfProperty;

        decimal CGLLimit = GetLimitData(txtCSL);
        bool domestic = true;
        decimal burglaryLimit = GetLimitData(txtBurglary);
        decimal burglaryJewelsLimit = GetLimitData(txtJewelry);
        if (burglaryLimit == 0)
        {
            burglaryJewelsLimit = 0;
        }
        decimal moneyLimit = GetLimitData(txtMoney);
        decimal glassLimit = GetLimitData(txtGlass);
        decimal electronicLimit = GetLimitData(txtElectronic);
        decimal personalLimit = GetLimitData(txtPersonal);
        bool family = chkFamily.Checked;
        bool techSupport = chkTechSupport.Checked;

        if (useProperty != PRIMARY_RESIDENCE)
        {
            burglaryJewelsLimit = moneyLimit = 0;
        }

        if (db.spUpd_V1_quoteCoverages(key, CGLLimit, domestic, burglaryLimit, burglaryJewelsLimit, moneyLimit,
                                   glassLimit, electronicLimit, personalLimit, family, techSupport) == 0)
        {
            var he = quoteInfo.insuredPersonHouse1.HouseEstate;
            db.spUpd_quote(key, he.TEV, he.City.HMP, he.City.State.TheftZone[0]);
            bool limitsEnabled = db.quotes.Where(m => m.id == key).Select(m => m.limitsEnabled).Single();
            if (!limitsEnabled)
            {
                SendEmail(key);
            }
            //invalidate cache (InsuranceApplication.aspx.cs+ShouldDisableButtons)
            Session["disableButtons"] = false;
        }
        else
        {
            throw new Exception("quoteCoverages != 0");
        }
    }

    private void SendEmail(int quoteid)
    {
        Control c = Page.LoadControl("~/UserControls/Calcula.ascx");
        (c as IQuoteData).quoteid = quoteid;
        StringWriter sw = new StringWriter();
        HtmlTextWriter writer = new HtmlTextWriter(sw);
        string renderedContents = string.Empty;
        try
        {
            c.RenderControl(writer);
            renderedContents = sw.GetStringBuilder().ToString();
        }
        catch (Exception e)
        {
            renderedContents = "Error while processing the data. Please access the quote information directly. Quote number = " + quoteid;
            renderedContents += "<br />Error message: " + e.Message;
            renderedContents += "<br />Stack trace: " + e.StackTrace;
        }
        finally
        {
            sw.Close();
            writer.Close();
        }
        //string from = ConfigurationSettings.AppSettings["MailFrom"];
        //string to = ConfigurationSettings.AppSettings["MailNoLimits"];
        //MailMessage message = new MailMessage(from, to);
        //message.CC.Add("manuel@creatsol.com");
        //message.IsBodyHtml = true;
        //message.Subject = "Homeowner's Insurance Application: Cotizacion sin limites no. " + quoteid;
        //message.Body = renderedContents;
        //SmtpClient client = new SmtpClient(ConfigurationSettings.AppSettings["MailServer"]);
        //if (ConfigurationSettings.AppSettings["MailAuthenticationRequired"] == "true") {
        //    client.Credentials = new NetworkCredential(from, ConfigurationSettings.AppSettings["MailPassword"]);
        //}
        //client.Send(message);
    }

}
