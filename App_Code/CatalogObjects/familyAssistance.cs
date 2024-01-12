using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for familyAssistance
/// </summary>
public partial class familyAssistance
{
    private static string CacheEntry = "familyAssistance";

    public static void InvalidateCache()
    {
        HttpContext context = HttpContext.Current;
        if (context.Cache[CacheEntry] != null)
            context.Cache.Remove(CacheEntry); // Invalidate cache
    }

    public static int Insert(familyAssistance toInsert)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.familyAssistances.InsertOnSubmit(toInsert);
        db.SubmitChanges();
        InvalidateCache();
        return toInsert.id;
    }

    public static void Delete(familyAssistance toDelete)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.familyAssistances.Attach(toDelete);
        db.familyAssistances.DeleteOnSubmit(toDelete);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static void Update(familyAssistance oldValue, familyAssistance updatedMaterial)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();

        // con viewstate=false se obtiene el antiguo record con el siguiente query
        //wallType oldValue = db.wallTypes.Single(b => b.MaterialID == updatedMaterial.MaterialID);

        db.familyAssistances.Attach(updatedMaterial, oldValue);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static IEnumerable<familyAssistance> Select()
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        return db.familyAssistances;
    }

    public static IEnumerable<familyAssistance> SelectCached()
    {
        HttpContext context = HttpContext.Current;
        List<familyAssistance> familyAssistances = (List<familyAssistance>)context.Cache[CacheEntry];
        if (familyAssistances == null) {
            familyAssistances = Select().ToList();
            context.Cache[CacheEntry] = familyAssistances;
        }
        return familyAssistances;
    }
}
