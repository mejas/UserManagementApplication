using System.Collections.Generic;
using UserManagementApplication.Data.DataEntities;

namespace UserManagementApplication.Data.Providers.Interfaces
{
    public interface IUserDataStorageProvider
    {
        IList<User> GetUsers();
        IList<User> GetUsers(string firstName, string lastName);
        User AddUser(User user);
        User UpdateUser(User user);
        User GetUser(string username);
        User GetUser(int id);
        void DeleteUser(User user);
    }
}
