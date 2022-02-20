using System;
using System.Net.Mail;
using System.Threading.Tasks;
using GeekCafe.SmtpUtility.Interfaces;
using Microsoft.Extensions.Logging;

namespace GeekCafe.SmtpUtility
{
    public class SmtpServices: ISmtpService
    {

        private ILogger<SmtpServices> _logger;


        public SmtpServices()
        {
            
        }

        public SmtpServices(ILogger<SmtpServices> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="useSsl"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="fromEmailAddress"></param>
        /// <param name="fromDisplay"></param>
        /// <param name="toEmailAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="bodyIsHtml"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="attachments"></param>
        /// <param name="throwExceptions"></param>
        /// <returns></returns>
        public async Task<bool> SendAsync(string host, int port, bool useSsl, string username,
            string password, string fromEmailAddress, string fromDisplay, string toEmailAddress,
            string toDisplay, string subject, string body, bool bodyIsHtml,
            MailAddressCollection cc = null, MailAddressCollection bcc = null,
            AttachmentCollection attachments = null,
            bool throwExceptions = false)
        {

            // tracking id for you;
            var id = Guid.NewGuid().ToString();

            try
            {
                
                var client = new SmtpClient(host, port);

                if (string.IsNullOrEmpty(toDisplay)) toDisplay = null;
                if (string.IsNullOrEmpty(fromDisplay)) fromDisplay = null;

                var from = new MailAddress(fromEmailAddress, fromDisplay, System.Text.Encoding.UTF8);

                client.EnableSsl = useSsl;
                client.Credentials = new System.Net.NetworkCredential(username, password);
                // Set destinations for the email message.
                if (string.IsNullOrEmpty(toDisplay)) toDisplay = null;

                var to = new MailAddress(toEmailAddress, toDisplay);
                // Specify the message content.
                var message = new MailMessage(from, to)
                {
                    Subject = subject,
                    Body = body,
                    // allow for emoji's etc 👍
                    BodyEncoding = System.Text.Encoding.UTF8,
                    SubjectEncoding = System.Text.Encoding.UTF8,
                    IsBodyHtml = bodyIsHtml
                };


                // add cc's
                if (cc != null)
                {
                    foreach (var m in cc)
                    {
                        message.CC.Add(m);
                        _logger?.LogDebug($"id: {id} CC'ing: {m.Address}");
                    }
                }

                // add bcc
                if (bcc != null)
                {
                    foreach (var m in bcc)
                    {
                        message.Bcc.Add(m);
                        _logger?.LogDebug($"id: {id} Bcc'ing: {m.Address}");
                    }
                }

                // check for attachments
                if (attachments != null)
                {
                    foreach  (var attachment in attachments)
                    {
                        message.Attachments.Add(attachment);
                    }
                }

                _logger?.LogDebug($"id: {id} Message Sending...");                

                await client.SendMailAsync(message);

                _logger?.LogDebug($"id: {id} Message Sent...");

                // Clean up.
                message.Dispose();

                _logger?.LogDebug($"id: {id} Goodbye");

                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"id: {id} Ex: {ex.Message}");
                //Log(id, "ERROR", $"Exception: {ex.Message}");

                if(ex.InnerException != null)
                {
                    _logger?.LogError($"id: {id} Inner Ex: {ex.InnerException.Message}");                    
                }


                if (throwExceptions) throw;

                return false;
            }
        }


        public async Task<bool> SendAsync(ISmtpConfig config, ISmtpEmail from, ISmtpEmail to,
            string subject, string body, bool bodyIsHtml,
            MailAddressCollection cc = null, MailAddressCollection bcc = null,
            AttachmentCollection attachments = null,
            bool throwExceptions = false)
        {
            return await SendAsync(config.Host, config.Port, config.UseSsl, config.UserName, config.Password,
                from.EmailAddress, from.DisplayName, to.EmailAddress, to.DisplayName,
                subject, body, bodyIsHtml, cc, bcc, attachments, throwExceptions
                ); 
        }

        
    }
}
