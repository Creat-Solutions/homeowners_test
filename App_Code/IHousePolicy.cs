using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public enum PolicyCacheStatus
{
    Incomplete,
    Complete
}

public class PolicyCacheData
{
    public PolicyCacheStatus Status { get; set; }
    public IHousePolicy Data { get; set; }
    public bool SkipPayment { get; set; }
}


/// <summary>
/// Interfaz que nos brinda los metodos para conversion de cotizacion a poliza
/// </summary>
public interface IHousePolicy
{
    string PolicyNo { get; }
    int PolicyID { get; }

    bool ConvertToPolicy(int quoteid, bool test, decimal premium, decimal taxPercentage, decimal comisionPercentage, decimal exchangeRate);
}

public interface IHousePolicyTester
{
    void TestRoutine(IHousePolicy data);
}
