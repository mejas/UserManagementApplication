
namespace UserManagementApplication.Common.Exceptions
{
    public class ErrorException : UserManagementApplicationException
    {
        public ErrorException(string message) :
            base(message, Enumerations.ErrorSeverity.Error)
        { }
    }
}
