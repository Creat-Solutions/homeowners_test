using ME.Admon;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Application_DirectCaptureEndorsmentInput : System.Web.UI.Page
{
    protected string policyNo;
    protected csPolicy csPol;
    protected void Page_Load(object sender, EventArgs e)
    {
        policyNo = Request.QueryString["policyNo"];
        if (string.IsNullOrEmpty(policyNo))
        {
            Response.Redirect("DirectCaptureEndorsment.aspx");
        }

        csPol = new csPolicy(policyNo.Trim(), ApplicationConstants.ADMINDB);

        lblPolicyNo.Text = policyNo;
        lblStartDate.Text = csPol.inception;
        lblEndDate.Text = csPol.expiration;
        txtFechaFin.Value = csPol.expiration;

        if (!Page.IsPostBack)
        {
            TextBox[] readOnlyInputs = new TextBox[] { txtMexicanTax, txtUSCommission, txtAmountDue, txtTotalPremium, lblTotCom, lblTotComPer };
            foreach (TextBox input in readOnlyInputs)
            {
                input.Attributes.Add("readonly", "readonly");
            }
        }

    }

    private void btnSave_Click(object sender, System.EventArgs e)
    {
        bool canContinue = true; // variable para indicar datos válidos

        // se borra todo campo que haya tenido error 
        ClearErrors();

        if (txtFechaIni.Value.Trim() == "")
        {
            canContinue = false;
            lblInception.Text = "Required";
            lblInception.Visible = true;
        }
        else
        {
            try
            {
                var startDate = DateTime.Parse(txtFechaIni.Value);
                var polStartDate = DateTime.Parse(csPol.inception);
                var polEndDate = DateTime.Parse(csPol.expiration);

                if (startDate < polStartDate || startDate > polEndDate)
                {
                    canContinue = false;
                    lblInception.Text = "Should not exceed the current policy dates";
                    lblInception.Visible = true;
                }

            }
            catch
            {
                canContinue = false;
                lblInception.Text = "Enter a valid date in format MM/dd/yyyy";
                lblInception.Visible = true;
            }
        }

        if (txtNetPremium.Text.Trim() == "")
        {
            canContinue = false;
            lblNetPremium.Visible = true;
        }
        else
        {
            try
            {
                double.Parse(txtNetPremium.Text);
            }
            catch
            {
                canContinue = false;
                lblNetPremium.Visible = true;
            }
        }
        if (txtPolicyFee.Text.Trim() == "")
        {
            canContinue = false;
            lblPolicyFee.Visible = true;
        }
        else
        {
            try
            {
                double.Parse(txtPolicyFee.Text);
            }
            catch
            {
                canContinue = false;
                lblPolicyFee.Visible = true;
            }
        }
        if (txtInstSurcharge.Text.Trim() == "")
        {
            canContinue = false;
            lblInstSurcharge.Visible = true;
        }
        else
        {
            try
            {
                double.Parse(txtInstSurcharge.Text, NumberStyles.Currency);
            }
            catch
            {
                canContinue = false;
                lblInstSurcharge.Visible = true;
            }
        }
        if (txtMexicanTax.Text.Trim() == "")
        {
            canContinue = false;
            lblMexicanTax.Visible = true;
        }
        else
        {
            try
            {
                double.Parse(txtMexicanTax.Text, NumberStyles.Currency);
            }
            catch
            {
                canContinue = false;
                lblMexicanTax.Visible = true;
            }
        }
        if (txtTotalPremium.Text.Trim() == "")
        {
            canContinue = false;
            lblTotalPremium.Visible = true;
        }
        else
        {
            try
            {
                double.Parse(txtTotalPremium.Text, NumberStyles.Currency);
            }
            catch
            {
                canContinue = false;
                lblTotalPremium.Visible = true;
            }
        }
        if (txtBrokerFee.Text.Trim() != "")
        {
            try
            {
                double.Parse(txtBrokerFee.Text);
            }
            catch
            {
                canContinue = false;
                lblBrokerFee.Visible = true;
            }
        }
        if (txtTotalCommission.Text.Trim() != "")
        {
            try
            {
                double.Parse(txtTotalCommission.Text, NumberStyles.Currency);
            }
            catch
            {
                canContinue = false;
                lblTotCom.Visible = true;
            }
        }
        if (txtUSCommission.Text.Trim() != "")
        {
            try
            {
                double.Parse(txtUSCommission.Text, NumberStyles.Currency);
            }
            catch
            {
                canContinue = false;
                lblUSCom.Visible = true;
            }
        }
        if (txtAmountDue.Text.Trim() != "")
        {
            try
            {
                double.Parse(txtAmountDue.Text, NumberStyles.Currency);
            }
            catch
            {
                canContinue = false;
                lblAmountDue.Visible = true;
            }
        }
        if (txtRXPF.Text.Trim() != "")
        {
            try
            {
                double.Parse(txtRXPF.Text);
            }
            catch
            {
                canContinue = false;
                lblRXPF.Visible = true;
            }
        }
        if (txtNotes.Text.Trim() == "")
        {
            canContinue = false;
            lblErrorNotes.Visible = true;
        }
        if (txtSpecialCoverageFee.Text.Trim() == "")
        {
            canContinue = false;
            lblErrorSpecialCoverageFee.Visible = true;
        }
        else
        {
            try
            {
                double.Parse(txtSpecialCoverageFee.Text);
            }
            catch
            {
                canContinue = false;
                lblErrorSpecialCoverageFee.Visible = true;
            }
        }

        if (canContinue)
        {
            csEndorsment endorsment = new csEndorsment(ApplicationConstants.ADMINDB);
            endorsment.policyNo = this.policyNo;
            endorsment.inception = txtFechaIni.Value;
            endorsment.expiration = txtFechaFin.Value;
            endorsment.issuedby = this.csPol.insurer;
            endorsment.notes = txtNotes.Text;
            endorsment.premium = double.Parse(txtNetPremium.Text, NumberStyles.Currency);
            endorsment.fee = double.Parse(txtPolicyFee.Text, NumberStyles.Currency);
            endorsment.tax = double.Parse(txtMexicanTax.Text, NumberStyles.Currency);
            endorsment.total = double.Parse(txtTotalPremium.Text, NumberStyles.Currency);
            endorsment.taxPercentage = double.Parse(ddlTaxPercentage.SelectedItem.Value, NumberStyles.Currency);
            endorsment.totalCommission = double.Parse(lblTotCom.Text, NumberStyles.Currency);
            endorsment.brokerCommission = double.Parse(txtAgentCom.Text, NumberStyles.Currency);
            endorsment.meCommission = double.Parse(txtUSCommission.Text, NumberStyles.Currency);
            endorsment.type = ddlEndorsment.SelectedValue;
            endorsment.application = ApplicationConstants.APPLICATION;
            endorsment.company = ApplicationConstants.COMPANY;
            //endorsment.endorsmentNo = String.Empty;
            endorsment.specialCoverageFee = double.Parse(txtSpecialCoverageFee.Text, NumberStyles.Currency);

            //if (ddlEndorsment.SelectedValue == "DELETE")
            //{
            //    endorsment.premium = endorsment.premium * -1;
            //    endorsment.fee = endorsment.fee * -1;
            //    endorsment.tax = endorsment.tax * -1;
            //    endorsment.total = endorsment.total * -1;
            //    endorsment.totalCommission = endorsment.totalCommission * -1;
            //    endorsment.brokerCommission = endorsment.brokerCommission * -1;
            //    endorsment.meCommission = endorsment.meCommission * -1;
            //}

            endorsment.Save();

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "alert('The endorsment has been registered correctly'); window.location.href = 'DirectCaptureEndorsmentInput.aspx'; ", true);
        }
    }

    override protected void OnInit(EventArgs e)
    {
        InitializeComponent();
        base.OnInit(e);
    }

    private void InitializeComponent()
    {
        this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
        this.Load += new System.EventHandler(this.Page_Load);
    }

    private void ClearErrors()
    {
        lblInstSurcharge.Visible = false;
        lblMexicanTax.Visible = false;
        lblNetPremium.Visible = false;
        lblPolicyFee.Visible = false;
        //lblPolicyNo.Visible = false;
        lblTotalPremium.Visible = false;
        lblBrokerFee.Visible = false;
        lblUSCom.Visible = false;
        lblAmountDue.Visible = false;
        lblErrorSpecialCoverageFee.Visible = false;
        //lblErrorNotes.Visible = false;
        lblRXPF.Visible = false;
    }
}