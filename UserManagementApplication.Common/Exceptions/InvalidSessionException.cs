
namespace UserManagementApplication.Common.Exceptions
{
    public class InvalidSessionException : ErrorException
    {
        public InvalidSessionException(string message)
            : base(message)
        { }
    }
}
