using Org.BouncyCastle.Asn1.X509;

namespace Klient.Management.Service.Models
{
    public class EmailConfiguration
    {
        public string From { get; set; }

        public string SmtpServer { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
