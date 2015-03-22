
namespace UserManagementApplication.Common.Exceptions
{
    public class ValidationException : WarningException
    {
        public ValidationException(string message)
            : base(message)
        { }
    }
}
