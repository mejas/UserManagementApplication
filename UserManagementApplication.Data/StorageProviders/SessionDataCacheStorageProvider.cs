using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementApplication.Data.DataEntities;
using UserManagementApplication.Data.StorageProviders.Interfaces;

namespace UserManagementApplication.Data.StorageProviders
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
