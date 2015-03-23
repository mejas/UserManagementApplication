﻿using System.Collections.Generic;
using UserManagementApplication.Client.Models.ServiceProxy;
using UserManagementApplication.Remoting.Data;
using UserManagementApplication.Remoting.Data.Request;

namespace UserManagementApplication.Client.Models
{
    public class UserManagementModel : ClientModel
    {
        protected UserServiceProxy SessionProxy { get; set; }

        public UserManagementModel()
        {
            SessionProxy = new UserServiceProxy();
        }

        public IList<User> GetUsers(UserSession sessionToken)
        {
            return InvokeMethod(() => SessionProxy.GetUsers(sessionToken));
        }

        public void DeleteUser(UserSession sessionToken, User user)
        {
            InvokeMethod(() => SessionProxy.Commit(sessionToken, user));
        }

        public void UnlockUser(UserSession sessionToken, User user)
        {
            InvokeMethod(() => SessionProxy.Commit(sessionToken, user));
        }

        public IList<User> FindUsers(UserSession sessionToken, string firstName, string lastName)
        {
            return InvokeMethod(
                () => SessionProxy.FindUsers(sessionToken, 
                                             new FindUserRequest() { FirstName = firstName, LastName = lastName }));
        }
    }
}
