using System;
using System.Net.Mail;
using System.Threading.Tasks;
using GeekCafe.SmtpUtility.Interfaces;

namespace GeekCafe.SmtpUtility
{
    public class SmtpServices: ISmtpService
    {

        public async Task<bool> SendAsync(string host, int port, bool useSsl, string username,
            string password, string fromEmailAddress, string fromDisplay, string toEmailAddress,
            string subject, string body, bool bodyIsHtml,
            MailAddressCollection cc = null, MailAddressCollection bcc = null,
            AttachmentCollection attachments = null,
            bool throwExceptions = false)
        {
            var id = Guid.NewGuid().ToString();

            try
            {
                
                var client = new SmtpClient(host, port);
                var from = new MailAddress(fromEmailAddress, fromDisplay, System.Text.Encoding.UTF8);

                client.EnableSsl = useSsl;
                client.Credentials = new System.Net.NetworkCredential(username, password);
                // Set destinations for the email message.
                var to = new MailAddress(toEmailAddress);
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
                        Log(id, "INFO", $"CC'ing: {m.Address}");
                    }
                }

                // add bcc
                if (bcc != null)
                {
                    foreach (var m in bcc)
                    {
                        message.Bcc.Add(m);
                        Log(id, "INFO", $"Bcc'ing: {m.Address}");
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

                Log(id, "INFO", "Sending message...");
                await client.SendMailAsync(message);
                Log(id, "INFO", "Message Sent...");

                // Clean up.
                message.Dispose();
                Log(id, "INFO", "Goodbye.");

                return true;
            }
            catch (Exception ex)
            {
                Log(id, "ERROR", $"Exception: {ex.Message}");

                if(ex.InnerException != null)
                {
                    Log(id, "ERROR", $"Inner Exception: {ex.InnerException.Message}" );
                }


                if (throwExceptions) throw;

                return false;
            }
        }

        private static void Log(string id, string logLevel, string message)
        {
            Console.WriteLine($"[EMAIL_SERVICE] [LEVEL] {logLevel} - [TRACKINGC_CODE]:{{{id}}} [MESSAGE]: {message}");
        }
    }
}
