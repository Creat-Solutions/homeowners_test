<%@ Control Language="C#" AutoEventWireup="true" CodeFile="QuickQuoteStep2.ascx.cs" Inherits="UserControls_QuickQuoteStep2" %>

<script type="text/javascript">

function validateAlphanumeric(source, arguments)
{
    
    if (arguments.Value.length == 0 || arguments.Value.match(/^[a-zA-Z0-9]+$/))
        arguments.IsValid = true;
    else
        arguments.IsValid = false;
}

$(function() {

    $('#<%= txtZipCode.ClientID %>').blur(function() {
        var zipCode = $(this).val();
        if (zipCode.length < 5) {
            var necessaryZeros = 5 - zipCode.length;
            for (i = 0; i < necessaryZeros; i++) {
                zipCode = '0' + zipCode;
            }
            
            $('#<%= txtZipCode.ClientID %>').val(zipCode);
        }
    });
});


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
        <asp:Label ID="lblZip" AssociatedControlID="txtZipCode" Text="Zip Code: " runat="server" />
        <asp:TextBox ID="txtZipCode" AutoPostBack="true" OnTextChanged="txtZipCode_TextChanged" MaxLength="10" runat="server" />
        <asp:RequiredFieldValidator ID="reqZipCode" Display="Dynamic" ControlToValidate="txtZipCode" Text="Required" runat="server" />
        <user:LengthValidator ID="lenZipCode" Text="Maximum length for zip code" Display="Dynamic" MaxLength="10" ControlToValidate="txtZipCode" runat="server" />
        <asp:Label ID="lblHouseEstateNotFound" Visible="false" CssClass="failureText" Text="El código postal es inexistente." runat="server" />
        <asp:HyperLink NavigateUrl="https://www.correosdemexico.gob.mx/SSLServicios/ConsultaCP/Descarga.aspx" Target="_blank" Text="Don’t know the zip code click here/Si no sabe el código haga click aquí" runat="server" />
    </div>
    <div class="rowes row2">
        <asp:Label ID="lblCP" Text="C&oacute;digo Postal" runat="server" />
    </div>

    <div class="row row2">
        <asp:Label ID="lblState" AssociatedControlID="txtState" Text="State: " runat="server" />
        <asp:TextBox ID="txtState" style="width: 175px; background-color: #DDD;" ReadOnly="true" runat="server" />
        <asp:DropDownList ID="ddlState" Visible="false" runat="server" DataValueField="ID" DataTextField="Name" />

    </div>
    <div class="rowes row2">
        <asp:Label ID="lblEstado" Text="Estado" runat="server" />
    </div>
    <div class="row row2">
        <asp:Label ID="lblCity" AssociatedControlID="ddlCity" Text="City:" runat="server" />
        <asp:TextBox ID="txtCity" style="width: 175px; background-color: #DDD;" ReadOnly="true" runat="server" />
        <asp:DropDownList ID="ddlCity" Visible="false" runat="server" DataValueField="ID" DataTextField="Name" />
    </div>
    <div class="rowes row2">
        <asp:Label ID="lblCiudad" Text="Ciudad" runat="server" />
    </div>
    
    <div class="row row2">
        <asp:Label ID="lblColony" AssociatedControlID="txtHouseEstate" Text="House Estate: " runat="server" />
        <asp:TextBox ID="txtHouseEstate" style="width: 220px; background-color: #DDD;" ReadOnly="true" runat="server" />
        <asp:DropDownList ID="ddlHouseEstate" style="width: auto;" Visible="false" runat="server" DataValueField="ID" DataTextField="Name" />
    </div>
    <div class="rowes row2">
        <asp:Label ID="lblColonia" Text="Colonia" runat="server" />
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
