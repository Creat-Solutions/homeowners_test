using System;
using System.Collections;
using System.Configuration;
using System.Text.RegularExpressions;
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
using Protec.Authentication.Membership;



public partial class UserControls_ApplicationStep2 : System.Web.UI.UserControl, IPolicyLoadable
{
    private string _title = "About the Insured Property";
    private int _selectedstate = 0;
    private int _selectedcity = 0;
    private int _selectedcolony = 0;
    private Regex regvalidator = new Regex("^[a-zA-Z0-9]+$");

    private bool m_loadFromPolicy = false;

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

    protected void Page_Load(object sender, EventArgs e)
    {
        
        
    }

    protected void valBankInformation_Validate(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = true;
        if (txtBankName.Text.Trim().Length > 0) {
            e.IsValid = e.Value.Trim().Length > 0;
        }
    }

    public void txtLoanNumber_Validate(object sender, ServerValidateEventArgs e)
    {
        if (e.Value.Length == 0 || regvalidator.IsMatch(e.Value))
            e.IsValid = true;
        else
            e.IsValid = false;
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
                throw new Exception("Session[quoteid] is null");
            }
            return (int)Session["quoteid"];
        }
    }

    public bool SaveStep()
    {
        if (Page.IsValid) {
            try
            {
                int key = getData();
                if (!LoadFromPolicy)
                {
                    Session["quoteid"] = key;
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
        return _title;
    }

    private void LoadData(int key)
    {
        if (key <= 0)
            throw new Exception("Session[quoteid] <= 0");

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
        insuredPersonHouse house = null;
        try
        {
            house = db.spList_V1_quotePersonHouse(key).Single();
        }
        catch
        {
        }
        if (house == null)
            return;
        txtStreet.Text = house.street;
        txtIntNumber.Text = house.InteriorNo;
        txtExtNumber.Text = house.exteriorNo;
        if (house.bankName != null)
            txtBankName.Text = house.bankName;
        if (house.loanNumber != null)
            txtLoanNumber.Text = house.loanNumber;
        if (house.bankAddress != null)
            txtBankAddress.Text = house.bankAddress;
        /* process keys for state & city */
        _selectedcity = house.HouseEstate.City.ID;
        _selectedstate = house.HouseEstate.City.State.ID;
        _selectedcolony = house.HouseEstate.ID;
        txtZip.Text = house.ZipCode;
    }

    private void LoadDataFromPolicy(int key)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        insuredPersonHouse house = null;
        try
        {
            house = db.spList_V1_policyPersonHouse(key).Single();
        }
        catch
        {
        }
        if (house == null)
            return;
        txtStreet.Text = house.street;
        txtIntNumber.Text = house.InteriorNo;
        txtExtNumber.Text = house.exteriorNo;
        if (house.bankName != null)
            txtBankName.Text = house.bankName;
        if (house.loanNumber != null)
            txtLoanNumber.Text = house.loanNumber;
        if (house.bankAddress != null)
            txtBankAddress.Text = house.bankAddress;
        /* process keys for state & city */
        _selectedcity = house.HouseEstate.City.ID;
        _selectedstate = house.HouseEstate.City.State.ID;
        _selectedcolony = house.HouseEstate.ID;
        txtZip.Text = house.ZipCode;
        txtZip.Enabled = false;
        ddlCity.Enabled = false;
        ddlColony.Enabled = false;
        ddlState.Enabled = false;
    }
    private int getData()
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        int key = GetKey();
        
        string intNo = null;
        string bank = null, loanNumber = null, bankAddress = null;
        if (!string.IsNullOrEmpty(txtIntNumber.Text))
            intNo = txtIntNumber.Text.Trim();
        if (!string.IsNullOrEmpty(txtBankName.Text))
            bank = txtBankName.Text.Trim();
        if (!string.IsNullOrEmpty(txtLoanNumber.Text))
            loanNumber = txtLoanNumber.Text.Trim();
        if (!string.IsNullOrEmpty(txtBankAddress.Text))
            bankAddress = txtBankAddress.Text.Trim();
        
        if (LoadFromPolicy)
        {
            string changedStreet = string.Empty;
            string changedIntNo = string.Empty;
            string changedExtNo = string.Empty;
            string changedBankName = string.Empty;
            string changedLoanNumber = string.Empty;
            string changedBankAddress = string.Empty;
            string changeDescription = string.Empty;
            try
            {
                insuredPersonHouse oldHouse = db.spList_V1_policyPersonHouse(key).Single();
                db.spUpd_V1_policyPersonHouse(
                    key,
                    txtStreet.Text,
                    intNo,
                    txtExtNumber.Text,
                    bank,
                    loanNumber,
                    bankAddress
                );
            
                if (txtStreet.Text != oldHouse.street)
                {
                    changedStreet = "Changed street from " + oldHouse.street + " to " + txtStreet.Text + ". ";
                }
                if (intNo != oldHouse.InteriorNo)
                {
                    changedIntNo = "Changed int # from " + oldHouse.InteriorNo + " to " + intNo + ". ";
                }
                if (txtExtNumber.Text != oldHouse.exteriorNo)
                {
                    changedExtNo = "Changed ext # from " + oldHouse.exteriorNo + " to " + txtExtNumber.Text + ". ";
                }
                if (bank != oldHouse.bankName)
                {
                    changedBankName = "Changed bank from " + oldHouse.bankName + " to " + bank + ". ";
                }
                if (loanNumber != oldHouse.loanNumber)
                {
                    changedLoanNumber = "Changed loan # from " + oldHouse.loanNumber + " to " + loanNumber + ". ";
                }
                if (bankAddress != oldHouse.bankAddress)
                {
                    changedBankAddress = "Changed bank address from " + oldHouse.bankAddress + " to " + bankAddress + ". ";
                }
                changeDescription = "Changed Insured House details. " +
                    changedStreet +
                    changedExtNo +
                    changedIntNo +
                    changedBankName +
                    changedLoanNumber +
                    changedBankAddress +
                    "Does not affect premium.";
                if (changeDescription.Length > 128)
                {
                    changeDescription = changeDescription.Substring(0, 128);
                }
                string policyno = Session["policyno"] as string;
                int userid = ((UserInformation)HttpContext.Current.User.Identity).UserID;
                csPolicy.ChangePolicy(
                    policyno,
                    changeDescription,
                    userid,
                    ApplicationConstants.APPLICATION,
                    ApplicationConstants.ADMINDB
                );
            }
            catch
            {
                Response.Redirect("~/Application/EditPolicy/", true);
            }
            return 0;
        }
        else
        {
            int colony = int.Parse(ddlColony.SelectedValue);
            City city = new City(int.Parse(ddlCity.SelectedValue));
            return db.spUpd_V1_quotePersonHouse(
                key,
                txtStreet.Text,
                intNo,
                txtExtNumber.Text,
                colony,
                city.TaxPercentage,
                bank,
                loanNumber,
                bankAddress
            );
        }
    }

    protected void ddlDataBound(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;
        ListItem item = new ListItem("---", "0");
        ddl.Items.Insert(0, item);
        
        if (ddl == ddlState && _selectedstate > 0) {
            ddl.SelectedValue = _selectedstate.ToString();
        } else if (ddl == ddlCity && _selectedcity > 0) {
            ddl.SelectedValue = _selectedcity.ToString();
        } else if (ddl == ddlColony && _selectedcolony > 0) {
            ddl.SelectedValue = _selectedcolony.ToString();
        }
    }

    protected void ddlColony_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlColony.SelectedIndex == 0) {
            txtZip.Text = string.Empty;
            return;
        }
        _selectedcolony = ddlColony.SelectedIndex;
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        int keyHouse = int.Parse(ddlColony.SelectedValue);
        txtZip.Text = new HouseEstate(keyHouse).ZipCode;
    }

    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlState.SelectedIndex == 0) {
            ddlCity.SelectedIndex = 0;
        }
    }

    protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCity.SelectedIndex == 0) {
            ddlColony.SelectedIndex = 0;
        }
    }
}
