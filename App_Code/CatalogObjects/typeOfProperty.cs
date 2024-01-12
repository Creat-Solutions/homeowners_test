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
/// Summary description for typeOfProperty
/// </summary>
public partial class typeOfProperty
{
    private static string CacheEntry = "typeOfProperty";

    public static void InvalidateCache()
    {
        HttpContext context = HttpContext.Current;
        if (context.Cache[CacheEntry] != null)
            context.Cache.Remove(CacheEntry); // Invalidate cache
    }

    public static int Insert(typeOfProperty toInsert)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.typeOfProperties.InsertOnSubmit(toInsert);
        db.SubmitChanges();
        InvalidateCache();
        return toInsert.type;
    }

    public static void Delete(typeOfProperty toDelete)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.typeOfProperties.Attach(toDelete);
        db.typeOfProperties.DeleteOnSubmit(toDelete);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static void Update(typeOfProperty oldValue, typeOfProperty updatedMaterial)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();

        // con viewstate=false se obtiene el antiguo record con el siguiente query
        //typeOfProperty oldValue = db.typeOfPropertys.Single(b => b.MaterialID == updatedMaterial.MaterialID);

        db.typeOfProperties.Attach(updatedMaterial, oldValue);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static IEnumerable<typeOfProperty> Select()
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        return from pType in db.typeOfProperties
               join pTypeSur in db.typeOfPropertySurs on pType.type equals pTypeSur.typeOfProperty
               where pTypeSur.company == ApplicationConstants.COMPANY
               select pType;
    }

    public static IEnumerable<typeOfProperty> SelectCached()
    {
        HttpContext context = HttpContext.Current;
        List<typeOfProperty> typeOfProperties = (List<typeOfProperty>)context.Cache[CacheEntry];
        if (typeOfProperties == null) {
            typeOfProperties = Select().ToList();
            context.Cache[CacheEntry] = typeOfProperties;
        }
        return typeOfProperties;
    }

    public static IEnumerable<typeOfProperty> SelectByLanguage()
    {
        HttpSessionState session = HttpContext.Current.Session;
        List<typeOfProperty> copy = new List<typeOfProperty>();
        var cached = SelectCached();
        foreach (var t in cached)
            copy.Add(new typeOfProperty
            {
                type = t.type,
                description = (session["CULTURE"] != null && (string)session["CULTURE"] == "es-MX") ? t.descriptionEs : t.description,
                descriptionEs = string.Empty
            });
        return copy;
    }
}
