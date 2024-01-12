using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for IQuoter
/// </summary>
public interface IQuoter
{
    int GetQuote(
        int personType,
        string name,
        string lastName,
        string email,
        string phone,
        decimal buildingAmount,
        decimal contentsAmount,
        int houseEstate,
        int typeOfClient,
        int typeOfProperty,
        int useOfProperty,
        int currency,
        int user
    );
}
