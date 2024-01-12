<%@ WebHandler Language="C#" Class="CalculaPDF" %>

using System;
using System.Web;

public class CalculaPDF : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

        int quoteid = 0;
        IQuoteCalc document = null;
        try {
            quoteid = int.Parse(context.Request["quoteid"]);
            document = new QuoteCalculo(quoteid);
        } catch {
            quoteid = 0;
        }
        if (quoteid == 0) {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Not a valid quote id");
        } else {
            context.Response.ContentType = "application/pdf";
            context.Response.AddHeader("Content-Disposition", "attachment; filename=QuoteCalculo" + quoteid + ".pdf");
            context.Response.BinaryWrite(document.GetGeneratedPDF());
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}