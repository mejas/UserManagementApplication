using UserManagementApplication.Data.Providers.Interfaces;

namespace UserManagementApplication.Data.DataEntities
{
    public class Session
    {
        public string SessionToken { get; set; }
        public User UserData { get; set; }

        protected ISessionDataStorageProvider StorageProvider { get; set; }

        public Session(ISessionDataStorageProvider storageProvider)
        {
            StorageProvider = storageProvider;
        }

        public Session CreateSession(string sessionToken, User user)
        {
            Session session = new Session(StorageProvider)
            {
                SessionToken = sessionToken,
                UserData = user
            };

            var result = StorageProvider.CreateSession(sessionToken, session);

            return result;
        }

        public Session GetSession(string sessionToken)
        {
            return StorageProvider.GetSession(sessionToken);
        }

        public void RemoveSession(string sessionToken)
        {
            StorageProvider.RemoveSession(sessionToken);
        }
    }
}
