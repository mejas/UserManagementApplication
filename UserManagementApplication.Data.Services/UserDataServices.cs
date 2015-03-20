using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementApplication.Common.Exceptions;
using UserManagementApplication.Data.Contracts;
using UserManagementApplication.Data.Contracts.Interfaces;
using UserManagementApplication.Data.DataEntities;
using UserManagementApplication.Data.StorageProviders;

namespace UserManagementApplication.Data.Services
{
    public class UserDataServices : IUserDataService
    {
        protected User UserEntity = null;

        public UserDataServices()
        {
            UserEntity = new User(new XMLStorageProvider());
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

        private User Translate(UserInformation user)
        {
            return new User(null)
            {
                UserId    = user.UserId,
                Username  = user.Username,
                Password  = user.Password,
                FirstName = user.FirstName,
                LastName  = user.LastName,
                Birthdate = user.Birthdate,
                RoleType  = user.RoleType
            };
        }

        protected UserInformation Translate(User userEntity)
        {
            return new UserInformation()
            {
                UserId    = userEntity.UserId,
                Username  = userEntity.Username,
                Password  = userEntity.Password,
                FirstName = userEntity.FirstName,
                LastName  = userEntity.LastName,
                Birthdate = userEntity.Birthdate,
                RoleType  = userEntity.RoleType,
                DataState = DataState.Clean
            };
        }
    }
}
