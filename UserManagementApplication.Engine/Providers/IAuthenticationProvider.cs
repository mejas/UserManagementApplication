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
