using System;
using System.Collections.Generic;
using UserManagementApplication.Common.Enumerations;
using UserManagementApplication.Common.Exceptions;
using UserManagementApplication.Data.StorageProviders.Interfaces;

namespace UserManagementApplication.Data.DataEntities
{
    public class User
    {
        #region Properties

        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public RoleType RoleType { get; set; }
        
        protected IUserDataStorageProvider StorageProvider { get; set; }
        
        #endregion

        #region Constructors

        public User()
        { }

        public User(IUserDataStorageProvider storageProvider)
        {
            StorageProvider = storageProvider;
            RoleType = RoleType.User;
        }

        #endregion

        #region Methods
        
        public IList<User> GetAll()
        {
            return StorageProvider.GetUsers();
        }

        public IList<User> GetUsers(string firstName, string lastName)
        {
            return StorageProvider.GetUsers(firstName, lastName);
        }

        public User Create( string username,
                            string password,
                            string firstName,
                            string lastName,
                            DateTime birthDate,
                            RoleType roleType = RoleType.User)
        {
            User user = new User(StorageProvider)
            {
                Username = username,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                Birthdate = birthDate,
                RoleType = roleType
            };

            var userMatch = StorageProvider.GetUserByUsername(user.Username);

            if (userMatch != null)
            {
                throw new ErrorException("Username already exists.");
            }

            var result = StorageProvider.AddUser(user);

            return result;
        }

        public User GetUserByUserName(string username)
        {
            return StorageProvider.GetUserByUsername(username);
        }

        public User Update(User user)
        {
            var result = StorageProvider.UpdateUser(user);

            return result;
        }

        public void Delete(User user)
        {
            StorageProvider.DeleteUser(user);
        }

        #endregion
    }
}
