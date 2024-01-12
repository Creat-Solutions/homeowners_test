using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Protec.Authentication.Membership;

public partial class Logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // limpia la sesion
        Session.Abandon();
        // avisa a protec que nos salimos y elimina la cookie de sesion
        SecurityManager.Logout();
        // nos manda al login de protec
        Response.Redirect("~/");
    }
}
