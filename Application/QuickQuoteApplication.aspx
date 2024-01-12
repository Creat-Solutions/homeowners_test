<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QuickQuoteApplication.aspx.cs" Inherits="QuickQuoteApplication" MasterPageFile="~/HouseMaster.master" %>
<asp:Content ContentPlaceHolderID="workloadContent" ID="contentForm" runat="server">
<script type="text/javascript">
    var fadeTimeout = 100.0;
    var theTimeout = null;
    var cancelTimeout = false;
    function ClearFade(literal) {
        cancelTimeout = true;
        if (!literal) {
            cancelTimeout = false;
            return;
        }
        literal.FadeDirection = -1;
        literal.FadeTimeout = fadeTimeout;
        setOpacity(literal, 1);
        clearTimeout(theTimeout);
        theTimeout = null;
        cancelTimeout = false;
    }

    function setOpacity(literal, value) {
        if (!literal)
            return;
        literal.style.opacity = value;
        literal.style.filter = 'alpha(opacity=' + (value * 100) +')';
    }
    
    function animateFade(lastTick, ntitle) {
        var ticks = new Date().getTime();
        var elapsed = ticks - lastTick;
        var literal = $get('lblFormTitle');
        if (literal.FadeTimeout <= elapsed) {
            if (literal.FadeDirection == -1) {
                setOpacity(literal, 0);
                literal.FadeDirection = 1;
                literal.FadeTimeout = fadeTimeout;
                literal.innerHTML = ntitle;
                if (!cancelTimeout)
                    theTimeout = setTimeout("animateFade(" + new Date().getTime() + ", '')", 33);
            } else {
                setOpacity(literal, 1);
                theTimeout = null;
            }
            return;
        }
        literal.FadeTimeout -= elapsed;
        var nOpacity = literal.FadeTimeout / fadeTimeout;
        if (literal.FadeDirection == 1)
            nOpacity = 1 - nOpacity;
        setOpacity(literal, nOpacity);
        if (!cancelTimeout)
            theTimeout = setTimeout("animateFade(" + ticks + ", '" + ntitle + "')", 33);
    }

    function changeTitle(ntitle) {
        var literal = $get('lblFormTitle');
        literal.FadeTimeout = fadeTimeout;
        literal.FadeDirection = -1;
        if (theTimeout != null)
            ClearFade(literal);
        theTimeout = setTimeout("animateFade(" + new Date().getTime() + ", '" + ntitle + "')", 33);
    }

    function changeToOriginal() {
        changeTitle('<%= formTitle.Text %>');
    }

    function removeMouseOut(obj) {
        if (obj) {
            obj.onmouseout = null;
        }
        return true;
    }
    
</script>

<div id="insuranceApp">
    <div id="headerApp"><span id="lblFormTitle" style="filter: alpha(opacity=100); display: inline-block;"><asp:Literal ID="formTitle" runat="server" /></span></div>
    <div id="barSteps">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:ImageButton ID="imgStep1" OnClientClick="return removeMouseOut(this);" OnClick="goToStep" CausesValidation="false" EnableViewState="false" ImageUrl="~/images/step1.jpg" runat="server" />
                </td>
                <td>
                    <asp:ImageButton ID="imgStep2" OnClientClick="return removeMouseOut(this);" OnClick="goToStep"  CausesValidation="false" EnableViewState="false" ImageUrl="~/images/step2.jpg" runat="server" />
                </td>
                <td>
                    <asp:ImageButton ID="imgStep3" OnClientClick="return removeMouseOut(this);" OnClick="goToStep" CausesValidation="false" EnableViewState="false" ImageUrl="~/images/step3.jpg" runat="server" />
                </td>
                <td>
                    <asp:ImageButton ID="imgStep4" OnClientClick="return removeMouseOut(this);" OnClick="goToStep" CausesValidation="false" EnableViewState="false" ImageUrl="~/images/step4.jpg" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <asp:Label ID="lblError" runat="server" /><br />
    <div id="quoteId" style="position: relative; width: 150px;">
        <asp:Label ID="lblQuote" style="position: absolute; right: 10px;" runat="server" />
        
    </div>
    <asp:PlaceHolder ID="placeHolderStep" EnableViewState="false" runat="server" />
</div>
<br />
<div style="position: relative;">
<asp:Button ID="btnSearchExpiring" CausesValidation="false" OnClick="btnSearchExpiring_Click" Text="Return to Expiring Policies Report" style="position: absolute; right: 60px;" runat="server" />
<asp:Button ID="btnPrevious" CausesValidation="false" OnClick="btnPrevious_click" Text="Previous" runat="server" />
<asp:Button ID="btnNext" OnClick="btnNext_click" Text="Next" runat="server" />
<asp:Button ID="btnCancel" OnClick="btnCancel_click" style="position: absolute; right: 0;" CausesValidation="false" Text="Cancel" runat="server" />
</div>
<br />
<br />

</asp:Content>
