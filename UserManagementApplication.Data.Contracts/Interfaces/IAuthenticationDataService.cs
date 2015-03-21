using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementApplication.Data.Contracts.Interfaces
{
    public interface IAuthenticationDataService
    {
        void StoreSession(string sessionToken, UserInformation userData);
        UserInformation GetUser(string sessionToken);
        void RemoveSession(string sessionToken);
    }
}
