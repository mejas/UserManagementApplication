using UserManagementApplication.Data.DataEntities;

namespace UserManagementApplication.Data.Providers.Interfaces
{
    public interface ISessionDataStorageProvider
    {
        Session GetSession(string sessionToken);
        Session CreateSession(string sessionToken, Session session);
        void RemoveSession(string sessionToken);
    }
}
