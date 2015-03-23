using System;
using UserManagementApplication.Common.Exceptions;

namespace UserManagementApplication.Client.Models
{
    public abstract class ClientModel
    {
        #region Events
        public event EventHandler<UserManagementApplicationException> HandleModelException;
        #endregion

        #region Methods
        protected void OnModelException(UserManagementApplicationException e)
        {
            EventHandler<UserManagementApplicationException> handler = HandleModelException;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected T InvokeMethod<T>(Func<T> method)
        {
            if (method != null)
            {
                try
                {
                    return method();
                }
                catch (UserManagementApplicationException ex)
                {
                    OnModelException(ex);
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
                    OnModelException(ex);
                }
            }
        } 
        #endregion
    }
}
