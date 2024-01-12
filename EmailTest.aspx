<%@ Page Title="Email Test" Language="C#" AutoEventWireup="true" CodeFile="EmailTest.aspx.cs" Inherits="EmailTest" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>House Insurance Application</title>
    <link href="/greybox/gb_styles.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var GB_ROOT_DIR = "/greybox/";
    </script>
    <script type="text/javascript" src="/greybox/AJS.js"></script>
    <script type="text/javascript" src="/greybox/AJS_fx.js"></script>
    <script type="text/javascript" src="/greybox/gb_scripts.js"></script>
    <!--[if gte IE 5.5]>
    <![if lt IE 7]>
    <style type="text/css">
    div#loadBar {
      position: absolute;
      left: expression( ( ignoreMe2 = document.documentElement.scrollLeft ? document.documentElement.scrollLeft : document.body.scrollLeft ) + 'px' );
      top: expression( ( -( ignoreMe = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop ) ) + 'px' );
    }
    </style>
    <![endif]>
    <![endif]-->
</head>
<body>

    <form id="form1" runat="server">
        To: <asp:TextBox ID="txtTo" runat="server" /><asp:RequiredFieldValidator ID="reqTo" Text="Required" ControlToValidate="txtTo" Display="Dynamic" runat="server" /><br />
        Subject: <asp:TextBox ID="txtSubject" runat="server" /><asp:RequiredFieldValidator ID="reqSubject" Text="Required" ControlToValidate="txtSubject" Display="Dynamic" runat="server" /><br />
        Message: <br />
        <asp:TextBox ID="txtMessage" TextMode="MultiLine" Columns="40" Rows="5" runat="server" /><br />
        <asp:Button ID="btnSend" Text="Send" OnClick="btnSend_Click" runat="server" />
        <asp:Label ID="lblResults" runat="server" />
    </form>
</body>
</html>


