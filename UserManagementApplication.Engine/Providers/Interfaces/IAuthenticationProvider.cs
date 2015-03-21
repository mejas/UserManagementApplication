using UserManagementApplication.Common.Enumerations;
using UserManagementApplication.Engine.BusinessEntities;

namespace UserManagementApplication.Engine.Providers.Interfaces
{
    public interface IAuthenticationProvider
    {
        UserSession CreateSession(string username, string password);
        bool HasPermission(UserSession session, RoleType roleTypeToTest);
        void TerminateSession(UserSession userSession);
    }
}
