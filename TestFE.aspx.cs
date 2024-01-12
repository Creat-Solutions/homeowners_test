using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TestFE : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) {
            lblResults.Text = "Failure";
            lblResults.Visible = false;
        }
    }

    protected void btnTest_Click(object sender, EventArgs e)
    {
        MapfreElectronicInvoiceParser parser = new MapfreElectronicInvoiceParser(txtXML.Text, txtXML.Text, true);
        parser.DecodeAndGenerate(1); //fake policyid
        lblResults.Text = "Success";
        lblResults.Visible = true;

    }
}
