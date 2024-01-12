<%@ Page Language="C#" MasterPageFile="~/HouseMaster.master" AutoEventWireup="true" CodeFile="TypesOfRisksSur.aspx.cs" Inherits="Catalog_TypesOfRisksSur" Title="Catalog: Type of Risk Surcharge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="workloadContent" Runat="Server">
<asp:UpdatePanel ID="updContent" runat="server">
<ContentTemplate>
<h1 class="pageHeader">Type of Risk Surcharge</h1>
<asp:DropDownList ID="ddlCompanies" runat="server" DataValueField="company" DataTextField="companyDescription" AutoPostBack="true" />
<br />
<asp:GridView ID="grdRisks" FooterStyle-CssClass="catalogTableFooter" ShowFooter="true" OnRowCreated="RowCreated"
             GridLines="None" CellSpacing="1" AlternatingRowStyle-CssClass="catalogTableAlternate" CssClass="catalogTable"
             EditRowStyle-CssClass="catalogTableEdit" HeaderStyle-CssClass="catalogTableHeader" 
             EmptyDataText="This catalog is empty." DataKeyNames="company,typeOfRisk" DataSourceID="odsGridSource" AutoGenerateColumns="false" runat="server">
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
            <asp:CompareValidator ID="cmpSurcharge" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtSurcharge" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valSurcharge" Text="Required" Display="Dynamic" ControlToValidate="txtSurcharge" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Additional" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("Additional") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtAdditional" Text='<%# Bind("Additional") %>'  runat="server" />
            <asp:CompareValidator ID="cmpAdditional" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtAdditional" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valAdditional" Text="Required" Display="Dynamic" ControlToValidate="txtAdditional" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:CheckBoxField HeaderText="Coverage" DataField="Coverage" />
    <asp:TemplateField HeaderText="Minimum" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("Minimum") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtMinimum" Text='<%# Bind("Minimum") %>'  runat="server" />
            <asp:CompareValidator ID="cmpMinimum" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtMinimum" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valMinimum" Text="Required" Display="Dynamic" ControlToValidate="txtMinimum" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Maximum" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("Maximum") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtMaximum" Text='<%# Bind("Maximum") %>'  runat="server" />
            <asp:CompareValidator ID="cmpMaximum" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtMaximum" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valMaximum" Text="Required" Display="Dynamic" ControlToValidate="txtMaximum" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Suggested" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("Suggested") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtSuggested" Text='<%# Bind("Suggested") %>'  runat="server" />
            <asp:CompareValidator ID="cmpSuggested" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtSuggested" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valSuggested" Text="Required" Display="Dynamic" ControlToValidate="txtSuggested" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:CommandField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ShowEditButton="true" ButtonType="Image" HeaderText="Edit" EditImageUrl="~/images/edit.gif"  UpdateImageUrl="~/images/ok.gif" CancelImageUrl="~/images/cancel.gif" ShowCancelButton="true" />
    </Columns>
</asp:GridView>

<asp:ObjectDataSource ID="odsGridSource" TypeName="TypeOfRiskSurcharge" SelectMethod="Select" UpdateMethod="Update" runat="server">
    <SelectParameters>
        <asp:ControlParameter Type="Int32" Name="company" ControlID="ddlCompanies" />
    </SelectParameters>
    <UpdateParameters>
        <asp:Parameter Name="company" Type="Int32" />
        <asp:Parameter Name="typeOfRisk" Type="Int32" />
        <asp:Parameter Name="surcharge" Type="Decimal" />
        <asp:Parameter Name="Additional" Type="Decimal" />
        <asp:Parameter Name="Coverage" Type="Boolean" />
        <asp:Parameter Name="Minimum" Type="Decimal" />
        <asp:Parameter Name="Maximum" Type="Decimal" />
        <asp:Parameter Name="Suggested" Type="Decimal" />
    </UpdateParameters>
</asp:ObjectDataSource>

</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

