﻿using UserManagementApplication.Common.Enumerations;
using UserManagementApplication.Engine.BusinessEntities;

namespace UserManagementApplication.Engine.Providers.Interfaces
{
    public interface IAuthenticationProvider
    {
        UserSession CreateSession(string username, string password);
        UserSession GetSession(string sessionToken);
        bool HasPermission(UserSession session, RoleType roleTypeToTest);
        void TerminateSession(UserSession userSession);
    }
}
