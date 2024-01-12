using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using ImportadorASicas;
using ME.Admon;
using Openpay.Entities;
using Protec.Authentication.Membership;
using Telepro.Conexion;

public partial class UserControls_GenerateStep4 : System.Web.UI.UserControl, IApplicationStep
{
	private int _stateselected = 0;
	internal int PERSON_TYPE = 0;
	internal int CURRENCY = 0;
	private Regex regvalidator = new Regex("^[a-zA-Z0-9-_/]+$");
	private const int MEXICO_ID = 2; //from admonPoliza table country
	private const int USA_ID = 1; //from admonPoliza table country
	protected int QuoteID { get; set; }
	protected decimal EXCHANGE_RATE = 0;
	private bool VALIDATE_EMAIL = true;
	private bool VALIDATE_PHONE = true;
	protected int RETROACTIVE_DAYS;

	private void LoadData(int key)
	{
		if (key <= 0)
		{
			return;
		}
		HouseInsuranceDataContext db = new HouseInsuranceDataContext();
		var quote = db.quotes.Single(m => m.id == key);

		VALIDATE_EMAIL = string.IsNullOrEmpty(quote.insuredPerson1.email);
		VALIDATE_PHONE = string.IsNullOrEmpty(quote.insuredPerson1.phone);
		PERSON_TYPE = quote.insuredPerson1.legalPerson ? ApplicationConstants.PERSONA_MORAL : ApplicationConstants.PERSONA_FISICA;
		ddlCountry.DataSource = AdminGateway.Gateway.GetCountries();
		ddlCountry.DataBind();
		hdnIssuedBy.Value = quote.issuedBy.ToString();

		LoadPremiumData(db, quote, key);
		ValidateMaximumValues(quote);
		LoadDataFromQuote(quote);
		updLiveUsa.Visible = btnYes.Checked;
	}

	private void LoadDataFromQuote(quote q)
	{
		hdnNumPayments.Value = q.cantidadDePagos.ToString();

		if (!string.IsNullOrEmpty(q.insuredPerson1.fax))
		{
			txtAreaFax.Text = q.insuredPerson1.fax.Substring(0, 3);
			txtFax1.Text = q.insuredPerson1.fax.Substring(3, 3);
			txtFax2.Text = q.insuredPerson1.fax.Substring(6);
		}
		txtAdditionalInsured.Text = q.insuredPerson1.additionalInsured;
		txtRFC.Text = q.insuredPerson1.rfc;
		txtInsuredStreet.Text = q.insuredPersonHouse1.street;
		txtInsuredExtNo.Text = q.insuredPersonHouse1.exteriorNo;
		txtInsuredIntNo.Text = q.insuredPersonHouse1.InteriorNo;
		txtBankName.Text = q.insuredPersonHouse1.bankName;
		txtBankAddress.Text = q.insuredPersonHouse1.bankAddress;
		txtLoanNumber.Text = q.insuredPersonHouse1.loanNumber;

		

		if (!string.IsNullOrEmpty(q.insuredPerson1.citizenship))
		{
			txtNationality.Text = q.insuredPerson1.citizenship;
		}

		if (!string.IsNullOrEmpty(q.insuredPerson1.occupation))
		{
			txtOccupation.Text = q.insuredPerson1.occupation;
		}

		if (q.startDate.HasValue)
		{
			txtStart.Text = q.startDate.Value.ToString("M/d/yyyy");
		}

		if (q.endDate.HasValue)
		{
			lblEndResult.Text = q.endDate.Value.ToString("M/d/yyyy");
		}

		if (q.insuredPerson1.DOB != null)
		{
			DateTime birthday = (DateTime) q.insuredPerson1.DOB;
			string month = birthday.Month < 10 ? "0" + birthday.Month.ToString() : birthday.Month.ToString();
			ddlDay.Items.FindByValue(birthday.Day.ToString()).Selected = true;
			ddlMonth.Items.FindByValue(month).Selected = true;
			ddlYear.Items.FindByValue(birthday.Year.ToString()).Selected = true;
		}
		if (!string.IsNullOrEmpty(q.insuredPerson1.street))
		{
			string[] address = q.insuredPerson1.street.Split(',');
			btnYes.Checked = true;
			txtStreet.Text = address[0].Trim();
			txtExtNo.Text = address.Length > 1 ? address[1].Trim() : string.Empty;
			txtIntNo.Text = address.Length > 2 ? address[2].Trim() : string.Empty;
			txtZip.Text = q.insuredPerson1.zipCode;
			if (q.insuredPerson1.state == null)
			{
				ddlState.Style.Add(HtmlTextWriterStyle.Display, "none");
				txtState.Style.Add(HtmlTextWriterStyle.Display, "inline");
				txtState.Text = q.insuredPerson1.stateText;
				btnNotUSA.Checked = true;
				btnUSA.Checked = false;
				ddlCountry.Style.Add(HtmlTextWriterStyle.Display, "inline");
				lblCountryText.Style.Add(HtmlTextWriterStyle.Display, "inline");
				lblCountryText.Style.Add(HtmlTextWriterStyle.Visibility, "hidden");
				_stateselected = 0;
			} else
			{
				btnUSA.Checked = true;
				btnNotUSA.Checked = false;
				txtState.Style.Add(HtmlTextWriterStyle.Display, "none");
				ddlState.Style.Add(HtmlTextWriterStyle.Display, "inline");
				ddlCountry.Style.Add(HtmlTextWriterStyle.Display, "none");
				lblCountryText.Style.Add(HtmlTextWriterStyle.Display, "none");
				_stateselected = (int) q.insuredPerson1.state;
			}
			txtCity.Text = q.insuredPerson1.city;
		} else
		{
			btnNo.Checked = true;
		}

		if (!VALIDATE_EMAIL)
		{
			lblEmail.Style.Add(HtmlTextWriterStyle.Display, "none");
			txtEmail.Style.Add(HtmlTextWriterStyle.Display, "none");
			lnkEmail.Style.Add(HtmlTextWriterStyle.Display, "none");
		}

		if (!VALIDATE_PHONE)
		{
			lblPhone.Style.Add(HtmlTextWriterStyle.Display, "none");
			txtAreaPhone.Style.Add(HtmlTextWriterStyle.Display, "none");
			txtPhone1.Style.Add(HtmlTextWriterStyle.Display, "none");
			txtPhone2.Style.Add(HtmlTextWriterStyle.Display, "none");
			lnkPhone.Style.Add(HtmlTextWriterStyle.Display, "none");
		}
	}

	private void ValidateMaximumValues(quote q)
	{
		decimal totalSum = 0M;

		totalSum += q.buildingLimit;
		totalSum += q.otherStructuresLimit;
		totalSum += q.contentsLimit;
		totalSum += q.debriLimit;
		totalSum += q.extraLimit;

		string sqlMaxLimit = $"select maxHOpropertyValue from ProductCompany where product = {ApplicationConstants.APPLICATION} and company = {ApplicationConstants.COMPANY}";

		dbConexion con = new dbConexion(ApplicationConstants.ADMINDB);
		DataTable dtProductCompany = con.ExecuteDS(sqlMaxLimit).Tables[0];

		if (dtProductCompany.Rows.Count > 0)
		{
			decimal MaxValue = 0M;
			decimal.TryParse(dtProductCompany.Rows[0]["maxHOpropertyValue"].ToString(), out MaxValue);

			if (q.currency == ApplicationConstants.DOLLARS_CURRENCY)
			{
				MaxValue = MaxValue;
			} else
			{
				UserInformation user = (UserInformation) HttpContext.Current.User.Identity;
				decimal tipoCambio = user.ExchangeRate;
				MaxValue = (MaxValue * tipoCambio);
			}

			if (totalSum >= MaxValue)
			{
				btnBuyQuote.Enabled = false;
				btnBuyQuote.Visible = false;
				string customVal = $"Maximum limit exceeded, must be less than {MaxValue.ToString("C2")}. It is not possible to issue the policy.";

				lblMaxSum.Text = customVal;
				lblMaxSum.Visible = true;
			}
			else
			{
				lblMaxSum.Text = string.Empty;
				lblMaxSum.Visible = false;
				btnBuyQuote.Enabled = true;
				btnBuyQuote.Visible = true;
			}
		}
	}

	public bool SaveStep()
	{
		List<IValidator> errored = Page.Validators.Cast<IValidator>().Where(v => !v.IsValid).ToList();
		if (Page.IsValid)
		{
			try
			{
				saveData();
			} catch
			{
				return false;
			}
			return true;
		}
		return false;
	}

	public void LoadStep()
	{
		int key;
		UserInformation info = new UserInformation();
		try
		{
			QuoteID = key = GetKey();
			
			HouseInsuranceDataContext db = new HouseInsuranceDataContext();
			var quote = db.quotes.SingleOrDefault(m => m.id == QuoteID);
			hdnCanChangeFee.Value = "false";

			info = (UserInformation) Page.User.Identity;
			if (info.Agent == ApplicationConstants.MEAGENT || info.UserID == ApplicationConstants.PTI_USER)
			{
				btnGenerateCalculo.Visible = true;
				Session["quoteIDCalculate"] = key;
				ME_PolicyFee.Visible = true;
				ME_Policy.Visible = true;
				hdnMacFeeAgent.Value = "true";
			} 
			else
			{
				ME_PolicyFee.Visible = false;
				ME_Policy.Visible = false;
				hdnMacFeeAgent.Value = "false";
			}

			string sqlChgFee = $"select bd.* from branchDetail bd inner join branch b on bd.branch = b.branch inner join branchUser bu on bu.branch = b.branch where product = {ApplicationConstants.APPLICATION} and company = {ApplicationConstants.COMPANY} and bu.branch = {info.Branch}";

			dbConexion con = new dbConexion(ApplicationConstants.ADMINDB);
			DataTable dtBranchDetail = con.ExecuteDS(sqlChgFee).Tables[0];

			//if (info.Agent == ApplicationConstants.WEST_COAST_AGENT || info.Agent == ApplicationConstants.MEAGENT || info.UserID == ApplicationConstants.PTI_USER)
			if (dtBranchDetail.Rows.Count > 0)
			{
				bool changeFee = bool.Parse(dtBranchDetail.Rows[0]["changePolicyFee"].ToString());
				bool isValid = false;
				bool notFound = true;
				if (changeFee)
				{
					con = new dbConexion(ApplicationConstants.DB);
					string sqlFeeLimits = $"select pfl.* from homeowner.policyFeeLimits pfl inner join [admonPoliza].[dbo].[branchDetail] bd on pfl.branchDetail = bd.branchDetail where bd.company = {ApplicationConstants.COMPANY} and bd.product = {ApplicationConstants.APPLICATION} and bd.branch = {info.Branch}";

					DataTable dtFeeLimits = con.ExecuteDS(sqlFeeLimits).Tables[0];

					if (dtFeeLimits.Rows.Count > 0)
					{
						var codigos = db.spList_Calcula(key, quote.insuredPersonHouse1.HouseEstate.City.HMP).Single();
						var netPremium = codigos.netPremium.Value;

						for (var r = 0; r < dtFeeLimits.Rows.Count; r++)
						{
							if (notFound == false)
							{
								continue;
							}

							decimal minPremium = 0;
							decimal maxPremium = 0;
							

							var minimoPremium = dtFeeLimits.Rows[r]["lowLimitNetPremium"].ToString();
							var maximoPremium = dtFeeLimits.Rows[r]["highLimitNetPremium"].ToString();
							var minimoFee = dtFeeLimits.Rows[r]["lowLimitFee"].ToString();
							var maximoFee = dtFeeLimits.Rows[r]["highLimitFee"].ToString();

							decimal.TryParse(minimoPremium, out minPremium);
							decimal.TryParse(maximoPremium, out maxPremium);

							decimal minFee = 0;
							decimal maxFee = 0;
							decimal.TryParse(minimoFee, out minFee);
							decimal.TryParse(maximoFee, out maxFee);


							if (netPremium <= maxPremium && netPremium >= minPremium)
							{
								notFound = false;
								isValid = true;
								hdnCanChangeFee.Value = "true";
								hdnMinPremium.Value = minimoPremium;
								hdnMaxPremium.Value = maximoPremium;
								hdnMinPolicyFee.Value = CURRENCY == ApplicationConstants.PESOS_CURRENCY ? (minFee * 10).ToString() : minimoFee;
								hdnMaxPolicyFee.Value = CURRENCY == ApplicationConstants.PESOS_CURRENCY ? (maxFee * 10).ToString() : maximoFee;

								int issuedBy = quote.issuedBy;
								if (issuedBy == 1 && DateTime.Now < new DateTime(2023, 06, 01))
								{
									hdnMinPolicyFee.Value = CURRENCY == ApplicationConstants.PESOS_CURRENCY ? (minFee * 10).ToString() : (minFee).ToString();
									hdnMaxPolicyFee.Value = CURRENCY == ApplicationConstants.PESOS_CURRENCY ? (maxFee * 10).ToString() : (maxFee).ToString();

								}
							}
						}

						if (notFound == true)
						{
							isValid = false;
						}
					} 
					else
					{
						isValid = false;
					}
					lblFee.Visible = isValid ? false : true;
					txtFee.Visible = isValid ? true : false;

				}
				else
				{
					lblFee.Visible = true;
					txtFee.Visible = false;
				}
			} 
			else
			{
				lblFee.Visible = true;
				txtFee.Visible = false;
			}

			if (info.Agent == ApplicationConstants.WEST_COAST_AGENT)
			{
				btnBuyQuote.Visible = false;
				btnBuyQuote.Enabled = false;
			}
		} catch
		{
			return;
		}
		FillDays();
		FillMonths();
		FillYears();
		
		UserInformationDB userDB = AdminGateway.Gateway.GetUserInformation(info.UserID);
		RETROACTIVE_DAYS = userDB.RetroactivePolicyDays;
		SetupBreakdownLinks();
		LoadData(key);
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (Page.IsPostBack)
		{
			LoadStep();
		}
	}

	public string Title()
	{
		return GetLocalResourceObject("strInsuredInformation").ToString();
	}

	private int GetKey()
	{
		return Session["quoteid"] == null ? 0 : (int) Session["quoteid"];
	}
	private void saveData()
	{
		HouseInsuranceDataContext db = new HouseInsuranceDataContext();
		int key = GetKey();
		if (key <= 0)
		{
			throw new Exception("key <= 0");
		}

		int userid = ((UserInformation) Page.User.Identity).UserID;

		string insuredStreet = null;
		string insuredExtNo = null;
		string insuredIntNo = null;
		string fax = null;
		string dob = null;
		string taxID = null;
		string nationality = null;
		string occupation = null;
		string legalRep = null;
		string idNumber = null;
		string idType = null;
		string incorporationNumber = null;
		string mercantileNumber = null;
		string fiel = null;
		string additionalInsured = null;

		string bankName = null;
		string bankAddress = null;
		string loanNumber = null;

		int countryID = MEXICO_ID;
		string street = null;
		string extNo = null;
		string intNo = null;
		int? state = null;
		string stateText = null;
		string city = null;
		string zip = null;
		string phone = null;
		string email = null;

		bool legalPerson = isCompany();

		insuredStreet = txtInsuredStreet.Text.Trim();
		insuredExtNo = txtInsuredExtNo.Text.Trim();
		insuredIntNo = string.IsNullOrEmpty(txtInsuredIntNo.Text) ? null : txtInsuredIntNo.Text.Trim();
		if (btnYes.Checked)
		{  //btnYes = Insured person lives in different address than the insured property
			street = txtStreet.Text.Trim();
			extNo = txtExtNo.Text.Trim();
			intNo = txtIntNo.Text.Trim();

			if (btnNotUSA.Checked)
			{
				state = null;
				stateText = txtState.Text.Trim();
				countryID = int.Parse(ddlCountry.SelectedValue);
			} else
			{
				state = int.Parse(ddlState.SelectedValue);
				stateText = ddlState.SelectedItem.Text;
				countryID = USA_ID;
			}
			city = txtCity.Text.Trim();
			zip = txtZip.Text.Trim();
		}

		bool inMexico = street == null;

		dob = ddlMonth.Value.Trim() + "/" + ddlDay.Value.Trim() + "/" + ddlYear.Value.Trim();
		if (!(string.IsNullOrEmpty(txtAreaFax.Text.Trim())
				|| string.IsNullOrEmpty(txtFax1.Text.Trim())
				|| string.IsNullOrEmpty(txtFax2.Text.Trim())))
		{
			fax = txtAreaFax.Text + txtFax1.Text + txtFax2.Text;
			fax = fax.Trim();
		}
		if (!string.IsNullOrEmpty(txtRFC.Text))
		{
			taxID = txtRFC.Text.Trim();
		}

		if (!string.IsNullOrEmpty(txtNationality.Text))
		{
			nationality = txtNationality.Text.Trim();
		}

		if (!string.IsNullOrEmpty(txtOccupation.Text))
		{
			occupation = txtOccupation.Text.Trim();
		}

		if (!string.IsNullOrEmpty(txtRepresentative.Text))
		{
			legalRep = txtRepresentative.Text.Trim();
		}

		if (!string.IsNullOrEmpty(txtID.Text))
		{
			idNumber = txtID.Text.Trim();
		}

		if (!string.IsNullOrEmpty(txtIDType.Text))
		{
			idType = txtIDType.Text.Trim();
		}

		if (!string.IsNullOrEmpty(txtIncNumber.Text))
		{
			incorporationNumber = txtIncNumber.Text.Trim();
		}

		if (!string.IsNullOrEmpty(txtMercNumber.Text))
		{
			mercantileNumber = txtMercNumber.Text.Trim();
		}

		if (!string.IsNullOrEmpty(txtFIEL.Text))
		{
			fiel = txtFIEL.Text.Trim();
		}

		if (!string.IsNullOrEmpty(txtAdditionalInsured.Text))
		{
			additionalInsured = txtAdditionalInsured.Text.Trim();
		}

		if (!string.IsNullOrEmpty(txtBankName.Text))
		{
			bankName = txtBankName.Text.Trim();
		}

		if (!string.IsNullOrEmpty(txtBankAddress.Text))
		{
			bankAddress = txtBankAddress.Text.Trim();
		}

		if (!string.IsNullOrEmpty(txtLoanNumber.Text))
		{
			loanNumber = txtLoanNumber.Text.Trim();
		}

		quote q = db.quotes.Single(m => m.id == key);
		PartialArt140 partial140 = new PartialArt140();

		string address = null;
		if (!inMexico)
		{
			address = street + ", " + extNo;
			if (!string.IsNullOrEmpty(intNo))
			{
				address += ", " + intNo;
			}
		}
		q.insuredPerson1.DOB = DateTime.Parse(dob);
		q.insuredPerson1.rfc = taxID;
		q.insuredPerson1.citizenship = nationality;
		q.insuredPerson1.occupation = occupation;
		q.insuredPerson1.additionalInsured = additionalInsured;
		q.insuredPerson1.state = state;
		q.insuredPerson1.stateText = stateText;
		q.insuredPerson1.city = city;
		q.insuredPerson1.street = address;
		q.insuredPerson1.zipCode = zip;
		if (VALIDATE_EMAIL)
		{
			email = txtEmail.Text.Trim();
			q.insuredPerson1.email = email;
		}
		if (VALIDATE_PHONE)
		{
			phone = txtAreaPhone.Text.Trim() + txtPhone1.Text.Trim() + txtPhone2.Text.Trim();
			q.insuredPerson1.phone = phone;
		}

		q.insuredPersonHouse1.street = insuredStreet;
		q.insuredPersonHouse1.InteriorNo = insuredIntNo;
		q.insuredPersonHouse1.exteriorNo = insuredExtNo;
		q.insuredPersonHouse1.bankAddress = bankAddress;
		q.insuredPersonHouse1.bankName = bankName;
		q.insuredPersonHouse1.loanNumber = loanNumber;

		Session["partial140"] = new PartialArt140
		{
			CountryID = countryID,
			FIEL = fiel,
			IDNumber = idNumber,
			IDType = idType,
			IncorporationNumber = incorporationNumber,
			LegalRepresentative = legalRep,
			MercantileNumber = mercantileNumber
		};

		rxpfSur surcharge = ApplicationConstants.GetRXPFSurcharge(db, q.company.Value, q.currency);
		decimal percentage;
		switch (q.cantidadDePagos)
		{
			case 1:
			default:
				percentage = 0;
				break;
			case 2:
				percentage = surcharge.biyearly;
				break;
			case 4:
				percentage = surcharge.quarterly;
				break;
		}

		var cotiza = db.spList_Calcula(key, q.insuredPersonHouse1.HouseEstate.City.HMP).Single();
		var netPremium = cotiza.netPremium.Value;

		decimal rxpfValue = (cotiza.netPremium ?? 0M) * percentage;

		q.cantidadDePagos = q.cantidadDePagos;
		q.recargoPagoFraccionado = rxpfValue;
		q.porcentajeRecargoPagoFraccionado = percentage;
		hdnNumPayments.Value = q.cantidadDePagos.ToString();
		db.SubmitChanges();
		SaveDateRange(txtStart.Text);
	}
	private void FillDays()
	{
		ddlDay.Items.Clear();
		for (int i = 1; i < 32; i++)
		{
			ddlDay.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
		}
	}

	private void FillMonths()
	{

		ddlMonth.Items.Clear();
		ddlMonth.Items.Add(
				new ListItem(
						GetLocalResourceObject("strJanuary").ToString(),
						"01"
				)
		);
		ddlMonth.Items.Add(
				new ListItem(
						GetLocalResourceObject("strFebruary").ToString(),
						"02"
				)
		);
		ddlMonth.Items.Add(
				new ListItem(
						GetLocalResourceObject("strMarch").ToString(),
						"03"
				)
		);
		ddlMonth.Items.Add(
				new ListItem(
						GetLocalResourceObject("strApril").ToString(),
						"04"
				)
		);
		ddlMonth.Items.Add(
				new ListItem(
						GetLocalResourceObject("strMay").ToString(),
						"05"
				)
		);
		ddlMonth.Items.Add(
				new ListItem(
						GetLocalResourceObject("strJune").ToString(),
						"06"
				)
		);
		ddlMonth.Items.Add(
				new ListItem(
						GetLocalResourceObject("strJuly").ToString(),
						"07"
				)
		);
		ddlMonth.Items.Add(
				new ListItem(
						GetLocalResourceObject("strAugust").ToString(),
						"08"
				)
		);
		ddlMonth.Items.Add(
				new ListItem(
						GetLocalResourceObject("strSeptember").ToString(),
						"09"
				)
		);
		ddlMonth.Items.Add(
				new ListItem(
						GetLocalResourceObject("strOctober").ToString(),
						"10"
				)
		);
		ddlMonth.Items.Add(
				new ListItem(
						GetLocalResourceObject("strNovember").ToString(),
						"11"
				)
		);
		ddlMonth.Items.Add(
				new ListItem(
						GetLocalResourceObject("strDecember").ToString(),
						"12"
				)
		);
	}

	private void FillYears()
	{
		int max = 0;
		if (!isCompany())
		{
			max = -18;
		}

		ddlYear.Items.Clear();
		for (int i = DateTime.Today.AddYears(max).Year; i >= DateTime.Today.AddYears(-100).Year; i--)
		{
			ddlYear.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
		}
	}

	private void LoadPremiumData(HouseInsuranceDataContext db, quote quote, int key)
	{
		getExchangeRate();
		CURRENCY = quote.currency;
		
		dbConexion con = new dbConexion(ApplicationConstants.ADMINDB);
		string sql = $"SELECT product, company, inscofee, MGAPolicyFee FROM admonPoliza.dbo.productCompany WHERE company = {ApplicationConstants.COMPANY} and product = {ApplicationConstants.APPLICATION}";
		DataTable dt = con.ExecuteDS(sql).Tables[0];

		decimal MGAPolicyFee = 0;
		decimal InsCoFee = 0;
		decimal AgentFee = 0; 
		decimal PolicyFee = 0;

		decimal.TryParse(dt.Rows[0]["MGAPolicyFee"].ToString(), out MGAPolicyFee);
		decimal.TryParse(dt.Rows[0]["inscofee"].ToString(), out InsCoFee);
		PolicyFee = ApplicationConstants.Fee(quote.currency, quote.fee.Value);
		MGAPolicyFee = ApplicationConstants.Fee(quote.currency, MGAPolicyFee);
		InsCoFee = ApplicationConstants.Fee(quote.currency, InsCoFee);

		//int issuedBy = 0;
		//int.TryParse(hdnIssuedBy.Value, out issuedBy);
		//if (issuedBy != 1 && DateTime.Now < new DateTime(2023, 06, 01))
		//{
		//	MGAPolicyFee = 0;
		//}

		var codigos = db.spList_Calcula(key, quote.insuredPersonHouse1.HouseEstate.City.HMP).Single();
		var netPremium = codigos.netPremium.Value;
		lblNetPremium.Text = netPremium.ToString("C2");
		

		lblFee.Text = Convert.ToInt32(Convert.ToDouble(PolicyFee)).ToString("C2");
		txtFee.Text = Convert.ToInt32(Convert.ToDouble(PolicyFee)).ToString();

		hdnTotaltFee.Value = PolicyFee.ToString();
		hdnMGAPolicyFee.Value = MGAPolicyFee.ToString();
		
		var info = (UserInformation) Page.User.Identity;
		if ((info.Agent == ApplicationConstants.MEAGENT || info.UserID == ApplicationConstants.PTI_USER))
		{
			if (chkLocalPayment.Checked)
			{
				//PolicyFee = ApplicationConstants.Fee(quote.currency, quote.fee.Value) - MGAPolicyFee;
				//MGAPolicyFee = 0M;
			}
			AgentFee = PolicyFee - (InsCoFee + MGAPolicyFee);
			lblInsCoFee.Text = Convert.ToInt32(Convert.ToDouble(InsCoFee)).ToString("C2");
			lblMGAFee.Text = Convert.ToInt32(Convert.ToDouble(MGAPolicyFee)).ToString("C2");
			lblAgentFee.Text = Convert.ToInt32(Convert.ToDouble(AgentFee)).ToString("C2");
		}
		else
		{
			decimal InsuranceFeeMacEdwardsFee = (InsCoFee + MGAPolicyFee);
			AgentFee = PolicyFee - InsuranceFeeMacEdwardsFee;
			lblInsCoFee.Text = Convert.ToInt32(Convert.ToDouble(InsuranceFeeMacEdwardsFee)).ToString("C2");
			lblMGAFee.Text = Convert.ToInt32(Convert.ToDouble(0M)).ToString("C2");
			lblAgentFee.Text = Convert.ToInt32(Convert.ToDouble(AgentFee)).ToString("C2");
		}

		decimal rxpfPercentage = quote.porcentajeRecargoPagoFraccionado;
		decimal rxpf = quote.recargoPagoFraccionado;
		decimal taxPercentage = db.fGet_QuoteTaxPercentage(key).Value;
		decimal tax = ApplicationConstants.CalculateTax(netPremium, taxPercentage, rxpfPercentage, quote.currency, PolicyFee);
		lblTax.Text = tax.ToString("C2");
		lblRXPF.Text = rxpf.ToString("C2");
		lblTotal.Text = ApplicationConstants.CalculateTotal(netPremium, taxPercentage, rxpfPercentage, quote.currency, PolicyFee).ToString("C2");
	}

	protected void valNationality_ServerValidate(object sender, ServerValidateEventArgs e)
	{
		if (isTotalOver2500())
		{
			e.IsValid = txtNationality.Text.Length > 0;
		}
		e.IsValid = true;
	}

	protected void valPhone_ServerValidate(object sender, ServerValidateEventArgs e)
	{
		e.IsValid = VALIDATE_PHONE ? txtAreaPhone.Text.Length >= 2 && txtPhone1.Text.Length >= 3 && txtPhone2.Text.Length == 4 : true;
	}

	protected void valEmail_ServerValidate(object sender, ServerValidateEventArgs e)
	{
		e.IsValid = VALIDATE_EMAIL ? txtEmail.Text.Length > 0 : true;
	}

	protected void valOccupation_ServerValidate(object sender, ServerValidateEventArgs e)
	{
		e.IsValid = isTotalOver2500() ? txtOccupation.Text.Length > 0 : true;
	}

	protected void valRepresentative_ServerValidate(object sender, ServerValidateEventArgs e)
	{
		e.IsValid = isTotalOver2500() && isCompany() ? txtRepresentative.Text.Length > 0 : true;
	}

	protected void valID_ServerValidate(object sender, ServerValidateEventArgs e)
	{
		e.IsValid = isTotalOver2500() && isCompany() ? txtID.Text.Length > 0 : true;
	}

	protected void valIDType_ServerValidate(object sender, ServerValidateEventArgs e)
	{
		e.IsValid = isTotalOver2500() && isCompany() ? txtIDType.Text.Length > 0 : true;
	}

	protected void valIncNumber_ServerValidate(object sender, ServerValidateEventArgs e)
	{
		e.IsValid = isTotalOver2500() && isCompany() ? txtIncNumber.Text.Length > 0 : true;
	}

	protected void valMercNumber_ServerValidate(object sender, ServerValidateEventArgs e)
	{
		e.IsValid = isTotalOver2500() && isCompany() ? txtMercNumber.Text.Length > 0 : true;
	}

	protected void valRFC_ServerValidate(object sender, ServerValidateEventArgs e)
	{
		e.IsValid = isCompany() ? txtRFC.Text.Length > 0 : true;
	}

	protected void valFee_ServerValidate(object sender, ServerValidateEventArgs e)
	{
		e.IsValid = txtFee.Visible.Equals(true) ? string.IsNullOrEmpty(txtFee.Text) ? false : isValidFee(Convert.ToInt32(txtFee.Text)) : true;
	}

	protected void ddlDataBound(object sender, EventArgs e)
	{
		DropDownList ddl = sender as DropDownList;

		ListItem item = new ListItem("---", "0");
		ddl.Items.Insert(0, item);
		if (ddl == ddlState && _stateselected > 0)
		{
			ddl.SelectedValue = _stateselected.ToString();
		}
	}

	protected void ValidateState(object sender, ServerValidateEventArgs e)
	{
		e.IsValid = btnNotUSA.Checked ? !string.IsNullOrEmpty(txtState.Text.Trim()) : ddlState.SelectedValue != "0";
	}

	protected void valBankInformation_Validate(object sender, ServerValidateEventArgs e)
	{
		e.IsValid = true;
		if (txtBankName.Text.Trim().Length > 0)
		{
			e.IsValid = e.Value.Trim().Length > 0;
		}
	}

	protected void validateFax_ServerValidate(object sender, ServerValidateEventArgs args)
	{
		args.IsValid = txtAreaFax.Text.Length > 0 || txtFax1.Text.Length > 0 || txtFax2.Text.Length > 0
			? txtAreaFax.Text.Length >= 2 && txtFax1.Text.Length >= 3 && txtFax2.Text.Length == 4
			: true;
	}

	protected void valStart_Validate(object sender, ServerValidateEventArgs e)
	{
		try
		{
			DateTime timeSelected = DateTime.Parse(e.Value);
			int agent = ((UserInformation) Page.User.Identity).Agent;
			e.IsValid = timeSelected >= DateTime.Now.AddDays(-RETROACTIVE_DAYS);
		} catch
		{
			e.IsValid = false;
		}
	}

	public void txtLoanNumber_Validate(object sender, ServerValidateEventArgs e)
	{
		e.IsValid = e.Value.Length == 0 || regvalidator.IsMatch(e.Value);
	}

	protected void CheckedChange(object sender, EventArgs e)
	{
		updLiveUsa.Visible = btnYes.Checked;
	}

	protected void LocalPayChange(object sender, EventArgs e)
	{
		
		//var check = chkLocalPayment.Checked;

		//decimal MGAPolicyFee = 0;
		//decimal InsCoFee = 0;
		//decimal AgentFee = 0;
		//decimal PolicyFee = 0;

		//QuoteID = GetKey();
			
		//HouseInsuranceDataContext db = new HouseInsuranceDataContext();
		//var quote = db.quotes.SingleOrDefault(m => m.id == QuoteID);

		////decimal netPremium = decimal.Parse(lblNetPremium.Text.Replace("$", "").Replace("$", "").Trim());
		////decimal mgaPolicyFee = decimal.Parse(lblAgentFee.Text.Replace("$", "").Replace("$", "").Trim());

		////decimal.TryParse(dt.Rows[0]["MGAPolicyFee"].ToString(), out MGAPolicyFee);
		////decimal.TryParse(dt.Rows[0]["inscofee"].ToString(), out InsCoFee);
		//PolicyFee = ApplicationConstants.Fee(quote.currency, quote.fee.Value);
		////MGAPolicyFee = ApplicationConstants.Fee(quote.currency, MGAPolicyFee);
		////InsCoFee = ApplicationConstants.Fee(quote.currency, InsCoFee);

		//if (!chkLocalPayment.Checked)
		//{
		//	decimal.TryParse(lblInsCoFee.Text.Replace("$", "").Replace("$", "").Trim(), out InsCoFee);
		//	decimal.TryParse(lblMGAFee.Text.Replace("$", "").Replace("$", "").Trim(), out MGAPolicyFee);
		//	decimal.TryParse(lblAgentFee.Text.Replace("$", "").Replace("$", "").Trim(), out AgentFee);

		//	AgentFee = PolicyFee - (InsCoFee + MGAPolicyFee);
		//	lblInsCoFee.Text = Convert.ToInt32(Convert.ToDouble(InsCoFee)).ToString("C2");
		//	lblMGAFee.Text = Convert.ToInt32(Convert.ToDouble(MGAPolicyFee)).ToString("C2");
		//	lblAgentFee.Text = Convert.ToInt32(Convert.ToDouble(AgentFee)).ToString("C2");

		//	lblFee.Text = Convert.ToInt32(Convert.ToDouble(PolicyFee)).ToString("C2");
		//	txtFee.Text = Convert.ToInt32(Convert.ToDouble(PolicyFee)).ToString();
		//	lblFee.Visible = false;
		//	txtFee.Visible = true;

		//	hdnTotaltFee.Value = PolicyFee.ToString();
		//} 
		//else
		//{
		//	decimal.TryParse(lblInsCoFee.Text.Replace("$", "").Replace("$", "").Trim(), out InsCoFee);
		//	decimal.TryParse(lblMGAFee.Text.Replace("$", "").Replace("$", "").Trim(), out MGAPolicyFee);
		//	decimal.TryParse(lblAgentFee.Text.Replace("$", "").Replace("$", "").Trim(), out AgentFee);
		//	decimal InsuranceFeeMacEdwardsFee = (MGAPolicyFee);
		//	AgentFee = PolicyFee - InsuranceFeeMacEdwardsFee;
		//	lblInsCoFee.Text = Convert.ToInt32(Convert.ToDouble(InsuranceFeeMacEdwardsFee)).ToString("C2");
		//	lblMGAFee.Text = Convert.ToInt32(Convert.ToDouble(0M)).ToString("C2");
		//	lblAgentFee.Text = Convert.ToInt32(Convert.ToDouble(AgentFee)).ToString("C2");

		//	lblFee.Text = Convert.ToInt32(Convert.ToDouble(PolicyFee)).ToString("C2");
		//	txtFee.Text = Convert.ToInt32(Convert.ToDouble(PolicyFee)).ToString();
		//	lblFee.Visible = true;
		//	txtFee.Visible = false;

		//	hdnTotaltFee.Value = PolicyFee.ToString();
		//}
	}

	protected override void OnPreRender(EventArgs e)
	{
		if (!btnNo.Checked && !btnYes.Checked)
		{
			btnNo.Checked = true;
		}

		updLiveUsa.Visible = btnYes.Checked;
		base.OnPreRender(e);
	}

	private bool isCompany()
	{
		return PERSON_TYPE == ApplicationConstants.PERSONA_MORAL;
	}

	private bool isTotalOver2500()
	{
		decimal total = decimal.Parse(Regex.Match(lblTotal.Text, @"-?\d{1,3}(,\d{3})*(\.\d+)?").Value);
		decimal limit = 2500;

		if (CURRENCY != ApplicationConstants.DOLLARS_CURRENCY)
		{
			limit = limit * EXCHANGE_RATE;
		}
		return total > limit;
	}

	private void getExchangeRate()
	{
		UserInformation user = (UserInformation) Page.User.Identity;
		EXCHANGE_RATE = user.ExchangeRate;
	}

	private void SetupBreakdownLinks()
	{
		string link = "~/Application/PaymentWithInstalmentBreakdown.aspx?id=" + QuoteID + "&numOfPayments=";
		lnkBiyearly.NavigateUrl = link + "2";
		lnkQuarterly.NavigateUrl = link + "4";
	}

	private void SaveDateRange(string start)
	{
		DateTime startDate = DateTime.Parse(start);
		int agent = ((UserInformation) HttpContext.Current.User.Identity).Agent;
		if (agent != ApplicationConstants.MEAGENT && startDate < DateTime.Today)
		{
			return;
		}

		startDate = startDate.AddHours(12); //12:00PM
		DateTime endDate = startDate.AddYears(1).AddSeconds(-1);
		HouseInsuranceDataContext db = new HouseInsuranceDataContext();
		HttpSessionState Session = HttpContext.Current.Session;
		if (Session["quoteid"] == null)
		{
			return;
		}

		int quoteid = (int) Session["quoteid"];
		var house = db.quotes.Single(m => m.id == quoteid).insuredPersonHouse1;
		HouseEstate he = house.HouseEstate;
		db.spUpd_DateRangeOfQuote(quoteid, startDate, endDate, he.TEV, he.City.HMP, he.City.State.TheftZone[0]);
	}

	private bool isValidFee(int fee)
	{
		//decimal netPremium = decimal.Parse(lblNetPremium.Text.Replace("$", "").Replace("$", "").Trim());
		//decimal mgaPolicyFee = decimal.Parse(lblAgentFee.Text.Replace("$", "").Replace("$", "").Trim());
		bool isValid = true;
		valFee.Text = "";

		decimal minFee = 0;
		decimal maxFee = 0;

		var minimoFee = hdnMinPolicyFee.Value;
		var maximoFee = hdnMaxPolicyFee.Value;

		decimal.TryParse(minimoFee, out minFee);
		decimal.TryParse(maximoFee, out maxFee);

		if (fee > maxFee)
		{
			isValid = false;
			valFee.Text = $"Invalid. Max Fee Valid: {maxFee.ToString("C2")}";
		} else if (fee < minFee)
		{
			isValid = false;
			valFee.Text = $"Invalid. Min Fee Valid: {minFee.ToString("C2")}";
		} else
		{
			isValid = true;
			valFee.Text = "";
		}

		return isValid;

		//if (netPremium <= maxPremium && netPremium >= minPremium)
		//{
		//	if (fee >= (maxFee - mgaPolicyFee) || fee <= (minFee - mgaPolicyFee))
		//	{
		//		notFound = false;
		//		isValid = false;
		//		valFee.Text = $"The Policy Fee would be between {(minFee).ToString("C2")} and {(maxFee).ToString("C2")}, according to the registers in table PolicyFeeLimits.";
		//	} else
		//	{
		//		notFound = false;
		//		isValid = true;
		//		valFee.Text = "";
		//	}
		//}

		//UserInformation info = new UserInformation();
		//info = (UserInformation) Page.User.Identity;

		//if (dt.Rows.Count > 0)
		//{
		//	for (var r = 0; r < dt.Rows.Count; r++)
		//	{
		//		if (notFound == false)
		//		{
		//			continue;
		//		}

		//	}
		//	if (notFound == true)
		//	{
		//		isValid = false;
		//		valFee.Text = "There are no parameters for the net premium";
		//	}
		//} else
		//{
		//	isValid = false;
		//	valFee.Text = "There are no data in table PolicyFeeLimits";
		//}

		//return isValid;
		//if (CURRENCY == ApplicationConstants.DOLLARS_CURRENCY)
		//{
		//    if (fee < 30)
		//    {
		//        isValid = false;
		//    }
		//    else
		//    {
		//        if (netPremium <= 1000)
		//        {
		//            isValid = fee <= 100;
		//        }
		//        else if (netPremium <= 2500)
		//        {
		//            isValid = fee <= 250;
		//        }
		//        else if (netPremium <= 10000)
		//        {
		//            isValid = fee <= 500;
		//        }
		//        else
		//        {
		//            isValid = fee <= 1000;
		//        }
		//    }
		//}
		//else
		//{
		//    if (fee < 300)
		//    {
		//        isValid = false;
		//    }
		//    else
		//    {
		//        if (netPremium <= 1000 * EXCHANGE_RATE)
		//        {
		//            isValid = fee <= 100 * EXCHANGE_RATE;
		//        }
		//        else if (netPremium <= 2500 * EXCHANGE_RATE)
		//        {
		//            isValid = fee <= 250 * EXCHANGE_RATE;
		//        }
		//        /*else if (netPremium <= 10000 * EXCHANGE_RATE)
		//        {
		//            isValid = fee <= 500 * EXCHANGE_RATE;
		//        }*/
		//        else
		//        {
		//            isValid = fee <= 500 * EXCHANGE_RATE;
		//        }
		//    }
		//}
		//return isValid;
	}

	protected void btnBuyQuote_Click(object sender, EventArgs e)
	{
		// the post back allow us to verify the data through the asp.net validators
		if (SaveStep())
		{
			Session["quoteidBuy"] = GetKey();
			Response.Redirect("~/Application/BuyPolicy.aspx", true);
		}
	}
	protected void btnSaveQuote_Click(object sender, EventArgs e)
	{
		SaveStep();
	}
}
