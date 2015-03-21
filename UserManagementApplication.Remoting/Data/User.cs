using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UserManagementApplication.Common.Enumerations;

namespace UserManagementApplication.Remoting.Data
{
    [DataContract]
    public class User
    {
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public DateTime Birthdate { get; set; }

        [DataMember]
        public int Age { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public RoleType RoleType { get; set; }

        [DataMember]
        public MessageState MessageState { get; set; }
    }
}
