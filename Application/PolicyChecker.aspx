<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PolicyChecker.aspx.cs" Inherits="Application_PolicyChecker" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>House Insurance: Policy Check</title>
    <style type="text/css">
    body {
        font-family: Trebuchet MS,Helvetica;
        font-size: 10pt;
	    text-align: center;
	}
	div#waitingDiv {
	    margin: 0 auto;
	    text-align: center;
	}
    </style>
    
    <script type="text/javascript">
        var timeout = null;
        var timeoutLimit = 60;
        function CheckSuccess(result) {
            if (result) {
                //change page
                top.location.href = '/Application/BuyPolicy.aspx';
            } else if (timeoutLimit > 0) {
                timeoutLimit--;
                timeout = setTimeout(CheckStatus, 4000);
            } else {
                alert("Timeout limit");
                CheckFailure(null);
            }
        }

        function CheckFailure(error) {
            PageMethods.CheckPolicyErrorSubmit(function(status) { }, function(status) { });
            top.location.href = '/Default.aspx?error=general';
        }
    
        function CheckStatus() {
            timeout = null;
            PageMethods.CheckPolicyStatus(CheckSuccess, CheckFailure);
        }
    
    </script>
    
</head>
<body onload="setTimeout(CheckStatus, 100);">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptManager" EnablePageMethods="true" runat="server" />
    <div id="waitingDiv">
        Please wait while we process your request.
    </div>
    </form>
</body>
</html>