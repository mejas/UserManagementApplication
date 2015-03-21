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
    public class SessionProxy : ISessionServices
    {
        private const string USER_SVC_ADDRESS = "net.tcp://localhost:8080/UserManagementApplication/Session";

        ChannelFactory<ISessionServices> _channelFactory = null;

        protected ISessionServices SessionService
        {
            get
            {
                return _channelFactory.CreateChannel(new EndpointAddress(USER_SVC_ADDRESS));
            }
        }

        public SessionProxy()
        {
            _channelFactory = new ChannelFactory<ISessionServices>(new NetTcpBinding()
                {
                    ReceiveTimeout = TimeSpan.FromMinutes(5),
                    SendTimeout = TimeSpan.FromMinutes(5),
                    MaxBufferSize = 655360,
                    MaxBufferPoolSize = 655360,
                    MaxReceivedMessageSize = 655360
                });
        }

        public UserSession Logon(LogonRequest request)
        {
            try
            {
                return SessionService.Logon(request);
            }
            catch (FaultException ex)
            {
                throw new ErrorException(ex.Message);
            }
        }

        public void Logoff(UserSession session)
        {
            try
            {
                SessionService.Logoff(session);
            }
            catch (FaultException ex)
            {
                throw new ErrorException(ex.Message);
            }
        }
    }
}
