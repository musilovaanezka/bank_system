using stin.Data;
using stin.Models;
using System.Security.Cryptography.X509Certificates;

namespace stin.Services
{
    public class AuthenticationCodeService
    {
        public AuthenticationCodeService() { }
        public AutenticationCode getAuthenticationCode(string userName)
        {

            return new AutenticationCode()
            {
                Username = userName,
                EndDateTime = DateTime.Now.AddDays(1),
                Code = GenerateCode()
            };
        }


        private string GenerateCode()
        {
            // vhodnější by bylo využít RNGCryptoServiceProvider
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random rnd = new Random();
            string result = "";
            for (int i = 0; i < 7; i++)
            {
                result += chars[rnd.Next(0, chars.Length - 1)];
            }

            return result;

        }
    }
}
