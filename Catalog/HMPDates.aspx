<%@ Page Title="" Language="C#" MasterPageFile="~/HouseMaster.master" AutoEventWireup="true" CodeFile="HMPDates.aspx.cs" Inherits="Catalog_HMPDates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="workloadContent" Runat="Server">
<h1 class="pageHeader">HMP Dates</h1>
<p>
    You must specify the date range for which the users will not be able to generate a quote with HMP protection.
</p>
<table class="catalogTable">
    <tr class="catalogTableHeader">
        <th></th>
        <th><asp:Label ID="lblHMPStart" Text="Start Date" runat="server" /></th>
        <th><asp:Label ID="lblHMPEnd" Text="End Date" runat="server" /></th>
    </tr>
    <tr>
        <td><asp:Label ID="lblHMPDates" Text="HMP Inactive Dates" runat="server" /></td>
        <td><ajax:CalendarExtender ID="calStart" TargetControlID="txtStart" EnableViewState="false" runat="server" />
            <asp:TextBox ID="txtStart" runat="server" />
            <asp:CompareValidator ID="valStart" Display="Dynamic" Text="Invalid Date" Type="Date" Operator="DataTypeCheck" ControlToValidate="txtStart" runat="server" />
            <asp:RequiredFieldValidator ID="reqStart" Display="Dynamic" ControlToValidate="txtStart" Text="Required" runat="server" /></td>
        <td><ajax:CalendarExtender ID="calEnd" TargetControlID="txtEnd" EnableViewState="false" runat="server" />
            <asp:TextBox ID="txtEnd" runat="server" />
            <asp:CompareValidator ID="valEnd" Display="Dynamic" Text="Invalid Date" Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEnd" runat="server" />
            <asp:CompareValidator ID="CompareValidator1" Display="Dynamic" Text="Invalid Date" Type="Date" Operator="GreaterThan" ControlToCompare="txtStart" ControlToValidate="txtEnd" runat="server" />
            <asp:RequiredFieldValidator ID="reqEnd" Display="Dynamic" ControlToValidate="txtEnd" Text="Required" runat="server" /></td>
    </tr>
</table>
<asp:Button ID="btnHMPActive" OnClick="btnHMPActive_Click" Text="Save" runat="server" /><br />
<asp:TextBox ID="txtError" ForeColor="Red" Visible="false" Text="An invalid date or an unknown error ocurred, please verify your data" runat="server" />
</asp:Content>

