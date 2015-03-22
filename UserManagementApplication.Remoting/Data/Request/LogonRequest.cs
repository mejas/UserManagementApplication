using System.Runtime.Serialization;

namespace UserManagementApplication.Remoting.Data.Request
{
    [DataContract]
    public class LogonRequest
    {
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}
