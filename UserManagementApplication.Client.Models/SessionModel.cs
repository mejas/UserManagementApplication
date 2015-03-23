using UserManagementApplication.Client.Models.ServiceProxy;
using UserManagementApplication.Remoting.Data;
using UserManagementApplication.Remoting.Data.Request;

namespace UserManagementApplication.Client.Models
{
    public class SessionModel : ClientModel
    {
        protected string SessionToken { get; set; }
        protected SessionServiceProxy SessionProxy { get; set; }

        public SessionModel()
        {
            SessionProxy = new SessionServiceProxy();
        }

        public UserSession Login(string username, string password)
        {
            LogonRequest logonRequest = new LogonRequest()
            {
                Username = username,
                Password = password
            };

            return InvokeMethod(() => SessionProxy.Logon(logonRequest));
        }

        public void Logout(UserSession sessionToken)
        {
            InvokeMethod(() => SessionProxy.Logoff(sessionToken));
        }

        public void ForceLogout(UserSession userSession, User user)
        {
            InvokeMethod(() => SessionProxy.TerminateSession(userSession, user));
        }
    }
}
