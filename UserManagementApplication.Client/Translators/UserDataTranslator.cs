using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementApplication.Client.Translators
{
    public class UserDataTranslator
    {
        public Remoting.Data.User Translate(Client.ViewData.UserData userData)
        {
            if (userData != null)
            {
                return new Remoting.Data.User()
                {
                    Age       = userData.Age,
                    Birthdate = userData.Birthdate,
                    FirstName = userData.FirstName,
                    LastName  = userData.LastName,
                    Password  = userData.Password,
                    RoleType  = userData.RoleType,
                    UserId    = userData.UserId,
                    Username  = userData.Username,
                    BadLogins = userData.BadLogins
                };
            }

            return null;
        }

        public Client.ViewData.UserData Translate(Remoting.Data.User user)
        {
            if (user != null)
            {
                return new Client.ViewData.UserData()
                {
                    Age       = user.Age,
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

        public IList<ViewData.UserData> Translate(IList<Remoting.Data.User> result)
        {
            return result.ToList().ConvertAll<ViewData.UserData>(Translate);
        }
    }
}
