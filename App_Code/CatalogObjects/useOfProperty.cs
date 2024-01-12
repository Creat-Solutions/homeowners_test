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
/// Summary description for useOfProperty
/// </summary>
public partial class useOfProperty
{
    private static int WEEKENDS_ID = 3;
    private static string CacheEntry = "useOfProperty";

    public static void InvalidateCache()
    {
        HttpContext context = HttpContext.Current;
        if (context.Cache[CacheEntry] != null)
            context.Cache.Remove(CacheEntry); // Invalidate cache
    }

    public static int Insert(useOfProperty toInsert)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.useOfProperties.InsertOnSubmit(toInsert);
        db.SubmitChanges();
        InvalidateCache();
        return toInsert.use;
    }

    public static void Delete(useOfProperty toDelete)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.useOfProperties.Attach(toDelete);
        db.useOfProperties.DeleteOnSubmit(toDelete);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static void Update(useOfProperty oldValue, useOfProperty updatedMaterial)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();

        // con viewstate=false se obtiene el antiguo record con el siguiente query
        //typeOfRisk oldValue = db.typeOfRisks.Single(b => b.MaterialID == updatedMaterial.MaterialID);

        db.useOfProperties.Attach(updatedMaterial, oldValue);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static IEnumerable<useOfProperty> Select()
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        return db.useOfProperties;
    }

    public static IEnumerable<useOfProperty> SelectCached()
    {
        HttpContext context = HttpContext.Current;
        List<useOfProperty> useOfProperties = (List<useOfProperty>)context.Cache[CacheEntry];
        if (useOfProperties == null) {
            useOfProperties = Select().ToList();
            context.Cache[CacheEntry] = useOfProperties;
        }
        return useOfProperties;
    }

    public static IEnumerable<useOfProperty> SelectWithoutWeekends()
    {
        HttpSessionState session = HttpContext.Current.Session;
        List<useOfProperty> copy = new List<useOfProperty>();
        var cached = SelectCached();
        foreach (var u in cached)
            if (u.use != WEEKENDS_ID)
            {
                copy.Add(new useOfProperty
                {
                    use = u.use,
                    description = (session["CULTURE"] != null && (string)session["CULTURE"] == "es-MX") ? u.descriptionEs : u.description,
                    descriptionEs = string.Empty
                });
            }
        return copy;
    }
}
