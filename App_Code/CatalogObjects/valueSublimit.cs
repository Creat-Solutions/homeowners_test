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
/// Summary description for valueSublimit
/// </summary>
public partial class valueSublimit
{
    private static string CacheEntry = "valueSublimits";
    

    public static void InvalidateCache()
    {
        HttpContext context = HttpContext.Current;
        if (context.Cache[CacheEntry] != null)
            context.Cache.Remove(CacheEntry); // Invalidate cache
    }
    /*
    public static int Insert(valueSublimit toInsert)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.valueSublimits.InsertOnSubmit(toInsert);
        db.SubmitChanges();
        InvalidateCache();
        return toInsert.id;
    }
    
    public static void Delete(valueSublimit toDelete)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.valueSublimits.Attach(toDelete);
        db.valueSublimits.DeleteOnSubmit(toDelete);
        db.SubmitChanges();
        InvalidateCache();
    }
    */
    public static void Update(valueSublimit oldValue, valueSublimit updatedMaterial)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();

        db.valueSublimits.Attach(updatedMaterial, oldValue);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static IEnumerable<valueSublimit> Select()
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        return db.valueSublimits;
    }

    public static IEnumerable<valueSublimit> Select(int company)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        return db.valueSublimits.Where(m => m.company == company);
    }

    public static IEnumerable<valueSublimit> SelectCached()
    {
        HttpContext context = HttpContext.Current;
        List<valueSublimit> valueSublimits = (List<valueSublimit>)context.Cache[CacheEntry];
        if (valueSublimits == null) {
            valueSublimits = Select().ToList();
            context.Cache[CacheEntry] = valueSublimits;
        }
        return valueSublimits;
    }

}
