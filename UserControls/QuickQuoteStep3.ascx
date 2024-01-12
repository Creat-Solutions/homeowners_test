<%@ Control Language="C#" AutoEventWireup="true" CodeFile="QuickQuoteStep3.ascx.cs" Inherits="UserControls_QuickQuoteStep3" %>
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
    
    if (!(txtBuilding && txtContent)) {
        return;
    }
    var debriMinimum = $get('<%= tdDebriMinimum.ClientID %>');
    var debriMaximum = $get('<%= tdDebriMaximum.ClientID %>');

    var extraMinimum = $get('<%= tdExtraMinimum.ClientID %>');
    var extraMaximum = $get('<%= tdExtraMaximum.ClientID %>');

    var burglaryMinimum = $get('<%= tdBurglaryMinimum.ClientID %>');
    var burglaryMaximum = $get('<%= tdBurglaryMaximum.ClientID %>');

    var jewelryMinimum = $get('<%= tdJewelryMinimum.ClientID %>');
    var jewelryMaximum = $get('<%= tdJewelryMaximum.ClientID %>');

    var electronicsMinimum = $get('<%= tdElectronicMinimum.ClientID %>');
    var electronicsMaximum = $get('<%= tdElectronicMaximum.ClientID %>');


    if (!(debriMinimum && debriMaximum && extraMaximum && extraMaximum &&
          burglaryMinimum && burglaryMaximum && jewelryMinimum && jewelryMaximum &&
          electronicsMinimum && electronicsMaximum)) {
        return;
    }

    var buildingValue = 0;
    if (!txtBuilding.disabled) {
        buildingValue = Number.parseInvariant(txtBuilding.value);
        if (isNaN(buildingValue)) {
            buildingValue = 0;
        }
    }
    
    var contentsValue = Number.parseInvariant(txtContent.value);
    if (isNaN(contentsValue)) {
        contentsValue = 0;
    }

    var sum = buildingValue + contentsValue;
    //hard coded values
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

    if (contentsValue == 0) {
        burglaryMinimum.innerHTML = (Number(percentageMinimum[2])).format("N") + " % ";
        burglaryMaximum.innerHTML = (Number(percentageMaximum[2])).format("N") + " % ";

        jewelryMinimum.innerHTML = (Number(percentageMinimum[3])).format("N") + " % ";
        jewelryMaximum.innerHTML = (Number(percentageMaximum[3])).format("N") + " % ";

        electronicsMinimum.innerHTML = (Number(percentageMinimum[4])).format("N") + " % ";
        electronicsMaximum.innerHTML = (Number(percentageMaximum[4])).format("N") + " % ";
    } else {
        burglaryMinimum.innerHTML = "$" + (Number(percentageMinimum[2]) / 100 * contentsValue).format("N");
        burglaryMaximum.innerHTML = "$" + (Number(percentageMaximum[2]) / 100 * contentsValue).format("N");

        jewelryMinimum.innerHTML = "$" + (Number(percentageMinimum[3]) / 100 * contentsValue).format("N");
        jewelryMaximum.innerHTML = "$" + (Number(percentageMaximum[3]) / 100 * contentsValue).format("N");

        electronicsMinimum.innerHTML = "$" + (Number(percentageMinimum[4]) / 100 * contentsValue).format("N");
        electronicsMaximum.innerHTML = "$" + (Number(percentageMaximum[4]) / 100 * contentsValue).format("N");
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
    chkActivatedClick(obj, '<%= txtLossRents.ClientID %>', '<%= valLossRents.ClientID %>');
}

function checkOutdoorDescription(obj, args)
{
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

function ValidateLimit(obj, args, min, max, percentage, sumForPercentage, emptyAllowed) {
    var valText = 'Invalid value. ';
    var chkLimits = $get('<%= chkLimits.ClientID %>');
    
    if (!chkLimits) {
        args.IsValid = false;
        return;
    }
    
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

        var contentsValue = Number.parseInvariant(contentsLimit.value);
        if (isNaN(contentsValue)) {
            contentsValue = 0;
        }
        
        var sum = 0;
        if (sumForPercentage) {
            var buildingLimit = Number.parseInvariant(buildingLimit.value);
            if (isNaN(buildingLimit)) {
                buildingLimit = 0;
            }
            sum = contentsValue + buildingLimit;
        } else {
            sum = contentsValue;
        }

        if (sum === 0) {
            if (sumForPercentage) {
                valText = valText + 'Please fill the Building and Contents fields.';
            } else {
                valText = valText + 'Please fill the Contents field.';
            }
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

function ValidateBuilding(obj, args) {
    var chkBuilding = $get('<%= chkBuilding.ClientID %>');
    var mins = GetMinimumArray();
    var maxs = GetMaximumArray();
    var minimum = limitsDisabled ? Number.NEGATIVE_INFINITY : Number(mins[0]);
    var maximum = limitsDisabled ? Number.POSITIVE_INFINITY : Number(maxs[0]);
    ValidateLimit(obj, args, minimum, maximum, false, false, !chkBuilding.checked);
}

function ValidateOutdoor(obj, args) {
    var chkOutdoor = $get('<%= chkOutdoor.ClientID %>');
    var mins = GetMinimumArray();
    var maxs = GetMaximumArray();
    ValidateLimit(obj, args, Number(mins[1]), Number(maxs[1]), false, false, !chkOutdoor.checked);
}

function ValidateContents(obj, args) {
    var mins = GetMinimumArray();
    var maxs = GetMaximumArray();
    var minimum = limitsDisabled ? Number.NEGATIVE_INFINITY : Number(mins[2]);
    var maximum = limitsDisabled ? Number.POSITIVE_INFINITY : Number(maxs[2]);
    ValidateLimit(obj, args, minimum, maximum, false, false, false);
}

function ValidateDebri(obj, args) {
    var chkDebri = $get('<%= chkDebri.ClientID %>');
    ValidateLimit(obj, args, Number(percentageMinimum[0]), Number(percentageMaximum[0]), true, true, false);
}

function ValidateExtra(obj, args) {
    var chkExtra = $get('<%= chkExtra.ClientID %>');
    ValidateLimit(obj, args, Number(percentageMinimum[1]), Number(percentageMaximum[1]), true, true, false);
}

function ValidateLossRents(obj, args) {
    var chkLossRents = $get('<%= chkLossRents.ClientID %>');
    var mins = GetMinimumArray();
    var maxs = GetMaximumArray();
    ValidateLimit(obj, args, Number(mins[3]), Number(maxs[3]), false, false, !chkLossRents.checked);
}



function ValidateOtherLimit(obj, args, min, max, contentsFailure, emptyAllowed) {
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

    var valid = value >= min && value <= max;
    if (!valid) {
        if (getContents() === 0 && contentsFailure) {
            valText = valText + 'Please fill the Contents field.';
        } else {
            valText = valText + 'Minimum: $' + min.format("N") + ', Maximum: $' + max.format("N");
        }
        obj.innerHTML = valText;
    }
    args.IsValid = valid;
}

function ValidateCSL(obj, args) {
    var mins = GetMinimumArray();
    var maxs = GetMaximumArray();
    ValidateOtherLimit(obj, args, Number(mins[4]), Number(maxs[4]), false, false);
}

function getContents() {
    var contentsTxt = $get('<%= txtContents.ClientID %>');
    var contents = Number.parseInvariant(contentsTxt.value.replace(/,/g, ''));
    if (isNaN(contents)) {
        return 0;
    }
    return contents;
}

function ValidateBurglary(obj, args) {
    var chkBurglary = $get('<%= chkBurglary.ClientID %>');
    var contents = getContents();
    ValidateOtherLimit(obj, args, Number(percentageMinimum[2]) * contents / 100, Number(percentageMaximum[2]) * contents / 100, true, !chkBurglary.checked);
}

function ValidateJewelry(obj, args) {
    var chkJewelry = $get('<%= chkJewelry.ClientID %>');
    var mins = GetMinimumArray();
    var maxs = GetMaximumArray();
    var contents = getContents();
    ValidateOtherLimit(obj, args, Number(percentageMinimum[3]) * contents / 100, Number(percentageMaximum[3]) * contents / 100, true, !chkJewelry.checked);
}

function ValidateMoney(obj, args) {
    var chkMoney = $get('<%= chkMoney.ClientID %>');
    var mins = GetMinimumArray();
    var maxs = GetMaximumArray();
    ValidateOtherLimit(obj, args, Number(mins[5]), Number(maxs[5]), false, !chkMoney.checked);
}

function ValidateGlass(obj, args) {
    var chkGlass = $get('<%= chkGlass.ClientID %>');
    var mins = GetMinimumArray();
    var maxs = GetMaximumArray();
    ValidateOtherLimit(obj, args, Number(mins[6]), Number(maxs[6]), false, !chkGlass.checked);
}

function ValidateElectronic(obj, args) {
    var chkElectronic = $get('<%= chkElectronic.ClientID %>');
    var mins = GetMinimumArray();
    var maxs = GetMaximumArray();
    var contents = getContents();
    ValidateOtherLimit(obj, args, Number(percentageMinimum[4]) * contents / 100, Number(percentageMaximum[4]) * contents / 100, true, !chkElectronic.checked);
}

function ValidatePersonal(obj, args) {
    var chkPersonal = $get('<%= chkPersonal.ClientID %>');
    var mins = GetMinimumArray();
    var maxs = GetMaximumArray();
    ValidateOtherLimit(obj, args, Number(mins[7]), Number(maxs[7]), false, !chkPersonal.checked);
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

    outdoorMinimum.innerHTML = "$" + Number(mins[1]).format("N");
    outdoorMaximum.innerHTML = "$" + Number(maxs[1]).format("N");
    lossRentsMinimum.innerHTML = "$" + Number(mins[3]).format("N");
    lossRentsMaximum.innerHTML = "$" + Number(maxs[3]).format("N");

    $('#<%= tdCSLMinimum.ClientID %>').html(Number(mins[4]).format("N"));
    $('#<%= tdCSLMaximum.ClientID %>').html(Number(maxs[4]).format("N"));

    $('#<%= tdMoneyMinimum.ClientID %>').html(Number(mins[5]).format("N"));
    $('#<%= tdMoneyMaximum.ClientID %>').html(Number(maxs[5]).format("N"));

    $('#<%= tdGlassMinimum.ClientID %>').html(Number(mins[6]).format("N"));
    $('#<%= tdGlassMaximum.ClientID %>').html(Number(maxs[6]).format("N"));

    $('#<%= tdPersonalMinimum.ClientID %>').html(Number(mins[7]).format("N"));
    $('#<%= tdPersonalMaximum.ClientID %>').html(Number(maxs[7]).format("N"));
   
    Page_ClientValidate();
}

function StartCurrency(cDollars) {
    currencyDollars = cDollars;
}

function changeCovered(lblLimit, covered) {
    if (covered) {
        lblLimit.innerHTML = "COVERED / CUBIERTO";
    } else {
        lblLimit.innerHTML = "EXCLUDED / EXCLU&Iacute;DO";
    }
}

function chkActivatedClick(obj, txtID, valID) {
    var txtLimit = $get(txtID);
    var valLimit = $get(valID);

    if (!(txtLimit && obj && valLimit)) {
        return;
    }

    txtLimit.value = '';
    txtLimit.disabled = !obj.checked;
    ValidatorValidate(valLimit);
}

var primaryResidence = true;

function ToggleActivationJewelryMedical(obj) {
    if (primaryResidence) {
        chkActivatedJewelry(obj);
        $get('<%= chkJewelry.ClientID %>').disabled = !obj.checked;
        $get('<%= chkJewelry.ClientID %>').checked = false;
        $get('<%= txtJewelry.ClientID %>').disabled = true;
    }
}

function chkActivatedBurglary(obj) {
    chkActivatedClick(obj, '<%= txtBurglary.ClientID %>', '<%= valBurglary.ClientID %>');
    ToggleActivationJewelryMedical(obj);
}

function chkActivatedJewelry(obj) {
    chkActivatedClick(obj, '<%= txtJewelry.ClientID %>', '<%= valJewelry.ClientID %>');
}

function chkActivatedMoney(obj) {
    chkActivatedClick(obj, '<%= txtMoney.ClientID %>', '<%= valMoney.ClientID %>');
}

function chkActivatedGlass(obj) {
    chkActivatedClick(obj, '<%= txtGlass.ClientID %>', '<%= valGlass.ClientID %>');
}

function chkActivatedElectronic(obj) {
    chkActivatedClick(obj, '<%= txtElectronic.ClientID %>', '<%= valElectronic.ClientID %>');
}

function chkActivatedPersonal(obj) {
    chkActivatedClick(obj, '<%= txtPersonal.ClientID %>', '<%= valPersonal.ClientID %>');
}

function chkActivatedFamily(obj) {
    var lblFamily = $get('<%= lblFamilyCovered.ClientID %>');
    changeCovered(lblFamily, obj.checked);
}

function chkTechSupport(obj) {
    var lblTech = $get('<%= lblTechSupportCovered.ClientID %>');
    changeCovered(lblTech, obj.checked);
}

function SelectOnFocus() {
    if (Number($(this).val()) === 0) {
        var txt = this;
        setTimeout( function() {
            txt.select();
        }, 100);
    }
}

$(function() {
    $('.limitValue').focus(SelectOnFocus).click(SelectOnFocus);
    $('#<%= chkOtherLimits.ClientID %>').click(function() {
        if (this.checked) {
            $('#<%= divOtherLimits.ClientID %>').show();
        } else {
            $('#<%= divOtherLimits.ClientID %>').hide();
        }
    });
});


</script>

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
            <td><asp:TextBox ID="txtBuilding" onblur="UpdateValuesAndFormat(this);" runat="server" CssClass="limitValue" /></td>
            <td id="tdBuildingMaximum" runat="server"></td>
            <td>
                <asp:CustomValidator ID="valBuilding" Text="Invalid value." Display="Dynamic" ControlToValidate="txtBuilding" ValidateEmptyText="true" ClientValidationFunction="ValidateBuilding" OnServerValidate="valBuilding_ServerValidate" runat="server" />
            </td>
        </tr>
        <tr>
            <td><asp:HyperLink ID="lnkOutdoorQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?id=2" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
            <td class="vsubname"><asp:Label ID="lblOutdoor" AssociatedControlID="txtOutdoor" runat="server" /><br /><asp:Label ID="lblExterior" CssClass="rowes" AssociatedControlID="txtOutdoor" runat="server" /></td>
            <td id="tdOutdoorMinimum" runat="server"></td>
            <td><asp:CheckBox ID="chkOutdoor" onclick="chkActivatedOutdoor(this);" runat="server" /></td>
            <td><asp:TextBox ID="txtOutdoor" onblur="FormatMoney(this);" CssClass="limitValue" runat="server" /></td>
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
            <td><asp:TextBox ID="txtContents" onblur="UpdateValuesAndFormat(this);" CssClass="limitValue" runat="server" /></td>
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
            <td><asp:TextBox ID="txtDebri" onblur="FormatMoney(this);" CssClass="limitValue" runat="server" /></td>
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
            <td><asp:TextBox ID="txtExtra" onblur="FormatMoney(this);" CssClass="limitValue" runat="server" /></td>
            <td id="tdExtraMaximum" runat="server"></td>
            <td>
                <asp:CustomValidator ID="valExtra" Text="Invalid value." Display="Dynamic" ControlToValidate="txtExtra" ValidateEmptyText="true" ClientValidationFunction="ValidateExtra" OnServerValidate="valExtra_ServerValidate" runat="server" />
            </td>
        </tr>
       
        <tr>
            <td><asp:HyperLink ID="lnkCSLQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?coverage=1&id=1" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
            <td class="vsubname"><asp:Label ID="lblCSL" AssociatedControlID="txtCSL" runat="server" /><br /><asp:Label ID="lblCSLEs" CssClass="rowes" AssociatedControlID="txtCSL" runat="server" /></td>
            <td id="tdCSLMinimum" runat="server"></td>
            <td><asp:CheckBox ID="CheckBox1" Enabled="false" Checked="true" runat="server" /></td>
            <td><asp:TextBox ID="txtCSL" onblur="FormatMoney(this);" CssClass="limitValue" runat="server" /></td>
            <td id="tdCSLMaximum" runat="server"></td>
            <td>
                <asp:CustomValidator ID="valCSL" Text="Invalid value." Display="Dynamic" ControlToValidate="txtCSL" ValidateEmptyText="true" ClientValidationFunction="ValidateCSL" OnServerValidate="valCSL_ServerValidate" runat="server" />
            </td>
        </tr>
        
        <tr>
            <td><asp:HyperLink ID="lnkBurglaryQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?coverage=1&id=3" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
            <td class="vsubname"><asp:Label ID="lblBurglary" AssociatedControlID="txtBurglary" runat="server" /><br /><asp:Label ID="lblRobo" CssClass="rowes" AssociatedControlID="txtBurglary" runat="server" /></td>
            <td id="tdBurglaryMinimum" runat="server"></td>
            <td><asp:CheckBox ID="chkBurglary" onclick="chkActivatedBurglary(this);" runat="server" /></td>
            <td><asp:TextBox ID="txtBurglary" onblur="FormatMoney(this);" CssClass="limitValue" runat="server" /></td>
            <td id="tdBurglaryMaximum" runat="server"></td>
            <td>
                <asp:CustomValidator ID="valBurglary" Text="Invalid value." Display="Dynamic" ControlToValidate="txtBurglary" ValidateEmptyText="true" ClientValidationFunction="ValidateBurglary" OnServerValidate="valBurglary_ServerValidate" runat="server" />
            </td>
        </tr>
        <tr>
            <td><asp:HyperLink ID="lnkJewelryQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?coverage=1&id=4" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
            <td class="vsubname"><asp:Label ID="lblJewelry" AssociatedControlID="txtJewelry" runat="server" /><br /><asp:Label ID="lblJoyas" CssClass="rowes" AssociatedControlID="txtJewelry" runat="server" /></td>
            <td id="tdJewelryMinimum" runat="server"></td>
            <td><asp:CheckBox ID="chkJewelry" onclick="chkActivatedJewelry(this);" runat="server" /></td>
            <td><asp:TextBox ID="txtJewelry" onblur="FormatMoney(this);" CssClass="limitValue" runat="server" /></td>
            <td id="tdJewelryMaximum" runat="server"></td>
            <td>
                <asp:CustomValidator ID="valJewelry" Text="Invalid value." Display="Dynamic" ControlToValidate="txtJewelry" ValidateEmptyText="true" ClientValidationFunction="ValidateJewelry" OnServerValidate="valJewelry_ServerValidate" runat="server" />
            </td>
        </tr>
        
        <tr>
            <td><asp:HyperLink ID="lnkGlassQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?coverage=1&id=6" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
            <td class="vsubname"><asp:Label ID="lblGlass" AssociatedControlID="txtGlass" runat="server" /><br /><asp:Label ID="lblCristales" CssClass="rowes" AssociatedControlID="txtGlass" runat="server" /></td>
            <td id="tdGlassMinimum" runat="server"></td>
            <td><asp:CheckBox ID="chkGlass" onclick="chkActivatedGlass(this);" runat="server" /></td>
            <td><asp:TextBox ID="txtGlass" onblur="FormatMoney(this);" CssClass="limitValue" runat="server" /></td>
            <td id="tdGlassMaximum" runat="server"></td>
            <td>
                <asp:CustomValidator ID="valGlass" Text="Invalid value" Display="Dynamic" ControlToValidate="txtGlass" ValidateEmptyText="true" ClientValidationFunction="ValidateGlass" OnServerValidate="valGlass_ServerValidate" runat="server" />
            </td>
        </tr>
       
        
        
        
    </tbody>
</table>

<asp:CheckBox ID="chkOtherLimits" runat="server" />
<asp:Label ID="lblOtherLimits" Text="Other Limits / Otros l&iacute;mites" AssociatedControlID="chkOtherLimits" runat="server" />

<div id="divOtherLimits" runat="server">
    

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
                <td><asp:HyperLink ID="lnkLossRentQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?id=6" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
                <td class="vsubname"><asp:Label ID="lblLossRent" AssociatedControlID="txtLossRents" runat="server" /><br /><asp:Label ID="lblRenta" CssClass="rowes" AssociatedControlID="txtLossRents" runat="server" /></td>
                <td id="tdLossRentsMinimum" runat="server"></td>
                <td><asp:CheckBox ID="chkLossRents" onclick="chkActivatedLossRents(this);" runat="server" /></td>
                <td><asp:TextBox ID="txtLossRents" onblur="FormatMoney(this);" CssClass="limitValue" runat="server" /></td>
                <td id="tdLossRentsMaximum" runat="server"></td>
                <td>
                    <asp:CustomValidator ID="valLossRents" Text="Invalid value" Display="Dynamic" ControlToValidate="txtLossRents" ValidateEmptyText="true" ClientValidationFunction="ValidateLossRents" OnServerValidate="valLossRents_ServerValidate" runat="server" />
                </td>
            </tr>
            <tr>
                <td><asp:HyperLink ID="lnkDomesticQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?coverage=1&id=2" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
                <td class="vsubname"><asp:Label ID="lblDomestic" runat="server" /><br /><asp:Label ID="lblDomesticEs" CssClass="rowes" runat="server" /></td>
                <td id="tdDomesticMinimum" runat="server"></td>
                <td><asp:CheckBox ID="chkDomestic" Checked="true" Enabled="false" runat="server" /></td>
                <td><asp:Label ID="lblDomesticCoverd" Text="COVERED / CUBIERTO" runat="server" /></td>
                <td id="tdDomesticMaximum" runat="server"></td>
                <td>
                </td>
            </tr>
            <tr>
                <td><asp:HyperLink ID="lnkMoneyQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?coverage=1&id=5" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
                <td class="vsubname"><asp:Label ID="lblMoney" AssociatedControlID="txtMoney" runat="server" /><br /><asp:Label ID="lblDinero" CssClass="rowes" AssociatedControlID="txtMoney" runat="server" /></td>
                <td id="tdMoneyMinimum" runat="server"></td>
                <td><asp:CheckBox ID="chkMoney" onclick="chkActivatedMoney(this);" runat="server" /></td>
                <td><asp:TextBox ID="txtMoney" onblur="FormatMoney(this);" CssClass="limitValue" runat="server" /></td>
                <td id="tdMoneyMaximum" runat="server"></td>
                <td>
                    <asp:CustomValidator ID="valMoney" Text="Invalid value." Display="Dynamic" ControlToValidate="txtMoney" ValidateEmptyText="true" ClientValidationFunction="ValidateMoney" OnServerValidate="valMoney_ServerValidate" runat="server" />
                </td>
             </tr>
             <tr>
                <td><asp:HyperLink ID="lnkElectronicQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?coverage=1&id=7" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
                <td class="vsubname"><asp:Label ID="lblElectronic" AssociatedControlID="txtElectronic" runat="server" /><br /><asp:Label ID="lblElectronico" CssClass="rowes" AssociatedControlID="txtElectronic" runat="server" /></td>
                <td id="tdElectronicMinimum" runat="server"></td>
                <td><asp:CheckBox ID="chkElectronic" onclick="chkActivatedElectronic(this);" runat="server" /></td>
                <td><asp:TextBox ID="txtElectronic" onblur="FormatMoney(this);" CssClass="limitValue" runat="server" /></td>
                <td id="tdElectronicMaximum" runat="server"></td>
                <td>
                    <asp:CustomValidator ID="valElectronic" Text="Invalid value" Display="Dynamic" ControlToValidate="txtElectronic" ValidateEmptyText="true" ClientValidationFunction="ValidateElectronic" OnServerValidate="valElectronic_ServerValidate" runat="server" />
                </td>
            </tr>
            <tr>
                <td><asp:HyperLink ID="lnkPersonalQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?coverage=1&id=8" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
                <td class="vsubname"><asp:Label ID="lblPersonal" AssociatedControlID="txtPersonal" runat="server" /><br /><asp:Label ID="lblPersonalEs" CssClass="rowes" AssociatedControlID="txtPersonal" runat="server" /></td>
                <td id="tdPersonalMinimum" runat="server"></td>
                <td><asp:CheckBox ID="chkPersonal" onclick="chkActivatedPersonal(this);" runat="server" /></td>
                <td><asp:TextBox ID="txtPersonal" onblur="FormatMoney(this);" CssClass="limitValue" runat="server" /></td>
                <td id="tdPersonalMaximum" runat="server"></td>
                <td>
                    <asp:CustomValidator ID="valPersonal" Text="Invalid value" Display="Dynamic" ControlToValidate="txtPersonal" ValidateEmptyText="true" ClientValidationFunction="ValidatePersonal" OnServerValidate="valPersonal_ServerValidate" runat="server" />
                </td>
            </tr>
            <tr>
                <td><asp:HyperLink ID="lnkFamilyQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?coverage=1&id=9" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
                <td class="vsubname"><asp:Label ID="lblFamily" runat="server" /><br /><asp:Label ID="lblFamilia" CssClass="rowes" runat="server" /></td>
                <td id="tdFamilyMinimum" runat="server"></td>
                <td><asp:CheckBox ID="chkFamily" onclick="chkActivatedFamily(this);" runat="server" /></td>
                <td><asp:Label ID="lblFamilyCovered" Text="COVERED / CUBIERTO" runat="server" /></td>
                <td id="tdFamilyMaximum" runat="server"></td>
                <td></td>            
            </tr>
            <tr>
                <td><asp:HyperLink ID="lnkTechSupport" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?coverage=1&id=10" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
                <td class="vsubname"><asp:Label ID="lblTechSupport" runat="server" /><br /><asp:Label ID="lblAsistenciaInformatica" CssClass="rowes" runat="server" /></td>
                <td id="tdTechSupportMinimum" runat="server"></td>
                <td><asp:CheckBox ID="chkTechSupport" onclick="chkTechSupport(this);" runat="server" /></td>
                <td><asp:Label ID="lblTechSupportCovered" Text="COVERED / CUBIERTO" runat="server" /></td>
                <td id="tdTechSupportMaximum" runat="server"></td>
                <td></td>            
            </tr>
        </tbody>
    </table>

</div>
<br />
<br />
<div class="row">
    <asp:Label ID="lblOutdoorDescription" Text="Outdoor Property Description: " AssociatedControlID="txtOutdoorDescription" runat="server" />
    <asp:CustomValidator ID="valOutdoorDescription" Display="Dynamic" ValidateEmptyText="true" Text="Required" ControlToValidate="txtOutdoorDescription" ClientValidationFunction="checkOutdoorDescription" OnServerValidate="valOutdoor_Validate" runat="server" />
</div>
<div class="rowes" style="clear: both;">
    <asp:Label ID="lblOutdoorDescripcion" Text="Descripci&oacute;n de bienes a la intemperie" runat="server" />
</div>
<asp:TextBox ID="txtOutdoorDescription" Columns="50" Rows="8" TextMode="MultiLine" runat="server" />
   
   
   
<script type="text/javascript">

    
    
</script>



   
