using System.Runtime.Serialization;

namespace UserManagementApplication.Remoting.Data
{
    [DataContract]
    public class UserSession
    {
        [DataMember]
        public string SessionToken { get; set; }

        [DataMember]
        public User UserData { get; set; }
    }
}
