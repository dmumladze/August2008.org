using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using August2008.Common.Interfaces;
using log4net;

namespace August2008.Services
{
    public class EmailService : IEmailService
    {
        private ILog Log;
        private string SmtpServer;
        private string Username;
        private string Password;

        public EmailService(ILog log)
        {
            Log = log;
            SmtpServer = ConfigurationManager.AppSettings["August2008:SmtpServer"];
            Username = ConfigurationManager.AppSettings["August2008:SmtpUsername"];
            Password = ConfigurationManager.AppSettings["August2008:SmtpPassword"];
        }
        public void SendEmail(string from, string to, string subject, string body)
        {
            try
            {
                var smtp = new SmtpClient(SmtpServer);
                smtp.Credentials = new NetworkCredential(Username, Password);
                smtp.Send(new MailMessage(from, to, subject, body));
            }
            catch (Exception ex)
            {
                Log.Error("Error while sending email.", ex);
            }
        }
    }
}
