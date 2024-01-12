<%@ WebHandler Language="C#" Class="GetPoliza" %>

using System;
using System.Web;

public class GetPoliza : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        int polizaid = 0;
        PolizaPDF poliza = null, polizaIngles = null;
        string errorMessage = string.Empty;
        try {
            //bool english = false;
            string policyno = null;
            if (string.IsNullOrEmpty(context.Request["polizano"])) {
                polizaid = int.Parse(context.Request["poliza"]);
            } else {
                policyno = context.Request["polizano"].Trim();
            }
            //if (!string.IsNullOrEmpty(context.Request["eng"])) {
            //    english = context.Request["eng"] == "1";
            //}
            if (policyno == null) {
                poliza = new PolizaPDF(polizaid, false, false);
                polizaIngles = new PolizaPDF(polizaid, true, false);
            } else {
                poliza = new PolizaPDF(policyno, false);
                polizaIngles = new PolizaPDF(policyno, true);
            }
            poliza.Merge(polizaIngles);
        } catch (Exception e) {
            errorMessage = e.Message + "\n" + e.StackTrace;
            polizaid = -1;
        }
        if (polizaid == -1) {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Not a valid policy number");
            if (!string.IsNullOrEmpty(errorMessage)) {
                context.Response.Write(errorMessage);
            }
            return;
        }
        context.Response.ContentType = "application/pdf";
        context.Response.AddHeader("Content-Disposition", "attachment; filename=Poliza" + poliza.GetPolizaNo() + ".pdf");
        context.Response.BinaryWrite(poliza.GetGeneratedPDF());
        //context.Response.TransmitFile(path);

    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}