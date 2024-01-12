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
using System.Collections.Generic;
using ME.Admon;
using Protec.Authentication.Membership;


public partial class InsuranceApplication : Application
{

    private const string DISABLEBUTTONS = "disableButtons";
    private const string QUOTEID = "quoteid";

    protected void goToStep(object sender, EventArgs e)
    {
        Control rsender = sender as Control;
        int steptogo = int.Parse(rsender.ID.Substring(7));
        //revisa usuarios que no sean de M&E para evitar una excepcion OutOfRange (hardcoded)
        int agent = ((UserInformation)User.Identity).Agent;
        if ((agent != ApplicationConstants.MEAGENT || ((UserInformation)User.Identity).UserID == ApplicationConstants.PTI_USER) && steptogo > 7)
            return;
        LoadStep(steptogo - 1, ChangeTopMenu);
    }

    private void ChangeTopMenu()
    {
        ImageButton imgStep = null;
        int step = StepIndex + 1;
        int quoteid = 0;
        try {
            quoteid = (int)Session["quoteid"];
        } catch {
        }
        imgStep1.Attributes.Add("onmouseover", "javascript:changeTitle('About the Insured');");
        imgStep1.Attributes.Add("onmouseout", "javascript:changeToOriginal();");
        imgStep2.Attributes.Add("onmouseover", "javascript:changeTitle('About the Insured Property');");
        imgStep2.Attributes.Add("onmouseout", "javascript:changeToOriginal();");
        imgStep3.Attributes.Add("onmouseover", "javascript:changeTitle('Additional Property Information');");
        imgStep3.Attributes.Add("onmouseout", "javascript:changeToOriginal();");
        imgStep4.Attributes.Add("onmouseover", "javascript:changeTitle('Additional Security Information');");
        imgStep4.Attributes.Add("onmouseout", "javascript:changeToOriginal();");
        imgStep5.Attributes.Add("onmouseover", "javascript:changeTitle('Values and Sublimits');");
        imgStep5.Attributes.Add("onmouseout", "javascript:changeToOriginal();");
        imgStep6.Attributes.Add("onmouseover", "javascript:changeTitle('Additional Coverages');");
        imgStep6.Attributes.Add("onmouseout", "javascript:changeToOriginal();");
        imgStep7.Attributes.Add("onmouseover", "javascript:changeTitle('Calculation');");
        imgStep7.Attributes.Add("onmouseout", "javascript:changeToOriginal();");
        imgStep8.Attributes.Add("onmouseover", "javascript:changeTitle('Quote');");
        imgStep8.Attributes.Add("onmouseout", "javascript:changeToOriginal();");
        switch (step) {
            case 1:
                imgStep = imgStep1;
                break;
            case 2:
                imgStep = imgStep2;
                break;
            case 3:
                imgStep = imgStep3;
                break;
            case 4:
                imgStep = imgStep4;
                break;
            case 5:
                imgStep = imgStep5;
                break;
            case 6:
                imgStep = imgStep6;
                break;
            case 7:
                imgStep = imgStep7;
                break;
            case 8:
                imgStep = imgStep8;
                break;
        }

        if (quoteid == 0)
            DisableButtons();

        if (imgStep != null) {
            imgStep.Attributes.Remove("onmouseover");
            imgStep.Attributes.Remove("onmouseout");
            imgStep.ImageUrl = "~/images/step" + step + "On.jpg";
        }
    }

    public InsuranceApplication() :
        base("StepIndex")
    {
        AddStep("~/UserControls/ApplicationStep1.ascx");
        AddStep("~/UserControls/ApplicationStep2.ascx");
        AddStep("~/UserControls/ApplicationStep3.ascx");
        AddStep("~/UserControls/ApplicationStep4.ascx");
        AddStep("~/UserControls/ApplicationStep5.ascx");
        AddStep("~/UserControls/ApplicationStep6.ascx");
        RegisterSessionVariable(DISABLEBUTTONS);
        RegisterSessionVariable(QUOTEID);

        ChangeValidateButtons(ValidateButtons);
    }

    private void DisableButtons()
    {
        imgStep1.Enabled = imgStep2.Enabled = imgStep3.Enabled = imgStep4.Enabled =
        imgStep5.Enabled = imgStep6.Enabled = imgStep7.Enabled = imgStep8.Enabled = false;
    }

    private bool ShouldDisableButtons()
    {
        try {
            bool disable = true;
            if (Session["quoteid"] == null) {
                return true;
            }
            int quoteid = (int)Session["quoteid"];
            HouseInsuranceDataContext db = new HouseInsuranceDataContext();
            decimal csl = db.quotes.Where(m => m.id == quoteid).Select(m => m.CSLLimit).Single();
            disable = csl <= 0;
            return disable;
        } catch {
            return true;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (ShouldDisableButtons()) {
            DisableButtons();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try {
            //verifica que solo sea usuarios de M&E
            int agent = ((UserInformation)User.Identity).Agent;
            if (agent == ApplicationConstants.MEAGENT || ((UserInformation)User.Identity).UserID == ApplicationConstants.PTI_USER) {
                AddStep("~/UserControls/CalculaStep.ascx");
            } else {
                imgStep8.Visible = imgStep8.Enabled = false;
            }
        } catch {
        }
        AddStep("~/UserControls/CotizaStep.ascx");
        placeHolder = placeHolderStep;
        btnSearchExpiring.Visible = Session["startDateExp"] != null && Session["endDateExp"] != null;
        title = formTitle;
        lblError.Text = string.Empty;
        if (!Page.IsPostBack)
            ChangeTopMenu();
        LoadStep(!Page.IsPostBack);
        if (Session["quoteid"] != null) {
            lblQuote.Text = "Quote No. " + Session["quoteid"];
        } else {
            lblQuote.Text = string.Empty;
        }
    }

    private void ValidateButtons()
    {
        if (StepIndex == 0)
            btnPrevious.Enabled = false;
        else
            btnPrevious.Enabled = true;
        if (StepIndex == NumberSteps - 1)
            btnNext.Enabled = false;
        else
            btnNext.Enabled = true;
    }

    protected void btnSearchExpiring_Click(object sender, EventArgs e)
    {
        string test = "0";
        if (Session["testExp"] != null) {
            test = (bool)Session["testExp"] ? "1" : "0";
        }
        Response.Redirect("~/Reports/ReportPolicyExpiring.aspx?startDateExp=" + Session["startDateExp"] +
                          "&endDateExp=" + Session["endDateExp"] + "&test=" + test + "&search=1");
    }

    protected void btnPrevious_click(object sender, EventArgs e)
    {
        PreviousStep(ChangeTopMenu);
    }

    protected void btnNext_click(object sender, EventArgs e)
    {
        NextStep(ChangeTopMenu);
    }
    
    protected new void NextStep(ApplicationDelegate del)
    {
        base.NextStep(del);
        if (!ShouldDisableButtons() && GetSessionVariable("quoteid") != null) {
            HouseInsuranceDataContext db = new HouseInsuranceDataContext();
            int quoteid = (int)GetSessionVariable("quoteid");
            HouseEstate he = db.quotes.Single(m => m.id == quoteid).insuredPersonHouse1.HouseEstate;
            db.spUpd_quote(quoteid, he.TEV, he.City.HMP, he.City.State.TheftZone[0]);
        }
            
    }
    

    protected void btnCancel_click(object sender, EventArgs e)
    {
        Session["quoteid"] = null;
        CancelApplication();
        Response.Redirect("~/Default.aspx");
    }
    
}
