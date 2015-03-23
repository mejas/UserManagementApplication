using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementApplication.Common.Security
{
    public class HashGenerator
    {
        public string GenerateHash(string input)
        {
            string combined = input;

            var bytes = Encoding.UTF8.GetBytes(combined);

            var hasher = new SHA256Managed();

            var hashedInput = hasher.ComputeHash(bytes);

            return Convert.ToBase64String(hashedInput);
        }
    }
}
