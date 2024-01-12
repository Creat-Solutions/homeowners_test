<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FHM.aspx.cs" Inherits="Catalog_FHM" MasterPageFile="~/HouseMaster.master" Title="Catalog: HMP" %>

<asp:Content ID="cntHolder" ContentPlaceHolderID="workloadContent" runat="server">
<asp:UpdatePanel ID="updContent" runat="server">
<ContentTemplate>
<h1 class="pageHeader">HMP</h1>
<asp:GridView ID="grdFHM" FooterStyle-CssClass="catalogTableFooter" ShowFooter="true" OnRowCreated="RowCreated"
             GridLines="None" CellSpacing="1" AlternatingRowStyle-CssClass="catalogTableAlternate" CssClass="catalogTable"
             EditRowStyle-CssClass="catalogTableEdit" HeaderStyle-CssClass="catalogTableHeader" 
             OnDataBound="grdFHM_DataBound" EmptyDataText="This catalog is empty." DataKeyNames="id" DataSourceID="odsGridSource" AutoGenerateColumns="false" runat="server">
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
            <asp:TextBox ID="txtEditDescription" MaxLength="50" Text='<%# Bind("description") %>' runat="server" />
            <user:LengthValidator ID="lenDescription" Text="(too many characters)" MaxLength="50" Display="Dynamic" ControlToValidate="txtEditDescription" runat="server" />
            <asp:RequiredFieldValidator ID="valDescription" Display="Dynamic" ControlToValidate="txtEditDescription" Text="(required)" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:CommandField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ShowEditButton="true" ButtonType="Image" HeaderText="Edit" EditImageUrl="~/images/edit.gif"  UpdateImageUrl="~/images/ok.gif" CancelImageUrl="~/images/cancel.gif" ShowCancelButton="true" />
</Columns>
</asp:GridView>
<br/><asp:LinkButton CssClass="linkButton" ID="lnkInsert" Text="Insert" OnClick="lnkInsert_Click" runat="server" />
<asp:LinkButton CssClass="linkButton" ID="lnkDelete" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete these rows?')" OnClick="lnkDelete_Click" runat="server" />

<asp:FormView ID="frmFHM" OnItemInserted="frmFHM_ItemInserted" DataSourceID="odsGridSource" DefaultMode="Insert" runat="server">
        
            <InsertItemTemplate>
                <asp:Label ID="lblDescription" AssociatedControlID="txtInsertDescription" Text="Description: " runat="server" />
                <asp:TextBox ID="txtInsertDescription" MaxLength="50" EnableViewState="false" Text='<%# Bind("description") %>' runat="server" />
                <user:LengthValidator ID="lenDescription" Text="(too many characters)" MaxLength="50" Display="Dynamic" ControlToValidate="txtInsertDescription" runat="server" />
                <asp:RequiredFieldValidator ID="valDescription" Display="Dynamic" ControlToValidate="txtInsertDescription" Text="(required)" runat="server" />
                <br />
                <asp:LinkButton CssClass="linkButton" ID="lnkDoInsert" OnClick="lnkDoInsert_Click" Text="Insert" CommandName="Insert" runat="server" />
                <asp:LinkButton CssClass="linkButton" ID="lnkCancel" CausesValidation="false" Text="Cancel" CommandName="Cancel" runat="server" />
            </InsertItemTemplate>
</asp:FormView>
<asp:Label ID="lblError" EnableViewState="false" Visible="false" ForeColor="Red" runat="server" />
<asp:ObjectDataSource ID="odsGridSource" ConflictDetection="CompareAllValues" DataObjectTypeName="FHM" TypeName="FHM" SelectMethod="SelectCached" UpdateMethod="Update" InsertMethod="Insert" DeleteMethod="Delete" OldValuesParameterFormatString="oldValue" runat="server" />

</ContentTemplate>
</asp:UpdatePanel>

</asp:Content>
