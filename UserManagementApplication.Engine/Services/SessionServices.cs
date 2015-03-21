using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementApplication.Engine.BusinessEntities;
using UserManagementApplication.Remoting.Data.Request;
using UserManagementApplication.Remoting.Services;

namespace UserManagementApplication.Engine.Services
{
    public class SessionServices : ISessionServices
    {
        public Remoting.Data.UserSession Logon(LogonRequest request)
        {
            UserSession userSession = new UserSession();

            var result = userSession.AuthenticateUser(request.Username, request.Password);

            return Translate(result);
        }

        public void Logoff(Remoting.Data.UserSession session)
        {
            UserSession userSession = new UserSession();

            userSession.TerminateSession(Translate(session));
        }

        private UserSession Translate(Remoting.Data.UserSession session)
        {
            if (session != null)
            {
                return new UserSession()
                {
                    SessionToken = session.SessionToken
                };
            }

            return null;
        }

        private Remoting.Data.UserSession Translate(UserSession result)
        {
            if (result != null)
            {
                return new Remoting.Data.UserSession()
                {
                    SessionToken = result.SessionToken
                };
            }

            return null;
        }
    }
}
