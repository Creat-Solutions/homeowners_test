<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GenerateStep4.ascx.cs" Inherits="UserControls_GenerateStep4" %>

<table width="100%">
	<tr>
		<td class="quoteStep borderStep" valign="top" style="width: 40%;">
			<table class="quoteAppData quoteAppDataAmple">
				<tr>
					<td>
						<h1>
							<asp:Literal ID="Literal1" Text="<%$ Resources:, strPolicyInfo %>" runat="server" /></h1>
						<div class="row step4">
							<asp:Label ID="lblInsuredStreet" AssociatedControlID="txtInsuredStreet" Text="<%$ Resources:, strInsuredStreet %>" runat="server" />
							<asp:TextBox ID="txtInsuredStreet" CssClass="step4LeftTop" MaxLength="50" runat="server" />
							<asp:RequiredFieldValidator ID="reqInsuredStreet" Text="<%$ Resources:, strRequired %>" Display="Dynamic" ControlToValidate="txtInsuredStreet" runat="server" />
							<user:LengthValidator ID="valInsuredStreet" Text="<%$ Resources:, strValLength %>" MaxLength="50" Display="Dynamic" ControlToValidate="txtInsuredStreet" runat="server" />
						</div>
						<div class="row step4">
							<asp:Label ID="lblInsuredExtNo" AssociatedControlID="txtInsuredExtNo" Text="<%$ Resources:, strInsuredExtNo %>" runat="server" />
							<asp:TextBox ID="txtInsuredExtNo" CssClass="step4LeftTop" MaxLength="10" runat="server" />
							<asp:RequiredFieldValidator ID="reqInsuredExtNo" Text="<%$ Resources:, strRequired %>" Display="Dynamic" ControlToValidate="txtInsuredExtNo" runat="server" />
							<user:LengthValidator ID="valInsuredExtNo" Text="<%$ Resources:, strValLength %>" MaxLength="10" Display="Dynamic" ControlToValidate="txtInsuredExtNo" runat="server" />
						</div>
						<div class="row step4">
							<asp:Label ID="lblInsuredIntNo" AssociatedControlID="txtInsuredIntNo" Text="<%$ Resources:, strInsuredIntNo %>" runat="server" />
							<asp:TextBox ID="txtInsuredIntNo" CssClass="step4LeftTop" MaxLength="10" runat="server" />
							<user:LengthValidator ID="valInsuredIntNo" Text="<%$ Resources:, strValLength %>" MaxLength="10" Display="Dynamic" ControlToValidate="txtInsuredIntNo" runat="server" />
						</div>
					</td>
				</tr>
				<tr>
					<td>
						<div class="row step4">
							<asp:Label ID="lblFax" AssociatedControlID="txtAreaFax" Text="Fax: " runat="server" />
							<asp:TextBox ID="txtAreaFax" onkeypress="checkKeyInt(event)" MaxLength="3" Columns="3" onkeyup="setFax1Focus();" runat="server" />
							<asp:TextBox ID="txtFax1" onkeypress="checkKeyInt(event)" onkeyup="setFax2Focus();" MaxLength="4" Columns="3" runat="server" /><asp:TextBox ID="txtFax2" Columns="4" MaxLength="4" onkeypress="checkKeyInt(event)" runat="server" />
							<asp:CompareValidator ID="comFaxA" Display="Dynamic" Text="<%$ Resources:, strValCharacters %>" ControlToValidate="txtAreaFax" Type="Integer" Operator="DataTypeCheck" runat="server" />
							<asp:CompareValidator ID="comFax1" Display="Dynamic" Text="<%$ Resources:, strValCharacters %>" ControlToValidate="txtFax1" Type="Integer" Operator="DataTypeCheck" runat="server" />
							<asp:CompareValidator ID="comFax2" Display="Dynamic" Text="<%$ Resources:, strValCharacters %>" ControlToValidate="txtFax2" Type="Integer" Operator="DataTypeCheck" runat="server" />
							<asp:CustomValidator ID="valFax" ValidateEmptyText="true" ControlToValidate="txtFax2" ClientValidationFunction="validateFax" Display="Dynamic"
								OnServerValidate="validateFax_ServerValidate" Text="<%$ Resources:, strValLength %>" ForeColor="" CssClass="failureText"
								runat="server" />
						</div>
					</td>
				</tr>
				<tr>
					<td>
						<div class="row step4">
							<asp:Label ID="lblTaxID" AssociatedControlID="txtRFC" Text="<%$ Resources:, strTaxID %>" runat="server" />
							<asp:TextBox ID="txtRFC" CssClass="step4LeftTop" EnableViewState="false" MaxLength="13" runat="server" />
							<asp:CustomValidator ID="valRFC" ValidateEmptyText="true" Display="Dynamic" ClientValidationFunction="validateRFC" OnServerValidate="valRFC_ServerValidate" ControlToValidate="txtRFC" Text="<%$ Resources:, strRequired%>" runat="server" />
							<user:LengthValidator ID="lenRFC" Display="Dynamic" MaxLength="13" Text="<%$ Resources:, strValFormat%>" ControlToValidate="txtRFC" runat="server" />
						</div>
					</td>
				</tr>
				<tr>
					<td>
						<div class="row step4">
							<asp:Label ID="lblAdditionalInsured" AssociatedControlID="txtAdditionalInsured" Text="<%$ Resources:, strAdditionalInsured %>" runat="server" />
							<asp:TextBox ID="txtAdditionalInsured" CssClass="step4LeftTop" MaxLength="100" runat="server" />
							<user:LengthValidator ID="lenAdditionalInsured" MaxLength="100" Display="Dynamic" Text="<%$ Resources:, strValLength %>" ControlToValidate="txtAdditionalInsured" runat="server" />
						</div>
					</td>
				</tr>
				<tr>
					<td>
						<div class="row step4">
							<asp:Label ID="lblBankName" AssociatedControlID="txtBankName" Text="<%$ Resources:, strBankName %>" runat="server" />
							<asp:TextBox ID="txtBankName" CssClass="step4LeftTop" MaxLength="128" runat="server" />
							<user:LengthValidator ID="valBankName" Display="Dynamic" Text="<%$ Resources:, strValLength %>" MaxLength="128" ControlToValidate="txtBankName" runat="server" />
						</div>
					</td>
				</tr>
				<tr>
					<td>
						<div class="row step4">
							<asp:Label ID="lblBankAddress" AssociatedControlID="txtBankAddress" Text="<%$ Resources:, strBankAddress %>" runat="server" />
							<asp:TextBox ID="txtBankAddress" CssClass="step4LeftTop" MaxLength="254" runat="server" />
							<asp:CustomValidator ID="valBankAddress" ClientValidationFunction="bankInformation_Validate" ControlToValidate="txtBankAddress" Text="<%$ Resources:, strRequired %>" Display="Dynamic" ValidateEmptyText="true" OnServerValidate="valBankInformation_Validate" runat="server" />
							<user:LengthValidator ID="valBankAddress2" Text="Input too long" MaxLength="254" ControlToValidate="txtBankAddress" Display="Dynamic" runat="server" />
						</div>
					</td>
				</tr>
				<tr>
					<td>
						<div class="row step4">
							<asp:Label ID="lblLoanNumber" AssociatedControlID="txtLoanNumber" Text="<%$ Resources:, strLoanNumber %>" runat="server" />
							<asp:TextBox ID="txtLoanNumber" CssClass="step4LeftTop" MaxLength="32" runat="server" />
							<asp:CustomValidator ID="valLoanNumber" ClientValidationFunction="bankInformation_Validate" ControlToValidate="txtLoanNumber" Text="<%$ Resources:, strRequired %>" Display="Dynamic" ValidateEmptyText="true" OnServerValidate="valBankInformation_Validate" runat="server" />
							<asp:CustomValidator ID="valLoanNumber2" ClientValidationFunction="validateAlphanumeric" OnServerValidate="txtLoanNumber_Validate" Text="<%$ Resources:, strValFormat %>" Display="Dynamic" ControlToValidate="txtLoanNumber" runat="server" />
						</div>
					</td>
				</tr>
				<tr>
					<td>
						<h1>
							<asp:Literal ID="Literal2" Text="<%$ Resources:, strArt140 %>" runat="server" /></h1>
						<div class="row step4">
							<asp:Label runat="server" ID="lblDOB" Text="<%$ Resources:, strDOB %>" AssociatedControlID="ddlMonth" />
							<select class="select" id="ddlMonth" onchange="setDays()" runat="server">
							</select>
							<select class="select" id="ddlDay" name="ddlDay" runat="server">
							</select>
							<select class="select" id="ddlYear" onchange="setDays()" name="ddlYear" runat="server">
							</select>
							<asp:HyperLink ID="lnkDOB" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?help140=DOB" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" />
						</div>
						<div class="row step4">
							<asp:Label runat="server" ID="lblEmail" Text="<%$ Resources:, strEmail %>" AssociatedControlID="txtEmail" />
							<asp:TextBox ID="txtEmail" CssClass="step4Right" MaxLength="128" runat="server" />
							<asp:RegularExpressionValidator ID="regexEmail" ValidationExpression=".+@.+\..+" Text="<%$ Resources:, strValEmail %>" ControlToValidate="txtEmail" Display="Dynamic" runat="server" />
							<asp:CustomValidator ID="valEmail" ValidateEmptyText="true" ClientValidationFunction="validateEmail" OnServerValidate="valEmail_ServerValidate" Text="<%$ Resources:, strRequired %>" Display="Dynamic" ControlToValidate="txtEmail" runat="server" />
							<user:LengthValidator ID="lenEmail" Text="<%$ Resources:, strValLength %>" MaxLength="128" Display="Dynamic" ControlToValidate="txtEmail" runat="server" />
							<asp:HyperLink ID="lnkEmail" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?help140=Email" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" />
						</div>
						<div class="row step4">
							<asp:Label runat="server" ID="lblPhone" Text="<%$ Resources:, strPhone %>" AssociatedControlID="txtAreaPhone" />
							<asp:TextBox ID="txtAreaPhone" onkeypress="checkKeyInt(event)" MaxLength="4" Columns="3" onkeyup="setPhone1Focus();" runat="server" />
							<asp:TextBox ID="txtPhone1" onkeypress="checkKeyInt(event)" onkeyup="setPhone2Focus();" MaxLength="4" Columns="3" runat="server" /><asp:TextBox ID="txtPhone2" Columns="4" MaxLength="4" onkeypress="checkKeyInt(event)" runat="server" />
							<asp:CompareValidator ID="CompareValidator1" Display="Dynamic" Text="<%$ Resources:, strValCharacters %>" ControlToValidate="txtAreaPhone" Type="Integer" Operator="DataTypeCheck" runat="server" />
							<asp:CompareValidator ID="CompareValidator2" Display="Dynamic" Text="<%$ Resources:, strValCharacters %>" ControlToValidate="txtPhone1" Type="Integer" Operator="DataTypeCheck" runat="server" />
							<asp:CompareValidator ID="CompareValidator3" Display="Dynamic" Text="<%$ Resources:, strValCharacters %>" ControlToValidate="txtPhone2" Type="Integer" Operator="DataTypeCheck" runat="server" />
							<asp:CustomValidator ID="valPhone" ValidateEmptyText="true" ControlToValidate="txtPhone2" ClientValidationFunction="validatePhone" Display="Dynamic"
								OnServerValidate="valPhone_ServerValidate" Text="<%$ Resources:, strRequired %>" ForeColor="" CssClass="failureText"
								runat="server" />
							<asp:HyperLink ID="lnkPhone" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?help140=Phone" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" />
						</div>
					</td>
				</tr>
				<tr class="over2500">
					<td>
						<div class="row step4">
							<asp:Label ID="lblNationality" AssociatedControlID="txtNationality" Text="<%$ Resources:, strNationality%>" runat="server" />
							<asp:TextBox ID="txtNationality" CssClass="step4Right" MaxLength="100" runat="server" />
							<asp:CustomValidator ID="reqNationality" ValidateEmptyText="true" ClientValidationFunction="validateNationality" OnServerValidate="valNationality_ServerValidate" Text="<%$ Resources:, strRequired %>" Display="Dynamic" ControlToValidate="txtNationality" runat="server" />
							<user:LengthValidator ID="valNationality" Text="<%$ Resources:, strValLength %>" MaxLength="100" Display="Dynamic" ControlToValidate="txtNationality" runat="server" />
							<asp:HyperLink ID="lnkBuildingQuest" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?help140=Nationality" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" />
						</div>
					</td>
				</tr>
				<tr class="over2500">
					<td>
						<div class="row step4">
							<asp:Label ID="lblOccupation" AssociatedControlID="txtOccupation" Text="<%$ Resources:, strOccupation %>" runat="server" />
							<asp:TextBox ID="txtOccupation" CssClass="step4Right" MaxLength="100" runat="server" />
							<asp:CustomValidator ID="reqOccupation" ValidateEmptyText="true" ClientValidationFunction="validateOccupation" OnServerValidate="valOccupation_ServerValidate" Text="<%$ Resources:, strRequired %>" Display="Dynamic" ControlToValidate="txtOccupation" runat="server" />
							<user:LengthValidator ID="valOccupation" Text="<%$ Resources:, strValLength %>" MaxLength="100" Display="Dynamic" ControlToValidate="txtOccupation" runat="server" />
							<asp:HyperLink ID="HyperLink2" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?help140=Occupation" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" />
						</div>
					</td>
				</tr>
				<tr class="corpOnly">
					<td>
						<div class="row step4">
							<asp:Label ID="lblRepresentative" AssociatedControlID="txtRepresentative" Text="<%$ Resources:, strRepresentative %>" runat="server" />
							<asp:TextBox ID="txtRepresentative" CssClass="step4Right" MaxLength="100" runat="server" />
							<asp:CustomValidator ID="reqRepresentative" ValidateEmptyText="true" ClientValidationFunction="validateRepresentative" OnServerValidate="valRepresentative_ServerValidate" Text="<%$ Resources:, strRequired %>" Display="Dynamic" ControlToValidate="txtRepresentative" runat="server" />
							<user:LengthValidator ID="valRepresentative" Text="<%$ Resources:, strValLength %>" MaxLength="100" Display="Dynamic" ControlToValidate="txtRepresentative" runat="server" />
							<asp:HyperLink ID="HyperLink3" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?help140=Representative" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" />
						</div>
					</td>
				</tr>
				<tr class="corpOnly">
					<td>
						<div class="row step4">
							<asp:Label ID="lblID" AssociatedControlID="txtID" Text="<%$ Resources:, strID %>" runat="server" />
							<asp:TextBox ID="txtID" CssClass="step4Right" MaxLength="100" runat="server" />
							<asp:CustomValidator ID="reqID" ValidateEmptyText="true" ClientValidationFunction="validateID" OnServerValidate="valID_ServerValidate" Text="<%$ Resources:, strRequired %>" Display="Dynamic" ControlToValidate="txtID" runat="server" />
							<user:LengthValidator ID="valID" Text="<%$ Resources:, strValLength %>" MaxLength="100" Display="Dynamic" ControlToValidate="txtID" runat="server" />
							<asp:HyperLink ID="HyperLink4" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?help140=ID" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" />
						</div>
					</td>
				</tr>
				<tr class="corpOnly">
					<td>
						<div class="row step4">
							<asp:Label ID="lblIDType" AssociatedControlID="txtIDType" Text="<%$ Resources:, strIDType %>" runat="server" />
							<asp:TextBox ID="txtIDType" CssClass="step4Right" MaxLength="100" runat="server" />
							<asp:CustomValidator ID="reqIDType" ValidateEmptyText="true" ClientValidationFunction="validateIDType" OnServerValidate="valIDType_ServerValidate" Text="<%$ Resources:, strRequired %>" Display="Dynamic" ControlToValidate="txtIDType" runat="server" />
							<user:LengthValidator ID="valIDType" Text="<%$ Resources:, strValLength %>" MaxLength="100" Display="Dynamic" ControlToValidate="txtIDType" runat="server" />
							<asp:HyperLink ID="HyperLink5" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?help140=IDType" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" />
						</div>
					</td>
				</tr>
				<tr class="corpOnly">
					<td>
						<div class="row step4">
							<asp:Label ID="lblIncNumber" AssociatedControlID="txtIncNumber" Text="<%$ Resources:, strIncNumber %>" runat="server" />
							<asp:TextBox ID="txtIncNumber" CssClass="step4Right" MaxLength="100" runat="server" />
							<asp:CustomValidator ID="reqIncNumber" ValidateEmptyText="true" ClientValidationFunction="validateIncNumber" OnServerValidate="valIncNumber_ServerValidate" Text="<%$ Resources:, strRequired %>" Display="Dynamic" ControlToValidate="txtIncNumber" runat="server" />
							<user:LengthValidator ID="valIncNumber" Text="<%$ Resources:, strValLength %>" MaxLength="100" Display="Dynamic" ControlToValidate="txtIncNumber" runat="server" />
							<asp:HyperLink ID="HyperLink6" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?help140=IncNumber" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" />
						</div>
					</td>
				</tr>
				<tr class="corpOnly">
					<td>
						<div class="row step4">
							<asp:Label ID="lblMercNumber" AssociatedControlID="lblMercNumber" Text="<%$ Resources:, strMercNumber %>" runat="server" />
							<asp:TextBox ID="txtMercNumber" CssClass="step4Right" MaxLength="100" runat="server" />
							<asp:CustomValidator ID="reqMercNumber" ValidateEmptyText="true" ClientValidationFunction="validateMercNumber" OnServerValidate="valMercNumber_ServerValidate" Text="<%$ Resources:, strRequired %>" Display="Dynamic" ControlToValidate="txtMercNumber" runat="server" />
							<user:LengthValidator ID="valMercNumber" Text="<%$ Resources:, strValLength %>" MaxLength="100" Display="Dynamic" ControlToValidate="txtMercNumber" runat="server" />
							<asp:HyperLink ID="HyperLink7" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?help140=MercNumber" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" />
						</div>
					</td>
				</tr>
				<tr class="corpOnly">
					<td>
						<div class="row step4">
							<asp:Label ID="lblFIEL" AssociatedControlID="txtFIEL" Text="FIEL: " runat="server" />
							<asp:TextBox ID="txtFIEL" CssClass="step4Right" MaxLength="100" runat="server" />
							<user:LengthValidator ID="valFIEL" Text="<%$ Resources:, strValLength %>" MaxLength="100" Display="Dynamic" ControlToValidate="txtFIEL" runat="server" />
							<asp:HyperLink ID="HyperLink8" ImageUrl="~/images/help.gif" NavigateUrl="~/Application/Helper.ashx?help140=FIEL" onclick="return GB_showCenter('Help', this.href, 500, 500);" runat="server" />
						</div>
					</td>
				</tr>
			</table>
		</td>
		<td class="quoteStep" valign="top">
			<table class="quoteAppData">
				<tr>
					<td>
						<br />
						<asp:UpdatePanel ID="UpdatePanel1" runat="server">
							<ContentTemplate>
								<div>
									<span class="baseRed">
										<asp:Literal ID="litSameAddress" Text="<%$ Resources:, strSameAddress %>" runat="server" /></span>
									<asp:RadioButton ID="btnYes" OnCheckedChanged="CheckedChange" AutoPostBack="true" GroupName="liveOutside" runat="server" />
									<asp:Literal ID="litYes" Text="<%$ Resources:, strYes %>" runat="server" />
									&nbsp;
																<asp:RadioButton ID="btnNo" AutoPostBack="true" OnCheckedChanged="CheckedChange" GroupName="liveOutside" runat="server" />No
																<br />
									<br />
									<asp:UpdatePanel ID="updLiveUsa" runat="server">
										<ContentTemplate>
											<div class="row step4">
												<asp:Label ID="lblCountry" AssociatedControlID="btnUSA" Text="<%$ Resources:, strCountry %>" runat="server" />
												<asp:RadioButton ID="btnUSA" onclick="ChangeUSA(true);" Checked="true" GroupName="liveUSA" runat="server" />
												<asp:Literal ID="litUSA" Text="<%$ Resources:, strUSA %>" runat="server" />
												<asp:RadioButton ID="btnNotUSA" onclick="ChangeUSA(false);" GroupName="liveUSA" runat="server" />
												<asp:Literal ID="litAnotherCountry" Text="<%$ Resources:, strAnotherCountry %>" runat="server" />
											</div>
											<div class="row step4">
												<asp:Label ID="lblCountryText" Style="display: none;" AssociatedControlID="ddlCountry" Text="-" runat="server" />
												<asp:DropDownList ID="ddlCountry" Width="144px" Style="display: none;" runat="server" DataValueField="countryID" DataTextField="countryName">
												</asp:DropDownList>
											</div>
											<div class="row step4">
												<asp:Label ID="lblStreet" AssociatedControlID="txtStreet" Text="<%$ Resources:, strStreet %>" runat="server" />
												<asp:TextBox ID="txtStreet" CssClass="step4LeftBottom" MaxLength="40" runat="server" />
												<asp:RequiredFieldValidator ID="reqStreet" Text="<%$ Resources:, strRequired %>" Display="Dynamic" ControlToValidate="txtStreet" runat="server" />
												<user:LengthValidator ID="valStreet" Text="<%$ Resources:, strValLength %>" MaxLength="40" Display="Dynamic" ControlToValidate="txtStreet" runat="server" />
											</div>
											<div class="row step4">
												<asp:Label ID="lblExtNo" AssociatedControlID="txtExtNo" Text="<%$ Resources:, strExtNo %>" runat="server" />
												<asp:TextBox ID="txtExtNo" CssClass="step4LeftBottom" MaxLength="40" runat="server" />
												<asp:RequiredFieldValidator ID="reqExtNo" Text="<%$ Resources:, strRequired %>" Display="Dynamic" ControlToValidate="txtExtNo" runat="server" />
												<user:LengthValidator ID="valExtNo" Text="<%$ Resources:, strValLength %>" MaxLength="40" Display="Dynamic" ControlToValidate="txtExtNo" runat="server" />
											</div>
											<div class="row step4">
												<asp:Label ID="lblIntNo" AssociatedControlID="txtIntNo" Text="<%$ Resources:, strIntNo %>" runat="server" />
												<asp:TextBox ID="txtIntNo" CssClass="step4LeftBottom" MaxLength="40" runat="server" />
												<user:LengthValidator ID="valIntNo" Text="<%$ Resources:, strValLength %>" MaxLength="40" Display="Dynamic" ControlToValidate="txtIntNo" runat="server" />
											</div>
											<div class="row step4">
												<asp:Label ID="lblState" AssociatedControlID="ddlState" Text="<%$ Resources:, strState %>" runat="server" />
												<asp:DropDownList EnableViewState="false" ID="ddlState" OnDataBound="ddlDataBound" DataTextField="Name" DataValueField="ID" DataSourceID="odsStates" runat="server" />
												<asp:TextBox ID="txtState" CssClass="step4LeftBottom" Style="display: none;" runat="server" />
												<asp:CustomValidator ID="reqState" ClientValidationFunction="ValidateState" OnServerValidate="ValidateState" ValidateEmptyText="true" ControlToValidate="txtState" Text="<%$ Resources:, strRequired %>" runat="server" />
												<user:LengthValidator ID="lenState" Text="<%$ Resources:, strValLength %>" MaxLength="64" Display="Dynamic" ControlToValidate="txtState" runat="server" />
											</div>
											<div class="row step4">
												<asp:Label ID="lblCity" AssociatedControlID="txtCity" Text="<%$ Resources:, strCity %>" runat="server" />
												<asp:TextBox ID="txtCity" CssClass="step4LeftBottom" MaxLength="32" runat="server" />
												<asp:RequiredFieldValidator ID="reqCity" Text="<%$ Resources:, strRequired %>" Display="Dynamic" ControlToValidate="txtCity" runat="server" />
												<user:LengthValidator ID="lenCity" Text="<%$ Resources:, strValLength %>" MaxLength="32" Display="Dynamic" ControlToValidate="txtCity" runat="server" />
											</div>
											<%-- WARNING: Hard coded values
																				If the ID in the DB is changed, the parameter must be updated
											--%>
											<asp:ObjectDataSource ID="odsStates" TypeName="ME.Admon.Country" SelectMethod="GetStates" runat="server">
												<SelectParameters>
													<asp:Parameter Name="country" Type="Int32" DefaultValue="1" />
												</SelectParameters>
											</asp:ObjectDataSource>

											<div class="row step4">
												<asp:Label ID="lblZip" AssociatedControlID="txtZip" Text="<%$ Resources:, strZip %>" runat="server" />
												<asp:TextBox ID="txtZip" CssClass="step4LeftBottom" MaxLength="25" runat="server" />
												<asp:RequiredFieldValidator ID="reqZip" Text="<%$ Resources:, strRequired %>" Display="Dynamic" ControlToValidate="txtZip" runat="server" />
												<user:LengthValidator ID="valZip" Text="<%$ Resources:, strValLength %>" Display="Dynamic" MaxLength="25" ControlToValidate="txtZip" runat="server" />
												<asp:RegularExpressionValidator ID="regZip" Text="<%$ Resources:, strValFormat %>"
													Display="Dynamic" ValidationExpression="^[A-Za-z0-9]+([\s\w\d]*(\([\s\w\d]*\))*)*$"
													ControlToValidate="txtZip" runat="server" />
											</div>
										</ContentTemplate>
									</asp:UpdatePanel>
								</div>
							</ContentTemplate>
						</asp:UpdatePanel>
					</td>
				</tr>
				<tr>
					<td>
						<br />
						<div class="row step4">
							<asp:Label ID="lblStart" Text="Start Date:" AssociatedControlID="txtStart" runat="server" />
							<ajax:CalendarExtender ID="calStart" TargetControlID="txtStart" OnClientDateSelectionChanged="DateChange" runat="server" />
							<asp:TextBox ID="txtStart" runat="server" />
							<asp:RequiredFieldValidator ID="reqStart" Text="Required" Display="Dynamic" ControlToValidate="txtStart" runat="server" />
							<asp:CompareValidator ID="valStart" Text="Invalid Date" Display="Dynamic" ControlToValidate="txtStart" Operator="DataTypeCheck" Type="Date" runat="server" />
							<asp:CustomValidator ID="valCStart" Text="Invalid Date" Display="Dynamic" ClientValidationFunction="ValidateStartDate" ControlToValidate="txtStart" OnServerValidate="valStart_Validate" runat="server" />
						</div>
						<div class="row step4">
							<asp:Label ID="lblEnd" Text="End Date:" AssociatedControlID="lblEndResult" runat="server" />
							<asp:Label ID="lblEndResult" runat="server" />
						</div>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
<br />
<br />
<table width="95%">
	<tr>
		<td align="center" class="quoteStep borderStep" style="width: 50%" valign="top">
			<table class="quoteAppData quoteAppDataAmple">
				<tr>
					<td width="50%" align="left" valign="top">
						<table>
							<tr>
								<td align="right" class="baseRed">
									<asp:Literal ID="litPremium" Text="<%$ Resources:, strPremium %>" runat="server" />
								</td>
								<td>
									<asp:Label ID="lblNetPremium" runat="server" /></td>
							</tr>
							<tr>
								<td align="right" class="baseRed">
									<asp:Literal ID="litFee" Text="Policy Fee (Total):" runat="server" /></td>
								<td>
									<asp:Label ID="lblFee" Visible="false" runat="server" />
									<asp:TextBox ID="txtFee" Visible="false" runat="server" CssClass="step4Fee" onkeypress="checkKeyInt(event)" onblur="calcular()"></asp:TextBox>
									<asp:CustomValidator ID="valFee" ValidateEmptyText="true" Display="Dynamic" OnServerValidate="valFee_ServerValidate" AutoPostBack="True" ControlToValidate="txtFee" runat="server" />
									<%--<asp:Label ID="lblInvalidFee" Display="Dynamyc" ForeColor="Red" Font-Size="Small" runat="server">Invalid</asp:Label>--%>
								</td>
							</tr>
							<tr>
								<td align="right" class="baseRed">
									<asp:Literal ID="litInsCoFee" Text="Ins. Company Fee:" runat="server" />
								</td>
								<td>
									<asp:Label ID="lblInsCoFee" runat="server" />
								</td>
							</tr>
							<tr id="ME_PolicyFee" runat="server" visible="false">
								<td align="right" class="baseRed">
									<asp:Literal ID="litMGAFee" Text="M&E Fee:" runat="server" />
								</td>
								<td>
									<asp:Label ID="lblMGAFee" runat="server" />
								</td>
							</tr>
							<tr>
								<td align="right" class="baseRed">
									<asp:Literal ID="litAgentFee" Text="Agent Fee:" runat="server" />
								</td>
								<td>
									<asp:Label ID="lblAgentFee" runat="server" />
								</td>
							</tr>
							<tr>
								<td align="right" class="baseRed">
									<asp:Literal ID="litRXPF" Text="<%$ Resources:, strRXPF %>" runat="server" /></td>
								<td>
									<asp:Label ID="lblRXPF" runat="server" /></td>
							</tr>
							<tr>
								<td align="right" class="baseRed">
									<asp:Literal ID="litTax" Text="<%$ Resources:, strTax %>" runat="server" /></td>
								<td>
									<asp:Label ID="lblTax" runat="server" /></td>
							</tr>
							<tr>
								<td align="right" class="baseRed">
									<asp:Literal ID="litTotal" Text="<%$ Resources:, strTotal %>" runat="server" /></td>
								<td>
									<asp:Label ID="lblTotal" runat="server" /></td>
							</tr>
							<%--<tr>
								<td align="right" class="baseRed">
									<asp:CustomValidator ID="valFee" ValidateEmptyText="true" Display="Dynamic" OnServerValidate="valFee_ServerValidate" AutoPostBack="True" ControlToValidate="txtFee" runat="server" />
								</td>
							</tr>--%>
						</table>

					</td>
					<td>
						<table>
							<tr>
								<td>&nbsp;</td>
							</tr>
							<tr id="ME_Policy" runat="server" visible="false">
								<td>
									<asp:CheckBox ID="chkLocalPayment" CssClass="nofloat" Checked="false" Text="Local Payment" onclick="calculateFee(this);" runat="server" />
								</td>
							</tr>
							<tr>
								<td>
									<span class="baseRed">
										<asp:Literal ID="litNumPayments" Text="<%$ Resources:, strNumPayments %>" runat="server" />
									</span>
									<select runtat="server" class="select" name="ddlNumPayments" id="ddlNumPayments" style="height: 17px">
										<option value="1">1</option>
										<option value="2">2</option>
										<option value="4">4</option>
									</select>
								</td>
							</tr>
							<tr>
								<td>&nbsp;</td>
							</tr>
							<tr>
								<td>&nbsp;</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td>
						<asp:HyperLink ID="lnkBiyearly" onclick="return GB_showCenter('Breakdown', this.href, 300, 800);" runat="server">View Breakdown</asp:HyperLink>
						<asp:HyperLink ID="lnkQuarterly" onclick="return GB_showCenter('Breakdown', this.href, 300, 800);" runat="server">View Breakdown</asp:HyperLink>
					</td>
				</tr>
			</table>
		</td>
		<td align="center" valign="top">
			<asp:Button ID="btnGenerateCalculo" CausesValidation="false" CssClass="step4Buttons" Text="<%$ Resources:, strGenerateCalculation %>" Visible="false" runat="server" /><br />
			<br />
			<asp:Button ID="btnViewPDF" OnClientClick="return ViewPDF();" CssClass="step4Buttons" Text="PDF Espa&ntilde;ol" runat="server" /><br />
			<br />
			<asp:Button ID="btnViewPDFEnglish" OnClientClick="return ViewPDFEnglish();" CssClass="step4Buttons" Text="PDF English" runat="server" /><br />
			<br />
			<asp:Button ID="btnSaveQuote" CssClass="step4Buttons"
				Text="<%$ Resources:, strSaveQuote %>" runat="server"
				OnClick="btnSaveQuote_Click" />
			<asp:Button ID="btnBuyQuote" CssClass="step4Buttons" Text="<%$ Resources:, strBuyQuote %>" OnClick="btnBuyQuote_Click" runat="server" />
			<br />
			<asp:Label ID="lblMaxSum" ForeColor="DarkRed" Visible="false" runat="server" />
		</td>
		<%--<td align="center" valign="top">
						<br />
						<asp:Label ID="lblCustomFee" Display="Dynamyc" ForeColor="Red" Font-Size="Small" runat="server" Visible="false"></asp:Label>
				</td>--%>
	</tr>
</table>

<asp:HiddenField ID="hdnNumPayments" runat="server" />
<asp:HiddenField ID="hdnTotaltFee" runat="server" />
<asp:HiddenField ID="hdnMGAPolicyFee" runat="server" />
<asp:HiddenField ID="hdnIssuedBy" runat="server" />
<asp:HiddenField ID="hdnCanChangeFee" runat="server" />
<asp:HiddenField ID="hdnMacFeeAgent" runat="server" />
<asp:HiddenField ID="hdnMinPolicyFee" runat="server" />
<asp:HiddenField ID="hdnMaxPolicyFee" runat="server" />
<asp:HiddenField ID="hdnMinPremium" runat="server" />
<asp:HiddenField ID="hdnMaxPremium" runat="server" />

<script type="text/javascript">
	//var lblTotal = $get('<%= lblTotal.ClientID %>');
	//var totalString = lblTotal.innerHTML;
	var hdnNumPayments = $("#<%=hdnNumPayments.ClientID %>");
	var hdnTotaltFee = $("#<%=hdnTotaltFee.ClientID %>");
	var hdnMGAPolicyFee = $("#<%=hdnMGAPolicyFee.ClientID %>");
	var lnkBiyearly = $("#<%=lnkBiyearly.ClientID %>");
	var lnkQuarterly = $("#<%=lnkQuarterly.ClientID %>");
	var txtStart = $("#<%=txtStart.ClientID %>");
	var lblEndResult = $("#<%=lblEndResult.ClientID %>");
	var lblFee = $("#<%=lblFee.ClientID %>");

	var lblNetPremium = $("#<%=lblNetPremium.ClientID %>");
	var txtFee = $("#<%=txtFee.ClientID %>");
	var lblRXPF = $("#<%=lblRXPF.ClientID %>");
	var lblTax = $("#<%=lblTax.ClientID %>");
	var lblTotal = $("#<%=lblTotal.ClientID %>");

	var lblInsCoFee = $("#<%=lblInsCoFee.ClientID %>");
	var lblMGAFee = $("#<%=lblMGAFee.ClientID %>");
	var lblAgentFee = $("#<%=lblAgentFee.ClientID %>");




	var formatter = new Intl.NumberFormat('en-US', {
		style: 'currency',
		currency: 'USD',
	});

	function calcular()
	{
		var meFeeVisible = Boolean($("#<%=hdnMacFeeAgent.ClientID %>"));
		var macFeeV = $get('<%= lblMGAFee.ClientID %>');
		//var net = parseFloat(lblNetPremium.val().replace(/[^0-9\.]+/g, ""));
		var net = parseFloat($get('<%= lblNetPremium.ClientID %>').innerHTML.replace(/[^0-9\.]+/g, ""));
		var fee = parseFloat(txtFee.val());
		var insCoFee = parseFloat($get('<%= lblInsCoFee.ClientID %>').innerHTML.replace(/[^0-9\.]+/g, ""));
		var meFee = macFeeV == undefined ? 0 : parseFloat(macFeeV.innerHTML.replace(/[^0-9\.]+/g, ""));
		var agentFee = parseFloat($get('<%= lblAgentFee.ClientID %>').innerHTML.replace(/[^0-9\.]+/g, ""));
		var rxpf = parseFloat($get('<%= lblRXPF.ClientID %>').innerHTML.replace(/[^0-9\.]+/g, ""));
		var tax = parseFloat($get('<%= lblTax.ClientID %>').innerHTML.replace(/[^0-9\.]+/g, ""));
		var total = parseFloat($get('<%= lblTotal.ClientID %>').innerHTML.replace(/[^0-9\.]+/g, ""));

		var insCo_ME_Fee = Number(insCoFee + meFee);
		var totAgentFee = Number(fee - insCo_ME_Fee)
		lblAgentFee.html(formatter.format(totAgentFee));

		var totTax = Number((net + fee + rxpf) * (16 / 100));
		lblTax.html(formatter.format(totTax));

		var endTotal = Number(net + fee + rxpf) + Number((net + fee + rxpf) * (16 / 100));
		lblTotal.html(formatter.format(endTotal));

		hdnTotaltFee.val(fee);

		OnCloseBox();
	};

	function calculateFee(obj)
	{
		if (!obj)
		{
			return;
		}

		var meFeeVisible = Boolean($("#<%=hdnMacFeeAgent.ClientID %>"));
		//var macFeeV = $get('<%= lblMGAFee.ClientID %>');
		var meFee = Number(hdnMGAPolicyFee.val());//macFeeV == undefined ? 0 : parseFloat(macFeeV.innerHTML.replace(/[^0-9\.]+/g, ""));
		//var net = parseFloat(lblNetPremium.val().replace(/[^0-9\.]+/g, ""));
		var net = parseFloat($get('<%= lblNetPremium.ClientID %>').innerHTML.replace(/[^0-9\.]+/g, ""));
		//var fee = parseFloat(txtFee.val()) - meFee;
		var insCoFee = parseFloat($get('<%= lblInsCoFee.ClientID %>').innerHTML.replace(/[^0-9\.]+/g, ""));
		var agentFee = parseFloat($get('<%= lblAgentFee.ClientID %>').innerHTML.replace(/[^0-9\.]+/g, ""));
		var rxpf = parseFloat($get('<%= lblRXPF.ClientID %>').innerHTML.replace(/[^0-9\.]+/g, ""));
		var tax = parseFloat($get('<%= lblTax.ClientID %>').innerHTML.replace(/[^0-9\.]+/g, ""));
		var total = parseFloat($get('<%= lblTotal.ClientID %>').innerHTML.replace(/[^0-9\.]+/g, ""));

		var fee;
		var insCo_ME_Fee;

		var textboxFee = document.getElementById("<%= txtFee.ClientID %>");

		if (obj.checked)
		{
			textboxFee.disabled = obj.checked;
			console.log("/*** CHECKED **/" + textboxFee.disabled);
			fee = parseFloat(hdnTotaltFee.val()) - meFee;
			console.log("TOTAL FEE: ", + fee);
			console.log("TOTAL M&E_Fee: ", + meFee);
			lblMGAFee.html(formatter.format(0));
			txtFee.val(fee);
			insCo_ME_Fee = Number(insCoFee + 0);
			console.log("TOTAL Insurance_Co_Fee: ", + insCo_ME_Fee);
		}
		else
		{
			textboxFee.disabled = obj.checked;
			console.log("/*** UNCHECKED **/" + textboxFee.disabled);
			fee = parseFloat(hdnTotaltFee.val()) + meFee;
			console.log("TOTAL FEE: ", + fee);
			console.log("TOTAL M&E_Fee: ", + meFee);
			lblMGAFee.html(formatter.format(meFee));
			txtFee.val(fee);
			insCo_ME_Fee = Number(insCoFee + meFee);
			console.log("TOTAL Insurance_Co_Fee: ", + insCo_ME_Fee);
		}

		var totAgentFee = Number(fee - insCo_ME_Fee)
		lblAgentFee.html(formatter.format(totAgentFee));

		var totTax = Number((net + fee + rxpf) * (16 / 100));
		lblTax.html(formatter.format(totTax));

		var endTotal = Number(net + fee + rxpf) + Number((net + fee + rxpf) * (16 / 100));
		lblTotal.html(formatter.format(endTotal));

		hdnTotaltFee.val(fee);



		OnCloseBox();
	};

	function validateAlphanumeric(source, arguments)
	{
		if (arguments.Value.length == 0 || arguments.Value.match(/^[a-zA-Z0-9-_/]+$/))
			arguments.IsValid = true;
		else
			arguments.IsValid = false;
	}

	function ValidateState(obj, args)
	{
		var txtState = $get('<%= txtState.ClientID %>');
		var ddlState = $get('<%= ddlState.ClientID %>');
		var chkNotUSA = $get('<%= btnNotUSA.ClientID %>');
		if (chkNotUSA.checked)
		{
			args.IsValid = txtState.value.length > 0;
		} else
		{
			args.IsValid = ddlState.selectedIndex > 0;
		}
	}

	function bankInformation_Validate(source, args)
	{
		var txtBankName = $get('<%= txtBankName.ClientID %>');
		args.IsValid = true;
		if (txtBankName.value.trim().length > 0)
		{
			args.IsValid = args.Value.trim().length > 0;
		}
	}

	function validateRFC(obj, args)
	{
		var txtRFC = $get('<%= this.txtRFC.ClientID %>');

		if (!(txtRFC))
		{
			args.IsValid = false;
			return;
		}
		if (isCompany())
		{
			args.IsValid = txtRFC.value.length > 0
		}
		else
		{
			args.IsValid = true;
		}
	}

	function DateToday()
	{
		var today = new Date();
		today.setHours(0, 0, 0, 0);
		return today;
	}

	function ValidateStartDate(obj, args)
	{
		var time = Date.parse(args.Value);
		if (isNaN(time))
		{
			args.IsValid = false;
			return;
		}
		var today = DateToday().getTime() - (<%= RETROACTIVE_DAYS %> * 24 * 60 * 60 * 1000);
		args.IsValid = time >= today;
	}

	function OnDateChangeFailure(error)
	{
		alert("Could not save or an unknown error occurred!");
		return false;
	}

	function OnDateChangeSuccess(result)
	{
		if (result != "0")
		{
			$get('<%= lblEndResult.ClientID %>').innerHTML = result;
			return true;
		} else
		{
			OnFailure(null);
		}
	}

	function isCompany()
	{
		return <%= PERSON_TYPE %> == 1;
	}

	function isDollarCurrency()
	{
		return <%= CURRENCY %> == 2;
	}

	function isTotalOver2500()
	{
		var lblTotal = $get('<%= lblTotal.ClientID %>');
		var totalString = lblTotal.innerHTML;
		var total = Number(totalString.replace(/[^0-9\.]+/g, ""));
		var limit = 2500;

		if (!isDollarCurrency())
		{
			limit = limit * <%= EXCHANGE_RATE %>;
		}
		return total > limit;
	}

	function validateNationality(obj, args)
	{
		if (isTotalOver2500())
		{
			txtNationality = $get('<%= txtNationality.ClientID %>');
			args.IsValid = txtNationality.value.length > 0;
		}
		else
		{
			args.IsValid = true;
		}
	}

	function validateEmail(obj, args)
	{
		txtEmail = $get('<%= txtEmail.ClientID %>');
		if (txtEmail.style.display != 'none')
		{
			args.IsValid = txtEmail.value.length > 0;
		}
		else
		{
			args.IsValid = true;
		}
	}

	function validateFax(obj, args)
	{
		txtAreaFax = $get('<%= txtAreaFax.ClientID %>');
		txtFax1 = $get('<%= txtFax1.ClientID %>');
		txtFax2 = $get('<%= txtFax2.ClientID %>');
		if (txtAreaFax.value.length > 0 || txtFax1.value.length > 0 || txtFax2.value.length > 0)
		{
			args.IsValid = (txtAreaFax.value.length >= 2 && txtFax1.value.length >= 3 && txtFax2.value.length == 4);
		}
		else
		{
			args.IsValid = true;
		}
	}

	function validatePhone(obj, args)
	{
		txtAreaPhone = $get('<%= txtAreaPhone.ClientID %>');
		txtPhone1 = $get('<%= txtPhone1.ClientID %>');
		txtPhone2 = $get('<%= txtPhone2.ClientID %>');
		if (txtAreaPhone.style.display != 'none')
		{
			args.IsValid = (txtAreaPhone.value.length >= 2 && txtPhone1.value.length >= 3 && txtPhone2.value.length == 4);
		}
		else
		{
			args.IsValid = true;
		}
	}

	function validateOccupation(obj, args)
	{
		if (isTotalOver2500())
		{
			txtOccupation = $get('<%= txtOccupation.ClientID %>');
			args.IsValid = txtOccupation.value.length > 0;
		}
		else
		{
			args.IsValid = true;
		}
	}

	function validateRepresentative(obj, args)
	{
		if (isTotalOver2500() && isCompany())
		{
			txtRepresentative = $get('<%= txtRepresentative.ClientID %>');
			args.IsValid = txtRepresentative.value.length > 0;
		}
		else
		{
			args.IsValid = true;
		}
	}

	function validateID(obj, args)
	{
		if (isTotalOver2500() && isCompany())
		{
			txtID = $get('<%= txtID.ClientID %>');
			args.IsValid = txtID.value.length > 0;
		}
		else
		{
			args.IsValid = true;
		}
	}

	function validateIDType(obj, args)
	{
		if (isTotalOver2500() && isCompany())
		{
			txtIDType = $get('<%= txtIDType.ClientID %>');
			args.IsValid = txtIDType.value.length > 0;
		}
		else
		{
			args.IsValid = true;
		}
	}

	function validateIncNumber(obj, args)
	{
		if (isTotalOver2500() && isCompany())
		{
			txtIncNumber = $get('<%= txtIncNumber.ClientID %>');
			args.IsValid = txtIncNumber.value.length > 0;
		}
		else
		{
			args.IsValid = true;
		}
	}

	function validateMercNumber(obj, args)
	{
		if (isTotalOver2500() && isCompany())
		{
			txtMercNumber = $get('<%= txtMercNumber.ClientID %>');
			args.IsValid = txtMercNumber.value.length > 0;
		}
		else
		{
			args.IsValid = true;
		}
	}

	function isLeapYear(year)
	{
		return ((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0);
	}

	function setFax1Focus()
	{
		var actual = document.getElementById('<%= txtAreaFax.ClientID %>');
		var next = document.getElementById('<%= txtFax1.ClientID %>');
		var limit = 3;

		if (actual.value.length >= limit && next)
			next.focus();
	}

	function setFax2Focus()
	{
		var area = document.getElementById('<%= txtAreaFax.ClientID %>');
		var actual = document.getElementById('<%= txtFax1.ClientID %>');
		var next = document.getElementById('<%= txtFax2.ClientID %>');
		var limit = 6 - area.value.length;
		if (actual.value.length >= limit && next)
			next.focus();
	}

	function setPhone1Focus()
	{
		var actual = document.getElementById('<%= txtAreaPhone.ClientID %>');
		var next = document.getElementById('<%= txtPhone1.ClientID %>');
		var limit = 3;

		if (actual.value.length >= limit && next)
			next.focus();
	}

	function setPhone2Focus()
	{
		var area = document.getElementById('<%= txtAreaPhone.ClientID %>');
		var actual = document.getElementById('<%= txtPhone1.ClientID %>');
		var next = document.getElementById('<%= txtPhone2.ClientID %>');
		var limit = 6 - area.value.length;
		if (actual.value.length >= limit && next)
			next.focus();
	}

	function getFebruaryDays()
	{
		var yearList = $('#<%= ddlYear.ClientID %>');
		if (typeof yearList.val() === "undefined" || yearList.val() == "")
		{
			return 28;
		}
		if (isLeapYear(Number(yearList.val())))
		{
			return 29;
		}
		else
		{
			return 28;
		}
	}

	function setDays()
	{

		var daysList = $("#<%= ddlDay.ClientID %>");
		var monthList = $('#<%= ddlMonth.ClientID %>');
		var totalDays = 0;
		var origVal = daysList.val();

		switch (monthList.val())
		{
			case "01":
			case "03":
			case "05":
			case "07":
			case "08":
			case "10":
			case "12":
				totalDays = 31;
				break;
			case "04":
			case "06":
			case "09":
			case "11":
				totalDays = 30;
				break;
			case "02":
				totalDays = getFebruaryDays();
				break;
			default:
				totalDays = 31;
				break;
		}

		// empty options from list
		if (daysList[0].length > 0) { daysList[0].length = 0; }
		// create new option object for each entry
		for (var i = 1; i <= totalDays; i++)
		{
			daysList[0].options[i - 1] = new Option(i, i);
		}
		if (daysList[0].length >= origVal)
		{
			daysList[0].options[origVal - 1].selected = true;
		} else
		{
			daysList[0].options[0].selected = true;
		}
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

	function changeCorpLabels()
	{
		var dob = '<%= GetLocalResourceObject("strCorpDOB").ToString() %>';
		var occupation = '<%= GetLocalResourceObject("strCorpOccupation").ToString() %>';
		var lblDOB = $get('<%= lblDOB.ClientID %>');
		var lblOccupation = $get('<%= lblOccupation.ClientID %>');
		lblDOB.innerHTML = dob;
		lblOccupation.innerHTML = occupation;
	}

	function hideControlsByPersonType()
	{
		$('.corpOnly').hide();
	}

	function hideControlsByPrice()
	{
		$('.corpOnly').hide();
		$('.over2500').hide();
	}

	function ChangeUSA(usa)
	{

		var txtState = $get('<%= txtState.ClientID %>');
		var ddlState = $get('<%= ddlState.ClientID %>');
		var ddlCountry = $get('<%= ddlCountry.ClientID %>');
		var lblCountryText = $get('<%= lblCountryText.ClientID %>');
		if (usa)
		{
			txtState.style.display = 'none';
			ddlState.style.display = 'inline';
			ddlCountry.style.display = 'none';
			lblCountryText.style.display = 'none';
		} else
		{
			txtState.style.display = 'inline';
			ddlState.style.display = 'none';
			ddlCountry.style.display = 'inline';
			lblCountryText.style.display = 'inline';
			lblCountryText.style.visibility = 'hidden';
		}

	}

	function ViewPDF()
	{
		window.location.href = '/GetCotiza.ashx?quote=<%= (int)Session["quoteid"] %>';
		return false;
	}

	function ViewPDFEnglish()
	{
		window.location.href = '/GetCotiza.ashx?quote=<%= (int)Session["quoteid"] %>&eng=1';
		return false;
	}

	function OnCloseBox()
	{
		$('#ddlNumPayments').trigger("change");
	}

	function DateChange()
	{
		var parts = txtStart.val().split('/');
		//please put attention to the month (parts[0]), Javascript counts months from 0:
		// January - 0, February - 1, etc
		var startDate = new Date(parts[2], parts[0] - 1, parts[1]);
		var endDate = startDate;
		endDate.setFullYear(endDate.getFullYear() + 1);
		var endDateString = (endDate.getMonth() + 1).toString() + "/" + endDate.getDate().toString() + "/" + endDate.getFullYear().toString();
		lblEndResult.html(endDateString);
	}

	function registerPayment(payments)
	{
		var fee = getFee();
		if (isValidFee(fee))
		{
			DisplayLoading();
			$.ajax({
				url: '<%= VirtualPathUtility.ToAbsolute("~/Application/SavePaymentType.ashx") %>',
				type: 'POST',
				data: { 'quoteid': <%= QuoteID %>, 'numberOfPayments': payments, 'policyFee': fee },
				success: function (data)
				{
					if (data.Result)
					{
						lblNetPremium.val(data.Premium);
						lblFee.html(data.Fee);
						lblRXPF.html(data.RXPF);
						lblTax.html(data.Tax);
						lblTotal.html(data.Total);
					} else
					{
						alert("Error with the server. Please try again!");
					}
					HideFullScreen();
				},
				error: function (error, exc)
				{
					alert("Error with the server. Please try again!");
					HideFullScreen();
				}
			});
		}
		//else 
		//{
		//	//lblRXPF.html(formatter.format(0));
		//	//console.log(valFee);
		//}
	}

	function isValidFee(fee)
	{
		//var netPremium = Number(lblNetPremium.html().replace("$", "").replace(",", "").trim());
		var isValid = true;
		var hdnMinPolicyFee = $("#<%=hdnMinPolicyFee.ClientID %>");
		var hdnMaxPolicyFee = $("#<%=hdnMaxPolicyFee.ClientID %>");
		var hdnMinPremium = $("#<%=hdnMinPremium.ClientID %>");
		var hdnMaxPremium = $("#<%=hdnMaxPremium.ClientID %>");

		var nFee = Number(fee);
		var minFee = Number(hdnMinPolicyFee.val());
		var maxFee = Number(hdnMaxPolicyFee.val());
		var minPremium = Number(hdnMinPremium.val());
		var maxPremium = Number(hdnMaxPremium.val());

		var valFee = $("#<%=valFee.ClientID %>");

		if (nFee > maxFee)
		{
			//var valFee = $get('<%= valFee.ClientID %>');
			valFee.html('Invalid Fee,  Max Fee Value would be ');
			isValid = false;
		} else if (nFee < minFee)
		{
			valFee.html('Invalid Fee');
			isValid = false;
		} else
		{
			valFee.html('');
			isValid = true;
		}

		return isValid;
	}

	function handleBreakdownLinks(numPayments)
	{
		if (Number(numPayments) === 4)
		{
			lnkBiyearly.hide();
			lnkQuarterly.show();
		}
		else if (Number(numPayments) === 2)
		{
			lnkBiyearly.show();
			lnkQuarterly.hide();
		}
		else
		{
			lnkBiyearly.hide();
			lnkQuarterly.hide();
		}
	}

	function getFee()
	{
		var changeFee = Boolean($("#<%=hdnCanChangeFee.ClientID %>"));
		var fee = 0;

		if (changeFee)
		{
			if (!$('#<%= txtFee.ClientID %>').val())
			{
				return false;
			}
			else
			{
				fee = Number($('#<%= txtFee.ClientID %>').val().replace(/\$|,/g, ''));
			}
		}
		else
		{
			fee = Number(hdnTotaltFee.val());
		}

		<%--var checked = $('#<%= chkChangeFee.ClientID %>').is(":checked");
			var fee = 0;
			if (checked)
			{
				
				
			}
			else
			{
				fee = Number(lblFee.html().replace(/\$|,/g, ''));
			}
			return fee;--%>
		return fee;
	}

	$(function ()
	{
		debugger;
		var temp = hdnNumPayments.val();
		var mySelect = document.getElementById('ddlNumPayments');

		for (var i, j = 0; i = mySelect.options[j]; j++)
		{
			if (i.value == temp)
			{
				mySelect.selectedIndex = j;
				break;
			}
		}

		$('#<%= btnGenerateCalculo.ClientID %>').click(function (event)
		{
			event.preventDefault();
			return GB_showCenter('Calculation', '/Application/CalculateModal.aspx', 550, 700, OnCloseBox);
		});

		if (!isTotalOver2500())
		{
			hideControlsByPrice();
		}

		if (!isCompany())
		{
			hideControlsByPersonType();
		}
		else
		{
			changeCorpLabels();
		}

		$('#ddlNumPayments').change(function ()
		{
			var numPayments = $(this).val();
			handleBreakdownLinks(numPayments);
			registerPayment(numPayments);
		});

		if (hdnNumPayments.val())
		{
			$('#ddlNumPayments').val(hdnNumPayments.val());
		}

		if (txtStart.val())
		{
			DateChange();
		}

		$('#ddlNumPayments').trigger("change");

			<%--$('#<%= txtFee.ClientID %>').hide();--%>

			<%--$('#<%= chkChangeFee.ClientID %>').click(function ()
			{
				$('#<%= lblFee.ClientID %>').toggle(!this.checked);
					$('#<%= txtFee.ClientID %>').toggle(this.checked);
					if (!this.checked)
					{
								$('#<%= txtFee.ClientID %>').val('');
								var valFee = $get('<%= valFee.ClientID %>');
								var minFee = $get('<%= cmpMinFee.ClientID %>');
								var maxFee = $get('<%= cmpMaxFee.ClientID %>');
								ValidatorValidate(valFee);
								ValidatorValidate(minFee);
								ValidatorValidate(maxFee);
					}
				});--%>
	});

	<%--function validateFee(obj, args)
	{
		var txtFee = $get('<%= this.txtFee.ClientID %>');
			var checked = $('#<%= chkChangeFee.ClientID %>').is(":checked");

		if (checked)
		{
			if (!(txtFee))
			{
				args.IsValid = false;
				return;
			}
			else
			{
				args.IsValid = isValidFee(getFee());
			}
		}
		else
		{
			args.IsValid = true;
		}
	}--%>
</script>
