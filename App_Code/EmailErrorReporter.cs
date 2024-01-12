using NETInteractions_Hayw.MailSettings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Services;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

public class EmailErrorReporter
{
    public string To { get; set; }
    public string Subject { get; set; }

    public EmailErrorReporter()
    {
        To = "eric@creatsol.com";
        Subject = "Error en aplicacion de Homeowners Mapfre";
    }

    public void SendErrorReport(Exception ex)
    {
        if (ex == null)
        {
            return;
        }

        string title = "<p>Error en: " + HttpContext.Current.Request.Url.ToString() + "</p>";

        SendErrorReport(title + BuildExceptionMessage(ex, null, 1));
    }

    private string BuildExceptionMessage(Exception ex, Exception fatherExcep, int level)
    {
        if (ex == fatherExcep)
        {
            return "<p>Misma excepcion que la excepcion padre.</p>";
        }

        StringBuilder sb = new StringBuilder();

        sb.Append("<p>ex.Message: <b>");
        sb.Append(ex.Message);
        sb.Append("</b><br />ex.Source: <b>");
        sb.Append(ex.Source);
        sb.Append("</b><br />ex.TargetSite: <b>");
        sb.Append(ex.TargetSite.Name);
        sb.Append("</b><br />ex.StackTrace: </p><pre>");
        sb.Append(ex.StackTrace + "</pre>");

        if (ex.Data.Count > 0)
        {
            sb.Append("<p>Diccionario ex.Data:</p>");
            sb.Append("<ul>");
            foreach (var key in ex.Data.Keys)
            {
                string data = ex.Data[key].ToString();
                sb.Append("<li>");
                sb.Append(key.ToString());
                sb.Append(": ");
                sb.Append(data);
                sb.Append("</li>");
            }
            sb.Append("</ul>");
        }


        if (ex.InnerException != null)
        {
            sb.Append("<p>ex.InnerException: <br />");
            sb.Append(string.Format("<p>--------Inicio InnerException({0})--------</p>", level));
            sb.Append(BuildExceptionMessage(ex.InnerException, ex, level + 1));
            sb.Append(string.Format("<p>--------Fin InnerException({0})-----------</p>", level));
        }

        return sb.ToString();
    }

    public void SendErrorReport(string message)
    {
        //EmailService service = new EmailService(To, true);
        //service.Subject = Subject;
        //service.Body = message;
        //service.IsHighPriority = true;
        //service.Send();
        //

        string path = AppDomain.CurrentDomain.BaseDirectory + "\\Log.txt";
        if (!File.Exists(path))
        {
            File.Create(path).Close();
        }

        try
        {
            EmailSettings _mail = new EmailSettings();
            _mail.alias = ConfigurationSettings.AppSettings["mail_alias"];
            _mail.toAddress = new System.Net.Mail.MailAddressCollection();
            _mail.toAddress.Add(To);
            _mail.subject = Subject;
            _mail.body = message;
            _mail.bodyHTML = true;
            _mail.CreateMail();

            if (!string.IsNullOrEmpty(_mail.Alert))
            {
                File.AppendAllText(
                path,
                DateTime.Now.ToString() + Environment.NewLine +
                    _mail.Alert
                );
            }
        }
        catch (Exception ex)
        {
            File.AppendAllText(
                path,
                DateTime.Now.ToString() + Environment.NewLine +
                    ex.Message + Environment.NewLine +
                    ex.InnerException.ToString() + Environment.NewLine +
                    ex.StackTrace + Environment.NewLine + Environment.NewLine
                );
        }
    }

}