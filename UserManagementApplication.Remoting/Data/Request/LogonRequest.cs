using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementApplication.Remoting.Data.Request
{
    [DataContract]
    public class LogonRequest
    {
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public SecureString Password { get; set; }
    }
}
