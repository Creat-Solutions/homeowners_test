using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using ME.Admon;

using Protec.Authentication.Membership;

using Telepro.Conexion;

public partial class UserControls_GenerateStep3 : System.Web.UI.UserControl, IApplicationStep
{
	protected bool CURRENCY_DOLLARS = true;
	private const int PRIMARY_RESIDENCE = 1;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (Session["quoteid"] == null)
		{
			throw new Exception("Session[quoteid] is null");
		}

		int key = (int) Session["quoteid"];
		HouseInsuranceDataContext db = new HouseInsuranceDataContext();
		var currency = db.quotes.Select(m => new
		{
			m.id,
			m.currency
		}).Single(m => m.id == key);
		CURRENCY_DOLLARS = currency.currency == ApplicationConstants.DOLLARS_CURRENCY ? true : false;
	}

	public void LoadStep()
	{
		if (Session["quoteid"] == null)
		{
			throw new Exception("Session[quoteid] is null");
		}

		int key = (int) Session["quoteid"];
		HouseInsuranceDataContext db = new HouseInsuranceDataContext();

		var info = db.quotes.Select(m => new
		{
			m.id,
			m.buildingLimit,
			m.outdoorLimit,
			m.contentsLimit,
			m.debriLimit,
			m.extraLimit,
			m.lossOfRentsLimit,
			m.outdoorDescription,
			m.fireAll,
			m.quake,
			m.HMP,
			m.limitsEnabled,
			m.currency,
			m.otherStructuresLimit,
			m.otherStructuresDescription,
			m.additionalNotes
		}).Single(m => m.id == key);

		LoadTable(db, info.buildingLimit, info.otherStructuresLimit, info.contentsLimit, info.currency);

		LoadTextbox(txtBuilding, info.buildingLimit, chkBuilding);

		if (info.outdoorLimit == 0)
		{
			txtOutdoorDescription.Enabled = false;
			txtOutdoorDescription.Text = string.Empty;
		} else
		{
			txtOutdoorDescription.Enabled = true;
			txtOutdoorDescription.Text = info.outdoorDescription;
		}

		if (info.otherStructuresLimit == 0)
		{
			txtOtherStructuresDescription.Enabled = false;
			txtOtherStructuresDescription.Text = string.Empty;
		} else
		{
			txtOtherStructuresDescription.Enabled = true;
			txtOtherStructuresDescription.Text = info.otherStructuresDescription;
		}

		if (!string.IsNullOrEmpty(info.additionalNotes))
		{
			txtAdditionalNotes.Text = info.additionalNotes;
		}

		txtContents.Text = info.contentsLimit.ToString("N");


		txtDebris.Text = info.debriLimit.ToString("N");
		txtExtra.Text = info.extraLimit.ToString("N");
		LoadTextbox(txtLossRents, info.lossOfRentsLimit, chkLossRents);
		LoadTextbox(txtOtherStructures, info.otherStructuresLimit, chkOtherStructures);
		UserInformation userInfo = (UserInformation) Page.User.Identity;
		bool isAdministrator = userInfo.UserType == SecurityManager.ADMINISTRATOR;
		bool enabledHMP = db.spIsValidHMPDate() == 1 || isAdministrator;
		chkHMP.Enabled = enabledHMP;
		lblInactiveHMP.Visible = !enabledHMP;
		//if (info.fireAll != null)
		//    chkFire.Checked = (bool)info.fireAll;
		if (info.quake != null)
		{
			chkQuake.Checked = (bool) info.quake;
		}

		if (info.HMP != null && enabledHMP)
		{
			chkHMP.Checked = (bool) info.HMP;
		} else if (!enabledHMP)
		{
			chkHMP.Checked = false;
		}

		if (chkHMP.Checked)
		{
			LoadTextbox(txtOutdoor, info.outdoorLimit, chkOutdoor);
			chkOutdoor.Enabled = true;
		} else
		{
			LoadTextbox(txtOutdoor, 0, chkOutdoor);
			chkOutdoor.Enabled = false;
		}

		if (!info.limitsEnabled)
		{
			Page.ClientScript.RegisterStartupScript(GetType(), "BoolLimits", "ToggleValidators();", true);
		}
		string dlls = info.currency == ApplicationConstants.DOLLARS_CURRENCY ? "true" : "false";
		Page.ClientScript.RegisterStartupScript(GetType(), "BoolCurrencyDollars", "StartCurrency(" + dlls + ");", true);

		if ((chkHMP.Checked || chkQuake.Checked) && chkLossRents.Checked)
		{
			lossRentsWarning.Style.Add(HtmlTextWriterStyle.Display, "inline");
		} else
		{
			lossRentsWarning.Style.Add(HtmlTextWriterStyle.Display, "none");
		}

		LoadTable(db, key, info.contentsLimit, info.currency);

		var q = db.quotes.Select(m => new
		{
			m.id,
			m.CSLLimit,
			m.moneySecurityLimit,
			m.accidentalGlassBreakageLimit,
			m.electronicEquipmentLimit,
			m.personalItemsLimit,
			m.domesticWorkersLiability,
			m.burglaryLimit,
			m.burglaryJewelryLimit,
			m.familyAssistance,
			m.limitsEnabled,
			m.useOfProperty,
			m.techSupport
		}).Single(m => m.id == key);

		txtCSL.Text = q.CSLLimit.ToString("N");
		chkDomestic.Checked = q.domesticWorkersLiability;
		LoadLimitValue(txtBurglary, q.burglaryLimit, chkBurglary);
		LoadLimitValue(txtJewelry, q.burglaryJewelryLimit, chkJewelry, q.useOfProperty);
		LoadLimitValue(txtMoney, q.moneySecurityLimit, chkMoney, q.useOfProperty);
		LoadLimitValue(txtGlass, q.accidentalGlassBreakageLimit, chkGlass);
		LoadLimitValue(txtElectronic, q.electronicEquipmentLimit, chkElectronic);
		LoadLimitValue(txtPersonal, q.personalItemsLimit, chkPersonal);
		chkFamily.Checked = q.familyAssistance;
		chkTechSupport.Checked = q.techSupport;

		string boolPrimary = q.useOfProperty == PRIMARY_RESIDENCE ? "true" : "false";
		Page.ClientScript.RegisterStartupScript(GetType(), "primaryResidenceInit", "primaryResidence = " + boolPrimary + ";", true);// +

		lblFamilyCovered.Text = chkFamily.Checked ? GetLocalResourceObject("strCovered").ToString() : GetLocalResourceObject("strExcluded").ToString();

		lblTechSupportCovered.Text = chkTechSupport.Checked ? GetLocalResourceObject("strCovered").ToString() : GetLocalResourceObject("strExcluded").ToString();
		LoadPremium(key);
	}

	public bool SaveStep()
	{
		if (Page.IsValid)
		{
			try
			{
				saveData((int) Session["quoteid"]);
			} catch
			{
				return false;
			}
			return true;
		}
		return false;
	}

	public string Title()
	{
		return GetLocalResourceObject("strLimitsDetails").ToString();
	}

	private void LoadPremium(int key)
	{
		if (key <= 0)
		{
			throw new Exception("key <= 0");
		}

		HouseInsuranceDataContext db = new HouseInsuranceDataContext();
		try
		{
			var q = db.quotes.SingleOrDefault(m => m.id == key && m.company == ApplicationConstants.COMPANY);
			if (q == null)
			{
				return;
			}
			HouseEstate he = q.insuredPersonHouse1.HouseEstate;
			var codigos = db.spList_Calcula(q.id, he.City.HMP).Single();
			decimal netPremium = (decimal) codigos.netPremium;
			lblPremium.Text = netPremium.ToString("C");
		} catch { }
	}

	private void LoadLimitValues(HtmlTableCell minimum, HtmlTableCell maximum, decimal min, decimal max, bool percentage, decimal sum)
	{
		string minStr, maxStr;
		if (percentage)
		{
			if (sum == 0)
			{
				minStr = (min / 100).ToString("P");
				maxStr = (max / 100).ToString("P");
			} else
			{
				minStr = (sum * min / 100).ToString("C");
				maxStr = (sum * max / 100).ToString("C");
			}
		} else
		{
			minStr = min.ToString("C");
			maxStr = max.ToString("C");
		}
		minimum.InnerHtml = minStr;
		maximum.InnerHtml = maxStr;
	}

	private void LoadTable(HouseInsuranceDataContext db, decimal buildingLimit, decimal otherStructuresLimit, decimal contentsLimit, int currency)
	{
		var limits = db.valueSublimits.Where(m => m.company == ApplicationConstants.COMPANY).ToArray();
		if (limits.Length != 6)
		{
			return;
		}

		HtmlTableCell[] tdMinimum = { tdBuildingMinimum, tdOutdoorMinimum, tdContentsMinimum, tdDebrisMinimum, tdExtraMinimum, tdLossRentsMinimum };
		HtmlTableCell[] tdMaximum = { tdBuildingMaximum, tdOutdoorMaximum, tdContentsMaximum, tdDebrisMaximum, tdExtraMaximum, tdLossRentsMaximum };

		UserInformation user = (UserInformation) HttpContext.Current.User.Identity;
		decimal tipoCambio = user.ExchangeRate;
		var sum = buildingLimit + contentsLimit;
		bool pesos = currency == ApplicationConstants.PESOS_CURRENCY;
		for (int i = 0; i < 6; i++)
		{
			decimal minimum = limits[i].minimum;
			decimal maximum = limits[i].maximum;
			if (limits[i].percentage)
			{
				Page.ClientScript.RegisterArrayDeclaration("percentageMinimum", minimum.ToString("F2"));
				Page.ClientScript.RegisterArrayDeclaration("percentageMaximum", maximum.ToString("F2"));
			} else
			{
				Page.ClientScript.RegisterArrayDeclaration("valuesMinimum", minimum.ToString("F2"));
				Page.ClientScript.RegisterArrayDeclaration("valuesMaximum", maximum.ToString("F2"));
				if (pesos)
				{
					minimum *= tipoCambio;
					maximum *= tipoCambio;
					Page.ClientScript.RegisterArrayDeclaration("valuesPesosMinimum", minimum.ToString("F2"));
					Page.ClientScript.RegisterArrayDeclaration("valuesPesosMaximum", maximum.ToString("F2"));
				} else
				{
					Page.ClientScript.RegisterArrayDeclaration("valuesPesosMinimum", (minimum * tipoCambio).ToString("F2"));
					Page.ClientScript.RegisterArrayDeclaration("valuesPesosMaximum", (maximum * tipoCambio).ToString("F2"));
				}
			}
			if (i == 0)
			{
				// building limit
				LoadLimitValues(tdMinimum[i], tdMaximum[i], minimum, maximum - otherStructuresLimit, false, 0);
				LoadLimitValues(tdOtherStructuresMinimum, tdOtherStructuresMaximum, 0, maximum - buildingLimit, false, 0);
			} else
			{
				LoadLimitValues(tdMinimum[i], tdMaximum[i], minimum, maximum, limits[i].percentage, sum);
			}
		}
	}

	private void LoadTextbox(TextBox txtLimit, decimal value, CheckBox chkLimit)
	{
		if (value == 0)
		{
			txtLimit.Enabled = chkLimit.Checked = false;
			txtLimit.Text = string.Empty;
		} else
		{
			txtLimit.Enabled = chkLimit.Checked = true;
			txtLimit.Text = value.ToString("N");
		}
	}

	private decimal GetLimitData(TextBox txtLimit)
	{
		try
		{
			return string.IsNullOrEmpty(txtLimit.Text.Trim()) ? 0 : decimal.Parse(txtLimit.Text.Trim());
		} catch
		{
			return 0;
		}
	}

	private void saveData(int key)
	{
		if (key <= 0)
		{
			throw new Exception("key <= 0");
		}

		decimal buildingLimit = GetLimitData(txtBuilding);
		decimal outdoorLimit = chkHMP.Checked ? GetLimitData(txtOutdoor) : 0;
		decimal contentsLimit = GetLimitData(txtContents);
		decimal debrisLimit = GetLimitData(txtDebris);
		decimal extraLimit = GetLimitData(txtExtra);
		decimal lossOfRentsLimit = GetLimitData(txtLossRents);
		var otherStructuresLimit = GetLimitData(txtOtherStructures);

		string description = null;
		if (outdoorLimit > 0)
		{
			description = txtOutdoorDescription.Text.Trim();
		}

		string otherStructuresDescription = null;
		if (otherStructuresLimit > 0)
		{
			otherStructuresDescription = txtOtherStructuresDescription.Text.Trim();
		}

		//bool fireAll = chkFire.Checked;
		HouseInsuranceDataContext db = new HouseInsuranceDataContext();

		bool fireAll = true;
		UserInformation userInfo = (UserInformation) Page.User.Identity;
		bool isAdministrator = userInfo.UserType == SecurityManager.ADMINISTRATOR;
		bool enabledHMP = db.spIsValidHMPDate() == 1 || isAdministrator;

		bool quake = chkQuake.Checked, HMP = enabledHMP && chkHMP.Checked;
		bool limitsEnabled = true;

		int tipoCambio = CURRENCY_DOLLARS ? ApplicationConstants.DOLLARS_CURRENCY : ApplicationConstants.PESOS_CURRENCY;

		db.spUpd_V1_quoteLimits(
				key,
				buildingLimit,
				outdoorLimit,
				contentsLimit,
				debrisLimit,
				extraLimit,
				lossOfRentsLimit,
				otherStructuresLimit,
				description,
				otherStructuresDescription,
				fireAll,
				quake,
				HMP,
				limitsEnabled,
				tipoCambio
		);

		var quoteInfo = db.quotes.Where(m => m.id == key).Select(m => new { m.useOfProperty, m.insuredPersonHouse1 }).Single();
		int useProperty = (int) quoteInfo.useOfProperty;

		decimal CGLLimit = GetLimitData(txtCSL);
		bool domestic = true;
		decimal burglaryLimit = GetLimitData(txtBurglary);
		decimal burglaryJewelsLimit = GetLimitData(txtJewelry);
		if (burglaryLimit == 0)
		{
			burglaryJewelsLimit = 0;
		}
		decimal moneyLimit = GetLimitData(txtMoney);
		decimal glassLimit = GetLimitData(txtGlass);
		decimal electronicLimit = GetLimitData(txtElectronic);
		decimal personalLimit = GetLimitData(txtPersonal);
		bool family = chkFamily.Checked;
		bool techSupport = chkTechSupport.Checked;

		if (useProperty != PRIMARY_RESIDENCE)
		{
			burglaryJewelsLimit = moneyLimit = 0;
		}

		if (db.spUpd_V1_quoteCoverages(key, CGLLimit, domestic, burglaryLimit, burglaryJewelsLimit, moneyLimit,
															 glassLimit, electronicLimit, personalLimit, family, techSupport) == 0)
		{
			var he = quoteInfo.insuredPersonHouse1.HouseEstate;
			db.spUpd_quote(key, he.TEV, he.City.HMP, he.City.State.TheftZone[0]);
			limitsEnabled = db.quotes.Where(m => m.id == key).Select(m => m.limitsEnabled).Single();
			if (!limitsEnabled)
			{
				SendEmail(key);
			}
			//invalidate cache (InsuranceApplication.aspx.cs+ShouldDisableButtons)
			Session["disableButtons"] = false;
		} else
		{
			throw new Exception("quoteCoverages != 0");
		}
		using (HouseInsuranceDataContext myDC = new HouseInsuranceDataContext())
		{
			if (!string.IsNullOrEmpty(txtAdditionalNotes.Text.Trim()))
			{
				var db_quote = myDC.quotes.Where(m => m.id == key).Single();
				if (db_quote != null)
				{
					db_quote.additionalNotes = txtAdditionalNotes.Text.Trim();
					myDC.SubmitChanges();
				}
			}
		}
	}

	private void SendEmail(int quoteid)
	{
		Control c = Page.LoadControl("~/UserControls/Calcula.ascx");
		(c as IQuoteData).quoteid = quoteid;
		StringWriter sw = new StringWriter();
		HtmlTextWriter writer = new HtmlTextWriter(sw);
		string renderedContents = string.Empty;
		try
		{
			c.RenderControl(writer);
			renderedContents = sw.GetStringBuilder().ToString();
		} catch (Exception e)
		{
			renderedContents = "Error while processing the data. Please access the quote information directly. Quote number = " + quoteid;
			renderedContents += "<br />Error message: " + e.Message;
			renderedContents += "<br />Stack trace: " + e.StackTrace;
		} finally
		{
			sw.Close();
			writer.Close();
		}
		//string from = ConfigurationSettings.AppSettings["MailFrom"];
		//string to = ConfigurationSettings.AppSettings["MailNoLimits"];
		//MailMessage message = new MailMessage(from, to);
		//message.CC.Add("manuel@creatsol.com");
		//message.IsBodyHtml = true;
		//message.Subject = "Homeowner's Insurance Application: Cotizacion sin limites no. " + quoteid;
		//message.Body = renderedContents;
		//SmtpClient client = new SmtpClient(ConfigurationSettings.AppSettings["MailServer"]);
		//if (ConfigurationSettings.AppSettings["MailAuthenticationRequired"] == "true")
		//{
		//    client.Credentials = new NetworkCredential(from, ConfigurationSettings.AppSettings["MailPassword"]);
		//}
		//client.Send(message);
	}

	private void LoadLimits(HtmlTableCell minimum, HtmlTableCell maximum, decimal min, decimal max, bool percentage, decimal contentsLimit)
	{
		string minStr, maxStr;
		if (percentage)
		{
			if (contentsLimit == 0)
			{
				minStr = (min / 100).ToString("P");
				maxStr = (max / 100).ToString("P");
			} else
			{
				minStr = (contentsLimit * min / 100).ToString("C");
				maxStr = (contentsLimit * max / 100).ToString("C");
			}
		} else
		{
			minStr = min.ToString("C");
			maxStr = max.ToString("C");
		}
		minimum.InnerHtml = minStr;
		maximum.InnerHtml = maxStr;
	}

	private void LoadTable(HouseInsuranceDataContext db, int quoteid, decimal contentsLimit, int currency)
	{
		var coverages = db.additionalCoverages.ToArray();
		if (coverages.Length != 10)
		{
			return;
		}

		HtmlTableCell[] tdMinimum = { tdCSLMinimum, tdDomesticMinimum, tdBurglaryMinimum, tdJewelryMinimum, tdMoneyMinimum, tdGlassMinimum, tdElectronicMinimum, tdPersonalMinimum, tdFamilyMinimum };
		HtmlTableCell[] tdMaximum = { tdCSLMaximum, tdDomesticMaximum, tdBurglaryMaximum, tdJewelryMaximum, tdMoneyMaximum, tdGlassMaximum, tdElectronicMaximum, tdPersonalMaximum, tdFamilyMaximum };

		bool pesos = currency == ApplicationConstants.PESOS_CURRENCY;
		UserInformation user = (UserInformation) HttpContext.Current.User.Identity;
		decimal tipoCambio = user.ExchangeRate;

		for (int i = 0; i < coverages.Length; i++)
		{
			if (coverages[i].minimum == null && coverages[i].maximum == null)
			{
				continue;
			}

			decimal minimum = coverages[i].minimum ?? 0;
			decimal maximum = coverages[i].maximum ?? 0;
			if (coverages[i].percentage)
			{
				Page.ClientScript.RegisterArrayDeclaration("additionalPercentageMinimum", minimum.ToString("F2"));
				Page.ClientScript.RegisterArrayDeclaration("additionalPercentageMaximum", maximum.ToString("F2"));
			} else
			{
				if (pesos)
				{
					minimum *= tipoCambio;
					maximum *= tipoCambio;

					if (i == 0 && maximum > 20000000M)
					{
						maximum = 20000000M;
					}
				}
				Page.ClientScript.RegisterArrayDeclaration("additionalValuesMinimum", minimum.ToString("F2"));
				Page.ClientScript.RegisterArrayDeclaration("additionalValuesMaximum", maximum.ToString("F2"));
			}
			LoadLimits(tdMinimum[i], tdMaximum[i], minimum, maximum, coverages[i].percentage, contentsLimit);
		}
	}

	private void LoadLimitValue(TextBox txtLimit, decimal value, CheckBox chkLimit, int? use)
	{
		if (use != null && use != PRIMARY_RESIDENCE)
		{
			chkLimit.Enabled = chkLimit.Checked = txtLimit.Enabled = false;
			txtLimit.Text = string.Empty;
		} else if (value == 0)
		{
			chkLimit.Checked = txtLimit.Enabled = false;
			txtLimit.Text = string.Empty;
		} else
		{
			chkLimit.Checked = txtLimit.Enabled = true;
			txtLimit.Text = value.ToString("N");
		}
	}

	protected void valOutdoor_Validate(object source, ServerValidateEventArgs args)
	{
		TextBox txtOutdoor;
		CustomValidator validator = source as CustomValidator;
		try
		{
			txtOutdoor = validator.FindControl(validator.ControlToValidate) as TextBox;
			if (txtOutdoor == null || chkOutdoor == null)
			{
				throw new Exception();
			}
		} catch
		{
			args.IsValid = false;
			return;
		}
		args.IsValid = !chkOutdoor.Checked || txtOutdoor.Text.Length > 0;
	}

	protected void valOtherStructuresDescription_Validate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = !chkOtherStructures.Checked || txtOtherStructuresDescription.Text.Length > 0;
	}

	protected void valBuilding_ServerValidate(object source, ServerValidateEventArgs args)
	{
		decimal otherStructures = 0;
		if (chkOtherStructures.Checked)
		{
			try
			{
				otherStructures = decimal.Parse(txtOtherStructures.Text.Replace(",", ""));
			} catch
			{
			}
		}
		valLimit_ServerValidate(source as CustomValidator, args, 1, !chkBuilding.Checked, otherStructures);
	}

	protected void valOtherStructures_ServerValidate(object source, ServerValidateEventArgs args)
	{
		decimal building = 0;
		if (chkBuilding.Checked)
		{
			try
			{
				building = decimal.Parse(txtBuilding.Text.Replace(",", ""));
			} catch
			{
			}
		}
		valLimit_ServerValidate(source as CustomValidator, args, 1, !chkOtherStructures.Checked, building, 0);
	}

	protected void valOutdoor_ServerValidate(object source, ServerValidateEventArgs args)
	{
		valLimit_ServerValidate(source as CustomValidator, args, 2, !chkOutdoor.Checked);
	}

	protected void valContents_ServerValidate(object source, ServerValidateEventArgs args)
	{
		valLimit_ServerValidate(source as CustomValidator, args, 3, false);
	}

	protected void valDebris_ServerValidate(object source, ServerValidateEventArgs args)
	{
		valLimit_ServerValidate(source as CustomValidator, args, 4, false);
	}

	protected void valExtra_ServerValidate(object source, ServerValidateEventArgs args)
	{
		valLimit_ServerValidate(source as CustomValidator, args, 5, false);
	}

	protected void valLossRents_ServerValidate(object source, ServerValidateEventArgs args)
	{
		valLimit_ServerValidate(source as CustomValidator, args, 6, !chkLossRents.Checked);
	}

	private void valLimit_ServerValidate(CustomValidator validator, ServerValidateEventArgs args, int id, bool emptyAllowed)
	{
		valLimit_ServerValidate(validator, args, id, emptyAllowed, 0, null);
	}

	private void valLimit_ServerValidate(CustomValidator validator, ServerValidateEventArgs args, int id, bool emptyAllowed, decimal maxSubstract)
	{
		valLimit_ServerValidate(validator, args, id, emptyAllowed, maxSubstract, null);
	}

	private void LoadLimitValue(TextBox txtLimit, decimal value, CheckBox chkLimit)
	{
		LoadLimitValue(txtLimit, value, chkLimit, null);
	}

	protected void valCSL_ServerValidate(object source, ServerValidateEventArgs args)
	{
		valAdditionalLimit_ServerValidate(source as CustomValidator, args, 1, false);
	}

	protected void valBurglary_ServerValidate(object source, ServerValidateEventArgs args)
	{
		valAdditionalLimit_ServerValidate(source as CustomValidator, args, 3, !chkBurglary.Checked);
	}

	protected void valJewelry_ServerValidate(object source, ServerValidateEventArgs args)
	{
		valAdditionalLimit_ServerValidate(source as CustomValidator, args, 4, !chkJewelry.Checked);
	}

	protected void valMoney_ServerValidate(object source, ServerValidateEventArgs args)
	{
		valAdditionalLimit_ServerValidate(source as CustomValidator, args, 5, !chkMoney.Checked);
	}

	protected void valGlass_ServerValidate(object source, ServerValidateEventArgs args)
	{
		valAdditionalLimit_ServerValidate(source as CustomValidator, args, 6, !chkGlass.Checked);
	}

	protected void valElectronic_ServerValidate(object source, ServerValidateEventArgs args)
	{
		valAdditionalLimit_ServerValidate(source as CustomValidator, args, 7, !chkElectronic.Checked);
	}

	protected void valPersonal_ServerValidate(object source, ServerValidateEventArgs args)
	{
		valAdditionalLimit_ServerValidate(source as CustomValidator, args, 8, !chkPersonal.Checked);
	}


	private void valAdditionalLimit_ServerValidate(CustomValidator validator, ServerValidateEventArgs args, int id, bool emptyAllowed)
	{
		decimal value;
		validator.Text = "Invalid value. ";

		if (string.IsNullOrEmpty(args.Value))
		{
			args.IsValid = emptyAllowed;
			if (!emptyAllowed)
			{
				validator.Text = "Required";
			}
			return;
		}

		try
		{
			value = decimal.Parse(args.Value);
		} catch
		{
			args.IsValid = false;
			return;
		}
		if (id <= 0)
		{
			args.IsValid = false;
			return;
		}
		HouseInsuranceDataContext db = new HouseInsuranceDataContext();
		decimal minimum, maximum;
		try
		{
			int quoteid = (int) Session["quoteid"];
			var limit = db.additionalCoverages.Select(m => new { m.id, m.minimum, m.maximum, m.percentage }).Single(m => m.id == id);
			var info = db.quotes.Where(m => m.id == quoteid).Select(m => new { m.contentsLimit, m.currency }).Single();
			decimal content = 0M;
			try
			{
				if (!string.IsNullOrEmpty(txtContents.Text))
				{
					content = decimal.Parse(txtContents.Text);
				}
			} catch
			{
				content = 0M;
			}
			int currency = info.currency;
			minimum = (decimal) limit.minimum;
			maximum = (decimal) limit.maximum;
			if (limit.percentage)
			{
				minimum *= (decimal) content / 100;
				maximum *= (decimal) content / 100;
			} else if (currency != 2)
			{
				UserInformation user = (UserInformation) HttpContext.Current.User.Identity;
				decimal tipoCambio = user.ExchangeRate;
				minimum *= tipoCambio;
				maximum *= tipoCambio;

				// max 20M en pesos
				if (validator == valCSL && maximum > 20000000M)
				{
					maximum = 20000000M;
				}

			}
		} catch
		{
			args.IsValid = false;
			return;
		}
		bool valid = value >= minimum && value <= maximum;
		if (!valid)
		{
			validator.Text += "Minimum: " + minimum.ToString("C") + ", Maximum: " + maximum.ToString("C");
		}

		args.IsValid = valid;
	}


	private void valLimit_ServerValidate(CustomValidator validator, ServerValidateEventArgs args, int id, bool emptyAllowed, decimal maxSubstract, decimal? forcedMinimum)
	{
		decimal value;
		validator.Text = "Invalid value. ";

		if (string.IsNullOrEmpty(args.Value))
		{
			args.IsValid = emptyAllowed;
			if (!emptyAllowed)
			{
				validator.Text = "Required";
			}
			return;
		}

		try
		{
			value = decimal.Parse(args.Value);
		} catch
		{
			args.IsValid = false;
			return;
		}

		if ((validator == valBuilding || validator == valContents))
		{
			args.IsValid = true;
			return;
		}

		HouseInsuranceDataContext db = new HouseInsuranceDataContext();
		decimal minimum, maximum;
		try
		{
			var limit = db.valueSublimits.Where(m => m.company == ApplicationConstants.COMPANY).Select(m => new { m.id, m.minimum, m.maximum, m.percentage }).Single(m => m.id == id);
			if (limit.percentage)
			{
				TextBox building = txtBuilding;
				TextBox content = txtContents;

				decimal buildingValue = 0;
				try
				{
					buildingValue = string.IsNullOrEmpty(building.Text) ? 0M : decimal.Parse(building.Text);
				} catch
				{
					buildingValue = 0;
				}

				decimal sum = buildingValue + decimal.Parse(content.Text);

				if (validator == valDebris || validator == valExtra)
				{
					minimum = sum * limit.minimum / 100;
					maximum = sum * limit.maximum / 100;
				} else
				{
					throw new Exception();
				}
			} else
			{
				minimum = limit.minimum;
				maximum = limit.maximum;
				//distinto a Dolares
				if (!CURRENCY_DOLLARS)
				{
					UserInformation user = (UserInformation) HttpContext.Current.User.Identity;
					decimal tipoCambio = user.ExchangeRate;
					minimum *= tipoCambio;
					maximum *= tipoCambio;
				}
			}
		} catch
		{
			args.IsValid = false;
			return;
		}
		maximum -= maxSubstract;
		if (forcedMinimum != null)
		{
			minimum = forcedMinimum.Value;
		}

		bool valid = value >= minimum && value <= maximum;
		if (!valid)
		{
			validator.Text += "Minimum: " + minimum.ToString("C") + ", Maximum: " + maximum.ToString("C");
		}

		args.IsValid = valid;
	}

	protected void btnUpdate_Click(object sender, EventArgs e)
	{
		if (Page.IsValid)
		{
			try
			{
				saveData((int) Session["quoteid"]);
				LoadPremium((int) Session["quoteid"]);
			} catch { }
		}
	}

	/// <summary>
	/// Obtiene el tipo de cambio al dia del dólar a pesos desde la API del diario oficial de la federación
	/// </summary>
	/// <returns>decimal con el tipo de cambio del dólar</returns>
	//private decimal getExchangeRateFromDOF_API()
	//{
	//	UserInformation user = (UserInformation) HttpContext.Current.User.Identity;
	//	decimal tipoCambio = user.ExchangeRate;

	//	string apiUrl = $"{ApplicationConstants.DiarioOficialFederacion_API}";

	//	// Crear una instancia de HttpClient
	//	using (HttpClient client = new HttpClient())
	//	{
	//		try
	//		{
	//			// Realizar una solicitud GET a la API
	//			HttpResponseMessage response = client.Get(apiUrl);

	//			// Comprobar si la solicitud fue exitosa (código de estado 200)
	//			if (response.IsSuccessStatusCode)
	//			{
	//				// Leer el contenido de la respuesta como una cadena JSON
	//				string jsonResponse = response.Content.ReadAsString();

	//				// Deserializar el JSON en una instancia de la clase ApiResponse
	//				Api_DOF_Response apiResponse = JsonConvert.DeserializeObject<Api_DOF_Response>(jsonResponse);

	//				// Puedes acceder a los datos como propiedades de la clase ApiResponse
	//				if (apiResponse.messageCode == 200)
	//				{
	//					;
	//				}

	//				{
	//					// Buscar el primer indicador con codTipoIndicador igual a 158
	//					var indicadorDolar = apiResponse.ListaIndicadores
	//							.FirstOrDefault(indicador => indicador.codTipoIndicador == 158);

	//					if (indicadorDolar != null)
	//					{
	//						decimal.TryParse(indicadorDolar.valor, out tipoCambio)

	//													return tipoCambio;
	//					} else
	//					{
	//						return tipoCambio;
	//					}
	//				}

	//									else
	//				{
	//					{
	//					{
	//					{
	//					return tipoCambio;
	//				}
	//				}
	//				}
	//				}
	//			} else
	//			{
	//				return tipoCambio;
	//			}
	//		} catch (Exception ex)
	//		{
	//			return tipoCambio;
	//		}
	//	}

	//	return tipoCambio;
	//}
	//protected void CustomSumValidator_ServerValidate(object source, ServerValidateEventArgs args)
	//{
	//	decimal totalSum = 0;

	//	// Obtiene el ID del control que se está validando
	//	string controlId = ((CustomValidator) source).ControlToValidate;
	//	TextBox textBox = FindControl(controlId) as TextBox;

	//	if (textBox != null && !string.IsNullOrEmpty(textBox.Text))
	//	{
	//		totalSum += decimal.Parse(textBox.Text);
	//	}

	//	// Realiza la validación
	//	args.IsValid = totalSum <= 500;
	//}

	protected void CustomSumValidator_ServerValidate(object source, ServerValidateEventArgs args)
	{
		decimal totalSum = 0M;

		// Obtén los valores de los campos y suma solo si no están vacíos
		if (!string.IsNullOrEmpty(txtBuilding.Text))
		{
			totalSum += decimal.Parse(txtBuilding.Text);
		}

		if (!string.IsNullOrEmpty(txtOtherStructures.Text))
		{
			totalSum += decimal.Parse(txtOtherStructures.Text);
		}

		if (!string.IsNullOrEmpty(txtContents.Text))
		{
			totalSum += decimal.Parse(txtContents.Text);
		}

		if (!string.IsNullOrEmpty(txtDebris.Text))
		{
			totalSum += decimal.Parse(txtDebris.Text);
		}

		if (!string.IsNullOrEmpty(txtExtra.Text))
		{
			totalSum += decimal.Parse(txtExtra.Text);
		}

		string sqlMaxLimit = $"select maxHOpropertyValue from ProductCompany where product = {ApplicationConstants.APPLICATION} and company = {ApplicationConstants.COMPANY}";

		dbConexion con = new dbConexion(ApplicationConstants.ADMINDB);
		DataTable dtProductCompany = con.ExecuteDS(sqlMaxLimit).Tables[0];

		if (dtProductCompany.Rows.Count > 0)
		{
			decimal MaxValue = 0M;
			decimal.TryParse(dtProductCompany.Rows[0]["maxHOpropertyValue"].ToString(), out MaxValue);

			if (CURRENCY_DOLLARS)
			{
				MaxValue = MaxValue;
			} else
			{
				UserInformation user = (UserInformation) HttpContext.Current.User.Identity;
				decimal tipoCambio = user.ExchangeRate;
				MaxValue = (MaxValue * tipoCambio);
			}

			// Realiza la validación
			//args.IsValid = totalSum < MaxValue;
			if (totalSum >= MaxValue)
			{
				CustomValidator3.Text = $"Maximum limit exceeded, must be less than {MaxValue.ToString("C2")}. It will not be possible to issue the policy.";
			}
		}


	}
}
