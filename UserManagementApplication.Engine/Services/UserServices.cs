using System.Collections.Generic;
using System.Linq;
using UserManagementApplication.Common.Enumerations;
using UserManagementApplication.Engine.Translators;
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

                var result = user.Find(new SessionTranslator().Translate(session));

                return new UserTranslator().Translate(result);
            });
        }

        public IList<User> FindUsers(UserSession session, FindUserRequest request)
        {
            return InvokeMethod(() =>
            {
                EBC.User user = new EBC.User();

                var result = user.Find(new SessionTranslator().Translate(session), request.FirstName, request.LastName);

                return new UserTranslator().Translate(result);
            });
        }

        public User Commit(UserSession session, User user)
        {
            return InvokeMethod(() =>
            {
                var translatedSession = new SessionTranslator().Translate(session);

                var userTranslator = new UserTranslator();
                var translatedUser = userTranslator.Translate(user);

                EBC.User ebcUser = new EBC.User();
                EBC.User result = null;

                switch (user.MessageState)
                {
                    case MessageState.New:
                        {
                            result = ebcUser.Create(translatedSession, translatedUser);
                            break;
                        }
                    case MessageState.Modified:
                        {
                            result = ebcUser.Update(translatedSession, translatedUser);
                            break;
                        }
                    case MessageState.Deleted:
                        {
                            ebcUser.Remove(translatedSession, translatedUser);

                            result = null;
                            break;
                        }
                }

                return userTranslator.Translate(result);

            });
        }
    }
}
