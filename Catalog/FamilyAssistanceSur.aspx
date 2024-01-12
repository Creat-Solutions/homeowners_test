<%@ Page Language="C#" MasterPageFile="~/HouseMaster.master" AutoEventWireup="true" CodeFile="FamilyAssistanceSur.aspx.cs" Inherits="Catalog_FamilyAssistanceSur" Title="Catalog: Family Assistance Surcharge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="workloadContent" Runat="Server">
<asp:UpdatePanel ID="updContent" runat="server">
<ContentTemplate>
<h1 class="pageHeader">Family Assistance Surcharge</h1>
<asp:DropDownList ID="ddlCompanies" runat="server" DataValueField="company" DataTextField="companyDescription" AutoPostBack="true" />
<br />
<asp:GridView ID="grdFamilyAssistance" FooterStyle-CssClass="catalogTableFooter" ShowFooter="true" OnRowCreated="RowCreated"
             GridLines="None" CellSpacing="1" AlternatingRowStyle-CssClass="catalogTableAlternate" CssClass="catalogTable"
             EditRowStyle-CssClass="catalogTableEdit" HeaderStyle-CssClass="catalogTableHeader" 
             EmptyDataText="This catalog is empty." DataKeyNames="company,familyAssistance" DataSourceID="odsGridSource" AutoGenerateColumns="false" runat="server">
    <Columns>
    <asp:TemplateField HeaderText="Description">
        <ItemTemplate>
            <%# Eval("description") %>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Code">
        <ItemTemplate>
            <%# Eval("code") %>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Value 1" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("val1") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtVal1" Text='<%# Bind("val1") %>'  runat="server" />
            <asp:CompareValidator ID="cmpVal1" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtVal1" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valVal1" Text="Required" Display="Dynamic" ControlToValidate="txtVal1" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Value 2" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("val2") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtVal2" Text='<%# Bind("val2") %>'  runat="server" />
            <asp:CompareValidator ID="cmpVal2" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtVal2" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valVal2" Text="Required" Display="Dynamic" ControlToValidate="txtVal2" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    
    <asp:CommandField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ShowEditButton="true" ButtonType="Image" HeaderText="Edit" EditImageUrl="~/images/edit.gif"  UpdateImageUrl="~/images/ok.gif" CancelImageUrl="~/images/cancel.gif" ShowCancelButton="true" />
    </Columns>
</asp:GridView>

<asp:ObjectDataSource ID="odsGridSource" TypeName="FamilyAssistanceSurcharge" SelectMethod="Select" UpdateMethod="Update" runat="server">
    <SelectParameters>
        <asp:ControlParameter Type="Int32" Name="company" ControlID="ddlCompanies" />
    </SelectParameters>
    <UpdateParameters>
        <asp:Parameter Name="company" Type="Int32" />
        <asp:Parameter Name="familyAssistance" Type="Int32" />
        <asp:Parameter Name="val1" Type="Decimal" />
        <asp:Parameter Name="val2" Type="Decimal" />
    </UpdateParameters>
</asp:ObjectDataSource>

</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

