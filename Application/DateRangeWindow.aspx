<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DateRangeWindow.aspx.cs" Inherits="Application_DateRangeWindow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>House Insurance: Date Range</title>
    <link href="/greybox/gb_styles.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var GB_ROOT_DIR = "/greybox/";
    </script>
    <script type="text/javascript" src="/greybox/AJS.js"></script>
    <script type="text/javascript" src="/greybox/AJS_fx.js"></script>
    <script type="text/javascript" src="/greybox/gb_scripts.js"></script>
    <script type="text/javascript">
        function OnCloseBox() {
            location.href = '/Application/Load.aspx?option=clear';
        }
    
        function showDateRange() {
            return GB_showCenter('Choose Policy Range', '/Application/DateRange.aspx', 300, 500, OnCloseBox);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptManager" runat="server" />
    <div>
    
    </div>
    </form>
</body>
</html>
