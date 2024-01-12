using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for QuoteData
/// </summary>
public class QuoteData
{
    public QuotePackage Single { get; set; }

    public QuotePackage Biyearly { get; set; }

    public QuotePackage Quarterly { get; set; }

    public QuotePackage Monthly { get; set; }
}
