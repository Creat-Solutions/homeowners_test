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
/// Summary description for windowProtection
/// </summary>
public partial class windowProtection
{
    private static string CacheEntry = "windowProtection";

    public static void InvalidateCache()
    {
        HttpContext context = HttpContext.Current;
        if (context.Cache[CacheEntry] != null)
            context.Cache.Remove(CacheEntry); // Invalidate cache
    }

    public static int Insert(windowProtection toInsert)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.windowProtections.InsertOnSubmit(toInsert);
        db.SubmitChanges();
        InvalidateCache();
        return toInsert.protection;
    }

    public static void Delete(windowProtection toDelete)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.windowProtections.Attach(toDelete);
        db.windowProtections.DeleteOnSubmit(toDelete);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static void Update(windowProtection oldValue, windowProtection updatedMaterial)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();

        // con viewstate=false se obtiene el antiguo record con el siguiente query
        //wallType oldValue = db.wallTypes.Single(b => b.MaterialID == updatedMaterial.MaterialID);

        db.windowProtections.Attach(updatedMaterial, oldValue);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static IEnumerable<windowProtection> Select()
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        return db.windowProtections;
    }

    public static IEnumerable<windowProtection> SelectCached()
    {
        HttpContext context = HttpContext.Current;
        List<windowProtection> windowProtections = (List<windowProtection>)context.Cache[CacheEntry];
        if (windowProtections == null) {
            windowProtections = Select().ToList();
            context.Cache[CacheEntry] = windowProtections;
        }
        return windowProtections;
    }
}
