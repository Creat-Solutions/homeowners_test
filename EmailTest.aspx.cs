using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EmailTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        //string to = txtTo.Text.Trim();
        //EmailService service = new EmailService(to, false);
        //service.AddCC("manuel@creatsol.com");
        //service.Subject = txtSubject.Text.Trim();
        //service.Body = txtMessage.Text;
        //try {
        //    service.Send();
        //    lblResults.Text = "Success: Email sent";
        //} catch (Exception excep) {
        //    lblResults.Text = "Error: " + excep.Message;
        //}

        ClearForm();
    }

    private void ClearForm()
    {
        txtTo.Text = string.Empty;
        txtSubject.Text = string.Empty;
        txtMessage.Text = string.Empty;
    }
}
