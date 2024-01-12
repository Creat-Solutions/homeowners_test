<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GenerateStep3.ascx.cs" Inherits="UserControls_GenerateStep3" %>

<script type="text/javascript">
	var limitsDisabled = false;

	var currencyDollars = '<%= CURRENCY_DOLLARS ? "true" : "false"%>';

	function GetMinimumArray()
	{
		return currencyDollars ? valuesMinimum : valuesPesosMinimum;
	}

	function GetMaximumArray()
	{
		return currencyDollars ? valuesMaximum : valuesPesosMaximum;
	}

	var isNav;
	function checkKeyInt(e)
	{
		var keyChar;

		if (isNav == 0)
		{
			if (e.which < 48 || e.which > 57) e.RETURNVALUE = false;
		} else
		{
			if (e.keyCode < 48 || e.keyCode > 57) e.returnValue = false;
		}
	}
	function ToggleValidators()
	{

		var buildingMinimum = $get('<%= tdBuildingMinimum.ClientID %>');
	var buildingMaximum = $get('<%= tdBuildingMaximum.ClientID %>');

	var contentsMinimum = $get('<%= tdContentsMinimum.ClientID %>');
	var contentsMaximum = $get('<%= tdContentsMaximum.ClientID %>');

		var mins = GetMinimumArray();
		var maxs = GetMaximumArray();
		buildingMinimum.innerHTML = "$" + Number(mins[0]).format("N");
		buildingMaximum.innerHTML = "$" + Number(maxs[0]).format("N");
		contentsMinimum.innerHTML = "$" + Number(mins[2]).format("N");
		contentsMaximum.innerHTML = "$" + Number(maxs[2]).format("N");
	}

	function UpdateValues()
	{
		var txtBuilding = $get('<%= txtBuilding.ClientID %>');
	var txtContent = $get('<%= txtContents.ClientID %>');
	var txtOtherStructures = $('#<%= txtOtherStructures.ClientID %>')[0];

	if (!(txtBuilding && txtContent))
	{
		return;
	}
	var debrisMinimum = $get('<%= tdDebrisMinimum.ClientID %>');
	var debrisMaximum = $get('<%= tdDebrisMaximum.ClientID %>');

	var extraMinimum = $get('<%= tdExtraMinimum.ClientID %>');
	var extraMaximum = $get('<%= tdExtraMaximum.ClientID %>');

	var burglaryMinimum = $get('<%= tdBurglaryMinimum.ClientID %>');
	var burglaryMaximum = $get('<%= tdBurglaryMaximum.ClientID %>');

	var jewelryMinimum = $get('<%= tdJewelryMinimum.ClientID %>');
	var jewelryMaximum = $get('<%= tdJewelryMaximum.ClientID %>');

	var electronicMinimum = $get('<%= tdElectronicMinimum.ClientID %>');
	var electronicMaximum = $get('<%= tdElectronicMaximum.ClientID %>');

	if (!(debrisMinimum && debrisMaximum && extraMaximum && extraMaximum))
	{
		return;
	}

	var buildingValue = 0;
	buildingValue = Number.parseInvariant(txtBuilding.value);
	if (isNaN(buildingValue))
	{
		buildingValue = 0;
	}
	var buildingMax = $('#<%= tdBuildingMaximum.ClientID %>');
	var maxValue = GetMaximumArray()[0];
	var otherStructuresValue = 0;
	if (!txtOtherStructures.disabled)
	{
		otherStructuresValue = Number.parseInvariant(txtOtherStructures.value);
		if (isNaN(otherStructuresValue))
		{
			otherStructuresValue = 0;
		}
		maxValue -= otherStructuresValue;
	}
	buildingMax.html('$' + maxValue.format('N'));

	var contentsValue = Number.parseInvariant(txtContent.value);
	if (isNaN(contentsValue))
	{
		contentsValue = 0;
	}

	var otherStructuresMax = $('#<%= tdOtherStructuresMaximum.ClientID %>');
		var maxs = GetMaximumArray();
		otherStructuresMax.html("$" + (maxs[0] - buildingValue).format("N"));

		var sum = buildingValue + otherStructuresValue + contentsValue;
		if (sum === 0)
		{
			debrisMinimum.innerHTML = Number(percentageMinimum[0]).format("N") + " % ";
			debrisMaximum.innerHTML = Number(percentageMaximum[0]).format("N") + " % ";
			extraMinimum.innerHTML = Number(percentageMinimum[1]).format("N") + " % ";
			extraMaximum.innerHTML = Number(percentageMaximum[1]).format("N") + " % ";
		} else
		{
			debrisMinimum.innerHTML = "$" + (Number(percentageMinimum[0]) / 100 * sum).format("N");
			debrisMaximum.innerHTML = "$" + (Number(percentageMaximum[0]) / 100 * sum).format("N");
			extraMinimum.innerHTML = "$" + (Number(percentageMinimum[1]) / 100 * sum).format("N");
			extraMaximum.innerHTML = "$" + (Number(percentageMaximum[1]) / 100 * sum).format("N");
		}

		if (contentsValue === 0)
		{
			burglaryMinimum.innerHTML = Number(percentageMinimum[0]).format("N") + " % ";
			burglaryMaximum.innerHTML = Number(percentageMaximum[0]).format("N") + " % ";
			jewelryMinimum.innerHTML = Number(percentageMinimum[1]).format("N") + " % ";
			jewelryMaximum.innerHTML = Number(percentageMaximum[1]).format("N") + " % ";
			electronicMinimum.innerHTML = Number(percentageMinimum[2]).format("N") + " % ";
			electronicMaximum.innerHTML = Number(percentageMaximum[2]).format("N") + " % ";
		}
		else
		{
			burglaryMinimum.innerHTML = "$" + (Number(additionalPercentageMaximum[0]) / 100 * contentsValue).format("N");
			burglaryMaximum.innerHTML = "$" + (Number(additionalPercentageMaximum[0]) / 100 * contentsValue).format("N");
			jewelryMinimum.innerHTML = "$" + (Number(additionalPercentageMaximum[1]) / 100 * contentsValue).format("N");
			jewelryMaximum.innerHTML = "$" + (Number(additionalPercentageMaximum[1]) / 100 * contentsValue).format("N");
			electronicMinimum.innerHTML = "$" + (Number(additionalPercentageMaximum[2]) / 100 * contentsValue).format("N");
			electronicMaximum.innerHTML = "$" + (Number(additionalPercentageMaximum[2]) / 100 * contentsValue).format("N");
		}
	}

	function changeOutdoor(sobj)
	{
		if (!sobj)
		{
			return true;
		}
		var valOutdoor = $get('<%= this.valOutdoor.ClientID %>');
	var txtOutdoor = $get('<%= this.txtOutdoorDescription.ClientID %>');
		if (!(valOutdoor && txtOutdoor))
		{
			return true;
		}
		if (!sobj.checked)
		{
			txtOutdoor.value = '';
		}
		txtOutdoor.disabled = !sobj.checked;
		ValidatorEnable(valOutdoor, sobj.checked);
	}

	function changeOtherStructures(sobj)
	{
		if (!sobj)
		{
			return;
		}
		var valOtherStructures = $('#<%= valOSDescription.ClientID %>')[0];
	var txtOtherStructures = $('#<%= txtOtherStructuresDescription.ClientID %>')[0];
		if (!(valOtherStructures && txtOtherStructures))
		{
			return;
		}
		if (!sobj.checked)
		{
			txtOtherStructures.value = '';
		}
		txtOtherStructures.disabled = !sobj.checked;
		ValidatorEnable(valOtherStructures, sobj.checked);
	}

	function chkActivatedClick(obj, txtID, valID)
	{
		var txtLimit = $get(txtID);
		var valLimit = $get(valID);

		if (!(txtLimit && obj && valLimit))
		{
			return;
		}

		txtLimit.value = '';
		txtLimit.disabled = !obj.checked;
		UpdateValues();
		ValidatorValidate(valLimit);
	}

	function chkActivatedBuilding(obj)
	{
		chkActivatedClick(obj, '<%= txtBuilding.ClientID %>', '<%= valBuilding.ClientID %>');
	if (!obj.checked)
	{
		var chkOtherStructures = $('#<%= chkOtherStructures.ClientID %>')[0];
			chkOtherStructures.disabled = true;
			chkActivatedOtherStructures(chkOtherStructures);
		}

	}

	function chkActivatedOtherStructures(obj)
	{
		chkActivatedClick(obj, '<%= txtOtherStructures.ClientID %>', '<%= valOtherStructures.ClientID %>');
		changeOtherStructures(obj);
	}

	function chkActivatedOutdoor(obj)
	{
		chkActivatedClick(obj, '<%= txtOutdoor.ClientID %>', '<%= valOutdoor.ClientID %>');
		changeOutdoor(obj);
	}

	function chkActivatedContents(obj)
	{
		chkActivatedClick(obj, '<%= txtContents.ClientID %>', '<%= valContents.ClientID %>');
	}

	function chkActivatedDebris(obj)
	{
		chkActivatedClick(obj, '<%= txtDebris.ClientID %>', '<%= valDebris.ClientID %>');
	}

	function chkActivatedExtra(obj)
	{
		chkActivatedClick(obj, '<%= txtExtra.ClientID %>', '<%= valExtra.ClientID %>');
	}

	function chkActivatedLossRents(obj)
	{
		toggleLossOfRentsWithHmpQuake();
		chkActivatedClick(obj, '<%= txtLossRents.ClientID %>', '<%= valLossRents.ClientID %>');
	}

	function toggleLossOfRentsWithHmpQuake()
	{
		var quake = $get('<%= chkQuake.ClientID %>');
	var hmp = $get('<%= chkHMP.ClientID %>');
	var lossOfRents = $get('<%= chkLossRents.ClientID %>');
	var rowLossRentWarning = $get('<%= lossRentsWarning.ClientID %>');
		if (!(quake && lossOfRents && hmp))
		{
			return;
		}

		if ((quake.checked || hmp.checked) && lossOfRents.checked)
		{
			rowLossRentWarning.style.display = 'inline';
		}
		else
		{
			rowLossRentWarning.style.display = 'none';
		}
	}

	function checkOtherStructuresDescription(obj, args)
	{
		var chkOtherStructures = $get('<%= chkOtherStructures.ClientID %>');
	var txtOtherStructures = $get('<%= txtOtherStructuresDescription.ClientID %>');
		if (!(chkOtherStructures && txtOtherStructures))
		{
			args.IsValid = false;
			return;
		}
		args.IsValid = !chkOtherStructures.checked || txtOtherStructures.value.length > 0;
	}

	function checkOutdoorDescription(obj, args)
	{
		var chkOutdoor = $get('<%= chkOutdoor.ClientID %>');
	var txtOutdoor = $get('<%= txtOutdoorDescription.ClientID %>');
		if (!(chkOutdoor && txtOutdoor))
		{
			args.IsValid = false;
			return;
		}
		args.IsValid = !chkOutdoor.checked || txtOutdoor.value.length > 0;
	}

	function UpdateValuesAndFormat(obj)
	{
		UpdateValues();
		FormatMoney(obj);
	}

	function ValidateLimitWithPercentage(obj, args, min, max, percentage, emptyAllowed)
	{
		var valText = '<%= GetLocalResourceObject("strInvalidValue").ToString() %>';

	var value = null;
	if (args.Value === null || args.Value === '')
	{
		args.IsValid = emptyAllowed;
		if (!emptyAllowed)
		{
			obj.innerHTML = '<%= GetLocalResourceObject("strRequired").ToString() %>';
		}
		return;
	}

	value = Number.parseInvariant(args.Value);
	if (isNaN(value))
	{
		args.IsValid = false;
		return;
	}

	var minimum = 0;
	var maximum = 0;
	if (percentage)
	{
		var buildingLimit = $get('<%= txtBuilding.ClientID %>');
			var contentsLimit = $get('<%= txtContents.ClientID %>');
			if (!(buildingLimit && contentsLimit))
			{
				args.IsValid = false;
				return;
			}

			var buildingValue = Number.parseInvariant(buildingLimit.value);
			if (isNaN(buildingValue))
			{
				buildingValue = 0;
			}

			var sum = buildingValue + Number.parseInvariant(contentsLimit.value);
			if (sum === 0)
			{
				valText = valText + 'Please fill the Building and Contents fields.';
				obj.innerHTML = valText;
				args.IsValid = false;
				return;
			}
			minimum = sum * min / 100;
			maximum = sum * max / 100;
		} else
		{
			minimum = min;
			maximum = max;
		}

		var valid = value >= minimum && value <= maximum;
		if (!valid)
		{
			valText = valText + 'Min: $' + minimum.format("N") + ', Max: $' + maximum.format("N");
			obj.innerHTML = valText;
		}
		args.IsValid = valid;
	}

	function ValidateOtherStructures(obj, args)
	{
		var maxs = GetMaximumArray();
		var chkOtherStructures = $get('<%= chkOtherStructures.ClientID %>');
	var minimum = 0;
	var buildingLimit = Number.parseInvariant($('#<%= txtBuilding.ClientID %>').val());
		if (isNaN(buildingLimit))
		{
			buildingLimit = 0;
		}
		var maximum = limitsDisabled ? Number.POSITIVE_INFINITY : Number(maxs[0]) - buildingLimit;
		ValidateLimitWithPercentage(obj, args, minimum, maximum, false, !chkOtherStructures.checked);
	}

	function ValidateBuilding(obj, args)
	{
		var chkBuilding = $get('<%= chkBuilding.ClientID %>');
	var chkOtherStructures = $get('<%= chkOtherStructures.ClientID %>');
	var mins = GetMinimumArray();
	var maxs = GetMaximumArray();
	var minimum = limitsDisabled ? Number.NEGATIVE_INFINITY : Number(mins[0]);
	var maximum = limitsDisabled ? Number.POSITIVE_INFINITY : Number(maxs[0]);
	if (!chkOtherStructures.disabled)
	{
		var otherStructures = Number.parseInvariant($('#<%= txtOtherStructures.ClientID %>').val());
			if (!isNaN(otherStructures))
			{
				maximum -= otherStructures;
			}
		}
		ValidateLimitWithPercentage(obj, args, minimum, maximum, false, !chkBuilding.checked);
	}

	function ValidateOutdoor(obj, args)
	{
		var chkOutdoor = $get('<%= chkOutdoor.ClientID %>');
		var mins = GetMinimumArray();
		var maxs = GetMaximumArray();
		ValidateLimitWithPercentage(obj, args, Number(mins[1]), Number(maxs[1]), false, !chkOutdoor.checked);
	}

	function ValidateContents(obj, args)
	{
		var mins = GetMinimumArray();
		var maxs = GetMaximumArray();
		var minimum = limitsDisabled ? Number.NEGATIVE_INFINITY : Number(mins[2]);
		var maximum = limitsDisabled ? Number.POSITIVE_INFINITY : Number(maxs[2]);
		ValidateLimitWithPercentage(obj, args, minimum, maximum, false, false);
	}

	function ValidateDebris(obj, args)
	{
		var chkDebris = $get('<%= chkDebris.ClientID %>');
		ValidateLimitWithPercentage(obj, args, Number(percentageMinimum[0]), Number(percentageMaximum[0]), true, false);
	}

	function ValidateExtra(obj, args)
	{
		var chkExtra = $get('<%= chkExtra.ClientID %>');
		ValidateLimitWithPercentage(obj, args, Number(percentageMinimum[1]), Number(percentageMaximum[1]), true, false);
	}

	function ValidateLossRents(obj, args)
	{
		var chkLossRents = $get('<%= chkLossRents.ClientID %>');
		var mins = GetMinimumArray();
		var maxs = GetMaximumArray();
		ValidateLimitWithPercentage(obj, args, Number(mins[3]), Number(maxs[3]), false, !chkLossRents.checked);
	}

	function StartCurrency(cDollars)
	{
		currencyDollars = cDollars;
	}

	function chkQuake_Click()
	{
		toggleLossOfRentsWithHmpQuake();
	}

	function chkHMP_Click(obj)
	{
		if (!obj)
		{
			return;
		}
		var chkOutdoor = $get('<%= chkOutdoor.ClientID %>');
		if (chkOutdoor.parentNode.tagName.toLowerCase() == 'span')
		{
			chkOutdoor.parentNode.disabled = !obj.checked;
		}
		chkOutdoor.disabled = !obj.checked;
		chkOutdoor.checked = false;
		chkActivatedOutdoor(chkOutdoor);
		toggleLossOfRentsWithHmpQuake();
	}
	function changeCovered(lblLimit, covered)
	{
		if (covered)
		{
			lblLimit.innerHTML = '<%= GetLocalResourceObject("strCovered").ToString() %>';
				} else
				{
					lblLimit.innerHTML = '<%= GetLocalResourceObject("strExcluded").ToString() %>';
		}
	}

	function chkActivatedClick(obj, txtID, valID)
	{
		var txtLimit = $get(txtID);
		var valLimit = $get(valID);

		if (!(txtLimit && obj && valLimit))
		{
			return;
		}

		txtLimit.value = '';
		txtLimit.disabled = !obj.checked;
		ValidatorValidate(valLimit);
	}

	var primaryResidence = true;

	function ToggleActivationJewelryMedical(obj)
	{
		if (primaryResidence)
		{
			chkActivatedJewelry(obj);
			$get('<%= chkJewelry.ClientID %>').disabled = !obj.checked;
					$get('<%= chkJewelry.ClientID %>').checked = false;
					$get('<%= txtJewelry.ClientID %>').disabled = true;
		}
	}

	function chkActivatedBurglary(obj)
	{
		chkActivatedClick(obj, '<%= txtBurglary.ClientID %>', '<%= valBurglary.ClientID %>');
		ToggleActivationJewelryMedical(obj);
	}

	function chkActivatedJewelry(obj)
	{
		chkActivatedClick(obj, '<%= txtJewelry.ClientID %>', '<%= valJewelry.ClientID %>');
	}

	function chkActivatedMoney(obj)
	{
		chkActivatedClick(obj, '<%= txtMoney.ClientID %>', '<%= valMoney.ClientID %>');
	}

	function chkActivatedGlass(obj)
	{
		chkActivatedClick(obj, '<%= txtGlass.ClientID %>', '<%= valGlass.ClientID %>');
	}

	function chkActivatedElectronic(obj)
	{
		chkActivatedClick(obj, '<%= txtElectronic.ClientID %>', '<%= valElectronic.ClientID %>');
	}

	function chkActivatedPersonal(obj)
	{
		chkActivatedClick(obj, '<%= txtPersonal.ClientID %>', '<%= valPersonal.ClientID %>');
	}

	function chkActivatedFamily(obj)
	{
		var lblFamily = $get('<%= lblFamilyCovered.ClientID %>');
		changeCovered(lblFamily, obj.checked);
	}

	function chkTechSupport(obj)
	{
		var lblTech = $get('<%= lblTechSupportCovered.ClientID %>');
		changeCovered(lblTech, obj.checked);
	}

	function ValidateLimit(obj, args, min, max, percentage, emptyAllowed)
	{
		var valText = '';
		var value = null;

		if (args.Value === null || args.Value === '')
		{
			args.IsValid = emptyAllowed;
			if (!emptyAllowed)
			{
				obj.innerHTML = '<%= GetLocalResourceObject("strRequired").ToString() %>';
				}
				return;
			}

			value = Number.parseInvariant(args.Value);
			if (isNaN(value))
			{
				args.IsValid = false;
				return;
			}

			if (percentage)
			{
				var contentsLimit = $get('<%= txtContents.ClientID %>');
			if (!contentsLimit)
			{
				args.IsValid = false;
				return;
			}

			var contentsValue = Number.parseInvariant(contentsLimit.value);
			if (isNaN(contentsValue))
			{
				contentsValue = 0;
			}

			if (contentsValue === 0)
			{
				valText = valText + 'Please fill the Contents fields.';
				obj.innerHTML = valText;
				args.IsValid = false;
				return;
			}
			minimum = contentsValue * min / 100;
			maximum = contentsValue * max / 100;
		} else
		{
			minimum = min;
			maximum = max;
		}

		var valid = value >= minimum && value <= maximum;
		if (!valid)
		{
			valText = valText + 'Min: $' + minimum.format("N") + ', Max: $' + maximum.format("N");
			obj.innerHTML = valText;
		}
		args.IsValid = valid;
	}

	function ValidateCSL(obj, args)
	{
		ValidateLimit(obj, args, Number(additionalValuesMinimum[0]), Number(additionalValuesMaximum[0]), false, false);
	}

	function ValidateBurglary(obj, args)
	{
		var chkBurglary = $get('<%= chkBurglary.ClientID %>');
		var percentage = true;
		ValidateLimit(obj, args, Number(additionalPercentageMinimum[0]), Number(additionalPercentageMaximum[0]), percentage, !chkBurglary.checked);
	}

	function ValidateJewelry(obj, args)
	{
		var chkJewelry = $get('<%= chkJewelry.ClientID %>');
		var percentage = true;
		ValidateLimit(obj, args, Number(additionalPercentageMinimum[1]), Number(additionalPercentageMaximum[1]), percentage, !chkJewelry.checked);
	}

	function ValidateMoney(obj, args)
	{
		var chkMoney = $get('<%= chkMoney.ClientID %>');
		ValidateLimit(obj, args, Number(additionalValuesMinimum[1]), Number(additionalValuesMaximum[1]), false, !chkMoney.checked);
	}

	function ValidateGlass(obj, args)
	{
		var chkGlass = $get('<%= chkGlass.ClientID %>');
		ValidateLimit(obj, args, Number(additionalValuesMinimum[2]), Number(additionalValuesMaximum[2]), false, !chkGlass.checked);
	}

	function ValidateElectronic(obj, args)
	{
		var chkElectronic = $get('<%= chkElectronic.ClientID %>');
		var percentage = true;
		ValidateLimit(obj, args, Number(additionalPercentageMinimum[2]), Number(additionalPercentageMaximum[2]), percentage, !chkElectronic.checked);
	}

	function ValidatePersonal(obj, args)
	{
		var chkPersonal = $get('<%= chkPersonal.ClientID %>');
		ValidateLimit(obj, args, Number(additionalValuesMinimum[3]), Number(additionalValuesMaximum[3]), false, !chkPersonal.checked);
	}
</script>

<table width="40%">
	<tr>
		<td style="color: #8d0e0d;">
			<asp:Literal ID="litBasic" Text="<%$ Resources:, strBasic %>" runat="server" />
		</td>
		<td style="color: #8d0e0d;" colspan="2">
			<asp:Literal ID="LitOptional" Text="<%$ Resources:, strOptional %>" runat="server" />
		</td>
	</tr>
	<tr>
		<td align="center">
			<asp:CheckBox ID="chkFire" CssClass="nofloat" Checked="true" Text="<%$ Resources:, strFire %>" Enabled="false" runat="server" />
		</td>
		<td align="center">
			<asp:CheckBox ID="chkQuake" CssClass="nofloat" Text="<%$ Resources:, strQuake %>" onclick="chkQuake_Click();" runat="server" />
		</td>
		<td>
			<asp:CheckBox ID="chkHMP" CssClass="nofloat" Text="<%$ Resources:, strHMP %>" onclick="chkHMP_Click(this);" runat="server" />
		</td>
	</tr>
	<tr>
		<td></td>
		<td></td>
		<td>
			<asp:Label ID="lblInactiveHMP" Text="<%$ Resources:, strInactiveHMP %>" runat="server" /></td>
	</tr>
</table>
<br />
<table>
	<tr>
		<td class="quoteStep borderStep" valign="top" style="width: 45%;">

			<table class="quoteAppData quoteAppDataAmple">

				<tr>
					<td class="step3Left">
						<asp:Label ID="lblBuilding" AssociatedControlID="txtBuilding" Text="<%$ Resources:, strBuilding %>" runat="server" /></td>
					<td style="display: none;" id="tdBuildingMinimum" runat="server"></td>
					<td>
						<asp:CheckBox ID="chkBuilding" Checked="true" onclick="chkActivatedBuilding(this);" runat="server" /></td>
					<td>
						<asp:TextBox ID="txtBuilding" onblur="UpdateValuesAndFormat(this);" onkeypress="checkKeyInt(event)" runat="server" /></td>
					<td style="display: none;" id="tdBuildingMaximum" runat="server"></td>
					<td>
						<asp:CustomValidator ID="valBuilding" Text="<%$ Resources:, strInvalidValue %>" Display="Dynamic" ControlToValidate="txtBuilding" ValidateEmptyText="true" ClientValidationFunction="ValidateBuilding" OnServerValidate="valBuilding_ServerValidate" runat="server" />
					</td>
					<td>
						<asp:HyperLink ID="lnkBuildingQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?id=1" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
				</tr>
				<tr>
					<td class="step3Left">
						<asp:Label ID="lblOtherStr" Text="<%$ Resources:, strOtherStr %>" AssociatedControlID="txtOtherStructures" runat="server" /></td>
					<td style="display: none;" id="tdOtherStructuresMinimum" runat="server">$0.00</td>
					<td>
						<asp:CheckBox ID="chkOtherStructures" Checked="true" onclick="chkActivatedOtherStructures(this);" runat="server" /></td>
					<td>
						<asp:TextBox ID="txtOtherStructures" onblur="UpdateValuesAndFormat(this);" onkeypress="checkKeyInt(event)" runat="server" /></td>
					<td style="display: none;" id="tdOtherStructuresMaximum" runat="server"></td>
					<td>
						<asp:CustomValidator ID="valOtherStructures" Text="<%$ Resources:, strInvalidValue %>" Display="Dynamic" ControlToValidate="txtOtherStructures" ValidateEmptyText="true" ClientValidationFunction="ValidateOtherStructures" OnServerValidate="valOtherStructures_ServerValidate" runat="server" />
					</td>
					<td>
						<asp:HyperLink ID="lnkOtherStructuresQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?id=1" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
				</tr>
				<tr>
					<td class="step3Left">
						<asp:Label ID="lblOutdoor" Text="<%$ Resources:, strOutdoor %>" AssociatedControlID="txtOutdoor" runat="server" /></td>
					<td style="display: none;" id="tdOutdoorMinimum" runat="server"></td>
					<td>
						<asp:CheckBox ID="chkOutdoor" onclick="chkActivatedOutdoor(this);" runat="server" /></td>
					<td>
						<asp:TextBox ID="txtOutdoor" onblur="FormatMoney(this);" onkeypress="checkKeyInt(event)" runat="server" /></td>
					<td style="display: none;" id="tdOutdoorMaximum" runat="server"></td>
					<td>
						<asp:CustomValidator ID="valOutdoor" Text="<%$ Resources:, strInvalidValue %>" Display="Dynamic" ControlToValidate="txtOutdoor" ValidateEmptyText="true" ClientValidationFunction="ValidateOutdoor" OnServerValidate="valOutdoor_ServerValidate" runat="server" />
					</td>
					<td>
						<asp:HyperLink ID="lnkOutdoorQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?id=2" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
				</tr>
				<tr>
					<td class="step3Left">
						<asp:Label ID="lblContents" Text="<%$ Resources:, strContents %>" AssociatedControlID="txtContents" runat="server" /></td>
					<td style="display: none;" id="tdContentsMinimum" runat="server"></td>
					<td>
						<asp:CheckBox ID="chkContents" Enabled="false" Checked="true" onclick="chkActivatedContents(this);" runat="server" /></td>
					<td>
						<asp:TextBox ID="txtContents" onblur="UpdateValuesAndFormat(this);" onkeypress="checkKeyInt(event)" runat="server" /></td>
					<td style="display: none;" id="tdContentsMaximum" runat="server"></td>
					<td>
						<asp:CustomValidator ID="valContents" Text="<%$ Resources:, strInvalidValue %>" Display="Dynamic" ControlToValidate="txtContents" ValidateEmptyText="true" ClientValidationFunction="ValidateContents" OnServerValidate="valContents_ServerValidate" runat="server" />
					</td>
					<td>
						<asp:HyperLink ID="lnkContentsQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?id=3" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
				</tr>
				<tr>
					<td class="step3Left">
						<asp:Label ID="lblDebris" Text="<%$ Resources:, strDebris %>" AssociatedControlID="txtDebris" runat="server" /></td>
					<td style="display: none;" id="tdDebrisMinimum" runat="server"></td>
					<td>
						<asp:CheckBox ID="chkDebris" Enabled="false" Checked="true" onclick="chkActivatedDebris(this);" runat="server" /></td>
					<td>
						<asp:TextBox ID="txtDebris" onblur="FormatMoney(this);" onkeypress="checkKeyInt(event)" runat="server" /></td>
					<td style="display: none;" id="tdDebrisMaximum" runat="server"></td>
					<td>
						<asp:CustomValidator ID="valDebris" Text="<%$ Resources:, strInvalidValue %>" Display="Dynamic" ControlToValidate="txtDebris" ValidateEmptyText="true" ClientValidationFunction="ValidateDebris" OnServerValidate="valDebris_ServerValidate" runat="server" />
					</td>
					<td>
						<asp:HyperLink ID="lnkDebrisQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?id=4" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
				</tr>
				<tr>
					<td class="step3Left">
						<asp:Label ID="lblExtra" Text="<%$ Resources:, strExtra %>" AssociatedControlID="txtExtra" runat="server" /></td>
					<td style="display: none;" id="tdExtraMinimum" runat="server"></td>
					<td>
						<asp:CheckBox ID="chkExtra" Enabled="false" Checked="true" onclick="chkActivatedExtra(this);" runat="server" /></td>
					<td>
						<asp:TextBox ID="txtExtra" onblur="FormatMoney(this);" onkeypress="checkKeyInt(event)" runat="server" /></td>
					<td style="display: none;" id="tdExtraMaximum" runat="server"></td>
					<td>
						<asp:CustomValidator ID="valExtra" Text="<%$ Resources:, strInvalidValue %>" Display="Dynamic" ControlToValidate="txtExtra" ValidateEmptyText="true" ClientValidationFunction="ValidateExtra" OnServerValidate="valExtra_ServerValidate" runat="server" />
					</td>
					<td>
						<asp:HyperLink ID="lnkExtraQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?id=5" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
				</tr>
				<tr>
					<td class="step3Left">
						<asp:Label ID="lblLossRent" Text="<%$ Resources:, strLossRent %>" AssociatedControlID="txtLossRents" runat="server" /></td>
					<td style="display: none;" id="tdLossRentsMinimum" runat="server"></td>
					<td>
						<asp:CheckBox ID="chkLossRents" onclick="chkActivatedLossRents(this);" runat="server" /></td>
					<td>
						<asp:TextBox ID="txtLossRents" onblur="FormatMoney(this);" onkeypress="checkKeyInt(event)" runat="server" /></td>
					<td style="display: none;" id="tdLossRentsMaximum" runat="server"></td>
					<td>
						<asp:CustomValidator ID="valLossRents" Text="<%$ Resources:, strInvalidValue %>" Display="Dynamic" ControlToValidate="txtLossRents" ValidateEmptyText="true" ClientValidationFunction="ValidateLossRents" OnServerValidate="valLossRents_ServerValidate" runat="server" />
					</td>
					<td>
						<asp:HyperLink ID="lnkLossRentQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?id=6" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
				</tr>
				<tr>
					<td class="step3Right" colspan="3">
						<%--<asp:CustomValidator ID="valSum" Text="Maximum value exceeded" Display="Dynamic" OnServerValidate="CustomSumValidator_ServerValidate" runat="server" />--%>
						<asp:CustomValidator ID="CustomValidator3" Text="" Display="Dynamic" ControlToValidate="txtContents" OnServerValidate="CustomSumValidator_ServerValidate" runat="server" />
					</td>
				</tr>
			</table>
		</td>
		<td class="quoteStep" valign="top">
			<table class="quoteAppData">
				<tr>
					<td class="step3Right">
						<asp:Label ID="lblCSL" Text="<%$ Resources:, strCSL %>" AssociatedControlID="txtCSL" runat="server" /></td>
					<td style="display: none;" id="tdCSLMinimum" runat="server"></td>
					<td>
						<asp:CheckBox ID="CheckBox1" Enabled="false" Checked="true" runat="server" /></td>
					<td>
						<asp:TextBox ID="txtCSL" onblur="FormatMoney(this);" onkeypress="checkKeyInt(event)" runat="server" /></td>
					<td style="display: none;" id="tdCSLMaximum" runat="server"></td>
					<td>
						<asp:CustomValidator ID="valCSL" Text="<%$ Resources:, strInvalidValue %>" Display="Dynamic" ControlToValidate="txtCSL" ValidateEmptyText="true" ClientValidationFunction="ValidateCSL" OnServerValidate="valCSL_ServerValidate" runat="server" />
					</td>
					<td>
						<asp:HyperLink ID="lnkCSLQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?coverage=1&id=1" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
				</tr>
				<tr>
					<td class="step3Right">
						<asp:Label ID="lblDomestic" Text="<%$ Resources:, strDomestic %>" runat="server" /></td>
					<td style="display: none;" id="tdDomesticMinimum" runat="server"></td>
					<td>
						<asp:CheckBox ID="chkDomestic" Checked="true" Enabled="false" runat="server" /></td>
					<td align="center">
						<asp:Label ID="lblDomesticCoverd" Text="<%$ Resources:, strCovered %>" runat="server" /></td>
					<td style="display: none;" id="tdDomesticMaximum" runat="server"></td>
					<td></td>
					<td>
						<asp:HyperLink ID="lnkDomesticQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?coverage=1&id=2" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
				</tr>
				<tr>
					<td class="step3Right">
						<asp:Label ID="lblBurglary" Text="<%$ Resources:, strBurglary %>" AssociatedControlID="txtBurglary" runat="server" /></td>
					<td style="display: none;" id="tdBurglaryMinimum" runat="server"></td>
					<td>
						<asp:CheckBox ID="chkBurglary" onclick="chkActivatedBurglary(this);" runat="server" /></td>
					<td>
						<asp:TextBox ID="txtBurglary" onblur="FormatMoney(this);" onkeypress="checkKeyInt(event)" runat="server" /></td>
					<td style="display: none;" id="tdBurglaryMaximum" runat="server"></td>
					<td>
						<asp:CustomValidator ID="valBurglary" Text="<%$ Resources:, strInvalidValue %>" Display="Dynamic" ControlToValidate="txtBurglary" ValidateEmptyText="true" ClientValidationFunction="ValidateBurglary" OnServerValidate="valBurglary_ServerValidate" runat="server" />
					</td>
					<td>
						<asp:HyperLink ID="lnkBurglaryQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?coverage=1&id=3" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
				</tr>
				<tr>
					<td class="step3Right">
						<asp:Label ID="lblJewelry" Text="<%$ Resources:, strJewelry %>" AssociatedControlID="txtJewelry" runat="server" /></td>
					<td style="display: none;" id="tdJewelryMinimum" runat="server"></td>
					<td>
						<asp:CheckBox ID="chkJewelry" onclick="chkActivatedJewelry(this);" runat="server" /></td>
					<td>
						<asp:TextBox ID="txtJewelry" onblur="FormatMoney(this);" onkeypress="checkKeyInt(event)" runat="server" /></td>
					<td style="display: none;" id="tdJewelryMaximum" runat="server"></td>
					<td>
						<asp:CustomValidator ID="valJewelry" Text="<%$ Resources:, strInvalidValue %>" Display="Dynamic" ControlToValidate="txtJewelry" ValidateEmptyText="true" ClientValidationFunction="ValidateJewelry" OnServerValidate="valJewelry_ServerValidate" runat="server" />
					</td>
					<td>
						<asp:HyperLink ID="lnkJewelryQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?coverage=1&id=4" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
				</tr>
				<tr>
					<td class="step3Right">
						<asp:Label ID="lblMoney" Text="<%$ Resources:, strMoney %>" AssociatedControlID="txtMoney" runat="server" /></td>
					<td style="display: none;" id="tdMoneyMinimum" runat="server"></td>
					<td>
						<asp:CheckBox ID="chkMoney" onclick="chkActivatedMoney(this);" runat="server" /></td>
					<td>
						<asp:TextBox ID="txtMoney" onblur="FormatMoney(this);" onkeypress="checkKeyInt(event)" runat="server" /></td>
					<td style="display: none;" id="tdMoneyMaximum" runat="server"></td>
					<td>
						<asp:CustomValidator ID="valMoney" Text="<%$ Resources:, strInvalidValue %>" Display="Dynamic" ControlToValidate="txtMoney" ValidateEmptyText="true" ClientValidationFunction="ValidateMoney" OnServerValidate="valMoney_ServerValidate" runat="server" />
					</td>
					<td>
						<asp:HyperLink ID="lnkMoneyQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?coverage=1&id=5" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
				</tr>
				<tr>
					<td class="step3Right">
						<asp:Label ID="lblGlass" Text="<%$ Resources:, strGlass %>" AssociatedControlID="txtGlass" runat="server" /></td>
					<td style="display: none;" id="tdGlassMinimum" runat="server"></td>
					<td>
						<asp:CheckBox ID="chkGlass" onclick="chkActivatedGlass(this);" runat="server" /></td>
					<td>
						<asp:TextBox ID="txtGlass" onblur="FormatMoney(this);" onkeypress="checkKeyInt(event)" runat="server" /></td>
					<td style="display: none;" id="tdGlassMaximum" runat="server"></td>
					<td>
						<asp:CustomValidator ID="valGlass" Text="<%$ Resources:, strInvalidValue %>" Display="Dynamic" ControlToValidate="txtGlass" ValidateEmptyText="true" ClientValidationFunction="ValidateGlass" OnServerValidate="valGlass_ServerValidate" runat="server" />
					</td>
					<td>
						<asp:HyperLink ID="lnkGlassQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?coverage=1&id=6" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
				</tr>
				<tr>
					<td class="step3Right">
						<asp:Label ID="lblElectronic" Text="<%$ Resources:, strElectronic %>" AssociatedControlID="txtElectronic" runat="server" /></td>
					<td style="display: none;" id="tdElectronicMinimum" runat="server"></td>
					<td>
						<asp:CheckBox ID="chkElectronic" onclick="chkActivatedElectronic(this);" runat="server" /></td>
					<td>
						<asp:TextBox ID="txtElectronic" onblur="FormatMoney(this);" onkeypress="checkKeyInt(event)" runat="server" /></td>
					<td style="display: none;" id="tdElectronicMaximum" runat="server"></td>
					<td>
						<asp:CustomValidator ID="valElectronic" Text="<%$ Resources:, strInvalidValue %>" Display="Dynamic" ControlToValidate="txtElectronic" ValidateEmptyText="true" ClientValidationFunction="ValidateElectronic" OnServerValidate="valElectronic_ServerValidate" runat="server" />
					</td>
					<td>
						<asp:HyperLink ID="lnkElectronicQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?coverage=1&id=7" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
				</tr>
				<tr>
					<td class="step3Right">
						<asp:Label ID="lblPersonal" Text="<%$ Resources:, strPersonal %>" AssociatedControlID="txtPersonal" runat="server" /></td>
					<td style="display: none;" id="tdPersonalMinimum" runat="server"></td>
					<td>
						<asp:CheckBox ID="chkPersonal" onclick="chkActivatedPersonal(this);" runat="server" /></td>
					<td>
						<asp:TextBox ID="txtPersonal" onblur="FormatMoney(this);" onkeypress="checkKeyInt(event)" runat="server" /></td>
					<td style="display: none;" id="tdPersonalMaximum" runat="server"></td>
					<td>
						<asp:CustomValidator ID="valPersonal" Text="<%$ Resources:, strInvalidValue %>" Display="Dynamic" ControlToValidate="txtPersonal" ValidateEmptyText="true" ClientValidationFunction="ValidatePersonal" OnServerValidate="valPersonal_ServerValidate" runat="server" />
					</td>
					<td>
						<asp:HyperLink ID="lnkPersonalQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?coverage=1&id=8" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
				</tr>
				<tr>
					<td class="step3Right">
						<asp:Label ID="lblFamily" Text="<%$ Resources:, strFamily %>" runat="server" /></td>
					<td style="display: none;" id="tdFamilyMinimum" runat="server"></td>
					<td>
						<asp:CheckBox ID="chkFamily" onclick="chkActivatedFamily(this);" runat="server" /></td>
					<td align="center">
						<asp:Label ID="lblFamilyCovered" Text="<%$ Resources:, strCovered %>" runat="server" /></td>
					<td style="display: none;" id="tdFamilyMaximum" runat="server"></td>
					<td></td>
					<td>
						<asp:HyperLink ID="lnkFamilyQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?coverage=1&id=9" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
				</tr>
				<tr>
					<td class="step3Right">
						<asp:Label ID="lblTechSupport" Text="<%$ Resources:, strTechSupport %>" runat="server" /></td>
					<td style="display: none;" id="tdTechSupportMinimum" runat="server"></td>
					<td>
						<asp:CheckBox ID="chkTechSupport" onclick="chkTechSupport(this);" runat="server" /></td>
					<td align="center">
						<asp:Label ID="lblTechSupportCovered" Text="<%$ Resources:, strCovered %>" runat="server" /></td>
					<td style="display: none;" id="tdTechSupportMaximum" runat="server"></td>
					<td></td>
					<td>
						<asp:HyperLink ID="lnkTechSupport" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?coverage=1&id=10" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
				</tr>
			</table>
		</td>
	</tr>
</table>
<table>
	<tr id="lossRentsWarning" style="display: none;" runat="server">
		<td>
			<br />
			<asp:Label ID="lblLossRentWarning" Text="<%$ Resources:, strLossRentsWarning %>" runat="server" />
		</td>
	</tr>
</table>
<br />
<table width="90%">
	<tr>
		<td style="width: 33%;">
			<div class="row">
				<asp:Label ID="lblOutdoorDescription" CssClass="baseRed" Text="<%$ Resources:, strOutdoorDescription %>" AssociatedControlID="txtOutdoorDescription" runat="server" />
				<asp:CustomValidator ID="valOutdoorDescription" Display="Dynamic" ValidateEmptyText="true" Text="<%$ Resources:, strRequired %>" ControlToValidate="txtOutdoorDescription" ClientValidationFunction="checkOutdoorDescription" OnServerValidate="valOutdoor_Validate" runat="server" />
			</div>
			<asp:TextBox ID="txtOutdoorDescription" Columns="33" Rows="4" TextMode="MultiLine" runat="server" />
		</td>
		<td style="width: 33%;">
			<div class="row">
				<asp:Label ID="lblOSDescription" CssClass="baseRed" Text="<%$ Resources:, strOSDescription %>" AssociatedControlID="txtOtherStructuresDescription" runat="server" />
				<asp:CustomValidator ID="valOSDescription" Display="Dynamic" ValidateEmptyText="true" Text="<%$ Resources:, strRequired %>" ControlToValidate="txtOtherStructuresDescription" ClientValidationFunction="checkOtherStructuresDescription" OnServerValidate="valOtherStructuresDescription_Validate" runat="server" />
			</div>
			<asp:TextBox ID="txtOtherStructuresDescription" Columns="33" Rows="4" TextMode="MultiLine" runat="server" />
		</td>
		<td style="width: 33%;">
			<div class="row">
				<asp:Label ID="lblAdditionalNotes" CssClass="baseRed" Text="<%$ Resources:, strAdditionalNotes %>" AssociatedControlID="txtAdditionalNotes" runat="server" />
			</div>
			<asp:TextBox ID="txtAdditionalNotes" Columns="33" Rows="4" TextMode="MultiLine" runat="server" />
		</td>
	</tr>
	<tr>
		<td>
			<br />
		</td>
	</tr>
	<tr>
		<td></td>
		<td>
			<asp:UpdatePanel ID="UpdatePanel1" runat="server">
				<ContentTemplate>
					<asp:Button ID="btnUpdate" CssClass="updateButton" Text="<%$ Resources:, strRefreshPremium %>" runat="server"
						OnClick="btnUpdate_Click" />
					<h1>&nbsp;&nbsp;&nbsp;<asp:Literal ID="ltlPremium" Text="<%$ Resources:, strPremium %>" runat="server" /><asp:Literal ID="lblPremium" runat="server" /></h1>
				</ContentTemplate>
			</asp:UpdatePanel>
		</td>
	</tr>
</table>
