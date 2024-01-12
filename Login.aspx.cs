using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Protec.Authentication.Membership;

//public partial class Login : Protec.Authentication.Component.LoginPage
//{
//    public Login() :
//        base(ApplicationConstants.APPLICATION, true)
//    {
//    }
//}

public partial class Login : Page
{
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (!Page.IsPostBack)
        {
            lblInvalid.Visible = false;
        }

        if (User.Identity.IsAuthenticated)
        {
            Response.Redirect("~/");
        }
        else
        {
            Response.AddHeader("X-Login-Required", "true");
        }

    }



    private string GetRawRedirectUrl()
    {
        string redirectUrl = string.Empty;

        HttpRequest Request = HttpContext.Current.Request;

        if (Request.QueryString["ReturnUrl"] != null)
        {
            redirectUrl = Request.QueryString["ReturnUrl"];
        }
        else if (Request.QueryString["returnUrl"] != null)
        {
            redirectUrl = Request.QueryString["returnUrl"];
        }
        else if (Request.QueryString["returnurl"] != null)
        {
            redirectUrl = Request.QueryString["returnurl"];
        }
        return redirectUrl;
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (Request.IsAuthenticated)
        {
            Response.Redirect("~/", true);
            return;
        }

        if (Page.IsValid)
        {
            bool auth = SecurityManager.ValidateLogin(txtUsername.Text, txtPassword.Text, ApplicationConstants.APPLICATION);
            lblInvalid.Visible = !auth;
            if (auth)
            {
                string redirectUrl = GetRawRedirectUrl();
                if (string.IsNullOrEmpty(redirectUrl))
                {
                    redirectUrl = "~/";
                }
                Response.Redirect(Server.HtmlDecode(redirectUrl), true);
                return;
            }
        }
    }
}