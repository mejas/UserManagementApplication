using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UserManagementApplication.Common.Exceptions;

namespace UserManagementApplication.Engine.Services
{
    public class RemotingServiceBase
    {
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
                catch (ErrorException ex)
                {
                    HandleException(ex);
                }
            }
        }

        protected virtual void HandleException(ErrorException ex)
        {
            //log message
            throw new FaultException(ex.Message);
        }
    }
}
