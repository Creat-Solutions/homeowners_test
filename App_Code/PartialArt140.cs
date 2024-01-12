using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PartialArt140
/// Class to me used to store partial information 
/// about article 140 before saving to database
/// </summary>
public class PartialArt140
{
    public int CountryID { get; set; }
    public string LegalRepresentative { get; set; }
    public string IDNumber { get; set; }
    public string IDType { get; set; }
    public string IncorporationNumber { get; set; }
    public string MercantileNumber { get; set; }
    public string FIEL { get; set; }
}
