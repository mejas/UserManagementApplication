using System;
using UserManagementApplication.Common.Exceptions;

namespace UserManagementApplication.Client.Models
{
    public abstract class ClientModel
    {
        public event EventHandler<UserManagementApplicationException> HandleModelException;

        protected void OnModelException(UserManagementApplicationException e)
        {
            EventHandler<UserManagementApplicationException> handler = HandleModelException;

            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
