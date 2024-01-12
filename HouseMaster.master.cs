using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.IO;
using System.Text;
using Protec.Authentication.Membership;

public partial class HouseMaster : System.Web.UI.MasterPage
{
    private string _title = String.Empty;
    public string TitlePage
    {
        get { return _title; }
        set
        {
            if (value != null)
                _title = value;
            else
                _title = String.Empty;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterClientScriptInclude("Utils", Request.ApplicationPath + "Utils.js");
        titlePage.Text = _title;
        UserInformation user = (UserInformation)HttpContext.Current.User.Identity;
        lblUsername.Text = Server.HtmlEncode(user.Name);
        lblExchange.Text = "Exchange rate: " + user.ExchangeRate.ToString("C") + " MX Pesos / $1 Dlls";
        lblDate.Text = DateTime.Today.ToLongDateString();
    }
}
