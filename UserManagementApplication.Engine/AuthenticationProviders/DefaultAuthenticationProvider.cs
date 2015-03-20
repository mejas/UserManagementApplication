using System.Collections.Generic;
using System.IO;
using UserManagementApplication.Common.Enumerations;
using UserManagementApplication.Common.Exceptions;
using UserManagementApplication.Data.Contracts;
using UserManagementApplication.Data.Contracts.Interfaces;
using UserManagementApplication.Engine.BusinessEntities;
using UserManagementApplication.Engine.Providers;

namespace UserManagementApplication.Engine.AuthenticationProviders
{
    public class DefaultAuthenticationProvider : IAuthenticationProvider
    {
        protected Dictionary<string, UserInformation> UserSessions = new Dictionary<string, UserInformation>();
        protected IUserDataService UserDataService { get; set; }

        public DefaultAuthenticationProvider(IUserDataService userDataService)
        {
            UserDataService = userDataService;
        }

        public UserSession CreateSession(string username, string password)
        {
            var userData = UserDataService.GetUser(username);

            ////TODO: hash passwords here before checking
            if (!(password == userData.Password))
            {
                throw new ErrorException("Invalid user credentials");
            }

            string sessionToken = generateSessionToken();

            UserSessions[sessionToken] = userData;

            return new UserSession(this)
            {
                SessionToken = sessionToken
            };
        }

        public bool HasPermission(UserSession session, RoleType roleTypeToTest)
        {
            UserInformation userInfo;

            if (!UserSessions.TryGetValue(session.SessionToken, out userInfo))
            {
                throw new ErrorException("Session has expired.");
            }

            if (userInfo.RoleType == roleTypeToTest)
            {
                return true;
            }
            else
            {
                return roleTypeToTest == (userInfo.RoleType | roleTypeToTest);
            }
        }

        public void TerminateSession(UserSession userSession)
        {
            UserInformation userInfo;

            if (UserSessions.TryGetValue(userSession.SessionToken, out userInfo))
            {
                UserSessions.Remove(userSession.SessionToken);
            }
        }

        private string generateSessionToken()
        {
            return Path.GetRandomFileName().Replace(".", "");
        }
    }
}
