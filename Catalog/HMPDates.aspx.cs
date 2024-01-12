using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Catalog_HMPDates : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) {
            try {
                HouseInsuranceDataContext db = new HouseInsuranceDataContext();
                var values = db.spList_ValidHMPDates().Single();
                txtStart.Text = values.fecha_ini.ToString("d");
                txtEnd.Text = values.fecha_fin.ToString("d");
            } catch {
                txtStart.Text = string.Empty;
                txtEnd.Text = string.Empty;
            }
        }
    }

    protected void btnHMPActive_Click(object sender, EventArgs e)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        try {
            DateTime start = DateTime.Parse(txtStart.Text);
            DateTime end = DateTime.Parse(txtEnd.Text);
            db.spUpd_ValidHMPDates(start, end);
        } catch {
            txtError.Visible = true;
        } finally {
            db.Connection.Close();
        }
        if (!txtError.Visible)
            Response.Redirect("~/Default.aspx", true);
    }
}
