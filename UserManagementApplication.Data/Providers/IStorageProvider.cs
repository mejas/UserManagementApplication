using System.Collections.Generic;
using UserManagementApplication.Data.DataEntities;

namespace UserManagementApplication.Data.Providers
{
    public interface IStorageProvider
    {
        IList<User> GetAllUsers();
        User AddUser(User user);
        User UpdateUser(User user);
        void DeleteUser(User user);
    }
}
