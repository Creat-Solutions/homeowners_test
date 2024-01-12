<%@ WebHandler Language="C#" Class="GenerateQuote" %>

using System;
using System.Web;
using System.Linq;
using Protec.Authentication.Membership;
using System.Web.SessionState;
using ME.Admon;

public class GenerateQuote : IHttpHandler, IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) {

        int quoteid = 0;

        try {
            int policyid = int.Parse(context.Request.QueryString["id"]);
            UserInformation info = (UserInformation)HttpContext.Current.User.Identity;
            if (policyid > 0) {
                HouseInsuranceDataContext db = new HouseInsuranceDataContext();
                var policy = db.policies.Single(m => m.id == policyid);
                int issuedBy = policy.issuedBy;
                bool allowed = true;
                if (info.UserType != SecurityManager.ADMINISTRATOR) {
                    if (info.UserType == SecurityManager.ADMINAG) {
                        allowed = info.ListUsersInAgent().Contains(issuedBy);
                    } else {
                        allowed = info.Agent == ApplicationConstants.MEAGENT || issuedBy == info.UserID || info.UserID == ApplicationConstants.PTI_USER;
                    }
                }
                if (allowed) {
                    var he = policy.insuredPersonHouse1.HouseEstate;
                    quoteid = db.spConvertToQuote(policyid, he.TEV, he.City.HMP, he.City.State.TheftZone[0]);
                    policy.quoteRenewal = quoteid;
                    var quote = db.quotes.Single(m => m.id == quoteid);
                    if (quote.taxPercentage == null) {
                        quote.taxPercentage = quote.insuredPersonHouse1.HouseEstate.City.TaxPercentage;
                    }
                    db.SubmitChanges();
                    
                }
            }
        } catch {
            quoteid = 0;
        }

        if (quoteid == 0) {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Invalid policy ID");
        } else {
            if (!string.IsNullOrEmpty(context.Request.QueryString["startDateExp"]) &&
                !string.IsNullOrEmpty(context.Request.QueryString["endDateExp"]) &&
                ValidDate(context.Request.QueryString["startDateExp"]) &&
                ValidDate(context.Request.QueryString["endDateExp"])) {
                context.Session["startDateExp"] = context.Request.QueryString["startDateExp"];
                context.Session["endDateExp"] = context.Request.QueryString["endDateExp"];
                context.Session["testExp"] = context.Request.QueryString["test"] == "1";
            }
            context.Response.Redirect("~/Application/Load.aspx?option=quote&id=" + quoteid);
        }
    }

    private bool ValidDate(string date)
    {
        try {
            DateTime dt = DateTime.Parse(date);
            return true;
        } catch {
            return false;
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}