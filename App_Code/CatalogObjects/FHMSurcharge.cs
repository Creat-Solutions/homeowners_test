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


public class FHMSurcharge
{
	public static List<spList_V1_FHMSurResult> Select(int company)
    {
        if (company < 1)
            return null;
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        return db.spList_V1_FHMSur(company).ToList();
    }

    public static void Update(int company, int FHM, decimal cInterior, decimal cBeach, decimal Rem, decimal GE, decimal dedInterior, decimal dedBeach, decimal coAseguro )
    {
        if (company < 1 || FHM < 1)
            return;
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.spUpd_V1_FHMSur(company, FHM, cInterior, cBeach, Rem, GE, dedInterior, dedBeach, coAseguro);
    }
}
