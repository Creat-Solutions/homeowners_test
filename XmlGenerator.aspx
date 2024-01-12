<%@ Page Title="" Language="C#" MasterPageFile="~/HouseMaster.master" AutoEventWireup="true" CodeFile="XmlGenerator.aspx.cs" Inherits="XmlGeneratorForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="workloadContent" Runat="Server">

<script type="text/javascript">

    function validateRange(obj, args) {
        var regEx = /(\d|\d-\d)(,(\d|\d-\d))*/;
        var txtRango = args.Value;
        args.IsValid = regEx.test(txtRango);
    }

</script>

<asp:Label ID="lblRango" Text="Rango de cotizaciones (1-3 o 1 o 1,2,3): " AssociatedControlID="txtRango" runat="server" />
<asp:TextBox ID="txtRango" runat="server" /><br />
<asp:CheckBox ID="chkTest" Text="Generar xml para servidor de prueba" runat="server" />
<asp:RequiredFieldValidator ID="reqRango" Display="Dynamic" Text="Required" ControlToValidate="txtRango" runat="server" />
<asp:CustomValidator ID="valRango" Display="Dynamic" Text="Invalid" ClientValidationFunction="validateRange" OnServerValidate="valRango_OnServerValidate" ControlToValidate="txtRango" runat="server" />
<br />
<asp:Button ID="btnGet" OnClick="btnGet_OnClick" Text="Obtener XMLs" runat="server" />

</asp:Content>

