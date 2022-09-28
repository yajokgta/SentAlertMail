using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SendAlertEmail.Extensions
{
    class SendEmail
    {
        public static string _SMTPServer = System.Web.Configuration.WebConfigurationManager.AppSettings["SMTPServer"];
        public static string _SMTPPort = System.Web.Configuration.WebConfigurationManager.AppSettings["SMTPPort"];
        public static string _SMTPEnableSSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SMTPEnableSSL"];
        public static string _SMTPUser = System.Web.Configuration.WebConfigurationManager.AppSettings["SMTPUser"];
        public static string _SMTPPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["SMTPPassword"];
        public static string _DisplayName = System.Web.Configuration.WebConfigurationManager.AppSettings["DisplayName"];

        public static string sendEmail(string Body, string MailTo, string subject)
        {
            var result = "Succes";
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                NetworkCredential basicCredential = new NetworkCredential(_SMTPUser, _SMTPPassword);
                MailMessage message = new MailMessage();
                MailAddress fromAddress = new MailAddress(_SMTPUser, _DisplayName);

                smtpClient.Host = _SMTPServer;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = basicCredential;
                smtpClient.EnableSsl = Convert.ToBoolean(_SMTPEnableSSL);
                smtpClient.Port = Convert.ToInt32(_SMTPPort);

                message.From = fromAddress;
                //message.CC = sCc;
                message.Subject = subject;

                //Set IsBodyHtml to true means you can send HTML email.
                message.IsBodyHtml = true;

                message.Priority = MailPriority.High;
                message.Body = Body;
                string[] mail = MailTo.Split(',');
                foreach (string s in mail)
                {
                    if (s != "")
                    {
                        message.Bcc.Add(new MailAddress(s));
                    }
                }

                message.Sender = fromAddress;
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
                result = ex.ToString();
            }
            return result;
        }
    }
}
