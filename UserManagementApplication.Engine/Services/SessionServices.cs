using System.ServiceModel;
using UserManagementApplication.Engine.BusinessEntities;
using UserManagementApplication.Engine.Translators;
using UserManagementApplication.Remoting.Data.Request;
using UserManagementApplication.Remoting.Services;

namespace UserManagementApplication.Engine.Services
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = false)]
    public class SessionServices : RemotingServiceBase, ISessionServices
    {
        public Remoting.Data.UserSession Logon(LogonRequest request)
        {
            return InvokeMethod(() =>
                {
                    UserSession userSession = new UserSession();

                    var result = userSession.AuthenticateUser(request.Username, request.Password);

                    return new SessionTranslator().Translate(result);
                });
        }

        public void Logoff(Remoting.Data.UserSession session)
        {
            InvokeMethod(() =>
            {
                UserSession userSession = new UserSession();

                userSession.TerminateSession(new SessionTranslator().Translate(session));
            });
        }

        public void TerminateSession(Remoting.Data.UserSession session, Remoting.Data.User user)
        {
            InvokeMethod(() =>
                {
                    UserSession userSession = new UserSession();

                    userSession.TerminateSession(   new SessionTranslator().Translate(session), 
                                                    new UserTranslator().Translate(user));
                });
        }
    }
}
