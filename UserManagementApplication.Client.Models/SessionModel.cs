using UserManagementApplication.Client.Models.ServiceProxy;
using UserManagementApplication.Common.Exceptions;
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

        public bool Login(string username, string password)
        {
            LogonRequest logonRequest = new LogonRequest()
            {
                Username = username,
                Password = password
            };

            return InvokeMethod(() =>
            {
                var session = SessionProxy.Logon(logonRequest);

                SessionToken = session.SessionToken;

                return true;
            });
        }

        public string GetSessionToken()
        {
            return SessionToken;
        }

        public void Logout(string sessionToken)
        {
            InvokeMethod(() => SessionProxy.Logoff(new UserSession() { SessionToken = sessionToken }));
        }
    }
}
