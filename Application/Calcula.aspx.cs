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
using Protec.Authentication.Membership;


public partial class Calcula : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //verifica M&E usuarios
        try {
            UserInformation user = (UserInformation)User.Identity;
            if (Request["quote"] != null &&
                (user.Agent == ApplicationConstants.MEAGENT || user.UserID == ApplicationConstants.PTI_USER)) {
                IQuoteData calcula = Page.LoadControl("~/UserControls/Calcula.ascx") as IQuoteData;
                calcula.quoteid = int.Parse(Request["quote"]);
                plhCalcula.Controls.Add((Control)calcula);
            }
        } catch {
        }
    }
}
