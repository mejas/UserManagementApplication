using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementApplication.Data.Contracts;
using UserManagementApplication.Data.Contracts.Interfaces;
using UserManagementApplication.Data.DataEntities;
using UserManagementApplication.Data.StorageProviders;

namespace UserManagementApplication.Data.Services
{
    public class AuthenticationDataServices : IAuthenticationDataService
    {
        public Session SessionEntity = null;

        public AuthenticationDataServices()
        {
            SessionEntity = new Session(new SessionDataCacheStorageProvider());
        }

        public void StoreSession(string sessionToken, UserInformation userData)
        {
            SessionEntity.CreateSession(sessionToken, Translate(userData));
        }

        public UserInformation GetUser(string sessionToken)
        {
            var result = SessionEntity.GetSession(sessionToken);

            UserInformation userInfo = null;

            if (result != null)
            {
                userInfo = Translate(result.UserData);
            }

            return userInfo;
        }

        public void RemoveSession(string sessionToken)
        {
            SessionEntity.RemoveSession(sessionToken);
        }

        private UserInformation Translate(User user)
        {
            return new UserInformation()
            {
                Birthdate = user.Birthdate,
                FirstName = user.FirstName,
                LastName  = user.LastName,
                Password  = user.Password,
                RoleType  = user.RoleType,
                UserId    = user.UserId,
                Username  = user.Username,
                DataState = DataState.Clean
            };
        }

        private User Translate(UserInformation userData)
        {
            return new User()
            {
                Birthdate = userData.Birthdate,
                FirstName = userData.FirstName,
                LastName  = userData.LastName,
                Password  = userData.Password,
                RoleType  = userData.RoleType,
                UserId    = userData.UserId,
                Username  = userData.Username
            };
        }
    }
}
