<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GenerateStep1.ascx.cs" Inherits="UserControls_GenerateStep1" %>

<div class="row step1">
    <asp:Label ID="lblPersonType" AssociatedControlID="rbnIndividual" Text="<%$ Resources:, strPersonType %>" runat="server" />
    <asp:RadioButton Checked="True" ID="rbnIndividual" GroupName="grpPersonType" runat="server" /><asp:Literal ID="litIndividual" Text="<%$ Resources:, strIndividual %>" runat="server" />
    <asp:RadioButton ID="rbnCorporation" GroupName="grpPersonType" runat="server" /><asp:Literal ID="litCorporation" Text="<%$ Resources:, strCorporation %>" runat="server" />
    <asp:CustomValidator ID="reqPersonType" ValidateEmptyText="true" ClientValidationFunction="validatePersonType" Display="Dynamic"
        OnServerValidate="reqPerson_ServerValidate" Text="<%$ Resources:, strValRadioButton %>" ForeColor="" CssClass="failureText"
        runat="server" />
</div>
<div class="row step1">
    <asp:Label ID="lblName" AssociatedControlID="txtName" Text="<%$ Resources:, strName %>" runat="server" />
    <asp:TextBox ID="txtName" MaxLength="100" runat="server" />
    <asp:RequiredFieldValidator ID="reqName" Text="<%$ Resources:, strRequired %>" Display="Dynamic" ControlToValidate="txtName" runat="server" />
    <user:LengthValidator ID="valName" Text="<%$ Resources:, strValLength %>" MaxLength="100" Display="Dynamic" ControlToValidate="txtName" runat="server" />
</div>
<div id="divLastName" class="row step1">
    <asp:Label ID="lblLastName" AssociatedControlID="txtLastName" Text="<%$ Resources:, strLastName %>" runat="server" />
    <asp:TextBox ID="txtLastName" EnableViewState="false" MaxLength="64" runat="server" />
    <asp:CustomValidator ID="reqLast" Text="<%$ Resources:, strRequired %>" Display="Dynamic" ValidateEmptyText="True" OnServerValidate="txtLastName_ServerValidate" ClientValidationFunction="validateLastName" ControlToValidate="txtLastName" runat="server" />
    <asp:RegularExpressionValidator ID="cmpLast" Text="<%$ Resources:, strValCharacters %>" ControlToValidate="txtLastName" Display="Dynamic" ValidationExpression="^[a-zA-Z0-9\s\.]+$" runat="server" />
    <user:LengthValidator ID="valLast" Text="<%$ Resources:, strValLength %>" MaxLength="64" Display="Dynamic" ControlToValidate="txtLastName" runat="server" />
</div>
<div id="divLastName2" class="row step1">
    <asp:Label ID="lblLastName2" AssociatedControlID="txtLastName2" Text="<%$ Resources:, strLastName2 %>" runat="server" />
    <asp:TextBox ID="txtLastName2" EnableViewState="false" MaxLength="64" runat="server" />
</div>
<div class="row step1">
    <asp:Label ID="lblEmail" AssociatedControlID="txtEmail" Text="<%$ Resources:, strEmail %>" runat="server" />
    <asp:TextBox ID="txtEmail" Columns="32" MaxLength="128" runat="server" />
    <asp:RegularExpressionValidator ID="regexEmail" ValidationExpression=".+@.+\..+" Text="<%$ Resources:, strValEmail %>" ControlToValidate="txtEmail" Display="Dynamic" runat="server" />
    <user:LengthValidator ID="valEmail" Text="<%$ Resources:, strValLength %>" MaxLength="128" Display="Dynamic" ControlToValidate="txtEmail" runat="server" />
    <asp:CustomValidator ID="reqFieldsEmail" ValidateEmptyText="true" ControlToValidate="txtEmail" ClientValidationFunction="valFields" Display="Dynamic"
        OnServerValidate="reqFields_ServerValidate" Text="<%$ Resources:, strValEmailPhone %>" ForeColor="" CssClass="failureText"
        runat="server" />
</div>
<div class="row step1">
    <asp:Label ID="lblPhone" AssociatedControlID="txtAreaPhone" Text="<%$ Resources:, strPhone %>" runat="server" />
    <asp:TextBox ID="txtAreaPhone" onkeypress="checkKeyInt(event)" MaxLength="3" Columns="3" onkeyup="setPhone1Focus();" runat="server" />
    <asp:TextBox ID="txtPhone1" onkeypress="checkKeyInt(event)" onkeyup="setPhone2Focus();" MaxLength="4" Columns="3" runat="server" /><asp:TextBox ID="txtPhone2" Columns="4" MaxLength="4" onkeypress="checkKeyInt(event)" runat="server" />
    <asp:CustomValidator ID="reqPhone2" ValidateEmptyText="true" ControlToValidate="txtPhone2" ClientValidationFunction="valFields" Display="Dynamic"
        OnServerValidate="reqFields_ServerValidate" Text="<%$ Resources:, strValEmailPhone %>" ForeColor="" CssClass="failureText"
        runat="server" />
    <asp:CompareValidator ID="comPhoneA" Display="Dynamic" Text="<%$ Resources:, strValCharacters %>" ControlToValidate="txtAreaPhone" Type="Integer" Operator="DataTypeCheck" runat="server" />
    <asp:CompareValidator ID="comPhone1" Display="Dynamic" Text="<%$ Resources:, strValCharacters %>" ControlToValidate="txtPhone1" Type="Integer" Operator="DataTypeCheck" runat="server" />
    <asp:CompareValidator ID="comPhone2" Display="Dynamic" Text="<%$ Resources:, strValCharacters %>" ControlToValidate="txtPhone2" Type="Integer" Operator="DataTypeCheck" runat="server" />
</div>
<div class="row step1">
    <asp:Label ID="lblClient" AssociatedControlID="ddlClient" Text="<%$ Resources:, strClient %>" runat="server" />
    <asp:DropDownList ID="ddlClient" OnDataBound="ddlDataBound" DataSourceID="odsCont" DataTextField="description" DataValueField="type" runat="server" />
    <asp:RequiredFieldValidator ID="reqRisk" ControlToValidate="ddlClient" Text="<%$ Resources:, strRequired %>" Display="Dynamic" InitialValue="0" runat="server" />
</div>
<div class="row step1">
    <asp:Label ID="lblCurrency" AssociatedControlID="ddlClient" Text="<%$ Resources:, strCurrency %>" runat="server" />
    <asp:RadioButton Checked="True" ID="rbDollars" GroupName="grpCurrency" runat="server" /><asp:Literal ID="Literal1" Text="<%$ Resources:, strDollars %>" runat="server" />
    <asp:RadioButton ID="rbPesos" GroupName="grpCurrency" runat="server" />Pesos
    <asp:CustomValidator ID="reqCurrency" ValidateEmptyText="true" ClientValidationFunction="validateCurrency" Display="Dynamic"
        OnServerValidate="reqCurrency_ServerValidate" Text="<%$ Resources:, strValRadioButton %>" ForeColor="" CssClass="failureText"
        runat="server" />
</div>
<div class="row step1" id="divBuilding">
    <asp:Label ID="lblValue" AssociatedControlID="txtValue" Text="<%$ Resources:, strValue %>" runat="server" />
    <asp:TextBox ID="txtValue" onkeypress="checkKeyInt(event)" EnableViewState="false" MaxLength="16" runat="server" />

    <asp:CustomValidator ID="valValue" ValidateEmptyText="true" ControlToValidate="txtValue" ClientValidationFunction="validateValue" Display="Dynamic"
        OnServerValidate="txtValue_ServerValidate" ForeColor="" CssClass="failureText"
        runat="server" />
</div>
<div class="row step1" id="divContents">
    <asp:Label ID="lblContents" AssociatedControlID="txtContents" Text="<%$ Resources:, strValueContents %>" runat="server" />
    <asp:TextBox ID="txtContents" onkeypress="checkKeyInt(event)" EnableViewState="false" MaxLength="16" runat="server" />
    <asp:CustomValidator ID="valContents" ValidateEmptyText="true" ControlToValidate="txtContents" ClientValidationFunction="validateContents" Display="Dynamic"
        OnServerValidate="txtContents_ServerValidate" ForeColor="" CssClass="failureText"
        runat="server" />




</div>
<div class="row step1">
    <asp:Label ID="lblUse" AssociatedControlID="ddlUse" runat="server" Text="<%$ Resources:, strUse %>" />
    <asp:DropDownList ID="ddlUse" OnDataBound="ddlDataBound" DataSourceID="odsUse" DataTextField="description" DataValueField="use" runat="server" />
    <asp:RequiredFieldValidator ID="reqUse" Display="Dynamic" InitialValue="0" ControlToValidate="ddlUse" Text="<%$ Resources:, strRequired %>" runat="server" />
</div>

<div class="row step1">
    <asp:Label ID="lblZip" AssociatedControlID="txtZipCode" Text="<%$ Resources:, strZip %>" runat="server" />
    <asp:TextBox ID="txtZipCode" onkeypress="checkKeyInt(event)" MaxLength="10" runat="server" />
    <asp:RequiredFieldValidator ID="reqZipCode" Display="Dynamic" ControlToValidate="txtZipCode" Text="<%$ Resources:, strRequired %>" runat="server" />
    <user:LengthValidator ID="lenZipCode" Text="Maximum length for zip code" Display="Dynamic" MaxLength="10" ControlToValidate="txtZipCode" runat="server" />
    <asp:Label ID="lblHouseEstateNotFound" CssClass="failureText" Text="<%$ Resources:, strValZip %>" runat="server" />
    <asp:HyperLink ID="HyperLink1" NavigateUrl="https://www.correosdemexico.gob.mx/SSLServicios/ConsultaCP/Descarga.aspx" Target="_blank" Text="<%$ Resources:, strZipLink %>" runat="server" />
</div>
<div class="row step1">
    <asp:Label ID="lblState" AssociatedControlID="txtState" Text="<%$ Resources:, strState %>" runat="server" />
    <asp:TextBox ID="txtState" Style="width: 175px; background-color: #DDD;" ReadOnly="true" runat="server" />
    <select id="ddlState"></select>
    <asp:HiddenField ID="hfState" runat="server" />
</div>
<div class="row step1">
    <asp:Label ID="lblCity" AssociatedControlID="txtCity" Text="<%$ Resources:, strCity %>" runat="server" />
    <asp:TextBox ID="txtCity" Style="width: 175px; background-color: #DDD;" ReadOnly="true" runat="server" />
    <select id="ddlCity"></select>
    <asp:HiddenField ID="hfCity" runat="server" />
</div>
<div class="row step1">
    <asp:Label ID="lblColony" AssociatedControlID="txtHouseEstate" Text="<%$ Resources:, strColony %>" runat="server" />
    <asp:TextBox ID="txtHouseEstate" Style="width: 220px; background-color: #DDD;" ReadOnly="true" runat="server" />
    <select id="ddlHouseEstate" style="width: auto;"></select>
    <asp:HiddenField ID="hfHouseEstate" runat="server" />
</div>


<asp:ObjectDataSource ID="odsUse" TypeName="useOfProperty" SelectMethod="SelectWithoutWeekends" runat="server" />
<asp:ObjectDataSource ID="odsCont" TypeName="clientType" SelectMethod="SelectWithoutMortgage" runat="server" />

<script type="text/javascript">
    var ddlStates = $('#ddlState');
    var txtState = $('#<%= txtState.ClientID %>');
    var txtZipCode = $('#<%= txtZipCode.ClientID %>');
    var ddlCities = $('#ddlCity');
    var txtCity = $('#<%= txtCity.ClientID %>');
    var ddlHouseEstate = $('#ddlHouseEstate');
    var txtHouseEstate = $('#<%= txtHouseEstate.ClientID %>');
    var hfHouseEstate = $('#<%= hfHouseEstate.ClientID %>');
    var hfCity = $('#<%= hfCity.ClientID %>');
    var hfState = $('#<%= hfState.ClientID %>');
    var manualSearch = false;

    function valFields(obj, args) {
        if ($.trim($('#<%= txtEmail.ClientID %>').val()).length === 0 &&
            ($.trim($('#<%= txtAreaPhone.ClientID %>').val()).length < 2 ||
                $.trim($('#<%= txtPhone1.ClientID %>').val()).length < 3 ||
            $.trim($('#<%= txtPhone2.ClientID %>').val()).length < 4)) {

            args.IsValid = false;

        } else {
            $('#<%= reqFieldsEmail.ClientID %>, #<%= reqPhone2.ClientID %>').hide();
            args.IsValid = true;
        }
    }

    function validateValue(obj, args) {
        var building_min = 0; var building_max = 0;
        var clientType = Number($('#<%= ddlClient.ClientID %>').val());

        building_min = <%= BUILDING_MIN %>;
        building_max = <%= BUILDING_MAX %>;

        var xRate = Number(<%= EXCHANGE_RATE %>);

        var buildingValue = 0;
        buildingValue = Number.parseInvariant($('#<%= txtValue.ClientID %>').val());
        if (isNaN(buildingValue)) {
            buildingValue = 0;
        }

        var currency = getSelectedCurrency()

        if (currency != 'rbDollars') {
            building_min = building_min * xRate;
            building_max = building_max * xRate;
        }


        var validationText = '';

        if ((clientType === Number(<%= ApplicationConstants.OWNER_TYPE %>) && buildingValue > 0) || clientType === Number(<%= ApplicationConstants.LANDLORD_TYPE %>)) {
            if (buildingValue < building_min || buildingValue > building_max) {
                validationText = '<%= GetLocalResourceObject("strValValue").ToString() %>' + building_min.toString() + ' - ' + building_max.toString();
                obj.innerHTML = validationText;
                args.IsValid = false;
            }
        }
        else {
            args.IsValid = true;
        }
    }

    function validateContents(obj, args) {
        var contents_min = 0; var contents_max = 0;
        var clientType = Number($('#<%= ddlClient.ClientID %>').val());

        contents_min = <%= CONTENTS_MIN %>;
        contents_max = <%= CONTENTS_MAX %>;

        var xRate = Number(<%= EXCHANGE_RATE %>);

        var contentsValue = 0;
        contentsValue = Number.parseInvariant($('#<%= txtContents.ClientID %>').val());
        if (isNaN(contentsValue)) {
            contentsValue = 0;
        }

        var currency = getSelectedCurrency()

        if (currency != 'rbDollars') {
            contents_min = contents_min * xRate;
            contents_max = contents_max * xRate;
        }


        var validationText = '';

        if (clientType === Number(<%= ApplicationConstants.OWNER_TYPE %>) || clientType === Number(<%= ApplicationConstants.RENTER_TYPE %>)) {
            if (contentsValue < contents_min || contentsValue > contents_max) {
                validationText = '<%= GetLocalResourceObject("strValValue").ToString() %>' + contents_min.toString() + ' - ' + contents_max.toString();
                obj.innerHTML = validationText;
                args.IsValid = false;
            }
        }
        else {
            args.IsValid = true;
        }
    }

    function validatePersonType(obj, args) {
        if ($('[name$=grpPersonType]').is(':checked')) {
            args.IsValid = true;
        }
        else {
            args.IsValid = false;
        }
    }

    function validateCurrency(obj, args) {
        if ($('[name$=grpCurrency]').is(':checked')) {
            args.IsValid = true;
        }
        else {
            args.IsValid = false;
        }
    }

    function setPhone1Focus() {
        var actual = document.getElementById('<%= txtAreaPhone.ClientID %>');
        var next = document.getElementById('<%= txtPhone1.ClientID %>');
        var limit = 3;

        if (actual.value.length >= limit && next)
            next.focus();
    }

    function setPhone2Focus() {
        var area = document.getElementById('<%= txtAreaPhone.ClientID %>');
        var actual = document.getElementById('<%= txtPhone1.ClientID %>');
        var next = document.getElementById('<%= txtPhone2.ClientID %>');
        var limit = 6 - area.value.length;
        if (actual.value.length >= limit && next)
            next.focus();
    }

    var isNav;
    function checkKeyInt(e) {
        var keyChar;

        if (isNav == 0) {
            if (e.which < 48 || e.which > 57) e.RETURNVALUE = false;
        } else {
            if (e.keyCode < 48 || e.keyCode > 57) e.returnValue = false;
        }
    }

    function validateLastName(obj, args) {
        var txtLastName = $get('<%= this.txtLastName.ClientID %>');
        var type = getSelectedPersonType();
        if (!(txtLastName && type)) {
            args.IsValid = false;
            return;
        }

        if (type == "rbnIndividual") {
            args.IsValid = (txtLastName.value.length > 0);
            return;
        }
        args.IsValid = true;

    }

    function getSelectedCurrency() {
        var myRadio = $("input[name$=grpCurrency]")
        var selectedVal = "";
        var selected = myRadio.filter(':checked').val();
        if (selected.length > 0) {
            selectedVal = selected;
        }
        return selectedVal
    }

    function getSelectedPersonType() {
        var myRadio = $("input[name$=grpPersonType]")
        var selectedVal = "";
        var selected = myRadio.filter(':checked').val();
        if (selected.length > 0) {
            selectedVal = selected;
        }
        return selectedVal
    }

    function HandleUnknownZipCode() {
        hfHouseEstate.val('');
        hfCity.val('');
        hfState.val('');
        $('#<%= lblHouseEstateNotFound.ClientID %>').show();
        HideFullScreen();
    }

    function HandleHouseEstates(data, setCurrentHouseEstate) {

        if (data.houseEstates.length === 0 && !manualSearch) {
            HandleUnknownZipCode();
            return false;
        }

        $('#<%= lblHouseEstateNotFound.ClientID %>').hide();
        if (data.houseEstates.length > 1 || manualSearch) {
            txtHouseEstate.hide().val('');
            var h = undefined;
            var firstVal = undefined;

            for (h in data.houseEstates) {
                var houseEstate = data.houseEstates[h];
                if (firstVal === undefined) {
                    firstVal = houseEstate.ID;
                }
                ddlHouseEstate.append($('<option value="' + houseEstate.ID + '">' + houseEstate.Name + '</option>'));
            }
            if (setCurrentHouseEstate) {
                ddlHouseEstate.val(hfHouseEstate.val());
            } else {
                hfHouseEstate.val(firstVal);
            }
            ddlHouseEstate.show();

            txtHouseEstate.hide();
            return false;
        }

        ddlHouseEstate.hide();
        txtHouseEstate.show().val(data.houseEstates[0].Name);
        hfHouseEstate.val(data.houseEstates[0].ID);
        return true;
    }

    function HandleCities(data) {
        if (data.cities.length === 0 && !manualSearch) {
            HandleUnknownZipCode();
            return false;
        }

        if (data.cities.length > 1 || manualSearch) {
            txtCity.hide().val('');
            var c = undefined;

            for (c in data.cities) {
                var city = data.cities[c];
                ddlCities.append($('<option value="' + city.ID + '">' + city.Name + '</option>'));
            }

            ddlCities.show();
            ddlHouseEstate.show();

            txtCity.hide();
            txtHouseEstate.hide();
            return false;
        }

        ddlCities.hide();
        txtCity.show().val(data.cities[0].Name);
        return true;
    }

    function ChangeState(zipCode, state) {
        ddlCities.html('');
        ddlHouseEstate.html('');
        txtCity.val('');
        txtHouseEstate.val('');

        if (manualSearch && Number(state) === 0) {
            return;
        }

        DisplayLoading();
        IHomeServices.GetLocations(zipCode, state, null, function (data) {
            HideFullScreen();
            if (!HandleCities(data)) {
                return;
            }

            HandleHouseEstates(data);
        }, HideFullScreen);
    }

    function ChangeCity(zipCode, city) {
        ddlHouseEstate.html('');
        txtHouseEstate.val('');

        DisplayLoading();
        IHomeServices.GetLocations(zipCode, null, city, function (data) {
            HandleHouseEstates(data);
            HideFullScreen();
        }, HideFullScreen);
    }

    var lastZipCode = null;
    function ChangeZipCode(zipCode, setCurrentHouseEstate) {
        if (lastZipCode == zipCode) {
            return;
        }

        ddlCities.html('');
        ddlHouseEstate.html('');
        txtState.val('');
        txtCity.val('');
        txtHouseEstate.val('');

        DisplayLoading();
        IHomeServices.GetLocations(zipCode, null, null, function (data) {
            HideFullScreen();


            lastZipCode = zipCode;

            if (data.states.length == 0 && !manualSearch) {
                HandleUnknownZipCode();
                return;
            }

            if (data.states.length > 1 && !manualSearch) {
                ddlStates.html('');
                for (s in data.states) {
                    var state = data.states[s];
                    ddlStates.append($('<option value="' + state.ID + '">' + state.Name + '</option>'));
                }
                txtState.hide().val('');
                var s = undefined;

                ddlStates.show();
                ddlCities.show();
                ddlHouseEstate.show();

                txtState.hide();
                txtCity.hide();
                txtHouseEstate.hide();
                return;
            }
            if (!manualSearch) {
                ddlStates.hide();
                txtState.show().val(data.states[0].Name);
            }

            if (!HandleCities(data)) {
                return;
            }

            HandleHouseEstates(data, setCurrentHouseEstate);
        }, HandleUnknownZipCode);
    }

    $(function () {
        $('#<%= lblHouseEstateNotFound.ClientID %>').hide();
        var ddlHouseEstateLength = ddlHouseEstate.children('option').length;
        var ddlCitiesLength = ddlCities.children('option').length;
        var ddlStatesLength = ddlStates.children('option').length;

        if (ddlHouseEstateLength > 0) {
            txtHouseEstate.hide();
            ddlHouseEstate.show();
        }
        else {
            txtHouseEstate.show();
            ddlHouseEstate.hide();
        }

        if (ddlCitiesLength > 0) {
            txtCity.hide();
            ddlCities.show();
        }
        else {
            txtCity.show();
            ddlCities.hide();
        }

        if (ddlStatesLength > 0) {
            txtState.hide();
            ddlStates.show();
        }
        else {
            txtState.show();
            ddlStates.hide();
        }


        $('#<%= txtZipCode.ClientID %>').blur(function () {
            var zipCode = $(this).val();
            if (zipCode.length == 0) {
                return false;
            }
            if (zipCode.length < 5) {
                var necessaryZeros = 5 - zipCode.length;
                for (i = 0; i < necessaryZeros; i++) {
                    zipCode = '0' + zipCode;
                }
                $('#<%= txtZipCode.ClientID %>').val(zipCode);
            }
            ChangeZipCode(zipCode);
        });

        ddlStates.change(function () {
            var zipCode = $('#<%= txtZipCode.ClientID %>').val();
            var state = $(this).val();
            $('#<%= hfState.ClientID %>').val(state);
            ChangeState(zipCode, state);
        });

        ddlCities.change(function () {
            var zipCode = $('#<%= txtZipCode.ClientID %>').val();
            var city = $(this).val();
            $('#<%= hfCity.ClientID %>').val(city);
            ChangeCity(zipCode, city);
        });

        ddlHouseEstate.change(function () {
            $('#<%= hfHouseEstate.ClientID %>').val($(this).val());
        });

        $('#<%= ddlClient.ClientID %>').change(function () {
            var clientType = Number($('#<%= ddlClient.ClientID %>').val());
            var divBuilding = $('#divBuilding');
            var divContents = $('#divContents');
            var txtValue = $('#<%=txtValue.ClientID %>');
            var txtContents = $('#<%=txtContents.ClientID %>');

            if (clientType === Number(<%= ApplicationConstants.RENTER_TYPE %>)) {
                divContents.show();
                divBuilding.hide();
                txtValue.val('');
                ValidatorValidate($get('<%= valContents.ClientID %>'));
            }
            else if (clientType === Number(<%= ApplicationConstants.LANDLORD_TYPE %>)) {
                divContents.hide();
                divBuilding.show();
                txtContents.val('');
                ValidatorValidate($get('<%= valValue.ClientID %>'));
            }
            else {
                divBuilding.show();
                divContents.show();
                ValidatorValidate($get('<%= valValue.ClientID %>'));
                ValidatorValidate($get('<%= valContents.ClientID %>'));
            }
        });

        $("input[name$=grpPersonType]").change(function () {
            var type = getSelectedPersonType();
            var apobj = $get('<%= this.txtLastName.ClientID %>');
            var validator = $get('<%= this.reqLast.ClientID %>');
            var lblLastName = $get('<%= this.lblLastName.ClientID %>');
            if (!(apobj && validator))
                return;
            if (type == "rbnIndividual") {
                apobj.disabled = false;
                $('#divLastName').show();
                $('#divLastName2').show();
                ValidatorEnable(validator, true);
            } else {
                apobj.value = "";
                apobj.disabled = true;
                $('#divLastName').hide();
                $('#divLastName2').hide();
                ValidatorEnable(validator, false);
            }
            ValidatorValidate($get('<%= this.reqPersonType.ClientID %>'));
        });

        $("input[name$=grpCurrency]").change(function () {
            var valValue = $get('<%= valValue.ClientID %>');
            var valContents = $get('<%= valContents.ClientID %>');
            ValidatorValidate(valValue);
            ValidatorValidate(valContents);
            ValidatorValidate($get('<%= this.reqCurrency.ClientID %>'));
        });

        if (hfHouseEstate.val() && txtZipCode.val()) {
            ChangeZipCode(txtZipCode.val(), true);
        }
        else if (txtZipCode.val()) {
            ChangeZipCode(txtZipCode.val());
        }

        $("input[name$=grpPersonType]").trigger('change');
        $('#<%= ddlClient.ClientID %>').trigger('change');
        $('#<%= reqRisk.ClientID %>').hide();
        $('#<%= reqLast.ClientID %>').hide();
    });

</script>
