
namespace UserManagementApplication.Common.Exceptions
{
    public class WarningException : UserManagementApplicationException
    {
        public WarningException(string message) :
            base(message, Enumerations.ErrorSeverity.Warning)
        { }
    }
}
