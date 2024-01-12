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
using System.Text.RegularExpressions;
using Protec.Authentication.Membership;


public partial class UserControls_QuickQuoteStep4 : System.Web.UI.UserControl, IPolicyLoadable
{
    private string _title = "About the Insured";
    private int _stateselected = 0;
    private bool m_loadFromPolicy = false;
    protected int QuoteID { get; set; }
    public bool LoadFromPolicy
    {
        get
        {
            return m_loadFromPolicy;
        }
        set
        {
            m_loadFromPolicy = value;
        }
    }

    protected void ValidateState(object sender, ServerValidateEventArgs e)
    {
        if (btnNotUSA.Checked)
        {
            e.IsValid = !string.IsNullOrEmpty(txtState.Text.Trim());
        }
        else
        {
            e.IsValid = ddlState.SelectedValue != "0";
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        txtAreaPhone.Attributes.Add("onkeyup", string.Format("return keyLimit('{0}', '{1}', 3);", txtAreaPhone.ClientID, txtPhone1.ClientID));
        txtPhone1.Attributes.Add("onkeyup", string.Format("return keyLimit('{0}', '{1}', 3);", txtPhone1.ClientID, txtPhone2.ClientID));

    }

    private int GetKey()
    {
        if (LoadFromPolicy)
        {
            return (int)Session["policyid"];
        }
        else
        {
            if (Session["quoteid"] == null)
            {
                return 0;
            }
            return (int)Session["quoteid"];
        }
    }

    public void LoadStep()
    {
        int key;
        try
        {
            QuoteID = key = GetKey();
            UserInformation info = (UserInformation)Page.User.Identity;
            btnGenerateCalculo.Visible = info.Agent == ApplicationConstants.MEAGENT;
        }
        catch
        {
            return;
        }
        LoadData(key);
    }
    public bool SaveStep()
    {
        if (Page.IsValid)
        {
            try
            {
                int key = getData();
                if (!LoadFromPolicy)
                {
                    Session["quoteid"] = key;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        return false;
    }
    public string Title()
    {
        return _title;
    }

    private void LoadData(int key)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        var quote = db.quotes.Single(m => m.id == key);
        LoadPremiumData(db, quote, key);

        if (key <= 0)
        {
            return;
        }

        LoadDataFromQuote(quote);
    }

    private void LoadDataFromQuote(quote quote)
    {
        FillPersonData(quote.insuredPerson1);
    }

    private void LoadPremiumData(HouseInsuranceDataContext db, quote quote, int key)
    {
        var codigos = db.spList_Calcula(key, quote.insuredPersonHouse1.HouseEstate.City.HMP).Single();
        var netPremium = codigos.netPremium.Value;
        lblNetPremium.Text = netPremium.ToString("C2");
        lblFee.Text = ApplicationConstants.Fee(quote.currency, quote.fee.Value).ToString("C2");
        decimal rxpfPercentage = quote.porcentajeRecargoPagoFraccionado;
        decimal taxPercentage = db.fGet_QuoteTaxPercentage(key).Value;
        decimal tax = ApplicationConstants.CalculateTax(netPremium, taxPercentage, rxpfPercentage, quote.currency, quote.fee.Value);
        lblTax.Text = tax.ToString("C2");
        lblTotal.Text = ApplicationConstants.CalculateTotal(netPremium, taxPercentage, rxpfPercentage, quote.currency, quote.fee.Value).ToString("C2");
    }


    private void FillPersonData(insuredPerson person)
    {
        if (person == null)
            return;
        if (person.legalPerson)
        {
            txtLastName.Text = string.Empty;
            txtLastName.Enabled = false;
            ddlPersonType.SelectedIndex = 1;

            //make last name dissapear with CSS
            txtLastName.Style.Add(HtmlTextWriterStyle.Display, "none");
            lblLastName.Style.Add(HtmlTextWriterStyle.Display, "none");
            lblApellidos.Style.Add(HtmlTextWriterStyle.Display, "none");
            lblTaxID.Text = "Tax ID (AAA######AAA): ";
        }
        else
        {
            lblTaxID.Text = "Tax ID (AAAA######AAA): ";
            txtLastName.Text = person.lastName;
            ddlPersonType.SelectedIndex = 0;
        }
        txtName.Text = person.firstname;

        if (person.rfc != null)
        {
            txtRFC.Text = person.rfc;
        }

        if (person.phone != null)
        {
            txtAreaPhone.Text = person.phone.Substring(0, 3);
            txtPhone1.Text = person.phone.Substring(3, 3);
            txtPhone2.Text = person.phone.Substring(6, 5);
        }
        else
        {
            txtAreaPhone.Text = string.Empty;
            txtPhone1.Text = string.Empty;
            txtPhone2.Text = string.Empty;
        }

        txtEmail.Text = person.email;
        if (person.additionalInsured != null)
            txtAdditionalInsured.Text = person.additionalInsured;
        if (person.street != null)
        {
            btnYes.Checked = true;
            txtAddress.Text = person.street;
            txtZip.Text = person.zipCode;
            if (person.state == null)
            {
                ddlState.Style.Add(HtmlTextWriterStyle.Display, "none");
                txtState.Style.Add(HtmlTextWriterStyle.Display, "inline");
                txtState.Text = person.stateText;
                btnNotUSA.Checked = true;
                btnUSA.Checked = false;
                _stateselected = 0;
            }
            else
            {
                btnUSA.Checked = true;
                btnNotUSA.Checked = false;
                txtState.Style.Add(HtmlTextWriterStyle.Display, "none");
                ddlState.Style.Add(HtmlTextWriterStyle.Display, "inline");
                _stateselected = (int)person.state;
            }
            txtCity.Text = person.city;
        }
        else
        {
            btnNo.Checked = true;
        }

    }
    private int getData()
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        int key = GetKey();
        int userid = ((UserInformation)HttpContext.Current.User.Identity).UserID;
        string street = null;
        string email = null;
        string fax = null;
        string phone = null;
        string city = null;
        string zip = null;
        string rfc = null;
        bool legalPerson = ddlPersonType.SelectedValue == "1";
        string lastname = legalPerson ? null : txtLastName.Text.Trim();
        string additionalInsured = null;
        int? state = null;
        string stateText = null;
        if (btnYes.Checked)
        {
            street = txtAddress.Text.Trim();
            zip = txtZip.Text.Trim();
            if (btnNotUSA.Checked)
            {
                state = null;
                stateText = txtState.Text.Trim();
            }
            else
            {
                state = int.Parse(ddlState.SelectedValue);
                stateText = null;
            }
            city = txtCity.Text.Trim();
        }
        bool inMexico = street == null;

        if (!string.IsNullOrEmpty(txtRFC.Text))
            rfc = txtRFC.Text.Trim();
        if (!string.IsNullOrEmpty(txtEmail.Text))
            email = txtEmail.Text.Trim();
        if (!string.IsNullOrEmpty(txtAdditionalInsured.Text))
            additionalInsured = txtAdditionalInsured.Text.Trim();
        if (!(string.IsNullOrEmpty(txtAreaPhone.Text) || string.IsNullOrEmpty(txtPhone1.Text) || string.IsNullOrEmpty(txtPhone2.Text)))
        {
            phone = txtAreaPhone.Text + txtPhone1.Text + txtPhone2.Text;
            phone = phone.Trim();
        }

        if (LoadFromPolicy)
        {
            policy p = db.policies.Single(m => m.id == key);
            bool inMexicoOriginally = p.insuredPerson1.street == null;
            if (inMexicoOriginally != inMexico)
            {
                AdminGateway.Gateway.ChangeCountry(p.policyNo, inMexico);
            }
            db.spUpd_V2_policyPerson(
                key,
                txtName.Text.Trim(),
                lastname,
                string.Empty,
                street,
                email,
                fax,
                phone,
                city,
                state,
                zip,
                legalPerson,
                additionalInsured,
                rfc,
                stateText
            );
            return 0;
        }
        else
        {
            return db.spUpd_V2_quotePerson(
                key,
                userid,
                txtName.Text.Trim(),
                lastname,
                string.Empty,
                street,
                email,
                fax,
                phone,
                city,
                state,
                zip,
                legalPerson,
                ApplicationConstants.COMPANY,
                additionalInsured,
                rfc,
                stateText
            );
        }
    }
    protected void CheckedChange(object sender, EventArgs e)
    {
        updLiveUsa.Visible = btnYes.Checked;
    }


    protected override void OnPreRender(EventArgs e)
    {
        if (!btnNo.Checked && !btnYes.Checked)
            btnNo.Checked = true;

        updLiveUsa.Visible = btnYes.Checked;

        base.OnPreRender(e);
    }

    protected void chkState_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void chkCity_CheckedChanged(object sender, EventArgs e)
    {

    }

    protected void btnBuyQuote_Click(object sender, EventArgs e)
    {
        // the post back allow us to verify the data through the asp.net validators
        Response.Redirect("~/Application/Load.aspx?option=buyquote&id=" + GetKey());
    }

    protected void ddlDataBound(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;

        ListItem item = new ListItem("---", "0");
        ddl.Items.Insert(0, item);
        if (ddl == ddlState && _stateselected > 0)
        {
            ddl.SelectedValue = _stateselected.ToString();
        }
    }

    protected void txtLastName_ServerValidate(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = ddlPersonType.SelectedIndex == 1 || !string.IsNullOrEmpty(txtLastName.Text);
    }

    protected void valRFC_ServerValidate(object sender, ServerValidateEventArgs e)
    {
        if (string.IsNullOrEmpty(txtRFC.Text))
        {
            e.IsValid = true;
            return;
        }
        string pattern = string.Empty;
        if (ddlPersonType.SelectedIndex == 1)
        {
            /* si es persona moral */
            pattern = @"^[A-Za-z]{3}\d{2}(0[1-9]|1[0-2])(0[1-9]|1\d|2\d|3[01])[A-Za-z0-9]{3}$";
        }
        else
        {
            pattern = @"^[A-Za-z]{4}\d{2}(0[1-9]|1[0-2])(0[1-9]|1\d|2\d|3[01])[A-Za-z0-9]{3}$";
        }
        e.IsValid = Regex.IsMatch(txtRFC.Text, pattern);
    }

}
