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
/// Summary description for adjoiningEstate
/// </summary>
public partial class adjoiningEstate
{
    private static string CacheEntry = "adjoiningEstate";

    public static void InvalidateCache()
    {
        HttpContext context = HttpContext.Current;
        if (context.Cache[CacheEntry] != null)
            context.Cache.Remove(CacheEntry); // Invalidate cache
    }

    public static int Insert(adjoiningEstate toInsert)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.adjoiningEstates.InsertOnSubmit(toInsert);
        db.SubmitChanges();
        InvalidateCache();
        return toInsert.id;
    }

    public static void Delete(adjoiningEstate toDelete)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.adjoiningEstates.Attach(toDelete);
        db.adjoiningEstates.DeleteOnSubmit(toDelete);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static void Update(adjoiningEstate oldValue, adjoiningEstate updatedMaterial)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();

        db.adjoiningEstates.Attach(updatedMaterial, oldValue);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static IEnumerable<adjoiningEstate> Select()
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        return db.adjoiningEstates;
    }

    public static IEnumerable<adjoiningEstate> SelectCached()
    {
        HttpContext context = HttpContext.Current;
        List<adjoiningEstate> adjoiningEstate = (List<adjoiningEstate>)context.Cache[CacheEntry];
        if (adjoiningEstate == null) {
            adjoiningEstate = Select().ToList();
            context.Cache[CacheEntry] = adjoiningEstate;
        }
        return adjoiningEstate;
    }

}
