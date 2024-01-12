<%@ Page Title="" Language="C#" MasterPageFile="~/HouseMaster.master" AutoEventWireup="true" CodeFile="SpecializedReports.aspx.cs" Inherits="Reports_SpecializedReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="workloadContent" Runat="Server">
    <script type="text/javascript">

    function GetExcelReport() {
        var startDate = $get('<%= txtStartDate.ClientID %>').value;
        var endDate = $get('<%= txtEndDate.ClientID %>').value;
        var name = $get('<%= txtName.ClientID %>').value;
        location.href = '/Reports/ReportExcel.ashx?id=' + policy + '&name=' + name + '&agent=' + agent + '&startDate=' + startDate + '&endDate=' + endDate;
        return false;
    }

</script>

<table>
    <tr>
        <td class="pageHeader">Policy Reports</td>
    </tr>
    <tr>
        <td>
        <table>
            <tr>
                <td>Roof Type: </td><td><asp:DropDownList ID="ddlRoof" CssClass="step2" OnDataBound="ddlDataBound" DataSourceID="odsRoof" DataTextField="description" DataValueField="type" runat="server" /></td>
                <td>Issue Date Range: </td><td colspan="2"><asp:TextBox ID="txtStartDate" EnableViewState="false" runat="server" /> - <asp:TextBox ID="txtEndDate" EnableViewState="false" runat="server" />
                    <ajax:CalendarExtender ID="calStartDate" TargetControlID="txtStartDate" runat="server" />
                    <ajax:CalendarExtender ID="calEndDate" TargetControlID="txtEndDate" runat="server" />
                    <asp:CompareValidator ID="cmpStartDate" ControlToValidate="txtStartDate" Display="Dynamic" Text="Invalid Date" Operator="DataTypeCheck" Type="Date" runat="server" />
                    <asp:CompareValidator ID="cmpEndDate" ControlToValidate="txtEndDate" Display="Dynamic" Text="Invalid Date" Operator="DataTypeCheck" Type="Date" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Minimum Home Value: </td><td><asp:TextBox ID="txtValue" runat="server" />
                </td>
                <td><asp:Label ID="lblName" Text="Client's Name: " AssociatedControlID="txtName" runat="server" /></td>
                <td><asp:TextBox ID="txtName" MaxLength="64" runat="server" /></td>
                <td align="right">
                    <asp:Button ID="btnReporte" OnClick="btnSearch_Click" Text="Search" runat="server" />
                    <input type="button" onclick="return GetExcelReport();" value="Get Report in an Excel" />
                </td>
            </tr>
        </table>
        </td>
    </tr>
</table>
    
<asp:ObjectDataSource ID="odsRoof" TypeName="roofType" SelectMethod="SelectByLanguage" runat="server" />
<asp:ObjectDataSource ID="odsWall" TypeName="wallType" SelectMethod="SelectByLanguage" runat="server" />
<asp:ObjectDataSource ID="odsDistanceSea" TypeName="distanceFromSea" SelectMethod="SelectForRBLByLanguage" runat="server" />
<asp:PlaceHolder ID="plhReportes" runat="server" />
</asp:Content>

