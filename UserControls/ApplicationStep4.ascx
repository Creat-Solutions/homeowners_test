<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ApplicationStep4.ascx.cs" Inherits="UserControls_ApplicationStep4" %>
<h3>Distance From Sea and Storm Shutters</h3>
<div class="row row4nofloat">
    <asp:Label ID="lblShutters" AssociatedControlID="rblShutters" Text="I. Does your property have storm shutters?" runat="server" />
    <asp:RequiredFieldValidator ID="reqShutters" Text="Required" Display="Dynamic" ControlToValidate="rblShutters" runat="server" />
    <br />
    <div class="rowes">
        <asp:Label ID="lblShuttersEs" AssociatedControlID="rblShutters" Text="&iquest;Se cuenta con protecciones met&aacute;licas?" runat="server" />
    </div>
    <asp:RadioButtonList ID="rblShutters" runat="server">
        <asp:ListItem Text="Yes / S&iacute;" Value="1" />
        <asp:ListItem Text="No / No" Value="0" />
    </asp:RadioButtonList>
    
</div>
<br />
<div class="row row4distance">
    <asp:Label ID="lblDistanceSea" AssociatedControlID="rblDistanceSea" Text="II. Distance Above sea water level line (Beach Front): " runat="server" />
    <div class="rowes">
    <asp:Label ID="lblDistanciaMar" Font-Italic="true" Font-Size="Smaller" AssociatedControlID="rblDistanceSea" Text="Distancia por arriba de la l&iacute;nea del mar" runat="server" />
    </div>
    &nbsp;<asp:RequiredFieldValidator ID="reqDistance" ControlToValidate="rblDistanceSea" Text="Required" Display="Dynamic" runat="server" />
    <asp:RadioButtonList ID="rblDistanceSea" DataSourceID="odsDistanceSea" 
        DataValueField="distance" DataTextField="description" runat="server" 
         />
</div>
<hr />
<h3>Burglary Securities</h3>
<div class="row row4nofloat">
    <asp:Label ID="lblBurglary" Text="Burglary Securites:" runat="server" /><br />
    <div class="rowes">
        <asp:Label ID="lblRobo" Text="Protecciones contra robo" runat="server" />
    </div>
    <table>
    <tbody>
        <tr>
            <td><asp:CheckBox ID="chk24Guards" Text="24 Hours Guards / Guardias las 24 Horas" runat="server" /></td>
            <td><asp:CheckBox ID="chkCentralAlarm" Text="Central Alarm / Alarma central" runat="server" /></td>
        </tr>
        <tr>
            <td><asp:CheckBox ID="chkTVCircuit" Text="TV Circuit / Circuito de TV" runat="server" /></td>
            <td><asp:CheckBox ID="chkTVGuard" Text="Local Guard with TV Circuit / Video Portero" runat="server" /></td>
            
        </tr>
        <tr>
            <td><asp:CheckBox ID="chkLocalAlarm" Text="Local Alarm / Alarma local" runat="server" /></td>
            <td></td>
        </tr>
    </tbody>
    </table>
</div>

<asp:ObjectDataSource ID="odsDistanceSea" TypeName="distanceFromSea" SelectMethod="SelectForRBL" runat="server" />