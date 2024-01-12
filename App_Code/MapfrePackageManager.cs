using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MapfrePackageManager
/// </summary>
class MapfreFactorPackage
    {
        public int ID { get; set; }
        public int WallType { get; set; }
        public int RoofType { get; set; }
        public int UseOfProperty { get; set; }
        public int Stories { get; set; }
        public int Underground { get; set; }
        public bool AdjoiningEstates { get; set; }
        public int DistanceFromSea { get; set; }
        public bool Shutters { get; set; }
        public bool Guards24 { get; set; }
        public bool CentralAlarm { get; set; }
        public bool LocalAlarm { get; set; }
        public bool TVCircuit { get; set; }
        public bool TVGuard { get; set; }
        public bool FireAll { get; set; }
        public bool Quake { get; set; }
        public bool HMP { get; set; }
        public decimal ContentsFactor { get; set; }
        public decimal OutdoorFactor { get; set; }
        public decimal DebriFactor { get; set; }
        public decimal ExtraFactor { get; set; }
        public decimal LossOfRentsFactor { get; set; }
        public decimal MoneySecurityFactor { get; set; }
        public decimal AccidentalGlassBreakageFactor { get; set; }
        public decimal PersonalFactor { get; set; }
        public decimal ElectronicFactor { get; set; }
        public bool DomesticWorkersLiability { get; set; }
        public decimal BurglaryFactor { get; set; }
        public decimal BurglaryJewelryFactor { get; set; }
        public bool FamilyAssistance { get; set; }
        public decimal CSLFactor { get; set; }
        public bool TechSupport { get; set; }
        public bool SmokeAlarm { get; set; }
        public bool FireExtinguisher { get; set; }
        public bool FireHydrant { get; set; }
        public decimal AccidentalGlassContentsFactor { get; set; }
        public decimal OtherStructuresFactor { get; set; }
    }

class MapfrePackageManager
{
    private SqlConnection conn;
    private SqlTransaction trans;

    public MapfrePackageManager(SqlConnection conn, SqlTransaction trans)
    {
        this.conn = conn;
        this.trans = trans;
    }

    public MapfrePackageManager(SqlConnection conn)
        : this(conn, null)
    {
    }

    public MapfreFactorPackage GetPackage()
    {
        var command = new SqlCommand(
            "SELECT id, wallType, roofType, useOfProperty, stories," +
            "underground, adjoiningEstates, distanceFromSea, shutters, " +
            "guards24, centralAlarm, localAlarm, tvCircuit, tvGuard, " +
            "fireAll, quake, hmp, contentsFactor, outdoorFactor, debriFactor, " +
            "extraFactor, lossOfRentsFactor, moneySecurityFactor, electronicFactor, " +
            "accidentalGlassBreakageFactor, personalFactor, domesticWorkersLiability, " +
            "burglaryFactor, burglaryJewelryFactor, familyAssistance, " +
            "CSLFactor, techSupport, smokeAlarm, fireExtinguisher, fireHydrant, " +
            "accidentalGlassContentsFactor, otherStructuresFactor FROM homeowner.packageService " +
            "WHERE id = 4",
            conn,
            trans
        );
        MapfreFactorPackage mfp = new MapfreFactorPackage();
        using (var reader = command.ExecuteReader(CommandBehavior.SingleResult))
        {
            while (reader.Read())
            {
                mfp.ID = int.Parse(reader["id"].ToString());
                mfp.WallType = int.Parse(reader["wallType"].ToString());
                mfp.RoofType = int.Parse(reader["roofType"].ToString());
                mfp.UseOfProperty = int.Parse(reader["useOfProperty"].ToString());
                mfp.Stories = int.Parse(reader["stories"].ToString());
                mfp.Underground = int.Parse(reader["underground"].ToString());
                mfp.AdjoiningEstates = bool.Parse(reader["adjoiningEstates"].ToString());
                mfp.DistanceFromSea = int.Parse(reader["distanceFromSea"].ToString());
                mfp.Shutters = bool.Parse(reader["shutters"].ToString());
                mfp.Guards24 = bool.Parse(reader["guards24"].ToString());
                mfp.CentralAlarm = bool.Parse(reader["centralAlarm"].ToString());
                mfp.LocalAlarm = bool.Parse(reader["localAlarm"].ToString());
                mfp.TVCircuit = bool.Parse(reader["tvCircuit"].ToString());
                mfp.TVGuard = bool.Parse(reader["tvGuard"].ToString());
                mfp.FireAll = bool.Parse(reader["fireAll"].ToString());
                mfp.Quake = bool.Parse(reader["quake"].ToString());
                mfp.HMP = bool.Parse(reader["hmp"].ToString());
                mfp.ContentsFactor = decimal.Parse(reader["contentsFactor"].ToString());
                mfp.OutdoorFactor = decimal.Parse(reader["outdoorFactor"].ToString());
                mfp.DebriFactor = decimal.Parse(reader["debriFactor"].ToString());
                mfp.ExtraFactor = decimal.Parse(reader["extraFactor"].ToString());
                mfp.LossOfRentsFactor = decimal.Parse(reader["lossOfRentsFactor"].ToString());
                mfp.MoneySecurityFactor = decimal.Parse(reader["moneySecurityFactor"].ToString());
                mfp.AccidentalGlassBreakageFactor = decimal.Parse(reader["accidentalGlassBreakageFactor"].ToString());
                mfp.PersonalFactor = decimal.Parse(reader["personalFactor"].ToString());
                mfp.ElectronicFactor = decimal.Parse(reader["electronicFactor"].ToString());
                mfp.DomesticWorkersLiability = bool.Parse(reader["domesticWorkersLiability"].ToString());
                mfp.BurglaryFactor = decimal.Parse(reader["burglaryFactor"].ToString());
                mfp.BurglaryJewelryFactor = decimal.Parse(reader["burglaryJewelryFactor"].ToString());
                mfp.FamilyAssistance = bool.Parse(reader["familyAssistance"].ToString());
                mfp.CSLFactor = decimal.Parse(reader["CSLFactor"].ToString());
                mfp.TechSupport = bool.Parse(reader["techSupport"].ToString());
                mfp.SmokeAlarm = bool.Parse(reader["smokeAlarm"].ToString());
                mfp.FireExtinguisher = bool.Parse(reader["fireExtinguisher"].ToString());
                mfp.FireHydrant = bool.Parse(reader["fireHydrant"].ToString());
                mfp.AccidentalGlassContentsFactor = decimal.Parse(reader["accidentalGlassContentsFactor"].ToString());
                mfp.OtherStructuresFactor = decimal.Parse(reader["otherStructuresFactor"].ToString());
            }
        }
        return mfp;
    }
}
