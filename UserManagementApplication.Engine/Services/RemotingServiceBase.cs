using System;
using System.ServiceModel;
using UserManagementApplication.Common.Diagnostics;
using UserManagementApplication.Common.Diagnostics.Interfaces;
using UserManagementApplication.Common.Exceptions;

namespace UserManagementApplication.Engine.Services
{
    public class RemotingServiceBase
    {
        #region Properties
        protected ILogProvider LogProvider { get; set; } 
        #endregion

        #region Constructors
        public RemotingServiceBase()
            : this(new DefaultLogProvider())
        { }

        public RemotingServiceBase(ILogProvider logProvider)
        {
            LogProvider = logProvider;
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
                catch (ErrorException ex)
                {
                    HandleException(ex);
                }
                catch (Exception ex)
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

        protected virtual void HandleException(UserManagementApplicationException ex)
        {
            LogProvider.LogMessage(ex);
            throw new FaultException(ex.Message);
        }
        #endregion

        #region Functions
        private void HandleException(Exception ex)
        {
            LogProvider.LogMessage(ex);
            throw new FaultException("A server side error occured.");
        } 
        #endregion
    }
}
