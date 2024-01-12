<%@ Page Language="C#" MasterPageFile="~/HouseMaster.master" AutoEventWireup="true" CodeFile="ReportOutdoorProperty.aspx.cs" Inherits="Reports_ReportOutdoorProperty" Title="Policy Reports Outdoor Properties" %>

<asp:Content ID="Content1" ContentPlaceHolderID="workloadContent" runat="Server">

    <script type="text/javascript">

        function GetExcelReport() {
            var agent = $get('<%= txtAgent.ClientID %>').value;
            var startDate = $get('<%= txtStartDate.ClientID %>').value;
            var endDate = $get('<%= txtEndDate.ClientID %>').value;
            var policy = $get('<%= txtPolicy.ClientID %>').value;
            var name = $get('<%= txtName.ClientID %>').value;
            location.href = '/Reports/ReportExcel.ashx?id=' + policy + '&outdoor=2' + '&name=' + name + '&agent=' + agent + '&startDate=' + startDate + '&endDate=' + endDate;
            return false;
        }

    </script>

    <table>
        <tr>
            <td class="pageHeader">Policy Reports (Only policies with outdoor properties)</td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>Broker: </td>
                        <td>
                            <asp:TextBox ID="txtAgent" runat="server" />
                            <asp:CompareValidator ID="cmpAgent" Display="Dynamic" Text="Invalid value" ControlToValidate="txtAgent" Operator="DataTypeCheck" Type="Integer" runat="server" />
                        </td>
                        <td>Issue Date Range: </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtStartDate" EnableViewState="false" runat="server" />
                            -
                            <asp:TextBox ID="txtEndDate" EnableViewState="false" runat="server" />
                            <ajax:CalendarExtender ID="calStartDate" TargetControlID="txtStartDate" runat="server" />
                            <ajax:CalendarExtender ID="calEndDate" TargetControlID="txtEndDate" runat="server" />
                            <asp:CompareValidator ID="cmpStartDate" ControlToValidate="txtStartDate" Display="Dynamic" Text="Invalid Date" Operator="DataTypeCheck" Type="Date" runat="server" />
                            <asp:CompareValidator ID="cmpEndDate" ControlToValidate="txtEndDate" Display="Dynamic" Text="Invalid Date" Operator="DataTypeCheck" Type="Date" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>Policy Number: </td>
                        <td>
                            <asp:TextBox ID="txtPolicy" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="lblName" Text="Client's Name: " AssociatedControlID="txtName" runat="server" /></td>
                        <td>
                            <asp:TextBox ID="txtName" MaxLength="64" runat="server" /></td>
                        <td align="right">
                            <asp:Button ID="btnReporte" OnClick="btnSearch_Click" Text="Search" runat="server" />
                            <input type="button" onclick="return GetExcelReport();" value="Get Report in an Excel" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:PlaceHolder ID="plhReportes" runat="server" />
</asp:Content>
