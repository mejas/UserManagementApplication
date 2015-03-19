using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementApplication.Data.DataEntities;

namespace UserManagementApplication.Data.Providers
{
    public interface IStorageProvider
    {
        IList<User> GetAllUsers();
        User AddUser(User user);
        User UpdateUser(User user);
    }
}
