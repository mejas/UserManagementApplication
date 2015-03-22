using System.Collections.Generic;
using UserManagementApplication.Data.DataEntities;
using UserManagementApplication.Data.Providers.Interfaces;

namespace UserManagementApplication.Data.Providers
{
    public class SessionDataCacheStorageProvider : ISessionDataStorageProvider
    {
        private Dictionary<string, Session> _sessions = new Dictionary<string, Session>();

        public Session GetSession(string sessionToken)
        {
            Session session = null;

            _sessions.TryGetValue(sessionToken, out session);

            return session;
        }

        public Session CreateSession(string sessionToken, Session session)
        {
            _sessions[sessionToken] = session;

            return session;
        }

        public void RemoveSession(string sessionToken)
        {
            Session session;

            if (_sessions.TryGetValue(sessionToken, out session))
            {
                _sessions.Remove(sessionToken);
            }
        }
    }
}
