using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementApplication.Common.Enumerations;
using UserManagementApplication.Common.Exceptions;
using UserManagementApplication.Remoting.Data;
using UserManagementApplication.Remoting.Data.Request;
using UserManagementApplication.Remoting.Services;
using EBC = UserManagementApplication.Engine.BusinessEntities;

namespace UserManagementApplication.Engine.Services
{
    public class UserServices : IUserServices
    {
        public IList<User> GetUsers(UserSession session)
        {
            verifyUserPermissions(session);

            EBC.User user = new EBC.User();

            return user.Find().ToList().ConvertAll<User>(Translate);
        }

        public IList<User> FindUsers(UserSession session, FindUserRequest request)
        {
            verifyUserPermissions(session);

            EBC.User user = new EBC.User();

            return user.Find().ToList().ConvertAll<User>(Translate);
        }

        public User Commit(UserSession session, User user)
        {
            verifyUserPermissions(session);

            EBC.User ebcUser = new EBC.User();

            switch (user.MessageState)
            {
                case MessageState.New:
                    {
                        var result = ebcUser.Create(user.Username,
                                                    user.Password,
                                                    user.FirstName,
                                                    user.LastName,
                                                    user.Birthdate,
                                                    user.RoleType);
                        return Translate(result);
                    }
                case MessageState.Modified:
                    {
                        var result = ebcUser.Update(Translate(user));

                        return Translate(result);
                    }
                case MessageState.Deleted:
                    {
                        ebcUser.Remove(Translate(user));

                        return null;
                    }
                default:
                    return user;
            }
        }

        private void verifyUserPermissions(UserSession session)
        {
            EBC.UserSession userSession = new EBC.UserSession();

            if (!userSession.IsPermitted(Translate(session), Common.Enumerations.RoleType.User))
            {
                throw new ErrorException("User is not authorized for this operation!");
            }
        }

        private EBC.UserSession Translate(UserSession session)
        {
            if (session != null)
            {
                return new EBC.UserSession()
                {
                    SessionToken = session.SessionToken
                };
            }

            return null;
        }

        private User Translate(EBC.User user)
        {
            if (user != null)
            {
                return new User()
                {
                    Age          = user.Age,
                    Birthdate    = user.Birthdate,
                    FirstName    = user.FirstName,
                    LastName     = user.LastName,
                    MessageState = Common.Enumerations.MessageState.Clean,
                    Password     = user.Password,
                    RoleType     = user.RoleType,
                    UserId       = user.UserId,
                    Username     = user.Username
                };
            }

            return null;
        }

        private EBC.User Translate(User user)
        {
            if (user != null)
            {
                return new EBC.User()
                {
                    Birthdate = user.Birthdate,
                    FirstName = user.FirstName,
                    LastName  = user.LastName,
                    Password  = user.Password,
                    RoleType  = user.RoleType,
                    UserId    = user.UserId,
                    Username  = user.Username
                };
            }

            return null;
        }
    }
}
