using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

/// <summary>
/// Summary description for EmailService
/// </summary>
//public class EmailService
//{
//    private MailMessage m_message;

//    public bool IsHighPriority
//    {
//        get
//        {
//            return m_message.Priority == MailPriority.High;
//        }
//        set
//        {
//            m_message.Priority = value ? MailPriority.High : MailPriority.Normal;
//        }
//    }

//    public string Body
//    {
//        get
//        {
//            return m_message.Body;
//        }

//        set
//        {
//            m_message.Body = value;
//        }
//    }

//    public string Subject
//    {
//        get
//        {
//            return m_message.Subject;
//        }

//        set
//        {
//            m_message.Subject = value;
//        }
//    }

//    public EmailService(string from, string to, bool html)
//    {

//        m_message = new MailMessage(from, to);
//        m_message.IsBodyHtml = html;

//    }

//    public EmailService(string from, string to) :
//        this(from, to, false)
//    {

//    }

//    public EmailService(string to, bool html) :
//        this(ConfigurationSettings.AppSettings["MailFrom"], to, html)
//    {
//    }

//    public EmailService(string to) :
//        this(to, false)
//    {
//    }

//    public void Send()
//    {
//        SmtpClient client = new SmtpClient(ConfigurationSettings.AppSettings["MailServer"]);
//        if (ConfigurationSettings.AppSettings["MailAuthenticationRequired"] == "true")
//        {
//            client.Port = int.Parse(ConfigurationSettings.AppSettings["MailSmtpPort"]);
//            client.EnableSsl = ConfigurationSettings.AppSettings["MailSmtpSSL"] == "true" ? true : false;
//            client.UseDefaultCredentials = ConfigurationSettings.AppSettings["MailSmtpDefaultCredential"] == "true" ? true : false;
//            client.DeliveryMethod = SmtpDeliveryMethod.Network;
//            client.Credentials = new NetworkCredential(m_message.From.Address, ConfigurationSettings.AppSettings["MailPassword"]);            
//        }
//        client.Send(m_message);
//    }

//    public void AddAttachment(Stream stream, string filename, string type)
//    {
//        Attachment attachment = new Attachment(stream, filename, type);
//        m_message.Attachments.Add(attachment);
//    }

//    public void AddCC(string to)
//    {
//        m_message.CC.Add(to);
//    }

//    public void AddBCC(string to)
//    {
//        m_message.Bcc.Add(to);
//    }
//}
