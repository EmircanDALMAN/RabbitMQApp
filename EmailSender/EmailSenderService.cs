using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace EmailSender
{
    public static class EmailSenderService
    {
        public static void Send(string to, string message)
        {
            const string email = ""; //Sizin email adresiniz
            const string password = ""; //Sizin email şifreniz
            var smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com", 
                Port = 587, 
                EnableSsl = true
            };

            var credential = new NetworkCredential(email, password);
            smtpClient.Credentials = credential;

            var sender = new MailAddress(email, ""); //Tırnak içine kimden geldiğini yazabilirsiniz
            var receiver = new MailAddress(to);

            var mail = new MailMessage(sender, receiver)
            {
                Subject = "", //Nereden Geldiğini söyleyebilirsiniz 
                Body = message
            };
            smtpClient.Send(mail);
        }
    }
}
