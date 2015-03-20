
using UserManagementApplication.Common.Enumerations;
using UserManagementApplication.Engine.Providers;
namespace UserManagementApplication.Engine.BusinessEntities
{
    public class UserSession
    {
        public string SessionToken { get; set; }
        public User User { get; set; }

        IAuthenticationProvider AuthenticationProvider { get; set; }

        public UserSession(IAuthenticationProvider authenticationProvider)
        {
            AuthenticationProvider = authenticationProvider;
        }

        public UserSession AuthenticateUser(string username, string password)
        {
            var userSession = AuthenticationProvider.CreateSession(username, password);

            return userSession;
        }

        public bool IsPermitted(UserSession session, RoleType roleTypeToTest)
        {
            return AuthenticationProvider.HasPermission(session, roleTypeToTest);
        }

        public void TerminateSession(UserSession userSession)
        {
            AuthenticationProvider.TerminateSession(userSession);
        }
    }
}
