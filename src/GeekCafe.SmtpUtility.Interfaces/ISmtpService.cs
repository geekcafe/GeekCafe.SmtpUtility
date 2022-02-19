using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace GeekCafe.SmtpUtility.Interfaces
{
    public interface ISmtpService
    {
        Task<bool> SendAsync(string host, int port, bool useSsl, string username,
            string password, string fromEmailAddress, string fromDisplay, string toEmailAddress,
            string subject, string body, bool bodyIsHtml,
            MailAddressCollection cc = null, MailAddressCollection bcc = null,
            AttachmentCollection attachments = null,
            bool throwExceptions = false);
    }
}
