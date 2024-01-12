<%@ Page Language="C#" MasterPageFile="~/HouseMaster.master" AutoEventWireup="true" CodeFile="Limitless.aspx.cs" Inherits="Limitless" Title="Untitled Page" %>
<asp:Content ID="cntContent" ContentPlaceHolderID="workloadContent" Runat="Server">
<p>Please review the following information.<br />
Favor de revisar la siguiente informaci&oacute;n.</p>
<asp:HyperLink ID="lnkApp" Text="Go back to change the information" NavigateUrl="~/InsuranceApplication.aspx" runat="server" />

<div class="row row1">
    <asp:Label ID="lblName" Text="*Name: " runat="server" />
    <asp:TextBox ID="txtName" Enabled="false" MaxLength="64" runat="server" />
</div>
<div class="rowes row1">
    <asp:Label ID="lblNombre" Text="Nombre" runat="server" />
</div>
<div class="row row1">
    <asp:Label ID="lblLastName" AssociatedControlID="txtLastName" Text="*Last Name: " runat="server" />
    <asp:TextBox ID="txtLastName" Enabled="false" MaxLength="64" runat="server" />
 </div>
<div class="rowes row1">
    <asp:Label ID="lblApellidos" Text="Apellido" runat="server" />
</div>
<div class="row row1">
    <asp:Label ID="lblEmail" AssociatedControlID="txtEmail" Text="Email: " runat="server" />
    <asp:TextBox ID="txtEmail" Enabled="false" Columns="32" MaxLength="128" runat="server" />
</div>
<div class="rowes row1">
    <asp:Label ID="lblCorreoE" Text="Correo electr&oacute;nico" runat="server" />
</div>
<div class="row row1">
    <asp:Label ID="lblPhone" AssociatedControlID="txtAreaPhone" Text="Phone No.: " runat="server" />
    <asp:TextBox ID="txtPhone"  Enabled="false" runat="server" />
</div>
<div class="rowes row1">
    <asp:Label ID="lblTelefono" Text="N&uacute;mero de Tel&eacute;fono" runat="server" />
</div>
<div class="row row1">
    <asp:Label ID="lblFax" AssociatedControlID="txtAreaFax" Text="Fax No.: " runat="server" />
    <asp:TextBox ID="txtFax" Enabled="false" runat="server" />
</div>
<div class="rowes row1">
    <asp:Label ID="lblFaxEs" Text="N&uacute;mero de Fax" runat="server" />
</div>
<asp:Button ID="btnConfirm" Text="Confirm Information" OnClientClick="" OnClick="btnConfirm_Click" runat="server" />

</asp:Content>

