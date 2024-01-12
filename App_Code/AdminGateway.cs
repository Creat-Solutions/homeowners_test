using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ME.Admon;
using ME.Mexicard;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Protec.Authentication.Membership;
using Protec.Tools;
using Telepro.Conexion;

public struct AgentInformation
{
	public int Agent { get; set; }
	public string AgentName { get; set; }
	public static readonly AgentInformation EmptyAgentInformation = new AgentInformation {
		Agent = 0,
		AgentName = string.Empty
	};
}

public struct BranchInformation
{
	public int Branch { get; set; }
	
	public string BranchAddress { get; set; }
	public string BranchCityState { get; set; }
	public string BranchCode { get; set; }
	public string BranchCountry { get; set; }
	public string BranchZipCode { get; set; }
	public static readonly BranchInformation EmptyBranchInformation = new BranchInformation {
		Branch = 0,
		BranchAddress = string.Empty,
		BranchCityState = string.Empty,
		BranchCode = string.Empty,
		BranchCountry = string.Empty,
		BranchZipCode = string.Empty
	};
}

public struct UserInformationDB
{
	public AgentInformation AgentInformation { get; set; }
	public BranchInformation BranchInformation { get; set; }    
	public string UserLicense { get; set; }
	public string UserTelephone { get; set; }
	public int User { get; set; }
	public int RetroactivePolicyDays { get; set; }
	public string UserName { get; set; }
	public string BranchUserPhone { get; set; }
	public static readonly UserInformationDB EmptyUserInformation = new UserInformationDB {
		AgentInformation = AgentInformation.EmptyAgentInformation,
		BranchInformation = BranchInformation.EmptyBranchInformation,
		UserLicense = string.Empty,
		UserTelephone = string.Empty,
		User = 0,
		UserName = string.Empty,
		BranchUserPhone = string.Empty,
	};
}

public class CountryData
{
	public string countryName { get; set; }
	public int countryID { get; set; }
}

public class LocationQuoteData
{
	public string TheftZone { get; set; }
	public int QuakeZone { get; set; }
	public int HMPZone { get; set; }
	public decimal TaxPercentage { get; set; }
	public string ZipCode { get; set; }
	public int HouseEstate { get; set; }
}

/// <summary>
/// Singleton Gateway with the admin application
/// </summary>
public class AdminGateway
{
	private SqlConnection GetConnection(int connIndex)
	{
		var conn = new SqlConnection(new ConexionToCS().GetConnectionString(connIndex));
		conn.Open();
		return conn;
	}
	private static AdminGateway m_gateway = new AdminGateway();

	private AdminGateway()
	{
		
	}

	/*
	public bool CanEditAgentInformation(string policyno)
	{
		return true;
	}
	*/
	public LocationQuoteData GetLocationQuoteData(int houseEstate)
	{
		using (var conn = GetConnection(ApplicationConstants.ADMINDB))
		{
			var command = new SqlCommand(
				"select h.zonaTEV, h.codigoPostal, c.taxPercentage, c.zonaFHM, s.zonaRobo from " +
				"houseEstate h join city c on h.city = c.city join state s on c.state = s.id " +
				"where houseEstate = @he",
				conn
			);
			command.Parameters.Add(new SqlParameter("@he", houseEstate));
			using (var reader = command.ExecuteReader())
			{
            return !reader.Read()
               ? null
               : new LocationQuoteData
            {
               TaxPercentage = decimal.Parse(reader["taxPercentage"].ToString()),
               HMPZone = int.Parse(reader["zonaFHM"].ToString()),
               TheftZone = reader["zonaRobo"].ToString(),
               QuakeZone = int.Parse(reader["zonaTEV"].ToString()),
               ZipCode = reader["codigoPostal"].ToString(),
               HouseEstate = houseEstate
            };
         }
      }
	}

	public IEnumerable<CountryData> GetCountries()
	{
		List<CountryData> countries = new List<CountryData>();
		using (var conn = GetConnection(ApplicationConstants.ADMINDB))
		{
			var command = new SqlCommand(
				"SELECT country, description FROM country WHERE country NOT IN (1,2) ORDER BY country",
				conn
			);
			using (var reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					countries.Add(
						new CountryData
						{
							countryID = int.Parse(reader["country"].ToString()),
							countryName = reader["description"].ToString()
						}
					);
				}
			}
		}
		return countries;
	}

	public bool CanEditAgentInformation(int agent, string policyno, string userType)
	{
		return (agent == ApplicationConstants.MEAGENT || userType == SecurityManager.ADMINISTRATOR) &&
			   csPolicy.IsPending(policyno.Trim().ToUpper(), ApplicationConstants.ADMINDB);
	}
	

	public bool AgentCanLoadPolicy(string policyNo, int agent, int userID, string userType)
	{
		bool canUsePolicy = agent == ApplicationConstants.MEAGENT || userType == SecurityManager.ADMINISTRATOR || userID == ApplicationConstants.PTI_USER;
		if (!canUsePolicy) {
			HouseInsuranceDataContext db = new HouseInsuranceDataContext();
			var dataStep = db.policies.Where(m => m.policyNo == policyNo).Select(m => m.issuedBy);
			if (dataStep.Count() > 0) {
				int issuedBy = dataStep.Single();
				var users = GetListOfUsersFromAgent(agent, userID);
				canUsePolicy = users.Contains(issuedBy);
			}

		}
		return canUsePolicy;
	}

	public string ListAllUsersFromAgent(int agentID, int user)
	{
		DataTable dt = csAgent.ListAllUsers(agentID, ApplicationConstants.ADMINDB).Tables[0];
		string agentes = string.Empty;
		for (int i = 0; i < dt.Rows.Count; i++) {
			agentes += dt.Rows[i]["cveUsuario"].ToString() + ", ";
		}


		//si el agente es M&E, incluir nuestro usuario
		if (agentID == ApplicationConstants.MEAGENT || user == ApplicationConstants.PTI_USER) {

			agentes += user + ", ";

		}

		agentes += "0";
		return agentes;      
	}

	public DataSet GetListOfBranches(int company, int agent)
	{
		return csBranch.List(company, ApplicationConstants.APPLICATION, agent, ApplicationConstants.ADMINDB);
	}

	public DataSet GetListOfAgents(int company)
	{
		return csAgent.List(ApplicationConstants.ADMINDB, company, ApplicationConstants.APPLICATION);
	}

	public DataSet GetListOfAgentsCanChangePolicyFee(int company)
   {
      dbConexion dbConexion = new dbConexion(ApplicationConstants.ADMINDB);
         return dbConexion.ExecuteDS("spLst_AgentByCoProduct_ChangePolicyFee " + company + ", " + ApplicationConstants.APPLICATION);
	}

	public DataSet GetListOfBranchUsers(int branch)
	{
		return csBranchUser.ListByBranch(branch, ApplicationConstants.ADMINDB);
	}

	public DataSet GetListOfBranchUsersFromProduct(int branch, int product, int company)
	{
		return csBranchUser.ListByBranchProduct(branch, product, ApplicationConstants.ADMINDB, company);
	}

	public bool IsUserAuthorizedInProduct(int user, int product)
	{
		dbConexion conexion = new dbConexion(ApplicationConstants.ADMINDB);
		try {
			return conexion.ExecuteDS("spGet_usu " + user + ", " + product).Tables[0].Rows.Count > 0;
		} catch {
			return false;
		}
	}

	public void ChangeAgent(string policyno, int newUser, int responsibleUser, string notes)
	{
		csPolicy.ChangePolicy(
			policyno, 
			notes, 
			responsibleUser, 
			ApplicationConstants.APPLICATION, 
			ApplicationConstants.ADMINDB
		);
		csPolicy.ChangeAgent(
			policyno, 
			newUser.ToString(), 
			ApplicationConstants.ADMINDB
		);
	}

	private string GetCountryAbbreviationByID(int countryID)
	{
		using (var conn = GetConnection(ApplicationConstants.ADMINDB))
		{
			var command = new SqlCommand(
				"SELECT abbreviation " +
				"FROM country " +
				"WHERE country = @ID",
				conn
			);
			command.Parameters.Add(new SqlParameter("@ID", countryID));
			using (var reader = command.ExecuteReader())
			{
            return !reader.Read() ? null : reader["abbreviation"].ToString();
         }
      }
	}

	public void ChangeCountry(string policyno, bool inMexico)
	{
		csPolicy p = new csPolicy(policyno, ApplicationConstants.ADMINDB);
		p.insuredCountry = inMexico ? "MX" : "USA";
		p.Save();
	}

	public void ChangeCountry(string policyno, int countryID)
	{
		string country = GetCountryAbbreviationByID(countryID);
		csPolicy p = new csPolicy(policyno, ApplicationConstants.ADMINDB);
		p.insuredCountry = country;
		p.Save();
	}

	public IEnumerable<int> GetListOfUsersFromAgent(int agent, int user)
	{
		DataTable dt = csAgent.ListAllUsers(agent, ApplicationConstants.ADMINDB).Tables[0];
		List<int> usuarios = new List<int>();
		for (int i = 0; i < dt.Rows.Count; i++) {
			usuarios.Add((int)dt.Rows[i]["cveUsuario"]);
		}

		//si el agente es M&E, incluir nuestro usuario
		if (agent == ApplicationConstants.MEAGENT || user == ApplicationConstants.PTI_USER) {
			usuarios.Add(user);
		}

		usuarios.Add(0);
		return usuarios;
	}

	public bool IsTestUser()
	{
		UserInformation user = (UserInformation)HttpContext.Current.User.Identity;
      return !user.IsAuthenticated ? throw new System.Security.SecurityException("User is not authenticated") : user.IsTestUser;
   }

   public bool MustPayWithCC(int userID)
	{
		Usuario usuario = new Usuario(
			userID,
			ApplicationConstants.ADMINDB,
			ApplicationConstants.APPLICATION
		);
		csBranchUser bUser = new csBranchUser(usuario.relation, ApplicationConstants.ADMINDB);
		csBranch branch = new csBranch(bUser.branch, ApplicationConstants.ADMINDB);
		return branch.paymentType == ApplicationConstants.paymentType;
	}

	private BranchInformation GetBranchInformation(csBranch branch)
	{
		return new BranchInformation {
			Branch = branch.id,
			BranchAddress = branch.address,
			BranchCityState = branch.city + ", " + branch.state,
			BranchCode = branch.code,
			BranchCountry = branch.country,
			BranchZipCode = branch.zipCode
		};
	}

	public BranchInformation GetBranchInformation(int branchID)
	{
		csBranch branch = new csBranch(branchID, ApplicationConstants.ADMINDB);
		return GetBranchInformation(branch);
	}


	public UserInformationDB GetUserInformation(int userID)
	{
		Usuario usuario = new Usuario(userID, ApplicationConstants.ADMINDB, ApplicationConstants.APPLICATION);
		csBranchUser brUser = new csBranchUser(usuario.relation, ApplicationConstants.ADMINDB);
		csBranchDetail bd = new csBranchDetail(
			brUser.branch, 
			ApplicationConstants.COMPANY, 
			ApplicationConstants.APPLICATION, 
			ApplicationConstants.ADMINDB
		);
		csBranch branch = new csBranch(brUser.branch, ApplicationConstants.ADMINDB);
		csAgent agent = new csAgent(branch.agent, ApplicationConstants.ADMINDB);
		
		return new UserInformationDB {
			AgentInformation = new AgentInformation {
				Agent = agent.Key,
				AgentName = agent.Name
			},
			User = usuario.ID,
			RetroactivePolicyDays = bd.retroactiveDaysPolicy,
			UserName = usuario.name,
			BranchInformation = GetBranchInformation(branch),
			UserLicense = brUser.license,
			UserTelephone = brUser.phoneComplete,
		};
	}


	public static AdminGateway Gateway
	{
		get
		{
			return m_gateway;
		}
	}
}
