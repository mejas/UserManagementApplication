using System;
using System.ServiceModel;
using UserManagementApplication.Common.Diagnostics;
using UserManagementApplication.Common.Diagnostics.Interfaces;
using UserManagementApplication.Common.Exceptions;

namespace UserManagementApplication.Engine.Services
{
    public class RemotingServiceBase
    {
        protected ILogProvider LogProvider { get; set; }

        public RemotingServiceBase()
            : this(new DefaultLogProvider())
        { }

        public RemotingServiceBase(ILogProvider logProvider)
        {
            LogProvider = logProvider;
        }

        protected T InvokeMethod<T>(Func<T> method)
        {
            if (method != null)
            {
                try
                {
                    return method();
                }
                catch (ErrorException ex)
                {
                    HandleException(ex);
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
                catch (UserManagementApplicationException ex)
                {
                    HandleException(ex);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        private void HandleException(Exception ex)
        {
            LogProvider.LogMessage(ex);
        }

        protected virtual void HandleException(UserManagementApplicationException ex)
        {
            LogProvider.LogMessage(ex);
            throw new FaultException(ex.Message);
        }
    }
}
