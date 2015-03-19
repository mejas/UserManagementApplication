using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementApplication.Common.Enumerations;

namespace UserManagementApplication.Common.Exceptions
{
    public class UserManagementApplicationException : ApplicationException
    {
        public ErrorSeverity ErrorSeverity { get; private set; }

        public UserManagementApplicationException(string message, ErrorSeverity errorSeverity)
            : base(message)
        { }
    }
}
