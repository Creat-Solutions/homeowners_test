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

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["error"])) {
            switch (Request.QueryString["error"]) {
                case "db":
                    lblFailure.Text = "A database error occurred while handling your data. Please try again after a few moments.";
                    break;
                case "security":
                    lblFailure.Text = "You don't have enough permissions to do such action.";
                    break;
                default:
                    lblFailure.Text = "An unkown error occurred while handling your data. The developers have been informed. Please try again.";
                    break;
            }
        }
    }
}
