using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for QuotePackage
/// </summary>
public class QuotePackage
{
    
    public int QuoteID { get; set; }

    
    public decimal NetPremium { get; set; }

    
    public decimal TEVPremium { get; set; }

    
    public decimal HMPPremium { get; set; }

    
    public decimal Fee { get; set; }

    
    public decimal Tax { get; set; }

    
    public decimal TaxPercentage { get; set; }

    
    public decimal Total { get; set; }

    
    public int Package { get; set; }

    
    public string Description { get; set; }
}
