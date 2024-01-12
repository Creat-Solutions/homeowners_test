<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Cotiza.aspx.cs" Inherits="Cotiza" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cotiza</title>
    <script type="text/javascript">
    function printMe() {
        window.print();
    }
    </script>
</head>
<body onload="printMe()">
    <form id="form1" runat="server">
    <div>
        <asp:PlaceHolder ID="plhCotiza" runat="server" />
    </div>
    </form>
</body>
</html>
