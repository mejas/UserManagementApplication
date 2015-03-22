using System.Collections.Generic;
using System.Linq;
using UserManagementApplication.Common.Enumerations;
using UserManagementApplication.Remoting.Data;
using UserManagementApplication.Remoting.Data.Request;
using UserManagementApplication.Remoting.Services;
using EBC = UserManagementApplication.Engine.BusinessEntities;

namespace UserManagementApplication.Engine.Services
{
    public class UserServices : RemotingServiceBase, IUserServices
    {
        public IList<User> GetUsers(UserSession session)
        {
            return InvokeMethod(() =>
            {
                EBC.User user = new EBC.User();

                return user.Find(Translate(session)).ToList().ConvertAll<User>(Translate);
            });
        }

        public IList<User> FindUsers(UserSession session, FindUserRequest request)
        {
            return InvokeMethod(() =>
            {
                EBC.User user = new EBC.User();

                return user.Find(Translate(session), request.FirstName, request.LastName).ToList().ConvertAll<User>(Translate);
            });
        }

        public User Commit(UserSession session, User user)
        {
            return InvokeMethod(() =>
            {
                EBC.User ebcUser = new EBC.User();

                switch (user.MessageState)
                {
                    case MessageState.New:
                        {
                            var result = ebcUser.Create(Translate(session),
                                                        user.Username,
                                                        user.Password,
                                                        user.FirstName,
                                                        user.LastName,
                                                        user.Birthdate,
                                                        user.RoleType);
                            return Translate(result);
                        }
                    case MessageState.Modified:
                        {
                            var result = ebcUser.Update(Translate(session), Translate(user));

                            return Translate(result);
                        }
                    case MessageState.Deleted:
                        {
                            ebcUser.Remove(Translate(session), Translate(user));

                            return null;
                        }
                    default:
                        return user;
                }
            });
        }

        protected EBC.UserSession Translate(UserSession session)
        {
            if (session != null)
            {
                var userSession = new EBC.UserSession() { SessionToken = session.SessionToken };

                return userSession.GetUserSession(userSession);
            }

            return null;
        }

        protected User Translate(EBC.User user)
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
                    Username     = user.Username,
                    BadLogins    = user.BadLogins
                };
            }

            return null;
        }

        protected EBC.User Translate(User user)
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
                    Username  = user.Username,
                    BadLogins = user.BadLogins
                };
            }

            return null;
        }
    }
}
