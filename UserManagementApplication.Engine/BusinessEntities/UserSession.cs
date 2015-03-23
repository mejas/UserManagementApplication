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
        #region Properties

        public string SessionToken { get; set; }
        public User User { get; set; }

        #endregion

        #region Providers
        
        IAuthenticationProvider AuthenticationProvider { get; set; }
        ILogProvider LogProvider { get; set; }
        
        #endregion

        #region Constructors
        
        public UserSession(IAuthenticationProvider authenticationProvider)
            : this(authenticationProvider, new DefaultLogProvider())
        { }

        public UserSession()
            : this(new DefaultAuthenticationProvider(), new DefaultLogProvider())
        { }

        public UserSession(IAuthenticationProvider authenticationProvider, ILogProvider logProvider)
        {
            AuthenticationProvider = authenticationProvider;
            LogProvider = logProvider;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Authenticates the user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public UserSession AuthenticateUser(string username, string password)
        {
            LogMessage(String.Format("Authenticate {0}", username));

            var userSession = AuthenticationProvider.CreateSession(username, password);

            return userSession;
        }

        /// <summary>
        /// Determines whether the specified session is cleared for the evaluated role.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="roleTypeToTest">The role type to test.</param>
        /// <returns></returns>
        public bool IsClearedForRole(UserSession session, RoleType roleTypeToTest)
        {
            LogMessage(session, String.Format("Checking against RoleType {0}", roleTypeToTest));

            bool value = AuthenticationProvider.HasPermission(session, roleTypeToTest);

            return value;
        }

        /// <summary>
        /// Terminates the session.
        /// </summary>
        /// <param name="userSession">The user session.</param>
        public void TerminateSession(UserSession userSession)
        {
            LogMessage(userSession, "Session termination");

            AuthenticationProvider.TerminateSession(userSession);
        }

        /// <summary>
        /// Terminates the session.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="username">The username.</param>
        /// <exception cref="System.InvalidOperationException">User is not allowed to perform this operation.</exception>
        public void TerminateSession(UserSession session, User user)
        {
            if (!IsClearedForRole(session, RoleType.Admin))
            {
                throw new InvalidOperationException("User is not allowed to perform this operation.");
            }

            AuthenticationProvider.TerminateSession(new UserSession() { User = user });
        }

        /// <summary>
        /// Gets the user session.
        /// </summary>
        /// <param name="userSession">The user session.</param>
        /// <returns></returns>
        public UserSession GetUserSession(UserSession userSession)
        {
            LogMessage(String.Format("Retrieve session for token {0}", userSession.SessionToken));

            return AuthenticationProvider.GetSession(userSession.SessionToken);
        }

        #endregion

        #region Methods
        
        private void LogMessage(UserSession userSession, string operation)
        {
            LogProvider.LogMessage(String.Format("[Server][{0}] Executed operation {1}", userSession.User.Username, operation));
        }

        private void LogMessage(string message)
        {
            LogProvider.LogMessage(String.Format("[Server] {0}", message));
        }

        #endregion
    }
}
