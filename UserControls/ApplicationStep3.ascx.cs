using System;
using System.Collections;
using System.Collections.Generic;
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



public partial class UserControls_ApplicationStep3 : System.Web.UI.UserControl, IApplicationStep
{
    private string _title = "Additional Property Information";
    private Hashtable _fsecurities = null;

    protected void Page_Load(object sender, EventArgs e)
    {

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
        
        try {
            var info = db.quotes.Select(m => new {
                m.id,
                m.typeOfClient,
                m.useOfProperty,
                m.roofType,
                m.wallType,
                m.adjoiningEstates,
                m.typeOfProperty,
                m.stories,
                m.underground
            }).Single(m => m.id == key);
            if (info == null)
                throw new Exception("info == null");
            if (info.typeOfClient != null) {
                ddlClient.SelectedValue = info.typeOfClient.ToString();
            }
            if (info.useOfProperty != null) {
                ddlUse.SelectedValue = info.useOfProperty.ToString();
            }
            if (info.typeOfProperty != null) {
                ddlType.SelectedValue = info.typeOfProperty.ToString();
            }
            if (info.roofType != null) {
                ddlRoof.SelectedValue = info.roofType.ToString();
            }
            if (info.wallType != null) {
                ddlWall.SelectedValue = info.wallType.ToString();
            }
            if (info.adjoiningEstates != null) {
                rbnAdjNo.Checked = !(bool)info.adjoiningEstates;
                rbnAdjYes.Checked = (bool)info.adjoiningEstates;
            } else {
                rbnAdjYes.Checked = true;
            }
            if (info.stories != null) {
                txtStories.Text = info.stories.ToString();
            } else {
                txtStories.Text = string.Empty;
            }
            if (info.underground != null) {
                txtUnderground.Text = info.underground.ToString();
            } else {
                txtUnderground.Text = string.Empty;
            }
            
            //info = db.additionalPropertyInformations.Single(m => m.id == (int)key);
        } catch {
        }
    }

    private int getData()
    {
        if (Session["quoteid"] == null)
            throw new Exception("Session[quoteid] is null");
        
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        int id = (int)Session["quoteid"];
        bool adjoining = rbnAdjYes.Checked;
        db.spUpd_v1_addPropInfo(
            id,
            int.Parse(ddlRoof.SelectedValue),
            int.Parse(ddlWall.SelectedValue),
            int.Parse(ddlClient.SelectedValue),
            int.Parse(ddlUse.SelectedValue),
            int.Parse(ddlType.SelectedValue),
            adjoining,
            int.Parse(txtStories.Text),
            int.Parse(txtUnderground.Text)
        );
        return id;
    }

    protected override void OnPreRender(EventArgs e)
    {
       
    }

    protected void ddlWall_Validate(object sender, ServerValidateEventArgs e)
    {
        try {
            int selectedWallType = int.Parse(ddlWall.SelectedValue);
            int selectedRoofType = int.Parse(ddlRoof.SelectedValue);
            if (selectedRoofType == ApplicationConstants.CONCRETE_ROOFTYPE) {
                e.IsValid = selectedWallType == ApplicationConstants.CONCRETE_WALLTYPE;
            } else {
                e.IsValid = true;
            }
        } catch {
            e.IsValid = false;
        }
    }

    protected void txtStories_Validate(object sender, ServerValidateEventArgs e)
    {
        try {
            int numberStories = int.Parse(e.Value);
            int typeOfProperty = int.Parse(ddlType.SelectedValue);
            if (typeOfProperty == ApplicationConstants.CONDO_TYPE) {
                e.IsValid = numberStories >= 2;
            } else {
                e.IsValid = numberStories >= 1;
            }
        } catch {
            e.IsValid = false;
        }
    }

    protected void ddlDataBound(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;
        ListItem item = new ListItem("---", "0");
        ddl.Items.Insert(0, item);
    }

    protected void cblDataBound(object sender, EventArgs e)
    {
        CheckBoxList list = sender as CheckBoxList;
        if (_fsecurities == null)
            return;
        foreach (ListItem item in list.Items) {
            bool? value = (bool ?)_fsecurities[int.Parse(item.Value)];
            if (value != null && (bool)value) {
                item.Selected = true;
            }
        }
    }
}
