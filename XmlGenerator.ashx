<%@ WebHandler Language="C#" Class="XmlGenerator" %>

using System;
using System.Web;
using System.Linq;
using Ionic.Zip;
using System.IO;
using System.Collections.Generic;
using System.Text;
public class XmlGenerator : IHttpHandler {
    
    
    
    public void ProcessRequest (HttpContext context) {
        string rangeString = context.Request.QueryString["range"];
        bool test = context.Request.QueryString["test"] == "1";
        if (!RangeService.IsValidRangeString(rangeString)) {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Invalid range");
            return;
        }
        IEnumerable<Range> ranges = null;
        try {
            ranges = RangeService.ParseRange(rangeString);
        } catch {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Invalid range");
            return;
        }
        context.Response.Clear();
        context.Response.ContentType = "application/zip";
        context.Response.AddHeader("Content-Disposition", "attachment; filename=RangoZip.zip");
        HouseInsuranceDataContext db = new HouseInsuranceDataContext();
        using (ZipFile zipResult = new ZipFile()) {
            foreach (Range range in ranges) {
                foreach (int quote in range) {
                    quote data = null;
                    try {
                        data = db.quotes.Where(m => m.id == quote).Single();
                    } catch {
                        continue;
                    }
                    MapfrePolicyXMLGenerator xmlGenerator = new MapfrePolicyXMLGenerator(db, data, test);
                    string xml = string.Empty;
                    try {
                        xml = xmlGenerator.GetXml(true);
                    } catch {
                        continue;
                    }
                    zipResult.AddEntry("Test" + quote + ".xml", xml, Encoding.UTF8);
                }
            }
            zipResult.Save(context.Response.OutputStream);
        }  
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}