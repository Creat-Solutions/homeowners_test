using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Application_DateRangeWindow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ClientScriptManager scriptManager = Page.ClientScript;
        scriptManager.RegisterStartupScript(typeof(Page), "BuyCotizaStep", "showDateRange();", true);
    }
}
