using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ME.Admon;
using Telepro.Conexion;

public class StateManager
{
    public static IEnumerable<StateInternal> SelectByCountry(int country)
    {
        var states = Country.GetStates(country);
        return returnStates(states);
    }

    public static IEnumerable<StateInternal> Select()
    {
        var states = State.Get(null).Where(s => s.Country.ID == 2).OrderBy(x => x.MapfreCode);

        return returnStates(states);
        //return states.ToList();
    }

    private static IEnumerable<StateInternal> returnStates(IEnumerable<State> states)
    {
        return states.Select(s =>
        {
            return new StateInternal
            {
                ID = s.ID,
                Name = s.Name,
                Abbreviation = s.Abbreviation,
                ZurichCode = s.ZurichCode,
                TheftZone = s.TheftZone,
                MapfreCode = s.MapfreCode,
                DaylightSavings = s.DaylightSavings,
                Country = s.Country,
                CountryID = s.Country.ID,
                CountryName = s.Country.Name
            };
        });
    }

    public static void Update(StateInternal oldValue, StateInternal c)
    {
        dbConexion con = new dbConexion(ApplicationConstants.ADMINDB);

        string sqlUpdate = @"UPDATE state SET description = '" + c.Name + "', abreviation = '" + c.Abbreviation + "', " + "zurichCode = '" + c.ZurichCode + "', " +
            "mapfreCode = " + c.MapfreCode + ", daylightSavings = " + c.DaylightSavings + ", " +
            "zonaRobo = '" + c.TheftZone + "' where id = " + c.ID;

        con.ExecuteDS(sqlUpdate);

    }

    public static void Insert(StateInternal s)
    {
        dbConexion con = new dbConexion(ApplicationConstants.ADMINDB);

        string sqlInsert = @"INSERT INTO state (description, abreviation, zurichCode, idcountry, mapfreCode, daylightSavings, zonaRobo) VALUES ('" +
                            s.Name + "', '" + s.Abbreviation + "', '" + s.ZurichCode + "', " + s.Country.ID + ", " + s.MapfreCode + ", " + s.DaylightSavings + ", " + s.TheftZone + ")";

        con.ExecuteDS(sqlInsert);
    }

    //public static void Update(State oldValue, State updatedMaterial)
    //{
    //    updatedMaterial.Save();
    //}

    //public static void Insert(State state)
    //{
    //    state.Save();
    //}
}

public class StateInternal
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Abbreviation { get; set; }
    public string ZurichCode { get; set; }
    public string TheftZone { get; set; }
    public int MapfreCode { get; set; }
    public int? DaylightSavings { get; set; }
    public ME.Admon.Country Country { get; set; }
    public int CountryID { get; set; }
    public string CountryName { get; set; }
}