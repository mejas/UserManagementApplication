using System;
using System.Collections.Generic;
using UserManagementApplication.Common.Enumerations;
using UserManagementApplication.Common.Exceptions;
using UserManagementApplication.Data.Providers;
using UserManagementApplication.Data.Providers.Interfaces;

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
        public int BadLogins { get; set; }
        public string Salt { get; set; }
        
        protected IUserDataStorageProvider StorageProvider { get; set; }
        protected IDataSecurityProvider DataSecurityProvider { get; set; }
        
        #endregion

        #region Constructors

        public User() { }

        public User(IUserDataStorageProvider storageProvider, IDataSecurityProvider dataSecurityProvider)
        {
            StorageProvider = storageProvider;
            DataSecurityProvider = dataSecurityProvider;

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
            string salt = DataSecurityProvider.GenerateSalt();

            User user = new User(StorageProvider, DataSecurityProvider)
            {
                Username  = username,
                Password  = DataSecurityProvider.GenerateHash(password, salt),
                FirstName = firstName,
                LastName  = lastName,
                Birthdate = birthDate,
                RoleType  = roleType,
                BadLogins = 0,
                Salt = salt
            };

            var userMatch = StorageProvider.GetUser(user.Username);

            if (userMatch != null)
            {
                throw new ErrorException("Username already exists.");
            }

            var result = StorageProvider.AddUser(user);

            return result;
        }

        public User GetUserByUserName(string username)
        {
            return StorageProvider.GetUser(username);
        }

        public User GetUserByUserId(int userId)
        {
            return StorageProvider.GetUser(userId);
        }

        public User Update(User user)
        {
            var currentUser = StorageProvider.GetUser(user.UserId);

            if (currentUser == null)
            {
                throw new ErrorException("User not found.");
            }

            currentUser.Username  = user.Username;
            if (user.Password != currentUser.Password)
            {
                currentUser.Password = DataSecurityProvider.GenerateHash(user.Password, currentUser.Salt);
            }
            currentUser.FirstName = user.FirstName;
            currentUser.LastName  = user.LastName;
            currentUser.Birthdate = user.Birthdate;
            currentUser.RoleType  = user.RoleType;
            currentUser.BadLogins = user.BadLogins;

            var result = StorageProvider.UpdateUser(currentUser);

            return result;
        }

        public void Delete(User user)
        {
            StorageProvider.DeleteUser(user);
        }

        #endregion
    }
}
