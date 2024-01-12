using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ME.Admon;


public class CountryManager
{
    public static List<Country> Select()
    {
        return Country.Get(null).Where(c => c.ID.Equals(1) || c.ID.Equals(2)).ToList();
    }

    public static void Update(Country oldValue, Country updatedMaterial)
    {
        updatedMaterial.Save();
    }

    public static void Insert(Country state)
    {
        state.Save();
    }
}
