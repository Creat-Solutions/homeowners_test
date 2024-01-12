using ME.Admon;
using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telepro.Conexion;

public partial class Application_DirectCapture : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			TextBox[] readOnlyInputs = new TextBox[] { txtMexicanTax, txtUSCommission, txtAmountDue, txtTotalPremium, lblTotCom, lblTotComPer };
			foreach (TextBox input in readOnlyInputs)
			{
				input.Attributes.Add("readonly", "readonly");
			}
			InitCompany(0, sender, e);
		}
	}

	override protected void OnInit(EventArgs e)
	{
		InitializeComponent();
		base.OnInit(e);
	}

	private void InitializeComponent()
	{
		//this.ddlCompany.SelectedIndexChanged += new System.EventHandler(this.ddlCompany_SelectedIndexChanged);
		//this.ddlproduct.SelectedIndexChanged += new System.EventHandler(this.ddlproduct_SelectedIndexChanged);
		//this.ddlAgent.SelectedIndexChanged += new System.EventHandler(this.ddlAgent_SelectedIndexChanged);
		//this.ddlBranch.SelectedIndexChanged += new System.EventHandler(this.ddlBranch_SelectedIndexChanged);
		this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
		this.Load += new System.EventHandler(this.Page_Load);

	}

	private void InitCompany(int company, object sender, System.EventArgs e)
	{
		ddlCompany.Items.Clear();
		ddlCompany.DataSource = csCompany.List(ApplicationConstants.ADMINDB);
		ddlCompany.DataBind();
		ddlCompany.Items.Insert(0, new ListItem("-- Select --", "-1"));
		try
		{
			//ddlCompany.Items.FindByValue(company.ToString()).Selected = true;
		}
		catch { }

		ddlCompany_SelectedIndexChanged(sender, e);
	}

	private void InitAgent(int agent)
	{
		ddlAgent.Items.Clear();
		ddlAgent.DataSource = csAgent.List(ApplicationConstants.ADMINDB,
			int.Parse(ddlCompany.SelectedItem.Value),
			int.Parse(ddlproduct.SelectedItem.Value));
		ddlAgent.DataBind();
		ddlAgent.Items.Insert(0, new ListItem("-- Select --", "-1"));
		try
		{
			//ddlproduct.Items.FindByValue(agent.ToString()).Selected = true;
		}
		catch { }
	}

	protected void reqPerson_ServerValidate(object sender, ServerValidateEventArgs args)
	{
		args.IsValid = rbnCorporation.Checked || rbnIndividual.Checked;
	}

	private void ClearErrors()
	{
		lblAddress.Visible = false;
		lblExpiration.Visible = false;
		lblInception.Visible = false;
		lblInstSurcharge.Visible = false;
		lblInsuredName.Visible = false;
		lblMexicanTax.Visible = false;
		lblNetPremium.Visible = false;
		lblPolicyFee.Visible = false;
		lblPolicyNo.Visible = false;
		lblTotalPremium.Visible = false;
		lblBrokerFee.Visible = false;
		lblUSCom.Visible = false;
		lblAmountDue.Visible = false;
		lblErrorSpecialCoverageFee.Visible = false;
		lblErrorDescription.Visible = false;
		lblErrorNoPoliza.Visible = false;
		lblRXPF.Visible = false;
		lblCurrency.Visible = false;
	}

	private void ddlAgent_SelectedIndexChanged(object sender, System.EventArgs e)
	{
		GetBranch();
		GetUser();
	}

	private void GetBranch()
	{
		ddlBranch.Items.Clear();
		ddlBranch.DataSource = csBranch.List(int.Parse(ddlCompany.SelectedItem.Value),
			int.Parse(ddlproduct.SelectedItem.Value),
			int.Parse(ddlAgent.SelectedItem.Value),
			ApplicationConstants.ADMINDB);
		ddlBranch.DataBind();
		ddlBranch.Items.Insert(0, new ListItem("-- Select --", "-1"));
	}

	private void GetUser()
	{
		if (ddlBranch.Items.Count > 0)
		{
			ddlUser.DataSource = csBranchUser.ListByBranch(int.Parse(ddlBranch.SelectedItem.Value),
				ApplicationConstants.ADMINDB);
			ddlUser.DataBind();
		}
	}

	private void ddlBranch_SelectedIndexChanged(object sender, System.EventArgs e)
	{
		ddlUser.Items.Clear();
		ddlUser.DataSource = csBranchUser.ListByBranch(int.Parse(ddlBranch.SelectedItem.Value),
			ApplicationConstants.ADMINDB);
		ddlUser.DataBind();
	}

	protected void txtLastName_ServerValidate(object sender, ServerValidateEventArgs e)
	{
		e.IsValid = rbnIndividual.Checked ? !string.IsNullOrEmpty(txtLastName.Text) : true;

	}

	private void btnSave_Click(object sender, System.EventArgs e)
	{
		//este es código ejemplo de validación

		bool canContinue = true; // variable para indicar datos válidos
		bool validStartDate = true;

		// se borra todo campo que haya tenido error 
		ClearErrors();

		// valida que no exista el número de póliza
		csPolicy ps = new csPolicy(txtPolicyNo.Text.Trim(), ApplicationConstants.ADMINDB);
		if (ps.policy != -1)
		{ canContinue = false; lblErrorNoPoliza.Visible = true; }

		// aquí también se tienen que validar datos numéricos
		if (txtName.Text.Trim() == "")
		{ canContinue = false; lblInsuredName.Visible = true; }
		if (txtPolicyNo.Text.Trim() == "")
		{ canContinue = false; lblPolicyNo.Visible = true; }

		if (txtFechaIni.Value.Trim() == "")
		{
			canContinue = false;
			lblInception.Text = "Required";
			lblInception.Visible = true;
		}
		else
		{
			try
			{
				var startDate = DateTime.Parse(txtFechaIni.Value);
				if (startDate < DateTime.Now.AddYears(-1) || startDate > DateTime.Now.AddYears(1))
				{
					canContinue = false;
					lblInception.Text = "Should not exceed one year";
					lblInception.Visible = true;
				}

			}
			catch
			{
				canContinue = false;
				lblInception.Text = "Enter a valid date in format MM/dd/yyyy";
				lblInception.Visible = true;
			}
		}

		if (txtFechaFin.Value.Trim() == "")
		{
			canContinue = false;
			lblExpiration.Visible = true;
		}
		else
		{
			try
			{
				if (validStartDate)
				{
					var endDate = DateTime.Parse(txtFechaFin.Value);
					var startDate = DateTime.Parse(txtFechaIni.Value);

					if (endDate <= startDate)
					{
						canContinue = false;
						lblExpiration.Text = "End date must be greather than start date";
						lblExpiration.Visible = true;
					}
					else if ((endDate - startDate).TotalDays > 365)
					{
						canContinue = false;
						lblExpiration.Text = "Should not exceed one year of start date";
						lblExpiration.Visible = true;
					}
				}
				else
				{
					canContinue = false;
					lblExpiration.Text = "Enter a valid startDate first";
					lblExpiration.Visible = true;
				}

			}
			catch
			{
				canContinue = false;
				lblExpiration.Text = "Enter a valid date in format MM/dd/yyyy";
				lblExpiration.Visible = true;
			}
		}
		if (txtNetPremium.Text.Trim() == "")
		{
			canContinue = false;
			lblNetPremium.Visible = true;
		}
		else
		{
			try
			{
				double.Parse(txtNetPremium.Text);
			}
			catch
			{
				canContinue = false;
				lblNetPremium.Visible = true;
			}
		}
		if (txtPolicyFee.Text.Trim() == "")
		{
			canContinue = false;
			lblPolicyFee.Visible = true;
		}
		else
		{
			try
			{
				double.Parse(txtPolicyFee.Text);
			}
			catch
			{
				canContinue = false;
				lblPolicyFee.Visible = true;
			}
		}
		if (txtInstSurcharge.Text.Trim() == "")
		{
			canContinue = false;
			lblInstSurcharge.Visible = true;
		}
		else
		{
			try
			{
				double.Parse(txtInstSurcharge.Text, NumberStyles.Currency);
			}
			catch
			{
				canContinue = false;
				lblInstSurcharge.Visible = true;
			}
		}
		if (txtMexicanTax.Text.Trim() == "")
		{
			canContinue = false;
			lblMexicanTax.Visible = true;
		}
		else
		{
			try
			{
				double.Parse(txtMexicanTax.Text, NumberStyles.Currency);
			}
			catch
			{
				canContinue = false;
				lblMexicanTax.Visible = true;
			}
		}
		if (txtTotalPremium.Text.Trim() == "")
		{
			canContinue = false;
			lblTotalPremium.Visible = true;
		}
		else
		{
			try
			{
				double.Parse(txtTotalPremium.Text, NumberStyles.Currency);
			}
			catch
			{
				canContinue = false;
				lblTotalPremium.Visible = true;
			}
		}
		if (txtBrokerFee.Text.Trim() != "")
		{
			try
			{
				double.Parse(txtBrokerFee.Text);
			}
			catch
			{
				canContinue = false;
				lblBrokerFee.Visible = true;
			}
		}
		if (txtTotalCommission.Text.Trim() != "")
		{
			try
			{
				double.Parse(txtTotalCommission.Text, NumberStyles.Currency);
			}
			catch
			{
				canContinue = false;
				//					lblTotalCom.Visible = true;
			}
		}
		if (txtUSCommission.Text.Trim() != "")
		{
			try
			{
				double.Parse(txtUSCommission.Text, NumberStyles.Currency);
			}
			catch
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
				double.Parse(txtAmountDue.Text, NumberStyles.Currency);
			}
			catch
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
			}
			catch
			{
				canContinue = false;
				lblRXPF.Visible = true;
			}
		}
		if (txtDescription.Text.Trim() == "")
		{
			canContinue = false;
			lblErrorDescription.Visible = true;
		}
		if (txtSpecialCoverageFee.Text.Trim() == "")
		{
			canContinue = false;
			lblErrorSpecialCoverageFee.Visible = true;
		}
		else
		{
			try
			{
				double.Parse(txtSpecialCoverageFee.Text);
			}
			catch
			{
				canContinue = false;
				lblErrorSpecialCoverageFee.Visible = true;
			}
		}
		if (ddlCurrency.SelectedItem.Value == "0")
		{
			canContinue = false;
			lblCurrency.Visible = true;
		}

		if (canContinue)
		{
			#region homeownersdb
			hoPolicy_old hoPolicy = new hoPolicy_old(ApplicationConstants.DB);
			hoPolicy.policy = 0;
			hoPolicy.aplicacion = int.Parse(ddlproduct.SelectedItem.Value);
			hoPolicy.policyNo = txtPolicyNo.Text;
			hoPolicy.company = int.Parse(ddlCompany.SelectedItem.Value);
			hoPolicy.insured = int.Parse(ddlUser.SelectedItem.Value);
			hoPolicy.insurer = int.Parse(ddlUser.SelectedItem.Value);
			hoPolicy.issueDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm");
			hoPolicy.inception = txtFechaIni.Value;
			hoPolicy.expiration = txtFechaFin.Value;

			hoPolicy.netPremium = double.Parse(txtNetPremium.Text);
			hoPolicy.policyFee = double.Parse(txtPolicyFee.Text);
			hoPolicy.taxPercentage = double.Parse(ddlTaxPercentage.SelectedItem.Value) / 100;
			hoPolicy.mexicanTax = double.Parse(txtMexicanTax.Text, NumberStyles.Currency);
			hoPolicy.MGAFee = double.Parse(txtBrokerFee.Text);
			hoPolicy.usComission = double.Parse(txtUSCommission.Text, NumberStyles.Currency);
			hoPolicy.totalPremium = double.Parse(txtTotalPremium.Text, NumberStyles.Currency);
			hoPolicy.incSisPol = double.Parse(txtAgencyFee.Text, NumberStyles.Currency);
			hoPolicy.RXPF = double.Parse(txtRXPF.Text);
			hoPolicy.currency = int.Parse(ddlCurrency.SelectedItem.Value);
			hoPolicy.totalComission = double.Parse(lblTotCom.Text, NumberStyles.Currency);
			hoPolicy.insuredName = txtName.Text.Trim().Replace("'", "");
			hoPolicy.insuredAddress = txtAddress.Text;
			hoPolicy.insuredPhone = txtPhone.Text;
			hoPolicy.policyDescription = txtDescription.Text;
			hoPolicy.specialCoverageFee = double.Parse(txtSpecialCoverageFee.Text);
			hoPolicy.currency = int.Parse(ddlCurrency.SelectedItem.Value);
			hoPolicy.agentName = ddlAgent.SelectedItem.Text;
			hoPolicy.userName = ddlUser.SelectedItem.Text;
			hoPolicy.totalComissionPer = double.Parse(lblTotComPer.Text, NumberStyles.Currency) / 100;

			// the stored procedure gets the default if MGA Com is sent in 0
			double totMGA = (double.Parse(txtAgentCom.Text, NumberStyles.Currency) + double.Parse(txtMECom.Text, NumberStyles.Currency)) / 100;
			hoPolicy.mgaCom = totMGA;

			hoPolicy.Save();
			#endregion
			#region admon poliza
			var dtPolicy = hoPolicy.ReadPolicyHO(hoPolicy.policyNo);
			int insured = 0;
			if (dtPolicy.Rows.Count > 0)
				insured = int.Parse(dtPolicy.Rows[0]["insuredPerson"].ToString());

			csPolicy pol = new csPolicy(ApplicationConstants.ADMINDB);
			pol.policy = 0;
			pol.aplicacion = int.Parse(ddlproduct.SelectedItem.Value);
			pol.policyNo = txtPolicyNo.Text;
			pol.company = int.Parse(ddlCompany.SelectedItem.Value);
			pol.insured = insured;
			pol.insurer = int.Parse(ddlUser.SelectedItem.Value);
			pol.issueDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm");
			pol.inception = txtFechaIni.Value;
			pol.expiration = txtFechaFin.Value;

			pol.netPremium = double.Parse(txtNetPremium.Text);
			pol.policyFee = double.Parse(txtPolicyFee.Text);
			pol.taxPercentage = double.Parse(ddlTaxPercentage.SelectedItem.Value) / 100;
			pol.mexicanTax = double.Parse(txtMexicanTax.Text, NumberStyles.Currency);
			pol.MGAFee = double.Parse(txtBrokerFee.Text);
			pol.usComission = double.Parse(txtUSCommission.Text, NumberStyles.Currency);
			pol.totalPremium = double.Parse(txtTotalPremium.Text, NumberStyles.Currency);
			pol.incSisPol = double.Parse(txtAgencyFee.Text, NumberStyles.Currency);
			pol.RXPF = double.Parse(txtRXPF.Text);
			pol.currency = int.Parse(ddlCurrency.SelectedItem.Value);
			pol.totalComission = double.Parse(lblTotCom.Text, NumberStyles.Currency);
			pol.insuredName = txtName.Text.Trim().Replace("'", "");
			pol.insuredAddress = txtAddress.Text;
			pol.insuredPhone = txtPhone.Text;
			pol.policyDescription = txtDescription.Text;
			pol.specialCoverageFee = double.Parse(txtSpecialCoverageFee.Text);
			pol.currency = int.Parse(ddlCurrency.SelectedItem.Value);

			// the stored procedure gets the default if MGA Com is sent in 0
			double totMGAa = (double.Parse(txtAgentCom.Text, NumberStyles.Currency) + double.Parse(txtMECom.Text, NumberStyles.Currency)) / 100;
			pol.mgaCom = totMGAa;

			pol.Save(true);
			//Response.Redirect("DirectCapture.aspx");

			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "alert('The policy has been registered correctly'); window.location.href = 'DirectCapture.aspx'; ", true);
			#endregion
			// FIN admon poliza
		}
	}

	private void ddlCompany_SelectedIndexChanged(object sender, System.EventArgs e)
	{
		ddlproduct.Items.Clear();
		ddlproduct.DataSource = csProduct.ListByCompanyForCustomInput(
			int.Parse(ddlCompany.SelectedItem.Value),
			ApplicationConstants.ADMINDB);
		ddlproduct.DataBind();
		ddlproduct.Items.Insert(0, new ListItem("-- Select --", "-1"));
		try
		{
			//ddlproduct.Items.FindByValue(lblProduct.Text).Selected = true;
		}
		catch
		{ }
	}

	private void ddlproduct_SelectedIndexChanged(object sender, System.EventArgs e)
	{
		InitAgent(0);
	}

	private void ClearInfo()
	{
		txtPolicyNo.Text = ""; txtName.Text = "";
		txtAddress.Text = ""; txtPhone.Text = "";
		txtFechaIni.Value = ""; txtFechaFin.Value = "";
		txtDescription.Text = ""; txtNetPremium.Text = "0";
		txtPolicyFee.Text = "0"; txtInstSurcharge.Text = "0";
		txtMexicanTax.Text = "0";
		txtTotalPremium.Text = "0"; txtBrokerFee.Text = "0";
		txtTotalCommission.Text = "0"; txtUSCommission.Text = "0";
		txtAmountDue.Text = "0";
		txtSpecialCoverageFee.Text = "0";
		InitAgent(0);
	}

}

public class hoPolicy_old
{
	#region "Properties"
	private int db;
	public int policy;
	public string policyNo, inception, expiration, issueDate, policyDescription;
	public string insuredName, insuredAddress, insuredPhone, status, insuredCountry, balanceID;
	public int aplicacion, insured, insurer, company, totalPayments, currency;
	public double netPremium, policyFee, installmentSurcharge, RXPF, RXPFPercentage;
	public double taxPercentage, mexicanTax, brokerFee, totalComission, mgaCom;
	public double usComission, incSisPol, totalPremium, specialCoverageFee, MGAFee;
	public bool isCanceled;
	public double totalComissionPer;
	public string agentName, userName;

	#endregion
	#region "Constructors"
	public hoPolicy_old(int dataBase)
	{
		policy = -1;
		db = dataBase;
		isCanceled = false;
		policyNo = "";
		inception = DateTime.Today.ToShortDateString();
		expiration = DateTime.Today.ToShortDateString();
		issueDate = DateTime.Today.ToString();
		policyDescription = ""; insuredName = ""; insuredAddress = "";
		insuredPhone = ""; status = ""; balanceID = "";
		aplicacion = 0; insured = 0; insurer = 0; company = 1;
		netPremium = 0; policyFee = 0; installmentSurcharge = 0;
		taxPercentage = .11; mexicanTax = 0; brokerFee = 0; totalComission = 0;
		MGAFee = 0; totalPayments = 1; RXPF = 0; RXPFPercentage = 0;
		usComission = 0; incSisPol = 0; totalPremium = 0; specialCoverageFee = 0;
		currency = 2; // 2 para USA Dlls; 1 para Pesos Mexicano; De acuerdo a lo establecido en la applicación HO
		insuredCountry = "USA";
	}

	#endregion
	#region "Methods"

	public void Save()
	{
		dbConexion objCon = new dbConexion(db);
		string sql = "spIns_policyAsIs ";
		sql += policy.ToString() + ", " + aplicacion.ToString() + ", " +
			company.ToString() + ", " + insured.ToString() + ", '" +
			policyNo.Trim() + "', " + insurer.ToString() + ", '" +
			issueDate.Trim() + "', '" + inception.Trim() + "', '" +
			expiration.Trim() + "', " + netPremium.ToString() + ", " +
			policyFee.ToString() + ", " + installmentSurcharge.ToString() + ", " +
			taxPercentage.ToString() + ", " + mexicanTax.ToString() + ", " +
			brokerFee.ToString() + ", " + totalComission.ToString() + ", " +
			usComission.ToString() + ", " + incSisPol.ToString() + ", " +
			totalPremium.ToString() + ", '" + insuredName.Trim().Replace("'", "").Replace(",", " ") + "', '" +
			insuredAddress.Trim().Replace("'", "").Replace(",", " ") + "', '" +
			insuredPhone.Trim().Replace("'", "").Replace(",", " ") + "', '" +
			policyDescription.Trim().Replace("'", "").Replace(",", " ") + "', " +
			specialCoverageFee.ToString() + ", " + mgaCom + ", '" + insuredCountry.Trim() + "'," +
			MGAFee.ToString() + ", " + currency.ToString() + ", " + RXPF.ToString() + "," +
			totalComissionPer.ToString() + ", '" + agentName + "', '" + userName + "'";

		try
		{
			objCon.ExecuteQuery(sql);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
		}
	}

	public DataTable ReadPolicyHO(string strKey)
	{
		dbConexion objCon = new dbConexion(db);

		DataTable dt = objCon.ExecuteDS("select * from policy where policyNo = '" + strKey.ToString() + "'").Tables[0];

		return dt;
	}
	#endregion
}