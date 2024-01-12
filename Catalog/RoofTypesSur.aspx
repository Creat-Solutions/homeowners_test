<%@ Page Language="C#" MasterPageFile="~/HouseMaster.master" AutoEventWireup="true" CodeFile="RoofTypesSur.aspx.cs" Inherits="Catalog_RoofTypesSur" Title="Catalog: Roof Type Surcharge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="workloadContent" Runat="Server">
<asp:UpdatePanel ID="updContent" runat="server">
<ContentTemplate>
<h1 class="pageHeader">Roof Type Surcharge</h1>
<asp:DropDownList ID="ddlCompanies" runat="server" DataValueField="company" DataTextField="companyDescription" AutoPostBack="true" />
<br />
<asp:GridView ID="grdRoofTypes" FooterStyle-CssClass="catalogTableFooter" ShowFooter="true" OnRowCreated="RowCreated"
             GridLines="None" CellSpacing="1" AlternatingRowStyle-CssClass="catalogTableAlternate" CssClass="catalogTable"
             EditRowStyle-CssClass="catalogTableEdit" HeaderStyle-CssClass="catalogTableHeader" 
             EmptyDataText="This catalog is empty." DataKeyNames="company,roofType" DataSourceID="odsGridSource" AutoGenerateColumns="false" runat="server">
    <Columns>
    <asp:TemplateField HeaderText="Description">
        <ItemTemplate>
            <%# Eval("description") %>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Earthquake" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("TR") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtTR" Text='<%# Bind("TR") %>'  runat="server" />
            <asp:CompareValidator ID="cmpTR" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtTR" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valTR" Text="Required" Display="Dynamic" ControlToValidate="txtTR" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="HMP" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("FHM") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtFHM" Text='<%# Bind("FHM") %>'  runat="server" />
            <asp:CompareValidator ID="cmpFHM" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtFHM" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valFHM" Text="Required" Display="Dynamic" ControlToValidate="txtFHM" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Theft" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("Robo") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtRobo" Text='<%# Bind("Robo") %>'  runat="server" />
            <asp:CompareValidator ID="cmpRobo" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtRobo" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valRobo" Text="Required" Display="Dynamic" ControlToValidate="txtRobo" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:CommandField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ShowEditButton="true" ButtonType="Image" HeaderText="Edit" EditImageUrl="~/images/edit.gif"  UpdateImageUrl="~/images/ok.gif" CancelImageUrl="~/images/cancel.gif" ShowCancelButton="true" />
    
    </Columns>
</asp:GridView>

<asp:ObjectDataSource ID="odsGridSource" TypeName="RoofTypeSurcharge" SelectMethod="Select" UpdateMethod="Update" runat="server">
    <SelectParameters>
        <asp:ControlParameter Type="Int32" Name="company" ControlID="ddlCompanies" />
    </SelectParameters>
    <UpdateParameters>
        <asp:Parameter Name="company" Type="Int32" />
        <asp:Parameter Name="roofType" Type="Int32" />
        <asp:Parameter Name="TR" Type="Decimal" />
        <asp:Parameter Name="FHM" Type="Decimal" />
        <asp:Parameter Name="Robo" Type="Decimal" />
    </UpdateParameters>
</asp:ObjectDataSource>

</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

