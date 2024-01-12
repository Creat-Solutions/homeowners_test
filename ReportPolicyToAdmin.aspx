<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportPolicyToAdmin.aspx.cs" Inherits="ReportPolicyToAdmin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblResults" runat="server" />
        Policy number: <asp:TextBox ID="txtPolicyNo" runat="server" />
        <asp:RequiredFieldValidator ID="reqP" ControlToValidate="txtPolicyNo" Display="Dynamic" Text="Required" runat="server" />
        <asp:Button ID="btnReport" Text="Report" OnClick="btnReport_Click" runat="server" />
    </div>
    </form>
</body>
</html>
