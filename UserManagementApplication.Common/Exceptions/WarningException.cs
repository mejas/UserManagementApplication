using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementApplication.Common.Exceptions
{
    public class WarningException : UserManagementApplicationException
    {
        public WarningException(string message) :
            base(message, Enumerations.ErrorSeverity.Warning)
        { }
    }
}
