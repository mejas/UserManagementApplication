using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UserManagementApplication.Common.Exceptions;
using UserManagementApplication.Remoting.Data;
using UserManagementApplication.Remoting.Data.Request;
using UserManagementApplication.Remoting.Services;

namespace UserManagementApplication.Client.Models.ServiceProxy
{
    public class SessionServiceProxy : ProxyBase<ISessionServices>, ISessionServices
    {
        public SessionServiceProxy()
            : base("net.tcp://localhost:8080/UserManagementApplication/Session")
        { }

        public UserSession Logon(LogonRequest request)
        {
            return InvokeMethod(() => RemotingService.Logon(request));
        }

        public void Logoff(UserSession session)
        {
            InvokeMethod(() => RemotingService.Logoff(session));
        }
    }
}
