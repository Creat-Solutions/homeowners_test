using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Protec.Authentication.Membership;
using ME.Admon;
using System.Threading;
using System.Globalization;

public partial class GenerateQuote : Application
{
    private const string DISABLEBUTTONS = "disableButtons";
    private const string QUOTEID = "quoteid";

    protected override void InitializeCulture()
    {
        string selectedLanguage = string.Empty;
        if (Request.Form.Count > 0)
        {   
            foreach (string key in Request.Form)
            {
                if (key != null && key.Contains("imgMxFlag"))
                {
                    Session["CULTURE"] = "es-MX";
                }
                if (key != null && key.Contains("imgUsFlag"))
                {
                    Session["CULTURE"] = "en-US";
                }
            }
        }
        if (Session["CULTURE"] == null)
        {
            Session["CULTURE"] = "en-US";
        }
        selectedLanguage = (string)Session["CULTURE"];
        UICulture = selectedLanguage;
        Culture = selectedLanguage;

        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(selectedLanguage);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(selectedLanguage);
        base.InitializeCulture();
    }

    protected void goToStep(object sender, EventArgs e)
   { 
        Control rsender = sender as Control;
        int steptogo = int.Parse(rsender.ID.Substring(7));
        //revisa usuarios que no sean de M&E para evitar una excepcion OutOfRange (hardcoded)
        int agent = ((UserInformation)User.Identity).Agent;
        if ((agent != ApplicationConstants.MEAGENT || ((UserInformation)User.Identity).UserID == ApplicationConstants.PTI_USER)&& steptogo > 4)
            return;
        LoadStep(steptogo - 1, ChangeTopMenu);
    }

    public GenerateQuote() :
        base("StepIndex")
    {
        AddStep("~/UserControls/GenerateStep1.ascx");
        AddStep("~/UserControls/GenerateStep2.ascx");
        AddStep("~/UserControls/GenerateStep3.ascx");
        AddStep("~/UserControls/GenerateStep4.ascx");
        RegisterSessionVariable(DISABLEBUTTONS);
        RegisterSessionVariable(QUOTEID);

        ChangeValidateButtons(ValidateButtons);
    }

    private void ChangeTopMenu()
    {
        ImageButton imgStep = null;
        int step = StepIndex + 1;
        int quoteid = 0;
        try
        {
            quoteid = Session["quoteid"] != null ? (int)Session["quoteid"] : quoteid;
        }
        catch
        {
        }
        imgStep1.Attributes.Add("onmouseover", "javascript:changeTitle('" + GetLocalResourceObject("strStep1Title").ToString() + "');");
        imgStep1.Attributes.Add("onmouseout", "javascript:changeToOriginal();");
        imgStep2.Attributes.Add("onmouseover", "javascript:changeTitle('" + GetLocalResourceObject("strStep2Title").ToString() + "');");
        imgStep2.Attributes.Add("onmouseout", "javascript:changeToOriginal();");
        imgStep3.Attributes.Add("onmouseover", "javascript:changeTitle('" + GetLocalResourceObject("strStep3Title").ToString() + "');");
        imgStep3.Attributes.Add("onmouseout", "javascript:changeToOriginal();");
        imgStep4.Attributes.Add("onmouseover", "javascript:changeTitle('" + GetLocalResourceObject("strStep4Title").ToString() + "');");
        imgStep4.Attributes.Add("onmouseout", "javascript:changeToOriginal();");
        switch (step)
        {
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
        }

        if (quoteid == 0)
            DisableButtons();

        if (imgStep != null)
        {
            imgStep.Attributes.Remove("onmouseover");
            imgStep.Attributes.Remove("onmouseout");
            imgStep.ImageUrl = "~/images/step" + step + "On.jpg";
        }
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


    protected void Page_Load(object sender, EventArgs e)
    {
        placeHolder = placeHolderStep;
        btnSearchExpiring.Visible = Session["startDateExp"] != null && Session["endDateExp"] != null;
        title = formTitle;
        lblError.Text = string.Empty;
        if (!Page.IsPostBack)
            ChangeTopMenu();
        LoadStep(!Page.IsPostBack);
        if (Session["quoteid"] != null) {
            lblQuote.Text = GetLocalResourceObject("strQuoteNumber").ToString() + Session["quoteid"];
        } else {
            lblQuote.Text = string.Empty;
        }
    }

    private void ValidateButtons()
    {
        if (StepIndex == 0)
        {
            btnPrevious.Enabled = false;
            btnPrevious.Visible = false;
        }
        else
        {
            btnPrevious.Enabled = true;
            btnPrevious.Visible = true;
        }
        if (StepIndex == NumberSteps - 1)
        {
            btnNext.Enabled = false;
            btnNext.Visible = false;
        }
        else
        {
            btnNext.Enabled = true;
            btnNext.Visible = true;
        }
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

    private void DisableButtons()
    {
        imgStep1.Enabled = imgStep2.Enabled = imgStep3.Enabled = imgStep4.Enabled = false;
    }

    protected void btnCancel_click(object sender, EventArgs e)
    {
        Session["quoteid"] = null;
        CancelApplication();
        Response.Redirect("~/Default.aspx");
    }
}
