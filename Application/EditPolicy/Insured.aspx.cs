using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Application_EditPolicy_Insured : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Control step = new Control();
        if (Session["policyid"] == null) {
            Response.Redirect("~/Application/Load.aspx?option=editpolicy", true);
            return;
        }
        if (Request["step"] != null && Request["step"] == "2")
        {
            step = Page.LoadControl("~/UserControls/ApplicationStep2.ascx");
        }
        else
        {
            step = Page.LoadControl("~/UserControls/ApplicationStep1.ascx");
        }
        step.ID = "InsuredEditControl";
        IPolicyLoadable insuredStep = step  as IPolicyLoadable;
        insuredStep.LoadFromPolicy = true;
        placeHolder.Controls.Clear();
        placeHolder.Controls.Add(step);
        if (!Page.IsPostBack) {
            insuredStep.LoadStep();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        IPolicyLoadable insuredStep = placeHolder.Controls[0] as IPolicyLoadable; //unique control
        insuredStep.LoadFromPolicy = true;
        if (insuredStep.SaveStep()) {
            Response.Redirect("~/Application/EditPolicy/", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Application/EditPolicy/", true);
    }
}
