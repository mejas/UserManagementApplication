using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementApplication.Engine.BusinessEntities;
using UserManagementApplication.Engine.Enumerations;

namespace UserManagementApplication.Engine.Providers
{
    public interface IAuthenticationProvider
    {
        UserSession CreateSession(string username, string password);

        bool VerifyUserPermission(UserSession session, RoleType roleTypeToTest);

        void TerminateSession(UserSession userSession);
    }
}
