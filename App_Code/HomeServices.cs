using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
using ME.Admon;

[DataContract]
public class LocationDTO
{
    [DataMember]
    public int ID { get; set; }

    [DataMember]
    public string Name { get; set; }
}

[DataContract]
public class LocationData
{
    [DataMember]
    public IEnumerable<LocationDTO> states { get; set; }

    [DataMember]
    public IEnumerable<LocationDTO> cities { get; set; }

    [DataMember]
    public IEnumerable<LocationDTO> houseEstates { get; set; }
}


internal class CityComparer : IEqualityComparer<City>
{
    public bool Equals(City x, City y)
    {
        return x.ID == y.ID;
    }

    public int GetHashCode(City obj)
    {
        return obj.GetHashCode();
    }

}

internal class StateComparer : IEqualityComparer<State>
{

    public bool Equals(State x, State y)
    {
        return x.ID == y.ID;
    }

    public int GetHashCode(State obj)
    {
        return obj.GetHashCode();
    }

}

internal class HouseEstateComparer : IEqualityComparer<HouseEstate>
{

    public bool Equals(HouseEstate x, HouseEstate y)
    {
        return x.ID == y.ID;
    }

    public int GetHashCode(HouseEstate obj)
    {
        return obj.GetHashCode();
    }

}

internal class ZipCodeHandler
{
    private LocationData ErrorResponse()
    {
        return new LocationData
        {
            states = new List<LocationDTO>(),
            cities = new List<LocationDTO>(),
            houseEstates = new List<LocationDTO>()
        };
    }

    public LocationData HandleFilterRequest(string zipCode, int state, int city)
    {
        if ((!string.IsNullOrEmpty(zipCode) && zipCode.Length != 5) || state < 0 || city < 0)
        {
            return ErrorResponse();
        }

        if (string.IsNullOrEmpty(zipCode))
        {
            if (state > 0)
            {
                State loadedState;
                try
                {
                    loadedState = new State(state, ApplicationConstants.ADMINDB);
                }
                catch
                {
                    return ErrorResponse();
                }

                return new LocationData
                {
                    states = new LocationDTO[] {
                        new LocationDTO {
                            ID = state,
                            Name = loadedState.Name
                        }
                    },
                    cities = loadedState.Cities.Select(c => new LocationDTO
                    {
                        ID = c.ID,
                        Name = c.Name
                    }),
                    houseEstates = new LocationDTO[] { }
                };
            }
            else if (city > 0)
            {
                City loadedCity;
                try
                {
                    loadedCity = new City(city, ApplicationConstants.ADMINDB);
                }
                catch
                {
                    return ErrorResponse();
                }

                return new LocationData
                {
                    cities = new LocationDTO[] {
                        new LocationDTO {
                            ID = city,
                            Name = loadedCity.Name
                        }
                    },
                    houseEstates = loadedCity.HouseEstates.Select(h => new LocationDTO
                    {
                        ID = h.ID,
                        Name = h.Name
                    }),
                    states = new LocationDTO[] { }
                };
            }
            else
            {
                return new LocationData
                {
                    states = new LocationDTO[] { },
                    cities = new LocationDTO[] { },
                    houseEstates = new LocationDTO[] { }
                };
            }
        }
        else
        {
            var he = GetHouseEstateFromZipCode(zipCode ?? string.Empty);

            if (he.Count == 0)
            {
                if (state > 0)
                {
                    State loadedState;
                    try
                    {
                        loadedState = new State(state, ApplicationConstants.ADMINDB);
                    }
                    catch
                    {
                        return ErrorResponse();
                    }

                    return new LocationData
                    {
                        states = new LocationDTO[] {
                        new LocationDTO {
                            ID = state,
                            Name = loadedState.Name
                        }
                    },
                        cities = loadedState.Cities.Select(c => new LocationDTO
                        {
                            ID = c.ID,
                            Name = c.Name
                        }),
                        houseEstates = new LocationDTO[] { }
                    };
                }
                else if (city > 0)
                {
                    City loadedCity;
                    try
                    {
                        loadedCity = new City(city, ApplicationConstants.ADMINDB);
                    }
                    catch
                    {
                        return ErrorResponse();
                    }

                    return new LocationData
                    {
                        cities = new LocationDTO[] {
                        new LocationDTO {
                            ID = city,
                            Name = loadedCity.Name
                        }
                    },
                        houseEstates = loadedCity.HouseEstates.Select(h => new LocationDTO
                        {
                            ID = h.ID,
                            Name = h.Name
                        }),
                        states = new LocationDTO[] { }
                    };
                }
                else
                {
                    return new LocationData
                    {
                        states = new LocationDTO[] { },
                        cities = new LocationDTO[] { },
                        houseEstates = new LocationDTO[] { }
                    };
                }
            }
        }

        var het = GetHouseEstateFromZipCode(zipCode ?? string.Empty);

        if (het.Count == 0)
        {
            return ErrorResponse();
        }

        if (state == 0 && city == 0)
        {

            var cities = het.Select(m => m.City).ToUniqueList();
            var states = cities.Select(m => m.State).ToUniqueList();

            return new LocationData
            {
                cities = cities.Select(c => new LocationDTO
                {
                    ID = c.ID,
                    Name = c.Name
                }),
                states = states.Select(s => new LocationDTO
                {
                    ID = s.ID,
                    Name = s.Name
                }),
                houseEstates = het.Select(m => new LocationDTO
                {
                    ID = m.ID,
                    Name = m.Name
                })
            };
        }

        if (city != 0)
        {
            var transferHE = het
                .Where(m => m.City.ID == city)
                .Distinct(new HouseEstateComparer())
                .ToUniqueList();
            if (transferHE.Count == 0)
            {
                return ErrorResponse();
            }

            var theCity = transferHE[0].City;

            return new LocationData
            {
                cities = new LocationDTO[] { new LocationDTO {
                    ID = theCity.ID,
                    Name = theCity.Name
                }},
                states = new LocationDTO[] { new LocationDTO {
                    ID = theCity.State.ID,
                    Name = theCity.State.Name
                }},

                houseEstates = transferHE.Select(m => new LocationDTO
                {
                    ID = m.ID,
                    Name = m.Name
                })
            };
        }

        var transferHEState = het
            .Where(m => m.City.State.ID == city)
            .Distinct(new HouseEstateComparer())
            .ToUniqueList();
        var transferCities = transferHEState.Select(m => m.City).ToUniqueList();
        if (transferHEState.Count == 0)
        {
            return ErrorResponse();
        }

        var theState = transferHEState[0].City.State;

        return new LocationData
        {
            cities = transferCities.Select(m => new LocationDTO
            {
                ID = m.ID,
                Name = m.Name
            }),
            states = new LocationDTO[] { new LocationDTO {
                ID = theState.ID,
                Name = theState.Name
            }},
            houseEstates = transferHEState.Select(m => new LocationDTO
            {
                ID = m.ID,
                Name = m.Name
            })
        };


    }

    private List<HouseEstate> GetHouseEstateFromZipCode(string zipCode)
    {
        SearchParameter zipCodeParam = new SearchParameter
        {
            Operator = SearchOperator.EQUALS,
            Property = "ZipCode",
            Value = zipCode
        };
        var he = HouseEstate.Get(new List<SearchParameter>() { zipCodeParam }, ApplicationConstants.ADMINDB);
        return he.OrderBy(m => m.Name).ToList();
    }
}



// NOTE: If you change the class name "HomeServices" here, you must also update the reference to "HomeServices" in Web.config.
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class HomeServices : IHomeServices
{
    private Dictionary<int, IQuoter> RegisteredQuoters;

    public HomeServices()
    {
        RegisteredQuoters = new Dictionary<int, IQuoter>();
        RegisteredQuoters[ApplicationConstants.COMPANY] = new MapfreQuoter();
    }

    public LocationData GetLocations(string zipCode, int? state, int? city)
    {
        if (state == null)
        {
            state = 0;
        }
        if (city == null)
        {
            city = 0;
        }

        return new ZipCodeHandler().HandleFilterRequest(zipCode.Trim(), state.Value, city.Value);
    }

    public IEnumerable<int> GetQuotes(
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
    )
    {
        var userInfo = user;   //.User.Identity as UserInformation;
        int userID = 2;
        if (userInfo != -1)
        {
            userID = userInfo;
        }
        foreach (var quoter in RegisteredQuoters)
        {
            yield return quoter.Value.GetQuote(
                personType,
                name,
                lastName,
                email,
                phone,
                buildingAmount,
                contentsAmount,
                houseEstate,
                typeOfClient,
                typeOfProperty,
                useOfProperty,
                currency,
                userID
            );
        }
    }

    public int GetQuotePerCompany(
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
    )
    {
        IQuoter quoter;
        var userInfo = user;   //.User.Identity as UserInformation;
        int userID = 2;
        if (userInfo != null)
        {
            //userID = userInfo.UserID;
        }
        if (RegisteredQuoters.TryGetValue(ApplicationConstants.COMPANY, out quoter))
        {
            return quoter.GetQuote(
                personType,
                name,
                lastName,
                email,
                phone,
                buildingAmount,
                contentsAmount,
                houseEstate,
                typeOfClient,
                typeOfProperty,
                useOfProperty,
                currency,
                userID
            );
        }
        return 0;
    }
}