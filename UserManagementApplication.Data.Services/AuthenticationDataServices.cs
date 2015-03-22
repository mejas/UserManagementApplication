using UserManagementApplication.Data.Contracts;
using UserManagementApplication.Data.Contracts.Interfaces;
using UserManagementApplication.Data.DataEntities;
using UserManagementApplication.Data.StorageProviders;
using UserManagementApplication.Data.StorageProviders.Interfaces;

namespace UserManagementApplication.Data.Services
{
    public class AuthenticationDataServices : IAuthenticationDataService
    {
        private static ISessionDataStorageProvider _sessionDataStorageProvider = null;

        protected static ISessionDataStorageProvider SessionDataStorageProvider
        {
            get
            {
                if (_sessionDataStorageProvider == null)
                {
                    _sessionDataStorageProvider = new SessionDataCacheStorageProvider();
                }

                return _sessionDataStorageProvider;
            }
        }

        protected Session SessionEntity = null;

        public AuthenticationDataServices()
        {
            SessionEntity = new Session(SessionDataStorageProvider);
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
            if (user != null)
            {
                return new UserInformation()
                {
                    Birthdate = user.Birthdate,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = user.Password,
                    RoleType = user.RoleType,
                    UserId = user.UserId,
                    Username = user.Username,
                    DataState = DataState.Clean
                };
            }

            return null;
        }

        private User Translate(UserInformation userData)
        {
            if (userData != null)
            {
                return new User()
                {
                    Birthdate = userData.Birthdate,
                    FirstName = userData.FirstName,
                    LastName = userData.LastName,
                    Password = userData.Password,
                    RoleType = userData.RoleType,
                    UserId = userData.UserId,
                    Username = userData.Username
                };
            }

            return null;
        }
    }
}
