using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ME.Admon;
using Telepro.Conexion;

public class CityManager
{
    public static IEnumerable<CityInternal> GetRange(int startRowIndex, int maximumRows, string search, int state)
    {
        IEnumerable<City> cities;

        if (string.IsNullOrEmpty(search))
        {
            cities = State.GetCities(state).Skip(startRowIndex).Take(maximumRows);

            return returnCities(cities);
            //return State.GetCities(state).Skip(startRowIndex).Take(maximumRows);
        }

        cities = State.GetCities(state).Where(m => m.Name.ToUpper().Trim().Contains(search.ToUpper().Trim())).Skip(startRowIndex).Take(maximumRows);

        return returnCities(cities);
        //return State.GetCities(state).Where(m => m.Name.Contains(search)).Skip(startRowIndex).Take(maximumRows);
    }

    private static IEnumerable<CityInternal> returnCities(IEnumerable<City> cities)
    {
        return cities.Select(c =>
        {
            return new CityInternal
            {
                ID = c.ID,
                Name = c.Name,
                Code = c.Code,
                HMP = c.HMP,
                DeductibleQuake = c.DeductibleQuake,
                CoinsuranceQuake = c.CoinsuranceQuake,
                DeductibleHMP = c.DeductibleHMP,
                CoinsuranceHMP = c.CoinsuranceHMP,
                TaxPercentage = c.TaxPercentage,
                IDState = c.State.ID,
                NameState = c.State.Name,
                State = c.State
            };
        });
    }

    public static IEnumerable<City> Select()
    {
        return City.Get();
    }

    public static int GetTotal(string search, int state)
    {
        List<SearchParameter> conditions = new List<SearchParameter>();
        if (!string.IsNullOrEmpty(search))
        {
            conditions.Add(new SearchParameter
            {
                Operator = SearchOperator.LIKE,
                Property = "Name",
                Value = search
            });
        }

        conditions.Add(new SearchParameter
        {
            Property = "state",
            Value = state
        });

        int total = (int)City.Count(conditions);
        return total;
    }

    //public static void Update(City oldValue, City updatedMaterial)
    //{
    //    updatedMaterial.Save();
    //}

    public static void Update(CityInternal oldValue, CityInternal c)
    {
        dbConexion con = new dbConexion(ApplicationConstants.ADMINDB);

        string sqlUpdate = @"UPDATE city SET name = '" + c.Name + "', clave = '" + c.Code + "', " +
            "zonaFHM = " + c.HMP + ", dedu_terrem = " + c.DeductibleQuake + ", coase_terrem = " + c.CoinsuranceQuake + ", " +
            "dedu_FHM = " + c.DeductibleHMP + ", coase_FHM = " + c.CoinsuranceHMP + ", taxPercentage = " + c.TaxPercentage + " " +
            "where city = " + c.ID;

        con.ExecuteDS(sqlUpdate);
    }

    //public static void Insert(City state)
    //{
    //    state.Save();
    //}

    public static void Insert(CityInternal s)
    {
        dbConexion con = new dbConexion(ApplicationConstants.ADMINDB);

        DataTable dt = con.ExecuteDS("select MAX(city) as ID from city").Tables[0];
        int ID = 0;
        int.TryParse(dt.Rows[0]["ID"].ToString(), out ID);

        string sqlInsert = @"INSERT INTO city (city, name, clave, state, zonaFHM, dedu_terrem, coase_terrem, dedu_FHM, coase_FHM, taxPercentage) VALUES (" +
                            (ID + 1) + ", '" + s.Name + "', '" + s.Code + "', " + s.State.ID + ", " + s.HMP + ", " + s.DeductibleQuake + ", " + s.CoinsuranceQuake + ", " +
                            s.DeductibleHMP + ", " + s.CoinsuranceHMP + ", " + s.TaxPercentage + ")";

        con.ExecuteDS(sqlInsert);
    }
}

public class CityInternal
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public int HMP { get; set; }
    public int? DeductibleQuake { get; set; }
    public int? CoinsuranceQuake { get; set; }
    public int? DeductibleHMP { get; set; }
    public int? CoinsuranceHMP { get; set; }
    public decimal TaxPercentage { get; set; }
    public int IDState { get; set; }
    public string NameState { get; set; }
    public ME.Admon.State State { get; set; }
}