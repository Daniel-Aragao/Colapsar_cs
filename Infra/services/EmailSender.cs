using System;
using MimeKit;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;

namespace Infra.services
{
    public class EmailSender
    {
        public string server { get; }
        public int Port { get; }
        public string UserId { get; }
        public string User { get; }
        public string Password { get; }
        public string TargetId { get; set; }
        public string Target { get; set; }

        public EmailSender(IConfiguration config)
        {
            server = config["EmailConfiguration:SmtpServer"];
            Port = Int32.Parse(config["EmailConfiguration:SmtpPort"]);
            User = config["EmailConfiguration:SmtpUserName"];
            UserId = config["EmailConfiguration:SmtpUserId"];
            Password = config["EmailConfiguration:SmtpPassword"];
            Target = config["EmailConfiguration:targetName"];
            TargetId = config["EmailConfiguration:targetId"];

            string[] valid = 
            {
                server,
                Port.ToString(),
                User,
                UserId,
                Password,
                Target,
                TargetId
            };

            foreach(var str in valid)
            {
                if(string.IsNullOrWhiteSpace(str))
                {
                    throw new ArgumentException("Certify that all the EmailConfigurations are informed");
                }
            }
        }

        public void SendMail(string msg, string subject = "")
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(User, UserId));
                message.To.Add(new MailboxAddress(Target, TargetId));

                message.Subject = subject;

                message.Body = new TextPart("plain")
                {
                    Text = msg
                };

                using (var client = new SmtpClient())
                {
                    client.Connect(server, Port, SecureSocketOptions.SslOnConnect);

                    client.Authenticate(UserId, Password);

                    client.Send(message);

                    client.Disconnect(true);
                }

                Console.WriteLine("--> E-Mail sent -->");
            }
            catch(Exception e)
            {
                Console.WriteLine("--> Error sending e-mail: \n" + e);
            }
        }
    }
}