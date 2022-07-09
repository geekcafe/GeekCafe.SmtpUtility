using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace GeekCafe.SmtpUtility.Interfaces
{
    public interface ISmtpService
    {
        Task<bool> SendAsync(string host, int port, bool useSsl, string username,
            string password, string fromEmailAddress, string fromDisplay, string toEmailAddress,
            string toDisplay, string subject, string body, bool bodyIsHtml,
            MailAddressCollection cc = null, MailAddressCollection bcc = null,
            AttachmentCollection attachments = null,
            bool throwExceptions = false);


        Task<bool> SendAsync(ISmtpConfig config, ISmtpEmail from, ISmtpEmail to,
            string subject, string body, bool bodyIsHtml,
            MailAddressCollection cc = null, MailAddressCollection bcc = null,
            AttachmentCollection attachments = null,
            bool throwExceptions = false);

        MailAddressCollection GenerateMailAddressCollection(string emailAddresses);

        MailAddressCollection GenerateMailAddressCollection(IEnumerable<string> emailAddresses);
    }
}
