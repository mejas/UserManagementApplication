using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementApplication.Data.Enumerations;
using UserManagementApplication.Data.Providers;

namespace UserManagementApplication.Data.DataEntities
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public RoleType RoleType { get; set; }

        protected IStorageProvider StorageProvider { get; set; }
        
        public User(IStorageProvider storageProvider)
        {
            StorageProvider = storageProvider;
        }

        public IList<User> GetAll()
        {
            return StorageProvider.GetAllUsers();
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

            var result = StorageProvider.AddUser(user);

            return result;
        }

        public User Update(User user)
        {
            var result = StorageProvider.UpdateUser(user);

            return result;
        }
    }
}
