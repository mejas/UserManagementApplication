using System;
using UserManagementApplication.Common.Enumerations;

namespace UserManagementApplication.Common.Exceptions
{
    public class UserManagementApplicationException : ApplicationException
    {
        public ErrorSeverity ErrorSeverity { get; private set; }

        public UserManagementApplicationException(string message, ErrorSeverity errorSeverity)
            : base(message)
        {
            ErrorSeverity = errorSeverity;
        }
    }
}
