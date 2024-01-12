<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Cotiza.ascx.cs" Inherits="UserControls_Cotiza" %>
<table style="width: 100%;">
    <tbody>
        <tr>
            <td valign="top" style="width: 25%; text-align: center;">
                <asp:Image ImageUrl="~/images/logoME2.jpg" Width="150" AlternateText="Macafee and Edwards"
                    runat="server" />
            </td>
            <td valign="top" style="width: 49%; text-align: center; border-left: dotted 1px grey;
                border-right: dotted 1px grey;">
                <h1 class="cTitle">
                    MacAfee &amp; Edwards, Inc.</h1>
                <h2 class="cSub">
                    Mexican Insurance Specialist</h2>
                <h1 class="cTitle cColor">
                    HOMEOWNERS QUOTE</h1>
            </td>
            <td valign="top" style="width: 25%;">
                700 S. Flower Street #1216
                <br />
                Los Angeles, CA 90017<br />
                P. (213) 629-9777<br />
                F. (213) 629-9779
            </td>
        </tr>
    </tbody>
</table>
<h1 class="cSectionTitle">
    - GENERAL INFORMATION -
</h1>
Policy Term (Per&iacute;odo):
<asp:Label ID="lblPeriodStart" CssClass="cData" runat="server" />
to (al)
<asp:Label ID="lblPeriodEnd" CssClass="cData" runat="server" />
Days (D&iacute;as):
<asp:Label ID="lblPeriodDays" CssClass="cData" runat="server" />
<table class="cTable">
    <tbody>
        <tr>
            <td class="cAR">
                Named Insured:
            </td>
            <td>
                <asp:Label ID="lblName" CssClass="cData" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="cAR">
                Additional Insured:
            </td>
            <td>
                <asp:Label ID="lblAddInsured" CssClass="cData" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="cAR">
                Loss Payee (Beneficiario preferente):
            </td>
            <td>
                <asp:Label ID="lblPayee" CssClass="cData" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="cAR">
                Loan Number (N&uacute;mero de cr&eacute;dito):
            </td>
            <td>
                <asp:Label ID="lblLoanNumber" CssClass="cData" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="cAR">
                Address (Direcci&oacute;n):
            </td>
            <td>
                <asp:Label ID="lblAddress1" CssClass="cData" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Label ID="lblAddress2" CssClass="cData" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <span class="cData">MEXICO</span>
            </td>
        </tr>
        <tr>
            <td class="cAR">
                Jurisdiction (Jurisdicci&oacute;n):
            </td>
            <td>
                <span class="cData">Only in the Republic of Mexico (M&eacute;xico &uacute;nicamente)</span>
            </td>
        </tr>
    </tbody>
</table>
QUOTE BASE ON THE FOLLOWING PARAMETERS (CUOTA DE ACUERDO A LOS SIGUIENTES PAR&Aacute;METROS):<br />
<table class="cTable">
    <tbody>
        <tr>
            <td class="cAR" style="width: 35%;" valign="top">
                Type of Property (Tipo de Propiedad):
            </td>
            <td valign="top">
                <asp:Label ID="lblTypeOfProperty" CssClass="cData" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="cAR" valign="top">
                Use (Uso):
            </td>
            <td valign="top">
                <asp:Label ID="lblUseOfProperty" CssClass="cData" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="cAR" valign="top">
                Beach Front (Distancia al mar):
            </td>
            <td valign="top">
                <asp:Label ID="lblBeachFront" CssClass="cData" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="cAR" valign="top">
                Adjacent Structures (Colindantes sin fincar):
            </td>
            <td valign="top">
                <asp:Label ID="lblAdjacent" CssClass="cData" runat="server" />
            </td>
        </tr>
    </tbody>
</table>
<h1 class="cSectionTitle">
    - PROPERTY COVERAGE (BIENES CUBIERTOS) -
</h1>
<h3 class="cSectionSubtitle">
    I. LIMITS (L&Iacute;MITES)</h3>
<table class="cTable">
    <tbody>
        <tr>
            <td class="cAR">
                Currency (Moneda):
            </td>
            <td class="cData">
                &nbsp;
            </td>
            <td class="cData" style="text-align: right;">
                <asp:Label ID="lblCurrency" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="cAR">
                Building (Edificio):
            </td>
            <td class="cData">
                <asp:Label ID="lblBuildingMoney" Text="$" runat="server" />
            </td>
            <td style="text-align: right;">
                <asp:Label ID="lblBuildingLimit" CssClass="cData" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="cAR">
                Contents (Contenidos):
            </td>
            <td class="cData">
                $
            </td>
            <td style="text-align: right;">
                <asp:Label ID="lblContentsLimit" CssClass="cData" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="cAR">
                <sup>1</sup>Outdoor Property(Bienes a la intemperie):
            </td>
            <td class="cData">
                <asp:Label ID="lblOutdoorMoney" Text="$" runat="server" />
            </td>
            <td style="text-align: right;">
                <asp:Label ID="lblOutdoorLimit" CssClass="cData" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="cAR">
                Debri removal (Rem. escombros):
            </td>
            <td class="cData">
                <asp:Label ID="lblDebriMoney" Text="$" runat="server" />
            </td>
            <td style="text-align: right;">
                <asp:Label ID="lblDebriLimit" CssClass="cData" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="cAR">
                Extra Expenses (Gastos Extras):
            </td>
            <td class="cData">
                <asp:Label ID="lblExtraMoney" Text="$" runat="server" />
            </td>
            <td style="text-align: right;">
                <asp:Label ID="lblExtraLimit" CssClass="cData" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="cAR">
                Loss Of Rent - 4 months - (P&eacute;rdida de rentas - 4 meses):
            </td>
            <td class="cData">
                <asp:Label ID="lblLossRentMoney" Text="$" runat="server" />
            </td>
            <td style="text-align: right;">
                <asp:Label ID="lblLossOfRent" CssClass="cData" runat="server" />
            </td>
        </tr>
    </tbody>
</table>
<div class="smallL">
    <span style="font-weight: bold;"><sup>1</sup>Outdoor Property covered (Bienes a la Intemperie
        cubiertos):</span> Antennas/Satellites, Pools, Awnings, Palapas, Gardens &amp;
    Decorative Constructions, Streets, Patios, Decks, Roads, Outdoor Furniture, Docks,
    Buildings lacking roofs, windows or walls, Electric Substations, Sporting Installations,
    Signs, etc. - This type of Property may be covered by specifically scheduling (with
    it's respective value) each individual item to be covered
    <br />
    <span style="font-weight: bold;">-IF IT'S NOT SCHEDULED, IT'S NOT COVERED-.</span>
</div>
<h3 class="cSectionSubtitle">
    II. COVERAGES &amp; DEDUCTIBLES (COBERTURAS Y DEDUCIBLES)</h3>
<table class="cTable">
    <tbody>
        <tr>
            <td class="cAR">
                Fire (Incendio):
            </td>
            <td>
                All Risk Mexican Form (Todo Riesgo forma Mexicana)
            </td>
        </tr>
        <tr>
            <td class="cAR">
                Earthquake (Terremoto):
            </td>
            <td>
                <asp:Label ID="lblEarthquake" CssClass="cData" runat="server" /><%--<asp:CheckBox ID="chkQuake" OnCheckedChanged="Quake_CheckedChanged" AutoPostBack="true" runat="server" />--%>
            </td>
        </tr>
        <tr>
            <td class="cAR">
                <sup>2</sup>HMP (FHM):
            </td>
            <td>
                <asp:Label ID="lblHMP" CssClass="cData" runat="server" /><%--<asp:CheckBox ID="chkHMP" OnCheckedChanged="HMP_CheckedChanged" AutoPostBack="true" runat="server" />--%>
            </td>
        </tr>
    </tbody>
</table>
<div style="font-weight: bold; text-align: center;">
    EARTHQUAKE AND <sup>2</sup>HMP ARE EXCLUDED FOR LOSS OF RENT (TERREMOTO Y FHM ESTAN
    EXCLUIDOS PARA LA COBERTURA DE PERDIDA DE RENTAS)
</div>
<div class="smallL">
    <sup>2</sup>HMP: (Hydro-Meteorological Phenomenon includes the following perils,
    Mudslide, hail, frost, hurricane, flood, flood by rain, wave wash/Tidal, waves,
    hail) FHM (Fenómenos Hidro-Meteorológicos incluye los siguientes riesgos: avalancha
    de lodo, granizo, helado, huracán, Inundación, Inundación por lluvia, marejada,
    golpe de mar, Nevada y vientos tempestuosos)
</div>
<table class="cTable">
    <tbody>
        <tr>
            <th>
            </th>
            <th style="border: 1px dotted gray;">
                Deductible/<br />
                Deducible
            </th>
            <th style="border: 1px dotted gray">
                Co-Insurance/<br />
                Co-aseguro
            </th>
        </tr>
        <tr>
            <td class="cAR">
                Fire (Incendio):
            </td>
            <td style="text-align: center; border: 1px dotted gray;">
                None
            </td>
            <td style="text-align: center; border: 1px dotted gray;">
                None
            </td>
        </tr>
        <tr>
            <td class="cAR">
                Earthquake (Terremoto):
            </td>
            <td style="text-align: center; border: 1px dotted gray;">
                <asp:Label ID="lblTEVDeducible" runat="server" />
            </td>
            <td style="text-align: center; border: 1px dotted gray;">
                <asp:Label ID="lblTEVCoaseguro" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="cAR">
                *HMP (FHM):
            </td>
            <td style="text-align: center; border: 1px dotted gray;">
                <asp:Label ID="lblHMPDeducible" runat="server" />
            </td>
            <td style="text-align: center; border: 1px dotted gray;">
                <asp:Label ID="lblHMPCoaseguro" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="cAR">
                Outdoor Property (Bienes a la intemperie):
            </td>
            <td style="text-align: center; border: 1px dotted gray;">
                <asp:Label ID="lblOutdoorDeducible" runat="server" />
            </td>
            <td style="text-align: center; border: 1px dotted gray;">
                <asp:Label ID="lblOutdoorCoaseguro" runat="server" />
            </td>
        </tr>
    </tbody>
</table>
<h3 class="cSectionSubtitle">
    III. SPECIAL CLAUSES (CL&Aacute;USULAS ESPECIALES)</h3>
<table class="cTable">
    <tbody>
        <tr>
            <td>
                Valuation (Valuaci&oacute;n):
            </td>
            <td style="font-weight: bold;">
                <span class="cData">Replacement Cost for all coverage (Valor de Reposición para todas
                    las coberturas). </span>
            </td>
        </tr>
        <tr>
            <td valign="top">
                Note 1 (Nota 1):
            </td>
            <td style="font-weight: bold;">
                <span class="cData">First risk insurance except for the Earthquake coverage in which
                    we need at least 80% of the values. (Seguro a primer riesgo absoluto excepto para
                    la cobertura de Terremoto para la cual necesitamos por los menos el 80% de los valores,
                    la cual cuenta con un coaseguro del 80%). </span>
            </td>
        </tr>
    </tbody>
</table>
<h1 class="cSectionTitle">
    - ADDITIONAL COVERAGES (COBERTURAS ADICIONALES) -
</h1>
<asp:PlaceHolder ID="plhAdditional" runat="server" />
<table style="width: 100%;">
    <tbody>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                Insurance Carrier Information:<br />
                Mapfre México SA<br />
                AM Best Rating " A - "<br />
                www.mapfre.com.mx<br />
                <asp:Image ImageUrl="~/images/logoMapfre.png" AlternateText="Mapfre" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                This quotation is based on terms &amp; conditions of a standard Mexican Insurance
                policy. Please be aware that Mexican Insurance policies have different terms &amp;
                conditions from policies applicable in the USA. If you need to review the coverage
                terms, you can request a sample copy.
            </td>
        </tr>
    </tbody>
</table>

<script type="text/javascript">

    function disableNoPayment() {
        if (window.mustPayWithCC !== undefined) {
            mustPayWithCC(true);
        }
    }
    
    function enableNoPayment() {
        if (window.mustPayWithCC !== undefined) {
            mustPayWithCC(false);
        }
    }

    function payMonthly() {
        registerPayment(disableNoPayment, 12);
    }

    function payQuarterly() {
        registerPayment(enableNoPayment, 4);
    }

    function paySemiannual() {
        registerPayment(enableNoPayment, 2);
    }

    function payCash() {
        registerPayment(enableNoPayment, 1);
    }

    function registerPayment(callback, payments) {
        $.ajax({
            url: '<%= VirtualPathUtility.ToAbsolute("~/Application/SavePaymentType.ashx") %>',
            type: 'POST',
            data: { 'quoteid': <%= quoteid %>, 'numberOfPayments': payments },
            success: function(data) {
                if (data.Result) {
                    if (callback) {
                        callback();
                    }
                } else {
                    alert("Error with the server. Please try again!");
                }
            },
            error: function(error, exc) {
                alert("Error with the server. Please try again!");
            }
        });
    }

</script>

