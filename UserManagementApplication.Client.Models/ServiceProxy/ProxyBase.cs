using System;
using System.ServiceModel;
using UserManagementApplication.Common.Exceptions;

namespace UserManagementApplication.Client.Models.ServiceProxy
{
    public abstract class ProxyBase<T>
    {
        protected string ServiceAddress { get; private set; }
        private ChannelFactory<T> _channelFactory = null;

        protected T RemotingService
        {
            get
            {
                return _channelFactory.CreateChannel(new EndpointAddress(ServiceAddress));
            }
        }

        public ProxyBase(string serviceAddress)
        {
            ServiceAddress = serviceAddress;

            _channelFactory = new ChannelFactory<T>(new NetTcpBinding()
            {
                ReceiveTimeout         = TimeSpan.FromMinutes(5),
                SendTimeout            = TimeSpan.FromMinutes(5),
                MaxBufferSize          = 65536,
                MaxBufferPoolSize      = 65536,
                MaxReceivedMessageSize = 65536
            });
        }

        protected T InvokeMethod<T>(Func<T> method)
        {
            if (method != null)
            {
                try
                {
                    return method();
                }
                catch (FaultException fe)
                {
                    throw new ErrorException(fe.Message);
                }
            }
            
            return default(T);
        }

        protected void InvokeMethod(System.Action method)
        {
            if (method != null)
            {
                try
                {
                    method();
                }
                catch (FaultException fe)
                {
                    throw new ErrorException(fe.Message);
                }
            }
        }
    }
}
