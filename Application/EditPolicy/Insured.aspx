<%@ Page Title="" Language="C#" MasterPageFile="~/HouseMaster.master" AutoEventWireup="true" CodeFile="Insured.aspx.cs" Inherits="Application_EditPolicy_Insured" %>

<asp:Content ID="Content1" ContentPlaceHolderID="workloadContent" Runat="Server">
<div id="headerApp" style="padding: 0 15px 0 15px; width: 90%; border: 1px solid #CCC;">
    Edit Insured Information: <%= Session["policyno"] %>
</div>
<div id="insuranceApp" style="padding: 15px; position: relative; top:-5px; width: 90%;">
    <asp:PlaceHolder ID="placeHolder" EnableViewState="false" runat="server" />
    <div class="editPolicyButtons">
        <asp:Button ID="btnSave" Text="Save" OnClick="btnSave_Click" runat="server" />
        <asp:Button ID="btnCancel" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" runat="server" />
    </div>
</div>


</asp:Content>

