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
using System.Collections.Generic;
using Protec.Authentication.Membership;

public partial class UserControls_ApplicationStep5 : System.Web.UI.UserControl, IApplicationStep
{
    private string _title = "Values and Sublimits";

    protected void valOutdoor_Validate(object source, ServerValidateEventArgs args)
    {
        TextBox txtOutdoor;
        CustomValidator validator = source as CustomValidator;
        try {
            txtOutdoor = validator.FindControl(validator.ControlToValidate) as TextBox;
            if (txtOutdoor == null || chkOutdoor == null)
                throw new Exception();
        } catch {
            args.IsValid = false;
            return;
        }
        args.IsValid = !chkOutdoor.Checked || txtOutdoor.Text.Length > 0;
    }

    protected void valOtherStructuresDescription_Validate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = !chkOtherStructures.Checked || txtOtherStructuresDescription.Text.Length > 0;
    }

    protected void valBuilding_ServerValidate(object source, ServerValidateEventArgs args)
    {
        decimal otherStructures = 0;
        if (chkOtherStructures.Checked) {
            try {
                otherStructures = decimal.Parse(txtOtherStructures.Text.Replace(",", ""));
            } catch {
            }
        }
        valLimit_ServerValidate(source as CustomValidator, args, 1, !chkBuilding.Checked, otherStructures);
    }

    protected void valOtherStructures_ServerValidate(object source, ServerValidateEventArgs args)
    {
        decimal building = 0;
        if (chkBuilding.Checked) {
            try {
                building = decimal.Parse(txtBuilding.Text.Replace(",", ""));
            } catch {
            }
        }
        valLimit_ServerValidate(source as CustomValidator, args, 1, !chkOtherStructures.Checked, building, 0);
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
        valLimit_ServerValidate(validator, args, id, emptyAllowed, 0, null);
    }

    private void valLimit_ServerValidate(CustomValidator validator, ServerValidateEventArgs args, int id, bool emptyAllowed, decimal maxSubstract)
    {
        valLimit_ServerValidate(validator, args, id, emptyAllowed, maxSubstract, null);
    }

    private void valLimit_ServerValidate(CustomValidator validator, ServerValidateEventArgs args, int id, bool emptyAllowed, decimal maxSubstract, decimal? forcedMinimum)
    {
        decimal value;
        validator.Text = "Invalid value. ";

        if (string.IsNullOrEmpty(args.Value)) {
            args.IsValid = emptyAllowed;
            if (!emptyAllowed) {
                validator.Text = "Required";
            }
            return;
        }

        try {
            value = decimal.Parse(args.Value);
        } catch {
            args.IsValid = false;
            return;
        }

        if ((validator == valBuilding || validator == valContents) && chkLimits.Checked) {
            args.IsValid = true;
            return;
        }

        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        decimal minimum, maximum;
        try {
            var limit = db.valueSublimits.Where(m => m.company == ApplicationConstants.COMPANY).Select(m => new { m.id, m.minimum, m.maximum, m.percentage }).Single(m => m.id == id);
            if (limit.percentage) {
                TextBox building = txtBuilding; 
                TextBox content = txtContents;

                decimal buildingValue = 0;
                try {
                    buildingValue = string.IsNullOrEmpty(building.Text) ? 0M : decimal.Parse(building.Text);
                } catch {
                    buildingValue = 0;
                }

                decimal sum = buildingValue + decimal.Parse(content.Text);

                if (validator == valDebri || validator == valExtra) {
                    minimum = sum * limit.minimum / 100;
                    maximum = sum * limit.maximum / 100;
                } else {
                    throw new Exception();
                }
            } else {
                minimum = limit.minimum;
                maximum = limit.maximum;
                //distinto a Dolares
                if (ddlCurrency.SelectedValue != "2") {
                    UserInformation user = (UserInformation)HttpContext.Current.User.Identity;
                    decimal tipoCambio = user.ExchangeRate;
                    minimum *= tipoCambio;
                    maximum *= tipoCambio;
                }
            }
        } catch {
            args.IsValid = false;
            return;
        }
        maximum -= maxSubstract;
        if (forcedMinimum != null) {
            minimum = forcedMinimum.Value;
        }

        bool valid = value >= minimum && value <= maximum;
        if (!valid) {
            validator.Text += "Minimum: " + minimum.ToString("C") + ", Maximum: " + maximum.ToString("C");
        }
        
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
        if (percentage) {
            if (sum == 0) {
                minStr = (min / 100).ToString("P");
                maxStr = (max / 100).ToString("P");
            } else {
                minStr = (sum * min / 100).ToString("C");
                maxStr = (sum * max / 100).ToString("C");
            }
        } else {
            minStr = min.ToString("C");
            maxStr = max.ToString("C");
        }
        minimum.InnerHtml = minStr;
        maximum.InnerHtml = maxStr;
    }

    private void LoadTable(HouseInsuranceDataContext db, decimal buildingLimit, decimal otherStructuresLimit, decimal contentsLimit, int currency)
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
        var sum = buildingLimit + contentsLimit;
        bool pesos = currency == ApplicationConstants.PESOS_CURRENCY;
        for (int i = 0; i < 6; i++) {
            decimal minimum = limits[i].minimum;
            decimal maximum = limits[i].maximum;
            if (limits[i].percentage) {
                Page.ClientScript.RegisterArrayDeclaration("percentageMinimum", minimum.ToString("F2"));
                Page.ClientScript.RegisterArrayDeclaration("percentageMaximum", maximum.ToString("F2"));
            } else {
                Page.ClientScript.RegisterArrayDeclaration("valuesMinimum", minimum.ToString("F2"));
                Page.ClientScript.RegisterArrayDeclaration("valuesMaximum", maximum.ToString("F2"));
                if (pesos) {
                    minimum *= tipoCambio;
                    maximum *= tipoCambio;
                    Page.ClientScript.RegisterArrayDeclaration("valuesPesosMinimum", minimum.ToString("F2"));
                    Page.ClientScript.RegisterArrayDeclaration("valuesPesosMaximum", maximum.ToString("F2"));
                } else {
                    Page.ClientScript.RegisterArrayDeclaration("valuesPesosMinimum", (minimum * tipoCambio).ToString("F2"));
                    Page.ClientScript.RegisterArrayDeclaration("valuesPesosMaximum", (maximum * tipoCambio).ToString("F2"));
                }
            }
            if (i == 0) {
                // building limit
                LoadLimitValues(tdMinimum[i], tdMaximum[i], minimum, maximum - otherStructuresLimit, false, 0);
                LoadLimitValues(tdOtherStructuresMinimum, tdOtherStructuresMaximum, 0, maximum - buildingLimit, false, 0);
            } else {
                LoadLimitValues(tdMinimum[i], tdMaximum[i], minimum, maximum, limits[i].percentage, sum);
            }
            LoadLimitName(lblEnglish[i], lblSpanish[i], limits[i].name, limits[i].nameEs);

        }
    }

    private void LoadTextbox(TextBox txtLimit, decimal value, CheckBox chkLimit)
    {
        if (value == 0) {
            txtLimit.Enabled = chkLimit.Checked = false;
            txtLimit.Text = string.Empty;
        } else {
            txtLimit.Enabled = chkLimit.Checked = true;
            txtLimit.Text = value.ToString("N");
        }
    }

    private void LoadCurrency(int currency)
    {
        if (currency != 2 && currency != 1) {
            ddlCurrency.SelectedValue = ApplicationConstants.DOLLARS_CURRENCY.ToString();
            return;
        }
        ddlCurrency.SelectedValue = currency.ToString();
    }

    public void LoadStep()
    {
        int key = (int)Session["quoteid"];
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        var info = db.quotes.Select(m => new {
            m.id,
            m.buildingLimit,
            m.outdoorLimit,
            m.contentsLimit,
            m.debriLimit,
            m.extraLimit,
            m.lossOfRentsLimit,
            m.outdoorDescription,
            m.fireAll,
            m.quake,
            m.HMP,
            m.limitsEnabled,
            m.currency,
            m.otherStructuresLimit,
            m.otherStructuresDescription
        }).Single(m => m.id == key);

        LoadTable(db, info.buildingLimit, info.otherStructuresLimit, info.contentsLimit, info.currency);

        LoadCurrency(info.currency);

        LoadTextbox(txtBuilding, info.buildingLimit, chkBuilding);

        if (info.outdoorLimit == 0) {
            txtOutdoorDescription.Enabled = false;
            txtOutdoorDescription.Text = string.Empty;
        } else {
            txtOutdoorDescription.Enabled = true;
            txtOutdoorDescription.Text = info.outdoorDescription;
        }

        if (info.otherStructuresLimit == 0) {
            txtOtherStructuresDescription.Enabled = false;
            txtOtherStructuresDescription.Text = string.Empty;
        } else {
            txtOtherStructuresDescription.Enabled = true;
            txtOtherStructuresDescription.Text = info.otherStructuresDescription;
        }

        txtContents.Text = info.contentsLimit.ToString("N");

        
        txtDebri.Text = info.debriLimit.ToString("N");
        txtExtra.Text = info.extraLimit.ToString("N");
        LoadTextbox(txtLossRents, info.lossOfRentsLimit, chkLossRents);
        LoadTextbox(txtOtherStructures, info.otherStructuresLimit, chkOtherStructures);
        UserInformation userInfo = (UserInformation)Page.User.Identity;
        bool isAdministrator = userInfo.UserType == SecurityManager.ADMINISTRATOR;
        bool enabledHMP = db.spIsValidHMPDate() == 1 || isAdministrator;
        chkHMP.Enabled = enabledHMP;
        lblInactiveHMP.Visible = !enabledHMP;
        //if (info.fireAll != null)
        //    chkFire.Checked = (bool)info.fireAll;
        if (info.quake != null)
            chkQuake.Checked = (bool)info.quake;
        if (info.HMP != null && enabledHMP) {
            chkHMP.Checked = (bool)info.HMP;
        } else if (!enabledHMP) {
            chkHMP.Checked = false;
        }

        if (chkHMP.Checked) {
            LoadTextbox(txtOutdoor, info.outdoorLimit, chkOutdoor);
            chkOutdoor.Enabled = true;
        } else {
            LoadTextbox(txtOutdoor, 0, chkOutdoor);
            chkOutdoor.Enabled = false;
        }

        chkLimits.Visible = userInfo.Agent == ApplicationConstants.MEAGENT || userInfo.UserID == ApplicationConstants.PTI_USER;
        chkLimits.Checked = !info.limitsEnabled;
        if (!info.limitsEnabled) {
            Page.ClientScript.RegisterStartupScript(GetType(), "BoolLimits", "ToggleValidators();", true);
        }
        string dlls = info.currency == 2 ? "true" : "false";

        if ((chkHMP.Checked || chkQuake.Checked) && chkLossRents.Checked)
        {
            lossRentsWarning.Style.Add(HtmlTextWriterStyle.Display, "inline");
        }
        else
        {
            lossRentsWarning.Style.Add(HtmlTextWriterStyle.Display, "none");
        }
        Page.ClientScript.RegisterStartupScript(GetType(), "BoolCurrencyDollars", "StartCurrency(" + dlls + ");", true);
        
    }

    public bool SaveStep()
    {
        if (Page.IsValid) {
            try {
                saveData((int)Session["quoteid"]);
            } catch {
                return false;
            }
            return true;
        }
        return false;
    }

    public void Page_Load(object sender, EventArgs e)
    {
        UserInformation info = (UserInformation)Page.User.Identity;
        divLimits.Visible = info.Agent == ApplicationConstants.MEAGENT || info.UserType == SecurityManager.ADMINISTRATOR || info.UserID == ApplicationConstants.PTI_USER;
    }

    public string Title()
    {
        return _title;
    }

    private decimal GetLimitData(TextBox txtLimit)
    {
        try {
            return string.IsNullOrEmpty(txtLimit.Text) ? 0 : decimal.Parse(txtLimit.Text);
        } catch {
            return 0;
        }
    }

    private void saveData(int key)
    {
        decimal buildingLimit = GetLimitData(txtBuilding);
        decimal outdoorLimit = chkHMP.Checked ? GetLimitData(txtOutdoor) : 0;
        decimal contentsLimit = GetLimitData(txtContents);
        decimal debriLimit = GetLimitData(txtDebri);
        decimal extraLimit = GetLimitData(txtExtra);
        decimal lossOfRentsLimit = GetLimitData(txtLossRents);
        var otherStructuresLimit = GetLimitData(txtOtherStructures);

        string description = null;
        if (outdoorLimit > 0) {
            description = txtOutdoorDescription.Text.Trim();
        }

        string otherStructuresDescription = null;
        if (otherStructuresLimit > 0) {
            otherStructuresDescription = txtOtherStructuresDescription.Text.Trim();
        }

        //bool fireAll = chkFire.Checked;
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();

        bool fireAll = true;
        UserInformation userInfo = (UserInformation)Page.User.Identity;
        bool isAdministrator = userInfo.UserType == SecurityManager.ADMINISTRATOR;
        bool enabledHMP = db.spIsValidHMPDate() == 1 || isAdministrator;

        bool quake = chkQuake.Checked, HMP = enabledHMP && chkHMP.Checked;
        bool limitsEnabled = userInfo.Agent != ApplicationConstants.MEAGENT || !chkLimits.Checked || userInfo.UserID != ApplicationConstants.PTI_USER;

        int tipoCambio = int.Parse(ddlCurrency.SelectedValue);

        db.spUpd_V1_quoteLimits(
            key, 
            buildingLimit, 
            outdoorLimit, 
            contentsLimit, 
            debriLimit, 
            extraLimit, 
            lossOfRentsLimit, 
            otherStructuresLimit,
            description, 
            otherStructuresDescription,
            fireAll, 
            quake, 
            HMP, 
            limitsEnabled, 
            tipoCambio
        );
    }

}
