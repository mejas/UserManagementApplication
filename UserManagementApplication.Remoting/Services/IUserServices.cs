using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UserManagementApplication.Remoting.Data;
using UserManagementApplication.Remoting.Data.Request;

namespace UserManagementApplication.Remoting.Services
{
    [ServiceContract]
    public interface IUserServices
    {
        [OperationContract]
        IList<User> GetUsers(UserSession session);

        [OperationContract]
        IList<User> FindUsers(UserSession session, FindUserRequest request);

        [OperationContract]
        User Commit(UserSession session, User user);
    }
}
