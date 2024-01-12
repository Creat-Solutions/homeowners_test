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


public class TypeOfPropertySurcharge
{
	public static List<spList_V1_typeOfPropertySurResult> Select(int company)
    {
        if (company < 1)
            return null;
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        return db.spList_V1_typeOfPropertySur(company).ToList();
    }

    public static void Update(int company, int typeOfProperty, decimal surcharge)
    {
        if (company < 1 || typeOfProperty < 1)
            return;
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.spUpd_V1_typeOfPropertySur(company, typeOfProperty, surcharge);
    }
}
