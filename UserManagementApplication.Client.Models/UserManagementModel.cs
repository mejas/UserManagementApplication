using System.Collections.Generic;
using UserManagementApplication.Client.Models.ServiceProxy;
using UserManagementApplication.Remoting.Data;

namespace UserManagementApplication.Client.Models
{
    public class UserManagementModel : ClientModel
    {
        protected UserServiceProxy SessionProxy { get; set; }

        public UserManagementModel()
        {
            SessionProxy = new UserServiceProxy();
        }

        public IList<User> GetUsers(string sessionToken)
        {
            return InvokeMethod(() => SessionProxy.GetUsers(new UserSession() { SessionToken = sessionToken }));
        }

        public void DeleteUser(string sessionToken, User user)
        {
            InvokeMethod(() => SessionProxy.Commit(new UserSession() { SessionToken = sessionToken }, user));
        }
    }
}
