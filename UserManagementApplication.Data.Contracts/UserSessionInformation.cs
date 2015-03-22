using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementApplication.Data.Contracts
{
    public class UserSessionInformation
    {
        public string SessionToken { get; set; }
        public UserInformation User { get; set; }
    }
}
