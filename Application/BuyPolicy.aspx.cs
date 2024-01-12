using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ME.Admon;
using Protec.Authentication.Membership;

public partial class BuyPolicy : Application
{

    public BuyPolicy() :
        base("PolicyStep")
    {
        AddStep("~/UserControls/BuyCotizaStep.ascx");
        AddStep("~/UserControls/PayHousePolicy.ascx");
        AddStep("~/UserControls/PolicyPDF.ascx");
        ChangeValidateButtons(ValidateButtons);
    }

    private void ValidateButtons()
    {
        switch (StepIndex) {
            case 0:
            case 1:
                btnBuy.Visible = true;
                btnCancel.Text = "Cancel";
                break;
            case 2:
                btnCancel.Text = "Finish";
                btnBuy.Visible = false;
                break;
            default:
                break;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        var quote = db.quotes.SingleOrDefault(m => m.id == (int)Session["quoteid"]);
        int userid = ((UserInformation)HttpContext.Current.User.Identity).UserID;

        if (quote == null && Session["policyid"] == null) {
            Response.Redirect("~/Application/Load.aspx?option=clear", true);
            return;
        }

        if ((quote != null && quote.currency == ApplicationConstants.PESOS_CURRENCY) || userid == 11926)
        {
            chkCC.Visible = false;
            chkCC.Checked = false;
            if (StepIndex == 0) {
                btnBuy.Visible = !AdminGateway.Gateway.MustPayWithCC(userid);
            }
        } else {
            if (StepIndex == 0) {
                chkCC.Visible = !AdminGateway.Gateway.MustPayWithCC(userid);
            } else {
                chkCC.Visible = false;
            }
        }

        
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        placeHolder = plhContent;
        if (Session["quoteid"] == null) {
            Response.Redirect("~/Application/Load.aspx?option=clear", true);
            return;
        }

        
        
        LoadStep(!Page.IsPostBack);
    }

    protected void ValidateRequiredStartDate(object sender, ServerValidateEventArgs args)
    {
        args.IsValid = args.Value.Length > 0;
    }

    protected void btnClick(object sender, EventArgs e)
    {
        if (sender == btnBuy) {
            if (!Page.IsValid)
                return;
            if (chkCC.Visible && chkCC.Checked) {
                Session["DoNotSkipPayment"] = true;
            } else {
                Session["DoNotSkipPayment"] = null;
            }

            if (StepIndex == 1 && SaveCurrentStep()) {
                Session["PolicyStep"] = 2;
                Response.Redirect("~/Application/PolicyChecker.aspx", true);
            } else {
                NextStep(null);
            }            
        } else {
            CancelApplication();
            Session["policyid"] = null;
            Session["policyno"] = null;
            Session["skipPayment"] = null;
            Session["DoNotSkipPayment"] = null;
            Response.Redirect("~/Application/Load.aspx?option=clear", true);
        }
    }
}
