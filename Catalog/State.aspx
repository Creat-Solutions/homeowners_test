<%@ Page Language="C#" AutoEventWireup="true" CodeFile="State.aspx.cs" Inherits="Catalog_State" MasterPageFile="~/HouseMaster.master" Title="Catalog: State" %>

<asp:Content ID="cntHolder" ContentPlaceHolderID="workloadContent" runat="server">
    <h1 class="pageHeader">State</h1>

    <table cellpadding="0" cellspacing="5" style="margin-top: 10px" width="95%" border="1" align="center">
        <tr>
            <td class="claro" colspan="4">
                <asp:Label ID="lblSCountry" Text="Country: " runat="server" />
                <asp:DropDownList ID="ddlSCountry" AutoPostBack="true" AppendDataBoundItems="true" EnableViewState="false" DataSourceID="odsForeignSource" DataTextField="Name" DataValueField="ID" runat="server">
                    <asp:ListItem Selected="True" Text="---" Value="0" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator ErrorMessage="Required" ControlToValidate="ddlSCountry"
                    InitialValue="0" runat="server" ForeColor="Red" />
            </td>
        </tr>
        <tr>
            <td class="claro" colspan="4">
                <asp:GridView ID="grdState" FooterStyle-CssClass="catalogTableFooter" ShowFooter="true" OnRowCreated="RowCreated" Width="95%"
                    GridLines="None" CellSpacing="1" AlternatingRowStyle-CssClass="catalogTableAlternate" CssClass="catalogTable"
                    EditRowStyle-CssClass="catalogTableEdit" HeaderStyle-CssClass="catalogTableHeader"
                    OnDataBound="grdState_DataBound" EmptyDataText="This catalog is empty." DataKeyNames="ID" DataSourceID="odsGridSource" AutoGenerateColumns="false" runat="server">
                    <Columns>
                        <%--<user:ForeignField ForeignKeyField="ID" ForeignTextField="Name" DataField="Country" DataSourceID="odsForeignSource" HeaderText="Country" />--%>
                        <asp:TemplateField HeaderText="Country">
                            <ItemTemplate>
                                <%# Eval("CountryName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <%# Eval("Name") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditName" MaxLength="32" Text='<%# Bind("Name") %>' runat="server" />
                                <user:LengthValidator ID="lenName" Text="(too many characters)" MaxLength="32" Display="Dynamic" ControlToValidate="txtEditName" runat="server" />
                                <asp:RequiredFieldValidator ID="valName" Display="Dynamic" ControlToValidate="txtEditName" Text="(required)" runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Abbreviation">
                            <ItemTemplate>
                                <%# Eval("Abbreviation") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditAbb" MaxLength="4" Text='<%# Bind("Abbreviation") %>' runat="server" />
                                <user:LengthValidator ID="lenAbb" Text="Too many characters" MaxLength="4" Display="Dynamic" ControlToValidate="txtEditAbb" runat="server" />
                                <asp:RequiredFieldValidator ID="valAbb" Display="Dynamic" ControlToValidate="txtEditAbb" Text="Required" runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Zurich Code">
                            <ItemTemplate>
                                <%# Eval("ZurichCode") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtZClave" MaxLength="16" Text='<%# Bind("ZurichCode") %>' runat="server" />
                                <asp:RequiredFieldValidator ID="reqZClave" Text="Required" ControlToValidate="txtZClave" Display="Dynamic" runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mapfre Code">
                            <ItemTemplate>
                                <%# Eval("MapfreCode") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtClave" MaxLength="32" Text='<%# Bind("MapfreCode") %>' runat="server" />
                                <asp:CompareValidator ID="cmpClave" Display="Dynamic" Text="Input not valid" Operator="DataTypeCheck" Type="Integer" ControlToValidate="txtClave" runat="server" />
                                <asp:RequiredFieldValidator ID="reqClave" Text="Required" ControlToValidate="txtClave" Display="Dynamic" runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Theft Zone">
                            <ItemTemplate>
                                <%# Eval("TheftZone") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtZonaRobo" MaxLength="1" Text='<%# Bind("TheftZone") %>' runat="server" />
                                <user:LengthValidator ID="lenZonaRobo" MaxLength="1" Text="Too many characters" ControlToValidate="txtZonaRobo" Display="Dynamic" runat="server" />
                                <asp:RequiredFieldValidator ID="reqZonaRoboS" Display="Dynamic" ControlToValidate="txtZonaRobo" Text="Required" runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Daylight savings">
                            <ItemTemplate>
                                <%# Eval("DaylightSavings") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDaylight" Text='<%# Bind("DaylightSavings") %>' runat="server" />
                                <asp:CompareValidator ID="reqDaylight" Display="Dynamic" ControlToValidate="txtDaylight" Text="Invalid value" Operator="DataTypeCheck" Type="Integer" runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ShowEditButton="true" ButtonType="Image" HeaderText="Edit" EditImageUrl="~/images/edit.gif" UpdateImageUrl="~/images/ok.gif" CancelImageUrl="~/images/cancel.gif" ShowCancelButton="true" />
                    </Columns>
                </asp:GridView>

                <asp:FormView ID="frmState" OnItemInserting="ItemInserting" OnItemInserted="frmState_ItemInserted" DataSourceID="odsGridSource" DefaultMode="Insert" runat="server">
                    <InsertItemTemplate>
                        <div class="row">
                            <asp:Label ID="lblName" AssociatedControlID="txtInsertName" Text="Name: " runat="server" />
                            <asp:TextBox ID="txtInsertName" MaxLength="32" EnableViewState="false" Text='<%# Bind("Name") %>' runat="server" />
                            <user:LengthValidator ID="lenName" Text="(too many characters)" MaxLength="32" Display="Dynamic" ControlToValidate="txtInsertName" runat="server" />
                            <asp:RequiredFieldValidator ID="valName" Display="Dynamic" ControlToValidate="txtInsertName" Text="(required)" runat="server" />
                        </div>
                        <div class="row">
                            <asp:Label ID="lblCountry" AssociatedControlID="ddlCountry" Text="Country: " runat="server" />
                            <asp:DropDownList ID="ddlCountry" AppendDataBoundItems="true" EnableViewState="false" DataSourceID="odsForeignSource" DataTextField="name" DataValueField="id" runat="server">
                                <asp:ListItem Selected="True" Text="---" Value="0" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="valCountry" Text="Required" Display="Dynamic" InitialValue="0" ControlToValidate="ddlCountry" runat="server" />
                        </div>
                        <div class="row">
                            <asp:Label ID="lblAbbreviation" AssociatedControlID="txtInsertAbb" Text="Abbreviation: " runat="server" />
                            <asp:TextBox ID="txtInsertAbb" MaxLength="4" EnableViewState="false" Text='<%# Bind("Abbreviation") %>' runat="server" />
                            <user:LengthValidator ID="lenAbb" Text="(too many characters)" MaxLength="4" Display="Dynamic" ControlToValidate="txtInsertAbb" runat="server" />
                        </div>
                        <div class="row">
                            <asp:Label ID="lblZ" AssociatedControlID="txtZ" Text="Zurich Code: " runat="server" />
                            <asp:TextBox ID="txtZ" MaxLength="32" EnableViewState="false" Text='<%# Bind("ZurichCode") %>' runat="server" />
                            <asp:RequiredFieldValidator ID="reqZ" Text="Required" Display="Dynamic" ControlToValidate="txtZ" runat="server" />
                        </div>
                        <div class="row">
                            <asp:Label ID="lblKey" AssociatedControlID="txtInsertKey" Text="Mapfre Code: " runat="server" />
                            <asp:TextBox ID="txtInsertKey" MaxLength="32" EnableViewState="false" Text='<%# Bind("MapfreCode") %>' runat="server" />
                            <asp:CompareValidator ID="valInsertKey" Text="Invalid value" Display="Dynamic" ControlToValidate="txtInsertKey" Operator="DataTypeCheck" Type="Integer" runat="server" />
                            <asp:RequiredFieldValidator ID="reqInsertKey" Text="Required" Display="Dynamic" ControlToValidate="txtInsertKey" runat="server" />
                        </div>
                        <div class="row">
                            <asp:Label ID="lblTheftZone" AssociatedControlID="txtTheftZone" Text="Theft Zone: " runat="server" />
                            <asp:TextBox ID="txtTheftZone" MaxLength="1" EnableViewState="false" Text='<%# Bind("TheftZone") %>' runat="server" />
                            <user:LengthValidator ID="valTheftZone" MaxLength="1" Text="Too many characters" Display="Dynamic" ControlToValidate="txtTheftZone" runat="server" />
                            <asp:RequiredFieldValidator ID="reqTheftZone" Text="Required" Display="Dynamic" ControlToValidate="txtTheftZone" runat="server" />
                        </div>
                        <div class="row">
                            <asp:Label ID="lblDaylight" AssociatedControlID="txtDaylight" Text="Daylight savings: " runat="server" />
                            <asp:TextBox ID="txtDaylight" EnableViewState="false" Text='<%# Bind("DaylightSavings") %>' runat="server" />
                            <asp:CompareValidator ID="cmpDaylight" Text="Invalid value" Display="Dynamic" ControlToValidate="txtDaylight" Operator="DataTypeCheck" Type="Integer" runat="server" />
                        </div>
                        <asp:LinkButton CssClass="linkButton" ID="lnkDoInsert" OnClick="lnkDoInsert_Click" Text="Insert" CommandName="Insert" runat="server" />
                        <asp:LinkButton CssClass="linkButton" ID="lnkCancel" CausesValidation="false" Text="Cancel" CommandName="Cancel" runat="server" />
                    </InsertItemTemplate>
                </asp:FormView>

                <br />
                <asp:LinkButton CssClass="linkButton" ID="lnkInsert" Text="Insert" OnClick="lnkInsert_Click" CausesValidation="false" runat="server" />
                <%--<asp:LinkButton CssClass="linkButton" ID="lnkDelete" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete these rows?')" OnClick="lnkDelete_Click" runat="server" />--%>
                <br />
                <asp:Label ID="lblError" ForeColor="Red" EnableViewState="false" runat="server" />
            </td>
        </tr>
    </table>

    <%--
If you wish to synchronize the state codes with Mapfre values press 'Synchronize'
<asp:Button ID="btnSync" Text="Synchronize" EnableViewState="false" OnClientClick="this.disabled = true;" UseSubmitBehavior="false" OnClick="btnSynchronize_Click" runat="server" />
<br />
    --%>
    <asp:ObjectDataSource ID="odsGridSource" ConflictDetection="CompareAllValues" DataObjectTypeName="StateInternal"
        TypeName="StateManager" SelectMethod="SelectByCountry" UpdateMethod="Update"
        InsertMethod="Insert" OldValuesParameterFormatString="oldValue" runat="server">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlSCountry" DefaultValue="0" Name="country" PropertyName="SelectedValue" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsForeignSource" TypeName="CountryManager" SelectMethod="Select" runat="server" />
</asp:Content>
