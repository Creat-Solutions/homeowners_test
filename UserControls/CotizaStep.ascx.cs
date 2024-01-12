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

public partial class UserControls_CotizaStep : System.Web.UI.UserControl, IApplicationStep
{
    private bool loaded = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        int quoteid = (int)Session["quoteid"];
        if (!loaded)
            LoadStep();
    }

    public string Title()
    {
        return "Calculation";
    }

    public void LoadStep()
    {
        Control load;
        plhCotiza.Controls.Clear();
        try {
            IQuoteData calcula = Page.LoadControl("~/UserControls/Cotiza.ascx") as IQuoteData;
            calcula.quoteid = (int)Session["quoteid"];
            load = (Control)calcula;
        } catch {
            Label lbl = new Label();
            lbl.Text = "There was an error while trying to load this screen.";
            load = lbl;
        }
        plhCotiza.Controls.Add(load);
        loaded = true;
    }

    public bool SaveStep()
    {
        return true;
    }
}
