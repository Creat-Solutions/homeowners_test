using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ME.Admon;

public partial class UserControls_GenerateStep2 : System.Web.UI.UserControl, IApplicationStep
{
    private void LoadData(int key)
    {
        if (key <= 0)
        {
            throw new Exception("Session[quoteid] <= 0");
        }
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        try
        {
            var q = db.quotes.SingleOrDefault(m => m.id == key && m.company == ApplicationConstants.COMPANY);
            if (q == null)
            {
                return;
            }
            HouseEstate he = q.insuredPersonHouse1.HouseEstate;
            var codigos = db.spList_Calcula(q.id, he.City.HMP).Single();

            ddlType.SelectedValue = q.typeOfProperty.ToString();
            ddlRoof.SelectedValue = q.roofType.ToString();
            ddlWall.SelectedValue = q.wallType.ToString();
            txtStories.Text = q.stories.ToString();
            txtUnderground.Text = q.underground.ToString() == "0" ? string.Empty : q.underground.ToString();

            if (q.shutters != null)
            {
                rblShutters.SelectedValue = (bool)q.shutters ? "1" : "0";
            }
            else
            {
                rblShutters.SelectedValue = "0";
            }

            if (q.adjoiningEstates != null)
            {
                rblAdjoining.SelectedValue = (bool)q.adjoiningEstates ? "1" : "0";
            }
            else
            {
                rblAdjoining.SelectedValue = "0";
            }

            if (q.distanceFromSea != null)
            {
                rblDistanceSea.SelectedValue = q.distanceFromSea.ToString();
            }

            if (q.guards24 != null)
                chk24Guards.Checked = (bool)q.guards24;
            if (q.centralAlarm != null)
                chkCentralAlarm.Checked = (bool)q.centralAlarm;
            if (q.localAlarm != null)
                chkLocalAlarm.Checked = (bool)q.localAlarm;
            if (q.tvCircuit != null)
                chkTVCircuit.Checked = (bool)q.tvCircuit;
            if (q.tvGuard != null)
                chkTVGuard.Checked = (bool)q.tvGuard;
        }
        catch
        {

        }
    }

    public bool SaveStep()
    {
        if (Page.IsValid)
        {
            Session["quoteid"] = getData();
            return true;
        }
        return false;
    }

    public void LoadStep()
    {
        if (Session["quoteid"] == null)
            throw new Exception("Session[quoteid] is null");
        LoadData((int)Session["quoteid"]);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string Title()
    {
        return GetLocalResourceObject("strPropertyInfo").ToString();
    }

    protected void ddlWall_Validate(object sender, ServerValidateEventArgs e)
    {
        try
        {
            int selectedWallType = int.Parse(ddlWall.SelectedValue);
            int selectedRoofType = int.Parse(ddlRoof.SelectedValue);
            if (selectedRoofType == ApplicationConstants.CONCRETE_ROOFTYPE)
            {
                e.IsValid = selectedWallType == ApplicationConstants.CONCRETE_WALLTYPE;
            }
            else
            {
                e.IsValid = true;
            }
        }
        catch
        {
            e.IsValid = false;
        }
    }

    protected void txtStories_Validate(object sender, ServerValidateEventArgs e)
    {
        try
        {
            int numberStories = int.Parse(e.Value);
            int typeOfProperty = int.Parse(ddlType.SelectedValue);
            if (typeOfProperty == ApplicationConstants.CONDO_TYPE)
            {
                e.IsValid = numberStories >= 2;
            }
            else
            {
                e.IsValid = numberStories >= 1;
            }
        }
        catch
        {
            e.IsValid = false;
        }
    }

    protected void ddlDataBound(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;
        ListItem item = new ListItem("---", "0");
        ddl.Items.Insert(0, item);
    }

    int getData()
    {
        if (Session["quoteid"] == null)
                    throw new Exception("Session[quoteid] is null");
        int key = (int)Session["quoteid"];
        if (key <= 0)
        {
            throw new Exception("Session[quoteid] <= 0");
        }
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        try
        {
            var q = db.quotes.SingleOrDefault(m => m.id == key && m.company == ApplicationConstants.COMPANY);
            if (q == null)
            {
                return 0;
            }
            q.typeOfProperty = int.Parse(ddlType.SelectedValue);
            q.roofType = int.Parse(ddlRoof.SelectedValue);
            q.wallType = int.Parse(ddlWall.SelectedValue);
            q.stories = int.Parse(txtStories.Text.Trim());
            q.underground = string.IsNullOrEmpty(txtUnderground.Text.Trim()) ? 0 : int.Parse(txtUnderground.Text);

            q.shutters = rblShutters.SelectedValue == "1";
            q.adjoiningEstates = rblAdjoining.SelectedValue == "1";

            q.distanceFromSea = int.Parse(rblDistanceSea.SelectedValue);

            q.guards24 = chk24Guards.Checked;
            q.centralAlarm = chkCentralAlarm.Checked;
            q.tvCircuit = chkTVCircuit.Checked;
            q.tvGuard = chkTVGuard.Checked;
            q.localAlarm = chkLocalAlarm.Checked;

            db.SubmitChanges();
            
            HouseEstate he = q.insuredPersonHouse1.HouseEstate;
            db.spUpd_quote(key, he.TEV, he.City.HMP, he.City.State.TheftZone[0]);
        }
        catch { }
        return key;
    }
}
