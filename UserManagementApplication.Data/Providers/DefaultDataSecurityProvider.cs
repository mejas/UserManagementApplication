using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UserManagementApplication.Common.Security;
using UserManagementApplication.Data.Providers.Interfaces;

namespace UserManagementApplication.Data.Providers
{
    public class DefaultDataSecurityProvider : IDataSecurityProvider
    {
        public string GenerateHash(string input, string salt)
        {
            return new HashGenerator().GenerateHash(salt + input);
        }

        public string GenerateSalt()
        {
            return Path.GetRandomFileName().Replace(".", String.Empty);
        }
    }
}
