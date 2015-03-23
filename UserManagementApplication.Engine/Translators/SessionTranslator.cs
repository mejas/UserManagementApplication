using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementApplication.Engine.Translators
{
    public class SessionTranslator
    {
        public BusinessEntities.UserSession Translate(Remoting.Data.UserSession session)
        {
            if (session != null)
            {
                var userTranslator = new UserTranslator();

                return new BusinessEntities.UserSession()
                {
                    SessionToken = session.SessionToken,
                    User = userTranslator.Translate(session.UserData)
                };
            }

            return null;
        }

        public Remoting.Data.UserSession Translate(BusinessEntities.UserSession session)
        {
            if (session != null)
            {
                var userTranslator = new UserTranslator();

                return new Remoting.Data.UserSession()
                {
                    SessionToken = session.SessionToken,
                    UserData = userTranslator.Translate(session.User)
                };
            }

            return null;
        }
    }
}
