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
using System.Web.SessionState;

/// <summary>
/// Summary description for distanceFromSea
/// </summary>
public partial class distanceFromSea
{
    private static string CacheEntry = "distanceFromSeas";

    public static void InvalidateCache()
    {
        HttpContext context = HttpContext.Current;
        if (context.Cache[CacheEntry] != null)
            context.Cache.Remove(CacheEntry); // Invalidate cache
    }

    public static int Insert(distanceFromSea toInsert)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.distanceFromSeas.InsertOnSubmit(toInsert);
        db.SubmitChanges();
        InvalidateCache();
        return toInsert.distance;
    }

    public static void Update(distanceFromSea oldValue, distanceFromSea updatedMaterial)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();

        // con viewstate=false se obtiene el antiguo record con el siguiente query
        //distanceFromSea oldValue = db.distanceFromSeas.Single(b => b.MaterialID == updatedMaterial.MaterialID);

        db.distanceFromSeas.Attach(updatedMaterial, oldValue);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static IEnumerable<distanceFromSea> Select()
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        return db.distanceFromSeas;
    }

    public static IEnumerable<distanceFromSea> SelectCached()
    {
        HttpContext context = HttpContext.Current;
        List<distanceFromSea> distanceFromSeas = (List<distanceFromSea>)context.Cache[CacheEntry];
        if (distanceFromSeas == null) {
            distanceFromSeas = Select().ToList();
            context.Cache[CacheEntry] = distanceFromSeas;
        }
        return distanceFromSeas;
    }

    public static IEnumerable<distanceFromSea> SelectForRBL()
    {
        List<distanceFromSea> copy = new List<distanceFromSea>();
        var cached = SelectCached();
        foreach (var d in cached)
            copy.Add(new distanceFromSea {
                distance = d.distance,
                description = d.description + "<br /><span class=\"rowes\">" + d.descriptionEs + "</span>",
                descriptionEs = string.Empty
            });
        return copy;
    }

    public static IEnumerable<distanceFromSea> SelectForRBLByLanguage()
    {
        HttpSessionState session = HttpContext.Current.Session;
        List<distanceFromSea> copy = new List<distanceFromSea>();
        var cached = SelectCached();
        foreach (var d in cached)
            copy.Add(new distanceFromSea
            {
                distance = d.distance,
                description = (session["CULTURE"] != null && (string)session["CULTURE"] == "es-MX") ? d.descriptionEs : d.description,
                descriptionEs = string.Empty
            });
        return copy;
    }
}
