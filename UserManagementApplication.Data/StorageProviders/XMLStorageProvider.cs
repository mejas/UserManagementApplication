using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UserManagementApplication.Common.Exceptions;
using UserManagementApplication.Data.DataEntities;
using UserManagementApplication.Data.Providers;

namespace UserManagementApplication.Data.StorageProviders
{
    public class XMLStorageProvider : IStorageProvider
    {
        protected List<User> UserCache = new List<User>();
        private int _newUserId = 0;

        public IList<User> GetUsers()
        {
            return UserCache;
        }

        public User AddUser(User user)
        {
            user.UserId = _newUserId++;

            checkUserExistence(user);

            UserCache.Add(user);

            InvalidateCache();

            return user;
        }

        public User UpdateUser(User user)
        {
            var userToUpdate = UserCache.Find(item => item.UserId == user.UserId);

            userToUpdate.Username  = user.Username;
            userToUpdate.Password  = user.Password;
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName  = user.LastName;
            userToUpdate.Birthdate = user.Birthdate;
            userToUpdate.RoleType  = user.RoleType;

            InvalidateCache();

            return user;
        }

        public User GetUserByUsername(string username)
        {
            return UserCache.Find(item => item.Username == username);
        }

        public void DeleteUser(User user)
        {
            var userToRemove = UserCache.Find(item => item.UserId == user.UserId);

            UserCache.Remove(userToRemove);

            InvalidateCache();
        }

        protected void InvalidateCache()
        {
            serializeUsers();
        }

        private void serializeUsers()
        {
            throw new NotImplementedException();
        }

        private void checkUserExistence(User user)
        {
            var userMatch = GetUserByUsername(user.Username);

            if (userMatch != null)
            {
                throw new WarningException("Username already exists.");
            }
        }

        public IList<User> GetUsers(string firstName, string lastName)
        {
            return  UserCache.FindAll(user =>
                    String.IsNullOrEmpty(firstName) && user.LastName == lastName ||
                    String.IsNullOrEmpty(lastName) && user.FirstName == firstName ||
                    user.FirstName == firstName && user.LastName == lastName);
        }
    }
}
