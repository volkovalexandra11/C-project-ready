using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using MimeKit;
using MailKit;

namespace WebApplication2.Controllers
{
    public class Email
    {
        public void SendEmail(byte[] content, params string[] emails)
        {
            foreach (var email in emails)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Стремный график", "stremnye.graphiki@gmail.com"));
                message.To.Add(new MailboxAddress(email));
                message.Subject = "Твой график";
                var builder = new BodyBuilder{ HtmlBody = "<div style=\"color: green;\">Твой странный график ы</div>" };
                builder.Attachments.Add("Your graph.jpeg", content);
                builder.Attachments.Add("тихон.jpg");
                message.Body = builder
                    .ToMessageBody();

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s,c,h,e) => true;
                    client.Connect("smtp.gmail.com", 465, true);
                    client.Authenticate("stremnye.graphiki@gmail.com", "tupyegraphiki");
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
        }
    }
}
