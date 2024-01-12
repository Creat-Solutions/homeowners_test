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
/// Summary description for clientType
/// </summary>
public partial class clientType
{
    private static string CacheEntry = "clientTypes";
    private static int MORTGAGE_ID = 4;

    public static void InvalidateCache()
    {
        HttpContext context = HttpContext.Current;
        if (context.Cache[CacheEntry] != null)
            context.Cache.Remove(CacheEntry); // Invalidate cache
    }

    public static int Insert(clientType toInsert)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.clientTypes.InsertOnSubmit(toInsert);
        db.SubmitChanges();
        InvalidateCache();
        return toInsert.type;
    }

    public static void Delete(clientType toDelete)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.clientTypes.Attach(toDelete);
        db.clientTypes.DeleteOnSubmit(toDelete);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static void Update(clientType oldValue, clientType updatedMaterial)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();

        db.clientTypes.Attach(updatedMaterial, oldValue);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static IEnumerable<clientType> Select()
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        return db.clientTypes;
    }

    public static IEnumerable<clientType> SelectCached()
    {
        HttpContext context = HttpContext.Current;
        List<clientType> clientTypes = (List<clientType>)context.Cache[CacheEntry];
        if (clientTypes == null) {
            clientTypes = Select().ToList();
            context.Cache[CacheEntry] = clientTypes;
        }
        return clientTypes;
    }

    public static IEnumerable<clientType> SelectWithoutMortgage()
    {
        HttpSessionState session = HttpContext.Current.Session;
        List<clientType> copy = new List<clientType>();
        var cached = SelectCached();
        foreach (var c in cached)
            if (c.type != MORTGAGE_ID)
            {
                copy.Add(new clientType
                {
                    type = c.type,
                    description = (session["CULTURE"] != null && (string)session["CULTURE"] == "es-MX") ? c.descriptionEs : c.description,
                    descriptionEs = string.Empty
                });
            }
        return copy;
    }

}
