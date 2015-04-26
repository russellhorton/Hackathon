using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace EpilepsySite.Web.Helpers
{
    public class EmailHelper
    {
        public static void SendEmail(string toAddress, string fromAddress, string subject, string messageBody)
        {
            // Create the email object first, then add the properties.
            var myMessage = new SendGridMessage();

            // Add the message properties.
            myMessage.From = new MailAddress(fromAddress);

            // Add multiple addresses to the To field.
            List<String> recipients = new List<String>
            {
               toAddress                
            };

            myMessage.AddTo(recipients);

            myMessage.Subject = subject;

            //Add the HTML and Text bodies
            myMessage.Html = messageBody;
            myMessage.Text = messageBody;

            // Create network credentials to access your SendGrid account
            var username = Configuration.ConfigurationManager.SendGridUserName;
            var pswd = Configuration.ConfigurationManager.SendGridPassword;

            /* Alternatively, you may store these credentials via your Azure portal
               by clicking CONFIGURE and adding the key/value pairs under "app settings".
               Then, you may access them as follows: 
               var username = System.Environment.GetEnvironmentVariable("SENDGRID_USER"); 
               var pswd = System.Environment.GetEnvironmentVariable("SENDGRID_PASS");
               assuming you named your keys SENDGRID_USER and SENDGRID_PASS */
            
            var credentials = new NetworkCredential(username, pswd);

            // Create an Web transport for sending email.
            var transportWeb = new SendGrid.Web(credentials);
            
            // Send the email.
            // You can also use the **DeliverAsync** method, which returns an awaitable task.
            transportWeb.DeliverAsync(myMessage);
        }
    }
}