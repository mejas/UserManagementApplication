using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementApplication.Client.ViewData;

namespace UserManagementApplication.Client.Data
{
    public class SessionData
    {
        public string SessionToken { get; set; }
        public UserData UserData { get; set; }
    }
}
