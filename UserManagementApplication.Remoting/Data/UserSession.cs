using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementApplication.Remoting.Data
{
    [DataContract]
    public class UserSession
    {
        [DataMember]
        public string SessionToken { get; set; }
    }
}
