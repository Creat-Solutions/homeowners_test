<%@ Register TagPrefix="cc1" Namespace="Prodoc.Administracion" Assembly="ProGrid" %>

<%@ Page Language="C#" MasterPageFile="~/HouseMaster.master" AutoEventWireup="true" CodeFile="EditPolicyFeeLimits.aspx.cs" Inherits="Application_EditPolicyFeeLimits" Title="Edit Policy Fee Limits" %>

<asp:Content ContentPlaceHolderID="workloadContent" ID="contentForm" runat="server">
    <table width="100%">
        <tr>
            <td class="quoteStep borderStep" valign="top" style="width: 40%;"></td>
            <td class="quoteStep" valign="top"></td>
        </tr>
    </table>
    <table cellspacing="0" cellpadding="0" width="95%" border="0" align="center">
        <tr>
            <td class="Claro" colspan="4"><font size="+1"><b>Policy Fee Limits</b></font></td>
        </tr>
        <tr>
            <td class="claro">&nbsp;Super Producer:
            </td>
            <td class="claro">
                <asp:DropDownList ID="ddlSuperProducer" runat="server"
                    DataValueField="superProducer" DataTextField="name" AutoPostBack="true"
                    CssClass="input" OnSelectedIndexChanged="ddlSuperProducer_SelectedIndexChanged">
                </asp:DropDownList>
            </td>

            <td class="claro">Producer:
            </td>
            <td class="claro">
                <asp:DropDownList ID="ddlProducer" runat="server"
                    DataValueField="producer" DataTextField="name" AutoPostBack="True"
                    CssClass="input" OnSelectedIndexChanged="ddlProducer_SelectedIndexChanged">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td class="claro">&nbsp;Agent:
            </td>
            <td class="claro">
                <asp:DropDownList ID="ddlAgent" runat="server"
                    DataValueField="agent" DataTextField="name" AutoPostBack="True"
                    CssClass="input" OnSelectedIndexChanged="ddlAgent_SelectedIndexChanged">
                </asp:DropDownList>
            </td>

            <td class="claro">&nbsp;Branch:
            </td>
            <td class="claro">
                <asp:DropDownList ID="ddlBranch" runat="server"
                    DataValueField="branch" DataTextField="address" AutoPostBack="True"
                    CssClass="input" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr runat="server" id="changeFee" visible="false">
            <td class="claro">&nbsp;Change Policy Fee:
            </td>
            <td class="Claro">
                <asp:CheckBox ID="chkFee" runat="server" OnCheckedChanged="chkFee_CheckedChanged" AutoPostBack="true" />
            </td>
        </tr>
        <tr runat="server" id="tableLimits" visible="false">
            <td colspan="4">
                <cc1:ProGrid ID="grid" CssClass="catalogTable" runat="server" AllowPaging="true" allowmultiselect="false" allowmultiselectfooter="false"
                    DataKeyField="policyFeeLimit" Width="100%" OnDeleteCommand="Delete" OnUpdateCommand="UpdateCommand" PageSize="15">
                    <EditItemStyle CssClass="Select"></EditItemStyle>
                    <AlternatingItemStyle CssClass="Claro"></AlternatingItemStyle>
                    <ItemStyle CssClass="Medio"></ItemStyle>
                    <HeaderStyle HorizontalAlign="Center" ForeColor="White" CssClass="Oscuro"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="lowLimitNetPremium" HeaderText="Low Premium"></asp:BoundColumn>
                        <asp:BoundColumn DataField="highLimitNetPremium" HeaderText="High Premium"></asp:BoundColumn>
                        <asp:BoundColumn DataField="lowLimitFee" HeaderText="Low Fee"></asp:BoundColumn>
                        <asp:BoundColumn DataField="highLimitFee" HeaderText="High Fee"></asp:BoundColumn>
                        <asp:EditCommandColumn ButtonType="LinkButton" UpdateText="&lt;img src=../../images/OK.gif Border=0 alt='Save'&gt;"
                            HeaderText="Edit" CancelText="&lt;img src=../../images/Cancel.gif Border=0 alt='Cancel'&gt;" EditText="&lt;img src=../../images/Edit.gif Border=0 alt='Update information'&gt;">
                            <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                        </asp:EditCommandColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" ForeColor="White" CssClass="Oscuro" Mode="NumericPages"></PagerStyle>
                </cc1:ProGrid>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
                <br />
                <asp:Label runat="server" ID="lblError" Font-Size="Small" ForeColor="Red" Font-Bold="True" Visible="true"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
