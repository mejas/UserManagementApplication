
namespace UserManagementApplication.Data.Contracts.Interfaces
{
    public interface IAuthenticationDataService
    {
        void StoreSession(UserSessionInformation userData);
        UserSessionInformation GetUserSession(string sessionToken);
        void RemoveSession(string sessionToken);
        bool Authenticate(UserInformation userInformation, string password);
    }
}
