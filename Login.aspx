<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" EnableTheming="false" StylesheetTheme="" Inherits="Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Login</title>
    <link href="/greybox/styles.css" rel="stylesheet" />
    <link href="/greybox/styleAir.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="divLogin">
            <h1>Homeowner Quote Application</h1>
            <div id="divContentPane">
                <div id="divLeftPane">
                    <p>
                        <label id="lblUsername">Username:</label>
                    </p>
                    <p>
                        <asp:TextBox ID="txtUsername" runat="server" Width="119px" Height="23px"></asp:TextBox>
                    </p>
                    <p>
                        <label id="lblPassword">Password:</label>
                    </p>
                    <p>
                        <asp:TextBox ID="txtPassword" runat="server" Height="23px" Width="119px" TextMode="Password"></asp:TextBox>
                    </p>
                    <p>
                        <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="button_thin" OnClick="btnLogin_Click"></asp:Button>
                    </p>
                </div>
                <div id="divRightPane">
                    <img src="/images/logo.png" alt="Homeowners Quoter" border="0" />
                </div>
            </div>
            <div id="divLeftPane">
                <asp:Label ID="lblServerDown" CssClass="errorText" Visible="False" runat="server">
                The server is temporarily down, please try again in a few minutes</asp:Label>
                <br />
                <asp:Label ID="lblInvalid" CssClass="errorText" Visible="False" runat="server">
                There is a problem with you username / password.</asp:Label>
                <br />
                <%--<a href="/Page/ForgotPassword.aspx">Forgot password?</a>--%>
            </div>
        </div>
    </form>
</body>
</html>
