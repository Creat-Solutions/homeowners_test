<%@ Page Title="" Language="C#" MasterPageFile="~/HouseMaster.master" AutoEventWireup="true" CodeFile="BuyPolicy.aspx.cs" Inherits="BuyPolicy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="workloadContent" Runat="Server">

<script type="text/javascript">
    function policyClick() {
        var btnBuy = $get('<%= btnBuy.ClientID %>');
        btnBuy.disabled = true;
    }

    function mustPayWithCC(mustPay) {
        var chkCCQuery = $('#<%= chkCC.ClientID %>');
        if (chkCCQuery.size() > 0) {
            var chkCC = chkCCQuery[0];
            chkCC.checked = mustPay;
            chkCC.disabled = mustPay;
        }
    }


</script>




<asp:PlaceHolder ID="plhContent" runat="server" />
<br />



<div class="nofloat" style="font-size:larger;">

<asp:CheckBox ID="chkCC" Visible="false" Text="Do you want to pay with credit card? / &iquest;Desea pagar con tarjeta de cr&eacute;dito?" runat="server" />
</div>
<br />
<asp:Button ID="btnBuy" OnClick="btnClick" UseSubmitBehavior="false" OnClientClick="policyClick();" Text="Buy Policy" runat="server" /><asp:Button ID="btnCancel" UseSubmitBehavior="false" CausesValidation="false" OnClick="btnClick" Text="Cancel" runat="server" />
</asp:Content>

