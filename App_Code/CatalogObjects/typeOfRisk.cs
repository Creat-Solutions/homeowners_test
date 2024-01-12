using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;

/// <summary>
/// Summary description for typeOfRisk
/// </summary>
public partial class typeOfRisk
{
    private static string CacheEntry = "typeOfRisk";

    public static void InvalidateCache()
    {
        HttpContext context = HttpContext.Current;
        if (context.Cache[CacheEntry] != null)
            context.Cache.Remove(CacheEntry); // Invalidate cache
    }

    public static int Insert(typeOfRisk toInsert)
    {
        
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.typeOfRisks.InsertOnSubmit(toInsert);
        db.SubmitChanges();
        InvalidateCache();
        return toInsert.type;
    }

    public static void Delete(typeOfRisk toDelete)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.typeOfRisks.Attach(toDelete);
        db.typeOfRisks.DeleteOnSubmit(toDelete);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static void Update(typeOfRisk oldValue, typeOfRisk updatedMaterial)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();

        // con viewstate=false se obtiene el antiguo record con el siguiente query
        //typeOfRisk oldValue = db.typeOfRisks.Single(b => b.MaterialID == updatedMaterial.MaterialID);

        db.typeOfRisks.Attach(updatedMaterial, oldValue);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static IEnumerable<typeOfRisk> Select()
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        return db.typeOfRisks;
    }

    public static IEnumerable<typeOfRisk> SelectCached()
    {
        HttpContext context = HttpContext.Current;
        List<typeOfRisk> typeOfRisks = (List<typeOfRisk>)context.Cache[CacheEntry];
        if (typeOfRisks == null) {
            typeOfRisks = Select().ToList();
            context.Cache[CacheEntry] = typeOfRisks;
        }
        return typeOfRisks;
    }
}
