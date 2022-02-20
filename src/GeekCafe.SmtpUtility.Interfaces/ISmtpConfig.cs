using System;
namespace GeekCafe.SmtpUtility.Interfaces
{
    public interface ISmtpConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool UseSsl { get; set; } 
    }
}
