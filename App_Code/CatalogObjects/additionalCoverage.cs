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
/// Summary description for additionalCoverage
/// </summary>
public partial class additionalCoverage
{
    private static string CacheEntry = "additionalCoverages";

    public static void InvalidateCache()
    {
        HttpContext context = HttpContext.Current;
        if (context.Cache[CacheEntry] != null)
            context.Cache.Remove(CacheEntry); // Invalidate cache
    }

    public static int Insert(additionalCoverage toInsert)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.additionalCoverages.InsertOnSubmit(toInsert);
        db.SubmitChanges();
        InvalidateCache();
        return toInsert.id;
    }

    public static void Update(additionalCoverage oldValue, additionalCoverage updatedMaterial)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();

        // con viewstate=false se obtiene el antiguo record con el siguiente query
        //additionalCoverage oldValue = db.additionalCoverages.Single(b => b.MaterialID == updatedMaterial.MaterialID);

        db.additionalCoverages.Attach(updatedMaterial, oldValue);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static IEnumerable<additionalCoverage> Select()
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        return db.additionalCoverages;
    }

    public static IEnumerable<additionalCoverage> SelectCached()
    {
        HttpContext context = HttpContext.Current;
        List<additionalCoverage> additionalCoverages = (List<additionalCoverage>)context.Cache[CacheEntry];
        if (additionalCoverages == null) {
            additionalCoverages = Select().ToList();
            context.Cache[CacheEntry] = additionalCoverages;
        }
        return additionalCoverages;
    }
}
