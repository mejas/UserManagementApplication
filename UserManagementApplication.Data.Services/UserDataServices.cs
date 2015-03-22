using System.Collections.Generic;
using System.Linq;
using UserManagementApplication.Common.Exceptions;
using UserManagementApplication.Data.Contracts;
using UserManagementApplication.Data.Contracts.Interfaces;
using UserManagementApplication.Data.DataEntities;
using UserManagementApplication.Data.StorageProviders;
using UserManagementApplication.Data.StorageProviders.Interfaces;

namespace UserManagementApplication.Data.Services
{
    public class UserDataServices : IUserDataService
    {
        private static IUserDataStorageProvider _storageProvider = null;

        protected static IUserDataStorageProvider StorageProvider
        {
            get
            {
                if (_storageProvider == null)
                {
                    _storageProvider = new UserDataXmlStorageProvider();
                }

                return _storageProvider;
            }
        }

        protected User UserEntity = null;

        public UserDataServices()
        {
            UserEntity = new User(StorageProvider);
        }

        public IList<UserInformation> GetUsers()
        {
            return UserEntity.GetAll().ToList().ConvertAll<UserInformation>(Translate);
        }

        public IList<UserInformation> GetUsers(string firstName, string lastName)
        {
            return UserEntity.GetUsers(firstName, lastName).ToList().ConvertAll(Translate);
        }

        public UserInformation Commit(UserInformation user)
        {
            switch(user.DataState)
            {
                case DataState.New:
                    return Translate(UserEntity.Create(user.Username, user.Password, user.FirstName, user.LastName, user.Birthdate, user.RoleType));
                case DataState.Modified:
                    return Translate(UserEntity.Update(Translate(user)));
                case DataState.Deleted:
                    UserEntity.Delete(Translate(user));
                    return null;
                case DataState.Clean:
                    return user;
            }

            throw new ErrorException("Invalid operation type.");
        }

        public UserInformation GetUser(string username)
        {
            return Translate(UserEntity.GetUserByUserName(username));
        }

        private User Translate(UserInformation user)
        {
            if (user != null)
            {
                return new User()
                {
                    UserId    = user.UserId,
                    Username  = user.Username,
                    Password  = user.Password,
                    FirstName = user.FirstName,
                    LastName  = user.LastName,
                    Birthdate = user.Birthdate,
                    RoleType  = user.RoleType,
                    BadLogins = user.BadLogins
                };
            }

            return null;
        }

        protected UserInformation Translate(User userEntity)
        {
            if (userEntity != null)
            {
                return new UserInformation()
                {
                    UserId = userEntity.UserId,
                    Username = userEntity.Username,
                    Password = userEntity.Password,
                    FirstName = userEntity.FirstName,
                    LastName = userEntity.LastName,
                    Birthdate = userEntity.Birthdate,
                    RoleType = userEntity.RoleType,
                    BadLogins = userEntity.BadLogins,
                    DataState = DataState.Clean
                };
            }

            return null;
        }
    }
}
