using Microsoft.AspNet.Identity;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Quiver.Infrastructure.Configuration
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.sendgrid.net";
            client.EnableSsl = false;
            //client.Timeout = 10000;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("azure_008639c6a4fd8eeb79635814a15f9ad9@azure.com", "Hello.World2016");
            MailMessage mailMessage = new MailMessage("quiver@helloworldsoft.com", message.Destination, message.Subject, message.Body);
            mailMessage.IsBodyHtml = true;
            return client.SendMailAsync(mailMessage);
        }
    }
}
