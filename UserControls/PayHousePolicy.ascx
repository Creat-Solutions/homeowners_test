<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PayHousePolicy.ascx.cs" Inherits="UserControls_PayHousePolicy" %>
<%--<script type='text/javascript' src="https://openpay.s3.amazonaws.com/openpay.v1.js"></script>
<script type="text/javascript" src="https://openpay.s3.amazonaws.com/openpay-data.v1.min.js"></script>--%>
<script type="text/javascript">

    //var isMXcard = false;
    var alReadyPay = false;

    <%--function changeBuyStatus(cond) {
        var btn = $get('<%= Parent.FindControl("btnBuy").ClientID %>');
        btn.disabled = !cond;
    }--%>

    function trimCC(number) {
        return number.replace(/\D/g,"");
    }

    function validateLuhn(number, length) {
        var sum = 0, parity = length % 2;
        for (i = 0; i < length; i++) {
            var digit = number.charAt(i);
            if (i % 2 == parity) {
                digit = digit * 2;
                if (digit > 9)
                    digit = digit - 9;
            }
            sum = sum + parseInt(digit);
        }
        return sum % 10 == 0;
    }

    function verifyClientCC(src, args) {
        var type = $get('<%= ddlType.ClientID %>').value;
        if (args.Value.indexOf("xxxx") != -1) {
            args.IsValid = true;
            return;
        }
        var number = trimCC(args.Value);
        var length = number.length;
        args.IsValid = true;
        switch (type) {
            case "VISA":
                if (length != 16)
                    args.IsValid = false;
                else
                    args.IsValid = number.charAt(0) == 4;
                break;
            case "MC":
                if (length != 16)
                    args.IsValid = false;
                else
                    args.IsValid = !(number.charAt(0) != 5 || number.charAt(1) == 0 || number.charAt(1) > 5);
                break;
            case "DISCOVER":
                if (length != 16)
                    args.IsValid = false;
                else
                    args.IsValid = !(number.charAt(0) != 6 || number.charAt(1) != 0 || number.charAt(2) != 1 || number.charAt(3) != 1);
                break;
            case "AMEX":
                if (length != 15)
                    args.IsValid = false;
                else
                    args.IsValid = !(number.charAt(0) != 3 || (number.charAt(1) != 4 && number.charAt(1) != 7));
                break;
            default:
                args.IsValid = false;
                return;
        }
        if (!args.IsValid)
            return;
        args.IsValid = validateLuhn(number, length);
    }

    function DisableValidators() {
        Page_Validators = [];
    }

    function DisableBuyBtn() {
        var btnBuy = $get('<%= Parent.FindControl("btnBuy").ClientID %>');
        if (btnBuy == null) {
            alert("btnBuy is null");
            return;
        }
        btnBuy.disabled = true;
    }

    async function CheckValidatedPage() {
        if (Page_Validators.length == 0)
            return;
        var btnBuy = $get('<%= Parent.FindControl("btnBuy").ClientID %>');
        if (btnBuy == null) {
            alert("btnBuy is null");
        }
        if (!Page_IsValid) {
            for (x in Page_Validators) {
                ValidatorValidate(Page_Validators[x]);
            }
        }

        //if (isMXcard)
        //    await callCreateToken();

        btnBuy.disabled = Page_IsValid;
        return Page_IsValid;
    }

    async function successCard(_responseData) {
        debugger;
        console.log('successCard ', _responseData.data.id);
        <%--$('#<%= txtTokenId.ClientID %>').val(_responseData.data.id);--%>
        callDeviceSessionID();
        <%--var btnBuy = $get('<%= Parent.FindControl("btnBuy").ClientID %>');
        btnBuy.trigger('click');--%>
    };

    async function errorCard(_errorResponseData) {
        console.log(_errorResponseData);
        alert('Ocurrio un error: ' + _errorResponseData);
    };

    async function callCreateToken() {
        var _id = '', _apiKey = '', _data = null, _dataCard = '', direccion = '';
        direccion = 'reforma 522';
        <%--_id = '<%= MERCHANT_ID %>';--%>
        <%--_apiKey = '<%= PUBLIC_KEY %>';--%>
        <%--OpenPay.setSandboxMode(<%= IS_SANDBOX %>);--%>

        try {
            OpenPay.setId(_id);
            OpenPay.setApiKey(_apiKey);
            OpenPay.token.create({
                'card_number': '4111111111111111',
                'holder_name': 'eric diaz perez',
                'expiration_year': '23',
                'expiration_month': '01',
                'cvv2': '000',
                'address': {
                    'line1': 'reforma 522',
                    'line2': 'puebla',
                    'line3': 'puebla',
                    'state': 'puebla',
                    'city': 'puebla',
                    'postal_code': '72000',
                    'country_code': 'MX'
                }
                        <%--'card_number': $('#<%= txtCardCode.Text.Trim() %>').val(),
                        'holder_name': (String($('#<%= txtNameCC.ClientID %>').val())),
                        'expiration_year': -1 * (2000 - Number($('#<%= ddlExpYear.ClientID%>').val())),
                        'expiration_month': $('#<%= ddlExpMonth.ClientID%>').val(),
                        'cvv2': $('#<%=txtSecurityCode.ClientID %>').val(),
                        'address': {
                            'line1': String($('#<%=hdCompleteStreet.ClientID%>').val()),
                            'line2': String($('#<%=hdCity.ClientID%>').val()),
                            'line3': String($('#<%=hdHouseEstate.ClientID%>').val()),
                            'state': String($('#<%=hdState.ClientID%>').val()),
                            'city': String($('#<%=hdCity.ClientID%>').val()),
                            'postal_code': String($('#<%=hdZipCode.ClientID%>').val()),
                            'country_code': 'MX'
                        }--%>
                    }, await successCard, await errorCard);
        } catch (e) {
            alert('Falló la requisición (información incorrecta)');
        }
    };

        async function callDeviceSessionID() {
            var _id = '', _apiKey = '', _data = null, _dataCard = '';
            <%--_id = '<%= MERCHANT_ID %>';--%>
            <%--_apiKey = '<%= PUBLIC_KEY %>';--%>
            <%--OpenPay.setSandboxMode(<%= IS_SANDBOX %>);--%>
            var deviceDataId = OpenPay.deviceData.setup("workloadContent");
            console.log('Device Data: ', deviceDataId);
            <%--$('#<%= txtDeviceId.ClientID %>').val(deviceDataId);--%>
    }

    <%--function changeStatus() {
        isMXcard = !isMXcard;

        var btnBuy = $get('<%= Parent.FindControl("btnBuy").ClientID %>');
        var btnPayment = $('#<%=btnPayMX%>');
        
        if (btnBuy == null) {
            alert("btnBuy is null");
        }

        if (isMXcard) {
            btnPayment.visible = true;
            //$('#<%=btnPayMX%>').visible(true);
            btnBuy.disabled = true;
        }
        else {
            //$('#<%=btnPayMX%>').visible(false);
            btnBuy.disabled = false;
        }
    }--%>

    function Alerta(valor1, valor2, valor3) {
        var val1 = valor1;
        var val2 = valor2;
        var val3 = valor3;
        window.alert(val1 + val2 + val3);
    }
    
</script>
<div id="ccPolicy">
Pay Policy

Policy Information - <asp:Label ID="lblClient" runat="server" />
<table>
    <tr>
        <td>Address: </td>
        <td><asp:Label ID="lblAddress" runat="server" /></td>
    </tr>
    <tr>
        <td>Building Value: </td>
        <td><asp:Label ID="lblValue" runat="server" /></td>
    </tr>
    <tr>
        <td>Coverage Date Rate: </td>
        <td><asp:Label ID="lblStartDate" runat="server" /> - <asp:Label ID="lblEndDate" runat="server" /> (Central Time)</td>
    </tr>
    <tr>
        <td>CSL: </td>
        <td><asp:Label ID="lblCSL" runat="server" /></td>
    </tr>
    <tr>
        <td>Total: </td>
        <td><asp:Label ID="lblTotal" runat="server" /></td>
    </tr>
</table>

IMPORTANT: Click 'Buy Policy' only once and wait for confirmation of your payment. Thank you for your patience.
    
    <%--<asp:TextBox ID="txtTokenId" runat="server" Style="display: none"></asp:TextBox>--%>
    <%--<asp:TextBox ID="txtDeviceId" runat="server" Style="display: none"></asp:TextBox>--%>
    
<table>
    <%--<tr>
        <td><asp:Label ID="lblTypeCC" Text="Select credit card payment:" runat="server"/></td>
        <td>
            <asp:RadioButton ID="rbAmericanCC" CssClass="nofloat" Text="American Card" GroupName="defaultCC" Checked="true" runat="server" onClick="changeStatus()"/>
            <asp:RadioButton ID="rbMexicanCC" Text="Mexican Card" GroupName="defaultCC" CssClass="nofloat" runat="server" onClick="changeStatus()"/>
        </td>
    </tr>--%>
    <tr>
        <td><asp:Label ID="lblCCType" AssociatedControlID="ddlType" Text="Credit Card Type:" runat="server" /></td>
        <td><asp:DropDownList ID="ddlType" DataTextField="description" DataValueField="code" runat="server" /></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblCCNumber" AssociatedControlID="txtCCNumber" Text="Credit Card Number:" runat="server" /></td>
        <td><asp:TextBox ID="txtCCNumber" MaxLength="16" runat="server" />
        <asp:RequiredFieldValidator ID="reqCCNumber" ControlToValidate="txtCCNumber" Text="Required" Display="Dynamic" runat="server" />
        <asp:CustomValidator ID="valCCNumber" ControlToValidate="txtCCNumber" Display="Dynamic" Text="Credit Card Number is not valid" runat="server" />
        </td>
        <%--<td><asp:TextBox ID="txtCCNumber" MaxLength="16" runat="server" />
        <asp:RequiredFieldValidator ID="reqCCNumber" ControlToValidate="txtCCNumber" Text="Required" Display="Dynamic" runat="server" />
        <asp:CustomValidator ID="valCCNumber" ClientValidationFunction="verifyClientCC" OnServerValidate="CreditCardValidate" ControlToValidate="txtCCNumber" Display="Dynamic" Text="Credit Card Number is not valid" runat="server" />
        </td>--%>
    </tr>
    <tr>
        <td><asp:Label ID="lblExpiration" AssociatedControlID="ddlExpMonth" Text="Expiration Date: " runat="server" /></td>
        <td><asp:DropDownList ID="ddlExpMonth" runat="server" /><asp:DropDownList ID="ddlExpYear" runat="server" /></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblCardCode" AssociatedControlID="txtCardCode" Text="Security Code: " runat="server" /></td>
        <td><asp:TextBox ID="txtCardCode" runat="server" />
        <asp:RequiredFieldValidator ID="reqCardCode" ControlToValidate="txtCardCode" Text="Required" Display="Dynamic" runat="server" />
        </td>
    </tr>
</table>
Save this credit card as the default card on your Mexicard account: <asp:RadioButton ID="rbYes" CssClass="nofloat" Text="Yes" GroupName="defaultCard" runat="server" /><asp:RadioButton ID="rbNo" Text="No" GroupName="defaultCard" CssClass="nofloat" Checked="true" runat="server" />
<br />
<asp:TextBox ID="txtDisclaimer" Columns="75" Rows="20" ReadOnly="true" TextMode="MultiLine" runat="server">
The following Terms and Conditions govern your use of MacAfee and Edwards, Inc Internet payment service on our web site. By using the Internet payment service, you agree to be bound by the following Terms and Conditions.

1. You Authorize MacAfee and Edwards, Inc. to charge to your credit card the amount of the insurance premium. Once the policy has started its inception MacAfee and Edwards, Inc can not flat cancelled your policy.
 
2. Payment will be considered received by MacAfee and Edwards, Inc as of the date and time submitted by you, using the MacAfee and Edwards, Inc web site. 

3. Your payment cannot be submitted until you agree to these Terms and Conditions. Once you agree and submit your payment, the amount will be deducted from your Credit Card.

4. Please retain the tracking number (displayed on the Payment Confirmation Page) for your reference.

5. All policies issue for 5 or less days are non refundable and are not subject to cancellation. Premium, fees and taxes and are fully earned once the policy has been submitted .

In the event you believe there is an error with the payment, please contact us by calling (800) 334-7950. Your policy number and payment tracking number (#5 above) will be needed to research your payment.


From time to time, MacAfee and Edwards, Inc may modify these Terms and Conditions without notice. Any modifications to the Terms and Conditions will be posted on the web site.
</asp:TextBox>
<br />
<asp:RadioButton id="rbAccept" CssClass="nofloat" onclick="changeBuyStatus(true);" Text="I agree to the Terms and Conditions" GroupName="saveAccept" runat="server" /><br />
<asp:radiobutton id="rbNoAccept" CssClass="nofloat" onclick="changeBuyStatus(false);" Text="I do not agree" GroupName="saveAccept" Checked="True" runat="server" /><br />
If you do not agree to the Terms and Conditions please click the cancel button
</div>
<br />
<%--<asp:Button ID="btnPayMX" UseSubmitBehavior="false" OnClientClick="callCreateToken();" Visible="true" Text="Validate Payment" runat="server" />--%>