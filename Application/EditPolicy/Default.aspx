<%@ Page Title="" Language="C#" MasterPageFile="~/HouseMaster.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Application_EditPolicy_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="workloadContent" Runat="Server">

<script type="text/javascript">
    function btnPDF_Click() {
        location.href = '/GetPoliza.ashx?poliza=<%= PolicyID %>';
        return false;
    }

    function btnInvoice_Click() {
        window.location.href = '/GetInvoice.ashx?id=<%= PolicyID %>';
        return false;
    }

    function btnParentInvoice_Click() {
        window.location.href = '<%= VirtualPathUtility.ToAbsolute("~/GetInvoice.ashx") %>?id=<%= PolicyID %>&fepadre=1';
        return false;
    }
    
</script>
<div id="headerApp" style="padding: 0 15px 0 15px; width: 90%; border: 1px solid #CCC;">
    Edit Policy: <%= PolicyNo %>
</div>
<div id="insuranceApp" style="padding: 15px; position: relative; top:-5px; width: 90%;">
    
    <div class="editPolicyMain style="padding: 0 15px 0 15px;">
        <div class="editPolicyLeft">
            <fieldset>
                <legend>Insured Person Information</legend>
                
                Name: <asp:Label ID="lblName" runat="server" /><br />
                <asp:Label ID="lblRFC" runat="server" /><br /><br />
                Address: <br />
                <div style="padding-left: 15px;">
                    <asp:Label ID="lblAddressLine1" runat="server" /><br />
                    <asp:Label ID="lblAddressLine2" runat="server" />
                </div>
                <br />
                <asp:Label ID="lblContact" Text="Contact Information: " runat="server" />
                <asp:Literal ID="lblContactInformation" runat="server" />
                <div style="float: right;">
                    <input type="button" onclick="location.href = '<%= VirtualPathUtility.ToAbsolute("~/Application/EditPolicy/Insured.aspx") %>'" value="Change" />
                </div>
            </fieldset>
        </div>
        <div class="editPolicyRight">
            <fieldset>
                <legend>Insured House Information</legend>
                Address: <br />
                <div style="padding-left: 15px;">
                    <asp:Label ID="lblHouseStreet" runat="server" /><br />
                    <asp:Label ID="lblHouseExtNo" runat="server" />
                    <asp:Label ID="lblHouseIntNo" runat="server" />
                </div>
                <br />
                Bank Information: <br />
                <div style="padding-left: 15px;">
                    <asp:Label ID="lblBankName" runat="server" /><br />
                    <asp:Label ID="lblLoanNumber" runat="server" /><br />
                    <asp:Label ID="lblBankAddress" runat="server" /><br />
                </div>
                <div style="float: right;">
                    <input type="button" onclick="location.href = '/Application/EditPolicy/Insured.aspx?step=2'" value="Change" />
                </div>
            </fieldset>
        </div>
        <div class="editPolicyLeft">
            <fieldset id="fsAgentInformation">
                <legend>Agent Information</legend>
                Agent: <asp:Label ID="lblAgent" runat="server" /><br />
                Branch: <asp:Label ID="lblBranch" runat="server" /><br />
                Branch Phone No.: <asp:Label ID="lblBranchPhone" runat="server" /><br />
                Branch User: <asp:Label ID="lblUser" runat="server" />
                <div style="float: right;">
                    <input type="button" id="btnModifyAgent" runat="server" onclick="" value="Change" />
                </div>      
            </fieldset>
        </div>
    </div>
    <br style="clear: both;" />
    <div class="editPolicyButtons">
        <input id="btnPDF" type="button" value="Get PDF" onclick="return btnPDF_Click();" />
        <% if ((bool)Session["canGetInvoice"]) { %>
            <input id="btnInvoice" type="button" value="Get Invoice" onclick="return btnInvoice_Click();" />
            <% if (ShouldDisplayParentInvoice) { %>
                <input id="btnParentInvoice" type="button" value="Get General Invoice" onclick="return btnParentInvoice_Click();" />
            <% } %>
        <% } %>
    </div>
</div>

</asp:Content>

