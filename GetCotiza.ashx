<%@ WebHandler Language="C#" Class="GetCotiza" %>

using System;
using System.Web;
using System.Web.SessionState;

public class GetCotiza : IHttpHandler, IRequiresSessionState {

    public void ProcessRequest(HttpContext context)
    {
        int quoteid = 0;
        PolizaPDF poliza = null;
        try {
            bool english = false;
            quoteid = int.Parse(context.Request["quote"]);
            if (!string.IsNullOrEmpty(context.Request["eng"])) {
                english = context.Request["eng"] == "1";
            }
            poliza = new PolizaPDF(quoteid, english, true);
        } catch {
            quoteid = 0;
        }
        if (quoteid == 0) {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Not a valid policy number");
            return;
        }
        context.Response.ContentType = "application/pdf";
        context.Response.AddHeader("Content-Disposition", "attachment; filename=Cotizacion" + quoteid + ".pdf");
        context.Response.BinaryWrite(poliza.GetGeneratedPDF());
        //context.Response.TransmitFile(path);

    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}