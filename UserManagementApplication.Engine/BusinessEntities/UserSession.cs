﻿
using System;
using UserManagementApplication.Data.Contracts;
using UserManagementApplication.Data.Contracts.Interfaces;
using UserManagementApplication.Engine.Enumerations;
using UserManagementApplication.Engine.Providers;
namespace UserManagementApplication.Engine.BusinessEntities
{
    public class UserSession
    {
        public string SessionToken { get; set; }

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
            return AuthenticationProvider.VerifyUserPermission(session, roleTypeToTest);
        }

        public void TerminateSession(UserSession userSession)
        {
            AuthenticationProvider.TerminateSession(userSession);
        }
    }
}
