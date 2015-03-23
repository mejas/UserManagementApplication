using System.Collections.Generic;
using UserManagementApplication.Remoting.Data;
using UserManagementApplication.Remoting.Data.Request;
using UserManagementApplication.Remoting.Services;

namespace UserManagementApplication.Client.Models.ServiceProxy
{
    public class UserServiceProxy : ProxyBase<IUserServices>, IUserServices
    {
        #region Constructors
        public UserServiceProxy()
            : base("net.tcp://localhost:8080/UserManagementApplication/User")
        { } 
        #endregion

        #region Methods
        public IList<User> GetUsers(UserSession session)
        {
            return InvokeMethod(() => RemotingService.GetUsers(session));
        }

        public IList<User> FindUsers(UserSession session, FindUserRequest request)
        {
            return InvokeMethod(() => RemotingService.FindUsers(session, request));
        }

        public User Commit(UserSession session, User user)
        {
            return InvokeMethod(() => RemotingService.Commit(session, user));
        } 
        #endregion
    }
}
