using System.Runtime.Serialization;

namespace UserManagementApplication.Remoting.Data.Request
{
    [DataContract]
    public class FindUserRequest
    {
        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }
    }
}
