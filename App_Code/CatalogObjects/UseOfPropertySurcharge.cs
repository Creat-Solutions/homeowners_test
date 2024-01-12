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


public class UseOfPropertySurcharge
{
	public static List<spList_V1_useOfPropertySurResult> Select(int company)
    {
        if (company < 1)
            return null;
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        return db.spList_V1_useOfPropertySur(company).ToList();
    }

    public static void Update(int company, int useOfProperty, decimal surcharge)
    {
        if (company < 1 || useOfProperty < 1)
            return;
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.spUpd_V1_useOfPropertySur(company, useOfProperty, surcharge);
    }
}
