using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ME.Admon;

/// <summary>
/// insuredPersonHouse
/// </summary>
public partial class insuredPersonHouse
{
    public string ZipCode
    {
        get
        {
            return HouseEstate.ZipCode;
        }
    }

    private HouseEstate colonia = null;
    private int lastHouseEstate = -1;

    public HouseEstate HouseEstate
    {
        get
        {
            if (lastHouseEstate == -1 || lastHouseEstate != houseEstate) {
                colonia = new HouseEstate(houseEstate);
                lastHouseEstate = houseEstate;
            }
            
            return colonia;
        }
    }
}
