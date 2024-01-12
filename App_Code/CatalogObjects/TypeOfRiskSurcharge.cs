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


public class TypeOfRiskSurcharge
{
	public static List<spList_V1_typeOfRiskSurResult> Select(int company)
    {
        if (company < 1)
            return null;
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        return db.spList_V1_typeOfRiskSur(company).ToList();
    }

    public static void Update(int company, int typeOfRisk, decimal surcharge,
                              decimal Additional, bool Coverage, decimal Minimum,
                              decimal Maximum, decimal Suggested)
    {
        if (company < 1 || typeOfRisk < 1)
            return;
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        db.spUpd_V1_typeOfRiskSur(company, typeOfRisk, surcharge, Additional, Coverage, Minimum, Maximum, Suggested);
    }
}
