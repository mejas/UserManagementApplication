using UserManagementApplication.Remoting.Data;
using UserManagementApplication.Remoting.Data.Request;
using UserManagementApplication.Remoting.Services;

namespace UserManagementApplication.Client.Models.ServiceProxy
{
    public class SessionServiceProxy : ProxyBase<ISessionServices>, ISessionServices
    {
        #region Constructors
        public SessionServiceProxy()
            : base("net.tcp://localhost:8080/UserManagementApplication/Session")
        { } 
        #endregion

        #region Methods
        public UserSession Logon(LogonRequest request)
        {
            return InvokeMethod(() => RemotingService.Logon(request));
        }

        public void Logoff(UserSession session)
        {
            InvokeMethod(() => RemotingService.Logoff(session));
        }

        public void TerminateSession(UserSession session, User user)
        {
            InvokeMethod(() => RemotingService.TerminateSession(session, user));
        } 
        #endregion
    }
}
