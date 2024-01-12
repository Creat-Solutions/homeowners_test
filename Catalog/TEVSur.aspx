<%@ Page Language="C#" MasterPageFile="~/HouseMaster.master" AutoEventWireup="true" CodeFile="TEVSur.aspx.cs" Inherits="Catalog_TEVSur" Title="Catalog: TEV Surcharge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="workloadContent" Runat="Server">
<asp:UpdatePanel ID="updContent" runat="server">
<ContentTemplate>
<h1 class="pageHeader">TEV Surcharge</h1>
<asp:DropDownList ID="ddlCompanies" runat="server" DataValueField="company" DataTextField="companyDescription" AutoPostBack="true" />

<br />
<asp:GridView ID="grdTEV" FooterStyle-CssClass="catalogTableFooter" ShowFooter="true" OnRowCreated="RowCreated"
             GridLines="None" CellSpacing="1" AlternatingRowStyle-CssClass="catalogTableAlternate" CssClass="catalogTable"
             EditRowStyle-CssClass="catalogTableEdit" HeaderStyle-CssClass="catalogTableHeader" 
             EmptyDataText="This catalog is empty." DataKeyNames="company,TEV" DataSourceID="odsGridSource" AutoGenerateColumns="false" runat="server">
    <Columns>
    <asp:TemplateField HeaderText="Description">
        <ItemTemplate>
            <%# Eval("TEV") %>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Building" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("edif") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtEdif" Text='<%# Bind("edif") %>'  runat="server" />
            <asp:CompareValidator ID="cmpEdif" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtEdif" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valEdif" Text="Required" Display="Dynamic" ControlToValidate="txtEdif" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
     <asp:TemplateField HeaderText="Contents" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("cont") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtCont" Text='<%# Bind("cont") %>'  runat="server" />
            <asp:CompareValidator ID="cmpCont" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtCont" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valCont" Text="Required" Display="Dynamic" ControlToValidate="txtCont" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
     <asp:TemplateField HeaderText="Coinsurance" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("coaseguro") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtCoaseguro" Text='<%# Bind("coaseguro") %>'  runat="server" />
            <asp:CompareValidator ID="cmpCoaseguro" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtCoaseguro" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valCoaseguro" Text="Required" Display="Dynamic" ControlToValidate="txtCoaseguro" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
     <asp:TemplateField HeaderText="Deductible" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("deducible") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtDeducible" Text='<%# Bind("deducible") %>'  runat="server" />
            <asp:CompareValidator ID="cmpDeducible" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtDeducible" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valDeducible" Text="Required" Display="Dynamic" ControlToValidate="txtDeducible" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
     <asp:TemplateField HeaderText="Debri Removal" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("remocionEscombros") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtEscombros" Text='<%# Bind("remocionEscombros") %>'  runat="server" />
            <asp:CompareValidator ID="cmpEscombros" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtEscombros" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valEscombros" Text="Required" Display="Dynamic" ControlToValidate="txtEscombros" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
     <asp:TemplateField HeaderText="Extraordinary Expenses" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("gastoExtra") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtGastoExtra" Text='<%# Bind("gastoExtra") %>'  runat="server" />
            <asp:CompareValidator ID="cmpGastoExtra" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtGastoExtra" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valGastoExtra" Text="Required" Display="Dynamic" ControlToValidate="txtGastoExtra" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
     <asp:TemplateField HeaderText="Per Rent" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <%# Eval("perRenta") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtPerRenta" Text='<%# Bind("perRenta") %>'  runat="server" />
            <asp:CompareValidator ID="cmpPerRenta" Text="Invalid value" Display="Dynamic" Operator="DataTypeCheck" ControlToValidate="txtPerRenta" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valPerRenta" Text="Required" Display="Dynamic" ControlToValidate="txtPerRenta" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:CommandField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ShowEditButton="true" ButtonType="Image" HeaderText="Edit" EditImageUrl="~/images/edit.gif"  UpdateImageUrl="~/images/ok.gif" CancelImageUrl="~/images/cancel.gif" ShowCancelButton="true" />
    </Columns>
</asp:GridView>

<asp:ObjectDataSource ID="odsGridSource" TypeName="TEVSurcharge" SelectMethod="Select" UpdateMethod="Update" runat="server">
    <SelectParameters>
        <asp:ControlParameter Type="Int32" Name="company" ControlID="ddlCompanies" />
    </SelectParameters>
    <UpdateParameters>
        <asp:Parameter Name="company" Type="Int32" />
        <asp:Parameter Name="tev" Type="Int32" />
        <asp:Parameter Name="edif" Type="Decimal" />
        <asp:Parameter Name="cont" Type="Decimal" />
        <asp:Parameter Name="coaseguro" Type="Decimal" />
        <asp:Parameter Name="deducible" Type="Decimal" />
        <asp:Parameter Name="remocionEscombros" Type="Decimal" />
        <asp:Parameter Name="gastoExtra" Type="Decimal" />
        <asp:Parameter Name="perRenta" Type="Decimal" />
    </UpdateParameters>
</asp:ObjectDataSource>

</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

