<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ApplicationStep2.ascx.cs" Inherits="UserControls_ApplicationStep2" %>

<script type="text/javascript">

function validateAlphanumeric(source, arguments)
{
    
    if (arguments.Value.length == 0 || arguments.Value.match(/^[a-zA-Z0-9]+$/))
        arguments.IsValid = true;
    else
        arguments.IsValid = false;
}

function bankInformation_Validate(source, args) {
    var txtBankName = $get('<%= txtBankName.ClientID %>');
    args.IsValid = true;
    if (txtBankName.value.trim().length > 0) {
        args.IsValid = args.Value.trim().length > 0;
    }
}

</script>

<div class="row row2">
    <asp:Label ID="lblStreet" AssociatedControlID="txtStreet" Text="*Street: " runat="server" />
    <asp:TextBox ID="txtStreet" MaxLength="50" Columns="30" runat="server" />
    <asp:RegularExpressionValidator ID="cmpStreet" Text="Invalid characters (only alphanumeric)." ControlToValidate="txtStreet" Display="Dynamic" ValidationExpression="^[a-zA-Z0-9\s\.]+$" runat="server" />
    <asp:RequiredFieldValidator ID="reqStreet" Display="Dynamic" ControlToValidate="txtStreet" Text="Required" runat="server" />
    <user:LengthValidator ID="valStreet" Display="Dynamic" Text="Too many characters" MaxLength="50" ControlToValidate="txtStreet" runat="server" /> 
</div>
<div class="rowes row2">
    <asp:Label ID="lblCalle" Text="Calle" runat="server" />
</div>
<div class="row row2">
    <asp:Label ID="lblExtNumber" AssociatedControlID="txtExtNumber" Text="*Ext. Number: " runat="server" />
    <asp:TextBox ID="txtExtNumber" MaxLength="9" runat="server" />
    <asp:RequiredFieldValidator ID="reqExtNumber" Display="Dynamic" ControlToValidate="txtExtNumber" Text="Required" runat="server" />
    <asp:RegularExpressionValidator ID="cmpExtNumber" Text="Invalid characters (only alphanumeric)." ControlToValidate="txtExtNumber" Display="Dynamic" ValidationExpression="^[a-zA-Z0-9\s\.]+$" runat="server" />
    <user:LengthValidator ID="valExtNumber" Display="Dynamic" Text="Too many characters" MaxLength="9" ControlToValidate="txtExtNumber" runat="server" /> 
</div>
<div class="rowes row2">
    <asp:Label ID="lblNumeroExt" Text="N&uacute;mero exterior" runat="server" />
</div>
<div class="row row2">
    <asp:Label ID="lblIntNumber" AssociatedControlID="txtIntNumber" Text="Int. Number: " runat="server" />
    <asp:TextBox ID="txtIntNumber" MaxLength="10" runat="server" />
    <asp:RegularExpressionValidator ID="cmpIntNumber" Text="Invalid characters (only alphanumeric)." ControlToValidate="txtStreet" Display="Dynamic" ValidationExpression="^[a-zA-Z0-9\s\.]*$" runat="server" />
    <user:LengthValidator ID="valIntNumber" Display="Dynamic" Text="Too many characters" MaxLength="10" ControlToValidate="txtIntNumber" runat="server" /> 
</div>
<div class="rowes row2">
    <asp:Label ID="lblNumeroInt" Text="N&uacute;mero interior" runat="server" />
</div>

<asp:UpdatePanel ID="updAddress" runat="server">
<ContentTemplate>
    <div class="row row2">
        <asp:Label ID="lblState" AssociatedControlID="ddlState" Text="*State: " runat="server" />
        <asp:DropDownList EnableViewState="false" ID="ddlState" OnDataBound="ddlDataBound" AutoPostBack="true" DataTextField="Name" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" DataValueField="ID" DataSourceID="odsStates" runat="server" />
        <asp:RequiredFieldValidator ID="reqddlState" InitialValue="0" Display="Dynamic" Text="Required" ControlToValidate="ddlState" runat="server" />

    </div>
    <div class="rowes row2">
        <asp:Label ID="lblEstado" Text="Estado" runat="server" />
    </div>
    <div class="row row2">
        <asp:Label ID="lblCity" AssociatedControlID="ddlCity" Text="*City:" runat="server" />
        <asp:DropDownList EnableViewState="false" ID="ddlCity" OnDataBound="ddlDataBound" AutoPostBack="true" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged" DataTextField="Name" DataValueField="ID" DataSourceID="odsCities" runat="server" />
        <asp:RequiredFieldValidator ID="reqddlCity" InitialValue="0" Display="Dynamic" Text="Required" ControlToValidate="ddlCity" runat="server" />
    </div>
    <div class="rowes row2">
        <asp:Label ID="lblCiudad" Text="Ciudad" runat="server" />
    </div>
    
    <div class="row row2">
        <asp:Label ID="lblColony" AssociatedControlID="ddlColony" Text="*Colony: " runat="server" />
        <asp:DropDownList EnableViewState="false" ID="ddlColony" OnDataBound="ddlDataBound" AutoPostBack="true" OnSelectedIndexChanged="ddlColony_SelectedIndexChanged" DataTextField="Name" DataValueField="ID" DataSourceID="odsColonies" runat="server" />
        <asp:RequiredFieldValidator ID="reqddlColony" Display="Dynamic" ControlToValidate="ddlColony" Text="Required" InitialValue="0" runat="server" />
    </div>
    <div class="rowes row2">
        <asp:Label ID="lblColonia" Text="Colonia" runat="server" />
    </div>
    <div class="row row2">    
        <asp:Label ID="lblZip" AssociatedControlID="txtZip" Text="Zip: " runat="server" />
        <asp:TextBox ID="txtZip" ReadOnly="true" runat="server" />
        
    </div>
    <div class="rowes row2">
        <asp:Label ID="lblCP" Text="C&oacute;digo Postal" runat="server" />
    </div>
    
     <%-- WARNING: Hard coded values
            If the ID in the DB is changed, the parameter must be updated
     --%>
    <asp:ObjectDataSource ID="odsStates" TypeName="ME.Admon.Country" SelectMethod="GetStates" runat="server">
    <SelectParameters>
        <asp:Parameter Name="country" Type="Int32" DefaultValue="2" />
    </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsCities" TypeName="ME.Admon.State" SelectMethod="GetCities" runat="server">
    <SelectParameters>
        <asp:ControlParameter ControlID="ddlState" PropertyName="SelectedValue" Name="state" Type="Int32" />
    </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsColonies" TypeName="ME.Admon.City" SelectMethod="GetHouseEstates" runat="server">
    <SelectParameters>
        <asp:ControlParameter ControlID="ddlCity" PropertyName="SelectedValue" Name="city" Type="Int32" />
    </SelectParameters>
    </asp:ObjectDataSource>
</ContentTemplate>
</asp:UpdatePanel>

<hr />
<h3>Bank Information<br /><span style="font-size: small;">If loan has been secured</span></h3>
<div class="row row2">
    <asp:Label ID="lblBankName" AssociatedControlID="txtBankName" Text="Bank Name: " runat="server" />
    <asp:TextBox ID="txtBankName" Columns="32" MaxLength="128" runat="server" />
    <user:LengthValidator ID="valBankName" Display="Dynamic" Text="Too many characters" MaxLength="128" ControlToValidate="txtBankName" runat="server" /> 
</div>
<div class="rowes row2">
    <asp:Label ID="lblBanco" Text="Nombre del Banco" runat="server" />
</div>
<div class="row row2">
    <asp:Label ID="lblLoanNumber" AssociatedControlID="txtLoanNumber" Text="Loan Number: " runat="server" />
    <asp:TextBox ID="txtLoanNumber" Columns="32" MaxLength="32" runat="server" />
    <asp:CustomValidator ID="valLoanNumber" ClientValidationFunction="bankInformation_Validate" ControlToValidate="txtLoanNumber" Text="Required" Display="Dynamic" ValidateEmptyText="true" OnServerValidate="valBankInformation_Validate" runat="server" />
    <asp:CustomValidator ClientValidationFunction="validateAlphanumeric" OnServerValidate="txtLoanNumber_Validate" Text="Format is not valid." Display="Dynamic" ControlToValidate="txtLoanNumber" runat="server" />
</div>
<div class="rowes row2">
    <asp:Label ID="lblPrestamo" Text="N&uacute;mero de Pr&eacute;stamo" runat="server" />
</div>
<div class="row row2">
    <asp:Label ID="lblBankAddress" AssociatedControlID="txtBankAddress" Text="Bank Address: " runat="server" />
    <asp:TextBox ID="txtBankAddress" Columns="32" MaxLength="254" runat="server" />
    <asp:CustomValidator ID="valBankAddress" ClientValidationFunction="bankInformation_Validate" ControlToValidate="txtBankAddress" Text="Required" Display="Dynamic" ValidateEmptyText="true" OnServerValidate="valBankInformation_Validate" runat="server" />
    <user:LengthValidator ID="lenBankAddress" Text="Input too long" MaxLength="254" ControlToValidate="txtBankAddress" Display="Dynamic" runat="server" />    
</div>
<div class="rowes row2">
    <asp:Label ID="lblDireccionBanco" Text="Direcci&oacute;n del Banco" runat="server" />
</div>
