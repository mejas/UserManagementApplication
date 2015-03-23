using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementApplication.Engine.Translators
{
    public class UserTranslator
    {
        public Remoting.Data.User Translate(BusinessEntities.User user)
        {
            if (user != null)
            {
                return new Remoting.Data.User()
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

        public BusinessEntities.User Translate(Remoting.Data.User user)
        {
            if (user != null)
            {
                return new BusinessEntities.User()
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

        public IList<Remoting.Data.User> Translate(IList<BusinessEntities.User> userList)
        {
            return userList.ToList().ConvertAll<Remoting.Data.User>(Translate);
        }
    }
}
