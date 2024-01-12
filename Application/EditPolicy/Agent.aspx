<%@ Page Title="" Language="C#" MasterPageFile="~/HouseMaster.master" AutoEventWireup="true" CodeFile="Agent.aspx.cs" Inherits="Application_EditPolicy_Agent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="workloadContent" Runat="Server">
<div id="headerApp" style="padding: 0 15px 0 15px; width: 90%; border: 1px solid #CCC;">
    Edit Agent Information: <%= Session["policyno"] %>
</div>
<div id="insuranceApp" style="padding: 15px; position: relative; top:-5px; width: 90%;">
<asp:UpdatePanel ID="updPanel" runat="server">
<ContentTemplate>
<table class="agentEditTable">
    <tr>
        <td><asp:Label ID="lblAgent" AssociatedControlID="ddlAgent" Text="Agent: " runat="server" /></td>
        <td><asp:DropDownList ID="ddlAgent" DataTextField="name" DataValueField="agent" OnSelectedIndexChanged="ddlAgent_SelectedIndexChanged" AutoPostBack="true" runat="server" /></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblBranch" AssociatedControlID="ddlBranch" Text="Branch: " runat="server" /></td>
        <td>
            <asp:DropDownList ID="ddlBranch" DataTextField="address" DataValueField="branch" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" AutoPostBack="true" runat="server" />
            <asp:RequiredFieldValidator ID="reqBranch" InitialValue="0" Display="Dynamic" Text="Required" ControlToValidate="ddlBranch" runat="server" />
        </td>
    </tr>
    <tr>
        <td></td>
        <td>
            <asp:Label ID="lblCityState" runat="server" /><br />
            <asp:Label ID="lblCountry" runat="server" /><br />
            <asp:Label ID="lblZipCode" runat="server" /><br />
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="lblUser" AssociatedControlID="ddlUser" Text="User: " runat="server" /></td>
        <td>
            <asp:DropDownList ID="ddlUser" DataTextField="name" DataValueField="cveUsuario" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged" AutoPostBack="true" runat="server" />
            <asp:RequiredFieldValidator ID="reqUser" InitialValue="0" Text="Required" Display="Dynamic" ControlToValidate="ddlUser" runat="server" />
            <asp:CustomValidator ID="valUser" Display="Dynamic" ControlToValidate="ddlUser" OnServerValidate="ddlUser_Validate" Text="Invalid user" runat="server" />
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="lblLicense" Text="License: " runat="server" /></td>
        <td><asp:Label ID="lblLicenseText" runat="server" /></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblPhone" Text="Phone: " runat="server" /></td>
        <td><asp:Label ID="lblPhoneText" runat="server" /></td>
    </tr>
</table>
</ContentTemplate>
</asp:UpdatePanel>
<div class="editPolicyButtons">
    <asp:Button ID="btnSave" Text="Save" OnClick="btnSave_Click" runat="server" />
    <asp:Button ID="btnCancel" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" runat="server" />
</div>
</div>
<asp:Label ID="lblUserHidden" Visible="false" runat="server" />
<asp:Label ID="lblBranchHidden" Visible="false" runat="server" />
<asp:Label ID="lblAgentHidden" Visible="false" runat="server" />



</asp:Content>

