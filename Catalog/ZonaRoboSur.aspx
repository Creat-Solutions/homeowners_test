<%@ Page Language="C#" MasterPageFile="~/HouseMaster.master" AutoEventWireup="true" CodeFile="ZonaRoboSur.aspx.cs" Inherits="Catalog_ZonaRoboSur" Title="Catalog: Theft Zone Surcharge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="workloadContent" Runat="Server">
<asp:UpdatePanel ID="updContent" runat="server">
<ContentTemplate>
<h1 class="pageHeader">Theft Zone Surcharge</h1>
<asp:DropDownList ID="ddlCompanies" runat="server" DataValueField="company" DataTextField="companyDescription" AutoPostBack="true" />
<br />
<asp:GridView ID="grdZonaRobo" FooterStyle-CssClass="catalogTableFooter" ShowFooter="true" OnRowCreated="RowCreated"
             GridLines="None" CellSpacing="1" AlternatingRowStyle-CssClass="catalogTableAlternate" CssClass="catalogTable"
             EditRowStyle-CssClass="catalogTableEdit" HeaderStyle-CssClass="catalogTableHeader" 
             EmptyDataText="This catalog is empty." DataKeyNames="company,zonaRobo" DataSourceID="odsGridSource" AutoGenerateColumns="false" runat="server">
    <Columns>
    <asp:TemplateField HeaderText="Description">
        <ItemTemplate>
            <%# Eval("zonaRobo") %>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Household goods" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("menaje") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtMenaje" Text='<%# Bind("menaje") %>'  runat="server" />
            <asp:CompareValidator ID="cmpMenaje" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtMenaje" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valMenaje" Text="Required" Display="Dynamic" ControlToValidate="txtMenaje" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Jewels" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("joyas") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtJoyas" Text='<%# Bind("joyas") %>'  runat="server" />
            <asp:CompareValidator ID="cmpJoyas" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtJoyas" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valJoyas" Text="Required" Display="Dynamic" ControlToValidate="txtJoyas" runat="server" />
        </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Other" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("otro") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtOtro" Text='<%# Bind("otro") %>'  runat="server" />
            <asp:CompareValidator ID="cmpOtro" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtOtro" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valOtro" Text="Required" Display="Dynamic" ControlToValidate="txtOtro" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:CommandField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ShowEditButton="true" ButtonType="Image" HeaderText="Edit" EditImageUrl="~/images/edit.gif"  UpdateImageUrl="~/images/ok.gif" CancelImageUrl="~/images/cancel.gif" ShowCancelButton="true" />
    </Columns>
</asp:GridView>

<asp:ObjectDataSource ID="odsGridSource" TypeName="ZonaRoboSurcharge" SelectMethod="Select" UpdateMethod="Update" runat="server">
    <SelectParameters>
        <asp:ControlParameter Type="Int32" Name="company" ControlID="ddlCompanies" />
    </SelectParameters>
    <UpdateParameters>
        <asp:Parameter Name="company" Type="Int32" />
        <asp:Parameter Name="zonaRobo" Type="Char" />
        <asp:Parameter Name="menaje" Type="Decimal" />
        <asp:Parameter Name="joyas" Type="Decimal" />
        <asp:Parameter Name="otro" Type="Decimal" />
    </UpdateParameters>
</asp:ObjectDataSource>

</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

