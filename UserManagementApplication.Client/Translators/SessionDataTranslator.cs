using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementApplication.Client.Translators
{
    public class SessionDataTranslator
    {
        public Data.SessionData Translate(Remoting.Data.UserSession userSession)
        {
            if (userSession != null)
            {
                var userTranslator = new UserDataTranslator();

                return new Data.SessionData()
                {
                    SessionToken = userSession.SessionToken,
                    UserData     = userTranslator.Translate(userSession.UserData)
                };
            }

            return null;
        }

        public Remoting.Data.UserSession Translate(Data.SessionData userSession)
        {
            if (userSession != null)
            {
                var userTranslator = new UserDataTranslator();

                return new Remoting.Data.UserSession()
                {
                    SessionToken = userSession.SessionToken,
                    UserData     = userTranslator.Translate(userSession.UserData)
                };
            }

            return null;
        }
    }
}
