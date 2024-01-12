<%@ Page Language="C#" MasterPageFile="~/HouseMaster.master" AutoEventWireup="true" CodeFile="DirectCaptureEndorsmentInput.aspx.cs" Inherits="Application_DirectCaptureEndorsmentInput" Title="Direct HO Endorsment" %>

<asp:Content ContentPlaceHolderID="workloadContent" ID="contentForm" runat="server">
    <script type="text/javascript" src="../jquery-ui/development-bundle/jquery-1.4.2.js"></script>
    <script type="text/javascript" src="../jquery-ui/development-bundle/ui/jquery.ui.core.js"></script>
    <script type="text/javascript" src="../jquery-ui/development-bundle/ui/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="../jquery-ui/development-bundle/ui/jquery.ui.datepicker.js"></script>
    <script type="text/javascript" src="../jquery-ui/general.js"></script>

    <script type="text/javascript">
        $(function () {
            var dates = $('#txtFechaIni, #txtFechaFin').datepicker({
                defaultDate: "+1w",
                onSelect: function (selectedDate) {
                    var option = this.id == "txtFechaIni" ? "minDate" : "maxDate";
                    var instance = $(this).data("datepicker");
                    var date = $.datepicker.parseDate(instance.settings.dateFormat || $.datepicker._defaults.dateFormat, selectedDate, instance.settings);
                    dates.not(this).datepicker("option", option, date);
                }
            });

            calcular();
        });

        function calcular() {
            var net = parseFloat(document.getElementById("ctl00_workloadContent_txtNetPremium").value);
            var fee = parseFloat(document.getElementById("ctl00_workloadContent_txtPolicyFee").value);
            var bfe = parseFloat(document.getElementById("ctl00_workloadContent_txtBrokerFee").value);
            var mas = parseFloat(document.getElementById("ctl00_workloadContent_txtSpecialCoverageFee").value);
            var rxpf = parseFloat(document.getElementById("ctl00_workloadContent_txtRXPF").value);

            var taxp = parseFloat(document.getElementById("ctl00_workloadContent_ddlTaxPercentage").value) / 100;
            var tax = (net + fee + rxpf) * taxp;

            var total = net + fee + bfe + mas + tax + rxpf;

            document.getElementById('<%= txtMexicanTax.ClientID %>').value = formatter.format(tax);
            document.getElementById('<%= txtTotalPremium.ClientID %>').value = formatter.format(total);

            var mas = parseFloat(document.getElementById("ctl00_workloadContent_txtSpecialCoverageFee").value);
            var brokerPer = parseFloat(document.getElementById("ctl00_workloadContent_txtAgentCom").value) / 100;
            var mePer = parseFloat(document.getElementById("ctl00_workloadContent_txtMECom").value) / 100;

            var brokerCom = (net + mas) * brokerPer;
            document.getElementById("ctl00_workloadContent_txtTotalCommission").value = formatter.format(brokerCom);
            var meCom = (net + mas) * mePer;
            document.getElementById("ctl00_workloadContent_txtUSCommission").value = formatter.format(meCom);

            var totCom = brokerPer + mePer;
            document.getElementById("ctl00_workloadContent_lblTotComPer").value = totCom * 100;
            totCom = brokerCom + meCom;
            document.getElementById("ctl00_workloadContent_lblTotCom").value = formatter.format(totCom);

            var tot = total - brokerCom;
            document.getElementById("ctl00_workloadContent_txtAmountDue").value = formatter.format(tot);
        }

        var formatter = new Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: 'USD',
        });
    </script>

    <form id="addEndorsment" action="post">
        <table border="0" cellspacing="0" cellpadding="0" width="95%" align="center">
            <tbody>
                <tr height="23">
                    <td colspan="4">
                        <h3>Input New Endorsment</h3>
                    </td>
                </tr>
                <tr>
                    <td class="claro" colspan="4">
                        <div class="claro"><b>&nbsp;&nbsp;Original information: </b></div>
                    </td>
                </tr>

                <tr>
                    <td class="claro" align="right">Policy No&nbsp;</td>
                    <td class="claro"><b>
                        <asp:TextBox Font-Bold="true" ReadOnly="true" ID="lblPolicyNo" runat="server" CssClass="inputRight" ForeColor="#404040" BorderStyle="None" EnableViewState="false"></asp:TextBox></b></td>
                    <td style="width: 130px" class="claro" align="right">Notes&nbsp;</td>
                    <td class="claro" rowspan="3">
                        <asp:TextBox ID="txtNotes" runat="server" CssClass="text ui-widget-content ui-corner-all" Width="300px" Height="54px" TextMode="MultiLine"></asp:TextBox>
                        <asp:Label ID="lblErrorNotes" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="claro" valign="middle" align="right">Start Date Policy&nbsp;</td>
                    <td style="width: 336px" class="claro" valign="middle" colspan="2">
                        <asp:TextBox ReadOnly="true" ID="lblStartDate" runat="server" CssClass="inputRight" ForeColor="#404040" BorderStyle="None" EnableViewState="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="claro" valign="middle" align="right">End Date Policy&nbsp;</td>
                    <td style="width: 336px" class="claro" valign="middle" colspan="2">
                        <asp:TextBox ReadOnly="true" ID="lblEndDate" runat="server" CssClass="inputRight" ForeColor="#404040" BorderStyle="None" EnableViewState="false"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td class="claro" colspan="4">&nbsp;&nbsp;<b>Premium Breakdown Endorsment</b> </td>
                </tr>
                <tr height="23">
                    <td class="claro" align="right">Inception Date&nbsp;</td>
                    <td class="claro">
                        <input id="txtFechaIni" class="text ui-widget-content ui-corner-all" size="7" name="txtFechaIni" runat="server" />
                        <asp:Label ID="lblInception" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
                    </td>
                    <td style="width: 130px" class="claro" align="right">Expiration Date&nbsp;</td>
                    <td class="claro">
                        <input id="txtFechaFin" class="text ui-widget-content ui-corner-all" size="7" name="txtFechaFin" runat="server" disabled="disabled" />
                        <asp:Label ID="lblExpiration" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
                    </td>
                </tr>
                <tr height="23">
                    <td class="claro" valign="middle" align="right">Net Premium&nbsp;</td>
                    <td class="claro" valign="middle">
                        <asp:TextBox onblur="calcular()" ID="txtNetPremium" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all">0</asp:TextBox>
                        <asp:Label ID="lblNetPremium" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
                    </td>
                    <td style="width: 130px" class="claro" valign="middle" align="right">Agent Com. %&nbsp;</td>
                    <td class="claro" valign="middle">
                        <asp:TextBox onblur="calcular()" ID="txtAgentCom" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all">0</asp:TextBox>
                        <asp:Label ID="lblAgentCom" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
                    </td>
                </tr>
                <tr height="23">
                    <td class="claro" valign="middle" align="right">Policy Fee&nbsp;</td>
                    <td class="claro" valign="middle">
                        <asp:TextBox onblur="calcular()" ID="txtPolicyFee" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all">0</asp:TextBox>
                        <asp:Label ID="lblPolicyFee" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
                    </td>
                    <td class="claro" valign="middle" align="right">Agent Commission&nbsp;</td>
                    <td class="claro" valign="middle">
                        <asp:TextBox ID="txtTotalCommission" runat="server" CssClass="inputRight" ForeColor="#404040" BorderStyle="None" EnableViewState="false">0</asp:TextBox>
                    </td>
                </tr>
                <tr height="23">
                    <td class="claro" align="right">RxPF&nbsp;</td>
                    <td class="claro">
                        <asp:TextBox onblur="calcular()" ID="txtRXPF" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all">0</asp:TextBox>
                        <asp:Label ID="lblRXPF" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
                    </td>
                    <td class="claro" valign="middle" align="right">M&amp;E Com %&nbsp;</td>
                    <td class="claro" valign="middle">
                        <asp:TextBox onblur="calcular()" ID="txtMECom" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all">0</asp:TextBox>
                        <asp:Label ID="lblUSCom" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
                    </td>
                </tr>
                <tr height="23">
                    <td class="claro" valign="middle" align="right">Tax %&nbsp;</td>
                    <td class="claro" valign="middle">
                        <asp:DropDownList onblur="calcular()" ID="ddlTaxPercentage" runat="server" CssClass="text ui-widget-content ui-corner-all">
                            <asp:ListItem Value="0">0%</asp:ListItem>
                            <asp:ListItem Value="11">11%</asp:ListItem>
                            <asp:ListItem Value="16" Selected="True">16%</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 130px" class="claro" valign="middle" align="right">M&amp;E&nbsp;Commission</td>
                    <td>
                        <asp:TextBox ID="txtUSCommission" onkeypress="checkKey1(event)" runat="server" CssClass="inputRight" ForeColor="#404040" BorderStyle="None" EnableViewState="false">0</asp:TextBox>
                    </td>
                </tr>
                <tr height="23">
                    <td class="claro" valign="middle" align="right">Mexican Tax&nbsp;</td>
                    <td>
                        <asp:TextBox ID="txtMexicanTax" runat="server" CssClass="inputRight" ForeColor="#404040" BorderStyle="None" EnableViewState="false">0</asp:TextBox>
                        <asp:Label ID="lblMexicanTax" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
                    </td>
                    <td class="claro" valign="middle" align="right">Total Commission</td>
                    <td class="claro" valign="middle">
                        <asp:TextBox ID="lblTotCom" runat="server" ForeColor="#404040" BorderStyle="None" EnableViewState="false">0</asp:TextBox>&nbsp; 
                        (<asp:TextBox ID="lblTotComPer" runat="server" Width="64px" BorderStyle="None" EnableViewState="false" BorderColor="Transparent">0</asp:TextBox>%)</td>
                </tr>
                <tr height="23">
                    <td class="claro" valign="middle" align="right">Special Coverage Fee (MAS)&nbsp;</td>
                    <td class="claro" valign="middle">
                        <asp:TextBox onblur="calcular()" ID="txtSpecialCoverageFee" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all">0</asp:TextBox>
                        <asp:Label ID="lblErrorSpecialCoverageFee" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
                    </td>
                    <td style="width: 130px" class="claro" valign="middle" align="right">Amount&nbsp;Due Policy</td>
                    <td class="claro" valign="middle">
                        <asp:TextBox ID="txtAmountDue" onkeypress="checkKey1(event)" runat="server" CssClass="inputRight" ForeColor="#404040" BorderStyle="None" EnableViewState="false">0</asp:TextBox>
                        <asp:Label ID="lblAmountDue" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
                    </td>
                </tr>
                <tr height="23">
                    <td class="claro" valign="middle" align="right">Managing Agent Broker Fee&nbsp;</td>
                    <td class="claro" valign="middle">
                        <asp:TextBox onblur="calcular()" ID="txtBrokerFee" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all">0</asp:TextBox>
                        <asp:Label ID="lblBrokerFee" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
                    </td>
                    <td class="claro" valign="middle" align="right">Endorsment type&nbsp;</td>
                    <td class="claro" valign="middle">
                        <asp:DropDownList ID="ddlEndorsment" runat="server" CssClass="text ui-widget-content ui-corner-all">
                            <asp:ListItem Value="ADD" Selected="True">A</asp:ListItem>
                            <asp:ListItem Value="DELETE">D</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr height="23">
                    <td class="claro" valign="middle" align="right">Total Premium&nbsp;</td>
                    <td class="claro" valign="middle">
                        <asp:TextBox ID="txtTotalPremium" onkeypress="checkKey1(event)" runat="server" CssClass="inputRight" ForeColor="#404040" BorderStyle="None" EnableViewState="false">0</asp:TextBox>
                        <asp:Label ID="lblTotalPremium" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
                    </td>
                    <td class="claro" valign="middle" align="right"></td>
                    <td class="claro" valign="middle">
                        <asp:TextBox ID="txtInstSurcharge" onkeypress="checkKey1(event)" runat="server" CssClass="inputRight" Visible="False">0</asp:TextBox>
                        <asp:Label ID="lblInstSurcharge" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
                    </td>
                </tr>
                <tr height="23">
                    <td class="claro" valign="middle" align="right"></td>
                    <td class="claro" valign="middle" colspan="3"></td>
                </tr>
                <tr height="23">
                    <td style="width: 525px" class="LowerLeftMedium" valign="middle" width="98%" colspan="3" align="right">&nbsp; </td>
                    <td class="lowerRight" align="right">
                        <asp:Button ID="btnSave" CssClass="ui-state-default" runat="server" Text="Save"></asp:Button></td>
                </tr>
            </tbody>
        </table>
    </form>
</asp:Content>
