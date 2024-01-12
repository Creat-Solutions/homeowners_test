<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ValuesAndSublimits.aspx.cs" Inherits="Catalog_ValuesandSublimits" MasterPageFile="~/HouseMaster.master" Title="Catalog: Values and Sublimits" %>

<asp:Content ID="cntHolder" ContentPlaceHolderID="workloadContent" runat="server">
<asp:UpdatePanel ID="updContent" runat="server">
<ContentTemplate>
<h1 class="pageHeader">Values and Sublimits</h1>
<br />
<asp:DropDownList ID="ddlCompanies" runat="server" DataValueField="company" DataTextField="companyDescription" AutoPostBack="true" />
<br />
<asp:GridView ID="grdValuesSublimits" FooterStyle-CssClass="catalogTableFooter" ShowFooter="true" OnRowCreated="RowCreated"
             GridLines="None" CellSpacing="1" AlternatingRowStyle-CssClass="catalogTableAlternate" CssClass="catalogTable"
             EditRowStyle-CssClass="catalogTableEdit" HeaderStyle-CssClass="catalogTableHeader" 
             EmptyDataText="This catalog is empty." DataKeyNames="id, company" DataSourceID="odsGridSource" AutoGenerateColumns="false" runat="server">
<Columns>
    <asp:TemplateField HeaderText="Name">
        <ItemTemplate>
            <%# Eval("name") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtEditName" MaxLength="50" Text='<%# Bind("name") %>' runat="server" />
            <user:LengthValidator ID="lenName" MaxLength="50" Display="Dynamic" Text="Too many characters" ControlToValidate="txtEditName" runat="server" />
            <asp:RequiredFieldValidator ID="valName" Display="Dynamic" ControlToValidate="txtEditName" Text="Required" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Nombre">
        <ItemTemplate>
            <%# Eval("nameEs") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtEditNombre" MaxLength="50" Text='<%# Bind("nameEs") %>' runat="server" />
            <user:LengthValidator ID="lenNombre" MaxLength="50" Display="Dynamic" Text="Too many characters" ControlToValidate="txtEditNombre" runat="server" />
            <asp:RequiredFieldValidator ID="valNombre" Display="Dynamic" ControlToValidate="txtEditNombre" Text="Required" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Minimum">
        <ItemTemplate>
            <%# Eval("minimum") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtEditMin" Text='<%# Bind("minimum") %>' runat="server" />
            <asp:CompareValidator ID="cmpMin" Display="Dynamic" ControlToValidate="txtEditMin" Operator="DataTypeCheck" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valMin" Display="Dynamic" ControlToValidate="txtEditMin" Text="Required" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Maximum">
        <ItemTemplate>
            <%# Eval("maximum") %>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtEditMax" Text='<%# Bind("maximum") %>' runat="server" />
            <asp:CompareValidator ID="cmpMax" Display="Dynamic" ControlToValidate="txtEditMax" Operator="DataTypeCheck" Type="Double" runat="server" />
            <asp:RequiredFieldValidator ID="valMax" Display="Dynamic" ControlToValidate="txtEditMax" Text="Required" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:CheckBoxField DataField="percentage" HeaderText="Percentage?" />
    <asp:TemplateField HeaderText="Help Text">
        <ItemTemplate>
            <user:LimitedLabel MaxCharactersOnDisplay="30" ID="lblHelp" Text='<%# Eval("help") %>' runat="server" />
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtEditHelp" Text='<%# Bind("help") %>' TextMode="MultiLine" runat="server" />
            <asp:RequiredFieldValidator ID="valHelp" Display="Dynamic" ControlToValidate="txtEditHelp" Text="Required" runat="server" />
            <user:LengthValidator ID="lenHelp" Display="Dynamic" MaxLength="1000" ControlToValidate="txtEditHelp" Text="Required" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Texto de ayuda">
        <ItemTemplate>
            <user:LimitedLabel MaxCharactersOnDisplay="30" ID="lblHelpEs" Text='<%# Eval("helpEs") %>' runat="server" />
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="txtEditHelpEs" Text='<%# Bind("helpEs") %>' TextMode="MultiLine" runat="server" />
            <asp:RequiredFieldValidator ID="valHelpEs" Display="Dynamic" ControlToValidate="txtEditHelpEs" Text="Required" runat="server" />
            <user:LengthValidator ID="lenHelpEs" Display="Dynamic" MaxLength="1000" ControlToValidate="txtEditHelpEs" Text="Required" runat="server" />
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:CommandField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ShowEditButton="true" ButtonType="Image" HeaderText="Edit" EditImageUrl="~/images/edit.gif"  UpdateImageUrl="~/images/ok.gif" CancelImageUrl="~/images/cancel.gif" ShowCancelButton="true" />
</Columns>
</asp:GridView>

<asp:ObjectDataSource ID="odsGridSource" ConflictDetection="CompareAllValues" DataObjectTypeName="valueSublimit" TypeName="valueSublimit" SelectMethod="Select" UpdateMethod="Update" DeleteMethod="Delete" OldValuesParameterFormatString="oldValue" runat="server">
    <SelectParameters>
        <asp:ControlParameter Type="Int32" Name="company" ControlID="ddlCompanies" />
    </SelectParameters>
</asp:ObjectDataSource>

</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
