<%@ Register TagPrefix="cc1" Namespace="Prodoc.Administracion" Assembly="ProGrid" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HouseEstate.aspx.cs" Inherits="Catalog_HouseEstate" MasterPageFile="~/HouseMaster.master" Title="Catalog: House Estate" %>

<asp:Content ID="cntHolder" ContentPlaceHolderID="workloadContent" runat="server">
    <script type="text/javascript">
        var isNav;
        function checkKeyInt(e) {
            var keyChar;

            if (isNav == 0) {
                if (e.which < 48 || e.which > 57) e.RETURNVALUE = false;
            } else {
                if (e.keyCode < 48 || e.keyCode > 57) e.returnValue = false;
            }
        }
    </script>

    <h1 class="pageHeader">House Estate</h1>

    <table cellspacing="0" cellpadding="5" style="margin-top: 10px" width="95%" border="1" align="center">
        <tr>
            <td class="claro" colspan="4">
                <asp:Label ID="lblZip" AssociatedControlID="txtZipCode" Text="Zip Code:&nbsp;&nbsp;" runat="server" />
                <asp:TextBox ID="txtZipCode" onkeypress="checkKeyInt(event)" MinLength="5" MaxLength="5" runat="server" />
                <asp:Button Style="padding: 1px 3px;" ID="btnSearch" OnClick="search_Click" Text="Search" runat="server" />
                <asp:RequiredFieldValidator ID="reqZipCode" Display="Dynamic" ControlToValidate="txtZipCode" Text="Required" runat="server" />
                <asp:Label ID="lblZipNotFound" CssClass="failureText" Text="" Visible="true" runat="server" />
                <asp:HyperLink ID="HyperLink1" NavigateUrl="https://www.correosdemexico.gob.mx/SSLServicios/ConsultaCP/Descarga.aspx" Target="_blank" Text="Don't know the zip code? Click here" runat="server" />
            </td>
        </tr>
        <tr id="houseEstates" visible="false" runat="server">
            <td class="claro" colspan="2">&nbsp;State:&nbsp;&nbsp;
                <asp:DropDownList ID="ddlState" runat="server"
                    DataValueField="ID" DataTextField="Name" AutoPostBack="true"
                    CssClass="input" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                </asp:DropDownList>
            </td>

            <td class="claro" colspan="2">&nbsp;City&nbsp;&nbsp;
                <asp:DropDownList ID="ddlCity" runat="server"
                    DataValueField="ID" DataTextField="Name" AutoPostBack="true"
                    CssClass="input" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr id="tableHouseEstate" visible="false" runat="server">
            <td class="claro" colspan="4">
                <cc1:ProGrid ID="grid" CssClass="catalogTable" AllowPaging="true" allowmultiselect="false" allowmultiselectfooter="false" Width="95%"
                    DataKeyField="ID" OnDeleteCommand="Delete" OnUpdateCommand="UpdateCommand" PageSize="15"
                    PagerStyle-CssClass="catalogTableFooter" GridLines="None" CellSpacing="1" AlternatingRowStyle-CssClass="catalogTableAlternate"
                    EditRowStyle-CssClass="catalogTableEdit" HeaderStyle-CssClass="catalogTableHeader" runat="server">
                    <EditItemStyle CssClass="catalogTableEdit"></EditItemStyle>
                    <AlternatingItemStyle CssClass="catalogTableAlternate"></AlternatingItemStyle>
                    <ItemStyle CssClass="catalogTable"></ItemStyle>
                    <HeaderStyle HorizontalAlign="Center" CssClass="catalogTableHeader"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="Name" HeaderText="Name"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TEV" HeaderText="Zona TEV"></asp:BoundColumn>
                        <asp:BoundColumn DataField="City" HeaderText="City" ReadOnly></asp:BoundColumn>
                        <asp:BoundColumn DataField="ZipCode" HeaderText="Zip Code" ReadOnly></asp:BoundColumn>
                        <asp:EditCommandColumn
                            ButtonType="LinkButton"
                            UpdateText="&lt;img src=../../images/OK.gif Border=0 alt='Save'&gt;"
                            HeaderText="Edit"
                            CancelText="&lt;img src=../../images/Cancel.gif Border=0 alt='Cancel'&gt;"
                            EditText="&lt;img src=../../images/Edit.gif Border=0 alt='Update information'&gt;">
                            <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                        </asp:EditCommandColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" CssClass="catalogTableFooter" Mode="NumericPages"></PagerStyle>
                </cc1:ProGrid>
            </td>
        </tr>
    </table>
</asp:Content>
