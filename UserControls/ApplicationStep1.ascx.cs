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


public partial class UserControls_ApplicationStep1 : System.Web.UI.UserControl, IPolicyLoadable
{
    private string _title = "About the Insured";
    private int _stateselected = 0;
    private bool m_loadFromPolicy = false;

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

        txtAreaFax.Attributes.Add("onkeyup", string.Format("return keyLimit('{0}', '{1}', 3);", txtAreaFax.ClientID, txtFax1.ClientID));
        txtFax1.Attributes.Add("onkeyup", string.Format("return keyLimit('{0}', '{1}', 3);", txtFax1.ClientID, txtFax2.ClientID));


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
            key = GetKey();
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
        if (key <= 0)
        {
            btnNo.Checked = true;
            return;
        }

        if (LoadFromPolicy)
        {
            LoadDataFromPolicy(key);
        }
        else
        {
            LoadDataFromQuote(key);
        }
    }

    private void LoadDataFromQuote(int key)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        insuredPerson person = db.spList_V1_quotePerson(key).Single();
        FillData(person);
    }

    private void LoadDataFromPolicy(int key)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        insuredPerson person = db.spList_V1_policyPerson(key).Single();
        FillData(person);
    }

    private void FillData(insuredPerson person)
    {
        if (person == null)
        {
            btnNo.Checked = true;
            return;
        }

        if (person.legalPerson)
        {
            txtLastName.Text = string.Empty;
            txtLastName.Enabled = false;
            ddlPersonType.SelectedIndex = 1;

            //make last name dissapear with CSS
            txtLastName.Style.Add(HtmlTextWriterStyle.Display, "none");
            txtLastName2.Style.Add(HtmlTextWriterStyle.Display, "none");
            lblLastName.Style.Add(HtmlTextWriterStyle.Display, "none");
            lblLastName2.Style.Add(HtmlTextWriterStyle.Display, "none");
            lblApellidoP.Style.Add(HtmlTextWriterStyle.Display, "none");
            lblApellidoM.Style.Add(HtmlTextWriterStyle.Display, "none");
            lblTaxID.Text = "Tax ID (AAA######AAA): ";
        }
        else
        {
            lblTaxID.Text = "Tax ID (AAAA######AAA): ";
            txtLastName.Text = person.lastName;
            txtLastName2.Text = person.lastName2;
            ddlPersonType.SelectedIndex = 0;
        }
        txtName.Text = person.firstname;

        if (person.rfc != null)
        {
            txtRFC.Text = person.rfc;
        }

        if (!string.IsNullOrEmpty(person.phone) && person.phone.Length > 6)
        {
            txtAreaPhone.Text = person.phone.Substring(0, 3);
            txtPhone1.Text = person.phone.Substring(3, 3);
            txtPhone2.Text = person.phone.Substring(6);
        }
        else
        {
            txtAreaPhone.Text = string.Empty;
            txtPhone1.Text = string.Empty;
            txtPhone2.Text = string.Empty;
        }
        if (!string.IsNullOrEmpty(person.fax) && person.fax.Length > 6)
        {
            txtAreaFax.Text = person.fax.Substring(0, 3);
            txtFax1.Text = person.fax.Substring(3, 3);
            txtFax2.Text = person.fax.Substring(6);
        }
        else
        {
            txtAreaFax.Text = string.Empty;
            txtFax1.Text = string.Empty;
            txtFax2.Text = string.Empty;
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
        string lastname2 = legalPerson ? null : txtLastName2.Text.Trim();
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
        if (!(string.IsNullOrEmpty(txtAreaFax.Text) || string.IsNullOrEmpty(txtFax1.Text) || string.IsNullOrEmpty(txtFax2.Text)))
        {
            fax = txtAreaFax.Text + txtFax1.Text + txtFax2.Text;
            fax = fax.Trim();
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
                txtName.Text.Replace("&", " and ").Trim(),
                lastname,
                lastname2,
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
                txtName.Text.Replace("&", " and ").Trim(),
                lastname,
                lastname2,
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
            btnYes.Checked = true;

        updLiveUsa.Visible = btnYes.Checked;

        base.OnPreRender(e);
    }

    protected void chkState_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void chkCity_CheckedChanged(object sender, EventArgs e)
    {

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
