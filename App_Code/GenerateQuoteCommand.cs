using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for GenerateQuoteCommand
/// </summary>
public class GenerateQuoteCommand: AbstractLoadCommand
{
    public GenerateQuoteCommand(HttpContext context) : 
        base(context)
	{
		
	}

    public override void Init()
    {
        ClearQuoteAndPolicy();
        Response.Redirect("~/Application/GenerateQuote.aspx", true);
    }

    public override void Load(string id)
    {
    }
}