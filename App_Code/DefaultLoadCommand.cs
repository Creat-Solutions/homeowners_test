using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Comportamiento default
/// </summary>
public class DefaultLoadCommand : AbstractLoadCommand
{
	public DefaultLoadCommand(HttpContext context) : 
        base(context)
	{
		
	}

    public override void Init()
    {
    }

    public override void Load(string id)
    {
        
    }
}
