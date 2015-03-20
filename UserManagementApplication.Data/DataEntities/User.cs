using System;
using System.Collections.Generic;
using UserManagementApplication.Data.Contracts;
using UserManagementApplication.Data.Providers;
using UserManagementApplication.Data.StorageProviders;

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
        public DbRoleType RoleType { get; set; }
        
        protected IStorageProvider StorageProvider { get; set; }
        
        #endregion

        #region Constructors

        public User(IStorageProvider storageProvider)
        {
            StorageProvider = storageProvider;
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
                            DbRoleType roleType = DbRoleType.User)
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

            var result = StorageProvider.AddUser(user);

            return result;
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
