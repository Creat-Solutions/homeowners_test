using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Comando para cargar nuevas cotizaciones
/// </summary>
public class NewQuoteLoadCommand : AbstractLoadCommand
{
	public NewQuoteLoadCommand(HttpContext context) : 
        base(context)
	{
		
	}

    public override void Init()
    {
        ClearQuoteAndPolicy();
        Response.Redirect("~/Application/InsuranceApplication.aspx", true);
    }

    public override void Load(string id)
    {
    }
}
