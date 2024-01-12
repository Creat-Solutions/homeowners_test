<%@ Page Language="C#" MasterPageFile="~/HouseMaster.master" AutoEventWireup="true" CodeFile="DirectCapture.aspx.cs" Inherits="Application_DirectCapture" Title="Direct HO Input" %>

<asp:Content ContentPlaceHolderID="workloadContent" ID="contentForm" runat="server">

    <%--<script type="text/javascript" src="../jquery-ui/development-bundle/jquery-1.4.2.js"></script>
    <script type="text/javascript" src="../jquery-ui/development-bundle/ui/jquery.ui.core.js"></script>
    <script type="text/javascript" src="../jquery-ui/development-bundle/ui/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="../jquery-ui/development-bundle/ui/jquery.ui.datepicker.js"></script>
    <script type="text/javascript" src="../jquery-ui/general.js"></script>--%>

    <script type="text/javascript">

        <%--function DateToday() {
            var today = new Date();
            today.setHours(0, 0, 0, 0);
            return today;
        }

        function ValidateStartDate(obj, args) {
            var time = Date.parse(args.Value);
            if (isNaN(time)) {
                args.IsValid = false;
                return;
            }
        }

        function ValidateEndDate(obj, args) {
            var endTime = Date.parse(args.Value);
            if (isNaN(endTime)) {
                args.IsValid = false;
                return;
            }

            var parts = txtStart.val().split('/');
            var startTime = new Date(parts[2], parts[0] - 1, parts[1]);
            if (isNan(startTime)) {
                args.IsValid = false;
                return;
            }

            if (startTime > endTime) {
                args.IsValid = false;
                return;
            }
        }

        function OnDateChangeFailure(error) {
            alert("Could not save or an unknown error occurred!");
            return false;
        }

        function OnDateChangeSuccess(result) {
            if (result != "0") {
                $get('<%= txtEnd.ClientID %>').innerHTML = result;
                return true;
            } else {
                OnFailure(null);
            }
        }--%>

        //function DateChange() {
        //    var endD = txtEnd.val();

        //    if (endD == '' || endD == undefined) {
        //        var parts = txtStart.val().split('/');
        //        var startDate = new Date(parts[2], parts[0] - 1, parts[1]);
        //        var endDate = startDate;
        //        endDate.setFullYear(endDate.getFullYear() + 1);
        //        var endDateString = (endDate.getMonth() + 1).toString() + "/" + endDate.getDate().toString() + "/" + endDate.getFullYear().toString();
        //        txtEnd.val(endDateString);
        //    }
        //}

        $(function () {
            //var dates = $('#txtFechaIni, #txtFechaFin').datepicker({
            //    defaultDate: "+1w",
            //    onSelect: function (selectedDate) {
            //        var option = this.id == "txtFechaIni" ? "minDate" : "maxDate";
            //        var instance = $(this).data("datepicker");
            //        var date = $.datepicker.parseDate(instance.settings.dateFormat || $.datepicker._defaults.dateFormat, selectedDate, instance.settings);
            //        dates.not(this).datepicker("option", option, date);
            //    }
            //});

            //calcular();
        });

        function calcular() {
            var net = parseFloat(document.getElementById("ctl00_workloadContent_txtNetPremium").value);
            var fee = parseFloat(document.getElementById("ctl00_workloadContent_txtPolicyFee").value);
            var bfe = parseFloat(document.getElementById("ctl00_workloadContent_txtBrokerFee").value);
            var mas = parseFloat(document.getElementById("ctl00_workloadContent_txtSpecialCoverageFee").value);
            var rxpf = parseFloat(document.getElementById("ctl00_workloadContent_txtRXPF").value);
            var agencyFee = parseFloat(document.getElementById("ctl00_workloadContent_txtAgencyFee").value);

            var taxp = parseFloat(document.getElementById("ctl00_workloadContent_ddlTaxPercentage").value) / 100;
            var tax = (net + fee + rxpf) * taxp;

            var total = net + fee + bfe + mas + tax + rxpf;

            document.getElementById('<%= txtMexicanTax.ClientID %>').value = formatter.format(tax);
            document.getElementById('<%= txtTotalPremium.ClientID %>').value = formatter.format(total);

            var mas = parseFloat(document.getElementById("ctl00_workloadContent_txtSpecialCoverageFee").value);
            var brokerPer = parseFloat(document.getElementById("ctl00_workloadContent_txtAgentCom").value) / 100;
            var mePer = parseFloat(document.getElementById("ctl00_workloadContent_txtMECom").value) / 100;

            var brokerCom = (net + mas) * brokerPer;
            document.getElementById("ctl00_workloadContent_txtTotalCommission").value = formatter.format(brokerCom);
            var meCom = (net + mas) * mePer;
            document.getElementById("ctl00_workloadContent_txtUSCommission").value = formatter.format(meCom);

            var totCom = brokerPer + mePer;
            document.getElementById("ctl00_workloadContent_lblTotComPer").value = totCom * 100;
            totCom = brokerCom + meCom;
            document.getElementById("ctl00_workloadContent_lblTotCom").value = formatter.format(totCom);

            var tot = total - brokerCom;

            document.getElementById("ctl00_workloadContent_txtAmountDue").value = formatter.format(tot);

            var totalAgencyFee = fee - agencyFee;
            document.getElementById("ctl00_workloadContent_txtAFee").value = formatter.format(totalAgencyFee);
            var totalLessAgencyFee = tot - totalAgencyFee;
            document.getElementById("ctl00_workloadContent_txtAmountDue").value = formatter.format(totalLessAgencyFee);
        }

        $("input[name$=grpPersonType]").change(function () {
            debugger;
            var type = getSelectedPersonType();
            var apobj = $get('<%= this.txtName.ClientID %>');
            var validator = $get('<%= this.reqLast.ClientID %>');
            var lblLastName = $get('<%= this.txtName.ClientID %>');
            if (!(apobj && validator))
                return;
            if (type == "rbnIndividual") {
                apobj.disabled = false;
                $('#divLastName').show();
                ValidatorEnable(validator, true);
            } else {
                apobj.value = "";
                apobj.disabled = true;
                $('#divLastName').hide();
                ValidatorEnable(validator, false);
            }
            ValidatorValidate($get('<%= this.reqPersonType.ClientID %>'));
        });

        $("input[name$=grpPersonType]").trigger('change');

        var formatter = new Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: 'USD',
        });

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
            //document.getElementById("ctl00_workloadContent_txtState").value = '';
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

                debugger;

                if (data.states.length > 1 && !manualSearch) {
                    debugger;
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
        });

        function getSelectedPersonType() {
            var myRadio = $("input[name$=grpPersonType]")
            var selectedVal = "";
            var selected = myRadio.filter(':checked').val();
            if (selected.length > 0) {
                selectedVal = selected;
            }
            return selectedVal
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
            var txtLastName = $get('<%= this.txtName.ClientID %>');
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

        function validatePersonType(obj, args) {
            if ($('[name$=grpPersonType]').is(':checked')) {
                args.IsValid = true;
            }
            else {
                args.IsValid = false;
            }
        }

    </script>

    <form id="addClient" method="post">
        <table border="0" cellspacing="0" cellpadding="0" width="95%" align="center">
            <tbody>
                <tr height="23">
                    <td colspan="4">
                        <h3>Input New Policy</h3>
                    </td>
                </tr>
                <tr>
                    <td class="claro" colspan="4">
                        <div class="claro">
                            &nbsp;&nbsp;All fields are required.
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="claro" align="right">Policy No.&nbsp;</td>
                    <td class="claro" colspan="3">
                        <asp:TextBox ID="txtPolicyNo" runat="server" CssClass="text ui-widget-content ui-corner-all" MaxLength="32"></asp:TextBox>
                        <asp:Label ID="lblPolicyNo" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
                        <asp:Label Style="z-index: 0" ID="lblErrorNoPoliza" runat="server" Font-Size="XX-Small" ForeColor="Red" Visible="False">Policy Number already exists.</asp:Label>
                        <asp:RegularExpressionValidator ID="rexPolicyno" runat="server" ControlToValidate="txtPolicyNo" ErrorMessage="Please type only letters, numbers and &quot;-&quot;" ValidationExpression="[0-9a-zA-Z\-/,\s]+" Display="Dynamic"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td class="claro" align="right">Person Type&nbsp;</td>
                    <td class="claro">
                        <asp:RadioButton Checked="True" ID="rbnIndividual" GroupName="grpPersonType" runat="server" /><asp:Literal ID="litIndividual" Text="Individual" runat="server" />
                        <asp:RadioButton ID="rbnCorporation" GroupName="grpPersonType" runat="server" /><asp:Literal ID="litCorporation" Text="Corporation" runat="server" />
                        <asp:CustomValidator ID="reqPersonType" ValidateEmptyText="true" ClientValidationFunction="validatePersonType" Display="Dynamic" OnServerValidate="reqPerson_ServerValidate" Text="Required" ForeColor="" CssClass="failureText" runat="server" />
                </tr>
                </td>
                <tr height="23">
                    <td class="claro" align="right">Name&nbsp;</td>
                    <td class="claro" colspan="3">
                        <asp:TextBox ID="txtName" runat="server" MaxLength="32" Width="260px" CssClass="text ui-widget-content ui-corner-all"></asp:TextBox>
                        <asp:Label ID="lblInsuredName" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
                    </td>
                </tr>
                <tr height="23" id="divLastName">
                    <td class="claro" align="right">Last Name&nbsp;</td>
                    <td class="claro" colspan="3">
                        <asp:TextBox ID="txtLastName" runat="server" MaxLength="32" Width="260px" CssClass="text ui-widget-content ui-corner-all"></asp:TextBox>
                        <%--<asp:Label ID="lblLastName" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>--%>
                        <asp:CustomValidator ID="reqLast" Text="Required" Display="Dynamic" ValidateEmptyText="True" OnServerValidate="txtLastName_ServerValidate" ClientValidationFunction="validateLastName" ControlToValidate="txtLastName" runat="server" />
                        <asp:RegularExpressionValidator ID="cmpLast" Text="Invalid Characters" ControlToValidate="txtLastName" Display="Dynamic" ValidationExpression="^[a-zA-Z0-9\s\.]+$" runat="server" />
                        <user:LengthValidator ID="valLast" Text="Invalid Length Characters" MaxLength="64" Display="Dynamic" ControlToValidate="txtLastName" runat="server" />
                    </td>
                </tr>
                <tr height="23">
                    <td class="claro" align="right">Email&nbsp;</td>
                    <td class="claro" colspan="3">
                        <asp:TextBox ID="TextBox2" runat="server" MaxLength="32" Width="260px" CssClass="text ui-widget-content ui-corner-all"></asp:TextBox>
                        <asp:Label ID="Label3" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
                    </td>
                </tr>
                <tr height="23">
                    <td class="claro" align="right">Phone No.&nbsp;</td>
                    <td class="claro" colspan="3">
                        <asp:TextBox ID="txtPhone" runat="server" MaxLength="32" CssClass="text ui-widget-content ui-corner-all"></asp:TextBox><asp:Label ID="lblPhone" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label></td>
                </tr>
                <tr height="23">
                    <td class="claro" align="right">Zip Code&nbsp;</td>
                    <td class="claro" colspan="3">
                        <asp:TextBox ID="txtZipCode" CssClass="text ui-widget-content ui-corner-all" onkeypress="checkKeyInt(event)" MaxLength="10" runat="server" />
                        <asp:RequiredFieldValidator ID="reqZipCode" Display="Dynamic" ControlToValidate="txtZipCode" Text="Required" runat="server" />
                        <user:LengthValidator ID="lenZipCode" Text="Maximum length for zip code" Display="Dynamic" MaxLength="10" ControlToValidate="txtZipCode" runat="server" />
                        <asp:Label ID="lblHouseEstateNotFound" CssClass="failureText" Text="Invalid Zip Code" runat="server" />
                        <asp:HyperLink ID="HyperLink1" NavigateUrl="https://www.correosdemexico.gob.mx/SSLServicios/ConsultaCP/Descarga.aspx" Target="_blank" Text="Don't know the zip code? Click here" runat="server" />
                    </td>
                </tr>
                <tr height="23">
                    <td class="claro" align="right">State&nbsp;</td>
                    <td class="claro" colspan="3">
                        <asp:TextBox CssClass="text ui-widget-content ui-corner-all" ID="txtState" Style="width: 175px; background-color: #DDD;" ReadOnly="true" runat="server" />
                        <select id="ddlState"></select>
                        <asp:HiddenField ID="hfState" runat="server" />
                    </td>
                </tr>
                <tr height="23">
                    <td class="claro" align="right">City&nbsp;</td>
                    <td class="claro" colspan="3">
                        <asp:TextBox CssClass="text ui-widget-content ui-corner-all" ID="txtCity" Style="width: 175px; background-color: #DDD;" ReadOnly="true" runat="server" />
                        <select id="ddlCity"></select>
                        <asp:HiddenField ID="hfCity" runat="server" />
                    </td>
                </tr>
                <tr height="23">
                    <td class="claro" align="right">House Estate&nbsp;</td>
                    <td class="claro" colspan="3">
                        <asp:TextBox CssClass="text ui-widget-content ui-corner-all" ID="txtHouseEstate" Style="width: 220px; background-color: #DDD;" ReadOnly="true" runat="server" />
                        <select id="ddlHouseEstate" style="width: auto;"></select>
                        <asp:HiddenField ID="hfHouseEstate" runat="server" />
                    </td>
                </tr>
                <tr height="23">
                    <td class="claro" align="right">Insured Street&nbsp;</td>
                    <td class="claro" colspan="3">
                        <asp:TextBox ID="txtAddress" runat="server" MaxLength="255" Width="632px" CssClass="text ui-widget-content ui-corner-all"></asp:TextBox>
                        <asp:Label ID="lblAddress" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
                    </td>
                </tr>
                <tr height="23">
                    <td class="claro" align="right">Insured Exterior No.&nbsp;</td>
                    <td class="claro" colspan="3">
                        <asp:TextBox ID="TextBox3" runat="server" MaxLength="255" Width="632px" CssClass="text ui-widget-content ui-corner-all"></asp:TextBox>
                    </td>
                </tr>
                <tr height="23">
                    <td class="claro" align="right">Insured Interior No.&nbsp;</td>
                    <td class="claro" colspan="3">
                        <asp:TextBox ID="TextBox4" runat="server" MaxLength="255" Width="632px" CssClass="text ui-widget-content ui-corner-all"></asp:TextBox>
                    </td>
                </tr>
                <tr height="23">
                    <td class="claro" align="right">Inception Date&nbsp;</td>
                    <td class="claro">
                        <input id="txtFechaIni"
                            class="text ui-widget-content ui-corner-all" size="7" name="txtFechaIni"
                            runat="server">
                        <asp:Label ID="lblInception" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label></td>
                    <td style="width: 130px" class="claro" align="right">Expiration Date&nbsp;</td>
                    <td class="claro">
                        <input id="txtFechaFin"
                            class="text ui-widget-content ui-corner-all" size="7" name="txtFechaFin"
                            runat="server"><asp:Label ID="lblExpiration" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label></td>
                </tr>
                <tr height="23">
                    <td class="claro" align="right">Insurer&nbsp;</td>
                    <td class="claro">
                        <asp:DropDownList ID="ddlCompany" runat="server" CssClass="text ui-widget-content ui-corner-all" DataTextField="description" DataValueField="company" AutoPostBack="True"></asp:DropDownList>

                    </td>
                    <td style="width: 130px" class="claro" align="right">Type</td>
                    <td class="claro">
                        <asp:DropDownList ID="ddlproduct" runat="server" CssClass="text ui-widget-content ui-corner-all" DataTextField="productdescription" DataValueField="product" AutoPostBack="True"></asp:DropDownList>
                        <asp:Label ID="lblProduct" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small"></asp:Label>

                    </td>
                </tr>
                <tr>
                    <td class="claro" align="right">Agent&nbsp;</td>
                    <td class="claro">
                        <asp:DropDownList ID="ddlAgent" runat="server" CssClass="text ui-widget-content ui-corner-all" DataTextField="name" DataValueField="agent" AutoPostBack="True"></asp:DropDownList><
                        /td>
                    <td style="width: 130px" class="claro" align="right">Description&nbsp;</td>
                        <td class="claro" rowspan="3">
                            <asp:TextBox ID="txtDescription" runat="server" CssClass="text ui-widget-content ui-corner-all" Width="300px" Height="54px" TextMode="MultiLine"></asp:TextBox><asp:Label ID="lblErrorDescription" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>

                        </td>
                </tr>
                <tr>
                    <td class="claro" valign="middle" align="right">Branch&nbsp;</td>
                    <td style="width: 336px" class="claro" valign="middle" colspan="2">
                        <asp:DropDownList ID="ddlBranch" runat="server" CssClass="text ui-widget-content ui-corner-all" DataTextField="address" DataValueField="branch" AutoPostBack="True"></asp:DropDownList>

                    </td>
                </tr>
                <tr>
                    <td class="claro" valign="middle" align="right">User&nbsp;</td>
                    <td style="width: 336px" class="claro" valign="middle" colspan="2">
                        <asp:DropDownList ID="ddlUser" runat="server" CssClass="text ui-widget-content ui-corner-all" DataTextField="name" DataValueField="cveUsuario" AutoPostBack="True"></asp:DropDownList>

                    </td>
                </tr>
                <tr>
                    <td class="claro" colspan="4">&nbsp;&nbsp;<b>Premium Breakdown</b> </td>
                </tr>
                <tr>
                    <td class="claro" valign="middle" align="right">Net 
            Premium&nbsp;</td>
                    <td class="claro" valign="middle">
                        <asp:TextBox onblur="calcular()" ID="txtNetPremium" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all">0</asp:TextBox><asp:Label ID="lblNetPremium" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label></td>
                    <td style="width: 130px" class="claro" valign="middle" align="right">Agent Com. %&nbsp;</td>
                    <td class="claro" valign="middle">
                        <asp:TextBox onblur="calcular()" ID="txtAgentCom" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all">0</asp:TextBox><asp:Label ID="Label1" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label></td>
                </tr>
                <tr>
                    <td class="claro" valign="middle" align="right">Policy Fee&nbsp;</td>
                    <td class="claro" valign="middle">
                        <asp:TextBox onblur="calcular()" ID="txtPolicyFee" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all">0</asp:TextBox><asp:Label ID="lblPolicyFee" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label></td>
                    <td class="claro" valign="middle" align="right">Agent Commission&nbsp;</td>
                    <td class="claro" valign="middle">
                        <asp:TextBox ID="txtTotalCommission" runat="server" CssClass="inputRight" ForeColor="#404040" BorderStyle="None" EnableViewState="false">0</asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td class="claro" align="right">RxPF</td>
                    <td class="claro">
                        <asp:TextBox onblur="calcular()" ID="txtRXPF"
                            onkeypress="checkKey1(event)" runat="server"
                            CssClass="text ui-widget-content ui-corner-all">0</asp:TextBox>
                        <asp:Label ID="lblRXPF" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
                    </td>
                    <td class="claro" valign="middle" align="right">M&amp;E Com %&nbsp;</td>
                    <td class="claro" valign="middle">
                        <asp:TextBox onblur="calcular()" ID="txtMECom" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all">0</asp:TextBox><asp:Label ID="lblUSCom" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label></td>
                </tr>

                <tr>
                    <td class="claro" valign="middle" align="right">Tax 
          %&nbsp;</td>
                    <td class="claro" valign="middle">
                        <asp:DropDownList onblur="calcular()" ID="ddlTaxPercentage" runat="server" CssClass="text ui-widget-content ui-corner-all">
                            <asp:ListItem Value="0" Selected="True">0%</asp:ListItem>
                            <asp:ListItem Value="11">11%</asp:ListItem>
                            <asp:ListItem Value="16">16%</asp:ListItem>
                        </asp:DropDownList></td>
                    <td style="width: 130px" class="claro" valign="middle" align="right">M&amp;E&nbsp;Commission</td>
                    <td>
                        <asp:TextBox ID="txtUSCommission" onkeypress="checkKey1(event)" runat="server" CssClass="inputRight" ForeColor="#404040" BorderStyle="None" EnableViewState="false">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td class="claro" valign="middle" align="right">Mexican Tax</td>
                    <td>
                        <asp:TextBox ID="txtMexicanTax" runat="server" CssClass="inputRight" ForeColor="#404040" BorderStyle="None" EnableViewState="false">0</asp:TextBox><asp:Label ID="lblMexicanTax" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label></td>
                    <td class="claro" valign="middle" align="right">Total 
            Commission</td>
                    <td class="claro" valign="middle">
                        <asp:TextBox ID="lblTotCom" runat="server" ForeColor="#404040" BorderStyle="None" EnableViewState="false">0</asp:TextBox>&nbsp; 
            (<asp:TextBox ID="lblTotComPer" runat="server" Width="64px" BorderStyle="None" EnableViewState="false" BorderColor="Transparent">0</asp:TextBox>%)</td>
                </tr>
                <tr>
                    <td class="claro" valign="middle" align="right">Special Coverage Fee (MAS)&nbsp;</td>
                    <td class="claro" valign="middle">
                        <asp:TextBox onblur="calcular()" ID="txtSpecialCoverageFee" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all">0</asp:TextBox><asp:Label ID="lblErrorSpecialCoverageFee" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label></td>
                    <td style="width: 130px" class="claro" valign="middle" align="right">Amount&nbsp;Due Policy</td>
                    <td class="claro" valign="middle">
                        <asp:TextBox ID="txtAmountDue" onkeypress="checkKey1(event)" runat="server" CssClass="inputRight" ForeColor="#404040" BorderStyle="None" EnableViewState="false">0</asp:TextBox><asp:Label ID="lblAmountDue" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label></td>
                </tr>
                <tr>
                    <td class="claro" valign="middle" align="right">Managing Agent Broker Fee&nbsp;</td>
                    <td class="claro" valign="middle">
                        <asp:TextBox onblur="calcular()" ID="txtBrokerFee" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all">0</asp:TextBox><asp:Label ID="lblBrokerFee" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label></td>
                    <td class="claro" valign="middle" align="right">Currency</td>
                    <td class="claro" valign="middle">
                        <asp:DropDownList runat="server" ID="ddlCurrency" CssClass="text ui-widget-content ui-corner-all">
                            <asp:ListItem Value="0">-- Select --</asp:ListItem>
                            <asp:ListItem Value="2">US Dollars</asp:ListItem>
                            <asp:ListItem Value="1">MX Pesos</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lblCurrency" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="claro" valign="middle" align="right">Inc. Sis. Pol.&nbsp;</td>
                    <td class="claro" valign="middle">
                        <asp:TextBox onblur="calcular()" ID="txtAgencyFee" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all">0</asp:TextBox>
                        <asp:Label ID="lblAgencyFee" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
                    </td>
                    <td class="claro" valign="middle" align="right">Agency Fee&nbsp;</td>
                    <td class="claro" valign="middle">
                        <asp:TextBox ID="txtAFee" runat="server" CssClass="inputRight" ForeColor="#404040" BorderStyle="None" EnableViewState="false">0</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="claro" valign="middle" align="right">Total 
            Premium&nbsp;</td>
                    <td class="claro" valign="middle">
                        <asp:TextBox ID="txtTotalPremium" onkeypress="checkKey1(event)" runat="server" CssClass="inputRight" ForeColor="#404040" BorderStyle="None" EnableViewState="false">0</asp:TextBox><asp:Label ID="lblTotalPremium" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label></td>
                    <td class="claro" valign="middle" align="right"></td>
                    <td class="claro" valign="middle">
                        <asp:TextBox ID="txtInstSurcharge" onkeypress="checkKey1(event)" runat="server" CssClass="inputRight" Visible="False">0</asp:TextBox><asp:Label ID="lblInstSurcharge" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label></td>
                </tr>

                <tr>
                    <td class="claro" valign="middle" align="right"></td>
                    <td class="claro" valign="middle" colspan="3"></td>
                </tr>
                <tr>
                    <td style="width: 525px" class="LowerLeftMedium" valign="middle"
                        width="98%" colspan="3" align="right">&nbsp; </td>
                    <td class="lowerRight" align="right">
                        <asp:Button ID="btnSave" CssClass="ui-state-default" runat="server" Text="Save"></asp:Button></td>
                </tr>
            </tbody>
        </table>
    </form>
</asp:Content>
