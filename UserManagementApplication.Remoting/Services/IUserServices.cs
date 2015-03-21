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
        public IList<User> GetUsers(UserSession session);

        [OperationContract]
        public IList<User> FindUsers(UserSession session, FindUserRequest request);

        [OperationContract]
        public User Commit(UserSession session, User user);
    }
}
