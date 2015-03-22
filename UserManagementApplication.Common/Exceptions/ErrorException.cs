
using UserManagementApplication.Common.Enumerations;

namespace UserManagementApplication.Common.Exceptions
{
    public class ErrorException : UserManagementApplicationException
    {
        public ErrorException(string message) :
            base(message, ErrorSeverity.Error)
        { }
    }
}
