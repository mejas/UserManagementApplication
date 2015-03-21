using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementApplication.Common.Exceptions
{
    public class InvalidSessionException : ErrorException
    {
        public InvalidSessionException(string message)
            : base(message)
        { }
    }
}
