<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ApplicationStep5.ascx.cs" Inherits="UserControls_ApplicationStep5" %>

<script type="text/javascript">

    var limitsDisabled = false;

    var currencyDollars = true;

    function GetMinimumArray() {
        return currencyDollars ? valuesMinimum : valuesPesosMinimum;
    }

    function GetMaximumArray() {
        return currencyDollars ? valuesMaximum : valuesPesosMaximum;
    }

function ToggleValidators() {

    var buildingMinimum = $get('<%= tdBuildingMinimum.ClientID %>');
    var buildingMaximum = $get('<%= tdBuildingMaximum.ClientID %>');

    var contentsMinimum = $get('<%= tdContentsMinimum.ClientID %>');
    var contentsMaximum = $get('<%= tdContentsMaximum.ClientID %>');

    limitsDisabled = !limitsDisabled;
    if (limitsDisabled) {
        buildingMinimum.innerHTML = "N/A";
        buildingMaximum.innerHTML = "N/A";
        contentsMinimum.innerHTML = "N/A";
        contentsMaximum.innerHTML = "N/A";
    } else {
        var mins = GetMinimumArray();
        var maxs = GetMaximumArray();
        buildingMinimum.innerHTML = "$" + Number(mins[0]).format("N");
        buildingMaximum.innerHTML = "$" + Number(maxs[0]).format("N");
        contentsMinimum.innerHTML = "$" + Number(mins[2]).format("N");
        contentsMaximum.innerHTML = "$" + Number(maxs[2]).format("N");
    }
    
}

function UpdateValues() {
    var txtBuilding = $get('<%= txtBuilding.ClientID %>');
    var txtContent = $get('<%= txtContents.ClientID %>');
    var txtOtherStructures = $('#<%= txtOtherStructures.ClientID %>')[0];
    
    if (!(txtBuilding && txtContent)) {
        return;
    }
    var debriMinimum = $get('<%= tdDebriMinimum.ClientID %>');
    var debriMaximum = $get('<%= tdDebriMaximum.ClientID %>');

    var extraMinimum = $get('<%= tdExtraMinimum.ClientID %>');
    var extraMaximum = $get('<%= tdExtraMaximum.ClientID %>');

    if (!(debriMinimum && debriMaximum && extraMaximum && extraMaximum)) {
        return;
    }

    var buildingValue = 0;
    buildingValue = Number.parseInvariant(txtBuilding.value);
    if (isNaN(buildingValue)) { 
        buildingValue = 0;
    }
    var buildingMax = $('#<%= tdBuildingMaximum.ClientID %>');
    var maxValue = GetMaximumArray()[0];
    var otherStructuresValue = 0;
    if (!txtOtherStructures.disabled) {
        otherStructuresValue = Number.parseInvariant(txtOtherStructures.value);
        if (isNaN(otherStructuresValue)) {
            otherStructuresValue = 0;
        }
        maxValue -= otherStructuresValue;
    }
    buildingMax.html('$' + maxValue.format('N'));
    
    var contentsValue = Number.parseInvariant(txtContent.value);
    if (isNaN(contentsValue)) {
        contentsValue = 0;
    }

    var otherStructuresMax = $('#<%= tdOtherStructuresMaximum.ClientID %>');
    var maxs = GetMaximumArray();
    otherStructuresMax.html("$" + (maxs[0] - buildingValue).format("N"));

    var sum = buildingValue + otherStructuresValue + contentsValue;
    if (sum === 0) {
        debriMinimum.innerHTML = Number(percentageMinimum[0]).format("N") + " % ";
        debriMaximum.innerHTML = Number(percentageMaximum[0]).format("N") + " % ";
        extraMinimum.innerHTML = Number(percentageMinimum[1]).format("N") + " % ";
        extraMaximum.innerHTML = Number(percentageMaximum[1]).format("N") + " % ";
    } else {
        debriMinimum.innerHTML = "$" + (Number(percentageMinimum[0]) /100 * sum).format("N");
        debriMaximum.innerHTML = "$" + (Number(percentageMaximum[0]) /100 * sum).format("N");
        extraMinimum.innerHTML = "$" + (Number(percentageMinimum[1]) / 100 * sum).format("N");
        extraMaximum.innerHTML = "$" + (Number(percentageMaximum[1]) / 100 * sum).format("N");
    }
}

function changeOutdoor(sobj) {
    if (!sobj) {
        return true;
    }
    var valOutdoor = $get('<%= this.valOutdoor.ClientID %>');
    var txtOutdoor = $get('<%= this.txtOutdoorDescription.ClientID %>');
    if (!(valOutdoor && txtOutdoor)) {
        return true;
    }
    txtOutdoor.disabled = !sobj.checked;
    ValidatorEnable(valOutdoor, sobj.checked);
}

function changeOtherStructures(sobj) {
    if (!sobj) {
        return;
    }
    var valOtherStructures = $('#<%= valOSDescription.ClientID %>')[0];
    var txtOtherStructures = $('#<%= txtOtherStructuresDescription.ClientID %>')[0];
    if (!(valOtherStructures && txtOtherStructures)) {
        return;
    }
    txtOtherStructures.disabled = !sobj.checked;
    ValidatorEnable(valOtherStructures, sobj.checked);
}

function chkActivatedClick(obj, txtID, valID) {
    var txtLimit = $get(txtID);
    var valLimit = $get(valID);

    if (!(txtLimit && obj && valLimit)) {
        return;
    }

    txtLimit.value = '';
    txtLimit.disabled = !obj.checked;
    UpdateValues();
    ValidatorValidate(valLimit);
}

function chkActivatedBuilding(obj) {
    chkActivatedClick(obj, '<%= txtBuilding.ClientID %>', '<%= valBuilding.ClientID %>');
    if (!obj.checked) {
        var chkOtherStructures = $('#<%= chkOtherStructures.ClientID %>')[0];
        chkOtherStructures.disabled = true;
        chkActivatedOtherStructures(chkOtherStructures);
    }
    
}

function chkActivatedOtherStructures(obj) {
    chkActivatedClick(obj, '<%= txtOtherStructures.ClientID %>', '<%= valOtherStructures.ClientID %>');
    changeOtherStructures(obj);
}

function chkActivatedOutdoor(obj) {
    chkActivatedClick(obj, '<%= txtOutdoor.ClientID %>', '<%= valOutdoor.ClientID %>');
    changeOutdoor(obj);
}

function chkActivatedContents(obj) {
    chkActivatedClick(obj, '<%= txtContents.ClientID %>', '<%= valContents.ClientID %>');
}

function chkActivatedDebri(obj) {
    chkActivatedClick(obj, '<%= txtDebri.ClientID %>', '<%= valDebri.ClientID %>');
}

function chkActivatedExtra(obj) {
    chkActivatedClick(obj, '<%= txtExtra.ClientID %>', '<%= valExtra.ClientID %>');
}

function chkActivatedLossRents(obj) {
    var quake = $get('<%= chkQuake.ClientID %>');
    var hmp = $get('<%= chkHMP.ClientID %>');
    var rowLossRentWarning = $get('<%= lossRentsWarning.ClientID %>');
    if (!(quake && obj && hmp)) {
        return;
    }

    if ((quake.checked || hmp.checked) && obj.checked) {
        rowLossRentWarning.style.display = 'inline';
    }
    else {
        rowLossRentWarning.style.display = 'none';
    }
    chkActivatedClick(obj, '<%= txtLossRents.ClientID %>', '<%= valLossRents.ClientID %>');
}

function checkOtherStructuresDescription(obj, args) {
    var chkOtherStructures = $get('<%= chkOtherStructures.ClientID %>');
    var txtOtherStructures = $get('<%= txtOtherStructuresDescription.ClientID %>');
    if (!(chkOtherStructures && txtOtherStructures)) {
        args.IsValid = false;
        return;
    }
    args.IsValid = !chkOtherStructures.checked || txtOtherStructures.value.length > 0;
}

function checkOutdoorDescription(obj, args) {
    var chkOutdoor = $get('<%= chkOutdoor.ClientID %>');
    var txtOutdoor = $get('<%= txtOutdoorDescription.ClientID %>');
    if (!(chkOutdoor && txtOutdoor)) {
        args.IsValid = false;
        return;
    }
    args.IsValid = !chkOutdoor.checked || txtOutdoor.value.length > 0;
}

function UpdateValuesAndFormat(obj) {
    UpdateValues();
    FormatMoney(obj);
}

function ValidateLimit(obj, args, min, max, percentage, emptyAllowed) {
    var valText = 'Invalid value. ';
    
    var value = null;
    if (args.Value === null || args.Value === '') {
        args.IsValid = emptyAllowed;
        if (!emptyAllowed) {
            obj.innerHTML = "Required";
        }
        return;
    }

    value = Number.parseInvariant(args.Value);
    if (isNaN(value)) {
        args.IsValid = false;
        return;
    }

    var minimum = 0;
    var maximum = 0;
    if (percentage) {
        var buildingLimit = $get('<%= txtBuilding.ClientID %>');
        var contentsLimit = $get('<%= txtContents.ClientID %>');
        if (!(buildingLimit && contentsLimit)) {
            args.IsValid = false;
            return;
        }

        var buildingValue = Number.parseInvariant(buildingLimit.value);
        if (isNaN(buildingValue)) {
            buildingValue = 0;
        }

        var sum = buildingValue + Number.parseInvariant(contentsLimit.value);
        if (sum === 0) {
            valText = valText + 'Please fill the Building and Contents fields.';
            obj.innerHTML = valText;
            args.IsValid = false;
            return;
        }
        minimum = sum * min / 100;
        maximum = sum * max / 100;
    } else {
        minimum = min;
        maximum = max;
    }

    var valid = value >= minimum && value <= maximum;
    if (!valid) {
        valText = valText + 'Minimum: $' + minimum.format("N") + ', Maximum: $' + maximum.format("N");
        obj.innerHTML = valText;
    }
    args.IsValid = valid;
}

function ValidateOtherStructures(obj, args) {
    var maxs = GetMaximumArray();
    var chkOtherStructures = $get('<%= chkOtherStructures.ClientID %>');
    var minimum = 0;
    var buildingLimit = Number.parseInvariant($('#<%= txtBuilding.ClientID %>').val());
    if (isNaN(buildingLimit)) {
        buildingLimit = 0;
    }
    var maximum = limitsDisabled ? Number.POSITIVE_INFINITY : Number(maxs[0]) - buildingLimit;
    ValidateLimit(obj, args, minimum, maximum, false, !chkOtherStructures.checked);
}

function ValidateBuilding(obj, args) {
    var chkBuilding = $get('<%= chkBuilding.ClientID %>');
    var chkOtherStructures = $get('<%= chkOtherStructures.ClientID %>');
    var mins = GetMinimumArray();
    var maxs = GetMaximumArray();
    var minimum = limitsDisabled ? Number.NEGATIVE_INFINITY : Number(mins[0]);
    var maximum = limitsDisabled ? Number.POSITIVE_INFINITY : Number(maxs[0]);
    if (!chkOtherStructures.disabled) {
        var otherStructures = Number.parseInvariant($('#<%= txtOtherStructures.ClientID %>').val());
        if (!isNaN(otherStructures)) {
            maximum -= otherStructures;
        }
    }
    ValidateLimit(obj, args, minimum, maximum, false, !chkBuilding.checked);
}

function ValidateOutdoor(obj, args) {
    var chkOutdoor = $get('<%= chkOutdoor.ClientID %>');
    var mins = GetMinimumArray();
    var maxs = GetMaximumArray();
    ValidateLimit(obj, args, Number(mins[1]), Number(maxs[1]), false, !chkOutdoor.checked);
}

function ValidateContents(obj, args) {
    var mins = GetMinimumArray();
    var maxs = GetMaximumArray();
    var minimum = limitsDisabled ? Number.NEGATIVE_INFINITY : Number(mins[2]);
    var maximum = limitsDisabled ? Number.POSITIVE_INFINITY : Number(maxs[2]);
    ValidateLimit(obj, args, minimum, maximum, false, false);
}

function ValidateDebri(obj, args) {
    var chkDebri = $get('<%= chkDebri.ClientID %>');
    ValidateLimit(obj, args, Number(percentageMinimum[0]), Number(percentageMaximum[0]), true, false);
}

function ValidateExtra(obj, args) {
    var chkExtra = $get('<%= chkExtra.ClientID %>');
    ValidateLimit(obj, args, Number(percentageMinimum[1]), Number(percentageMaximum[1]), true, false);
}

function ValidateLossRents(obj, args) {
    var chkLossRents = $get('<%= chkLossRents.ClientID %>');
    var mins = GetMinimumArray();
    var maxs = GetMaximumArray();
    ValidateLimit(obj, args, Number(mins[3]), Number(maxs[3]), false, !chkLossRents.checked);
}



function ddlCurrency_OnChange(obj) {
    var ddlCurrency = $get('<%= ddlCurrency.ClientID %>');

    currencyDollars = ddlCurrency.options[ddlCurrency.selectedIndex].value == '2';
    var mins = GetMinimumArray();
    var maxs = GetMaximumArray();

    var outdoorMinimum = $get('<%= tdOutdoorMinimum.ClientID %>');
    var outdoorMaximum = $get('<%= tdOutdoorMaximum.ClientID %>');

    var lossRentsMinimum = $get('<%= tdLossRentsMinimum.ClientID %>');
    var lossRentsMaximum = $get('<%= tdLossRentsMaximum.ClientID %>');

    //fast hack for building & contents
    limitsDisabled = !limitsDisabled;
    ToggleValidators();
    UpdateValues();

    outdoorMinimum.innerHTML = "$" + Number(mins[1]).format("N");
    outdoorMaximum.innerHTML = "$" + Number(maxs[1]).format("N");
    lossRentsMinimum.innerHTML = "$" + Number(mins[3]).format("N");
    lossRentsMaximum.innerHTML = "$" + Number(maxs[3]).format("N");
   
    Page_ClientValidate();
}

function StartCurrency(cDollars) {
    currencyDollars = cDollars;
}

function chkHMP_Click(obj) {
    if (!obj) {
        return;
    }
    var chkOutdoor = $get('<%= chkOutdoor.ClientID %>');
    if (chkOutdoor.parentNode.tagName.toLowerCase() == 'span') {
        chkOutdoor.parentNode.disabled = !obj.checked;
    }
    chkOutdoor.disabled = !obj.checked;
    chkOutdoor.checked = false;
    chkActivatedOutdoor(chkOutdoor);
}

</script>
<h3>Desired Coverages</h3>
<div class="row row4nofloat">
    <asp:Label ID="lblCoverages" Text="Desired Coverages: " runat="server" />
    <div class="rowes">
        <asp:Label ID="lblCoberturas" Text="Coberturas deseadas" runat="server" />
    </div>
    <table>
        <tbody>
            <tr>
                <td><asp:CheckBox ID="chkQuake" Text="Earthquake / Terremoto" runat="server" /><br /></td>
                <td><asp:CheckBox ID="chkFire" Text="Fire All Risk / Todo Riesgo Incendio" Checked="true" Enabled="false" runat="server" /></td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="chkHMP" Text="HMP / FHM" onclick="chkHMP_Click(this);" runat="server" />
                    
                </td>
                <td><asp:Label ID="lblInactiveHMP" Text="HMP Deactivated" runat="server" /></td>
            </tr>
        </tbody>
    </table>
    
    
    
</div>
<hr />
<div id="divLimits" class="row" runat="server">
    Do not apply Limits / No aplicar l&iacute;mites
    <asp:CheckBox ID="chkLimits" onclick="ToggleValidators();" runat="server" />
</div>
<br />
<div class="row">
    <asp:Label ID="lblCurrency" AssociatedControlID="ddlCurrency" Text="Currency: " runat="server" />
    <asp:DropDownList ID="ddlCurrency" onchange="ddlCurrency_OnChange(this);" runat="server">
        <asp:ListItem Text="Mexican Pesos" Value="1" />
        <asp:ListItem Text="U.S. Dollars" Value="2" />
    </asp:DropDownList>
    <div class="rowes">
        <asp:Label ID="lblMoneda" Text="Moneda" runat="server" />
    </div>
</div>

<table>
    <thead>
        <tr>
            <th></th>
            <th></th>
            <th>Min</th>
            <th></th>
            <th></th>
            <th>Max</th>
            <th></th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td><asp:HyperLink ID="lnkBuildingQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?id=1" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
            <td class="vsubname"><asp:Label ID="lblBuilding" AssociatedControlID="txtBuilding" runat="server" /><br /><asp:Label ID="lblEdificio" CssClass="rowes" AssociatedControlID="txtBuilding" runat="server" /></td>
            <td id="tdBuildingMinimum" runat="server"></td>
            <td><asp:CheckBox ID="chkBuilding" Checked="true" onclick="chkActivatedBuilding(this);" runat="server" /></td>
            <td><asp:TextBox ID="txtBuilding" onblur="UpdateValuesAndFormat(this);" runat="server" /></td>
            <td id="tdBuildingMaximum" runat="server"></td>
            <td>
                <asp:CustomValidator ID="valBuilding" Text="Invalid value." Display="Dynamic" ControlToValidate="txtBuilding" ValidateEmptyText="true" ClientValidationFunction="ValidateBuilding" OnServerValidate="valBuilding_ServerValidate" runat="server" />
            </td>
        </tr>
        <tr>
            <td></td>
            <td class="vsubname"><asp:Label ID="lblOtherStr" Text="Other Structures" AssociatedControlID="txtOtherStructures" runat="server" /><br /><asp:Label ID="lblOtherStrEs" Text="Otras estructuras" CssClass="rowes" AssociatedControlID="txtOtherStructures" runat="server" /></td>
            <td id="tdOtherStructuresMinimum" runat="server">$0.00</td>
            <td><asp:CheckBox ID="chkOtherStructures" Checked="true" onclick="chkActivatedOtherStructures(this);" runat="server" /></td>
            <td><asp:TextBox ID="txtOtherStructures" onblur="UpdateValuesAndFormat(this);" runat="server" /></td>
            <td id="tdOtherStructuresMaximum" runat="server"></td>
            <td>
                <asp:CustomValidator ID="valOtherStructures" Text="Invalid value." Display="Dynamic" ControlToValidate="txtOtherStructures" ValidateEmptyText="true" ClientValidationFunction="ValidateOtherStructures" OnServerValidate="valOtherStructures_ServerValidate" runat="server" />
            </td>
        </tr>
        <tr>
            <td><asp:HyperLink ID="lnkOutdoorQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?id=2" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
            <td class="vsubname"><asp:Label ID="lblOutdoor" AssociatedControlID="txtOutdoor" runat="server" /><br /><asp:Label ID="lblExterior" CssClass="rowes" AssociatedControlID="txtOutdoor" runat="server" /></td>
            <td id="tdOutdoorMinimum" runat="server"></td>
            <td><asp:CheckBox ID="chkOutdoor" onclick="chkActivatedOutdoor(this);" runat="server" /></td>
            <td><asp:TextBox ID="txtOutdoor" onblur="FormatMoney(this);" runat="server" /></td>
            <td id="tdOutdoorMaximum" runat="server"></td>
            <td>
                <asp:CustomValidator ID="valOutdoor" Text="Invalid value." Display="Dynamic" ControlToValidate="txtOutdoor" ValidateEmptyText="true" ClientValidationFunction="ValidateOutdoor" OnServerValidate="valOutdoor_ServerValidate" runat="server" />
            </td>
        </tr>
        <tr>
            <td><asp:HyperLink ID="lnkContentsQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?id=3" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
            <td class="vsubname"><asp:Label ID="lblContents" AssociatedControlID="txtContents" runat="server" /><br /><asp:Label ID="lblContenidos" CssClass="rowes" AssociatedControlID="txtContents" runat="server" /></td>
            <td id="tdContentsMinimum" runat="server"></td>
            <td><asp:CheckBox ID="chkContents" Enabled="false" Checked="true" onclick="chkActivatedContents(this);" runat="server" /></td>
            <td><asp:TextBox ID="txtContents" onblur="UpdateValuesAndFormat(this);" runat="server" /></td>
            <td id="tdContentsMaximum" runat="server"></td>
            <td>
                <asp:CustomValidator ID="valContents" Text="Invalid value." Display="Dynamic" ControlToValidate="txtContents" ValidateEmptyText="true" ClientValidationFunction="ValidateContents" OnServerValidate="valContents_ServerValidate" runat="server" />
            </td>
        </tr>
        <tr>
            <td><asp:HyperLink ID="lnkDebriQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?id=4" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
            <td class="vsubname"><asp:Label ID="lblDebri" AssociatedControlID="txtDebri" runat="server" /><br /><asp:Label ID="lblEscombros" CssClass="rowes" AssociatedControlID="txtDebri" runat="server" /></td>
            <td id="tdDebriMinimum" runat="server"></td>
            <td><asp:CheckBox ID="chkDebri" Enabled="false" Checked="true" onclick="chkActivatedDebri(this);" runat="server" /></td>
            <td><asp:TextBox ID="txtDebri" onblur="FormatMoney(this);" runat="server" /></td>
            <td id="tdDebriMaximum" runat="server"></td>
            <td>
                <asp:CustomValidator ID="valDebri" Text="Invalid value." Display="Dynamic" ControlToValidate="txtDebri" ValidateEmptyText="true" ClientValidationFunction="ValidateDebri" OnServerValidate="valDebri_ServerValidate" runat="server" />
            </td>
        </tr>
        <tr>
            <td><asp:HyperLink ID="lnkExtraQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?id=5" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
            <td class="vsubname"><asp:Label ID="lblExtra" AssociatedControlID="txtExtra" runat="server" /><br /><asp:Label ID="lblExtraEs" CssClass="rowes" AssociatedControlID="txtExtra" runat="server" /></td>
            <td id="tdExtraMinimum" runat="server"></td>
            <td><asp:CheckBox ID="chkExtra" Enabled="false" Checked="true" onclick="chkActivatedExtra(this);" runat="server" /></td>
            <td><asp:TextBox ID="txtExtra" onblur="FormatMoney(this);" runat="server" /></td>
            <td id="tdExtraMaximum" runat="server"></td>
            <td>
                <asp:CustomValidator ID="valExtra" Text="Invalid value." Display="Dynamic" ControlToValidate="txtExtra" ValidateEmptyText="true" ClientValidationFunction="ValidateExtra" OnServerValidate="valExtra_ServerValidate" runat="server" />
            </td>
        </tr>
        <tr>
            <td><asp:HyperLink ID="lnkLossRentQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?id=6" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
            <td class="vsubname"><asp:Label ID="lblLossRent" AssociatedControlID="txtLossRents" runat="server" /><br /><asp:Label ID="lblRenta" CssClass="rowes" AssociatedControlID="txtLossRents" runat="server" /></td>
            <td id="tdLossRentsMinimum" runat="server"></td>
            <td><asp:CheckBox ID="chkLossRents" onclick="chkActivatedLossRents(this);" runat="server" /></td>
            <td><asp:TextBox ID="txtLossRents" onblur="FormatMoney(this);" runat="server" /></td>
            <td id="tdLossRentsMaximum" runat="server"></td>
            <td>
                <asp:CustomValidator ID="valLossRents" Text="Invalid value" Display="Dynamic" ControlToValidate="txtLossRents" ValidateEmptyText="true" ClientValidationFunction="ValidateLossRents" OnServerValidate="valLossRents_ServerValidate" runat="server" />
            </td>
        </tr>
    </tbody>
</table>

<table>
    <tr id="lossRentsWarning" style="display: none;" runat="server">
        <td>
            <br />
            <asp:Label ID="lblLossRentWarning" Text="* EARTHQUAKE AND HYDRO-METEOROLOGICAL PHENOMENON ARE EXCLUDED FOR LOSS OF RENT COVERAGE." runat="server" /><br />
            <asp:Label ID="lblLossRentWarningSpanish" Text="TERREMOTOS Y FENÓMENOS HIDROMETEOROLÓGICOS SE EXCLUYEN DE LA COBERTURA DE PÉRDIDA DE RENTAS." CssClass="rowes" runat="server" />
        </td>
    </tr>
</table>
<table cellspacing="10">
<tr>
    <td>
        <div class="row">
            <asp:Label ID="lblOutdoorDescription" Text="Outdoor Property Description: " AssociatedControlID="txtOutdoorDescription" runat="server" />
            <asp:CustomValidator ID="valOutdoorDescription" Display="Dynamic" ValidateEmptyText="true" Text="Required" ControlToValidate="txtOutdoorDescription" ClientValidationFunction="checkOutdoorDescription" OnServerValidate="valOutdoor_Validate" runat="server" />
        </div>
        <div class="rowes" style="clear: both;">
            <asp:Label ID="lblOutdoorDescripcion" Text="Descripci&oacute;n de bienes a la intemperie" runat="server" />
        </div>
        <asp:TextBox ID="txtOutdoorDescription" Columns="50" Rows="8" TextMode="MultiLine" runat="server" />

    </td>
    <td>
        <div class="row">
            <asp:Label ID="lblOSDescription" Text="Other Structures Description: " AssociatedControlID="txtOtherStructuresDescription" runat="server" />
            <asp:CustomValidator ID="valOSDescription" Display="Dynamic" ValidateEmptyText="true" Text="Required" ControlToValidate="txtOtherStructuresDescription" ClientValidationFunction="checkOtherStructuresDescription" OnServerValidate="valOtherStructuresDescription_Validate" runat="server" />
        </div>
        <div class="rowes" style="clear: both;">
            <asp:Label ID="lblOSDescripcion" Text="Descripci&oacute;n de otras estructuras" runat="server" />
        </div>
        <asp:TextBox ID="txtOtherStructuresDescription" Columns="50" Rows="8" TextMode="MultiLine" runat="server" />
    </td>
</tr>
</table>
