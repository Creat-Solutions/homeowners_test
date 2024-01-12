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
/// Summary description for other
/// </summary>
public partial class other
{
    private static string CacheEntry = "others";

    public static void InvalidateCache()
    {
        HttpContext context = HttpContext.Current;
        if (context.Cache[CacheEntry] != null)
            context.Cache.Remove(CacheEntry); // Invalidate cache
    }

    public static int Insert(other toInsert)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        if (toInsert.code == null)
            toInsert.code = string.Empty; //empty is valid
        db.others.InsertOnSubmit(toInsert);
        db.SubmitChanges();
        InvalidateCache();
        return toInsert.id;
    }

    public static void Delete(other toDelete)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.others.Attach(toDelete);
        db.others.DeleteOnSubmit(toDelete);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static void Update(other oldValue, other updatedMaterial)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        if (updatedMaterial.code == null)
            updatedMaterial.code = string.Empty;
        db.others.Attach(updatedMaterial, oldValue);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static IEnumerable<other> Select()
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        return db.others;
    }

    public static IEnumerable<other> SelectCached()
    {
        HttpContext context = HttpContext.Current;
        List<other> others = (List<other>)context.Cache[CacheEntry];
        if (others == null) {
            others = Select().ToList();
            context.Cache[CacheEntry] = others;
        }
        return others;
    }

}
