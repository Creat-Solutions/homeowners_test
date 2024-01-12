using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using ME.Admon;

public partial class Catalog_RXPFSur : System.Web.UI.Page
{
    private string[] MonthNames;

    public Catalog_RXPFSur()
    {
        CultureInfo info = new CultureInfo("en-us");
        MonthNames = info.DateTimeFormat.MonthNames;
    }

    protected void RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer) {
            int m = e.Row.Cells.Count;
            for (int i = m - 1; i >= 1; i--) {
                e.Row.Cells.RemoveAt(i);
            }
            e.Row.Cells[0].ColumnSpan = m;
        }
    }

    protected void grdCatalog_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow) {
            Label lblMonth = e.Row.FindControl("lblMonth") as Label;
            int month = (int)DataBinder.Eval(e.Row.DataItem, "month");
            lblMonth.Text = MonthNames[month - 1];
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) {
            ddlCompanies.DataSource = csProductCompany.List(ApplicationConstants.APPLICATION, ApplicationConstants.ADMINDB);
            ddlCompanies.DataBind();
            ddlCompanies.SelectedValue = ApplicationConstants.COMPANY.ToString();
        }
    }
}
