using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            return SessionProxy.GetUsers(new UserSession() { SessionToken = sessionToken });
        }
    }
}
