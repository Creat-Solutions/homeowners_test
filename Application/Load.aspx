<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Load.aspx.cs" Inherits="LoadPage" MasterPageFile="~/HouseMaster.master" %>

<asp:Content ContentPlaceHolderID="workloadContent" ID="contentForm" runat="server">
<div class="error">
    <asp:Label ID="lblError" runat="server" />
</div>
<asp:Label ID="lblLoad" Text="Load quote: " EnableViewState="false" AssociatedControlID="txtLoad" runat="server" />
<asp:TextBox ID="txtLoad" runat="server" />
<asp:Button ID="btnLoad" Text="Load" EnableViewState="false" OnClick="btnLoad_click" runat="server" />

</asp:Content>
