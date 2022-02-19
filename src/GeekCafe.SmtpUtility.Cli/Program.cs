using System;
using System.Threading.Tasks;

namespace GeekCafe.Smtp.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Email World!");

            try
            {
                var lib = new SmtpUtility.SmtpServices();

                var host = Environment.GetEnvironmentVariable("SMTP_HOST"); // "email-smtp.us-east-1.amazonaws.com";
                int port = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT")); // 587;
                var username = Environment.GetEnvironmentVariable("SMTP_USER_NAME");
                var password = Environment.GetEnvironmentVariable("SMTP_USER_PASSWORD");
                var fromEmail = Environment.GetEnvironmentVariable("SMTP_EMAIL_FROM");
                var displayName = Environment.GetEnvironmentVariable("SMTP_EMAIL_FROM_DISPLAY");
                var toEamil = Environment.GetEnvironmentVariable("SMTP_EMAIL_TO");

                var subject = $"Test email from c# cli using {host}:{port}";
                var message = $"test message at {DateTime.UtcNow} UTC";
                var t = Task.Run(async () =>
                {
                    await lib.SendAsync(host, port, true, username, password, fromEmail, displayName,
                        toEamil, subject, message, false);
                }
                );

                t.Wait();

                Console.WriteLine($"Message sent to {toEamil}");
                Console.WriteLine($"Message from {fromEmail}");
                Console.WriteLine($"Using username {username}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An execption occurred {ex.Message}");
            }

            

            Console.WriteLine("Exiting Program");
        }
    }
}
