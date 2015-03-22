using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementApplication.Common.Exceptions
{
    public class UnauthorizedOperationException : ErrorException
    {
        public UnauthorizedOperationException(string message) :
            base(message)
        { }
    }
}
