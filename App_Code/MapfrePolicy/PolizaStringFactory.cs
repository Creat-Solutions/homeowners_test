using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Xml;
using System.Web.Caching;
using System.IO;


/// <summary>
/// Recupera una cadena en base al archivo xml solicitado
/// </summary>
public class PolizaStringFactory
{
    private string m_filename = null;
    private XDocument m_document = null;
    private static string accessKey = "AccessTime_";
    private static string hashKey = "HashKey_";
    private Hashtable m_localCache;

	public PolizaStringFactory(string filename)
	{
        HttpServerUtility Server = HttpContext.Current.Server;
        Cache Cache = HttpContext.Current.Cache;
        m_filename = Server.MapPath(filename);
        
        if (Cache[accessKey + m_filename] == null || 
            Cache[hashKey + m_filename] == null) {

            Cache[accessKey + m_filename] = DateTime.Now.Ticks;
            m_localCache = new Hashtable();
            Cache[hashKey + m_filename] = m_localCache;
        } else {
            long ticks = (long)Cache[accessKey + m_filename];
            FileInfo info = new FileInfo(m_filename);
            if (info.LastWriteTime.Ticks > ticks) {
                Cache[accessKey + m_filename] = DateTime.Now.Ticks;
                m_localCache = new Hashtable();
                Cache[hashKey + m_filename] = m_localCache;
            } else {
                m_localCache = Cache[hashKey + m_filename] as Hashtable;
            }
        }
	}

    public string GetString(string key)
    {
        if (m_localCache == null) {
            throw new Exception("m_localCache is null");
        }
        if (m_localCache.ContainsKey(key)) {
            return m_localCache[key] as string;
        }
        string value = null;
        try {
            if (m_document == null)
                m_document = XDocument.Load(m_filename, LoadOptions.PreserveWhitespace); //lazy evalution of m_document
            value = m_document.Descendants("string").Where(m => m.Attribute("name").Value == key).
                Select(m => m.Value).Single();
            if (!string.IsNullOrEmpty(value))
                m_localCache[key] = value;
        } catch {
            return null;
        }
        return value;
    }

}
