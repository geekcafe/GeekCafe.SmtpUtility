using System;
namespace GeekCafe.SmtpUtility.Interfaces
{
    public interface ISmtpEmail
    {
        public string EmailAddress { get; set; }
        public string DisplayName { get; set; }
    }
}
