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



public partial class UserControls_QuickQuoteStep3 : System.Web.UI.UserControl, IApplicationStep
{
    private string _title = "Additional Security Information";
    protected void valOutdoor_Validate(object source, ServerValidateEventArgs args)
    {
        TextBox txtOutdoor;
        CustomValidator validator = source as CustomValidator;
        try
        {
            txtOutdoor = validator.FindControl(validator.ControlToValidate) as TextBox;
            if (txtOutdoor == null || chkOutdoor == null)
                throw new Exception();
        }
        catch
        {
            args.IsValid = false;
            return;
        }
        args.IsValid = !chkOutdoor.Checked || txtOutdoor.Text.Length > 0;
    }

    protected void valBuilding_ServerValidate(object source, ServerValidateEventArgs args)
    {
        valLimit_ServerValidate(source as CustomValidator, args, 1, !chkBuilding.Checked);
    }

    protected void valOutdoor_ServerValidate(object source, ServerValidateEventArgs args)
    {
        valLimit_ServerValidate(source as CustomValidator, args, 2, !chkOutdoor.Checked);
    }

    protected void valContents_ServerValidate(object source, ServerValidateEventArgs args)
    {
        valLimit_ServerValidate(source as CustomValidator, args, 3, false);
    }

    protected void valDebri_ServerValidate(object source, ServerValidateEventArgs args)
    {
        valLimit_ServerValidate(source as CustomValidator, args, 4, false);
    }

    protected void valExtra_ServerValidate(object source, ServerValidateEventArgs args)
    {
        valLimit_ServerValidate(source as CustomValidator, args, 5, false);
    }

    protected void valLossRents_ServerValidate(object source, ServerValidateEventArgs args)
    {
        valLimit_ServerValidate(source as CustomValidator, args, 6, !chkLossRents.Checked);
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

        if ((validator == valBuilding || validator == valContents) && chkLimits.Checked)
        {
            args.IsValid = true;
            return;
        }

        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        decimal minimum, maximum;
        try
        {
            var limit = db.valueSublimits.Where(m => m.company == ApplicationConstants.COMPANY).Select(m => new { m.id, m.minimum, m.maximum, m.percentage }).Single(m => m.id == id);
            if (limit.percentage)
            {
                TextBox building = txtBuilding;
                TextBox content = txtContents;

                decimal buildingValue = 0;
                try
                {
                    buildingValue = string.IsNullOrEmpty(building.Text) ? 0M : decimal.Parse(building.Text);
                }
                catch
                {
                    buildingValue = 0;
                }

                decimal sum = buildingValue + decimal.Parse(content.Text);

                if (validator == valDebri || validator == valExtra)
                {
                    minimum = sum * limit.minimum / 100;
                    maximum = sum * limit.maximum / 100;
                }
                else
                {
                    throw new Exception();
                }
            }
            else
            {
                minimum = limit.minimum;
                maximum = limit.maximum;
                //distinto a Dolares
                if (ddlCurrency.SelectedValue != "2")
                {
                    UserInformation user = (UserInformation)HttpContext.Current.User.Identity;
                    decimal tipoCambio = user.ExchangeRate;
                    minimum *= tipoCambio;
                    maximum *= tipoCambio;
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

    private void LoadLimitValues(HtmlTableCell minimum, HtmlTableCell maximum, decimal min, decimal max, bool percentage, decimal sum)
    {
        string minStr, maxStr;
        if (percentage)
        {
            if (sum == 0)
            {
                minStr = (min / 100).ToString("P");
                maxStr = (max / 100).ToString("P");
            }
            else
            {
                minStr = (sum * min / 100).ToString("C");
                maxStr = (sum * max / 100).ToString("C");
            }
        }
        else
        {
            minStr = min.ToString("C");
            maxStr = max.ToString("C");
        }
        minimum.InnerHtml = minStr;
        maximum.InnerHtml = maxStr;
    }

    private void LoadTable(HouseInsuranceDataContext db, decimal sum, int currency)
    {
        var limits = db.valueSublimits.Where(m => m.company == ApplicationConstants.COMPANY).ToArray();
        if (limits.Length != 6)
            return;

        Label[] lblEnglish = { lblBuilding, lblOutdoor, lblContents, lblDebri, lblExtra, lblLossRent };
        Label[] lblSpanish = { lblEdificio, lblExterior, lblContenidos, lblEscombros, lblExtraEs, lblRenta };
        HtmlTableCell[] tdMinimum = { tdBuildingMinimum, tdOutdoorMinimum, tdContentsMinimum, tdDebriMinimum, tdExtraMinimum, tdLossRentsMinimum };
        HtmlTableCell[] tdMaximum = { tdBuildingMaximum, tdOutdoorMaximum, tdContentsMaximum, tdDebriMaximum, tdExtraMaximum, tdLossRentsMaximum };

        UserInformation user = (UserInformation)HttpContext.Current.User.Identity;
        decimal tipoCambio = user.ExchangeRate;
        bool pesos = currency == ApplicationConstants.PESOS_CURRENCY;
        for (int i = 0; i < 6; i++)
        {
            decimal minimum = limits[i].minimum;
            decimal maximum = limits[i].maximum;
            var minimumPesos = minimum * tipoCambio;
            var maximumPesos = maximum * tipoCambio;
            if (limits[i].percentage)
            {
                Page.ClientScript.RegisterArrayDeclaration("percentageMinimum", minimum.ToString("F2"));
                Page.ClientScript.RegisterArrayDeclaration("percentageMaximum", maximum.ToString("F2"));
            }
            else
            {
                Page.ClientScript.RegisterArrayDeclaration("valuesPesosMinimum", minimumPesos.ToString("F2"));
                Page.ClientScript.RegisterArrayDeclaration("valuesPesosMaximum", maximumPesos.ToString("F2"));
                Page.ClientScript.RegisterArrayDeclaration("valuesMinimum", minimum.ToString("F2"));
                Page.ClientScript.RegisterArrayDeclaration("valuesMaximum", maximum.ToString("F2"));
            }
            LoadLimitValues(
                tdMinimum[i],
                tdMaximum[i],
                currency == ApplicationConstants.PESOS_CURRENCY ? minimumPesos : minimum,
                currency == ApplicationConstants.PESOS_CURRENCY ? maximumPesos : maximum,
                limits[i].percentage,
                sum
            );
            LoadLimitName(lblEnglish[i], lblSpanish[i], limits[i].name, limits[i].nameEs);

        }
    }

    private void LoadOtherLimitTable(HouseInsuranceDataContext db, int quoteid)
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
            var minimumPesos = minimum * tipoCambio;
            var maximumPesos = maximum * tipoCambio;

            if (coverages[i].percentage)
            {
                Page.ClientScript.RegisterArrayDeclaration("percentageMinimum", minimum.ToString("F2"));
                Page.ClientScript.RegisterArrayDeclaration("percentageMaximum", maximum.ToString("F2"));
            }
            else
            {


                if (i == 0 && maximum > 20000000M)
                {
                    maximum = 20000000M;
                }

                Page.ClientScript.RegisterArrayDeclaration("valuesPesosMinimum", minimumPesos.ToString("F2"));
                Page.ClientScript.RegisterArrayDeclaration("valuesPesosMaximum", maximumPesos.ToString("F2"));

                Page.ClientScript.RegisterArrayDeclaration("valuesMinimum", minimum.ToString("F2"));
                Page.ClientScript.RegisterArrayDeclaration("valuesMaximum", maximum.ToString("F2"));
            }

            LoadLimitValues(
                tdMinimum[i],
                tdMaximum[i],
                currency == ApplicationConstants.PESOS_CURRENCY ? minimumPesos : minimum,
                currency == ApplicationConstants.PESOS_CURRENCY ? maximumPesos : maximum,
                coverages[i].percentage,
                content
            );
        }
    }

    private void LoadTextbox(TextBox txtLimit, decimal value, CheckBox chkLimit)
    {
        if (value == 0)
        {
            txtLimit.Enabled = chkLimit.Checked = false;
            txtLimit.Text = string.Empty;
        }
        else
        {
            txtLimit.Enabled = chkLimit.Checked = true;
            txtLimit.Text = value.ToString("N");
        }
    }

    private void LoadCurrency(int currency)
    {
        if (currency != 2 && currency != 1)
            return;
        ddlCurrency.SelectedValue = currency.ToString();
    }

    public void LoadStep()
    {
        int key = (int)Session["quoteid"];
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();

        var info = db.quotes.Single(m => m.id == key);

        LoadTable(db, info.buildingLimit + info.contentsLimit, info.currency);

        LoadCurrency(info.currency);

        LoadTextbox(txtBuilding, info.buildingLimit, chkBuilding);

        if (info.outdoorLimit == 0)
        {
            txtOutdoorDescription.Enabled = false;
            txtOutdoorDescription.Text = string.Empty;
        }
        else
        {
            txtOutdoorDescription.Enabled = true;
            txtOutdoorDescription.Text = info.outdoorDescription;
        }

        txtContents.Text = info.contentsLimit.ToString("N");


        txtDebri.Text = info.debriLimit.ToString("N");
        txtExtra.Text = info.extraLimit.ToString("N");
        LoadTextbox(txtLossRents, info.lossOfRentsLimit, chkLossRents);

        bool isAdministrator = ((UserInformation)HttpContext.Current.User.Identity).UserType == SecurityManager.ADMINISTRATOR;
        bool validHMP = db.spIsValidHMPDate() == 1 || isAdministrator;

        if (info.HMP != null && validHMP && info.HMP.Value)
        {
            LoadTextbox(txtOutdoor, info.outdoorLimit, chkOutdoor);
            chkOutdoor.Enabled = true;
        }
        else
        {
            LoadTextbox(txtOutdoor, 0, chkOutdoor);
            chkOutdoor.Enabled = false;
        }

        chkLimits.Checked = !info.limitsEnabled;
        if (!info.limitsEnabled)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "BoolLimits", "ToggleValidators();", true);
        }
        string dlls = info.currency == 2 ? "true" : "false";
        Page.ClientScript.RegisterStartupScript(GetType(), "BoolCurrencyDollars", "StartCurrency(" + dlls + ");", true);

        // from step 6

        LoadOtherLimitTable(db, key);

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

        bool otherLimits = ShowOtherLimits(info);
        chkOtherLimits.Checked = otherLimits;
        divOtherLimits.Style["display"] = otherLimits ? "block" : "none";

    }

    private bool ShowOtherLimits(quote info)
    {
        return LimitWithValue(info.lossOfRentsLimit) ||
               LimitWithValue(info.moneySecurityLimit) ||
               LimitWithValue(info.electronicEquipmentLimit) ||
               LimitWithValue(info.personalItemsLimit) ||
               info.familyAssistance ||
               info.domesticWorkersLiability;
    }

    private bool LimitWithValue(decimal amount)
    {
        return amount > 0;
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

    public void Page_Load(object sender, EventArgs e)
    {
        UserInformation info = (UserInformation)Page.User.Identity;
        divLimits.Visible = info.Agent == ApplicationConstants.MEAGENT || info.UserType == SecurityManager.ADMINISTRATOR || info.UserID == ApplicationConstants.PTI_USER;
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
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();

        bool isAdministrator = ((UserInformation)HttpContext.Current.User.Identity).UserType == SecurityManager.ADMINISTRATOR;
        bool validHMP = db.spIsValidHMPDate() == 1 || isAdministrator;

        var quoteInfo = db.quotes.Where(m => m.id == key).Select(m => new
        {
            m.HMP,
            m.fireAll,
            m.quake,
            m.useOfProperty,
            m.insuredPersonHouse
        }).Single();

        decimal buildingLimit = GetLimitData(txtBuilding);
        decimal outdoorLimit = validHMP && txtOutdoor.Text.Length > 0 ? GetLimitData(txtOutdoor) : 0;
        decimal contentsLimit = GetLimitData(txtContents);
        decimal debriLimit = GetLimitData(txtDebri);
        decimal extraLimit = GetLimitData(txtExtra);
        decimal lossOfRentsLimit = GetLimitData(txtLossRents);

        string description = null;
        if (outdoorLimit > 0)
            description = txtOutdoorDescription.Text;

        bool limitsEnabled = !chkLimits.Checked;

        int tipoCambio = int.Parse(ddlCurrency.SelectedValue);

        db.spUpd_V1_quoteLimits(
            key,
            buildingLimit,
            outdoorLimit,
            contentsLimit,
            debriLimit,
            extraLimit,
            lossOfRentsLimit,
            0,
            description,
            null,
            quoteInfo.fireAll,
            quoteInfo.quake,
            quoteInfo.HMP,
            limitsEnabled,
            tipoCambio
        );

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
            var he = db.insuredPersonHouses.Single(m => m.id == quoteInfo.insuredPersonHouse).HouseEstate;
            db.spUpd_quote(key, he.TEV, he.City.HMP, he.City.State.TheftZone[0]);
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


    /// step 6
    /// 

    protected void valCSL_ServerValidate(object source, ServerValidateEventArgs args)
    {
        valOtherLimit_ServerValidate(source as CustomValidator, args, 1, false);
    }

    protected void valBurglary_ServerValidate(object source, ServerValidateEventArgs args)
    {
        valOtherLimit_ServerValidate(source as CustomValidator, args, 3, !chkBurglary.Checked);
    }

    protected void valJewelry_ServerValidate(object source, ServerValidateEventArgs args)
    {
        valOtherLimit_ServerValidate(source as CustomValidator, args, 4, !chkJewelry.Checked);
    }

    protected void valMoney_ServerValidate(object source, ServerValidateEventArgs args)
    {
        valOtherLimit_ServerValidate(source as CustomValidator, args, 5, !chkMoney.Checked);
    }

    protected void valGlass_ServerValidate(object source, ServerValidateEventArgs args)
    {
        valOtherLimit_ServerValidate(source as CustomValidator, args, 6, !chkGlass.Checked);
    }

    protected void valElectronic_ServerValidate(object source, ServerValidateEventArgs args)
    {
        valOtherLimit_ServerValidate(source as CustomValidator, args, 7, !chkElectronic.Checked);
    }

    protected void valPersonal_ServerValidate(object source, ServerValidateEventArgs args)
    {
        valOtherLimit_ServerValidate(source as CustomValidator, args, 8, !chkPersonal.Checked);
    }


    private void valOtherLimit_ServerValidate(CustomValidator validator, ServerValidateEventArgs args, int id, bool emptyAllowed)
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
            var info = new
            {
                contentsLimit = decimal.Parse(txtContents.Text.Replace(",", "")),
                currency = int.Parse(ddlCurrency.SelectedValue)
            };
            decimal content = info.contentsLimit;
            int currency = info.currency;
            minimum = (decimal)limit.minimum;
            maximum = (decimal)limit.maximum;
            if (limit.percentage)
            {
                minimum *= (decimal)content / 100;
                maximum *= (decimal)content / 100;
            }
            else if (currency == ApplicationConstants.PESOS_CURRENCY)
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