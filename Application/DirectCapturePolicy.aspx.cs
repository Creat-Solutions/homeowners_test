using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using ME.Admon;
using Protec.Authentication.Membership;
using Telepro.Conexion;

public partial class Application_DirectCapturePolicy : Page
{
	public const int ADMONDB = ApplicationConstants.ADMINDB;
	protected int RETROACTIVE_DAYS;


	protected void Page_Load(object sender, EventArgs e)
	{
		var userInfo = Page.GetUserInformation();
		int productDefault = ApplicationConstants.HO_CAPTURA_DIRECTA;
		var usu = new Usuario(userInfo.UserID, ADMONDB, productDefault);
		var bu = new csBranchUser(usu.relation, ADMONDB);
		var br = new csBranch(bu.branch, ADMONDB);
		DataTable companies = csBranch.GetCompanyByProduct(ADMONDB, bu.branch, productDefault);
		UserInformationDB userDB = AdminGateway.Gateway.GetUserInformation(userInfo.UserID);
		RETROACTIVE_DAYS = userDB.RetroactivePolicyDays;

		if (!Page.IsPostBack)
		{
			//TextBox[] readOnlyInputs = new TextBox[] { txtMexicanTax, txtUSCommission, txtAmountDue, txtTotalPremium, lblTotCom, lblTotComPer };
			//foreach (TextBox input in readOnlyInputs)
			//{
			//	input.Attributes.Add("readonly", "readonly");
			//}

			ddlCompany.DataSource = companies;
			ddlCompany.DataBind();
			ddlCompany.Items.FindByValue(ApplicationConstants.COMPANY.ToString()).Selected = true;
			int company = int.Parse(ddlCompany.Items[ddlCompany.SelectedIndex].Value);

			ddlProduct.DataSource = csProduct.ListByCompanyForCustomInput(company, ADMONDB);
			ddlProduct.DataBind();
			ddlProduct.Items.FindByValue(productDefault.ToString()).Selected = true;

			ddlAgent.DataSource = csAgent.List(ADMONDB, productDefault.ToString());
			ddlAgent.DataBind();
			ddlAgent.Items.FindByValue(br.agent.ToString()).Selected = true;

			ddlBranch.DataSource = csBranch.List(
				 company,
				 productDefault,
				 int.Parse(ddlAgent.SelectedItem.Value),
				 ADMONDB
			);
			ddlBranch.DataBind();
			ddlBranch.Items.FindByValue(bu.branch.ToString()).Selected = true;

			ddlUser.DataSource = csBranchUser.ListByBranch(
				 int.Parse(ddlBranch.SelectedItem.Value),
				 ADMONDB
			);
			ddlUser.DataBind();
			ddlUser.Items.FindByValue(userInfo.UserID.ToString()).Selected = true;

			calStart.SelectedDate = DateTime.Today;
			calEnd.SelectedDate = DateTime.Today.AddDays(1);
		} else
		{
			calStart.SelectedDate = null;
			calEnd.SelectedDate = null;
		}
	}

	protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
	{
		ddlAgent.Items.Clear();
		ddlBranch.Items.Clear();
		ddlUser.Items.Clear();

		divAgents.Visible = false;
		divBranches.Visible = false;
		divUsers.Visible = false;

		LoadDropDownLists(e);
	}

	private void LoadDropDownLists(EventArgs args)
	{
		int company = int.Parse(ddlCompany.Items[ddlCompany.SelectedIndex].Value);
		int product = int.Parse(ddlProduct.SelectedItem.Value);

		ddlAgent.DataSource = csAgent.List(ADMONDB, product);
		ddlAgent.DataBind();

		if (ddlAgent.Items.Count > 0)
		{
			ddlAgent.SelectedIndex = 0;
			int agente = int.Parse(ddlAgent.SelectedItem.Value);

			ddlBranch.DataSource = csBranch.List(company, product, agente, ADMONDB);
			ddlBranch.DataBind();
			divAgents.Visible = true;

			if (ddlBranch.Items.Count > 0)
			{
				ddlBranch.SelectedIndex = 0;
				int branch = int.Parse(ddlBranch.SelectedItem.Value);

				ddlUser.DataSource = csBranchUser.ListByBranch(branch, ADMONDB);
				ddlUser.DataBind();
				divBranches.Visible = true;

				if (ddlUser.Items.Count > 0)
				{
					ddlUser.SelectedIndex = 0;
					divUsers.Visible = true;
					errorGeneral.Visible = false;
					lblErrorInformation.Visible = false;
					lblErrorInformation.Text = "";
					divDescription.Visible = true;
					return;
				}
			}
		}

		errorGeneral.Visible = true;
		lblErrorInformation.Visible = true;
		lblErrorInformation.Text = "There is no information for the current selection.";
		divDescription.Visible = false;
	}

	protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
	{
		divAgents.Visible = false;
		divBranches.Visible = false;
		divUsers.Visible = false;

		int branch = int.Parse(ddlBranch.SelectedItem.Value);
		ddlUser.DataSource = csBranchUser.ListByBranch(branch, ADMONDB);
		ddlUser.DataBind();

		divAgents.Visible = true;
		divBranches.Visible = true;

		if (ddlUser.Items.Count > 0)
		{
			ddlUser.SelectedIndex = 0;
			divUsers.Visible = true;
			errorGeneral.Visible = false;
			lblErrorInformation.Visible = false;
			lblErrorInformation.Text = "";
			divDescription.Visible = true;
			return;
		}

		divUsers.Visible = false;
		errorGeneral.Visible = true;
		lblErrorInformation.Visible = true;
		lblErrorInformation.Text = "There is no information for the current selection.";
		divDescription.Visible = false;
	}

	protected void ddlAgent_SelectedIndexChanged(object sender, EventArgs e)
	{
		divAgents.Visible = false;
		divBranches.Visible = false;
		divUsers.Visible = false;

		int company = int.Parse(ddlCompany.Items[ddlCompany.SelectedIndex].Value);
		int product = int.Parse(ddlProduct.SelectedItem.Value);
		int agente = int.Parse(ddlAgent.SelectedItem.Value);

		ddlBranch.Items.Clear();
		ddlBranch.DataSource = csBranch.List(company, product, agente, ADMONDB);
		ddlBranch.DataBind();

		divAgents.Visible = true;

		if (ddlBranch.Items.Count > 0)
		{
			ddlBranch.SelectedIndex = 0;
			int branch = int.Parse(ddlBranch.SelectedItem.Value);

			ddlUser.DataSource = csBranchUser.ListByBranch(branch, ADMONDB);
			ddlUser.DataBind();
			divBranches.Visible = true;

			if (ddlUser.Items.Count > 0)
			{
				ddlUser.SelectedIndex = 0;
				divUsers.Visible = true;
				errorGeneral.Visible = false;
				lblErrorInformation.Visible = false;
				lblErrorInformation.Text = "";
				divDescription.Visible = true;
				return;
			}
		}

		errorGeneral.Visible = true;
		lblErrorInformation.Visible = true;
		lblErrorInformation.Text = "There is no information for the current selection.";
		divDescription.Visible = false;
	}

	//protected void txtLastName_ServerValidate(object sender, ServerValidateEventArgs e)
	//{
	//	e.IsValid = rbnIndividual.Checked ? !string.IsNullOrEmpty(txtLastName.Text) : true;
	//}

	//protected void reqPerson_ServerValidate(object sender, ServerValidateEventArgs args)
	//{
	//	args.IsValid = rbnCorporation.Checked || rbnIndividual.Checked;
	//}

	protected void valStart_Validate(object sender, ServerValidateEventArgs e)
	{
		try
		{
			//int agent = ((UserInformation) Page.User.Identity).Agent;
			DateTime timeSelected = DateTime.Parse(e.Value);
			TimeSpan diferencia = timeSelected.Subtract(DateTime.Now);
			int totalDias = Math.Abs(diferencia.Days);
			var valid =  !(RETROACTIVE_DAYS < totalDias);
			e.IsValid = valid;
			valCStart.IsValid = valid;
			valCStart.Visible = !valid;
		} catch
		{
			e.IsValid = false;
		}
	}

	protected void valPolicy_Validate(object sender, ServerValidateEventArgs e)
	{
		try
		{
			var ps = new csPolicy(txtPolicyNo.Text.Trim(), ApplicationConstants.ADMINDB);
			if (ps.policy != -1)
			{
				if (ps.status.ToUpper() != "CANC")
				{
					e.IsValid = false;
					policyNoVal.ErrorMessage = $"The policy already exists with status {ps.status}";
					policyNoVal.Visible = true;
					policyNoVal.IsValid = false;
				}
			} else
			{
				e.IsValid = true;
				policyNoVal.IsValid = true;
				policyNoVal.Visible = false;
			}
		} catch
		{
			e.IsValid = false;
			//policyNoValidate.IsValid = false;
			//policyNoValidate.Visible = true;
		}
	}

	private void ClearErrors()
	{
		//lblAddress.Visible = false;
		lblInstSurcharge.Visible = false;
		lblMexicanTax.Visible = false;
		lblNetPremium.Visible = false;
		lblPolicyFee.Visible = false;
		//lblPolicyNo.Visible = false;
		lblTotalPremium.Visible = false;
		lblBrokerFee.Visible = false;
		lblUSCom.Visible = false;
		lblAmountDue.Visible = false;
		lblErrorSpecialCoverageFee.Visible = false;
		lblRXPF.Visible = false;
		lblCurrency.Visible = false;
		//policyNoValidate.IsValid = true;
		//policyNoValidate.Visible = false;
	}

	protected void btnSave_Click(object sender, System.EventArgs e)
	{
		bool canContinue = true; // variable para indicar datos válidos
		bool validStartDate = true;

		// se borra todo campo que haya tenido error
		ClearErrors();

		// valida que no exista el número de póliza
		if (string.IsNullOrEmpty(txtPolicyNo.Text.Trim()))
		{
			canContinue = false;
			RequiredPolicyNo.Text = "Required";
			RequiredPolicyNo.Visible = true;
			RequiredPolicyNo.ErrorMessage = "Required";
			RequiredPolicyNo.IsValid = false;
		} else
		{
			var ps = new csPolicy(txtPolicyNo.Text.Trim(), ApplicationConstants.ADMINDB);
			if (ps.policy != -1)
			{
				if (ps.status.ToUpper() != "CANC")
				{
					canContinue = false;
					policyNoVal.Visible = true;
					policyNoVal.ErrorMessage = $"The policy already exists with status {ps.status}";
					policyNoVal.IsValid = false;
				}
			}
		}

		if (txtName.Text.Trim() == "")
		{
			canContinue = false;
			reqName.Text = "Required";
			reqName.Visible = true;
			reqName.ErrorMessage = "Required";
			reqName.IsValid = false;
		}

		// if (rbnIndividual.Checked && string.IsNullOrEmpty(txtLastName.Text.Trim()))
		// {
		//     canContinue = false;
		//     reqLastName.Text = "Required";
		//     reqLastName.Visible = true;
		//     reqLastName.ErrorMessage = "Required";
		//     reqLastName.IsValid = false;
		// }

		// if (rbnCorporation.Checked)
		// {
		//     reqLastName.Text = string.Empty;
		//     reqLastName.Visible = false;
		//     reqLastName.ErrorMessage = string.Empty;
		//     reqLastName.IsValid = true;
		// }

		if (txtEmail.Text.Trim() == "")
		{
			canContinue = false;
			reqFieldsEmail.Text = "Required";
			reqFieldsEmail.Visible = true;
			reqFieldsEmail.ErrorMessage = "Required";
			reqFieldsEmail.IsValid = false;
		}

		if (
			 txtAreaPhone.Text.Trim() == ""
			 || txtPhone1.Text.Trim() == ""
			 || txtPhone2.Text.Trim() == ""
		)
		{
			canContinue = false;
			reqFieldsPhone.Text = "Required";
			reqFieldsPhone.Visible = true;
			reqFieldsPhone.ErrorMessage = "Required";
			reqFieldsPhone.IsValid = false;
		}

		if (txtZipCode.Text.Trim() == "")
		{
			canContinue = false;
			reqZipCode.Text = "Required";
			reqZipCode.Visible = true;
			reqZipCode.ErrorMessage = "Required";
			reqZipCode.IsValid = false;
		}

		if (hfHouseEstate.Value.Trim() == "")
		{
			canContinue = false;
			lblErrorHouseEstate.Text = "Select an option";
			lblErrorHouseEstate.Visible = true;
		}

		if (txtAddress.Text.Trim() == "")
		{
			canContinue = false;
			reqStreet.Text = "Required";
			reqStreet.Visible = true;
			reqStreet.ErrorMessage = "Required";
			reqStreet.IsValid = false;
		}

		if (txtInsuredExtNo.Text.Trim() == "")
		{
			canContinue = false;
			reqInsuredExtNo.Text = "Required";
			reqInsuredExtNo.Visible = true;
			reqInsuredExtNo.ErrorMessage = "Required";
			reqInsuredExtNo.IsValid = false;
		}

		if (txtDescription.Text.Trim() == "")
		{
			canContinue = false;
			requireFieldDescription.Text = "Required";
			requireFieldDescription.Visible = true;
			requireFieldDescription.ErrorMessage = "Required";
			requireFieldDescription.IsValid = false;
		}

		if (ddlCurrency.SelectedItem.Value == "0")
		{
			canContinue = false;
			lblCurrency.Visible = true;
		}

		if (txtStart.Text.Trim() == "")
		{
			canContinue = false;
			reqStart.Text = "Required";
			reqStart.ErrorMessage = "Required";
			reqStart.IsValid = false;
			reqStart.Visible = true;
			validStartDate = false;
		} else
		{
			try
			{
				var startDate = DateTime.Parse(txtStart.Text.Trim());
				TimeSpan diferencia = startDate.Subtract(DateTime.Now);
				int totalDias = Math.Abs(diferencia.Days);


				if (totalDias > RETROACTIVE_DAYS)
				{
					canContinue = false;
					valCStart.Text = "Invalid date";
					valCStart.Visible = true;
					valCStart.IsValid = false;
					valCStart.ErrorMessage = "Invalid date";
					validStartDate = false;
				}

				if (startDate < DateTime.Now.AddYears(-1) || startDate > DateTime.Now.AddYears(1))
				{
					canContinue = false;
					valCStart.Text = "Should not exceed one year";
					valCStart.Visible = true;
					valCStart.IsValid = false;
					valCStart.ErrorMessage = "Should not exceed one year";
					validStartDate = false;
				}
			} catch
			{
				canContinue = false;
				valCStart.Text = "Enter a valid date in format MM/dd/yyyy";
				valCStart.ErrorMessage = "Enter a valid date in format MM/dd/yyyy";
				valCStart.Visible = true;
				valCStart.IsValid = false;
				validStartDate = false;
			}
		}

		if (txtEnd.Text.Trim() == "")
		{
			canContinue = false;
			reqEnd.Text = "Required";
			reqEnd.ErrorMessage = "Required";
			reqEnd.IsValid = false;
			reqEnd.Visible = true;
		} else
		{
			try
			{
				if (validStartDate)
				{
					var endDate = DateTime.Parse(txtEnd.Text.Trim());
					var startDate = DateTime.Parse(txtStart.Text.Trim());

					if (endDate <= startDate)
					{
						canContinue = false;
						valCEnd.Text = "End date must be greather than start date";
						valCEnd.ErrorMessage = "End date must be greather than start date";
						valCEnd.IsValid = false;
						valCEnd.Visible = true;
					} 
					//else if ((endDate - startDate).Days > (DateTime.IsLeapYear(DateTime.Now.Year) ? 366 : 365))
					//{
					//	canContinue = false;
					//	valCEnd.Text = "Should not exceed one year of start date";
					//	valCEnd.ErrorMessage = "Should not exceed one year of start date";
					//	valCEnd.IsValid = false;
					//	valCEnd.Visible = true;
					//}
				} else
				{
					canContinue = false;
					valCEnd.Text = "Enter a valid startDate first";
					valCEnd.ErrorMessage = "Enter a valid startDate first";
					valCEnd.Visible = true;
				}
			} catch
			{
				canContinue = false;
				valCEnd.Text = "Enter a valid date in format MM/dd/yyyy";
				valCEnd.ErrorMessage = "Enter a valid date in format MM/dd/yyyy";
				valCEnd.Visible = true;
			}
		}

		if (txtNetPremium.Text.Trim() == "")
		{
			canContinue = false;
			lblNetPremium.Visible = true;
		} else
		{
			try
			{
				double.Parse(txtNetPremium.Text);
			} catch
			{
				canContinue = false;
				lblNetPremium.Visible = true;
			}
		}
		if (txtPolicyFee.Text.Trim() == "")
		{
			canContinue = false;
			lblPolicyFee.Visible = true;
		} else
		{
			try
			{
				double.Parse(txtPolicyFee.Text);
			} catch
			{
				canContinue = false;
				lblPolicyFee.Visible = true;
			}
		}
		if (txtInstSurcharge.Text.Trim() == "")
		{
			canContinue = false;
			lblInstSurcharge.Visible = true;
		} else
		{
			try
			{
				double.Parse(txtInstSurcharge.Text);
			} catch
			{
				canContinue = false;
				lblInstSurcharge.Visible = true;
			}
		}
		if (txtMexicanTax.Text.Trim() == "")
		{
			canContinue = false;
			lblMexicanTax.Visible = true;
		} else
		{
			try
			{
				double.Parse(txtMexicanTax.Text);
			} catch
			{
				canContinue = false;
				lblMexicanTax.Visible = true;
			}
		}
		if (txtTotalPremium.Text.Trim() == "")
		{
			canContinue = false;
			lblTotalPremium.Visible = true;
		} else
		{
			try
			{
				double.Parse(txtTotalPremium.Text);
			} catch
			{
				canContinue = false;
				lblTotalPremium.Visible = true;
			}
		}
		//if (txtBrokerFee.Text.Trim() != "")
		//{
		//    try
		//    {
		//        double.Parse(txtBrokerFee.Text);
		//    }
		//    catch
		//    {
		//        canContinue = false;
		//        lblBrokerFee.Visible = true;
		//    }
		//}
		if (txtTotalCommission.Text.Trim() != "")
		{
			try
			{
				double.Parse(txtTotalCommission.Text);
			} catch
			{
				canContinue = false;
			}
		}
		if (txtUSCommission.Text.Trim() != "")
		{
			try
			{
				double.Parse(txtUSCommission.Text);
			} catch
			{
				canContinue = false;
				lblUSCom.Visible = true;
			}
		}
		if (txtAmountDue.Text.Trim() != "")
		{
			try
			{
				var amountDue = double.Parse(txtAmountDue.Text.Replace("$", ""));
				double.Parse(txtAmountDue.Text);
			} catch
			{
				canContinue = false;
				lblAmountDue.Visible = true;
			}
		}
		if (txtRXPF.Text.Trim() != "")
		{
			try
			{
				double.Parse(txtRXPF.Text);
			} catch
			{
				canContinue = false;
				lblRXPF.Visible = true;
			}
		}

		//if (txtSpecialCoverageFee.Text.Trim() == "")
		//{
		//    canContinue = false;
		//    lblErrorSpecialCoverageFee.Visible = true;
		//}
		//else
		//{
		//    try
		//    {
		//        double.Parse(txtSpecialCoverageFee.Text);
		//    }
		//    catch
		//    {
		//        canContinue = false;
		//        lblErrorSpecialCoverageFee.Visible = true;
		//    }
		//}

		if (canContinue)
		{
			#region homeownersdb
			var hoPolicy = new hoPolicy(ApplicationConstants.DB);
			hoPolicy.policy = 0;
			hoPolicy.aplicacion = int.Parse(ddlProduct.SelectedItem.Value);
			hoPolicy.policyNo = txtPolicyNo.Text;
			hoPolicy.company = int.Parse(ddlCompany.SelectedItem.Value);
			hoPolicy.insured = int.Parse(ddlUser.SelectedItem.Value);
			hoPolicy.insurer = int.Parse(ddlUser.SelectedItem.Value);
			hoPolicy.issueDate = DateTime.Now.ToString("MM/dd/yyyy");
			hoPolicy.inception = txtStart.Text.Trim();
			hoPolicy.expiration = txtEnd.Text.Trim();
			hoPolicy.netPremium = double.Parse(txtNetPremium.Text);
			hoPolicy.policyFee = double.Parse(txtPolicyFee.Text);
			hoPolicy.taxPercentage = double.Parse(ddlTaxPercentage.SelectedItem.Value) / 100;
			hoPolicy.mexicanTax = double.Parse(hfMexicanTax.Value, NumberStyles.Currency);
			//hoPolicy.MGAFee = double.Parse(txtBrokerFee.Text);
			hoPolicy.usComission = double.Parse(hfmeCommission.Value, NumberStyles.Currency);
			hoPolicy.totalPremium = double.Parse(hfTotalPremium.Value, NumberStyles.Currency);
			//hoPolicy.incSisPol = double.Parse(txtAgencyFee.Text);
			hoPolicy.RXPF = double.Parse(txtRXPF.Text);
			hoPolicy.currency = int.Parse(ddlCurrency.SelectedItem.Value);
			hoPolicy.totalComission = double.Parse(hftotalCommission.Value, NumberStyles.Currency);
			hoPolicy.insuredName = txtName.Text.Trim().Replace("'", "");
			hoPolicy.insuredAddress = txtAddress.Text;
			hoPolicy.insuredPhone = txtAreaPhone.Text + txtPhone1.Text + txtPhone2.Text;
			hoPolicy.policyDescription = txtDescription.Text;
			//hoPolicy.specialCoverageFee = double.Parse(txtSpecialCoverageFee.Text);
			hoPolicy.currency = int.Parse(ddlCurrency.SelectedItem.Value);
			hoPolicy.agentName = ddlAgent.SelectedItem.Text;
			hoPolicy.userName = ddlUser.SelectedItem.Text;
			hoPolicy.totalComissionPer = double.Parse(hfpercentageComission.Value, NumberStyles.Currency) / 100;
			hoPolicy.mgaCom = (double.Parse(txtAgentCom.Text) + double.Parse(txtMECom.Text)) / 100;
			//EJDP -> 20/ENERO/2023 CAMPOS INSURED PERSON, INSURED PERSON HOUSE
			int houseEstateId = 0;
			int.TryParse(hfHouseEstate.Value, out houseEstateId);
			HouseEstate estate = houseEstateId == 0 ? null : new HouseEstate(houseEstateId);

			hoPolicy.legalPerson = rbnCorporation.Checked;
			hoPolicy.zipCode = txtZipCode.Text;
			hoPolicy.email = txtEmail.Text;
			hoPolicy.lastName = txtLastName.Text;
			hoPolicy.lastName2 = txtLastName2.Text;
			hoPolicy.city = estate == null ? "" : estate.City.Name;
			hoPolicy.state = estate == null ? 0 : estate.City.State.ID;
			hoPolicy.stateText = estate == null ? "" : estate.City.State.Name;
			hoPolicy.interiorNo = txtInsuredIntNo.Text;
			hoPolicy.exteriorNo = txtInsuredExtNo.Text;
			hoPolicy.houseEstate = houseEstateId;
			hoPolicy.coloniaText = estate == null ? "" : estate.Name;
			hoPolicy.Save();
			#endregion


			#region admon poliza
			var dtPolicy = hoPolicy.ReadPolicyHO(hoPolicy.policyNo);
			int insured = 0;
			int policyID = 0;
			if (dtPolicy.Rows.Count > 0)
			{
				insured = int.Parse(dtPolicy.Rows[0]["insuredPerson"].ToString());
				policyID = int.Parse(dtPolicy.Rows[0]["policy"].ToString());
			}

			string interiorNo = string.IsNullOrEmpty(hoPolicy.interiorNo) ? string.Empty : $" - {hoPolicy.interiorNo}";
			string caddress = $"{hoPolicy.exteriorNo} {interiorNo} {hoPolicy.insuredAddress} {hoPolicy.city}, {hoPolicy.stateText}";
			double totMGAa = (double.Parse(txtAgentCom.Text) + double.Parse(txtMECom.Text)) / 100;

			var pol = new csPolicy(ApplicationConstants.ADMINDB);
			pol.policy = policyID;
			pol.aplicacion = int.Parse(ddlProduct.SelectedItem.Value);
			pol.company = int.Parse(ddlCompany.SelectedItem.Value);
			pol.insured = insured;
			pol.policyNo = txtPolicyNo.Text.Trim();
			pol.insurer = int.Parse(ddlUser.SelectedItem.Value);
			pol.issueDate = DateTime.Now.ToString("MM/dd/yyyy");
			pol.inception = txtStart.Text.Trim();
			pol.expiration = txtEnd.Text.Trim();
			pol.netPremium = double.Parse(txtNetPremium.Text);
			pol.policyFee = double.Parse(txtPolicyFee.Text);
			//@installmentSurcharge de donde se toma???
			pol.taxPercentage = double.Parse(ddlTaxPercentage.SelectedItem.Value) / 100;
			pol.mexicanTax = double.Parse(hfMexicanTax.Value, NumberStyles.Currency);
			//@brokerFee de donde se toma??
			pol.totalComission = double.Parse(hftotalCommission.Value, NumberStyles.Currency);
			pol.usComission = double.Parse(hfmeCommission.Value, NumberStyles.Currency);
			//pol.incSisPol = double.Parse(txtAgencyFee.Text);
			pol.totalPremium = double.Parse(hfTotalPremium.Value, NumberStyles.Currency);
			pol.insuredName = $"{txtName.Text.Trim().Replace("'", "")} {hoPolicy.lastName} {hoPolicy.lastName2}";
			pol.insuredAddress = caddress ?? string.Empty;
			pol.insuredPhone = $"{txtAreaPhone.Text}{txtPhone1.Text}{txtPhone2.Text}";
			pol.policyDescription = txtDescription.Text;
			//pol.specialCoverageFee = double.Parse(txtSpecialCoverageFee.Text);
			pol.mgaCom = totMGAa;
			pol.insuredCountry = "USA";
			//pol.MGAFee = double.Parse(txtBrokerFee.Text);
			pol.currency = int.Parse(ddlCurrency.SelectedItem.Value);
			pol.RXPF = double.Parse(txtRXPF.Text);
			pol.Save(true);
			#endregion
			// FIN admon poliza

			ScriptManager.RegisterClientScriptBlock(
				 this,
				 this.GetType(),
				 "script",
				 "alert('The policy has been registered correctly'); window.location.href = 'DirectCapturePolicy.aspx'; ",
				 true
			);
		}
	}
}

public class hoPolicy
{
	#region "Properties"
	private int db;
	public int policy,
		 state,
		 houseEstate;
	public string policyNo,
		 inception,
		 expiration,
		 issueDate,
		 policyDescription;
	public string insuredName,
		 insuredAddress,
		 insuredPhone,
		 status,
		 insuredCountry,
		 balanceID;
	public int aplicacion,
		 insured,
		 insurer,
		 company,
		 totalPayments,
		 currency;
	public double netPremium,
		 policyFee,
		 installmentSurcharge,
		 RXPF,
		 RXPFPercentage;
	public double taxPercentage,
		 mexicanTax,
		 brokerFee,
		 totalComission,
		 mgaCom;
	public double usComission,
		 incSisPol,
		 totalPremium,
		 specialCoverageFee,
		 MGAFee;
	public bool isCanceled,
		 legalPerson;
	public double totalComissionPer;
	public string agentName,
		 userName,
		 zipCode,
		 email,
		 lastName,
		 lastName2,
		 city,
		 stateText;
	public string interiorNo,
		 exteriorNo,
		 coloniaText;

	#endregion
	#region "Constructors"
	public hoPolicy(int dataBase)
	{
		policy = -1;
		db = dataBase;
		isCanceled = false;
		legalPerson = false;
		policyNo = "";
		zipCode = "";
		email = "";
		lastName = "";
		lastName2 = "";
		city = "";
		state = 0;
		stateText = "";
		interiorNo = "";
		exteriorNo = "";
		houseEstate = 0;
		coloniaText = "";
		inception = DateTime.Today.ToShortDateString();
		expiration = DateTime.Today.ToShortDateString();
		issueDate = DateTime.Today.ToString();
		policyDescription = "";
		insuredName = "";
		insuredAddress = "";
		insuredPhone = "";
		status = "";
		balanceID = "";
		aplicacion = 0;
		insured = 0;
		insurer = 0;
		company = 1;
		netPremium = 0;
		policyFee = 0;
		installmentSurcharge = 0;
		taxPercentage = .11;
		mexicanTax = 0;
		brokerFee = 0;
		totalComission = 0;
		MGAFee = 0;
		totalPayments = 1;
		RXPF = 0;
		RXPFPercentage = 0;
		usComission = 0;
		incSisPol = 0;
		totalPremium = 0;
		specialCoverageFee = 0;
		currency = 2; // 2 para USA Dlls; 1 para Pesos Mexicano; De acuerdo a lo establecido en la applicación HO
		insuredCountry = "USA";
	}

	#endregion
	#region "Methods"

	public void Save()
	{
		var objCon = new dbConexion(db);
		string sql = "spIns_DirectPolicyHO ";
		sql +=
			 policy.ToString()
			 + ", "
			 + aplicacion.ToString()
			 + ", "
			 + company.ToString()
			 + ", "
			 + insured.ToString()
			 + ", '"
			 + policyNo.Trim()
			 + "', "
			 + insurer.ToString()
			 + ", '"
			 + issueDate.Trim()
			 + "', '"
			 + inception.Trim()
			 + "', '"
			 + expiration.Trim()
			 + "', "
			 + netPremium.ToString()
			 + ", "
			 + policyFee.ToString()
			 + ", "
			 + installmentSurcharge.ToString()
			 + ", "
			 + taxPercentage.ToString()
			 + ", "
			 + mexicanTax.ToString()
			 + ", "
			 + brokerFee.ToString()
			 + ", "
			 + totalComission.ToString()
			 + ", "
			 + usComission.ToString()
			 + ", "
			 + incSisPol.ToString()
			 + ", "
			 + totalPremium.ToString()
			 + ", '"
			 + insuredName.Trim().Replace("'", "").Replace(",", " ")
			 + "', '"
			 + insuredAddress.Trim().Replace("'", "").Replace(",", " ")
			 + "', '"
			 + insuredPhone.Trim().Replace("'", "").Replace(",", " ")
			 + "', '"
			 + policyDescription.Trim().Replace("'", "").Replace(",", " ")
			 + "', "
			 + specialCoverageFee.ToString()
			 + ", "
			 + mgaCom
			 + ", '"
			 + insuredCountry.Trim()
			 + "',"
			 + MGAFee.ToString()
			 + ", "
			 + currency.ToString()
			 + ", "
			 + RXPF.ToString()
			 + ","
			 + totalComissionPer.ToString()
			 + ", '"
			 + agentName
			 + "', '"
			 + userName
			 + "', "
			 + (legalPerson ? "1" : "0")
			 + ", '"
			 + zipCode
			 + "', '"
			 + email
			 + "', '"
			 + lastName
			 + "', '"
			 + lastName2
			 + "', '"
			 + city
			 + "', "
			 + state.ToString()
			 + ", '"
			 + stateText
			 + "', '"
			 + interiorNo
			 + "', '"
			 + exteriorNo
			 + "',"
			 + houseEstate
			 + ", '"
			 + coloniaText
			 + "'";
		try
		{
			objCon.ExecuteQuery(sql);
		} catch (Exception ex)
		{
			Console.WriteLine(ex);
		}
	}

	public DataTable ReadPolicyHO(string strKey)
	{
		var objCon = new dbConexion(db);

		DataTable dt = objCon
			 .ExecuteDS("select * from policy where policyNo = '" + strKey.ToString() + "'")
			 .Tables[0];

		return dt;
	}
	#endregion
}
