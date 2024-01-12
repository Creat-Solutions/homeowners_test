using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Protec.Authentication.Membership;
using ME.Admon;

public partial class Application_EditPolicy_Default : System.Web.UI.Page
{
    protected string PolicyID = string.Empty;
    protected string PolicyNo = string.Empty;
    protected bool ShouldDisplayParentInvoice = false;

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Session["policyid"] != null) {
            PolicyID = ((int)Session["policyid"]).ToString();
        }
        if (Session["policyno"] != null) {
            PolicyNo = Session["policyno"] as string;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["policyid"] == null) {
            Response.Redirect("~/Application/Load.aspx?option=editpolicy", true);
            return;
        }
        int policyid = (int)Session["policyid"];
        LoadPolicyData(policyid);
    }

    private string GetName(insuredPerson person)
    {
        if (person.legalPerson) {
            return person.firstname;
        }
        return person.firstname + " " + person.lastName + " " + person.lastName2;
    }

    private string GetAddressLine1(insuredPerson person)
    {
        if (string.IsNullOrEmpty(person.street)) {
            return "ADDRESS IS THE SAME AS THE INSURED PROPERTY";
        }
        return person.street;
    }

    private string GetAddressLine2(insuredPerson person)
    {
        if (string.IsNullOrEmpty(person.street)) {
            return string.Empty;
        }
        string stateName = person.state == null ? person.stateText : new State(person.state.Value).Name;
        return person.city + ", " + stateName + ", " + person.zipCode;
    }

    private string TransformContactListToString(List<string> contact)
    {
        string htmlString = string.Empty;
        HtmlGenericControl listOfContactInformation = new HtmlGenericControl("ul");
        foreach (string contactInfo in contact) {
            HtmlGenericControl listMember = new HtmlGenericControl("li");
            listMember.InnerHtml = contactInfo;
            listOfContactInformation.Controls.Add(listMember);
        }
        return contact.Count() == 0 ? "None" : listOfContactInformation.RenderHtmlString();
    }

    private string GetContactInformation(insuredPerson person)
    {
        List<string> contactInformation = new List<string>();
        if (!string.IsNullOrEmpty(person.email)) {
            contactInformation.Add("Email: " + person.email);
        }
        if (!string.IsNullOrEmpty(person.phone)) {
            contactInformation.Add("Phone: " + person.phone);
        }
        if (!string.IsNullOrEmpty(person.fax)) {
            contactInformation.Add("Fax: " + person.fax);
        }
        return TransformContactListToString(contactInformation);
    }

    private string GetRFC(insuredPerson person)
    {
        return "RFC: " + (person.rfc ?? "N/A");
    }

    private string GetHouseStreet(insuredPersonHouse house)
    {
        return "Street: " + house.street;
    }

    private string GetHouseExtNo(insuredPersonHouse house)
    {
        return "Exterior #: " + house.exteriorNo;
    }

    private string GetHouseIntNo(insuredPersonHouse house)
    {
        return (house.InteriorNo != null) ? ("Interior #: " + house.InteriorNo) : string.Empty;
    }

    private string GetBankName(insuredPersonHouse house)
    {
        return (house.bankName != null) ? ("Bank: " + house.bankName) : "N/A";
    }

    private string GetLoanNumber(insuredPersonHouse house)
    {
        return (house.loanNumber != null) ? "Loan #: " + house.loanNumber : string.Empty;
    }

    private string GetBankAddress(insuredPersonHouse house)
    {
        return (house.bankAddress != null) ? "Bank Address: " + house.bankAddress : string.Empty;
    }

    private void LoadPolicyData(int policyid)
    {
        
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        insuredPerson person = db.spList_V1_policyPerson(policyid).Single();
        insuredPersonHouse house = db.spList_V1_policyPersonHouse(policyid).Single();
        policy policy = db.policies.Where(m => m.id == policyid).Single();
        lblName.Text = GetName(person);
        lblRFC.Text = GetRFC(person);
        lblAddressLine1.Text = GetAddressLine1(person);
        lblAddressLine2.Text = GetAddressLine2(person);
        lblContactInformation.Text = GetContactInformation(person);
        lblHouseStreet.Text = GetHouseStreet(house);
        lblHouseExtNo.Text = GetHouseExtNo(house);
        lblHouseIntNo.Text = GetHouseIntNo(house);
        lblBankName.Text = GetBankName(house);
        lblLoanNumber.Text = GetLoanNumber(house);
        lblBankAddress.Text = GetBankAddress(house);
        string policyNo = Session["policyno"] as string;
        UserInformation user = (UserInformation)User.Identity;
        bool canEditAgent = AdminGateway.Gateway.CanEditAgentInformation(
            user.Agent,
            policyNo,
            user.UserType
        );

        ShouldDisplayParentInvoice = policy.cantidadDePagos > 1;
        btnModifyAgent.Visible = canEditAgent;
        btnModifyAgent.Attributes.Add("onclick", "location.href = '" + VirtualPathUtility.ToAbsolute("~/Application/EditPolicy/Agent.aspx") + "'");
        LoadAgentInformation(policy.issuedBy);
    }

    private void LoadAgentInformation(int issuedBy)
    {
        UserInformationDB agentInfo = AdminGateway.Gateway.GetUserInformation(issuedBy);
        lblAgent.Text = agentInfo.AgentInformation.AgentName;
        lblBranch.Text = agentInfo.BranchInformation.BranchAddress;
        lblBranchPhone.Text = agentInfo.BranchUserPhone;
        lblUser.Text = agentInfo.UserName;
    }
}
