using UserManagementApplication.Client.Models.ServiceProxy;
using UserManagementApplication.Remoting.Data;

namespace UserManagementApplication.Client.Models
{
    public class UserAddEditModel : ClientModel
    {
        #region Properties
        protected UserServiceProxy SessionProxy { get; set; }
        #endregion

        #region Methods
        public UserAddEditModel()
        {
            SessionProxy = new UserServiceProxy();
        }

        public User Commit(UserSession session, User user)
        {
            return InvokeMethod(() => SessionProxy.Commit(session, user));
        } 
        #endregion
    }
}
