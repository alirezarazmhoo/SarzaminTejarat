using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Utility
{
    public class SendEmail
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }

        public string ErrorDescription { get; set; }
        public SendEmail(Setting setting)
        {
            DBContext db = new DBContext();
            HostName = setting.HostName;
            Port = 587;
            EmailAddress = setting.Email;
            #region Property
            Password = setting.EmailPassword;
            #endregion
        }


        public bool Send(string Body, string Subject, List<string> ListEmail)
        {

            MailMessage mail = new MailMessage();
            mail.BodyEncoding = System.Text.UTF8Encoding.UTF8;
            mail.Subject = Subject;
            mail.IsBodyHtml = true;
            mail.From = new MailAddress(EmailAddress, "سرزمین تجارت");
            mail.Body = "<div>" + Body + "  </div>";
            foreach (var item in ListEmail)
            {
                mail.To.Add(new MailAddress(item));
            }
            //System.Net.ServicePointManager.ServerCertificateValidationCallback +=
            //    (s, cert, chain, sslPolicyErrors) => true;



            SmtpClient smtp = new SmtpClient(HostName, Port);
            smtp.Credentials = new NetworkCredential(EmailAddress, Password);
            //smtp.EnableSsl = true;
            smtp.Send(mail);
            return true;


        }
    }
}