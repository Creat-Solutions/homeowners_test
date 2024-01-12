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
/// Summary description for city
/// </summary>
public partial class rxpfSur
{
    private const string CacheEntry = "rxpfSur";

    public static void InvalidateCache()
    {
        HttpContext context = HttpContext.Current;
        if (context.Cache[CacheEntry] != null)
            context.Cache.Remove(CacheEntry);
    }

    public static void Insert(rxpfSur toInsert)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.rxpfSurs.InsertOnSubmit(toInsert);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static void Delete(rxpfSur toDelete)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.rxpfSurs.Attach(toDelete);
        db.rxpfSurs.DeleteOnSubmit(toDelete);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static void Update(rxpfSur oldValue, rxpfSur updatedMaterial)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();

        db.rxpfSurs.Attach(updatedMaterial, oldValue);
        db.SubmitChanges();
        InvalidateCache();
    }

    public static IEnumerable<rxpfSur> Select()
    {
        HttpContext context = HttpContext.Current;
        if (context.Cache[CacheEntry] == null) {
            HouseInsuranceDataContext db = new HouseInsuranceDataContext();
            context.Cache[CacheEntry] = db.rxpfSurs.ToList();
        }
        return (IEnumerable<rxpfSur>)context.Cache[CacheEntry];
    }

    public static IEnumerable<rxpfSur> SelectWithCompanyAndCurrency(int company, int currency)
    {
        return Select().Where(m => m.company == company && m.currency == currency);
    }
}
