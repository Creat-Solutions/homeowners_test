<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PolicyPDF.ascx.cs" Inherits="UserControls_PolicyPDF" %>
<script type="text/javascript">
    function GetPDF() {
        window.location.href = '/GetPoliza.ashx?poliza=<%= (int)Session["policyid"] %>';
        return false;
    }

    function GetEInvoice() {
        window.location.href = '/GetInvoice.ashx?id=<%= (int)Session["policyid"] %>';
        return false;
    }

    function GetEPInvoice() {
        window.location.href = '<%= VirtualPathUtility.ToAbsolute("~/GetInvoice.ashx") %>?id=<%= (int)Session["policyid"] %>&fepadre=1';
        return false;
    }
</script>
<div align="center">
    Policy number <asp:Literal ID="lblPolicyNumber" runat="server" /> has been issued. To print your policy press the 'Print Policy' button.<br />
     <input type="button" onclick="return GetPDF();" value="Print Policy" /><br />
     <input type="button" onclick="return GetEInvoice();" value="Print Invoice" />
     <% if (Session["fepadre"] != null && (bool)Session["fepadre"]) { %>
        <input type="button" onclick="return GetEPInvoice();" value="Print General Invoice" />
     <% } %>
     <br />
     <%--<input type="button" onclick="return GetEnglishPDF();" value="Print Policy (English)" />--%>
</div>
<p>
We have issued your policy according to the information that you filled out on our online application. Please review the terms
and conditions of the policy and let us know if you have any doubts. Under Mexican Insurance Law, you have 30 days to make any
changes or modifications you dont agree with.
</p>
<asp:Literal ID="lblWarningSkip" runat="server">
<p>
We would appreciate if you cand send us a check for the above indicated amount payable to Macafee &amp; Edwards, Inc., no later
than 30 days from the inception date in order to avoid automatic cancellation (without any notice) according to Mexican
Insurance Law.
</p>
</asp:Literal>

