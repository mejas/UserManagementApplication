using System.ServiceModel;
using UserManagementApplication.Remoting.Data;
using UserManagementApplication.Remoting.Data.Request;

namespace UserManagementApplication.Remoting.Services
{
    [ServiceContract]
    public interface ISessionServices
    {
        [OperationContract]
        UserSession Logon(LogonRequest request);

        [OperationContract]
        void Logoff(UserSession session);

        [OperationContract]
        void TerminateSession(UserSession session, User user);
    }
}
