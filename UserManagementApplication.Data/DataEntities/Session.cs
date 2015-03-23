using System;
using UserManagementApplication.Data.Providers.Interfaces;

namespace UserManagementApplication.Data.DataEntities
{
    public class Session
    {
        #region Properties
        public string SessionToken { get; set; }
        public User UserData { get; set; } 
        
        protected ISessionDataStorageProvider StorageProvider { get; set; }
        #endregion

        #region Constructors
        public Session(ISessionDataStorageProvider storageProvider)
        {
            StorageProvider = storageProvider;
        } 
        #endregion

        #region Methods
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

        public void RemoveSession(Session session)
        {
            if (!String.IsNullOrEmpty(session.SessionToken))
            {
                StorageProvider.RemoveSessionByToken(session.SessionToken);
            }
            else if (session.UserData != null)
            {
                StorageProvider.RemoveSessionByUsername(session.UserData.Username);
            }
        } 
        #endregion
    }
}
