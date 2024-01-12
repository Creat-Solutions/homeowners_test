using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Protec.Authentication.Membership;
using System.Text.RegularExpressions;
using ME.Admon;

public partial class UserControls_GenerateStep1 : System.Web.UI.UserControl, IPolicyLoadable
{
    private bool m_loadFromPolicy = false;
    protected int QuoteID { get; set; }
    protected decimal BUILDING_MIN = 0;
    protected decimal BUILDING_MAX = 0;
    protected decimal CONTENTS_MIN = 0;
    protected decimal CONTENTS_MAX = 0;
    protected decimal EXCHANGE_RATE = 0;

    public bool LoadFromPolicy
    {
        get
        {
            return m_loadFromPolicy;
        }
        set
        {
            m_loadFromPolicy = value;
        }
    }


    private int GetKey()
    {
        if (LoadFromPolicy)
        {
            return (int)Session["policyid"];
        }
        else
        {
            if (Session["quoteid"] == null)
            {
                return 0;
            }
            return (int)Session["quoteid"];
        }
    }

    public void LoadStep()
    {
        int key;
        try
        {
            key = GetKey();
        }
        catch
        {
            return;
        }
        LoadData(key);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        getExchangeRate();
        getBuildingLimits();
    }

    public static Control GetPostBackControl(Page page)
    {
        Control control = null;

        string ctrlname = page.Request.Params.Get("__EVENTTARGET");
        if (ctrlname != null && ctrlname != string.Empty)
        {
            control = page.FindControl(ctrlname);
        }
        else
        {
            foreach (string ctl in page.Request.Form)
            {
                Control c = page.FindControl(ctl);
                if (c is System.Web.UI.WebControls.Button)
                {
                    control = c;
                    break;
                }
            }
        }
        return control;
    }

    public bool SaveStep()
    {
        if (Page.IsValid && !string.IsNullOrEmpty(hfHouseEstate.Value))
        {
            try
            {
                int key = getData();
                if (!LoadFromPolicy)
                {
                    Session["quoteid"] = key;
                    Session["partial140"] = null;
                }
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
        return GetLocalResourceObject("strInitialInfo").ToString();
    }

    private void LoadData(int key)
    {
        if (key <= 0)
        {
            return;
        }

        if (LoadFromPolicy)
        {
            LoadDataFromPolicy(key);
        }
        else
        {
            LoadDataFromQuote(key);
        }
    }

    private void LoadDataFromQuote(int key)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        var q = db.quotes.SingleOrDefault(m => m.id == key && m.company == ApplicationConstants.COMPANY);
        insuredPerson person = db.spList_V1_quotePerson(key).Single();
        insuredPersonHouse house = null;

        txtName.Text = person.firstname;
        if (person.legalPerson)
        {
            rbnCorporation.Checked = true;
            txtLastName.Text = string.Empty;
        }
        else
        {
            rbnIndividual.Checked = true;
            txtLastName.Text = person.lastName;
            txtLastName2.Text = person.lastName2;
        }
        if (!string.IsNullOrEmpty(person.email))
        {
            txtEmail.Text = person.email;
        }
        if (!string.IsNullOrEmpty(person.phone) && person.phone.Length > 6)
        {
            txtAreaPhone.Text = person.phone.Substring(0, 3);
            txtPhone1.Text = person.phone.Substring(3, 3);
            txtPhone2.Text = person.phone.Substring(6);
        }
        ddlClient.SelectedValue = q.clientType.type == ApplicationConstants.MORTGAGE_TYPE ? ApplicationConstants.OWNER_TYPE.ToString() : q.clientType.type.ToString();
        if (q.clientType.type == ApplicationConstants.RENTER_TYPE)
        {
            txtValue.Text = string.Empty;
            txtContents.Text = q.contentsLimit.ToString("0.####");
        }
        else
        {
            txtValue.Text = q.buildingLimit.ToString("0.####");
            txtContents.Text = q.contentsLimit.ToString("0.####");
        }
        if (q.currency == ApplicationConstants.DOLLARS_CURRENCY)
        {
            rbDollars.Checked = true;
        }
        else
        {
            rbPesos.Checked = true;
        }
        ddlUse.SelectedValue = q.useOfProperty == ApplicationConstants.WEEKEND_USE ? ApplicationConstants.VACATION_USE.ToString() : q.useOfProperty.ToString();
        try
        {
            house = db.spList_V1_quotePersonHouse(key).Single();
        }
        catch
        {
        }
        if (house == null)
            return;

        txtZipCode.Text = house.ZipCode;
        hfHouseEstate.Value = house.houseEstate.ToString();
    }

    private void LoadDataFromPolicy(int key)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        var p = db.policies.SingleOrDefault(m => m.id == key && m.company == ApplicationConstants.COMPANY);
        insuredPerson person = db.spList_V1_policyPerson(key).Single();
        insuredPersonHouse house = null;

        txtName.Text = person.firstname;
        if (person.legalPerson)
        {
            txtLastName.Text = string.Empty;
        }
        else
        {
            txtLastName.Text = person.lastName;
            txtLastName2.Text = person.lastName2;
        }
        if (!string.IsNullOrEmpty(person.email))
        {
            txtEmail.Text = person.email;
        }
        if (!string.IsNullOrEmpty(person.phone) && person.phone.Length > 6)
        {
            txtAreaPhone.Text = person.phone.Substring(0, 3);
            txtPhone1.Text = person.phone.Substring(3, 3);
            txtPhone2.Text = person.phone.Substring(6);
        }

        ddlClient.SelectedValue = p.clientType.type == ApplicationConstants.MORTGAGE_TYPE ? ApplicationConstants.OWNER_TYPE.ToString() : p.clientType.type.ToString(); p.clientType.ToString();
        txtValue.Text = p.buildingLimit.ToString();
        txtContents.Text = p.contentsLimit.ToString();
        ddlUse.SelectedValue = p.useOfProperty == ApplicationConstants.WEEKEND_USE ? ApplicationConstants.VACATION_USE.ToString() : p.useOfProperty.ToString();
        try
        {
            house = db.spList_V1_policyPersonHouse(key).Single();
        }
        catch
        {
        }
        if (house == null)
            return;

        txtZipCode.Text = house.ZipCode;
        hfHouseEstate.Value = house.houseEstate.ToString();
    }

    private int getData()
    {
        int personType = 0;
        int currency = 0;
        decimal buildingAmount = 0, contentsAmount = 0;
        if (rbnIndividual.Checked)
        {
            personType = ApplicationConstants.PERSONA_FISICA;
        }
        else
        {
            personType = ApplicationConstants.PERSONA_MORAL;
        }
        string name = txtName.Text.Replace("&", " and ").Trim();
        string lastName = personType == ApplicationConstants.PERSONA_MORAL ? null : txtLastName.Text.Trim();
        string lastName2 = personType == ApplicationConstants.PERSONA_MORAL ? null : txtLastName2.Text.Trim();

        string email = txtEmail.Text.Trim();
        string phone = txtAreaPhone.Text.Trim() + txtPhone1.Text.Trim() + txtPhone2.Text.Trim();
        int typeOfClient = int.Parse(ddlClient.SelectedValue);
        try
        {
            buildingAmount = decimal.Parse(txtValue.Text.Trim());
        }
        catch
        {
            buildingAmount = 0;
        }
        try
        {
            contentsAmount = decimal.Parse(txtContents.Text.Trim());
        }
        catch
        {
            contentsAmount = 0;
        }
        int houseEstate = int.Parse(hfHouseEstate.Value);
        int typeOfProperty = ApplicationConstants.DEFAULT_PROPERTY_TYPE;
        int useOfProperty = int.Parse(ddlUse.SelectedValue);
        if (rbPesos.Checked)
        {
            currency = ApplicationConstants.PESOS_CURRENCY;
        }
        else
        {
            currency = ApplicationConstants.DOLLARS_CURRENCY;
        }
        UserInformation user = (UserInformation)HttpContext.Current.User.Identity;
        int userID = user.UserID;

        int key = GetKey();

        if (key == 0)
        {
            lastName = string.Join("|", lastName, lastName2);

            MapfreQuoter mapfre = new MapfreQuoter();
            return mapfre.GetQuote(
                personType,
                name,
                lastName,
                email,
                phone,
                buildingAmount,
                contentsAmount,
                houseEstate,
                typeOfClient,
                typeOfProperty,
                useOfProperty,
                currency,
                userID
            );
        }
        else
        {
            HouseInsuranceDataContext db = new HouseInsuranceDataContext();
            try
            {
                var package = db.packageServices.Single(m => m.id == 4);
                var q = db.quotes.SingleOrDefault(m => m.id == key && m.company == ApplicationConstants.COMPANY);
                var client = db.clientTypes.Single(m => m.type == typeOfClient);
                if (q == null)
                {
                    return 0;
                }
                q.insuredPerson1.legalPerson = personType == 1 ? true : false;
                q.insuredPerson1.firstname = name;
                q.insuredPerson1.lastName = lastName;
                q.insuredPerson1.lastName2 = lastName2;
                q.insuredPerson1.email = email;
                q.insuredPerson1.phone = phone;
                q.clientType = client;
                if (typeOfClient == ApplicationConstants.RENTER)
                {
                    q.buildingLimit = 0M;
                    q.contentsLimit = contentsAmount;
                }
                else
                {
                    q.buildingLimit = buildingAmount;
                    q.contentsLimit = contentsAmount;
                }

                q.currency = currency;
                q.useOfProperty = useOfProperty;
                q.insuredPersonHouse1.houseEstate = houseEstate;

                db.SubmitChanges();
                var locationData = AdminGateway.Gateway.GetLocationQuoteData(houseEstate);
                db.spUpd_quote(
                    key,
                    locationData.QuakeZone,
                    locationData.HMPZone,
                    locationData.TheftZone.ToCharArray()[0]
                );
            }
            catch { }
            return key;
        }
    }

    protected void ddlDataBound(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;

        ListItem item = new ListItem("---", "0");
        ddl.Items.Insert(0, item);
    }

    protected void txtLastName_ServerValidate(object sender, ServerValidateEventArgs e)
    {
        if (rbnIndividual.Checked)
        {
            e.IsValid = !string.IsNullOrEmpty(txtLastName.Text);
        }
        else
        {
            e.IsValid = true;
        }

    }

    protected void reqFields_ServerValidate(object sender, ServerValidateEventArgs args)
    {
        args.IsValid = txtEmail.Text.Trim().Length > 0
            || (txtAreaPhone.Text.Trim().Length >= 2
            && txtPhone1.Text.Trim().Length >= 3
            && txtPhone2.Text.Trim().Length == 4);
    }

    protected void txtValue_ServerValidate(object sender, ServerValidateEventArgs args)
    {

        decimal building_min = 0, building_max = 0, buildingValue = 0;
        int clientType = int.Parse(ddlClient.SelectedValue);

        building_min = BUILDING_MIN;
        building_max = BUILDING_MAX;

        if (!string.IsNullOrEmpty(txtValue.Text))
        {
            try
            {
                buildingValue = decimal.Parse(txtValue.Text);
            }
            catch
            {
                buildingValue = 0;
            }
        }

        if (!rbDollars.Checked)
        {
            building_min = building_min * EXCHANGE_RATE;
            building_max = building_max * EXCHANGE_RATE;
        }

        if ((clientType == ApplicationConstants.OWNER_TYPE && buildingValue > 0) || clientType == ApplicationConstants.LANDLORD_TYPE)
        {
            if (buildingValue < building_min || buildingValue > building_max)
            {
                valValue.Text = GetLocalResourceObject("strValValue").ToString() + building_min.ToString() + " - " + building_max.ToString();
                args.IsValid = false;
            }
        }
        else
        {
            args.IsValid = true;
        }
    }

    protected void txtContents_ServerValidate(object sender, ServerValidateEventArgs args)
    {
        decimal contents_min = 0, contents_max = 0, contentsValue = 0;
        int clientType = int.Parse(ddlClient.SelectedValue);

        contents_min = CONTENTS_MIN;
        contents_max = CONTENTS_MAX;

        if (!string.IsNullOrEmpty(txtContents.Text))
        {
            try
            {
                contentsValue = decimal.Parse(txtContents.Text);
            }
            catch
            {
                contentsValue = 0;
            }
        }

        if (!rbDollars.Checked)
        {
            contents_min = CONTENTS_MIN * EXCHANGE_RATE;
            contents_max = CONTENTS_MAX * EXCHANGE_RATE;
        }

        if (clientType == ApplicationConstants.OWNER_TYPE || clientType == ApplicationConstants.RENTER_TYPE)
        {
            if (contentsValue < contents_min || contentsValue > contents_max)
            {
                valContents.Text = GetLocalResourceObject("strValValue").ToString() + contents_min.ToString() + " - " + contents_max.ToString();
                args.IsValid = false;
            }
        }
        else
        {
            args.IsValid = true;
        }
    }

    protected void reqCurrency_ServerValidate(object sender, ServerValidateEventArgs args)
    {
        args.IsValid = rbDollars.Checked || rbPesos.Checked;
    }

    protected void reqPerson_ServerValidate(object sender, ServerValidateEventArgs args)
    {
        args.IsValid = rbnCorporation.Checked || rbnIndividual.Checked;
    }

    private void getBuildingLimits()
    {

        MapfreQuoter mapfre = new MapfreQuoter();
        AmountLimits limits = mapfre.GetAmountLimits();
        BUILDING_MIN = limits.BuildingMinimum;
        BUILDING_MAX = limits.BuildingMaximum;
        CONTENTS_MIN = limits.ContentsMinimum;
        CONTENTS_MAX = limits.ContentsMaximum;
    }

    private void getExchangeRate()
    {
        UserInformation user = (UserInformation)HttpContext.Current.User.Identity;
        EXCHANGE_RATE = user.ExchangeRate;
    }
}
