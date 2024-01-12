<%@ Application Language="C#" %>
<%@ Import Namespace="System.Net.Mail" %>
<%@ Import Namespace="System.Diagnostics" %>
<script RunAt="server">

  void Application_Start(object sender, EventArgs e)
  {
    // Code that runs on application startup
    Protec.Authentication.Configuration.SetConfiguration(
            ApplicationConstants.ADMINDB,
            ApplicationConstants.APPLICATION
        );
  }

  void Application_End(object sender, EventArgs e)
  {
    //  Code that runs on application shutdown

  }

  void Session_End(object sender, EventArgs e)
  {

  }

  private enum ErrorType
  {
    General,
    Database,
    Security,
    Unauthenticated
  }

  private bool CanRedirect()
  {
    bool canRedirect = true;
    try
    {
      string headerTest = "X-Error-Type";
      Response.Headers.Add(headerTest, "test");
      Response.Headers.Remove(headerTest);
    }
    catch (PlatformNotSupportedException)
    {
      canRedirect = true;
    }
    catch (Exception)
    {
      canRedirect = false;
    }
    return canRedirect;
  }

  private void HandleError(ErrorType type)
  {
    if (CanRedirect())
    {
      if (type == ErrorType.Unauthenticated)
      {
        Response.Redirect("~/Login.aspx?error=general", true);
        return;
      }
      string redirection = "~/Default.aspx?error=";
      switch (type)
      {
        case ErrorType.Database:
          redirection += "database";
          break;
        case ErrorType.General:
          redirection += "general";
          break;
        case ErrorType.Security:
          redirection += "security";
          break;
      }
      Response.Redirect(redirection, true);
    }
    else
    {
      try
      {
        string to = "eric@creatsol.com";
        //EmailService service = new EmailService(to, true);
        //service.Subject = "Homeowner's Insurance Application Error: No redirection possible";
        //service.Body = "Unable to send a redirection code, headers already sent";
        //service.Send();
      }
      catch
      {
      }
    }
  }

  void Application_Error(object sender, EventArgs e)
  {
    Exception excep = Server.GetLastError();
    if (!Request.IsAuthenticated)
    {
      HandleError(ErrorType.Unauthenticated);
      return;
    }
    if (excep.GetType() == typeof(System.Web.HttpException)) /* pueden ser errores 404 */
      return;
    if (excep.GetType() == typeof(System.Data.SqlClient.SqlException))
    {
      HandleError(ErrorType.Database);
    }
    else if (excep.GetType() == typeof(System.Security.SecurityException))
    {
      HandleError(ErrorType.Security);
    }
    else
    {
      try
      {
        EmailErrorReporter reporter = new EmailErrorReporter();
        reporter.SendErrorReport(excep);
      }
      catch
      {
      }
      //HandleError(ErrorType.General);
    }
  }

  void Session_Start(object sender, EventArgs e)
  {
    // Code that runs when a new session is started

  }



</script>
