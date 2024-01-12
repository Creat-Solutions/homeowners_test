using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ME.Admon;


public class HouseEstateManager
{
    public static IEnumerable<HouseEstate> GetRange(int startRowIndex, int maximumRows, string search, int city)
    {
        if (string.IsNullOrEmpty(search)) {
            return City.GetHouseEstates(city).Skip(startRowIndex).Take(maximumRows);
        }
        return City.GetHouseEstates(city).Where(m => m.Name.Contains(search)).Skip(startRowIndex).Take(maximumRows);
    }

    public static long GetTotal(string search, int city)
    {
        List<SearchParameter> conditions = new List<SearchParameter>();
        if (!string.IsNullOrEmpty(search)) {
            conditions.Add(new SearchParameter {
                Operator = SearchOperator.LIKE,
                Property = "Name",
                Value = search
            });
        }

        conditions.Add(new SearchParameter {
            Property = "city",
            Value = city
        });

        return HouseEstate.Count(conditions);
    }

    public static void Update(HouseEstate oldValue, HouseEstate updatedMaterial)
    {
        updatedMaterial.Save();
    }

    public static void Insert(HouseEstate state)
    {
        state.Save();
    }
}
