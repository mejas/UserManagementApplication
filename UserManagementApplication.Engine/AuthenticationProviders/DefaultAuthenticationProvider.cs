using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UserManagementApplication.Common.Exceptions;
using UserManagementApplication.Data.Contracts;
using UserManagementApplication.Data.Contracts.Interfaces;
using UserManagementApplication.Engine.BusinessEntities;
using UserManagementApplication.Engine.Enumerations;
using UserManagementApplication.Engine.Providers;

namespace UserManagementApplication.Engine.AuthenticationProviders
{
    public class DefaultAuthenticationProvider : IAuthenticationProvider
    {
        protected Dictionary<string, UserSession> UserSessions = new Dictionary<string, UserSession>();
        protected User UserEntity { get; set; }

        public UserSession CreateSession(string username, string password)
        {
            User userData = UserEntity.GetUser(username);

            ////TODO: hash passwords here before checking
            if (!(password == userData.Password))
            {
                throw new ErrorException("Invalid user credentials");
            }

            return new UserSession(this)
            {
                SessionToken = generateSessionToken(userData)
            };
        }

        public bool VerifyUserPermission(UserSession session, RoleType roleTypeToTest)
        {
            throw new NotImplementedException();
        }

        public void TerminateSession(UserSession userSession)
        {
            throw new NotImplementedException();
        }

        private string generateSessionToken(User userData)
        {
            throw new NotImplementedException();
        }
    }
}
