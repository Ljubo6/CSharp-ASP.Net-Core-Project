using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniverseRestaurant.Services
{
    public class EmailSender : IMailSender
    {
        public  void SendEmailAsync(string email, string subject, string message)
        {
             Execute(email,subject, message);
        }

        private void Execute(string email, string subject, string msg)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Universe*Restaurant", "univer.restaurant@gmail.com"));
            message.To.Add(new MailboxAddress(email));
            message.Subject = subject;
            message.Body = new BodyBuilder() { HtmlBody = msg }.ToMessageBody();

            using (MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtp.gmail.com", 465, true); 
                client.Authenticate("univer.restaurant@gmail.com", "Admin123*");
                client.Send(message);

                client.Disconnect(true);            
            }           
        }
    }
}
