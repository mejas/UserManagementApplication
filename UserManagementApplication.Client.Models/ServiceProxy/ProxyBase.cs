using System;
using System.ServiceModel;
using UserManagementApplication.Common.Exceptions;

namespace UserManagementApplication.Client.Models.ServiceProxy
{
    public abstract class ProxyBase<T>
    {
        #region Declarations
        private ChannelFactory<T> _channelFactory = null;
    	#endregion

        #region Properties
        protected string ServiceAddress { get; private set; }

        protected T RemotingService
        {
            get
            {
                return _channelFactory.CreateChannel(new EndpointAddress(ServiceAddress));
            }
        }
        #endregion

        #region Constructors
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
        #endregion

        #region Methods
        protected T InvokeMethod<T>(Func<T> method)
        {
            if (method != null)
            {
                try
                {
                    return method();
                }
                catch (FaultException ex)
                {
                    throw new ErrorException(ex.Message);
                }
                catch (EndpointNotFoundException)
                {
                    throw new ErrorException("Connection to host could not be established.");
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
                catch (FaultException ex)
                {
                    throw new ErrorException(ex.Message);
                }
                catch (EndpointNotFoundException)
                {
                    throw new ErrorException("Connection to host could not be established.");
                }
            }
        } 
        #endregion
    }
}
