using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Application_DirectCaptureEndorsment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        SearchReportEngine search = new PolicySearchReport("", true);
        var results = search.Search(
            txtPolicy.Text.Trim(),
            "ENDORSMENT",
            "",
            "",
            ""
        );
        if (results == null)
        {
            plhReportes.Controls.Add(new Label
            {
                ID = "lblError",
                Text = "Please specify a search field"
            });
        }
        else
        {
            plhReportes.Controls.Add(BuildResults(results));
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

        if (report == null)
        {
            return new Label
            {
                ID = "lblError",
                Text = "There is no policy with such data."
            };
        }
        return (HtmlTable)viewer.GetViewOfResult(report);
    }
}