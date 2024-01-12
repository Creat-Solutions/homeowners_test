<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ApplicationStep3.ascx.cs" Inherits="UserControls_ApplicationStep3" %>

<script type="text/javascript">
    function ddlWall_Validate(obj, args) {
        var ddlRoofType = $get('<%= ddlRoof.ClientID %>');
        var roofType = parseInt(ddlRoofType.options[ddlRoofType.selectedIndex].value);
        if (roofType == <%= ApplicationConstants.CONCRETE_ROOFTYPE %>) {
            args.IsValid = parseInt(args.Value) == <%= ApplicationConstants.CONCRETE_WALLTYPE %>;
        } else {
            args.IsValid = true;
        }
    }
    
    function txtStories_Validate(obj, args) {
        var ddlType = $get('<%= ddlType.ClientID %>');
        var cmpStoriesText = $get('<%= cmpStoriesMore.ClientID %>');
        var numberStories = parseInt(args.Value);
        var typeOfProperty = parseInt(ddlType.options[ddlType.selectedIndex].value);
        if (typeOfProperty == <%= ApplicationConstants.CONDO_TYPE %>) {
            args.IsValid = numberStories >= 2;
            cmpStoriesText.innerHTML = 'Invalid data. Must be equal or greater than 2.';
        } else {
            args.IsValid = numberStories >= 1;
            cmpStoriesText.innerHTML = 'Invalid data. Must be equal or greater than 1.';
        }
    }
    
    function ddlRoof_Change() {
        var valWallType = $get('<%= valWall.ClientID %>');
        ValidatorValidate(valWallType);
    }
    
</script>

<div class="row3 row">
    <asp:Label ID="lblClient" AssociatedControlID="ddlClient" Text="*Client Type: " runat="server" />
    <asp:DropDownList ID="ddlClient" OnDataBound="ddlDataBound" DataSourceID="odsCont" DataTextField="description" DataValueField="type" runat="server" />
    <asp:RequiredFieldValidator ID="reqRisk" ControlToValidate="ddlClient" Text="Required" Display="Dynamic" InitialValue="0" runat="server" />
</div>
<div class="rowes row3">
    <asp:Label ID="lblCliente" Text="Tipo de contratante" runat="server" />
</div>

<div class="row3 row">
    <asp:Label ID="lblUse" AssociatedControlID="ddlUse" Text="*Use of Property: " runat="server" />
    <asp:DropDownList ID="ddlUse" OnDataBound="ddlDataBound" DataSourceID="odsUse" DataTextField="description" DataValueField="use" runat="server" />
    <asp:RequiredFieldValidator ID="reqUse" ControlToValidate="ddlUse" Text="Required" Display="Dynamic" InitialValue="0" runat="server" />
</div>
<div class="rowes row3">
    <asp:Label ID="lblUso" Text="Uso de la propiedad" runat="server" />
</div>

<div class="row3 row">
    <asp:Label ID="lblType" AssociatedControlID="ddlType" Text="*Type of Property: " runat="server" />
    <asp:DropDownList ID="ddlType" OnDataBound="ddlDataBound" DataSourceID="odsType" DataTextField="description" DataValueField="type" runat="server" />
    <asp:RequiredFieldValidator ID="reqType" ControlToValidate="ddlType" Text="Required" Display="Dynamic" InitialValue="0" runat="server" />
</div>
<div class="rowes row3">
    <asp:Label ID="lblTipo" Text="Tipo de vivienda" runat="server" />
</div>
<div class="row3 row">
    <asp:Label ID="lblAdjoining" AssociatedControlID="rbnAdjYes" Text="Surrounded by Adjecent Structures? " runat="server" />
    
    <asp:RadioButton ID="rbnAdjYes" GroupName="grpAdjoining" runat="server" />Yes / S&iacute; <asp:RadioButton ID="rbnAdjNo" GroupName="grpAdjoining" runat="server" />No
</div>
<div class="rowes row3">
    <asp:Label ID="lblColindantes" Text="&iquest;Colindantes fincados?" runat="server" />
</div>

<div class="row3 row">
    <asp:Label ID="lblRoof" AssociatedControlID="ddlRoof" Text="*Roof Type: " runat="server" />
    <asp:DropDownList ID="ddlRoof" OnDataBound="ddlDataBound" DataSourceID="odsRoof" onchange="ddlRoof_Change();" DataTextField="description" DataValueField="type" runat="server" />
    <asp:RequiredFieldValidator ID="reqRoof" ControlToValidate="ddlRoof" Text="Required" Display="Dynamic" InitialValue="0" runat="server" />
</div>
<div class="rowes row3">
    <asp:Label ID="lblTecho" Text="Tipo de techos" runat="server" />
</div>

<div class="row3 row">
    <asp:Label ID="lblWall" AssociatedControlID="ddlWall" Text="*Wall Type: " runat="server" />
    <asp:DropDownList ID="ddlWall" OnDataBound="ddlDataBound" DataSourceID="odsWall" DataTextField="description" DataValueField="type" runat="server" />
    <asp:RequiredFieldValidator ID="reqWall" ControlToValidate="ddlWall" Text="Required" Display="Dynamic" InitialValue="0" runat="server" />
    <asp:CustomValidator ID="valWall" ControlToValidate="ddlWall" Text="Invalid wall type" Display="Dynamic" ClientValidationFunction="ddlWall_Validate" OnServerValidate="ddlWall_Validate" runat="server" />
</div>
<div class="rowes row3">
    <asp:Label ID="lblParedes" Text="Tipo de muros" runat="server" />
</div>

<div class="row3 row">
    <asp:Label ID="lblStories" AssociatedControlID="txtStories" Text="*Stories: " runat="server" />
    <asp:TextBox ID="txtStories" runat="server" />
    <asp:RequiredFieldValidator ID="reqStories" ControlToValidate="txtStories" Text="Required" Display="Dynamic" runat="server" />
    <asp:CompareValidator ID="cmpStories" ControlToValidate="txtStories" Text="Invalid data" Display="Dynamic" Operator="DataTypeCheck" Type="Integer" runat="server" /> 
    <asp:CustomValidator ID="cmpStoriesMore" ControlToValidate="txtStories" Text="Invalid data. Compare with type of property" Display="Dynamic" ClientValidationFunction="txtStories_Validate" OnServerValidate="txtStories_Validate" runat="server" />
</div>
<div class="rowes row3">
    <asp:Label ID="lblStoriesEs" Text="Pisos" runat="server" />
</div>

<div class="row3 row">
    <asp:Label ID="lblUnderground" AssociatedControlID="txtUnderground" Text="*Underground Floors: " runat="server" />
    <asp:TextBox ID="txtUnderground" runat="server" />
    <asp:RequiredFieldValidator ID="reqUnderground" ControlToValidate="txtUnderground" Text="Required" Display="Dynamic" runat="server" />
    <asp:CompareValidator ID="cmpUnderground" ControlToValidate="txtUnderground" Text="Invalid data" Display="Dynamic" Operator="DataTypeCheck" Type="Integer" runat="server" /> 
    <asp:CompareValidator ID="cmpUndergroundMore" ControlToValidate="txtUnderground" Text="Invalid data. Must be equal or greater than 0." Display="Dynamic" Operator="GreaterThanEqual" ValueToCompare="0" runat="server" />
</div>
<div class="rowes row3">
    <asp:Label ID="lblSotanos" Text="S&oacute;tanos" runat="server" />
</div>


<asp:ObjectDataSource ID="odsCont" TypeName="clientType" SelectMethod="SelectCached" runat="server" />
<asp:ObjectDataSource ID="odsUse" TypeName="useOfProperty" SelectMethod="SelectCached" runat="server" />
<asp:ObjectDataSource ID="odsType" TypeName="typeOfProperty" SelectMethod="SelectCached" runat="server" />
<asp:ObjectDataSource ID="odsRoof" TypeName="roofType" SelectMethod="SelectCached" runat="server" />
<asp:ObjectDataSource ID="odsWall" TypeName="wallType" SelectMethod="SelectCached" runat="server" />