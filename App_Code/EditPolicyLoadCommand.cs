using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Protec.Authentication.Membership;

/// <summary>
/// Summary description for EditPolicyLoadCommand
/// </summary>
public class EditPolicyLoadCommand : AbstractLoadCommand
{
    private Label lblError = null;
    private Label lblLoad = null;
	public EditPolicyLoadCommand(HttpContext context, Label lblError, Label lblLoad) :
        base(context)
	{
		this.lblError = lblError;
        this.lblLoad = lblLoad;
	}

    public override void Init()
    {
        lblLoad.Text = "Load policy: ";
    }
    
    public override void Load(string id)
    {
        string policyNo = id.Trim();
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        var dataStep = db.policies.Where(m => m.policyNo == policyNo).Select(m => new { m.id, m.issuedBy, m.insuredPerson1 });
        UserInformation user = (UserInformation)HttpContext.Current.User.Identity;

        bool canUsePolicy = AdminGateway.Gateway.AgentCanLoadPolicy(
            policyNo, 
            user.Agent, 
            user.UserID, 
            user.UserType
        );

        if (dataStep.Count() == 0 || !canUsePolicy) {
            lblError.Text = "There is no policy with such a number.";
            return;
        }

        var data = dataStep.Single();

        Session["policyid"] = data.id;
        Session["policyno"] = id;
        Session["canGetInvoice"] = !string.IsNullOrEmpty(data.insuredPerson1.rfc);
        Response.Redirect("~/Application/EditPolicy", true);
    }
}
