<%@ Page Language="C#" AutoEventWireup="true" CodeFile="City.aspx.cs" Inherits="Catalog_City" MasterPageFile="~/HouseMaster.master" Title="Catalog: City" %>

<asp:Content ID="cntHolder" ContentPlaceHolderID="workloadContent" runat="server">
    <asp:UpdatePanel ID="updContent" runat="server">
        <ContentTemplate>
            <h1 class="pageHeader">City</h1>

            <table cellspacing="0" cellpadding="5" style="margin-top: 10px" width="95%" border="1" align="center">
                <tr>
                    <td class="Claro" colspan="4">
                        <asp:Label ID="lblSearch" AssociatedControlID="txtSearch" Text="Search: " runat="server" />
                        <br />
                        <asp:Literal ID="lblSState" Text="State: " runat="server" />
                        <asp:DropDownList ID="ddlStateSearch" AppendDataBoundItems="true" runat="server" DataSourceID="odsForeignSource" DataTextField="Name" DataValueField="ID">
                            <asp:ListItem Text="---" Value="0" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ErrorMessage="Required" ControlToValidate="ddlStateSearch"
                            InitialValue="0" runat="server" ForeColor="Red" />
                        <asp:Literal ID="lblSName" Text="Name: " runat="server" />
                        <asp:TextBox ID="txtSearch" runat="server" />
                        <asp:Button Style="padding: 1px 3px;" ID="btnSearch" OnClick="search_Click" Text="Search" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="claro" colspan="4">
                        <asp:GridView ID="grdCity" PagerStyle-CssClass="catalogTableFooter" Width="95%"
                            GridLines="None" CellSpacing="1" AlternatingRowStyle-CssClass="catalogTableAlternate" CssClass="catalogTable"
                            EditRowStyle-CssClass="catalogTableEdit" HeaderStyle-CssClass="catalogTableHeader" AllowPaging="true" PageSize="30"
                            OnDataBound="grdCity_DataBound" EmptyDataText="This catalog is empty." DataKeyNames="ID" DataSourceID="odsGridSource" AutoGenerateColumns="false" runat="server">
                            <Columns>
                                <asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <%# Eval("Name") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEditName" MaxLength="30" Text='<%# Bind("Name") %>' runat="server" />
                                        <user:LengthValidator ID="lenName" Text="(too many characters)" MaxLength="30" Display="Dynamic" ControlToValidate="txtEditName" runat="server" />
                                        <asp:RequiredFieldValidator ID="valName" Display="Dynamic" ControlToValidate="txtEditName" Text="Required" runat="server" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="IDState" Visible="false">
                                    <ItemTemplate>
                                        <%# Eval("IDState") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="State">
                                    <ItemTemplate>
                                        <%# Eval("NameState") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<user:ForeignField ForeignKeyField="ID" ForeignTextField="State.Name" DataField="State" DataSourceID="odsForeignSource" HeaderText="State" />--%>
                                <asp:TemplateField HeaderText="Key">
                                    <ItemTemplate>
                                        <%# Eval("Code") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtClave" MaxLength="8" Text='<%# Bind("Code") %>' runat="server" />
                                        <asp:RequiredFieldValidator ID="valClave" Display="Dynamic" ControlToValidate="txtClave" Text="Required" runat="server" />
                                        <user:LengthValidator ID="lenClave" Text="Too many characters" MaxLength="8" Display="Dynamic" ControlToValidate="txtClave" runat="server" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <user:ForeignField ForeignKeyField="id" ForeignTextField="description" DataField="HMP" DataSourceID="odsForeignFHM" HeaderText="HMP Zone" />
                                <asp:TemplateField HeaderText="Quake Deductible">
                                    <ItemTemplate>
                                        <%# Eval("DeductibleQuake")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDedTerr" Text='<%# Bind("DeductibleQuake") %>' runat="server" />
                                        <asp:CompareValidator ID="cmpDedTerr" Display="Dynamic" ControlToValidate="txtDedTerr" Text="Invalid value" Operator="DataTypeCheck" Type="Integer" runat="server" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quake Coinsurance">
                                    <ItemTemplate>
                                        <%# Eval("CoinsuranceQuake")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtCoaTerr" Text='<%# Bind("CoinsuranceQuake") %>' runat="server" />
                                        <asp:CompareValidator ID="cmpCoaTerr" Display="Dynamic" ControlToValidate="txtCoaTerr" Text="Invalid value" Operator="DataTypeCheck" Type="Integer" runat="server" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="HMP Deductible">
                                    <ItemTemplate>
                                        <%# Eval("DeductibleHMP")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDedFHM" Text='<%# Bind("DeductibleHMP") %>' runat="server" />
                                        <asp:CompareValidator ID="cmpDedFHM" Display="Dynamic" ControlToValidate="txtDedFHM" Text="Invalid value" Operator="DataTypeCheck" Type="Integer" runat="server" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="HMP Coinsurance">
                                    <ItemTemplate>
                                        <%# Eval("CoinsuranceHMP")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtCoaFHM" Text='<%# Bind("CoinsuranceHMP") %>' runat="server" />
                                        <asp:CompareValidator ID="cmpCoaFHM" Display="Dynamic" ControlToValidate="txtCoaFHM" Text="Invalid value" Operator="DataTypeCheck" Type="Integer" runat="server" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax Percentage">
                                    <ItemTemplate>
                                        <%# Eval("TaxPercentage")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtTax" Text='<%# Bind("TaxPercentage") %>' runat="server" />
                                        <asp:CompareValidator ID="cmpTax" Display="Dynamic" ControlToValidate="txtTax" Text="Invalid value" Operator="DataTypeCheck" Type="Double" runat="server" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ShowEditButton="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ButtonType="Image" HeaderText="Edit" EditImageUrl="~/images/edit.gif" UpdateImageUrl="~/images/ok.gif" CancelImageUrl="~/images/cancel.gif" ShowCancelButton="true" />
                            </Columns>
                        </asp:GridView>

                        <asp:FormView ID="frmCity" OnItemInserting="ItemInserting" OnItemInserted="frmCity_ItemInserted" DataSourceID="odsGridSource" DefaultMode="Insert" runat="server">
                            <InsertItemTemplate>
                                <div class="row">
                                    <asp:Label ID="lblName" AssociatedControlID="txtInsertName" Text="Name: " runat="server" />
                                    <asp:TextBox ID="txtInsertName" MaxLength="50" EnableViewState="false" Text='<%# Bind("Name") %>' runat="server" />
                                    <user:LengthValidator ID="lenName" Text="Too many characters" MaxLength="50" Display="Dynamic" ControlToValidate="txtInsertName" runat="server" />
                                    <asp:RequiredFieldValidator ID="valName" Display="Dynamic" ControlToValidate="txtInsertName" Text="Required" runat="server" />
                                </div>
                                <div class="row">
                                    <asp:Label ID="lblState" AssociatedControlID="ddlState" Text="State: " runat="server" />
                                    <asp:DropDownList ID="ddlState" AppendDataBoundItems="true" EnableViewState="false" DataSourceID="odsForeignSource" DataTextField="Name" DataValueField="ID" runat="server">
                                        <asp:ListItem Selected="True" Text="---" Value="0" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="valState" Text="Required" Display="Dynamic" InitialValue="0" ControlToValidate="ddlState" runat="server" />
                                </div>
                                <div class="row">
                                    <asp:Label ID="lblClave" AssociatedControlID="txtInsertClave" Text="Key: " runat="server" />
                                    <asp:TextBox ID="txtInsertClave" MaxLength="8" EnableViewState="false" Text='<%# Bind("Code") %>' runat="server" />
                                    <user:LengthValidator ID="lenClave" Text="Too many characters" MaxLength="8" Display="Dynamic" ControlToValidate="txtInsertClave" runat="server" />
                                    <asp:RequiredFieldValidator ID="reqClave" Display="Dynamic" ControlToValidate="txtInsertClave" Text="Required" runat="server" />
                                </div>
                                <div class="row">
                                    <asp:Label ID="lblZonaFHM" AssociatedControlID="ddlInsertZonaFHM" Text="HMP Zone: " runat="server" />
                                    <asp:DropDownList ID="ddlInsertZonaFHM" AppendDataBoundItems="true" EnableViewState="false" DataSourceID="odsForeignFHM" DataTextField="description" DataValueField="id" runat="server">
                                        <asp:ListItem Selected="True" Text="---" Value="0" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="reqInsertZonaFHM" Text="Required" Display="Dynamic" InitialValue="0" ControlToValidate="ddlInsertZonaFHM" runat="server" />
                                </div>
                                <div class="row">
                                    <asp:Label ID="lblQuakeD" Text="Quake Deductible: " AssociatedControlID="txtQuakeD" runat="server" />
                                    <asp:TextBox ID="txtQuakeD" Text='<%# Bind("DeductibleQuake") %>' runat="server" />
                                    <asp:CompareValidator ID="cmpQuakeD" ControlToValidate="txtQuakeD" Display="Dynamic" Operator="DataTypeCheck" Type="Integer" Text="Invalid value" runat="server" />
                                </div>
                                <div class="row">
                                    <asp:Label ID="lblQuakeC" Text="Quake Coinsurance: " AssociatedControlID="txtQuakeC" runat="server" />
                                    <asp:TextBox ID="txtQuakeC" Text='<%# Bind("CoinsuranceQuake") %>' runat="server" />
                                    <asp:CompareValidator ID="cmpQuakeC" ControlToValidate="txtQuakeC" Display="Dynamic" Operator="DataTypeCheck" Type="Integer" Text="Invalid value" runat="server" />
                                </div>
                                <div class="row">
                                    <asp:Label ID="lblHMPD" Text="HMP Deductible: " AssociatedControlID="txtHMPD" runat="server" />
                                    <asp:TextBox ID="txtHMPD" Text='<%# Bind("DeductibleHMP") %>' runat="server" />
                                    <asp:CompareValidator ID="cmpHMPD" ControlToValidate="txtHMPD" Display="Dynamic" Operator="DataTypeCheck" Type="Integer" Text="Invalid value" runat="server" />
                                </div>
                                <div class="row">
                                    <asp:Label ID="lblHMPC" Text="HMP Coinsurance: " AssociatedControlID="txtHMPC" runat="server" />
                                    <asp:TextBox ID="txtHMPC" Text='<%# Bind("CoinsuranceHMP") %>' runat="server" />
                                    <asp:CompareValidator ID="cmpHMPC" ControlToValidate="txtHMPC" Display="Dynamic" Operator="DataTypeCheck" Type="Integer" Text="Invalid value" runat="server" />
                                </div>
                                <div class="row">
                                    <asp:Label ID="lblTaxC" Text="Tax Percentage: " AssociatedControlID="txtTaxC" runat="server" />
                                    <asp:TextBox ID="txtTaxC" Text='<%# Bind("TaxPercentage") %>' runat="server" />
                                    <asp:CompareValidator ID="cmpTaxC" ControlToValidate="txtTaxC" Display="Dynamic" Operator="DataTypeCheck" Type="Double" Text="Invalid value" runat="server" />
                                </div>
                                <asp:LinkButton CssClass="linkButton" ID="lnkDoInsert" OnClick="lnkDoInsert_Click" Text="Insert" CommandName="Insert" runat="server" />
                                <asp:LinkButton CssClass="linkButton" ID="lnkCancel" CausesValidation="false" Text="Cancel" CommandName="Cancel" runat="server" />
                            </InsertItemTemplate>
                        </asp:FormView>

                        <br />
                        <asp:LinkButton CssClass="linkButton" ID="lnkInsert" Text="Insert" OnClick="lnkInsert_Click" CausesValidation="false" runat="server" />
                        <br />
                        <asp:Label ID="lblError" ForeColor="Red" EnableViewState="false" runat="server" />
                    </td>
                </tr>
            </table>

            <asp:ObjectDataSource ID="odsGridSource" ConflictDetection="CompareAllValues" DataObjectTypeName="CityInternal" EnablePaging="true"
                TypeName="CityManager" SelectMethod="GetRange" SelectCountMethod="GetTotal" UpdateMethod="Update"
                InsertMethod="Insert" OldValuesParameterFormatString="oldValue" runat="server">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtSearch" DefaultValue="" ConvertEmptyStringToNull="true" Name="search" Type="String" />
                    <asp:ControlParameter DefaultValue="0" Name="state" ControlID="ddlStateSearch" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:ObjectDataSource ID="odsForeignSource" TypeName="StateManager" SelectMethod="Select" runat="server" />
            <asp:ObjectDataSource ID="odsForeignFHM" TypeName="FHM" SelectMethod="SelectCached" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
