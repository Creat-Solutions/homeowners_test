<%@ WebHandler Language="C#" Class="Helper" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;

public class Helper : IHttpHandler, System.Web.SessionState.IReadOnlySessionState 
{

    Dictionary<string, string> art140HelpText = new Dictionary<string, string>();
    Dictionary<string, string> art140HelpTitle = new Dictionary<string, string>();
    public void ProcessRequest (HttpContext context) 
    {
        context.Response.ContentType = "text/html";
        int id = 0;
        string help140 = string.Empty;
        string help = string.Empty;
        bool coverage = false;
        if (context.Request.QueryString["id"] == null && context.Request.QueryString["help140"] == null)
        {
            return;
        }
        if (context.Request.QueryString["id"] != null)
        {
            id = int.Parse(context.Request.QueryString["id"]);
        }
        else
        {
            help140 = context.Request.QueryString["help140"];
        }
        try {
            coverage = context.Request.QueryString["coverage"] == "1";
        } catch {
        }
        if (id <= 0 && string.IsNullOrEmpty(help140))
        {
            return;
        }
        if (id > 0)
        {
            help = "<html><head><title>{0}</title></head><body style=\"font-family: Arial, Helvetica, sans-serif;\"><h4>{1}</h4><p>{2}</p><hr /><h4>{3}</h4><p>{4}</p></body></html>";
            HouseInsuranceDataContext db = new HouseInsuranceDataContext();
            if (coverage)
            {
                var info = db.additionalCoverages.Select(m => new { m.id, m.help, m.name, m.helpEs, m.nameEs }).Single(m => m.id == id);
                help = string.Format(help, "Help/Ayuda", info.name, info.help, info.nameEs, info.helpEs);
            }
            else
            {
                var info = db.valueSublimits.Select(m => new { m.id, m.help, m.name, m.helpEs, m.nameEs }).Single(m => m.id == id);
                help = string.Format(help, "Help/Ayuda", info.name, info.help, info.nameEs, info.helpEs);
            }
        }
        else
        {
            LoadArticle140Dictionaries();
            help = "<html><head><title>{0}</title></head><body style=\"font-family: Arial, Helvetica, sans-serif;\"><h4>{1}</h4><p>{2}</p></body></html>";
            help = string.Format(help, "Help/Ayuda", art140HelpTitle[help140], art140HelpText[help140]); 
        }
        context.Response.Write(help);
    }

    private void LoadArticle140Dictionaries()
    {
        System.Web.SessionState.HttpSessionState session = HttpContext.Current.Session;
        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo((string)session["CULTURE"]);
        art140HelpText.Add(
            "Nationality", 
            HttpContext.GetLocalResourceObject(
                "/UserControls/GenerateStep4.ascx", 
                "strNationalityHelp", 
                culture)
            .ToString()
        );
        art140HelpText.Add(
            "Occupation", 
            HttpContext.GetLocalResourceObject(
                "/UserControls/GenerateStep4.ascx", 
                "strOccupationHelp",
                culture)
            .ToString()
        );
        art140HelpText.Add(
            "Representative", 
            HttpContext.GetLocalResourceObject(
                "/UserControls/GenerateStep4.ascx", 
                "strLegalRepHelp",
                culture)
            .ToString()
        );
        art140HelpText.Add(
            "ID", 
            HttpContext.GetLocalResourceObject(
                "/UserControls/GenerateStep4.ascx", 
                "strIDHelp",
                culture)
            .ToString()
        );
        art140HelpText.Add(
            "IDType", 
            HttpContext.GetLocalResourceObject(
                "/UserControls/GenerateStep4.ascx", 
                "strIDTypeHelp",
                culture)
            .ToString()
        );
        art140HelpText.Add(
            "IncNumber", 
            HttpContext.GetLocalResourceObject(
                "/UserControls/GenerateStep4.ascx", 
                "strIncNumberHelp",
                culture)
            .ToString()
        );
        art140HelpText.Add(
            "MercNumber", 
            HttpContext.GetLocalResourceObject(
                "/UserControls/GenerateStep4.ascx", 
                "strMercNumberHelp",
                culture)
            .ToString()
        );
        art140HelpText.Add(
            "FIEL", 
            HttpContext.GetLocalResourceObject(
                "/UserControls/GenerateStep4.ascx", 
                "strFIELHelp",
                culture)
            .ToString()
        );
        art140HelpText.Add(
            "DOB", 
            HttpContext.GetLocalResourceObject(
                "/UserControls/GenerateStep4.ascx", 
                "strDOBHelp",
                culture)
            .ToString()
        );
        art140HelpText.Add(
            "Phone",
            HttpContext.GetLocalResourceObject(
                "/UserControls/GenerateStep4.ascx",
                "strPhoneHelp",
                culture)
            .ToString()
        );
        art140HelpText.Add(
            "Email",
            HttpContext.GetLocalResourceObject(
                "/UserControls/GenerateStep4.ascx",
                "strEmailHelp",
                culture)
            .ToString()
        );

        art140HelpTitle.Add(
            "Nationality", 
            HttpContext.GetLocalResourceObject(
                "/UserControls/GenerateStep4.ascx", 
                "strNationality",
                culture)
            .ToString() 
        );
        art140HelpTitle.Add(
            "Occupation", 
            HttpContext.GetLocalResourceObject(
                "/UserControls/GenerateStep4.ascx", 
                "strOccupation",
                culture)
            .ToString()
        );
        art140HelpTitle.Add(
            "Representative", 
            HttpContext.GetLocalResourceObject(
                "/UserControls/GenerateStep4.ascx", 
                "strRepresentative",
                culture)
            .ToString()
        );
        art140HelpTitle.Add(
            "ID", 
            HttpContext.GetLocalResourceObject(
                "/UserControls/GenerateStep4.ascx", 
                "strID",
                culture)
            .ToString()
        );
        art140HelpTitle.Add(
            "IDType", 
            HttpContext.GetLocalResourceObject(
                "/UserControls/GenerateStep4.ascx", 
                "strIDType",
                culture)
            .ToString()
        );
        art140HelpTitle.Add(
            "IncNumber", 
            HttpContext.GetLocalResourceObject(
                "/UserControls/GenerateStep4.ascx", 
                "strIncNumber",
                culture)
            .ToString()
        );
        art140HelpTitle.Add(
            "MercNumber", 
            HttpContext.GetLocalResourceObject(
                "/UserControls/GenerateStep4.ascx", 
                "strMercNumber",
                culture)
            .ToString()
        );
        art140HelpTitle.Add(
            "FIEL", 
            HttpContext.GetLocalResourceObject(
                "/UserControls/GenerateStep4.ascx", 
                "strFIEL",
                culture)
            .ToString()
        );
        art140HelpTitle.Add(
            "DOB", 
            HttpContext.GetLocalResourceObject(
                "/UserControls/GenerateStep4.ascx", 
                "strDOB",
                culture)
            .ToString()
        );
        art140HelpTitle.Add(
            "Phone",
            HttpContext.GetLocalResourceObject(
                "/UserControls/GenerateStep4.ascx",
                "strPhone",
                culture)
            .ToString()
        );
        art140HelpTitle.Add(
            "Email",
            HttpContext.GetLocalResourceObject(
                "/UserControls/GenerateStep4.ascx",
                "strEmail",
                culture)
            .ToString()
        );

    }
    
    public bool IsReusable {
        get {
            return true;
        }
    }

}