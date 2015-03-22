﻿using System;
using System.IO;
using UserManagementApplication.Common.Enumerations;
using UserManagementApplication.Common.Exceptions;
using UserManagementApplication.Data.Contracts;
using UserManagementApplication.Data.Contracts.Interfaces;
using UserManagementApplication.Data.Services;
using UserManagementApplication.Engine.BusinessEntities;
using UserManagementApplication.Engine.Providers.Interfaces;

namespace UserManagementApplication.Engine.Providers
{
    public class DefaultAuthenticationProvider : IAuthenticationProvider
    {
        protected IUserDataService UserDataService { get; set; }
        protected IAuthenticationDataService AuthenticationDataService { get; set; }

        public DefaultAuthenticationProvider()
            : this(new UserDataServices(), new AuthenticationDataServices())
        { }

        public DefaultAuthenticationProvider(IUserDataService userDataService, IAuthenticationDataService authenticationDataService)
        {
            UserDataService = userDataService;
            AuthenticationDataService = authenticationDataService;
        }

        public UserSession GetSession(string sessionToken)
        {
            var result = AuthenticationDataService.GetUserSession(sessionToken);

            return Translate(result);
        }

        public UserSession CreateSession(string username, string password)
        {
            var user = UserDataService.GetUser(username);

            ////TODO: hash passwords here before checking
            if (user == null || 
                !AuthenticationDataService.Authenticate(user, password))
            {
                if (user != null && user.RoleType != RoleType.Admin)
                {
                    user.BadLogins = user.BadLogins + 1;
                    user.DataState = DataState.Modified;

                    UserDataService.Commit(user);
                }

                throw new ErrorException("Invalid user credentials");
            }

            if (user.BadLogins >= 3)
            {
                throw new ErrorException("User is blocked");
            }

            var userSessionInfo = new UserSessionInformation()
            {
                SessionToken = generateSessionToken(),
                User = user
            };

            AuthenticationDataService.StoreSession(userSessionInfo);

            return Translate(userSessionInfo);
        }

        public bool HasPermission(UserSession session, RoleType roleTypeToTest)
        {
            UserSessionInformation userSessionInfo = AuthenticationDataService.GetUserSession(session.SessionToken);

            if (userSessionInfo == null)
            {
                throw new InvalidSessionException("Session has expired.");
            }

            if (userSessionInfo.User.RoleType == roleTypeToTest)
            {
                return true;
            }
            else
            {
                return roleTypeToTest == (userSessionInfo.User.RoleType | roleTypeToTest);
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

        protected UserSession Translate(UserSessionInformation userSessionInfo)
        {
            if (userSessionInfo != null)
            {
                return new UserSession()
                {
                    SessionToken = userSessionInfo.SessionToken,
                    User = new User().Translate(userSessionInfo.User)
                };
            }

            return null;
        }
    }
}
