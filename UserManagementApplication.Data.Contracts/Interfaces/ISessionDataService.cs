using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementApplication.Data.Contracts.Interfaces
{
    public interface ISessionDataService
    {
        Session AuthenticateUser(string username, string password);
        void TerminateSession(Session session);
        DbRoleType GetSessionRoleType(Session session);
    }
}
