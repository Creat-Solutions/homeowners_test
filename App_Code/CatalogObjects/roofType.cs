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
/// Summary description for roofType
/// </summary>
public partial class roofType
{
    private static string CacheEntry = "roofType";

    public static void InvalidateCache()
    {
        HttpContext context = HttpContext.Current;
        if (context.Cache[CacheEntry] != null)
            context.Cache.Remove(CacheEntry); // Invalidate cache
    }

    public static int Insert(roofType toInsert)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.roofTypes.InsertOnSubmit(toInsert);
        db.SubmitChanges();
        InvalidateCache();
        return toInsert.type;
    }

    public static void Delete(roofType toDelete)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.roofTypes.Attach(toDelete);
        db.roofTypes.DeleteOnSubmit(toDelete);
        db.SubmitChanges();
        InvalidateCache();
        
    }

    public static void Update(roofType oldValue, roofType updatedMaterial)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();

        // con viewstate=false se obtiene el antiguo record con el siguiente query
        //roofType oldValue = db.roofTypes.Single(b => b.MaterialID == updatedMaterial.MaterialID);

        db.roofTypes.Attach(updatedMaterial, oldValue);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static IEnumerable<roofType> Select()
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        return from rType in db.roofTypes
               join rTypeSur in db.roofTypeSurs on rType.type equals rTypeSur.roofType
               where rTypeSur.company == ApplicationConstants.COMPANY
               select rType;
    }

    public static IEnumerable<roofType> SelectCached()
    {
        HttpContext context = HttpContext.Current;
        List<roofType> roofTypes = (List<roofType>)context.Cache[CacheEntry];
        if (roofTypes == null) {
            roofTypes = Select().ToList();
            context.Cache[CacheEntry] = roofTypes;
        }
        return roofTypes;
    }

    public static IEnumerable<roofType> SelectByLanguage()
    {
        HttpSessionState session = HttpContext.Current.Session;
        List<roofType> copy = new List<roofType>();
        var cached = SelectCached();
        foreach (var t in cached)
            copy.Add(new roofType
            {
                type = t.type,
                description = (session["CULTURE"] != null && (string)session["CULTURE"] == "es-MX") ? t.descriptionEs : t.description,
                descriptionEs = string.Empty
            });
        return copy;
    }
}
