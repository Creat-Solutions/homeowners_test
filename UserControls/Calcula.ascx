<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Calcula.ascx.cs" Inherits="UserControls_Calcula" %>

<div style="text-align: center">
    <asp:Label ID="lblQuote" runat="server" />
</div>
<div style="text-align: left;">
    <asp:Label ID="lblName" runat="server" />
</div>
<div style="text-align: left;">
    <asp:Label ID="lblDiscount" runat="server" />
</div>
<div style="text-align: left;">
    <asp:Label ID="lblCurrency" runat="server" />
</div>
<div style="font-size: smaller; text-align: left;">
Zona FHM: <asp:Label ID="lblFHM" runat="server" /><br />
Zona Robo: <asp:Label ID="lblZonaRobo" runat="server" /><br />
Zona TEV: <asp:Label ID="lblTEV" runat="server" /><br />
<br />
<table>
<tbody>
<tr>
<td style="width: 300px;" valign="top">
<span style="font-weight: bold;">DEL ASEGURADO</span><br />
<table>
<tbody>
    <tr>
        <td style="text-align: right; width: 100px;">Propietario: </td><td><asp:Label ID="lblClientType" runat="server" /></td>
    </tr>
</tbody>
</table>
<span style="font-weight: bold;">DE LA VIVIENDA</span><br />
<table border="0">
<tbody>
    <tr>
        <td style="text-align: right; width: 100px;">Tipo de vivienda: </td><td><asp:Label ID="lblType" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align: right; width: 100px;">Uso: </td><td><asp:Label ID="lblUse" runat="server" /></td>
    </tr>
    <tr>
        <td valign="top" style="text-align: right; width: 100px;">Distancia al mar: </td><td><asp:Label ID="lblDistanceSea" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align: right; width: 100px;"></td><td><asp:Label ID="lblAdjoining" runat="server" /></td>
    </tr>
</tbody>
</table>
</td>
<td style="width: 300px;" valign="top">
<span style="font-weight: bold;">DE LA CONSTRUCCION</span><br />
<table>
<tbody>
    <tr>
        <td style="text-align: right; width: 100px;">Construcci&oacute;n Techo: </td><td><asp:Label ID="lblRoof" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align: right; width: 100px;">Construcci&oacute;n Muro:</td><td><asp:Label ID="lblWall" runat="server" /></td>
    </tr>
</tbody>
</table>
<span style="font-weight: bold;">SEGURIDAD ROBO</span><br />
<table>
<tbody>
    <tr>
        <td style="text-align: right; width: 100px;">Guardias: </td><td><asp:Label ID="lblGuards" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align: right; width: 100px;">Alarma central: </td><td><asp:Label ID="lblCentralAlarm" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align: right; width: 100px;">Alarma local: </td><td><asp:Label ID="lblLocalAlarm" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align: right; width: 100px;">Circuito de TV: </td><td><asp:Label ID="lblTVCircuit" runat="server" /></td>
    </tr>
    <tr>
        <td style="text-align: right; width: 100px;">Video Portero: </td><td><asp:Label ID="lblTVGuard" runat="server" /></td>
    </tr>
</tbody>
</table>
</td>
</tr>
</tbody>
</table>
<table>
<tbody>
    <tr>
        <td align="right">Earthquake / Terremoto: </td><td><asp:Label ID="lblQuake" runat="server" /></td>
    </tr>
    <tr>
        <td align="right">HMP / FHM: </td><td><asp:Label ID="lblHMP" runat="server" /></td>
    </tr>
</tbody>
</table>

</div>


<table style="font-size: smaller;">
<tbody>
<tr>
<td>
<asp:GridView ID="grdCalcula" EnableViewState="false" DataSourceID="odsCalcula" OnRowDataBound="grdCalcula_RowDataBound" AutoGenerateColumns="false" HeaderStyle-CssClass="calculaTableHeader" runat="server">
<EmptyDataTemplate>
There are no quote details for such id
</EmptyDataTemplate>
<Columns>
    <asp:BoundField HeaderText="Cod" DataField="codigoCobertura" />
    <asp:BoundField HeaderText="Cobertura" DataField="descripcionCobertura" />
    <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="Monto">
        <ItemTemplate>
            <asp:Label ID="lblCovered" runat="server" />
        </ItemTemplate>
    </asp:TemplateField>
    <asp:BoundField HeaderText="Tasa" DataField="tasa" />
    <asp:BoundField HeaderText="Rec" DataField="rec" />
    <asp:BoundField HeaderText="Des" DataField="des" />
    <asp:BoundField HeaderText="T. Final" DataField="tasaFinal" />
    <asp:BoundField HeaderText="Prima" DataField="prima" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:c}" />
    <asp:BoundField HeaderText="Fac" DataField="fac" />
    <asp:BoundField HeaderText="%" DataField="porcentaje" />
    <asp:BoundField HeaderText="Abs" DataField="comision" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:c}" />
    
</Columns>
</asp:GridView>
</td>
</tr>
<tr>
<td>
<table id="total" align="right">
<tbody>
<tr>
    <th colspan="3" align="center">PRIMA</th><th style="min-width: 100px; width: auto !important; width: 200px;">COMISI&Oacute;N</th>
</tr>
<tr>
    <td></td><td>Prima Neta: </td><td align="right"><asp:Label ID="lblPrima" runat="server" /></td><td align="right"><asp:Label ID="lblComision" runat="server" /></td>
</tr>
<tr>
    <td></td><td>Derechos: </td><td align="right"><asp:Label ID="lblDerechos" runat="server" /></td><td align="right"><asp:Label ID="lblPorComision" runat="server" />%</td>
</tr>
<tr>
    <td><asp:Label ID="lblPorRXPF" runat="server" /></td><td>RXPF: </td><td align="right"><asp:Label ID="lblRXPF" runat="server" /></td><td></td>
</tr>
<tr>
    <td><asp:Label ID="lblPorTax" runat="server" />%</td><td>Tax: </td><td align="right"><asp:Label ID="lblTax" runat="server" /></td><td></td>
</tr>
<tr>
    <td></td><td>TOTAL: </td><td align="right"><asp:Label ID="lblTotal" runat="server" /></td><td></td>
</tr>
</tbody>
</table>
</td>
</tr>
</tbody>
</table>
<p>
    RXPF might be 0 because it hasn't been selected if the quote is going to be payed in instalments.
</p>
<asp:ObjectDataSource ID="odsCalcula" TypeName="quoteDetail" DataObjectTypeName="quoteDetail" SelectMethod="SelectByQuote" runat="server">
<SelectParameters>
    <asp:Parameter Name="quote" Type="Int32" />
</SelectParameters>
</asp:ObjectDataSource>