<%@ WebHandler Language="C#" Class="GetInvoice" %>

using System;
using System.Web;

public class GetInvoice : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        int polizaid = 0;
        MapfreElectronicInvoice invoice = null;
        try {
            //bool english = false;
            polizaid = int.Parse(context.Request["id"]);
            bool fepadre = context.Request["fepadre"] == "1";
            //if (!string.IsNullOrEmpty(context.Request["eng"])) {
            //    english = context.Request["eng"] == "1";
            //}

            invoice = new MapfreElectronicInvoice(polizaid, fepadre);
            
        } catch {
            polizaid = 0;
        }
        if (polizaid == 0) {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Not a valid policy number");
            return;
        }

        context.Response.ContentType = "application/pdf";
        context.Response.AddHeader("Content-Disposition", "attachment; filename=Invoice" + invoice.GetPolizaNo() + ".pdf");
        context.Response.BinaryWrite(invoice.GetGeneratedPDF());
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}