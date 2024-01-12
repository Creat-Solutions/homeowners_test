using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;

public interface MapfreWebServices
{
    XmlNode WM_XMLBuzonesGeneral(string xml, string token);
}

public static class MapfreWebServicesFactory
{
    public static MapfreWebServices GetServices(bool isTest)
    {
        return isTest ? new MapfreWebServicesTest() : (MapfreWebServices)new MapfreWebServicesProduction();
    }
}

/// <summary>
/// Test Class for Mapfre Web services adjusting its timeout
/// </summary>
public class MapfreWebServicesTest : WSSegupymeTW.WSSegupymeTW, MapfreWebServices
{
    public MapfreWebServicesTest()
    {
        this.Timeout = 60 * 8 * 1000; //4 minutes
    }
}

/// <summary>
/// Class for Mapfre Web services adjusting timeout
/// </summary>
public class MapfreWebServicesProduction : MapfreWebServiceReference.WSSegupymeTW, MapfreWebServices
{
    public MapfreWebServicesProduction()
    {
        this.Timeout = 60 * 4 * 1000; // 4 minutes
    }
}

public interface MapfreTokenServices
{
    XmlNode GeneraToken_ws(string appName);
    XmlNode ObtenToken_ws(string appName);
    XmlNode ValidaToken_ws(string appName);
}

public static class MapfreTokenServicesFactory
{
    public static MapfreTokenServices GetServices(bool isTest)
    {
        return isTest ? new MapfreTokenServicesTest() : (MapfreTokenServices)new MapfreTokenServicesProduction();
    }
}

/// <summary>
/// Test Class for Mapfre Web services adjusting its timeout
/// </summary>
public class MapfreTokenServicesTest : WSSegupymeToken.AppToken, MapfreTokenServices
{
    public MapfreTokenServicesTest()
    {
        this.Timeout = 60 * 8 * 1000; //4 minutes
    }
}

/// <summary>
/// Class for Mapfre Web services adjusting timeout
/// </summary>
public class MapfreTokenServicesProduction : MapfreWebServiceToken.AppToken, MapfreTokenServices
{
    public MapfreTokenServicesProduction()
    {
        this.Timeout = 60 * 4 * 1000; // 4 minutes
    }
}