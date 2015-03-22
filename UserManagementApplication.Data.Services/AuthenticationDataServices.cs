using UserManagementApplication.Data.Contracts;
using UserManagementApplication.Data.Contracts.Interfaces;
using UserManagementApplication.Data.DataEntities;
using UserManagementApplication.Data.Providers;
using UserManagementApplication.Data.Providers.Interfaces;

namespace UserManagementApplication.Data.Services
{
    public class AuthenticationDataServices : IAuthenticationDataService
    {
        protected Session SessionEntity = null;

        public AuthenticationDataServices()
        {
            SessionEntity = new Session(ProviderSingleton.Instance.SessionDataStorageProvider);
        }

        public void StoreSession(UserSessionInformation userSession)
        {
            SessionEntity.CreateSession(userSession.SessionToken, Translate(userSession.User));
        }

        public UserSessionInformation GetUserSession(string sessionToken)
        {
            var result = SessionEntity.GetSession(sessionToken);

            UserInformation userInfo = null;

            if (result != null)
            {
                userInfo = Translate(result.UserData);
                return new UserSessionInformation()
                {
                    SessionToken = sessionToken,
                    User = userInfo
                };
            }

            return null;
        }

        public void RemoveSession(string sessionToken)
        {
            SessionEntity.RemoveSession(sessionToken);
        }

        public bool Authenticate(UserInformation userInformation, string password)
        {
            var dataSecurityProvider = ProviderSingleton.Instance.DataSecurityProvider;

            User user = new User(ProviderSingleton.Instance.UserDataStorageProvider, dataSecurityProvider);

            user = user.GetUserByUserName(userInformation.Username);

            return user.Password == dataSecurityProvider.GenerateHash(password, user.Salt);
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
                    LastName  = userData.LastName,
                    Password  = userData.Password,
                    RoleType  = userData.RoleType,
                    UserId    = userData.UserId,
                    Username  = userData.Username
                };
            }

            return null;
        }
    }
}
