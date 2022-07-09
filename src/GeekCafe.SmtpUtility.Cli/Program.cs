using System;
using System.Threading.Tasks;
using GeekCafe.Core.Utility.Extensions;

namespace GeekCafe.Smtp.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Email World!");


            var stressTest = Environment.GetEnvironmentVariable("STRESS_TEST").ToBool();

            if (stressTest)
            {
                var name = Environment.GetEnvironmentVariable("STRESS_TEST_EMAIL_NAME");
                var domain = Environment.GetEnvironmentVariable("STRESS_TEST_EMAIL_DOMAIN");
                var count = Environment.GetEnvironmentVariable("STRESS_TEST_EMAIL_COUNT").ToInt();
                var start = DateTime.UtcNow;
                RunStressTest(count, name, domain);
                var elapsed = start.ToElapsedTime();

                Console.WriteLine($"Elapsed Time: {elapsed}");

            }
            else
            {
                RunSingleTest();
            }


            Console.WriteLine("Exiting Program");
        }


        private static void RunSingleTest()
        {
            SendEmail(null);
        }

        private static void RunStressTest(int count, string name, string domain, string subject = null)
        {
            for(int i =0; i<count; i++)
            {
                var email_subject = subject ?? $"Stress Test No. {i}";
                var to = $"{name}+{i.ToString("000")}@{domain}";
                //Console.WriteLine(to);
                //Console.WriteLine(email_subject);
                SendEmail(to, subject);
            }
        }

        private static void SendEmail(string to = null, string subject = null)
        {
            try
            {
                var lib = new SmtpUtility.SmtpServices();

                var host = Environment.GetEnvironmentVariable("SMTP_HOST"); // "email-smtp.us-east-1.amazonaws.com";
                int port = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT")); // 587;
                var username = Environment.GetEnvironmentVariable("SMTP_USER_NAME");
                var password = Environment.GetEnvironmentVariable("SMTP_USER_PASSWORD");
                var fromEmail = Environment.GetEnvironmentVariable("SMTP_EMAIL_FROM");
                var displayName = Environment.GetEnvironmentVariable("SMTP_EMAIL_FROM_DISPLAY");
                var toEamil = to ?? Environment.GetEnvironmentVariable("SMTP_EMAIL_TO");

                var bcc = lib.GenerateMailAddressCollection("ericw1229@gmail.com; eric.wilson@webbasix.com, ewilson@webbasix.com");
                var cc = lib.GenerateMailAddressCollection("eric.wilson+one@geekcafe.com; eric.wilson+two@geekcafe.com, ericw1229+one@gmail.com");

                subject ??= $"Test email from c# cli using {host}:{port}";
                var message = $"test message at {DateTime.UtcNow} UTC / {DateTime.Now} ET";
                var t = Task.Run(async () =>
                {
                    await lib.SendAsync(host, port, true, username, password, fromEmail, displayName,
                        toEamil, "", subject, message, false,
                        cc, bcc);
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
        }
    }
}
