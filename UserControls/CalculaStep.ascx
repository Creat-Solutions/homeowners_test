<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CalculaStep.ascx.cs" Inherits="UserControls_CalculaStep" %>
<script type="text/javascript">
    function DiscountSurchargeValidate(obj, args) {
        var descuento = 0;
        var recargo = 0;
        var txtDescuento = $get('<%= txtDescuento.ClientID %>');
        var txtRecargo = $get('<%= txtRecargo.ClientID %>');
        if (txtDescuento !== null) {
            descuento = parseFloat(txtDescuento.value);
            if (isNaN(descuento)) {
                descuento = 0;
            }
        } else {
            descuento = <%= GetDescuento().ToString("F2") %>;
        }
        if (txtRecargo !== null) {
            recargo = parseFloat(txtRecargo.value);
            if (isNaN(recargo)) {
                recargo = 0;
            }
        } else {
            recargo = <%= GetRecargo().ToString("F2") %>;
        }
        args.IsValid = (descuento == 0 && recargo == 0) ||
                       (descuento != 0 && recargo == 0) ||
                       (descuento == 0 && recargo != 0);
    }
    
    function PrintCalculo() {
        var newWindow = window.open('/Application/Calcula.aspx?quote=<%= QuoteID %>', '_blank');
        newWindow.focus();
        return false;
    }
    
    function GeneratePDF() {
        location.href = '/CalculaPDF.ashx?quoteid=<%= QuoteID %>';
        return false;
    }
</script>
<div id="print">
    <input type="button" value="Print it" onclick="return PrintCalculo();" />
    <input type="button" value="Generate PDF" onclick="return GeneratePDF();" />
</div>
<asp:PlaceHolder ID="plhCalcula" runat="server" />
<div id="fieldBox">
    <div id="divDiscount" class="row" runat="server">
        <asp:Label ID="lblDiscount" AssociatedControlID="txtDescuento" Text="Discount: " runat="server" />
        <asp:TextBox ID="txtDescuento" runat="server" /> %
        <asp:CompareValidator ID="valDescuento" ControlToValidate="txtDescuento" Operator="DataTypeCheck" Type="Double" Display="Dynamic" Text="Invalid value" runat="server" />
        <asp:CompareValidator ID="rangeDescuento" ControlToValidate="txtDescuento" Operator="GreaterThanEqual" Type="Double" ValueToCompare="0" Text="Invalid value" Display="Dynamic" runat="server" />
        <asp:CompareValidator ID="rangeDescuento2" ControlToValidate="txtDescuento" Operator="LessThanEqual" Type="Double" ValueToCompare="100" Text="Invalid value" Display="Dynamic" runat="server" />
    </div>
    <div id="divDescuento" class="rowes" runat="server">
        <asp:Label ID="lblDescuento" Text="Descuento: " runat="server" />
    </div>
    <div id="divSurcharge" class="row" runat="server">
        <asp:Label ID="lblSurcharge" AssociatedControlID="txtRecargo" Text="Surcharge: " runat="server" />
        <asp:TextBox ID="txtRecargo" runat="server" /> %
        <asp:CompareValidator ID="valRecargo" ControlToValidate="txtRecargo" Operator="DataTypeCheck" Type="Double" Display="Dynamic" Text="Invalid value" runat="server" />
        <asp:CompareValidator ID="cmpRecargo" ControlToValidate="txtRecargo" Operator="GreaterThanEqual" Type="Double" ValueToCompare="0" Text="Invalid value" Display="Dynamic" runat="server" />
        <asp:CompareValidator ID="cmpRecargo2" ControlToValidate="txtRecargo" Operator="LessThanEqual" Type="Double" ValueToCompare="100" Text="Invalid value" Display="Dynamic" runat="server" />
    </div>
    <div id="divRecargo" class="rowes" runat="server">
        <asp:Label ID="lblRecargo" Text="Recargo: " runat="server" />
    </div>
    <asp:CustomValidator ID="valDiscountSurcharge" ClientValidationFunction="DiscountSurchargeValidate" OnServerValidate="valDiscountSurcharge_Validate" Text="Cannot have both a discount and surcharge" Display="Dynamic" runat="server" /><br />
    <asp:Button ID="btnRecalculate" Text="Recalculate" OnClick="btnRecalculate_Click" runat="server" />
</div>
