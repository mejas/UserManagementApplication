﻿using System.Collections.Generic;
using System.Linq;
using UserManagementApplication.Data.DataEntities;
using UserManagementApplication.Data.Providers.Interfaces;

namespace UserManagementApplication.Data.Providers
{
    public class SessionDataCacheStorageProvider : ISessionDataStorageProvider
    {
        #region Declarations
        private Dictionary<string, Session> _sessions = new Dictionary<string, Session>();
        #endregion

        #region Methods
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

        public void RemoveSessionByToken(string sessionToken)
        {
            Session session;

            if (_sessions.TryGetValue(sessionToken, out session))
            {
                _sessions.Remove(sessionToken);
            }
        }

        public void RemoveSessionByUsername(string username)
        {
            string sessionKey = _sessions.Where(item => item.Value.UserData.Username == username).Select(p => p.Key).FirstOrDefault();

            RemoveSessionByToken(sessionKey);
        } 
        #endregion
    }
}
