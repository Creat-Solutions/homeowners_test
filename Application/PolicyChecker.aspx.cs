using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Caching;
using System.Net.Mail;
using System.Configuration;
using System.Collections;
using System.Web.SessionState;
using System.Net;

public partial class Application_PolicyChecker : System.Web.UI.Page
{
    private const string POLICYCHECKER = "PolicyCheckerCache";

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static bool CheckPolicyStatus()
    {
        HttpResponse Response = HttpContext.Current.Response;
        HttpRequest Request = HttpContext.Current.Request;
        HttpSessionState Session = HttpContext.Current.Session;
        if (Session["quoteid"] == null)
            return false;
        int quoteid = (int)Session["quoteid"];
        if (Session[POLICYCHECKER] != null) {
            PolicyCacheData data = Session[POLICYCHECKER] as PolicyCacheData;
            if (data.Status == PolicyCacheStatus.Incomplete)
                return false;
            if (data.Data == null) /* programming error */
                return false;
            IHousePolicy policyData = data.Data;
            Session["policyid"] = policyData.PolicyID;
            Session["policyno"] = policyData.PolicyNo;
            if (data.SkipPayment) {
                Session["skipPayment"] = true;
            }
            Session[POLICYCHECKER] = null;
            return true;
        } else
            return false;
    }

    [WebMethod]
    public static void CheckPolicyErrorSubmit()
    {
        try {
            string from = ConfigurationSettings.AppSettings["MailFrom"];
            string to = "manuel@creatsol.com";
            MailMessage message = new MailMessage(from, to);
            message.IsBodyHtml = true;
            message.Subject = "Homeowner's Insurance Application Error: Check Policy Error";
            message.Body = "Unknown error";
            SmtpClient client = new SmtpClient(ConfigurationSettings.AppSettings["MailServer"]);
            if (ConfigurationSettings.AppSettings["MailAuthenticationRequired"] == "true") {
                client.Credentials = new NetworkCredential(from, ConfigurationSettings.AppSettings["MailPassword"]);
            }
            client.Send(message);
        } catch {
        }
    }
}
