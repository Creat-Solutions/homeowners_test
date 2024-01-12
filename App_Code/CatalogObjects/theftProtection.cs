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
/// Summary description for theftProtection
/// </summary>
public partial class theftProtection
{
    private static string CacheEntry = "theftProtections";

    public static void InvalidateCache()
    {
        HttpContext context = HttpContext.Current;
        if (context.Cache[CacheEntry] != null)
            context.Cache.Remove(CacheEntry); // Invalidate cache
    }

    public static int Insert(theftProtection toInsert)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.theftProtections.InsertOnSubmit(toInsert);
        db.SubmitChanges();
        InvalidateCache();
        return toInsert.id;
    }

    public static void Delete(theftProtection toDelete)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.theftProtections.Attach(toDelete);
        db.theftProtections.DeleteOnSubmit(toDelete);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static void Update(theftProtection oldValue, theftProtection updatedMaterial)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();

        db.theftProtections.Attach(updatedMaterial, oldValue);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static IEnumerable<theftProtection> Select()
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        return db.theftProtections;
    }

    public static IEnumerable<theftProtection> SelectCached()
    {
        HttpContext context = HttpContext.Current;
        List<theftProtection> theftProtections = (List<theftProtection>)context.Cache[CacheEntry];
        if (theftProtections == null) {
            theftProtections = Select().ToList();
            context.Cache[CacheEntry] = theftProtections;
        }
        return theftProtections;
    }

}
