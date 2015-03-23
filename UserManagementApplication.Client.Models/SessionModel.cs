using UserManagementApplication.Client.Models.ServiceProxy;
using UserManagementApplication.Common.Security;
using UserManagementApplication.Remoting.Data;
using UserManagementApplication.Remoting.Data.Request;

namespace UserManagementApplication.Client.Models
{
    public class SessionModel : ClientModel
    {
        #region Properties
        protected string SessionToken { get; set; }
        protected SessionServiceProxy SessionProxy { get; set; } 
        #endregion

        #region Constructors
        public SessionModel()
        {
            SessionProxy = new SessionServiceProxy();
        } 
        #endregion

        #region Methods
        public UserSession Login(string username, string password)
        {
            LogonRequest logonRequest = new LogonRequest()
            {
                Username = username,
                Password = new HashGenerator().GenerateHash(password)
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
        #endregion
    }
}
