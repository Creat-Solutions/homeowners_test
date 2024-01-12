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
using System.Collections.Generic;



public partial class UserControls_QuickQuoteStep2 : System.Web.UI.UserControl, IApplicationStep
{
    private string _title = "About the Insured Property";
    private int _selectedstate = 0;
    private int _selectedcity = 0;
    private int _selectedcolony = 0;
    private Regex regvalidator = new Regex("^[a-zA-Z0-9]+$");

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Page.IsPostBack && GetPostBackControl(Page) != txtHouseEstate) {
            LoadStep();
        }
    }

    public static Control GetPostBackControl(Page page)
    {
        Control control = null;

        string ctrlname = page.Request.Params.Get("__EVENTTARGET");
        if (ctrlname != null && ctrlname != string.Empty) {
            control = page.FindControl(ctrlname);
        } else {
            foreach (string ctl in page.Request.Form) {
                Control c = page.FindControl(ctl);
                if (c is System.Web.UI.WebControls.Button) {
                    control = c;
                    break;
                }
            }
        }
        return control;
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
        if (Session["quoteid"] == null)
            throw new Exception("Session[quoteid] is null");
        LoadData((int)Session["quoteid"]);
    }

    public bool SaveStep()
    {
        if (Page.IsValid) {
            Session["quoteid"] = getData();
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
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        insuredPersonHouse house = null;
        try {
            house = db.spList_V1_quotePersonHouse(key).Single();
        } catch {
        }
        if (house == null)
            return;
        txtStreet.Text = house.street;
        txtIntNumber.Text = house.InteriorNo;
        txtExtNumber.Text = house.exteriorNo;

        txtZipCode.Text = house.ZipCode;
        txtHouseEstate.Text = house.HouseEstate.Name;
        txtCity.Text = house.HouseEstate.City.Name;
        txtState.Text = house.HouseEstate.City.State.Name;

    }

    private List<HouseEstate> GetHouseEstateFromZipCode()
    {
        SearchParameter zipCode = new SearchParameter {
            Operator = SearchOperator.EQUALS,
            Property = "ZipCode",
            Value = txtZipCode.Text
        };
        var he = HouseEstate.Get(new List<SearchParameter>() { zipCode }, ApplicationConstants.ADMINDB);
        return he;
    }

    protected void txtZipCode_TextChanged(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtZipCode.Text.Trim())) {
            return;
        }

        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        var he = GetHouseEstateFromZipCode();
        lblHouseEstateNotFound.Visible = false;
        if (he.Count == 0) {
            lblHouseEstateNotFound.Visible = true;
            return;
        }

        if (he.Count == 1) {
            txtHouseEstate.Text = he[0].Name;
            txtCity.Text = he[0].City.Name;
            txtState.Text = he[0].City.State.Name;

            txtHouseEstate.Visible = true;
            txtCity.Visible = true;
            txtState.Visible = true;
            ddlHouseEstate.Visible = false;
            ddlCity.Visible = false;
            ddlState.Visible = false;
        } else {
            ddlHouseEstate.DataSource = he;
            ddlHouseEstate.DataBind();

            txtHouseEstate.Visible = false;
            ddlHouseEstate.Visible = true;

            /* multiple cities */
            if (he.Where(m => m.City.ID != he[0].City.ID).Count() > 0) {
                var cities = he.Select(m => m.City).ToList();
                ddlCity.DataSource = cities;
                ddlCity.DataBind();

                txtCity.Visible = false;
                ddlCity.Visible = true;

                /* multiple states */
                if (cities.Where(m => m.State.ID != cities[0].State.ID).Count() > 0) {
                    ddlState.DataSource = cities.Select(m => m.State);
                    ddlState.DataBind();

                    txtState.Visible = false;
                    ddlState.Visible = true;
                } else {
                    txtState.Text = cities[0].State.Name;

                    txtState.Visible = true;
                    ddlState.Visible = false;
                }
            } else {
                txtCity.Text = he[0].City.Name;
                txtState.Text = he[0].City.State.Name;

                txtState.Visible = true;
                ddlState.Visible = false;
                txtCity.Visible = true;
                ddlCity.Visible = false;
            }


        }
    }


    private int getData()
    {
        if (Session["quoteid"] == null) {
            throw new Exception("Session[quoteid] is null");
        }
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        int key = (int)Session["quoteid"];
        string intNo = null;
        string bank = null, loanNumber = null, bankAddress = null;
        var he = GetHouseEstateFromZipCode();
        int heID = ddlHouseEstate.Visible ? int.Parse(ddlHouseEstate.SelectedValue) : he[0].ID;
        if (!string.IsNullOrEmpty(txtIntNumber.Text)) {
            intNo = txtIntNumber.Text.Trim();
        }

        var taxPercentage = he[0].City.TaxPercentage;
        return db.spUpd_V1_quotePersonHouse(
            key, 
            txtStreet.Text, 
            intNo, 
            txtExtNumber.Text, 
            heID,
            taxPercentage,
            bank,
            loanNumber, 
            bankAddress
        );
    }



    /*
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
    */
}
