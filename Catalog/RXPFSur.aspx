<%@ Page Title="" Language="C#" MasterPageFile="~/HouseMaster.master" AutoEventWireup="true" CodeFile="RXPFSur.aspx.cs" Inherits="Catalog_RXPFSur" %>

<asp:Content ID="Content1" ContentPlaceHolderID="workloadContent" Runat="Server">
<asp:UpdatePanel ID="updContent" runat="server">
<ContentTemplate>
<h1 class="pageHeader">RXPF Surcharge</h1>
Company: 
<asp:DropDownList ID="ddlCompanies" runat="server" DataValueField="company" DataTextField="companyDescription" AutoPostBack="true" />


Currency:
<asp:DropDownList ID="ddlCurrency" AutoPostBack="true" runat="server">
    <asp:ListItem Text="Mexican Pesos" Value="1" />
    <asp:ListItem Text="U.S. Dollars" Value="2" />
</asp:DropDownList>
<br />

<asp:GridView ID="grdCatalog" FooterStyle-CssClass="catalogTableFooter" ShowFooter="true" OnRowCreated="RowCreated"
             GridLines="None" CellSpacing="1" AlternatingRowStyle-CssClass="catalogTableAlternate" CssClass="catalogTable"
             EditRowStyle-CssClass="catalogTableEdit" HeaderStyle-CssClass="catalogTableHeader" OnRowDataBound="grdCatalog_RowDataBound"
             EmptyDataText="This catalog is empty." DataKeyNames="company,currency,month" DataSourceID="odsGridSource" AutoGenerateColumns="false" runat="server">
    <Columns>
    <asp:TemplateField HeaderText="Month">
        <ItemTemplate>
            <asp:Label ID="lblMonth" runat="server" />
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Monthly">
        <ItemTemplate>
            <%# Eval("monthly") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtMonthly" Text='<%# Bind("monthly") %>'  runat="server" />
            <asp:CompareValidator ID="cmpMonthly" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtMonthly" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valMonthly" Text="Required" Display="Dynamic" ControlToValidate="txtMonthly" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Quarterly">
        <ItemTemplate>
            <%# Eval("quarterly") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtQuarterly" Text='<%# Bind("quarterly") %>'  runat="server" />
            <asp:CompareValidator ID="cmpQuarterly" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtQuarterly" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valQuarterly" Text="Required" Display="Dynamic" ControlToValidate="txtQuarterly" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Biyearly">
        <ItemTemplate>
            <%# Eval("biyearly") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtBiyearly" Text='<%# Bind("biyearly") %>'  runat="server" />
            <asp:CompareValidator ID="cmpBiyearly" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtBiyearly" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valBiyearly" Text="Required" Display="Dynamic" ControlToValidate="txtBiyearly" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    
    <asp:CommandField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ShowEditButton="true" ButtonType="Image" HeaderText="Edit" EditImageUrl="~/images/edit.gif"  UpdateImageUrl="~/images/ok.gif" CancelImageUrl="~/images/cancel.gif" ShowCancelButton="true" />
    </Columns>
</asp:GridView>

<asp:ObjectDataSource ID="odsGridSource" TypeName="rxpfSur" DataObjectTypeName="rxpfSur" OldValuesParameterFormatString="oldValue" ConflictDetection="CompareAllValues" SelectMethod="SelectWithCompanyAndCurrency" UpdateMethod="Update" runat="server">
    <SelectParameters>
        <asp:ControlParameter Type="Int32" Name="company" ControlID="ddlCompanies" />
        <asp:ControlParameter Type="Int32" Name="currency" ControlID="ddlCurrency" />
    </SelectParameters>
</asp:ObjectDataSource>

</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

