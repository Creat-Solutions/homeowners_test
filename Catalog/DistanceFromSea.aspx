<%@ Page Language="C#" MasterPageFile="~/HouseMaster.master" AutoEventWireup="true" CodeFile="DistanceFromSea.aspx.cs" Inherits="Catalog_DistanceFromSea" Title="Catalog: Distance From Sea" %>

<asp:Content ID="Content1" ContentPlaceHolderID="workloadContent" Runat="Server">
<asp:UpdatePanel ID="updContent" runat="server">
<ContentTemplate>
<h1 class="pageHeader">Distance From Sea</h1>
<asp:GridView ID="grdDistance" FooterStyle-CssClass="catalogTableFooter" ShowFooter="true" OnRowCreated="RowCreated"
             GridLines="None" CellSpacing="1" AlternatingRowStyle-CssClass="catalogTableAlternate" CssClass="catalogTable"
             EditRowStyle-CssClass="catalogTableEdit" HeaderStyle-CssClass="catalogTableHeader" 
             OnDataBound="grdDistance_DataBound" EmptyDataText="This catalog is empty." DataKeyNames="distance" DataSourceID="odsGridSource" AutoGenerateColumns="false" runat="server">
    <Columns>
    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
        <HeaderTemplate>
            <asp:CheckBox ID="checkAllBox" EnableViewState="false" runat="server" />
        </HeaderTemplate>
        <ItemTemplate>
            <asp:CheckBox ID="deleteBox" EnableViewState="false" runat="server" />
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Description">
        <ItemTemplate>
            <%# Eval("description") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtEditDescription" MaxLength="100" Columns="40" Text='<%# Bind("description") %>' runat="server" />
            <user:LengthValidator ID="lenDescription" Text="Too many characters" MaxLength="100" Display="Dynamic" ControlToValidate="txtEditDescription" runat="server" />
            <asp:RequiredFieldValidator ID="valDescription" Display="Dynamic" ControlToValidate="txtEditDescription" Text="Required" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Descripci&oacute;n">
        <ItemTemplate>
            <%# Eval("descriptionEs") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtEditDescripcion" MaxLength="100" Columns="40" Text='<%# Bind("descriptionEs") %>' runat="server" />
            <user:LengthValidator ID="lenDescripcion" Text="Too many characters" MaxLength="100" Display="Dynamic" ControlToValidate="txtEditDescripcion" runat="server" />
            <asp:RequiredFieldValidator ID="valDescripcion" Display="Dynamic" ControlToValidate="txtEditDescripcion" Text="Required" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:CommandField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ShowEditButton="true" ButtonType="Image" HeaderText="Edit" EditImageUrl="~/images/edit.gif"  UpdateImageUrl="~/images/ok.gif" CancelImageUrl="~/images/cancel.gif" ShowCancelButton="true" />
    </Columns>
</asp:GridView>
<br/><asp:LinkButton CssClass="linkButton" ID="lnkInsert" Text="Insert" OnClick="lnkInsert_Click" runat="server" />
<asp:LinkButton CssClass="linkButton" ID="lnkDelete" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete these rows?')" OnClick="lnkDelete_Click" runat="server" />

<asp:FormView ID="frmDistance" OnItemInserted="frmDistance_ItemInserted" DataSourceID="odsGridSource" DefaultMode="Insert" runat="server">
        
            <InsertItemTemplate>
                <div class="row">
                    <asp:Label ID="lblDescription" AssociatedControlID="txtInsertDescription" Text="Description: " runat="server" />
                    <asp:TextBox ID="txtInsertDescription" MaxLength="100" EnableViewState="false" Text='<%# Bind("description") %>' runat="server" />
                    <user:LengthValidator ID="lenDescription" Text="Too many characters" MaxLength="100" Display="Dynamic" ControlToValidate="txtInsertDescription" runat="server" />
                    <asp:RequiredFieldValidator ID="valDescription" Display="Dynamic" ControlToValidate="txtInsertDescription" Text="Required" runat="server" />
                </div>
                <div class="row">
                    <asp:Label ID="lblDescripcion" AssociatedControlID="txtInsertDescripcion" Text="Descripci&oacute;n: " runat="server" />
                    <asp:TextBox ID="txtInsertDescripcion" MaxLength="100" EnableViewState="false" Text='<%# Bind("descriptionEs") %>' runat="server" />
                    <user:LengthValidator ID="lenDescripcion" Text="Too many characters" MaxLength="100" Display="Dynamic" ControlToValidate="txtInsertDescripcion" runat="server" />
                    <asp:RequiredFieldValidator ID="valDescripcion" Display="Dynamic" ControlToValidate="txtInsertDescripcion" Text="Required" runat="server" />
                </div>
                <asp:LinkButton CssClass="linkButton" ID="lnkDoInsert" OnClick="lnkDoInsert_Click" Text="Insert" CommandName="Insert" runat="server" />
                <asp:LinkButton CssClass="linkButton" ID="lnkCancel" CausesValidation="false" Text="Cancel" CommandName="Cancel" runat="server" />
            </InsertItemTemplate>
</asp:FormView>
<asp:Label ID="lblError" EnableViewState="false" Visible="false" ForeColor="Red" runat="server" />
<asp:ObjectDataSource ID="odsGridSource" ConflictDetection="CompareAllValues" DataObjectTypeName="distanceFromSea" TypeName="distanceFromSea" SelectMethod="SelectCached" UpdateMethod="Update" InsertMethod="Insert" DeleteMethod="Delete" OldValuesParameterFormatString="oldValue" runat="server" />

</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

