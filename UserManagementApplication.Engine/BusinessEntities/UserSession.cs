
using UserManagementApplication.Common.Enumerations;
using UserManagementApplication.Common.Exceptions;
using UserManagementApplication.Engine.Providers;
using UserManagementApplication.Engine.Providers.Interfaces;
namespace UserManagementApplication.Engine.BusinessEntities
{
    public class UserSession
    {
        public string SessionToken { get; set; }
        public User User { get; set; }

        IAuthenticationProvider AuthenticationProvider { get; set; }

        public UserSession()
            : this(new DefaultAuthenticationProvider())
        { }

        public UserSession(IAuthenticationProvider authenticationProvider)
        {
            AuthenticationProvider = authenticationProvider;
        }

        public UserSession AuthenticateUser(string username, string password)
        {
            var userSession = AuthenticationProvider.CreateSession(username, password);

            return userSession;
        }

        //TODO: ADD TEST
        public bool IsClearedForRole(UserSession session, RoleType roleTypeToTest)
        {
            bool value = AuthenticationProvider.HasPermission(session, roleTypeToTest);

            if (!value)
            {
                throw new UnauthorizedOperationException("User is not authorized for this operation.");
            }

            return value;
        }

        public void TerminateSession(UserSession userSession)
        {
            AuthenticationProvider.TerminateSession(userSession);
        }

        public UserSession GetUserSession(UserSession userSession)
        {
            return AuthenticationProvider.GetSession(userSession.SessionToken);
        }

        public bool IsPermitted(UserSession userSession, RoleType roleType)
        {
            return AuthenticationProvider.HasPermission(userSession, roleType);
        }
    }
}
