﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using ME.Admon;
using Protec.Authentication.Membership;

public partial class ReportPolicy : System.Web.UI.Page
{

    protected void btnSearch_Click(object sender, EventArgs e)
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

        SearchReportEngine search = new PolicySearchReport(agentes, true);
        var results = search.Search(
            txtPolicy.Text,
            txtName.Text,
            txtStartDate.Text,
            txtEndDate.Text,
            txtAgent.Text
        );
        if (results == null) {
            plhReportes.Controls.Add(new Label {
                ID = "lblError",
                Text = "Please specify a search field"
            });
        } else {
            plhReportes.Controls.Add(BuildResults(results));
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) {
            calStartDate.SelectedDate = null;
            calEndDate.SelectedDate = null;
        } else {
            calStartDate.SelectedDate = DateTime.Today;
            calEndDate.SelectedDate = DateTime.Today.AddDays(1);
        }
    }

    private Control BuildResults(IEnumerable<SearchResult> results)
    {
        IResultViewer viewer = new HtmlReportViewer();
        ReportTable report = ReportTable.Build(
            string.Empty,
            results,
            new string[] {
                "Details",
                "Net Premium / Fee / Tax / Total",
                "Options"
            },
            new HtmlRowBuilder()
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
