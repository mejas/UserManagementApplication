
using System;
using UserManagementApplication.Common.Diagnostics;
using UserManagementApplication.Common.Diagnostics.Interfaces;
using UserManagementApplication.Common.Enumerations;
using UserManagementApplication.Engine.Providers;
using UserManagementApplication.Engine.Providers.Interfaces;
namespace UserManagementApplication.Engine.BusinessEntities
{
    public class UserSession
    {
        public string SessionToken { get; set; }
        public User User { get; set; }

        IAuthenticationProvider AuthenticationProvider { get; set; }
        ILogProvider LogProvider { get; set; }

        public UserSession()
            : this(new DefaultAuthenticationProvider(), new DefaultLogProvider())
        { }

        public UserSession(IAuthenticationProvider authenticationProvider)
            : this(authenticationProvider, new DefaultLogProvider())
        { }

        public UserSession(IAuthenticationProvider authenticationProvider, ILogProvider logProvider)
        {
            AuthenticationProvider = authenticationProvider;
            LogProvider = logProvider;
        }

        public UserSession AuthenticateUser(string username, string password)
        {
            LogMessage(String.Format("Authenticate {0}", username));

            var userSession = AuthenticationProvider.CreateSession(username, password);

            return userSession;
        }

        //TODO: ADD TEST
        public bool IsClearedForRole(UserSession session, RoleType roleTypeToTest)
        {
            LogMessage(session, String.Format("Checking against RoleType {0}", roleTypeToTest));

            bool value = AuthenticationProvider.HasPermission(session, roleTypeToTest);

            return value;
        }

        public void TerminateSession(UserSession userSession)
        {
            LogMessage(userSession, "Session termination");

            AuthenticationProvider.TerminateSession(userSession);
        }

        public UserSession GetUserSession(UserSession userSession)
        {
            LogMessage(String.Format("Retrieve session for token {0}", userSession.SessionToken));

            return AuthenticationProvider.GetSession(userSession.SessionToken);
        }

        private void LogMessage(UserSession userSession, string operation)
        {
            LogProvider.LogMessage(String.Format("[Server][{0}] Executed operation {1}", userSession.User.Username, operation));
        }

        private void LogMessage(string message)
        {
            LogProvider.LogMessage(String.Format("[Server] {0}", message));
        }
    }
}
