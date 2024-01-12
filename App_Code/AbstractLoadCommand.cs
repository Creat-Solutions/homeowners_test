using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

/// <summary>
/// Clase abstracta de la cual se debe heredar para poder ejecutar comandos para Load.aspx
/// </summary>
public abstract class AbstractLoadCommand
{
    private HttpContext Context;

    protected HttpSessionState Session
    {
        get
        {
            return Context.Session;
        }
    }

    protected HttpRequest Request
    {
        get
        {
            return Context.Request;
        }
    }

    protected HttpResponse Response
    {
        get
        {
            return Context.Response;
        }
    }

    protected void ClearQuoteAndPolicy()
    {
        Session["quoteid"] = null;
        Session["StepIndex"] = null;
        Session["policyid"] = null;
        Session["policyno"] = null;
        Session["skipPayment"] = null;
        Session["PolicyStep"] = null;
        Session["quoteidBuy"] = null;
        Session["startDateExp"] = null;
        Session["endDateExp"] = null;
        Session["testExp"] = null;
    }

	public AbstractLoadCommand(HttpContext context)
    {
        Context = context;
	}

    public abstract void Init();
    public abstract void Load(string id);
}
