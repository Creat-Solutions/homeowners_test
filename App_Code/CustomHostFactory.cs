using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Activation;

/// <summary>
/// Summary description for CustomHostFactory
/// </summary>
public class CustomHostFactory : ServiceHostFactory
{
    protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
    {
        ServiceHost host = null;

        foreach (Uri b in baseAddresses)
        {
            if (HttpContext.Current.Request.Url.Host == b.Host)
            {
                host = new ServiceHost(serviceType, b);
                break;
            }
        }
        if (host == null)
        {
            host = new ServiceHost(serviceType, baseAddresses[0]);
        }

        return host;
    }
}
