using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementApplication.Client.Models.ServiceProxy;
using UserManagementApplication.Remoting.Data;

namespace UserManagementApplication.Client.Models
{
    public class UserAddEditModel : ClientModel
    {
        protected UserServiceProxy SessionProxy { get; set; }

        public UserAddEditModel()
        {
            SessionProxy = new UserServiceProxy();
        }

        public User Commit(UserSession session, User user)
        {
            return InvokeMethod(() => SessionProxy.Commit(session, user));
        }
    }
}
