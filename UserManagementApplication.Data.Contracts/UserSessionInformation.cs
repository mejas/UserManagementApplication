
namespace UserManagementApplication.Data.Contracts
{
    public class UserSessionInformation
    {
        public string SessionToken { get; set; }
        public UserInformation User { get; set; }
    }
}
