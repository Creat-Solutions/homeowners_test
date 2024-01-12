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
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using ME.Admon;

public partial class Catalog_ValuesandSublimits : System.Web.UI.Page
{
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

    public void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) {
            ddlCompanies.DataSource = csProductCompany.List(ApplicationConstants.APPLICATION, ApplicationConstants.ADMINDB);
            ddlCompanies.DataBind();
            ddlCompanies.SelectedValue = ApplicationConstants.COMPANY.ToString();
        }
    }

}
