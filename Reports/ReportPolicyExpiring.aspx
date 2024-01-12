<%@ Page Language="C#" MasterPageFile="~/HouseMaster.master" AutoEventWireup="true" CodeFile="ReportPolicyExpiring.aspx.cs" Inherits="ReportPolicyExpiring" Title="Expiring Policy Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="workloadContent" Runat="Server">

<script type="text/javascript">

    function GetExcelReport() {
        var startDate = $get('<%= txtStartDate.ClientID %>').value;
        var endDate = $get('<%= txtEndDate.ClientID %>').value;
        var policyNo = $('#<%= txtPolicyNumber.ClientID %>').val();
        var name = $('#<%= txtName.ClientID %>').val();
        var test = $('#<%= ddlIncludeTest.ClientID %>').val();
        var includeRenewed = $('#<%= ddlIncludeRenewed.ClientID %>').val();
        location.href = '<%= VirtualPathUtility.ToAbsolute("~/Reports/ReportExcel.ashx") %>?startDate=' + startDate + '&endDate=' + endDate + '&test=' + test + '&expiring=1' +
                        '&policyNo=' + policyNo + '&name=' + name + '&includeRenewed=' + includeRenewed;
        return false;
    }

    function Search() {
        var startDate = $get('<%= txtStartDate.ClientID %>').value;
        var endDate = $get('<%= txtEndDate.ClientID %>').value;
        var policyNo = $('#<%= txtPolicyNumber.ClientID %>').val();
        var name = $('#<%= txtName.ClientID %>').val();
        var test = $('#<%= ddlIncludeTest.ClientID %>').val();
        var includeRenewed = $('#<%= ddlIncludeRenewed.ClientID %>').val();
        location.href = '<%= VirtualPathUtility.ToAbsolute("~/Reports/ReportPolicyExpiring.aspx") %>?startDateExp=' + startDate +
                        '&endDateExp=' + endDate + '&test=' + test + '&search=1' + '&policyNo=' + policyNo + '&name=' + name + '&includeRenewed=' + includeRenewed;
        return false;
    }

</script>
<br />
<table style="width: 80%;">
    <tr>
        <td class="pageHeader">Expiring Policy Reports</td>
    </tr>
    <tr>
        <td>
            <table>
                <tr>
                    <td>
                        Date Range of Expiricy Date: 
                    </td>
                    <td>
                         <asp:TextBox ID="txtStartDate" EnableViewState="false" runat="server" /> - 
                   
                        <asp:TextBox ID="txtEndDate" EnableViewState="false" runat="server" />
                        <ajax:CalendarExtender ID="calStartDate" TargetControlID="txtStartDate" runat="server" />
                        <ajax:CalendarExtender ID="calEndDate" TargetControlID="txtEndDate" runat="server" />
                        <asp:CompareValidator ID="cmpStartDate" ControlToValidate="txtStartDate" Display="Dynamic" Text="Invalid Date" Operator="DataTypeCheck" Type="Date" runat="server" />
                        <asp:CompareValidator ID="cmpEndDate" ControlToValidate="txtEndDate" Display="Dynamic" Text="Invalid Date" Operator="DataTypeCheck" Type="Date" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Client's name:
                    </td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Policy Number:
                    </td>
                    <td>
                        <asp:TextBox ID="txtPolicyNumber" runat="server" />
                        
                    </td>
                </tr>
                <tr>
                    <td>
                        Include already renewed policies?
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlIncludeRenewed" runat="server">
                            <asp:ListItem Text="No" Value="0" />
                            <asp:ListItem Text="Yes" Value="1" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Include Test Policies?
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlIncludeTest" runat="server">
                            <asp:ListItem Text="No" Value="0" />
                            <asp:ListItem Text="Yes" Value="1" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <input type="button" value="Search" onclick="return Search();" />
                        <input type="button" onclick="return GetExcelReport();" value="Get Report in an Excel" />
                    </td>
                </tr>
            </table>
           
            
            
        </td>
    </tr>
    
</table>
<asp:PlaceHolder ID="plhReportes" EnableViewState="false" runat="server" />
</asp:Content>

