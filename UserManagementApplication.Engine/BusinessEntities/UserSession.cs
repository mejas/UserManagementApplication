
using System;
using UserManagementApplication.Data.Contracts;
using UserManagementApplication.Data.Contracts.Interfaces;
using UserManagementApplication.Engine.Enumerations;
namespace UserManagementApplication.Engine.BusinessEntities
{
    public class UserSession
    {
        public string SessionToken { get; set; }

        ISessionDataService SessionDataService { get; set; }

        public UserSession()
        {

        }

        public UserSession(ISessionDataService sessionDataService)
        {
            SessionDataService = sessionDataService;
        }

        public UserSession AuthenticateUser(string username, string password)
        {
            var userSession = Translate(SessionDataService.AuthenticateUser(username, password));

            return userSession;
        }

        public bool IsPermitted(UserSession session, RoleType roleTypeToTest)
        {
            Enumerations.RoleType userRole = Translate(SessionDataService.GetSessionRoleType(Translate(session)));

            if (userRole == roleTypeToTest)
            {
                return true;
            }
        
            return Convert.ToBoolean(userRole & roleTypeToTest);
        }

        public void TerminateSession(UserSession userSession)
        {
            SessionDataService.TerminateSession(Translate(userSession));
        }

        protected Session Translate(UserSession userSession)
        {
            Session session = null;

            if (userSession != null)
            {
                session = new Session()
                {
                    SessionToken = userSession.SessionToken
                };
            }

            return session;
        }

        protected UserSession Translate(Session session)
        {
            UserSession userSession = null;

            if (session != null)
            {
                userSession = new UserSession()
                {
                    SessionToken = session.SessionToken
                };
            }

            return userSession;
        }

        protected Enumerations.RoleType Translate(Data.Contracts.DbRoleType roleType)
        {
            switch (roleType)
            {
                case Data.Contracts.DbRoleType.Administrator:
                    return Enumerations.RoleType.Admin;
                case Data.Contracts.DbRoleType.User:
                    return Enumerations.RoleType.User;
                default:
                    throw new ArgumentException("Invalid Data Contract RoleType", "roleType");
            }
        }
        
    }
}
