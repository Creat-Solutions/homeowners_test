<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CotizaStep.ascx.cs" Inherits="UserControls_CotizaStep" %>

<script type="text/javascript">

    function PrintDoc() {
        window.location.href = '/CotizaPDF.ashx?quote=<%= (int)Session["quoteid"] %>';
        return false;
    }

    function ViewPDF() {
        window.location.href = '/GetCotiza.ashx?quote=<%= (int)Session["quoteid"] %>';
        return false;
    }

    function ViewPDFEnglish() {
        window.location.href = '/GetCotiza.ashx?quote=<%= (int)Session["quoteid"] %>&eng=1';
        return false;
    }
</script>
<div id="print">
    <asp:Button ID="btnPrintDoc" OnClientClick="return PrintDoc();" Text="Quote in PDF" runat="server" />
    <asp:Button ID="btnViewPDF" OnClientClick="return ViewPDF();" Text="PDF Espa&ntilde;ol" runat="server" />
    <asp:Button ID="btnViewPDFEnglish" OnClientClick="return ViewPDFEnglish();" Text="PDF English" runat="server" />
</div>
<asp:PlaceHolder ID="plhCotiza" runat="server" />