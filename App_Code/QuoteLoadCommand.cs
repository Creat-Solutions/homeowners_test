using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Protec.Authentication.Membership;

/// <summary>
/// Carga cotizaciones
/// </summary>
public class QuoteLoadCommand : AbstractLoadCommand
{
    private Label lblError = null;
	public QuoteLoadCommand(HttpContext context, Label lblError) :
        base(context)
	{
        this.lblError = lblError;
	}

    private void LoadQuote(int key)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        UserInformation user = (UserInformation)HttpContext.Current.User.Identity;
        int agent = user.Agent;
        int userid = user.UserID;
        int q = 0;
        if (agent == ApplicationConstants.MEAGENT ||
            user.UserType == SecurityManager.ADMINISTRATOR || user.UserID == ApplicationConstants.PTI_USER) {
            q = db.quotes.Where(m => m.id == key).Select(m => m.id).Single();
        } else {
            var usuarios = AdminGateway.Gateway.GetListOfUsersFromAgent(agent, userid);
            q = db.quotes.Where(m => m.id == key && usuarios.Contains(m.issuedBy)).Select(m => m.id).Single();
        }
        if (q != key)
            throw new Exception();
        Session["quoteid"] = key;
        Session["StepIndex"] = null;
        Session["PolicyStep"] = null;
        Response.Redirect("~/Application/GenerateQuote.aspx", true);
    }

    public override void Init()
    {
        string key = Request.QueryString["id"];
        if (string.IsNullOrEmpty(key)) {
            return;
        }

        try {
            int id = int.Parse(key);
            LoadQuote(id);
        } catch {
            lblError.Text = "There is no quote with such id: " + key;
        }
    }



    public override void Load(string id)
    {
        int key = 0;
        try {
            key = int.Parse(id);
            LoadQuote(key);
        } catch {
            lblError.Text = "There is no quote with such id: " + id;
        }
    }
}
