using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementApplication.Common.Exceptions
{
    public class ValidationException : WarningException
    {
        public ValidationException(string message)
            : base(message)
        { }
    }
}
