
namespace UserManagementApplication.Data.Contracts.Interfaces
{
    public interface IAuthenticationDataService
    {
        void StoreSession(UserSessionInformation userData);
        UserSessionInformation GetUserSession(string sessionToken);
        void RemoveSession(UserSessionInformation userSessionInfo);
        bool Authenticate(UserInformation userSessionInfo, string password);
    }
}
