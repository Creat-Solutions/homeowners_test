using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Clear quote and policy data command
/// </summary>
public class ClearLoadCommand : AbstractLoadCommand
{
	public ClearLoadCommand(HttpContext context) :
        base(context)
	{
		
	}

    public override void Init()
    {
        ClearQuoteAndPolicy();
        Response.Redirect("~/Default.aspx", true);
    }

    public override void Load(string id)
    {
    }
}
