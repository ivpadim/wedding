using System.Net;
using System.Net.Mail;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Wedding.Mvc.Services
{
    public class GmailService
    {
        public static void Send(string toEmail, string toName, string subject, string body)
        {
            string gmailAccount = RoleEnvironment.GetConfigurationSettingValue("GmailAccount");
            string gmailPassword = RoleEnvironment.GetConfigurationSettingValue("GmailPassword");
            string gmailName = RoleEnvironment.GetConfigurationSettingValue("GmailName");

            var fromAddress = new MailAddress(gmailAccount, gmailName);
            var toAddress = new MailAddress(toEmail, toName);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(fromAddress.Address, gmailPassword),
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }

        }
    }
}