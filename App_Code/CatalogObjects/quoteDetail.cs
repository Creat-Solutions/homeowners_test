using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for quoteDetail
/// </summary>
public partial class quoteDetail
{
    public IEnumerable<quoteDetail> SelectByQuote(int quote)
    {
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        return db.quoteDetails.Where(m => m.quote == quote);
    }
}
