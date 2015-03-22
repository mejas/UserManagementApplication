using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UserManagementApplication.Data.Providers.Interfaces;

namespace UserManagementApplication.Data.Providers
{
    public class DefaultDataSecurityProvider : IDataSecurityProvider
    {
        public string GenerateHash(string input, string salt)
        {
            string combined = input + salt;

            var bytes = Encoding.UTF8.GetBytes(combined);

            var hasher = new SHA256Managed();

            var hashedInput = hasher.ComputeHash(bytes);

            return Convert.ToBase64String(hashedInput);
        }

        public string GenerateSalt()
        {
            return Path.GetRandomFileName().Replace(".", String.Empty);
        }
    }
}
