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



public partial class UserControls_ApplicationStep4 : System.Web.UI.UserControl, IApplicationStep
{
    private string _title = "Additional Security Information";
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
                m.distanceFromSea,
                m.shutters,
                m.guards24,
                m.centralAlarm,
                m.localAlarm,
                m.tvCircuit,
                m.tvGuard
            }).Single(m => m.id == key);
            if (info == null)
                throw new Exception("info == null");
            if (info.shutters != null)
                rblShutters.SelectedValue = (bool)info.shutters ? "1" : "0";
            else
                rblShutters.SelectedValue = "0";
            if (info.distanceFromSea != null)
                rblDistanceSea.SelectedValue = info.distanceFromSea.ToString();
            if (info.guards24 != null)
                chk24Guards.Checked = (bool)info.guards24;
            if (info.centralAlarm != null)
                chkCentralAlarm.Checked = (bool)info.centralAlarm;
            if (info.localAlarm != null)
                chkLocalAlarm.Checked = (bool)info.localAlarm;
            if (info.tvCircuit != null)
                chkTVCircuit.Checked = (bool)info.tvCircuit;
            if (info.tvGuard != null)
                chkTVGuard.Checked = (bool)info.tvGuard;
        } catch {
        }
        
    }

    private int getData()
    {
        if (Session["quoteid"] == null)
            throw new Exception("Session[quoteid] is null");
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        int id = (int)Session["quoteid"];
        db.spUpd_v1_addSecurityInfo(id, int.Parse(rblDistanceSea.SelectedValue),
                                    rblShutters.SelectedValue == "1", chk24Guards.Checked,
                                    chkCentralAlarm.Checked, chkLocalAlarm.Checked,
                                    chkTVCircuit.Checked, chkTVGuard.Checked);
        return id;
    }


}
