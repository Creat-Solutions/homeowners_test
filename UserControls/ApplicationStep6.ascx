<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ApplicationStep6.ascx.cs" Inherits="UserControls_ApplicationStep6" %>

<script type="text/javascript">

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

    function ValidateLimit(obj, args, min, max, emptyAllowed) {
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
            valText = valText + 'Minimum: $' + min.format("N") + ', Maximum: $' + max.format("N");
            obj.innerHTML = valText;
        }
        args.IsValid = valid;
    }

    function ValidateCSL(obj, args) {
        ValidateLimit(obj, args, Number(valuesMinimum[0]), Number(valuesMaximum[0]), false);
    }

    function ValidateBurglary(obj, args) {
        var chkBurglary = $get('<%= chkBurglary.ClientID %>');
        ValidateLimit(obj, args, Number(valuesMinimum[1]), Number(valuesMaximum[1]), !chkBurglary.checked);
    }

    function ValidateJewelry(obj, args) {
        var chkJewelry = $get('<%= chkJewelry.ClientID %>');
        ValidateLimit(obj, args, Number(valuesMinimum[2]), Number(valuesMaximum[2]), !chkJewelry.checked);
    }

    function ValidateMoney(obj, args) {
        var chkMoney = $get('<%= chkMoney.ClientID %>');
        ValidateLimit(obj, args, Number(valuesMinimum[3]), Number(valuesMaximum[3]), !chkMoney.checked);
    }

    function ValidateGlass(obj, args) {
        var chkGlass = $get('<%= chkGlass.ClientID %>');
        ValidateLimit(obj, args, Number(valuesMinimum[4]), Number(valuesMaximum[4]), !chkGlass.checked);
    }

    function ValidateElectronic(obj, args) {
        var chkElectronic = $get('<%= chkElectronic.ClientID %>');
        ValidateLimit(obj, args, Number(valuesMinimum[5]), Number(valuesMaximum[5]), !chkElectronic.checked);
    }

    function ValidatePersonal(obj, args) {
        var chkPersonal = $get('<%= chkPersonal.ClientID %>');
        ValidateLimit(obj, args, Number(valuesMinimum[6]), Number(valuesMaximum[6]), !chkPersonal.checked);
    }
    
</script>

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
            <td><asp:HyperLink ID="lnkCSLQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?coverage=1&id=1" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
            <td class="vsubname"><asp:Label ID="lblCSL" AssociatedControlID="txtCSL" runat="server" /><br /><asp:Label ID="lblCSLEs" CssClass="rowes" AssociatedControlID="txtCSL" runat="server" /></td>
            <td id="tdCSLMinimum" runat="server"></td>
            <td><asp:CheckBox ID="chkBuilding" Enabled="false" Checked="true" runat="server" /></td>
            <td><asp:TextBox ID="txtCSL" onblur="FormatMoney(this);" runat="server" /></td>
            <td id="tdCSLMaximum" runat="server"></td>
            <td>
                <asp:CustomValidator ID="valCSL" Text="Invalid value." Display="Dynamic" ControlToValidate="txtCSL" ValidateEmptyText="true" ClientValidationFunction="ValidateCSL" OnServerValidate="valCSL_ServerValidate" runat="server" />
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
            <td><asp:HyperLink ID="lnkBurglaryQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?coverage=1&id=3" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
            <td class="vsubname"><asp:Label ID="lblBurglary" AssociatedControlID="txtBurglary" runat="server" /><br /><asp:Label ID="lblRobo" CssClass="rowes" AssociatedControlID="txtBurglary" runat="server" /></td>
            <td id="tdBurglaryMinimum" runat="server"></td>
            <td><asp:CheckBox ID="chkBurglary" onclick="chkActivatedBurglary(this);" runat="server" /></td>
            <td><asp:TextBox ID="txtBurglary" onblur="FormatMoney(this);" runat="server" /></td>
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
            <td><asp:TextBox ID="txtJewelry" onblur="FormatMoney(this);" runat="server" /></td>
            <td id="tdJewelryMaximum" runat="server"></td>
            <td>
                <asp:CustomValidator ID="valJewelry" Text="Invalid value." Display="Dynamic" ControlToValidate="txtJewelry" ValidateEmptyText="true" ClientValidationFunction="ValidateJewelry" OnServerValidate="valJewelry_ServerValidate" runat="server" />
            </td>
        </tr>
        <tr>
            <td><asp:HyperLink ID="lnkMoneyQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?coverage=1&id=5" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
            <td class="vsubname"><asp:Label ID="lblMoney" AssociatedControlID="txtMoney" runat="server" /><br /><asp:Label ID="lblDinero" CssClass="rowes" AssociatedControlID="txtMoney" runat="server" /></td>
            <td id="tdMoneyMinimum" runat="server"></td>
            <td><asp:CheckBox ID="chkMoney" onclick="chkActivatedMoney(this);" runat="server" /></td>
            <td><asp:TextBox ID="txtMoney" onblur="FormatMoney(this);" runat="server" /></td>
            <td id="tdMoneyMaximum" runat="server"></td>
            <td>
                <asp:CustomValidator ID="valMoney" Text="Invalid value." Display="Dynamic" ControlToValidate="txtMoney" ValidateEmptyText="true" ClientValidationFunction="ValidateMoney" OnServerValidate="valMoney_ServerValidate" runat="server" />
            </td>
        </tr>
        <tr>
            <td><asp:HyperLink ID="lnkGlassQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?coverage=1&id=6" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
            <td class="vsubname"><asp:Label ID="lblGlass" AssociatedControlID="txtGlass" runat="server" /><br /><asp:Label ID="lblCristales" CssClass="rowes" AssociatedControlID="txtGlass" runat="server" /></td>
            <td id="tdGlassMinimum" runat="server"></td>
            <td><asp:CheckBox ID="chkGlass" onclick="chkActivatedGlass(this);" runat="server" /></td>
            <td><asp:TextBox ID="txtGlass" onblur="FormatMoney(this);" runat="server" /></td>
            <td id="tdGlassMaximum" runat="server"></td>
            <td>
                <asp:CustomValidator ID="valGlass" Text="Invalid value" Display="Dynamic" ControlToValidate="txtGlass" ValidateEmptyText="true" ClientValidationFunction="ValidateGlass" OnServerValidate="valGlass_ServerValidate" runat="server" />
            </td>
        </tr>
        <tr>
            <td><asp:HyperLink ID="lnkElectronicQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?coverage=1&id=7" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" /></td>
            <td class="vsubname"><asp:Label ID="lblElectronic" AssociatedControlID="txtElectronic" runat="server" /><br /><asp:Label ID="lblElectronico" CssClass="rowes" AssociatedControlID="txtElectronic" runat="server" /></td>
            <td id="tdElectronicMinimum" runat="server"></td>
            <td><asp:CheckBox ID="chkElectronic" onclick="chkActivatedElectronic(this);" runat="server" /></td>
            <td><asp:TextBox ID="txtElectronic" onblur="FormatMoney(this);" runat="server" /></td>
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
            <td><asp:TextBox ID="txtPersonal" onblur="FormatMoney(this);" runat="server" /></td>
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

