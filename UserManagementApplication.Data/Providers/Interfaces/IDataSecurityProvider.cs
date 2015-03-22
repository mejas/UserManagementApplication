using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementApplication.Data.Providers.Interfaces
{
    public interface IDataSecurityProvider
    {
        string GenerateHash(string input, string salt);
        string GenerateSalt();
    }
}
