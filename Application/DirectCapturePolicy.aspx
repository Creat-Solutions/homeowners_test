<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DirectCapturePolicy.aspx.cs" Inherits="Application_DirectCapturePolicy" MasterPageFile="~/HouseMaster.master" %>

<asp:Content ContentPlaceHolderID="workloadContent" ID="contentForm" runat="server">
	<div id="insuranceApp">
		<table width="100%">
			<tr height="23">
				<td colspan="4">
					<h3>Input New Policy</h3>
				</td>
			</tr>

			<tr>
				<td class="quoteStep borderStep" valign="top" style="width: 45%;">
					<table class="quoteAppData quoteAppDataAmple">
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="lblPolicyNo" AssociatedControlID="txtPolicyNo" Text="Policy No." runat="server" />
									<asp:TextBox ID="txtPolicyNo" Style="width: 200px;" MaxLength="30" runat="server" CssClass="text ui-widget-content ui-corner-all" />
									<asp:RequiredFieldValidator ID="RequiredPolicyNo" Text="Required" ErrorMessage="Required" Display="Dynamic" ControlToValidate="txtPolicyNo" runat="server" />
									<asp:CustomValidator ID="policyNoVal" OnServerValidate="valPolicy_Validate" runat="server" ErrorMessage="The policy no already exists"></asp:CustomValidator>
									<user:LengthValidator ID="lengthPolicyNo" Text="Invalid length" MaxLength="30" Display="Dynamic" ControlToValidate="txtPolicyNo" runat="server" />
									<asp:RegularExpressionValidator ID="rexPolicyno" runat="server" ControlToValidate="txtPolicyNo" ErrorMessage="Please type only letters, numbers and &quot;-&quot;" ValidationExpression="[0-9a-zA-Z\-/,\s]+" Display="Dynamic"></asp:RegularExpressionValidator>
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="lblPersonType" AssociatedControlID="rbnIndividual" Text="Person Type" runat="server" />
									<asp:RadioButton Checked="True" ID="rbnIndividual" GroupName="grpPersonType" runat="server" />
									<asp:Literal ID="litIndividual" Text="Individual" runat="server" />
									<asp:RadioButton ID="rbnCorporation" GroupName="grpPersonType" runat="server" />
									<asp:Literal ID="litCorporation" Text="Corporation" runat="server" />
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="lblName" AssociatedControlID="txtName" Text="Name" runat="server" />
									<asp:TextBox ID="txtName" Style="width: 200px;" MaxLength="100" runat="server" CssClass="text ui-widget-content ui-corner-all" />
									<asp:RequiredFieldValidator ID="reqName" Text="Required" Display="Dynamic" ControlToValidate="txtName" runat="server" />
									<user:LengthValidator ID="valName" Text="Invalid length" MaxLength="100" Display="Dynamic" ControlToValidate="txtName" runat="server" />
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div id="divLastName" class="row step1">
									<asp:Label ID="lblLastName" AssociatedControlID="txtLastName" Text="Last Name" runat="server" />
									<asp:TextBox ID="txtLastName" Style="width: 200px;" EnableViewState="false" MaxLength="64" runat="server" CssClass="text ui-widget-content ui-corner-all" />
									<%--<asp:RequiredFieldValidator ID="reqLastName" Text="Required" Display="Dynamic" ControlToValidate="txtLastName" runat="server" />--%>
									<asp:RegularExpressionValidator ID="cmpLast" Text="Invalid characters" ControlToValidate="txtLastName" Display="Dynamic" ValidationExpression="^[a-zA-Z0-9\s\.]+$" runat="server" />
									<user:LengthValidator ID="valLast" Text="Invalid length" MaxLength="64" Display="Dynamic" ControlToValidate="txtLastName" runat="server" />
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div id="divLastName2" class="row step1">
									<asp:Label ID="lblLastName2" AssociatedControlID="txtLastName2" Text="Second Last Name" runat="server" />
									<asp:TextBox ID="txtLastName2" Style="width: 200px;" EnableViewState="false" MaxLength="64" runat="server" CssClass="text ui-widget-content ui-corner-all" />
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="lblEmail" AssociatedControlID="txtEmail" Text="Email" runat="server" />
									<asp:TextBox ID="txtEmail" Style="width: 200px;" MaxLength="128" runat="server" CssClass="text ui-widget-content ui-corner-all" />
									<asp:RequiredFieldValidator ID="reqFieldsEmail" Text="Required" Display="Dynamic" ControlToValidate="txtEmail" runat="server" />
									<asp:RegularExpressionValidator ID="regexEmail" ValidationExpression=".+@.+\..+" Text="Invalid Email" ControlToValidate="txtEmail" Display="Dynamic" runat="server" />
									<user:LengthValidator ID="valEmail" Text="Invalid length" MaxLength="128" Display="Dynamic" ControlToValidate="txtEmail" runat="server" />
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="lblPhone" AssociatedControlID="txtAreaPhone" Text="Phone No." runat="server" />
									<asp:TextBox ID="txtAreaPhone" onkeypress="checkKeyInt(event)" MaxLength="3" Columns="4" onkeyup="setPhone1Focus();" runat="server" CssClass="text ui-widget-content ui-corner-all" />
									<asp:TextBox ID="txtPhone1" onkeypress="checkKeyInt(event)" onkeyup="setPhone2Focus();" MaxLength="4" Columns="4" runat="server" CssClass="text ui-widget-content ui-corner-all" />
									<asp:TextBox ID="txtPhone2" Columns="6" MaxLength="4" onkeypress="checkKeyInt(event)" runat="server" CssClass="text ui-widget-content ui-corner-all" />
									<asp:RequiredFieldValidator ID="reqFieldsPhone" Text="Required" Display="Dynamic" ControlToValidate="txtPhone2" runat="server" />
									<asp:CompareValidator ID="comPhoneA" Display="Dynamic" Text="Invalid length" ControlToValidate="txtAreaPhone" Type="Integer" Operator="DataTypeCheck" runat="server" />
									<asp:CompareValidator ID="comPhone1" Display="Dynamic" Text="Invalid length" ControlToValidate="txtPhone1" Type="Integer" Operator="DataTypeCheck" runat="server" />
									<asp:CompareValidator ID="comPhone2" Display="Dynamic" Text="Invalid length" ControlToValidate="txtPhone2" Type="Integer" Operator="DataTypeCheck" runat="server" />
								</div>
							</td>
						</tr>

					</table>
				</td>

				<td class="quoteStep" valign="top">
					<table class="quoteAppData">
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="lblZip" AssociatedControlID="txtZipCode" Text="Zip Code" runat="server" />
									<asp:TextBox ID="txtZipCode" Style="width: 70px;" onkeypress="checkKeyInt(event)" MaxLength="5" runat="server" CssClass="text ui-widget-content ui-corner-all" />
									<asp:RequiredFieldValidator ID="reqZipCode" Display="Dynamic" ControlToValidate="txtZipCode" Text="Required" runat="server" />
									<user:LengthValidator ID="lenZipCode" Text="Maximum length for zip code" Display="Dynamic" MaxLength="5" ControlToValidate="txtZipCode" runat="server" />
									<asp:Label ID="lblHouseEstateNotFound" CssClass="failureText" Text="There was an error retrieving the zip code information" runat="server" />
									<asp:HyperLink ID="HyperLink1" NavigateUrl="https://www.correosdemexico.gob.mx/SSLServicios/ConsultaCP/Descarga.aspx" Target="_blank" Text="Check zip codes" runat="server" />
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="lblState" AssociatedControlID="txtState" Text="State" runat="server" />
									<asp:TextBox ID="txtState" Style="width: 220px; background-color: #DDD;" ReadOnly="true" runat="server" CssClass="text ui-widget-content ui-corner-all" />
									<select id="ddlState"></select>
									<asp:HiddenField ID="hfState" runat="server" />
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="lblCity" AssociatedControlID="txtCity" Text="City" runat="server" />
									<asp:TextBox ID="txtCity" Style="width: 220px; background-color: #DDD;" ReadOnly="true" runat="server" CssClass="text ui-widget-content ui-corner-all" />
									<select id="ddlCity"></select>
									<asp:HiddenField ID="hfCity" runat="server" />
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="lblColony" AssociatedControlID="txtHouseEstate" Text="House Estate" runat="server" />
									<asp:TextBox ID="txtHouseEstate" Style="width: 220px; background-color: #DDD;" ReadOnly="true" runat="server" CssClass="text ui-widget-content ui-corner-all" />
									<select id="ddlHouseEstate" style="width: auto;"></select>
									<asp:HiddenField ID="hfHouseEstate" runat="server" />
									<asp:Label ID="lblErrorHouseEstate" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Select an option</asp:Label>
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="lblAddress" AssociatedControlID="txtAddress" Text="Street" runat="server" />
									<asp:TextBox ID="txtAddress" MaxLength="255" Style="width: 220px;" runat="server" CssClass="text ui-widget-content ui-corner-all" />
									<asp:RequiredFieldValidator ID="reqStreet" Text="Required" Display="Dynamic" ControlToValidate="txtAddress" runat="server" />
									<user:LengthValidator ID="lenAddress" Text="Invalid length" MaxLength="255" Display="Dynamic" ControlToValidate="txtAddress" runat="server" />
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="lblInsuredExtNo" AssociatedControlID="txtInsuredExtNo" Text="Exterior No." runat="server" />
									<asp:TextBox ID="txtInsuredExtNo" Style="width: 70px;" MaxLength="10" runat="server" CssClass="text ui-widget-content ui-corner-all" />
									<asp:RequiredFieldValidator ID="reqInsuredExtNo" Text="Required" Display="Dynamic" ControlToValidate="txtInsuredExtNo" runat="server" />
									<user:LengthValidator ID="valInsuredExtNo" Text="Invalid length" MaxLength="10" Display="Dynamic" ControlToValidate="txtInsuredExtNo" runat="server" />
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="lblInsuredIntNo" AssociatedControlID="txtInsuredIntNo" Text="Interior No." runat="server" />
									<asp:TextBox ID="txtInsuredIntNo" Style="width: 70px;" MaxLength="10" runat="server" CssClass="text ui-widget-content ui-corner-all" />
									<user:LengthValidator ID="valInsuredIntNo" Text="Invalid length" MaxLength="10" Display="Dynamic" ControlToValidate="txtInsuredIntNo" runat="server" />
								</div>
							</td>
						</tr>
						<tr>
							<td></td>
						</tr>

					</table>
				</td>
			</tr>

			<tr>
				<td class="quoteStep borderStep" valign="top" style="width: 45%;">
					<table class="quoteAppData quoteAppDataAmple">
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="lblStart" Text="Start Date" AssociatedControlID="txtStart" runat="server" />
									<asp:TextBox ID="txtStart" Style="width: 60px;" runat="server" CssClass="text ui-widget-content ui-corner-all" />
									<ajax:CalendarExtender ID="calStart" TargetControlID="txtStart" runat="server" />
									<asp:CompareValidator ID="valStart" ControlToValidate="txtStart" Display="Dynamic" Text="Invalid Date" Operator="DataTypeCheck" Type="Date" runat="server" />
									<asp:RequiredFieldValidator ID="reqStart" Text="Required" Display="Dynamic" ControlToValidate="txtStart" runat="server" />
									<asp:CustomValidator ID="valCStart" Text="Invalid Date" Display="Dynamic" ControlToValidate="txtStart" OnServerValidate="valStart_Validate" runat="server" />
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1" id="divCompanies" runat="server">
									<asp:Label ID="lblCompany" Text="Company" AssociatedControlID="ddlCompany" runat="server" />
									<asp:DropDownList ID="ddlCompany" Style="width: 200px;"
										runat="server"
										CssClass="text ui-widget-content ui-corner-all"
										DataTextField="description"
										DataValueField="company"
										AutoPostBack="True"
										OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
									</asp:DropDownList>
									<asp:RequiredFieldValidator ID="reqCompany" Text="Required" Display="Dynamic" ControlToValidate="ddlCompany" runat="server" />
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1" id="divAgents" runat="server">
									<asp:Label ID="lblAgent" Text="Agent" AssociatedControlID="ddlAgent" runat="server" />
									<asp:DropDownList ID="ddlAgent" Style="width: 200px;"
										runat="server"
										CssClass="text ui-widget-content ui-corner-all"
										DataTextField="name"
										DataValueField="agent"
										AutoPostBack="True"
										OnSelectedIndexChanged="ddlAgent_SelectedIndexChanged">
									</asp:DropDownList>
									<asp:RequiredFieldValidator ID="reqAgent" Text="Required" Display="Dynamic" ControlToValidate="ddlAgent" runat="server" />
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1" id="divBranches" runat="server">
									<asp:Label ID="lblBranch" Text="Branch" AssociatedControlID="ddlBranch" runat="server" />
									<asp:DropDownList ID="ddlBranch" Style="width: 200px;"
										runat="server"
										CssClass="text ui-widget-content ui-corner-all"
										DataTextField="address"
										DataValueField="branch"
										AutoPostBack="True"
										OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
									</asp:DropDownList>
									<asp:RequiredFieldValidator ID="reqBranch" Text="Required" Display="Dynamic" ControlToValidate="ddlBranch" runat="server" />
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1" id="divUsers" runat="server">
									<asp:Label ID="lblUser" Text="User" AssociatedControlID="ddlUser" runat="server" />
									<asp:DropDownList ID="ddlUser" Style="width: 200px;"
										runat="server"
										CssClass="text ui-widget-content ui-corner-all"
										DataTextField="name"
										DataValueField="cveUsuario"
										AutoPostBack="True">
									</asp:DropDownList>
									<asp:RequiredFieldValidator ID="reqUser" Text="Required" Display="Dynamic" ControlToValidate="ddlUser" runat="server" />
								</div>
							</td>
						</tr>
					</table>
				</td>
				<td class="quoteStep" valign="top">
					<table class="quoteAppData">
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="lblEnd" Text="End Date" AssociatedControlID="txtEnd" runat="server" />
									<asp:TextBox ID="txtEnd" Style="width: 60px;" runat="server" CssClass="text ui-widget-content ui-corner-all" />
									<ajax:CalendarExtender ID="calEnd" TargetControlID="txtEnd" runat="server" />
									<asp:CompareValidator ID="valEnd" ControlToValidate="txtEnd" Display="Dynamic" Text="Invalid Date" Operator="DataTypeCheck" Type="Date" runat="server" />
									<asp:RequiredFieldValidator ID="reqEnd" Text="Required" Display="Dynamic" ControlToValidate="txtEnd" runat="server" />
									<asp:CustomValidator ID="valCEnd" Text="Invalid Date" Display="Dynamic" ControlToValidate="txtEnd" runat="server" />
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1" id="divProducts" runat="server">
									<asp:Label ID="lblProduct" Text="Product" AssociatedControlID="ddlProduct" runat="server" />
									<asp:DropDownList ID="ddlProduct" Style="width: 220px;"
										runat="server"
										CssClass="text ui-widget-content ui-corner-all"
										DataTextField="productdescription"
										DataValueField="product"
										AutoPostBack="True"
										ReadOnly="true"
										Enabled="false">
									</asp:DropDownList>
									<asp:RequiredFieldValidator ID="reqProduct" Text="Required" Display="Dynamic" ControlToValidate="ddlProduct" runat="server" />
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1" id="divDescription" runat="server">
									<asp:Label ID="lblDescription" runat="server" Text="Description" AssociatedControlID="txtDescription"></asp:Label>
									<asp:TextBox ID="txtDescription" runat="server" Style="width: 220px;" CssClass="text ui-widget-content ui-corner-all" Height="54px" TextMode="MultiLine"></asp:TextBox>
									<asp:RequiredFieldValidator ID="requireFieldDescription" Text="Required" Display="Dynamic" ControlToValidate="txtDescription" runat="server" />
								</div>
							</td>
						</tr>

						<%--<tr>
														<td>
																<div class="row step1">
																</div>
														</td>
												</tr>--%>
					</table>
				</td>
			</tr>

			<tr id="errorGeneral" visible="false" runat="server">
				<td class="quoteStep borderStep" valign="top" style="width: 45%;">
					<table class="quoteAppData quoteAppDataAmple">
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="lblErrorInformation" Text="Error" Style="width: 200px;" runat="server" CssClass="text ui-widget-content ui-corner-all" />
								</div>
							</td>
						</tr>
					</table>
				</td>
			</tr>

			<%--<tr id="divDescription" runat="server">
								<td class="quoteStep" valign="top" style="width: 45%;">
										<table class="quoteAppData">
												<tr>
														<td>
																<div class="row step1">
																		<asp:Label ID="lblDescription" runat="server" Text="Description" AssociatedControlID="txtDescription"></asp:Label>
																		<asp:TextBox ID="txtDescription" runat="server" CssClass="text ui-widget-content ui-corner-all" Height="54px" TextMode="MultiLine" Style="width: 200px;"></asp:TextBox>
																		<asp:RequiredFieldValidator ID="requireFieldDescription" Text="Required" Display="Dynamic" ControlToValidate="txtDescription" runat="server" />
																</div>
														</td>
												</tr>
										</table>
								</td>
						</tr>--%>

			<tr height="23">
				<td colspan="4">
					<h3>Premium Breakdown</h3>
				</td>
			</tr>

			<tr>
				<td class="quoteStep borderStep" valign="top" style="width: 45%;">
					<table class="quoteAppData quoteAppDataAmple">
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="labelNetPremium" AssociatedControlID="txtNetPremium" Text="Net Premium" runat="server" />
									<asp:TextBox onblur="calcular()" ID="txtNetPremium" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all">0</asp:TextBox>
									<asp:Label ID="lblNetPremium" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="labelPolicyFee" AssociatedControlID="txtPolicyFee" Text="Policy Fee" runat="server" />
									<asp:TextBox onblur="calcular()" ID="txtPolicyFee" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all">0</asp:TextBox>
									<asp:Label ID="lblPolicyFee" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="labelRXPF" AssociatedControlID="txtRXPF" Text="RxPF" runat="server" />
									<asp:TextBox onblur="calcular()" ID="txtRXPF" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all">0</asp:TextBox>
									<asp:Label ID="lblRXPF" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="labelTaxPercentage" AssociatedControlID="ddlTaxPercentage" Text="Tax %" runat="server" />
									<asp:DropDownList onblur="calcular()" ID="ddlTaxPercentage" runat="server" CssClass="text ui-widget-content ui-corner-all">
										<asp:ListItem Value="0" Selected="True">0%</asp:ListItem>
										<asp:ListItem Value="11">11%</asp:ListItem>
										<asp:ListItem Value="16">16%</asp:ListItem>
									</asp:DropDownList>
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="labelMexicanTax" AssociatedControlID="txtMexicanTax" Text="Mexican Tax" runat="server" />
									<asp:TextBox ID="txtMexicanTax" runat="server" CssClass="text ui-widget-content ui-corner-all" ForeColor="#404040" BorderStyle="None" ReadOnly="false" Enabled="false" EnableViewState="false">0</asp:TextBox>
									<asp:Label ID="lblMexicanTax" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
								</div>
							</td>
						</tr>
						<tr id="hdnSpecialCoverageFee" visible="False" runat="server">
							<td>
								<div class="row step1">
									<asp:Label ID="labelSpecialCoverageFee" AssociatedControlID="txtSpecialCoverageFee" Text="Special Coverage Fee (MAS)" runat="server" />
									<asp:TextBox onblur="calcular()" ID="txtSpecialCoverageFee" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all">0</asp:TextBox>
									<asp:Label ID="lblErrorSpecialCoverageFee" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
								</div>
							</td>
						</tr>
						<tr id="hdnManagingAgentBrokerFee" visible="False" runat="server">
							<td>
								<div class="row step1">
									<asp:Label ID="labelBrokerFee" AssociatedControlID="txtBrokerFee" Text="Managing Agent Broker Fee" runat="server" />
									<asp:TextBox onblur="calcular()" ID="txtBrokerFee" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all">0</asp:TextBox>
									<asp:Label ID="lblBrokerFee" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
								</div>
							</td>
						</tr>
						<tr id="hdnIncSisPol" visible="False" runat="server">
							<td>
								<div class="row step1">
									<asp:Label ID="labelAgencyFee" AssociatedControlID="txtAgencyFee" Text="Inc. Sis. Pol." runat="server" />
									<asp:TextBox onblur="calcular()" ID="txtAgencyFee" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all">0</asp:TextBox>
									<asp:Label ID="lblAgencyFee" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="labelTotalPremium" AssociatedControlID="txtTotalPremium" Text="Total Premium" runat="server" />
									<asp:TextBox ID="txtTotalPremium" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all" ForeColor="#404040" BorderStyle="None" ReadOnly="false" Enabled="false" EnableViewState="false">0</asp:TextBox>
									<asp:Label ID="lblTotalPremium" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="labelCurrency" AssociatedControlID="ddlCurrency" Text="Currency" runat="server" />
									<asp:DropDownList runat="server" ID="ddlCurrency" CssClass="text ui-widget-content ui-corner-all">
										<asp:ListItem Value="0">-- Select --</asp:ListItem>
										<asp:ListItem Value="2">US Dollars</asp:ListItem>
										<asp:ListItem Value="1">MX Pesos</asp:ListItem>
									</asp:DropDownList>
									<asp:Label ID="lblCurrency" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
								</div>
							</td>
						</tr>
					</table>
				</td>

				<td class="quoteStep" valign="top">
					<table class="quoteAppData">
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="labelAgentComPercentage" AssociatedControlID="txtAgentCom" Text="Agent Com. %" runat="server" />
									<asp:TextBox onblur="calcular()" ID="txtAgentCom" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all">0</asp:TextBox>
									<asp:Label ID="Label1" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="labelTotalCommission" AssociatedControlID="txtTotalCommission" Text="Agent Commission" runat="server" />
									<asp:TextBox ID="txtTotalCommission" runat="server" CssClass="text ui-widget-content ui-corner-all" ForeColor="#404040" BorderStyle="None" ReadOnly="false" Enabled="false" EnableViewState="false">0</asp:TextBox>
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="labelUSCom" AssociatedControlID="txtMECom" Text="M&E Com %" runat="server" />
									<asp:TextBox onblur="calcular()" ID="txtMECom" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all">0</asp:TextBox>
									<asp:Label ID="lblUSCom" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="labelUSComission" AssociatedControlID="txtUSCommission" Text="M&E Commission" runat="server" />
									<asp:TextBox ID="txtUSCommission" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all" ForeColor="#404040" BorderStyle="None" ReadOnly="false" Enabled="false" EnableViewState="false">0</asp:TextBox>
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="labelTotCom" AssociatedControlID="lblTotCom" Text="Total Commission" runat="server" />
									<asp:TextBox ID="lblTotCom" runat="server" ForeColor="#404040" BorderStyle="None" ReadOnly="false" Enabled="false" EnableViewState="false">0</asp:TextBox>&nbsp;(
																		<asp:TextBox ID="lblTotComPer" runat="server" Width="64px" BorderStyle="None" ReadOnly="false" Enabled="false" EnableViewState="false" BorderColor="Transparent">0</asp:TextBox>%)
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="labelAmountDue" AssociatedControlID="txtAmountDue" Text="Amount Due Policy" runat="server" />
									<asp:TextBox ID="txtAmountDue" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all" ForeColor="#404040" BorderStyle="None" ReadOnly="false" Enabled="false" EnableViewState="false">0</asp:TextBox>
									<asp:Label ID="lblAmountDue" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="labelAFee" AssociatedControlID="txtAFee" Text="Agency Fee" runat="server" />
									<asp:TextBox ID="txtAFee" runat="server" CssClass="text ui-widget-content ui-corner-all" ForeColor="#404040" BorderStyle="None" ReadOnly="false" Enabled="false" EnableViewState="false">0</asp:TextBox>
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div class="row step1">
									<asp:Label ID="labelInstSurcharge" AssociatedControlID="txtInstSurcharge" Text="&nbsp; " runat="server" />
									<asp:TextBox ID="txtInstSurcharge" onkeypress="checkKey1(event)" runat="server" CssClass="text ui-widget-content ui-corner-all" Visible="False">0</asp:TextBox>
									<asp:Label ID="lblInstSurcharge" runat="server" Visible="False" ForeColor="Red" Font-Size="XX-Small">Required</asp:Label>
								</div>
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<asp:HiddenField ID="hfMexicanTax" runat="server" />
			<asp:HiddenField ID="hfTotalPremium" runat="server" />
			<asp:HiddenField ID="hfAgentCommission" runat="server" />
			<asp:HiddenField ID="hfmeCommission" runat="server" />
			<asp:HiddenField ID="hftotalCommission" runat="server" />
			<asp:HiddenField ID="hfpercentageComission" runat="server" />
			<asp:HiddenField ID="hfAmountDuePolicy" runat="server" />
			<asp:HiddenField ID="hfAgencyFee" runat="server" />
		</table>
	</div>

	<div style="position: relative;">
		<asp:Button runat="server" ID="btnSave" Text="Save" OnClick="btnSave_Click"></asp:Button>
	</div>

	<script type="text/javascript">
		function valFields(obj, args) {
			if ($.trim($('#<%= txtEmail.ClientID %>').val()).length === 0 &&
				($.trim($('#<%= txtAreaPhone.ClientID %>').val()).length < 2 ||
					$.trim($('#<%= txtPhone1.ClientID %>').val()).length < 3 ||
				$.trim($('#<%= txtPhone2.ClientID %>').val()).length < 4)) {
				args.IsValid = false;
			} else {
				$('#<%= reqFieldsEmail.ClientID %>, #<%= reqFieldsPhone.ClientID %>').hide();
				args.IsValid = true;
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

		function validatePersonType(obj, args) {
			if ($('[name$=grpPersonType]').is(':checked')) {
				args.IsValid = true;
			}
			else {
				args.IsValid = false;
			}
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

		function checkKey1(e) {
			var keyChar;

			if (isNav == 0) {
				if (e.which < 48 || e.which > 57) e.RETURNVALUE = false;
			} else {
				if ((e.keyCode < 48 || e.keyCode > 57) && e.keyCode != 46 && e.keyCode != 45) e.returnValue = false;
			}
		}

		function ValidChar(e) {
			var RegX = "[0-9a-zA-Z\-/,\s]+";
			var regex = new RegExp(RegX);

			alert(e.value);

			if (!regex.test(e.value)) {
				alert('Please enter a valid data.');
				return false;
			}
		}

		function IsNumeric(sText, obj) {
			var ValidChars = "0123456789";
			var IsNumber = true;
			var Char;
			var sVAL

			Char = sText.charAt(0);

			if (Char == "." && obj.value.indexOf('.') > -1)
				IsNumber = false;
			else {
				if (ValidChars.indexOf(Char) == -1) {
					IsNumber = false;
				}
			}
			return IsNumber;
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

		function getSelectedPersonType() {
			var myRadio = $("input[name$=grpPersonType]")
			var selectedVal = "";
			var selected = myRadio.filter(':checked').val();
			if (selected.length > 0) {
				selectedVal = selected;
			}
			return selectedVal
		}

		var formatter = new Intl.NumberFormat('en-US', {
			style: 'currency',
			currency: 'USD',
		});

		function calcular() {
			//var net = parseFloat(document.getElementById("ctl00_workloadContent_txtNetPremium").value);
			//var fee = parseFloat(document.getElementById("ctl00_workloadContent_txtPolicyFee").value);
			//var bfe = parseFloat(document.getElementById("ctl00_workloadContent_txtBrokerFee").value);
			//var mas = parseFloat(document.getElementById("ctl00_workloadContent_txtSpecialCoverageFee").value);
			//var rxpf = parseFloat(document.getElementById("ctl00_workloadContent_txtRXPF").value);
			//var agencyFee = parseFloat(document.getElementById("ctl00_workloadContent_txtAgencyFee").value);

			//var taxp = parseFloat(document.getElementById("ctl00_workloadContent_ddlTaxPercentage").value) / 100;

			var net = $('#<%= txtNetPremium.ClientID %>');
			var fee = $('#<%= txtPolicyFee.ClientID %>');
			//var bfe = $('#<%= txtBrokerFee.ClientID %>');
			//var mas = $('#<%= txtSpecialCoverageFee.ClientID %>');
			var rxpf = $('#<%= txtRXPF.ClientID %>');
			//var agencyFee = $('#<%= txtAgencyFee.ClientID %>');
			var taxp = $('#<%= ddlTaxPercentage.ClientID %>');// / 100;

			var tax = (parseFloat(net.val()) + parseFloat(fee.val()) + parseFloat(rxpf.val())) * (parseFloat(taxp.val()) / 100);

			var total = parseFloat(net.val()) + parseFloat(fee.val()) + /*parseFloat(bfe.val()) + parseFloat(mas.val()) +*/ tax + parseFloat(rxpf.val());

			<%--document.getElementById('<%= txtMexicanTax.ClientID %>').value = formatter.format(tax);
			document.getElementById('<%= txtTotalPremium.ClientID %>').value = formatter.format(total);--%>
			var mexTax = $('#<%= txtMexicanTax.ClientID %>');
			var totalPre = $('#<%= txtTotalPremium.ClientID %>');

			mexTax.val(formatter.format(tax));
			totalPre.val(formatter.format(total));

			//var mas = parseFloat(document.getElementById("ctl00_workloadContent_txtSpecialCoverageFee").value);
			//var brokerPer = parseFloat(document.getElementById("ctl00_workloadContent_txtAgentCom").value) / 100;
			//var mePer = parseFloat(document.getElementById("ctl00_workloadContent_txtMECom").value) / 100;
			var agentCom = $('#<%= txtAgentCom.ClientID %>');// / 100;
			var MECom = $('#<%= txtMECom.ClientID %>');// / 100;
			var brokerPer = parseFloat(agentCom.val()) / 100;
			var mePer = parseFloat(MECom.val()) / 100;

			var brokerCom = parseFloat(net.val()) /*+ parseFloat(mas.val()))*/ * brokerPer;
			var tTotCom = $('#<%= txtTotalCommission.ClientID %>');
			tTotCom.val(formatter.format(brokerCom));
			//document.getElementById("ctl00_workloadContent_txtTotalCommission").value = formatter.format(brokerCom);
			var mCom = parseFloat(net.val()) /*+ parseFloat(mas.val()))*/ * mePer;

			var usCom = $('#<%= txtUSCommission.ClientID %>');
			usCom.val(formatter.format(mCom));
			//document.getElementById("ctl00_workloadContent_txtUSCommission").value = formatter.format(meCom);

			var toCom = brokerPer + mePer;
			var totComPer = $('#<%= lblTotComPer.ClientID %>');
			totComPer.val(toCom * 100);
			//document.getElementById("ctl00_workloadContent_lblTotComPer").value = totCom * 100;
			var totCom = brokerCom + mCom;
			var lTotCom = $('#<%= lblTotCom.ClientID %>');
			lTotCom.val(formatter.format(totCom));
			//document.getElementById("ctl00_workloadContent_lblTotCom").value = formatter.format(totCom);

			var tot = total - brokerCom;

			var amountDue = $('#<%= txtAmountDue.ClientID %>');
			amountDue.val(formatter.format(tot));
			//document.getElementById("ctl00_workloadContent_txtAmountDue").value = formatter.format(tot);

			var totalAgencyFee = parseFloat(fee.val()); //- parseFloat(agencyFee.val());
			var aFee = $('#<%= txtAFee.ClientID %>');
			aFee.val(formatter.format(totalAgencyFee));
			//document.getElementById("ctl00_workloadContent_txtAFee").value = formatter.format(totalAgencyFee);
			var totalLessAgencyFee = tot - totalAgencyFee;
			//var aDue = $('#<%= txtAmountDue.ClientID %>');
			amountDue.val(formatter.format(totalLessAgencyFee));
			//document.getElementById("ctl00_workloadContent_txtAmountDue").value = formatter.format(totalLessAgencyFee);

			$('#<%= hfMexicanTax.ClientID %>').val(mexTax.val());
			$('#<%= hfTotalPremium.ClientID %>').val(totalPre.val());
			$('#<%= hfAgentCommission.ClientID %>').val(tTotCom.val());
			$('#<%= hfmeCommission.ClientID %>').val(usCom.val());
			$('#<%= hftotalCommission.ClientID %>').val(lTotCom.val());
			$('#<%= hfpercentageComission.ClientID %>').val(totComPer.val());
			$('#<%= hfAmountDuePolicy.ClientID %>').val(amountDue.val());
			//$('#<%= hfAgencyFee.ClientID %>').val(aFee.val());
		}

		$(function () {
			calcular();

			$("input[name$=grpPersonType]").change(function () {
				var type = getSelectedPersonType();
				var apobj = $get('<%= this.txtLastName.ClientID %>');
				var lblLastName = $get('<%= this.lblLastName.ClientID %>');
				if (!(apobj))
					return;
				if (type == "rbnIndividual") {
					apobj.disabled = false;
					$('#divLastName').show();
					$('#divLastName2').show();
				} else {
					apobj.value = "";
					apobj.disabled = true;
					$('#divLastName').hide();
					$('#divLastName2').hide();
				}
			});

			$("input[name$=grpPersonType]").trigger('change');

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

			if (hfHouseEstate.val() && txtZipCode.val()) {
				ChangeZipCode(txtZipCode.val(), true);
			}
			else if (txtZipCode.val()) {
				ChangeZipCode(txtZipCode.val());
			}
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

	</script>
</asp:Content>
