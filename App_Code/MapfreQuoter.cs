using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Xml.Linq;
using Protec.Tools;
using Telepro.Conexion;

/// <summary>
/// Summary description for MapfreQuoter
/// </summary>

public class MapfreQuoter : IQuoter
{
    private SqlConnection GetConnection(int connIndex)
    {
        var connection = new SqlConnection(new ConexionToCS().GetConnectionString(connIndex));
        connection.Open();
        return connection;
    }

    private SqlConnection GetMapfreConnection()
    {
        return GetConnection(ApplicationConstants.DB);
    }

    private int CreateQuote(
        SqlConnection conn,
        SqlTransaction trans,
        int personType,
        string name,
        string lastName,
        string email,
        string phone,
        decimal buildingAmount,
        decimal contentsAmount,
        int currency,
        MapfreFactorPackage package,
        int typeOfClient,
        int typeOfProperty,
        int useOfProperty,
        LocationQuoteData locationData,
        int user
    )
    {
        var lastNames = lastName.Split('|');
        lastName = lastNames[0];
        string lastName2 = lastNames[1];

        // first create the quote (normally we would require the person's data)
        var command = new SqlCommand("spUpd_V2_quotePerson", conn, trans);
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@id", 0);
        command.Parameters.AddWithValue("@userid", user);
        command.Parameters.AddWithValue("@firstName", name);

        if (string.IsNullOrEmpty(lastName))
        {
            command.Parameters.AddWithValue("@lastName", DBNull.Value);
        }
        else
        {
            command.Parameters.AddWithValue("@lastName", lastName);
        }

        if (string.IsNullOrEmpty(lastName2))
        {
            command.Parameters.AddWithValue("@lastName2", DBNull.Value);
        }
        else
        {
            command.Parameters.AddWithValue("@lastName2", lastName2);
        }

        command.Parameters.AddWithValue("@street", DBNull.Value);
        command.Parameters.AddWithValue("@fax", DBNull.Value);
        if (string.IsNullOrEmpty(email))
        {
            command.Parameters.AddWithValue("@email", DBNull.Value);
        }
        else
        {
            command.Parameters.AddWithValue("@email", email);
        }
        if (string.IsNullOrEmpty(phone))
        {
            command.Parameters.AddWithValue("@phone", DBNull.Value);
        }
        else
        {
            command.Parameters.AddWithValue("@phone", phone);
        }
        command.Parameters.AddWithValue("@city", DBNull.Value);
        command.Parameters.AddWithValue("@state", DBNull.Value);
        command.Parameters.AddWithValue("@zip", DBNull.Value);
        command.Parameters.AddWithValue("@legalPerson", personType);
        command.Parameters.AddWithValue("@company", ApplicationConstants.COMPANY);
        command.Parameters.AddWithValue("@addInsured", DBNull.Value);
        command.Parameters.AddWithValue("@rfc", DBNull.Value);
        command.Parameters.AddWithValue("@stateText", DBNull.Value);
        command.Parameters.Add("@retVal", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

        command.ExecuteNonQuery();
        if (command.Parameters["@retVal"].Value is DBNull)
        {
            return 0;
        }

        int quoteNo = (int)command.Parameters["@retVal"].Value;

        // now fill known risk's data
        command = new SqlCommand("spUpd_V1_quotePersonHouse", conn, trans);
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@id", quoteNo);
        command.Parameters.AddWithValue("@street", "");
        command.Parameters.AddWithValue("@intNumber", "");
        command.Parameters.AddWithValue("@extNumber", "");
        command.Parameters.AddWithValue("@houseEstate", locationData.HouseEstate);
        command.Parameters.AddWithValue("@taxPercentage", locationData.TaxPercentage);
        command.Parameters.AddWithValue("@bankName", "");
        command.Parameters.AddWithValue("@loanNumber", "");
        command.Parameters.AddWithValue("@bankAddress", "");
        command.ExecuteNonQuery();

        // now fill the property additional information
        command = new SqlCommand("spUpd_v1_addPropInfo", conn, trans);
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@id", quoteNo);
        command.Parameters.AddWithValue("@roofType", package.RoofType);
        command.Parameters.AddWithValue("@wallType", package.WallType);
        command.Parameters.AddWithValue("typeOfClient", typeOfClient);
        command.Parameters.AddWithValue("@useOfProperty", useOfProperty);
        command.Parameters.AddWithValue("@typeOfProperty", typeOfProperty);
        command.Parameters.AddWithValue("@adjoiningEstates", package.AdjoiningEstates);
        command.Parameters.AddWithValue("@stories", package.Stories);
        command.Parameters.AddWithValue("@underground", package.Underground);
        command.ExecuteNonQuery();

        // security info
        command = new SqlCommand("spUpd_v1_addSecurityInfo", conn, trans);
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@id", quoteNo);
        command.Parameters.AddWithValue("@distanceFromSea", package.DistanceFromSea);
        command.Parameters.AddWithValue("@shutters", package.Shutters);
        command.Parameters.AddWithValue("@guards24", package.Guards24);
        command.Parameters.AddWithValue("@centralAlarm", package.CentralAlarm);
        command.Parameters.AddWithValue("@localAlarm", package.LocalAlarm);
        command.Parameters.AddWithValue("@tvCircuit", package.TVCircuit);
        command.Parameters.AddWithValue("@tvGuard", package.TVGuard);
        command.ExecuteNonQuery();

        // fill the quote limits
        command = new SqlCommand("spUpd_V1_quoteLimits", conn, trans);
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@quoteid", quoteNo);
        if (typeOfClient == ApplicationConstants.RENTER)
        {
            command.Parameters.AddWithValue("@buildingLimit", 0M);
            command.Parameters.AddWithValue("@contentsLimit", contentsAmount);
        }
        else
        {
            command.Parameters.AddWithValue("@buildingLimit", buildingAmount);
            command.Parameters.AddWithValue("@contentsLimit", contentsAmount);
        }
        decimal amount = buildingAmount >= contentsAmount ? buildingAmount : contentsAmount;
        command.Parameters.AddWithValue("@outdoorLimit", amount * package.OutdoorFactor / 100);
        command.Parameters.AddWithValue("@debriLimit", amount * package.DebriFactor / 100);
        command.Parameters.AddWithValue("@extraLimit", amount * package.ExtraFactor / 100);
        command.Parameters.AddWithValue(
            "@lossOfRentsLimit",
            amount * package.LossOfRentsFactor / 100
        );
        command.Parameters.AddWithValue(
            "@otherStructuresLimit",
            amount * package.OtherStructuresFactor / 100
        );
        command.Parameters.AddWithValue("@outdoorDescription", "");
        command.Parameters.AddWithValue("@otherStructuresDescription", "");
        command.Parameters.AddWithValue("@fireAll", package.FireAll);
        command.Parameters.AddWithValue("@quake", package.Quake);
        command.Parameters.AddWithValue("@HMP", package.HMP);
        command.Parameters.AddWithValue("@limitsEnabled", true);
        command.Parameters.AddWithValue("@currency", currency);
        command.ExecuteNonQuery();

        // update coverages
        command = new SqlCommand("spUpd_V1_quoteCoverages", conn, trans);
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.Add(new SqlParameter("@quoteid", quoteNo));
        command.Parameters.Add(new SqlParameter("@CGLLimit", amount * package.CSLFactor / 100));
        command.Parameters.Add(new SqlParameter("@domestic", package.DomesticWorkersLiability));
        command.Parameters.Add(
            new SqlParameter("@burglaryLimit", amount * package.BurglaryFactor / 100)
        );
        command.Parameters.Add(
            new SqlParameter("@burglaryJewelryLimit", amount * package.BurglaryJewelryFactor / 100)
        );
        command.Parameters.Add(
            new SqlParameter("@moneyLimit", amount * package.MoneySecurityFactor / 100)
        );
        command.Parameters.Add(
            new SqlParameter("@glassLimit", amount * package.AccidentalGlassBreakageFactor / 100)
        );
        command.Parameters.Add(
            new SqlParameter("@electronicLimit", amount * package.ElectronicFactor / 100)
        );
        command.Parameters.Add(
            new SqlParameter("@personalLimit", amount * package.PersonalFactor / 100)
        );
        command.Parameters.Add(new SqlParameter("@family", package.FamilyAssistance));
        command.ExecuteNonQuery();

        // finally make the quote
        command = new SqlCommand("homeowner.spUpd_quote", conn, trans);
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.Add(new SqlParameter("@quoteid", quoteNo));
        command.Parameters.Add(new SqlParameter("@zonaTEV", locationData.QuakeZone));
        command.Parameters.Add(new SqlParameter("@zonaFHM", locationData.HMPZone));
        command.Parameters.Add(new SqlParameter("@zonaRobo", locationData.TheftZone[0]));
        command.ExecuteNonQuery();

        ////add the default fee
        //command = new SqlCommand("UPDATE quote SET fee = @fee WHERE quote = @quoteid ", conn, trans);
        //command.CommandType = System.Data.CommandType.Text;
        //command.Parameters.Add(new SqlParameter("@fee", ApplicationConstants.FEE));
        //command.Parameters.Add(new SqlParameter("@quoteid", quoteNo));
        //command.ExecuteNonQuery();

        //EJDP -> se obtiene el fee de admonPoliza
        command = new SqlCommand("homeowner.spUpd_quoteFee", conn, trans);
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.Add(new SqlParameter("@quoteID", quoteNo));
        command.Parameters.Add(new SqlParameter("@company", ApplicationConstants.COMPANY));
        command.Parameters.Add(new SqlParameter("@product", ApplicationConstants.APPLICATION));
        command.ExecuteNonQuery();

        return quoteNo;
    }

    //private decimal GetFee(int currency)
    //{
    //    if (currency == ApplicationConstants.DOLLARS_CURRENCY)
    //    {
    //        return ApplicationConstants.FEE;
    //    }
    //    return ApplicationConstants.FEE * 10;
    //}

    private MapfreFactorPackage GetPackage(SqlConnection conn, SqlTransaction trans)
    {
        return new MapfrePackageManager(conn, trans).GetPackage();
    }

    private SqlTransaction BeginTransaction(SqlConnection conn, string name)
    {
        return conn.BeginTransaction(name);
    }

    private void RollbackTransaction(SqlTransaction conn, string name)
    {
        conn.Rollback(name);
    }

    private void CommitTransaction(SqlTransaction conn)
    {
        conn.Commit();
    }

    public int GetQuote(
        int personType,
        string name,
        string lastName,
        string email,
        string phone,
        decimal buildingAmount,
        decimal contentsAmount,
        int houseEstate,
        int typeOfClient,
        int typeOfProperty,
        int useOfProperty,
        int currency,
        int user
    )
    {
        var admin = AdminGateway.Gateway;
        var locationData = admin.GetLocationQuoteData(houseEstate);
        using (SqlConnection conn = GetMapfreConnection())
        {
            var trans = BeginTransaction(conn, "quoter");
            // we iterate and bring all the packages because when we try to iterate with
            // .Select() an exception would be thrown because we haven't finished our previous
            // DataRead command.
            var package = GetPackage(conn, trans);

            try
            {
                return CreateQuote(
                    conn,
                    trans,
                    personType,
                    name,
                    lastName,
                    email,
                    phone,
                    buildingAmount,
                    contentsAmount,
                    currency,
                    package,
                    typeOfClient,
                    typeOfProperty,
                    useOfProperty,
                    locationData,
                    user
                );
            }
            catch
            {
                RollbackTransaction(trans, "quoter");
                trans = null;
                return 0;
            }
            finally
            {
                if (trans != null)
                {
                    CommitTransaction(trans);
                    trans = null;
                }
            }
        }
    }

    public AmountLimits GetAmountLimits()
    {
        // : ApplicationConstants.CONTENTS_RISK;
        decimal buildingRiskMin = 0,
            buildingRiskMax = 0;
        decimal contentsRiskMin = 0,
            contentsRiskMax = 0;
        var db = new HouseInsuranceDataContext();

        var min = db.typeOfRiskSurs
            .Where(
                m =>
                    m.company == ApplicationConstants.COMPANY
                    && m.typeOfRisk == ApplicationConstants.BUILDING_RISK
            )
            .Select(m => m.Minimum);
        if (min.Count() > 0)
        {
            buildingRiskMin = min.Single();
        }
        var max = db.typeOfRiskSurs
            .Where(
                m =>
                    m.company == ApplicationConstants.COMPANY
                    && m.typeOfRisk == ApplicationConstants.BUILDING_RISK
            )
            .Select(m => m.Maximum);
        if (max.Count() > 0)
        {
            buildingRiskMax = max.Single();
        }

        min = null;
        max = null;

        min = db.typeOfRiskSurs
            .Where(
                m =>
                    m.company == ApplicationConstants.COMPANY
                    && m.typeOfRisk == ApplicationConstants.CONTENTS_RISK
            )
            .Select(m => m.Minimum);
        if (min.Count() > 0)
        {
            contentsRiskMin = min.Single();
        }
        max = db.typeOfRiskSurs
            .Where(
                m =>
                    m.company == ApplicationConstants.COMPANY
                    && m.typeOfRisk == ApplicationConstants.CONTENTS_RISK
            )
            .Select(m => m.Maximum);
        if (max.Count() > 0)
        {
            contentsRiskMax = max.Single();
        }

        return new AmountLimits
        {
            BuildingMinimum = buildingRiskMin,
            BuildingMaximum = buildingRiskMax,
            ContentsMinimum = contentsRiskMin,
            ContentsMaximum = contentsRiskMax
        };
    }
}
