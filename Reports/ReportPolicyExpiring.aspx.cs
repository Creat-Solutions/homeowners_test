using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using ME.Admon;
using Protec.Authentication.Membership;

public partial class ReportPolicyExpiring : System.Web.UI.Page
{
    protected void Search(string name, string policyNo, string startDate, string endDate, bool test, bool includeRenewed)
    {
        string agentes = string.Empty;
        try {
            UserInformation user = (UserInformation)User.Identity;
            int agent = user.Agent;
            string userType = user.UserType;
            if (agent == ApplicationConstants.MEAGENT || userType == SecurityManager.ADMINISTRATOR || user.UserID == ApplicationConstants.PTI_USER) {
                agentes = null;
            } else {
                agentes = AdminGateway.Gateway.ListAllUsersFromAgent(agent, user.UserID);
            }
        } catch {
            Response.Redirect("~/Login.aspx");
            return;
        }

        SearchReportEngine search = new PolicySearchReport(agentes, test);
        IEnumerable<SearchResult> results;
        if (!string.IsNullOrEmpty(policyNo)) {
            results = search.Search(policyNo, null, null, null, null);
        } else if (!string.IsNullOrEmpty(name)) {
            results = search.Search(null, name, null, null, null);
        } else {
            results = search.SearchExpiring(startDate, endDate, includeRenewed);
        }

        if (results == null) {
            plhReportes.Controls.Add(new Label {
                ID = "lblError",
                Text = "Please specify a search field"
            });
        } else {
            plhReportes.Controls.Add(BuildResults(results, startDate, endDate, test, includeRenewed));
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        string startDate = Request["startDateExp"],
               endDate = Request["endDateExp"];
        bool validSearch = false;
        try {
            if (!string.IsNullOrEmpty(Page.Request["search"]) && Page.Request["search"] == "1") {
                DateTime st = string.IsNullOrEmpty(startDate) ? new DateTime(2005, 1, 1) : DateTime.Parse(startDate);
                DateTime et = string.IsNullOrEmpty(endDate) ? DateTime.Now : DateTime.Parse(endDate);
                validSearch = st < et || !string.IsNullOrEmpty(Page.Request["name"].Trim()) || !string.IsNullOrEmpty(Page.Request["policyNo"].Trim());
            }
        } catch {
            validSearch = false;
        }
        if (validSearch) {
            Search(
                (Request["name"] ?? string.Empty).Trim(),
                (Request["policyNo"] ?? string.Empty).Trim(),
                startDate,
                endDate,
                Request["test"] == "1",
                Request["includeRenewed"] == "1"
            );
        } else {
            calStartDate.SelectedDate = DateTime.Today.AddMonths(-1);
            calEndDate.SelectedDate = DateTime.Today.AddMonths(1);
        }
    }

    private Control BuildResults(IEnumerable<SearchResult> results, string start, string end, bool test, bool includeRenewed)
    {
        IResultViewer viewer = new HtmlReportViewer();
        ReportTable report = ReportTable.Build(
            string.Empty,
            results,
            includeRenewed ?
                new string[] {
                    "Details",
                    "Net Premium / Fee / Tax / Total",
                    "Policy renewal",
                    "Options",
                } :
                new string[] {
                    "Details",
                    "Net Premium / Fee / Tax / Total",
                    "Options"
                },
            new HtmlRowBuilder(start, end, test, includeRenewed)
        );

        if (report == null) {
            return new Label {
                ID = "lblError",
                Text = "There is no policy with such data."
            };
        }
        return (HtmlTable)viewer.GetViewOfResult(report);
    }
}
