using System;
using System.Runtime.Serialization;
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

        [DataMember]
        public int BadLogins { get; set; }
    }
}
