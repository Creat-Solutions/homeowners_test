<%@ Control Language="C#" AutoEventWireup="true" CodeFile="QuickQuoteStep4.ascx.cs" Inherits="UserControls_QuickQuoteStep4" %>
<script type="text/javascript">

    function ChangeUSA(usa) {

        var txtState = $get('<%= txtState.ClientID %>');
        var ddlState = $get('<%= ddlState.ClientID %>');
        if (usa) {
            txtState.style.display = 'none';
            ddlState.style.display = 'inline';
        } else {
            txtState.style.display = 'inline';
            ddlState.style.display = 'none';
        }
    
    }

    function ValidateState(obj, args) {

        var txtState = $get('<%= txtState.ClientID %>');
        var ddlState = $get('<%= ddlState.ClientID %>');
        var chkNotUSA = $get('<%= btnNotUSA.ClientID %>');
        if (chkNotUSA.checked) {
            args.IsValid = txtState.value.length > 0;
        } else {
            args.IsValid = ddlState.selectedIndex > 0;
        }
    
    }

function keyLimit(actualid, nextid, limit) {
    var actual = document.getElementById(actualid);
    var next = null;
    if (nextid)
        next = document.getElementById(nextid);
    if (!(actual && limit))
        return;
    if (actual.value.length >= limit && next)
        next.focus();
        
}

function changePersonType(sobj) {
    if (!sobj)
        return;
    var apobj = $get('<%= this.txtLastName.ClientID %>');
    var validator = $get('<%= this.reqLast.ClientID %>');
    var lblLastName = $get('<%= this.lblLastName.ClientID %>');
    var lblApellido = $get('<%= this.lblApellidos.ClientID %>');
    var lblTaxID = $get('<%= lblTaxID.ClientID %>');
    if (!(apobj && validator))
        return;
    if (sobj.selectedIndex == "0") {
        apobj.disabled = false;
        apobj.style.display = 'block';
        lblLastName.style.display = 'block';
        lblApellido.style.display = 'block';
        lblTaxID.innerHTML = "Tax ID (AAAA######AAA): ";
        ValidatorEnable(validator, true);
    } else {
        apobj.disabled = true;
        apobj.style.display = 'none';
        lblLastName.style.display = 'none';
        lblApellido.style.display = 'none';
        lblTaxID.innerHTML = "Tax ID (AAA######AAA): ";
        ValidatorEnable(validator, false);
    }
}

function validateLastName(obj, args)
{
    var txtLastName = $get('<%= this.txtLastName.ClientID %>');
    var ddlType = $get('<%= this.ddlPersonType.ClientID %>');
    
    if (!(txtLastName && ddlType)) {
        args.IsValid = false;
        return;
    }
    args.IsValid = ddlType.selectedIndex == 1 || txtLastName.value.length > 0;

}

function validateRFC(obj, args) {
    var txtRFC = $get('<%= this.txtRFC.ClientID %>');
    var ddlType = $get('<%= this.ddlPersonType.ClientID %>');

    if (!(txtRFC && ddlType)) {
        args.IsValid = false;
        return;
    }
    if (txtRFC.value.length == 0) {
        args.IsValid = true;
        return;
    }
    
    var regExMatcher = null;
    if (ddlType.selectedIndex == 1) {
        /* si es persona moral */
        regExMatcher = /^[A-Za-z]{3}\d{2}(0[1-9]|1[0-2])(0[1-9]|1\d|2\d|3[01])[A-Za-z0-9]{3}$/;
    } else {
        regExMatcher = /^[A-Za-z]{4}\d{2}(0[1-9]|1[0-2])(0[1-9]|1\d|2\d|3[01])[A-Za-z0-9]{3}$/;
    }
    args.IsValid = regExMatcher.test(txtRFC.value);
}

var saving = false;

function SaveInformation() {
    var name = $.trim($('#<%= txtName.ClientID %>').val());
    var lastName = $.trim($('#<%= txtLastName.ClientID %>').val());
    var personType = $('#<%= ddlPersonType.ClientID %>').val();
    var rfc = $.trim($('#<%= txtRFC.ClientID %>').val());
    var email = $.trim($('#<%= txtEmail.ClientID %>').val());
    var phone = $.trim($('#<%= txtAreaPhone.ClientID %>').val() + $('#<%= txtPhone1.ClientID %>').val() + $('#<%= txtPhone2.ClientID %>').val());
    var addInsured = $.trim($('#<%= txtAdditionalInsured.ClientID %>').val());
    var address = $.trim($('#<%= txtAddress.ClientID %>').val());
    var state = $('#<%= ddlState.ClientID %>').is(':visible') ? $('#<%= ddlState.ClientID %>').val() : '0';
    var stateText = $.trim($('#<%= txtState.ClientID %>'));
    var liveOutsideMexico = $('#<%= btnYes.ClientID %>').is(':checked');
    var city = $.trim($('#<%= txtCity.ClientID %>').val());
    var zipCode = $.trim($('<%= txtZip.ClientID %>').val());

    $('#<%= btnGenerateCalculo.ClientID %>, #<%= btnBuyQuote.ClientID %>, #<%= btnPrintQuote.ClientID %>').hide();
    if (name.length === 0 || (personType == '0' && lastName.length === 0) || 
        (liveOutsideMexico && (address.length === 0 || stateText.length === 0 || city.length === 0 || zipCode.length === 0)) ) {
        return;
    }

    var clientInfo = {
        Name: name,
        LastName: lastName,
        PersonType: personType,
        RFC: rfc,
        Email: email,
        Phone: phone,
        AdditionalInsured: addInsured,
        Address: address,
        State: Number(state),
        StateText: stateText,
        City: city,
        ZipCode: zipCode,
        LiveOutsideMexico: liveOutsideMexico
    };

    saving = true;
    $.ajax({
        url: '<%= VirtualPathUtility.ToAbsolute("~/SaveClientInfo.ashx") %>',
        data: JSON.stringify(clientInfo),
        type: 'POST',
        processData: false,
        contentType: 'application/json',
        async: false,
        success: function() {
            saving = false;
            $('#<%= btnGenerateCalculo.ClientID %>, #<%= btnBuyQuote.ClientID %>, #<%= btnPrintQuote.ClientID %>').show();
        }
    });

}

$(document).ajaxComplete(function() {
    if (!saving) {
        // refresh our 
        $(':text, #<%= ddlPersonType.ClientID %>, #<%= ddlState.ClientID %> :radio').unbind('change');
        $(':text, #<%= ddlPersonType.ClientID %>, #<%= ddlState.ClientID %> :radio').change(SaveInformation);
    }
});

$(function() {
    if ($('#<%= txtName.ClientID %>').val() === '') {
        $('#<%= btnGenerateCalculo.ClientID %>, #<%= btnBuyQuote.ClientID %>, #<%= btnPrintQuote.ClientID %>').hide();
    } else {
        $('#<%= btnGenerateCalculo.ClientID %>, #<%= btnBuyQuote.ClientID %>, #<%= btnPrintQuote.ClientID %>').show();
    }
    $('#<%= btnGenerateCalculo.ClientID %>').click(function(event) {
        event.preventDefault();
        location.href = '<%= VirtualPathUtility.ToAbsolute("~/CalculaPDF.ashx") %>?quoteid=<%= QuoteID %>';
    });

    $('#<%= btnBuyQuote.ClientID %>').click(function(event) {
        event.preventDefault();
        location.href = '<%= VirtualPathUtility.ToAbsolute("~/Application/Load.aspx") %>?option=buyquote&id=<%= QuoteID %>';
    });
    
    $('#<%= btnPrintQuote.ClientID %>').click(function(event) {
        event.preventDefault();
        location.href = '<%= VirtualPathUtility.ToAbsolute("~/GetCotiza.ashx") %>?quote=<%= QuoteID %>';
    });

    $(':text, #<%= ddlPersonType.ClientID %>, #<%= ddlState.ClientID %> :radio').change(SaveInformation);
});

</script>
<table width="100%">
<tr>
<td width="50%" valign="top">

<div class="row row1">
    <asp:Label ID="lblPersonType" AssociatedControlID="ddlPersonType" Text="*Person type: " runat="server" />
    <asp:DropDownList ID="ddlPersonType" onchange="changePersonType(this);" runat="server">
        <asp:ListItem Text="Individual / Persona f&iacute;sica" Value="0" />
        <asp:ListItem Text="Corporation / Persona moral" Value="1" />
    </asp:DropDownList>
</div>
<div class="rowes row1">
    <asp:Label ID="lblTipoPersona" Text="Tipo de persona" runat="server" />
</div>
<div class="row row1">
    <asp:Label ID="lblName" AssociatedControlID="txtName" Text="*Name: " runat="server" />
    <asp:TextBox ID="txtName" MaxLength="100" runat="server" />
    <asp:RequiredFieldValidator ID="reqName" Text="Required" Display="Dynamic" ControlToValidate="txtName" runat="server" />
    <asp:RegularExpressionValidator ID="cmpName" Text="Invalid characters (only alphanumeric)." ControlToValidate="txtName" Display="Dynamic" ValidationExpression="^[a-zA-Z0-9\s\.]+$" runat="server" />
    <user:LengthValidator ID="valName" Text="Too many characters" MaxLength="100" Display="Dynamic" ControlToValidate="txtName" runat="server" />
</div>
<div class="rowes row1">
    <asp:Label ID="lblNombre" Text="Nombre" runat="server" />
</div>
<div class="row row1">
    <asp:Label ID="lblLastName" AssociatedControlID="txtLastName" Text="*Last Name: " runat="server" />
    <asp:TextBox ID="txtLastName" EnableViewState="false" MaxLength="64" runat="server" />
    <asp:CustomValidator ID="reqLast" Text="Required" Display="Dynamic" OnServerValidate="txtLastName_ServerValidate" ClientValidationFunction="validateLastName" ControlToValidate="txtLastName" runat="server" />
    <asp:RegularExpressionValidator ID="cmpLast" Text="Invalid characters (only alphanumeric)." ControlToValidate="txtLastName" Display="Dynamic" ValidationExpression="^[a-zA-Z0-9\s\.]+$" runat="server" />
    <user:LengthValidator ID="valLast" Text="Too many characters" MaxLength="64" Display="Dynamic" ControlToValidate="txtLastName" runat="server" />
</div>
<div class="rowes row1">
    <asp:Label ID="lblApellidos" Text="Apellido" runat="server" />
</div>
<div class="row row1">
    <asp:Label ID="lblTaxID" AssociatedControlID="txtRFC" Text="Tax ID (AAAA######AAA): " runat="server" />
    <asp:TextBox ID="txtRFC" EnableViewState="false" MaxLength="13" runat="server" />
    <asp:CustomValidator ID="valRFC" Display="Dynamic" ClientValidationFunction="validateRFC" OnServerValidate="valRFC_ServerValidate" ControlToValidate="txtRFC" Text="Invalid format. Check for valid date." runat="server" />
    <user:LengthValidator ID="lenRFC" Display="Dynamic" MaxLength="13" Text="Invalid Format." ControlToValidate="txtRFC" runat="server" />
</div>
<div class="rowes row1">
    <asp:Label ID="lblRFC" Text="RFC" runat="server" />
</div>
<div class="row row1">
    <asp:Label ID="lblEmail" AssociatedControlID="txtEmail" Text="Email: " runat="server" />
    <asp:TextBox ID="txtEmail" Columns="32" MaxLength="128" runat="server" />
    <user:LengthValidator ID="valEmail" Text="Too many characters" MaxLength="128" Display="Dynamic" ControlToValidate="txtEmail" runat="server" />
</div>
<div class="rowes row1">
    <asp:Label ID="lblCorreoE" Text="Correo electr&oacute;nico" runat="server" />
</div>
<div class="row row1">
    <asp:Label ID="lblPhone" AssociatedControlID="txtAreaPhone" Text="Phone No.: " runat="server" />
    (<asp:TextBox ID="txtAreaPhone" MaxLength="3" Columns="3" runat="server" />)
    <asp:TextBox ID="txtPhone1" MaxLength="3" Columns="3" runat="server" />-<asp:TextBox ID="txtPhone2" MaxLength="4" Columns="4" runat="server" />
    <asp:CompareValidator ID="comPhoneA" Display="Dynamic" Text="Area code is not valid. " ControlToValidate="txtAreaPhone" Type="Integer" Operator="DataTypeCheck" runat="server" />
    <asp:CompareValidator ID="comPhone1" Display="Dynamic" Text="Telephone Prefix number is not valid. " ControlToValidate="txtPhone1" Type="Integer" Operator="DataTypeCheck" runat="server" />
    <asp:CompareValidator ID="comPhone2" Display="Dynamic" Text="Telephone Suffix number is not valid." ControlToValidate="txtPhone2" Type="Integer" Operator="DataTypeCheck" runat="server" />
</div>
<div class="rowes row1">
    <asp:Label ID="lblTelefono" Text="N&uacute;mero de Tel&eacute;fono" runat="server" />
</div>

<div class="row row1">
    <asp:Label ID="lblAdditionalInsured" AssociatedControlID="txtAdditionalInsured" Text="Additional Insured: " runat="server" />
    <asp:TextBox ID="txtAdditionalInsured" MaxLength="100" runat="server" />
    <asp:RegularExpressionValidator ID="cmpAdditional" Text="Invalid characters (only alphanumeric)." ControlToValidate="txtAdditionalInsured" Display="Dynamic" ValidationExpression="^[a-zA-Z0-9\s\.]+$" runat="server" />
    <user:LengthValidator ID="lenAdditionalInsured" MaxLength="100" Display="Dynamic" Text="Input too long" ControlToValidate="txtAdditionalInsured" runat="server" />
</div>
<div class="rowes row1">
    <asp:Label ID="lblAseguradoAdicional" Text="Asegurado adicional" runat="server" />
</div>


<asp:UpdatePanel runat="server">
<ContentTemplate>
    <div>
    Does the insured live outside of Mexico/&iquest;El asegurado vive fuera de M&eacute;xico?<br />
    <asp:RadioButton ID="btnNo" OnCheckedChanged="CheckedChange" AutoPostBack="true" GroupName="liveOutside" runat="server" />No &nbsp;<asp:RadioButton ID="btnYes" AutoPostBack="true" OnCheckedChanged="CheckedChange" GroupName="liveOutside" runat="server" />Yes
    <br /><br />
    <asp:UpdatePanel ID="updLiveUsa" runat="server">
    <ContentTemplate>
    <div class="row row1">
        <asp:RadioButton ID="btnUSA" onclick="ChangeUSA(true);" Checked="true" GroupName="liveUSA" runat="server" />USA<br />
        <asp:RadioButton ID="btnNotUSA" onclick="ChangeUSA(false);" GroupName="liveUSA" runat="server" />Another country
    </div>
    <div class="row row1">
        <asp:Label ID="lblAddress" AssociatedControlID="txtAddress" Text="*Address: " runat="server" />
        <asp:TextBox ID="txtAddress" MaxLength="40" Columns="40" runat="server" />
        <asp:RequiredFieldValidator ID="reqAddress" Text="Required" Display="Dynamic" ControlToValidate="txtAddress" runat="server" />
        <user:LengthValidator ID="valAddress" Text="Too many characters" MaxLength="40" Display="Dynamic" ControlToValidate="txtAddress" runat="server" />
    </div>
    <div class="rowes row1">
        <asp:Label ID="lblDireccion" Text="Direcci&oacute;n" runat="server" />
    </div>
    <div class="row row1">
        <asp:Label ID="lblState" AssociatedControlID="ddlState" Text="*State: " runat="server" />
        <asp:DropDownList EnableViewState="false" ID="ddlState" OnDataBound="ddlDataBound" DataTextField="Name" DataValueField="ID" DataSourceID="odsStates" runat="server" />
        <asp:TextBox ID="txtState" style="display: none;" runat="server" />
        <asp:CustomValidator ID="reqState" ClientValidationFunction="ValidateState" OnServerValidate="ValidateState" ValidateEmptyText="true" ControlToValidate="txtState" Text="Required" runat="server" />
        <user:LengthValidator ID="lenState" Text="Too many characters" MaxLength="64" Display="Dynamic" ControlToValidate="txtState" runat="server" />
    </div>
    <div class="rowes row1">
        <asp:Label ID="lblEstado" Text="Estado" runat="server" />
    </div>
    <div class="row row1">
        <asp:Label ID="lblCity" AssociatedControlID="txtCity" Text="*City: " runat="server" />
        <asp:TextBox ID="txtCity" MaxLength="32" runat="server" />
        <asp:RequiredFieldValidator ID="reqCity" Text="Required" Display="Dynamic" ControlToValidate="txtCity" runat="server" />
        <user:LengthValidator ID="lenCity" Text="Too many characters" MaxLength="32" Display="Dynamic" ControlToValidate="txtCity" runat="server" />
    </div>
    <div class="rowes row1">
        <asp:Label ID="lblCiudad" Text="Ciudad" runat="server" />
    </div>
    <%-- WARNING: Hard coded values
        If the ID in the DB is changed, the parameter must be updated
     --%>
    <asp:ObjectDataSource ID="odsStates" TypeName="ME.Admon.Country" SelectMethod="GetStates" runat="server">
    <SelectParameters>
        <asp:Parameter Name="country" Type="Int32" DefaultValue="1" />
    </SelectParameters>
    </asp:ObjectDataSource>
   
    <div class="row row1">
        <asp:Label ID="lblZip" AssociatedControlID="txtZip" Text="*Zip Code: " runat="server" />
        <asp:TextBox ID="txtZip" MaxLength="25" runat="server" />
        <asp:RequiredFieldValidator ID="reqZip" Text="Required" Display="Dynamic" ControlToValidate="txtZip" runat="server" />
        <user:LengthValidator ID="valZip" Text="Too many characters" Display="Dynamic" MaxLength="25" ControlToValidate="txtZip" runat="server" />
        <asp:RegularExpressionValidator ID="regZip" Text="ZIP code is not valid" 
            Display="Dynamic" ValidationExpression="^[A-Za-z0-9]+([\s\w\d]*(\([\s\w\d]*\))*)*$" 
            ControlToValidate="txtZip" runat="server" />
    </div>
    <div class="rowes row1">
        <asp:Label ID="lblCP" Text="C&oacute;digo Postal" runat="server" />
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    </div>
</ContentTemplate>
</asp:UpdatePanel>

</td>
<td width="50%" align="center" valign="top">
<br /> <br /> <br />
<table>
    <tr>
        <td>Net Premium: </td>
        <td><asp:Label ID="lblNetPremium" runat="server" /></td>
    </tr>
    <tr>
        <td>Fee: </td>
        <td><asp:Label ID="lblFee" runat="server" /></td>
    </tr>
    <tr>
        <td>Tax: </td>
        <td><asp:Label ID="lblTax" runat="server" /></td>
    </tr>
    <tr>
        <td>Total: </td>
        <td><asp:Label ID="lblTotal" runat="server" /></td>
    </tr>
</table>

<asp:Button ID="btnGenerateCalculo" Text="Print Calculation" runat="server" /><br /><br />
<asp:Button ID="btnPrintQuote" Text="Print Quote" runat="server" /><br /><br />
<asp:Button ID="btnBuyQuote" Text="Buy Quote" OnClick="btnBuyQuote_Click" runat="server" />

</td>

</tr>
</table>