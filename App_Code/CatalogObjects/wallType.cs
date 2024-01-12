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
/// Summary description for wallType
/// </summary>
public partial class wallType
{
    private static string CacheEntry = "wallType";

    public static void InvalidateCache()
    {
        HttpContext context = HttpContext.Current;
        if (context.Cache[CacheEntry] != null)
            context.Cache.Remove(CacheEntry); // Invalidate cache
    }

    public static int Insert(wallType toInsert)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.wallTypes.InsertOnSubmit(toInsert);
        db.SubmitChanges();
        InvalidateCache();
        return toInsert.type;
    }

    public static void Delete(wallType toDelete)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.wallTypes.Attach(toDelete);
        db.wallTypes.DeleteOnSubmit(toDelete);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static void Update(wallType oldValue, wallType updatedMaterial)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();

        // con viewstate=false se obtiene el antiguo record con el siguiente query
        //wallType oldValue = db.wallTypes.Single(b => b.MaterialID == updatedMaterial.MaterialID);

        db.wallTypes.Attach(updatedMaterial, oldValue);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static IEnumerable<wallType> Select()
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        return from wType in db.wallTypes
               join wTypeSur in db.wallTypeSurs on wType.type equals wTypeSur.wallType
               where wTypeSur.company == ApplicationConstants.COMPANY
               select wType;
    }

    public static IEnumerable<wallType> SelectCached()
    {
        HttpContext context = HttpContext.Current;
        List<wallType> wallTypes = (List<wallType>)context.Cache[CacheEntry];
        if (wallTypes == null) {
            wallTypes = Select().ToList();
            context.Cache[CacheEntry] = wallTypes;
        }
        return wallTypes;
    }

    public static IEnumerable<wallType> SelectByLanguage()
    {
        HttpSessionState session = HttpContext.Current.Session;
        List<wallType> copy = new List<wallType>();
        var cached = SelectCached();
        foreach (var t in cached)
            copy.Add(new wallType
            {
                type = t.type,
                description = (session["CULTURE"] != null && (string)session["CULTURE"] == "es-MX") ? t.descriptionEs : t.description,
                descriptionEs = string.Empty
            });
        return copy;
    }
}

