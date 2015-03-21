using System;
using System.Collections.Generic;
using System.IO;
using UserManagementApplication.Common.Enumerations;
using UserManagementApplication.Common.Exceptions;
using UserManagementApplication.Data.Contracts;
using UserManagementApplication.Data.Contracts.Interfaces;
using UserManagementApplication.Data.Services;
using UserManagementApplication.Engine.BusinessEntities;
using UserManagementApplication.Engine.Providers;
using UserManagementApplication.Engine.Providers.Interfaces;

namespace UserManagementApplication.Engine.Providers
{
    public class DefaultAuthenticationProvider : IAuthenticationProvider
    {
        protected IUserDataService UserDataService { get; set; }
        protected IAuthenticationDataService AuthenticationDataService { get; set; }

        public DefaultAuthenticationProvider() : this(new UserDataServices(), new AuthenticationDataServices()) { }

        public DefaultAuthenticationProvider(IUserDataService userDataService, IAuthenticationDataService authenticationDataService)
        {
            UserDataService = userDataService;
            AuthenticationDataService = authenticationDataService;
        }

        public UserSession CreateSession(string username, string password)
        {
            var userData = UserDataService.GetUser(username);

            ////TODO: hash passwords here before checking
            if (userData == null ||
                !(password == userData.Password))
            {
                throw new ErrorException("Invalid user credentials");
            }

            string sessionToken = generateSessionToken();

            AuthenticationDataService.StoreSession(sessionToken, userData);

            return new UserSession(this)
            {
                SessionToken = sessionToken
            };
        }

        public bool HasPermission(UserSession session, RoleType roleTypeToTest)
        {
            UserInformation userInfo = AuthenticationDataService.GetUser(session.SessionToken);

            if (userInfo == null)
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
            AuthenticationDataService.RemoveSession(userSession.SessionToken);
        }

        private string generateSessionToken()
        {
            return Path.GetRandomFileName().Replace(".", String.Empty);
        }
    }
}
