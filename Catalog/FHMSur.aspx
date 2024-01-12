<%@ Page Language="C#" MasterPageFile="~/HouseMaster.master" AutoEventWireup="true" CodeFile="FHMSur.aspx.cs" Inherits="Catalog_FHMSur" Title="Catalog: HMP Surcharge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="workloadContent" Runat="Server">
<asp:UpdatePanel ID="updContent" runat="server">
<ContentTemplate>
<h1 class="pageHeader">HMP Surcharge</h1>
<asp:DropDownList ID="ddlCompanies" runat="server" DataValueField="company" DataTextField="companyDescription" AutoPostBack="true" />

<br />
<asp:GridView ID="grdFHM" FooterStyle-CssClass="catalogTableFooter" ShowFooter="true" OnRowCreated="RowCreated"
             GridLines="None" CellSpacing="1" AlternatingRowStyle-CssClass="catalogTableAlternate" CssClass="catalogTable"
             EditRowStyle-CssClass="catalogTableEdit" HeaderStyle-CssClass="catalogTableHeader" 
             EmptyDataText="This catalog is empty." DataKeyNames="company,FHM" DataSourceID="odsGridSource" AutoGenerateColumns="false" runat="server">
    <Columns>
    <asp:TemplateField HeaderText="Description">
        <ItemTemplate>
            <%# Eval("description") %>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Interior Coinsurance" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("cInterior") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtCInterior" Text='<%# Bind("cInterior") %>'  runat="server" />
            <asp:CompareValidator ID="cmpCInterior" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtCInterior" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valCInterior" Text="Required" Display="Dynamic" ControlToValidate="txtCInterior" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Beach Coinsurance" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("cBeach") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtCBeach" Text='<%# Bind("cBeach") %>'  runat="server" />
            <asp:CompareValidator ID="cmpCBeach" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtCBeach" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valCBeach" Text="Required" Display="Dynamic" ControlToValidate="txtCBeach" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="HMP Rem" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("Rem") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtRem" Text='<%# Bind("Rem") %>'  runat="server" />
            <asp:CompareValidator ID="cmpRem" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtRem" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valRem" Text="Required" Display="Dynamic" ControlToValidate="txtRem" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="HMP GE" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("GE") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtGE" Text='<%# Bind("GE") %>'  runat="server" />
            <asp:CompareValidator ID="cmpGE" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtGE" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valGE" Text="Required" Display="Dynamic" ControlToValidate="txtGE" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Interior Deductible" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("dedInterior") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtDedInterior" Text='<%# Bind("dedInterior") %>'  runat="server" />
            <asp:CompareValidator ID="cmpDedInterior" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtDedInterior" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valDedInterior" Text="Required" Display="Dynamic" ControlToValidate="txtDedInterior" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Beach Deductible" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("dedBeach") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtDedBeach" Text='<%# Bind("dedBeach") %>'  runat="server" />
            <asp:CompareValidator ID="cmpDedBeach" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtDedBeach" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valDedBeach" Text="Required" Display="Dynamic" ControlToValidate="txtDedBeach" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Coinsurance" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("coAseguro") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtCoAseguro" Text='<%# Bind("coAseguro") %>'  runat="server" />
            <asp:CompareValidator ID="cmpCoAseguro" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtCoAseguro" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valCoAseguro" Text="Required" Display="Dynamic" ControlToValidate="txtCoAseguro" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:CommandField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ShowEditButton="true" ButtonType="Image" HeaderText="Edit" EditImageUrl="~/images/edit.gif"  UpdateImageUrl="~/images/ok.gif" CancelImageUrl="~/images/cancel.gif" ShowCancelButton="true" />
    </Columns>
</asp:GridView>

<asp:ObjectDataSource ID="odsGridSource" TypeName="FHMSurcharge" SelectMethod="Select" UpdateMethod="Update" runat="server">
    <SelectParameters>
        <asp:ControlParameter Type="Int32" Name="company" ControlID="ddlCompanies" />
    </SelectParameters>
    <UpdateParameters>
        <asp:Parameter Name="company" Type="Int32" />
        <asp:Parameter Name="FHM" Type="Int32" />
        <asp:Parameter Name="cInterior" Type="Decimal" />
        <asp:Parameter Name="cBeach" Type="Decimal" />
        <asp:Parameter Name="Rem" Type="Decimal" />
        <asp:Parameter Name="GE" Type="Decimal" />
        <asp:Parameter Name="dedInterior" Type="Decimal" />
        <asp:Parameter Name="dedBeach" Type="Decimal" />
        <asp:Parameter Name="coAseguro" Type="Decimal" />
    </UpdateParameters>
</asp:ObjectDataSource>

</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

