
namespace UserManagementApplication.Common.Exceptions
{
    public class UnauthorizedOperationException : ErrorException
    {
        public UnauthorizedOperationException(string message) :
            base(message)
        { }
    }
}
