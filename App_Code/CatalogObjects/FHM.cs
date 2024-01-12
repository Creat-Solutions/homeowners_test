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
/// Summary description for FHM
/// </summary>
public partial class FHM
{
    private static string CacheEntry = "FHM";

    public static void InvalidateCache()
    {
        HttpContext context = HttpContext.Current;
        if (context.Cache[CacheEntry] != null)
            context.Cache.Remove(CacheEntry); // Invalidate cache
    }

    public static int Insert(FHM toInsert)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.FHMs.InsertOnSubmit(toInsert);
        db.SubmitChanges();
        InvalidateCache();
        return toInsert.id;
    }

    public static void Delete(FHM toDelete)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.FHMs.Attach(toDelete);
        db.FHMs.DeleteOnSubmit(toDelete);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static void Update(FHM oldValue, FHM updatedMaterial)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();

        db.FHMs.Attach(updatedMaterial, oldValue);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static IEnumerable<FHM> Select()
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        return db.FHMs;
    }

    public static IEnumerable<FHM> SelectCached()
    {
        HttpContext context = HttpContext.Current;
        List<FHM> FHM = (List<FHM>)context.Cache[CacheEntry];
        if (FHM == null)
        {
            FHM = Select().ToList();
            context.Cache[CacheEntry] = FHM;
        }
        return FHM;
    }

}
