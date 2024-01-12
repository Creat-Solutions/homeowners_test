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


public partial class Cotiza : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        try {
            
            if (Request["quote"] != null) {
                IQuoteData calcula = Page.LoadControl("~/UserControls/Cotiza.ascx") as IQuoteData;
                calcula.quoteid = int.Parse(Request["quote"]);
                plhCotiza.Controls.Add((Control)calcula);
            }
        } catch {
            plhCotiza.Controls.Clear();
        }
    }
}
