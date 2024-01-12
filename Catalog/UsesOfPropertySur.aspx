<%@ Page Language="C#" MasterPageFile="~/HouseMaster.master" AutoEventWireup="true" CodeFile="UsesOfPropertySur.aspx.cs" Inherits="Catalog_UsesOfPropertySur" Title="Catalog: Use of Property Surcharge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="workloadContent" Runat="Server">
<asp:UpdatePanel ID="updContent" runat="server">
<ContentTemplate>
<h1 class="pageHeader">Use Of Property Surcharge</h1>
<asp:DropDownList ID="ddlCompanies" runat="server" DataValueField="company" DataTextField="companyDescription" AutoPostBack="true" />
<br />
<asp:GridView ID="grdUsesOfProperty" FooterStyle-CssClass="catalogTableFooter" ShowFooter="true" OnRowCreated="RowCreated"
             GridLines="None" CellSpacing="1" AlternatingRowStyle-CssClass="catalogTableAlternate" CssClass="catalogTable"
             EditRowStyle-CssClass="catalogTableEdit" HeaderStyle-CssClass="catalogTableHeader" 
             EmptyDataText="This catalog is empty." DataKeyNames="company,useOfProperty" DataSourceID="odsGridSource" AutoGenerateColumns="false" runat="server">
    <Columns>
    <asp:TemplateField HeaderText="Description">
        <ItemTemplate>
            <%# Eval("description") %>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Surcharge" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("surcharge") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtSurcharge" Text='<%# Bind("surcharge") %>'  runat="server" />
            <asp:CompareValidator ID="cmpSurcharge" Text="(Invalid value)" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtSurcharge" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valSurcharge" Text="(Required)" Display="Dynamic" ControlToValidate="txtSurcharge" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:CommandField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ShowEditButton="true" ButtonType="Image" HeaderText="Edit" EditImageUrl="~/images/edit.gif"  UpdateImageUrl="~/images/ok.gif" CancelImageUrl="~/images/cancel.gif" ShowCancelButton="true" />
    </Columns>
</asp:GridView>

<asp:ObjectDataSource ID="odsGridSource" TypeName="UseOfPropertySurcharge" SelectMethod="Select" UpdateMethod="Update" runat="server">
    <SelectParameters>
        <asp:ControlParameter Type="Int32" Name="company" ControlID="ddlCompanies" />
    </SelectParameters>
    <UpdateParameters>
        <asp:Parameter Name="company" Type="Int32" />
        <asp:Parameter Name="useOfProperty" Type="Int32" />
        <asp:Parameter Name="surcharge" Type="Decimal" />
    </UpdateParameters>
</asp:ObjectDataSource>

</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

