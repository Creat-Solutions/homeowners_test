using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: If you change the interface name "IHomeServices" here, you must also update the reference to "IHomeServices" in Web.config.
[ServiceContract(Namespace = "")]
public interface IHomeServices
{
    [OperationContract]
    LocationData GetLocations(string zipCode, int? state, int? city);

    [OperationContract]
    IEnumerable<int> GetQuotes(
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

    [OperationContract]
    int GetQuotePerCompany(
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