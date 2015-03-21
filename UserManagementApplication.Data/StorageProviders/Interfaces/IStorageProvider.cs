using System.Collections.Generic;
using UserManagementApplication.Data.DataEntities;

namespace UserManagementApplication.Data.StorageProviders.Interfaces
{
    public interface IUserDataStorageProvider
    {
        IList<User> GetUsers();
        IList<User> GetUsers(string firstName, string lastName);
        User AddUser(User user);
        User UpdateUser(User user);
        User GetUserByUsername(string username);
        void DeleteUser(User user);
    }
}
