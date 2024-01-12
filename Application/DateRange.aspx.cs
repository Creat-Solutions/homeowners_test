using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.SessionState;
using Protec.Authentication.Membership;
using ME.Admon;

public partial class Application_DateRange : System.Web.UI.Page
{
    protected int RETROACTIVE_DAYS;
    protected void Page_PreRender(object sender, EventArgs e)
    {
        
    }

    protected void valStart_Validate(object sender, ServerValidateEventArgs e)
    {
        try {
            DateTime timeSelected = DateTime.Parse(e.Value);
            int agent = ((UserInformation)User.Identity).Agent;
            e.IsValid = timeSelected >= DateTime.Now.AddDays(-RETROACTIVE_DAYS);         
        } catch {
            e.IsValid = false;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["quoteidBuy"] == null) {
            Response.Redirect("~/Application/Load.aspx?option=buyquote", true);
            return;
        }

        int quoteid = (int)Session["quoteidBuy"];
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        var quote = db.quotes.SingleOrDefault(m => m.id == quoteid);
        if (quote == null) {
            Response.Redirect("~/Application/Load.aspx?option=buyquote", true);
            return;
        }

        int userid = ((UserInformation)HttpContext.Current.User.Identity).UserID;
        UserInformationDB userDB = AdminGateway.Gateway.GetUserInformation(userid);
        RETROACTIVE_DAYS = userDB.RetroactivePolicyDays;
        if (AdminGateway.Gateway.MustPayWithCC(userid) && 
            quote.currency == ApplicationConstants.PESOS_CURRENCY) {
            divNoPesos.Visible = true;
            divNoPesosIssue.Visible = false;
        } else if (quote.currency == ApplicationConstants.PESOS_CURRENCY) {
            divNoPesosIssue.Visible = true;
            divNoPesos.Visible = false;
        } else {
            divNoPesos.Visible = divNoPesosIssue.Visible = false;
        }
    }

    [WebMethod]
    public static string SaveDateRange(string start)
    {
        try {
            DateTime startDate = DateTime.Parse(start);
            int agent = ((UserInformation)HttpContext.Current.User.Identity).Agent;
            if (agent != ApplicationConstants.MEAGENT && startDate < DateTime.Today)
                return "0";
            startDate = startDate.AddHours(12); //12:00PM
            DateTime endDate = startDate.AddYears(1).AddSeconds(-1);
            HouseInsuranceDataContext db = new HouseInsuranceDataContext();
            HttpSessionState Session = HttpContext.Current.Session;
            if (Session["quoteidBuy"] == null)
                return "0";
            int quoteid = (int)Session["quoteidBuy"];
            var house = db.quotes.Single(m => m.id == quoteid).insuredPersonHouse1;
            HouseEstate he = house.HouseEstate;
            db.spUpd_DateRangeOfQuote(quoteid, startDate, endDate, he.TEV, he.City.HMP, he.City.State.TheftZone[0]);
            return endDate.ToString("MM/dd/yyyy HH:mm");
        } catch {
            return "0";
        }
    }
}
