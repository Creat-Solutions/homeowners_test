<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DateRange.aspx.cs" Inherits="Application_DateRange" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Date Range</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptManager" EnablePageMethods="true" runat="server" />
    
    <script type="text/javascript">
        

        function OnFailure(error) {
            alert("Could not save or an unknown error occurred!");
            return false;
        }

        function DateToday() {
            var today = new Date();
            today.setHours(0, 0, 0, 0);
            return today;
        }

        function ValidateStartDate(obj, args) {
            var time = Date.parse(args.Value);
            if (isNaN(time)) {
                args.IsValid = false;
                return;
            }
            var today = DateToday().getTime() - (<%= RETROACTIVE_DAYS %> * 24 * 60 * 60 * 1000);
            args.IsValid = time >= today;
        }
        
        function OnSuccess(result) {
            if (result != "0") {
                $get('<%= lblEndResult.ClientID %>').innerHTML = result;
                top.location.href = '/Application/BuyPolicy.aspx';
                return true;
            } else {
                OnFailure(null);
            }
        }

        function NextWindow() {
            if(Page_IsValid){
                PageMethods.SaveDateRange($get('<%= txtStart.ClientID %>').value, OnSuccess, OnFailure);
                return true;
            }
            return false;
        }
        
        function CancelWindow() {
            top.location.href = '/Application/Load.aspx?option=clear';
            return true;
        }
        
    </script>
    <div>
        <table>
            <tr>
                <td><asp:Label ID="lblStart" Text="Start Date: " runat="server" /></td>
                <td><ajax:CalendarExtender ID="calStart" TargetControlID="txtStart" runat="server" />
                    <asp:TextBox ID="txtStart" runat="server" />
                    <asp:RequiredFieldValidator ID="reqStart" Text="Required" Display="Dynamic" ControlToValidate="txtStart" runat="server" />
                    <asp:CompareValidator ID="valStart" Text="Invalid Date" Display="Dynamic" ControlToValidate="txtStart" Operator="DataTypeCheck" Type="Date" runat="server" />
                    <asp:CustomValidator ID="valCStart" Text="Invalid Date" Display="Dynamic" ClientValidationFunction="ValidateStartDate" ControlToValidate="txtStart" OnServerValidate="valStart_Validate" runat="server" />
                    </td>
            </tr>
            <tr>
                <td><asp:Label ID="lblEnd" Text="End Date: " runat="server" /></td>
                <td><asp:Label ID="lblEndResult" runat="server" /></td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="divNoPesosIssue" runat="server" style="font-weight: bold;">
                        Although you will be able to issue the policy, you won't be able to pay it because it was quoted with Mexican Pesos as currency. Please contact your account supervisor so he can inform you
                        of other payment options.
                        <br /><br />
                        Aunque la p&oacute;liza podr&aacute; ser emitida, no podr&aacute ser pagada debido a que no se ofrece dicha opci&oacute;n para cotizaciones en pesos mexicanos. Favor de contactar al encargado de su
                        cuenta para que le informe de otras formas de pago.
                    </div>
                    <div id="divNoPesos" runat="server" style="font-weight: bold;">
                        We don't offer the option to buy a policy online with Mexican Pesos as currency. Please contact your account supervisor so he can inform you
                        of other payment options.
                        <br /><br />
                        No se ofrece la opci&oacute;n de compra de p&oacute;liza en l&iacute;nea con Pesos Mexicanos como moneda. Favor de contactar al encargado de su
                        cuenta para que le informe de otras formas de pago.
                    </div>
                </td>
            </tr>
        </table>
        <input type="button" id="btnNext" onclick="return NextWindow();" value="Next"/>
        <asp:Button ID="btnCancel" CausesValidation="false" OnClientClick="return CancelWindow();" Text="Cancel" runat="server" />
    </div>
    </form>
</body>
</html>
