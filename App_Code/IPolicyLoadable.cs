using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public interface IPolicyLoadable : IApplicationStep
{
    bool LoadFromPolicy { get; set; }
}
