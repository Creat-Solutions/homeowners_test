<%@ Page Language="C#" MasterPageFile="~/HouseMaster.master" AutoEventWireup="true" CodeFile="DirectCaptureEndorsment.aspx.cs" Inherits="Application_DirectCaptureEndorsment" Title="Direct HO Endorsment" %>

<asp:Content ContentPlaceHolderID="workloadContent" ID="contentForm" runat="server">
    <table>
    <tr>
        <td class="pageHeader">Type the policy number for which you want to input  a new endorsment</td>
    </tr>
    <tr>
        <td>
        <table>
            <tr>
                <td>Policy No: </td><td><asp:TextBox ID="txtPolicy" runat="server" />
                </td>
                <td align="right">
                    <asp:Button ID="btnReporte" OnClick="btnSearch_Click" Text="Search" runat="server" />
                </td>
            </tr>
        </table>
        </td>
    </tr>
    </table>
    
    <asp:PlaceHolder ID="plhReportes" runat="server" />
</asp:Content>