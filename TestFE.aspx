<%@ Page Title="" Language="C#" MasterPageFile="~/HouseMaster.master" AutoEventWireup="true" CodeFile="TestFE.aspx.cs" Inherits="TestFE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="workloadContent" Runat="Server">

<asp:Label ID="lblText" runat="server" Text="Texto (en base64)" runat="server" /><br />
<asp:TextBox ID="txtXML" TextMode="MultiLine" Rows="50" Columns="80" runat="server" /><br />
<asp:Button ID="btnTest" OnClick="btnTest_Click" Text="Test!" runat="server" /><br />
<asp:Label ID="lblResults" Visible="false" Text="Failure" runat="server" />


</asp:Content>

