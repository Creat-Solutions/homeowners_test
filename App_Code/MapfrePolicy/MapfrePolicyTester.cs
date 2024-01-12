using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Sends Email with xml string information
/// </summary>
public class MapfrePolicyTester : IHousePolicyTester
{
    private bool m_test;

    public MapfrePolicyTester(bool test)
    {
        m_test = test;
    }

    public void TestRoutine(IHousePolicy data)
    {
        if (m_test && data is MapfrePolicy) {

        }
    }
}
