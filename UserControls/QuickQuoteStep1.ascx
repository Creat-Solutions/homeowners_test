<%@ Control Language="C#" AutoEventWireup="true" CodeFile="QuickQuoteStep1.ascx.cs" Inherits="UserControls_QuickQuoteStep1" %>

<table>
<tr>
<td class="quoteStep borderStep" valign="top" style="padding-right: 40px;">

<table class="quoteAppData quoteAppDataAmple">

    <tr>
        <td class="labelText">
            <asp:Label ID="lblClientType" AssociatedControlID="ddlClientType" runat="server" Text="Type of client: " />
        </td>
        <td>
            <asp:DropDownList ID="ddlClientType" runat="server" DataTextField="description" DataValueField="type" />
            
        </td>
    </tr>
    <tr>
        <td class="rowes">
            Tipo de cliente
        </td>
        <td></td>
    </tr>
    
    <tr>
        <td class="labelText">
            <asp:Label ID="lblHouseType" AssociatedControlID="ddlHouseType" runat="server" Text="Type of property: " />
        </td>
        <td>
            <asp:DropDownList ID="ddlHouseType" DataTextField="description" DataValueField="type" runat="server" />
           
        </td>
    </tr>
    <tr>
        <td class="rowes">
            Tipo de propiedad
        </td>
        <td></td>
    </tr>
    <tr>
        <td class="labelText">
            <asp:Label ID="lblUse" AssociatedControlID="ddlUse" runat="server" Text="Use of property: " />
        </td>
        <td>
            <asp:DropDownList ID="ddlUse" DataValueField="use" DataTextField="description" runat="server" />
           
        </td>
    </tr>
    <tr>
        <td class="rowes">
            Uso de la propiedad
        </td>
        <td></td>
    </tr>
    
    <tr>
        <td class="labelText">
            <asp:Label ID="lblRoof" AssociatedControlID="ddlRoof" runat="server" Text="Roof type: " />
        </td>
        <td>
            <asp:DropDownList ID="ddlRoof" DataValueField="type" DataTextField="description" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="rowes">
            Tipo de techo
        </td>
        <td></td>
    </tr>
    <tr>
        <td class="labelText">
            <asp:Label ID="lblWall" AssociatedControlID="ddlWall" runat="server" Text="Wall type: " />
        </td>
        <td>
            <asp:DropDownList ID="ddlWall" DataValueField="type" DataTextField="description" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="rowes">
            Tipo de muro
        </td>
        <td></td>
    </tr>
    <tr>
        <td class="labelText">
            <asp:Label ID="lblFloors" AssociatedControlID="ddlFloors" runat="server" Text="Number of floors: " />
        </td>
        <td>
            <asp:DropDownList ID="ddlFloors" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="rowes">
            N&uacute;mero de pisos
        </td>
        <td></td>
    </tr>

</table>
<br /><br />
<h4 class="quoteTitle" style="margin-top: -10px;">Desired Coverages</h4>
<span class="rowes" style="font-weight: bold;">Coberturas deseadas</span>
<br />
<table>
<tr>
    <td>Earthquake / Terremoto</td>
    <td><asp:CheckBox ID="chkQuake" Text="" runat="server" /></td>
</tr>

<tr>
    <td>HMP / FHM</td>
    <td><asp:CheckBox ID="chkHMP" Text="" runat="server" /></td>
</tr>

<tr>
    <td>Fire All Risk / Todo Riesgo Incendio</td>
    <td><asp:CheckBox ID="chkFire" Text="" Checked="true" Enabled="false" runat="server" /></td>
</tr>
</table>




</td>
<td class="quoteStep" valign="top">


<h4 class="quoteTitle">Burglary securities</h4>
<span class="rowes" style="font-weight: bold;">Protecciones contra robo</span>
<br />
<asp:UpdatePanel ID="updAlarms" runat="server">
<ContentTemplate>
<table class="quoteAppData">

    <tr>
        <td class="labelText">
            <asp:RadioButton ID="rbNoAlarm" OnCheckedChanged="rbAlarm_CheckedChanged" AutoPostBack="true" GroupName="grpAlarm" runat="server" />
        </td>
        <td>
            <asp:Label ID="lblNoAlarm" AssociatedControlID="rbNoAlarm" runat="server" Text="No" />
            
        </td>
    </tr>
    <tr>
        <td class="labelText">
            <asp:RadioButton ID="rbWithAlarm" OnCheckedChanged="rbAlarm_CheckedChanged" AutoPostBack="true" GroupName="grpAlarm" runat="server" />
        </td>
        <td>
            <asp:Label ID="lblLocal" AssociatedControlID="rbWithAlarm" runat="server" Text="Yes/Sí: " />
            <asp:DropDownList ID="ddlAlarm" runat="server">
                <asp:ListItem Text="Alarma local" Value="1" />
                <asp:ListItem Text="Alarma central" Value="0" />
            </asp:DropDownList>
        </td>
    </tr>
</table>
</ContentTemplate>
</asp:UpdatePanel>

<h4 class="quoteTitle">Storm shutters</h4>
<span class="rowes" style="font-weight: bold;">Protecciones contra huracán</span>
<br />

<table class="quoteAppData">
    <tr>
        <td class="labelText">
            <asp:RadioButton ID="rbNoShutter" GroupName="rbShutters" runat="server" />
        </td>
        <td>
            No
        </td>
    </tr>
    <tr>
        <td>
            <asp:RadioButton ID="rbWithShutter" GroupName="rbShutters" runat="server" />
        </td>
        <td>
            Yes/S&iacute;
        </td>
    </tr>
</table>

<h4 class="quoteTitle">Surrounded by Adjacent Structures?</h4>
<span class="rowes" style="font-weight: bold;">&iquest;Colindantes sin fincar?</span>
<br /> 
<table class="quoteAppData">
    <tr>
        <td class="labelText">
            <asp:RadioButton ID="rbnAdjNo" GroupName="grpAdjoining" runat="server" />
        </td>
        <td>
            No
        </td>
    </tr>
    <tr>
        <td class="labelText">
            <asp:RadioButton ID="rbnAdjYes" GroupName="grpAdjoining" runat="server" />
        </td>
        <td>
            Yes / S&iacute;
        </td>
    </tr>
</table>

<h4 class="quoteTitle">Distance above sea water level line (Beach Front):</h4>
<span class="rowes" style="font-weight: bold;">Distancia por arriba de la l&iacute;nea del mar</span>


<div class="row">
    
    <asp:RequiredFieldValidator ID="reqDistance" ControlToValidate="rblDistanceSea" Text="Required" Display="Dynamic" runat="server" />
    <asp:RadioButtonList ID="rblDistanceSea" DataSourceID="odsDistanceSea" OnDataBound="rblDistanceSea_DataBound" CssClass="qsDistanceSea"
        DataValueField="distance" DataTextField="description" runat="server" 
         />
</div>

</td>
</tr>
</table>

<asp:ObjectDataSource ID="odsDistanceSea" TypeName="distanceFromSea" SelectMethod="SelectForRBL" runat="server" />