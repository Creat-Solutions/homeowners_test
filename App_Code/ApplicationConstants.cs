using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Protec.Tools;

public class TotalStrategy
{
	public TotalStrategy()
	{
	}

	public decimal CalculateTotal(decimal netPremium, decimal taxP, decimal rxpfP, decimal fee)
	{
		//netpremium + FEE + CalculateTax(netpremium)
		return ((netPremium * (1 + rxpfP)) + fee) * (1 + taxP);
	}

	public decimal CalculateTax(decimal netPremium, decimal taxP, decimal rxpfP, decimal fee)
	{
		return ((netPremium * (1 + rxpfP)) + fee) * taxP;
	}

	public decimal CalculateTotalWithData(decimal netPremium, decimal taxTotal, decimal rxpf, decimal fee)
	{
		return netPremium + fee + rxpf + taxTotal;
	}
}


public class ApplicationConstants
{
	public const int APPLICATION = 10;
	public const int HO_CAPTURA_DIRECTA = 23;
	public static string SALTENCRYPTION = "salt$1#2!/l2";
	public static string PASSENCRYPTION = "ccDat|n5e<u|2e|)";
	public const int WOOD_WALLTYPE = 5;
	public const int CONCRETE_WALLTYPE = 4;
	public const int CONDO_TYPE = 5;
	public const int WOOD_ROOFTYPE = 2;
	public const int CONCRETE_ROOFTYPE = 1;
	public const int ADMINDB = 3;
	public const int DB = 5;
	public const int COMPANY = 1;
	public const int RENTER = 1;
	public const int MORTGAGE_TYPE = 4;
	public const int OWNER_TYPE = 3;
	public const int LANDLORD_TYPE = 2;
	public const int PTI_USER = 1698; //usuario Paty Carrillo

	public const int DOLLARS_CURRENCY = 2;
	public const int PESOS_CURRENCY = 1;

	public const int RENTER_TYPE = 1;
	internal const int BUILDING_RISK = 1; //IS BUILDING OWNER
	internal const int CONTENTS_RISK = 2; //IS RENTING BUILDING
	public const int PERSONA_FISICA = 0;
	public const int PERSONA_MORAL = 1;
	public const int DEFAULT_PROPERTY_TYPE = 6; // house type
	public const int WEEKEND_USE = 3;
	public const int VACATION_USE = 2;


	public const string MAPFREMENAME = "Macafee and Edwards, Inc.";
	public const int MAPFREMEAGENT = 90297; //Agente M&E = 90297
	public const int MAPFREMEAGENTTEST = 52530; // Agente M&E de pruebas = 52530
	public const int COD_RAMO = 310;
	public const int MEAGENT = 1;
	public const int WEST_COAST_AGENT = 1263;
	public const int MAPFRENAMEMAXLENGTH = 30;
	public const decimal COMISSION_PERCENTAGE = 0.2300M;
	public const string DiarioOficialFederacion_API = "https://sidofqa.segob.gob.mx/dof/sidof/indicadores/";
	private static TotalStrategy m_strategy;

	//public static decimal FEE = 30M;

	public const string view_SICAS = "vw_WSSICAS_Homeowners_TEST";
	public const string paymentType = "1"; //1 CC, 2 EFT, 3 CHK, 4 WT

	public static string[] GetDebugEmails()
	{
		if (string.IsNullOrEmpty(ConfigurationSettings.AppSettings["MailDebug"]))
		{
			return new string[] { };
		}

		string[] emails = ConfigurationSettings.AppSettings["MailDebug"].Trim().Split(',');
		for (int i = 0; i < emails.Length; i++)
		{
			emails[i] = emails[i].Trim();
		}

		return emails;
	}

	static ApplicationConstants()
	{
		m_strategy = new TotalStrategy();
	}

	public static decimal Fee(int currency, decimal fee)
	{
		return currency == DOLLARS_CURRENCY ? fee : fee * 10;
	}


	public static decimal CalculateTotal(decimal netpremium, decimal taxP, decimal rxpfP, int currency, decimal fee)
	{
		return m_strategy.CalculateTotal(netpremium, taxP, rxpfP, Fee(currency, fee));
	}

	public static decimal CalculateTax(decimal netpremium, decimal taxP, decimal rxpfP, int currency, decimal fee)
	{
		return m_strategy.CalculateTax(netpremium, taxP, rxpfP, Fee(currency, fee));
	}

	public static decimal CalculateTotalWithData(decimal netPremium, decimal taxTotal, decimal rxpf, decimal fee)
	{
		return m_strategy.CalculateTotalWithData(netPremium, taxTotal, rxpf, fee);
	}

	public static rxpfSur GetRXPFSurcharge(HouseInsuranceDataContext db, int company, int currency)
	{
		if (currency != ApplicationConstants.PESOS_CURRENCY && currency != ApplicationConstants.DOLLARS_CURRENCY)
		{
			currency = ApplicationConstants.DOLLARS_CURRENCY;
		}

		return db.rxpfSurs.Single(m => m.month == DateTime.Now.Month && m.company == company && m.currency == currency);
	}

	private static SqlConnection GetConnection(int connIndex)
	{
		var connection = new SqlConnection(new ConexionToCS().GetConnectionString(connIndex));
		connection.Open();
		return connection;
	}

	public static SqlConnection GetHomeownerConnection()
	{
		return GetConnection(ApplicationConstants.DB);
	}

	public static SqlTransaction BeginTransaction(SqlConnection conn, string name)
	{
		return conn.BeginTransaction(name);
	}

	public static void RollbackTransaction(SqlTransaction conn, string name)
	{
		conn.Rollback(name);
	}

	public static void CommitTransaction(SqlTransaction conn)
	{
		conn.Commit();
	}
}

public class Indicador_DOF
{
    public int codIndicador { get; set; }
    public int codTipoIndicador { get; set; }
    public string fecha { get; set; }
    public string valor { get; set; }
}

public class Api_DOF_Response
{
    public int messageCode { get; set; }
    public string response { get; set; }
    public List<Indicador_DOF> ListaIndicadores { get; set; }
    public int TotalIndicadores { get; set; }
}