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


public class TheftProtectionSurcharge
{
	public static List<spList_V1_theftProtectionSurResult> Select(int company)
    {
        if (company < 1)
            return null;
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        return db.spList_V1_theftProtectionSur(company).ToList();
    }

    public static void Update(int company, int theftProtection, bool code, decimal DIS)
    {
        if (company < 1 || theftProtection < 1)
            return;
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.spUpd_V1_theftProtectionSur(company, theftProtection, code, DIS);
    }
}
