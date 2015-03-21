using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UserManagementApplication.Remoting.Data;
using UserManagementApplication.Remoting.Data.Request;

namespace UserManagementApplication.Remoting.Services
{
    [ServiceContract]
    public interface ISessionServices
    {
        [OperationContract]
        public UserSession Logon(LogonRequest request);

        [OperationContract]
        public void Logoff(UserSession session);
    }
}
