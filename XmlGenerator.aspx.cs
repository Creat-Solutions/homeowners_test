using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class XmlGeneratorForm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void valRango_OnServerValidate(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = RangeService.IsValidRangeString(txtRango.Text);
    }

    protected void btnGet_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("~/XmlGenerator.ashx?test=" + (chkTest.Checked ? "1" : "0") +"&range=" + txtRango.Text);
    }
}
