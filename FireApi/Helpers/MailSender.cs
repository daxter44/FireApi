using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FireApi.Helpers
{
    public static class MailSender
    {
        public static async Task  sendMail(String address, String user, String password)
        {
           
            try
            {
              
                // Send the message 
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("fireapp@interia.pl"));
                message.To.Add(new MailboxAddress( address));
                message.Subject = "Welcome in fireApp";

                message.Body = new TextPart("plain")
                {
                    Text = @"Hey " + user + @",

Here are new password : " + password +@"

-- FireApp"
                };

                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    
                    await client.ConnectAsync("poczta.interia.pl", 465, true);

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync("fireapp@interia.pl", "Ogien2020");

                    await client.SendAsync(message);
                     client.Disconnect(true);
                }

                // Clean up
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not send e-mail. Exception caught: " + e);
            }
           
           
        }
    }
}


