using System;
using UserManagementApplication.Common.Exceptions;

namespace UserManagementApplication.Common.Diagnostics.Interfaces
{
    public interface ILogProvider
    {
        void LogMessage(string message);
        void LogMessage(UserManagementApplicationException ex);
        void LogMessage(Exception ex);
    }
}
