<%@ Page Language="C#" MasterPageFile="~/HouseMaster.master" AutoEventWireup="true" CodeFile="TheftProtectionSur.aspx.cs" Inherits="Catalog_TheftProtectionSur" Title="Catalog: Theft Protection Surcharge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="workloadContent" Runat="Server">
<asp:UpdatePanel ID="updContent" runat="server">
<ContentTemplate>
<h1 class="pageHeader">Theft Protection Surcharge</h1>
<asp:DropDownList ID="ddlCompanies" runat="server" DataValueField="company" DataTextField="companyDescription" AutoPostBack="true" />

<br />
<asp:GridView ID="grdTheftProtection" FooterStyle-CssClass="catalogTableFooter" ShowFooter="true" OnRowCreated="RowCreated"
             GridLines="None" CellSpacing="1" AlternatingRowStyle-CssClass="catalogTableAlternate" CssClass="catalogTable"
             EditRowStyle-CssClass="catalogTableEdit" HeaderStyle-CssClass="catalogTableHeader" 
             EmptyDataText="This catalog is empty." DataKeyNames="company,theftProtection" DataSourceID="odsGridSource" AutoGenerateColumns="false" runat="server">
    <Columns>
    <asp:TemplateField HeaderText="Description">
        <ItemTemplate>
            <%# Eval("description") %>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:CheckBoxField DataField="code" HeaderText="Code" />
    <asp:TemplateField HeaderText="DIS" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("DIS") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtSurcharge" Text='<%# Bind("DIS") %>'  runat="server" />
            <asp:CompareValidator ID="cmpSurcharge" Text="(Invalid value)" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtSurcharge" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valSurcharge" Text="(Required)" Display="Dynamic" ControlToValidate="txtSurcharge" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:CommandField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ShowEditButton="true" ButtonType="Image" HeaderText="Edit" EditImageUrl="~/images/edit.gif"  UpdateImageUrl="~/images/ok.gif" CancelImageUrl="~/images/cancel.gif" ShowCancelButton="true" />
    </Columns>
</asp:GridView>

<asp:ObjectDataSource ID="odsGridSource" TypeName="TheftProtectionSurcharge" SelectMethod="Select" UpdateMethod="Update" runat="server">
    <SelectParameters>
        <asp:ControlParameter Type="Int32" Name="company" ControlID="ddlCompanies" />
    </SelectParameters>
    <UpdateParameters>
        <asp:Parameter Name="company" Type="Int32" />
        <asp:Parameter Name="theftProtection" Type="Int32" />
        <asp:Parameter Name="code" Type="Boolean" />
        <asp:Parameter Name="DIS" Type="Decimal" />
    </UpdateParameters>
</asp:ObjectDataSource>

</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

