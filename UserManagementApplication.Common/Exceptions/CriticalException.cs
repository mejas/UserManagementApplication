using UserManagementApplication.Common.Enumerations;

namespace UserManagementApplication.Common.Exceptions
{
    public class CriticalException : UserManagementApplicationException
    {
        public CriticalException(string message)
            : base(message, ErrorSeverity.Critical)
        { }
    }
}
