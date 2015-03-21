using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementApplication.Client.Models;
using UserManagementApplication.Client.ViewData;
using UserManagementApplication.Client.ViewDefinitions;
using UserManagementApplication.Remoting.Data;

namespace UserManagementApplication.Client.Presenters
{
    public class UserManagementPresenter
    {
        protected IUserManagementView View { get; set; }
        protected UserManagementModel Model { get; set; }

        public UserManagementPresenter(IUserManagementView view)
        {
            View = view;
            View.OnViewLoaded += View_OnViewLoaded;
            Model = new UserManagementModel();
            Model.HandleModelException += Model_HandleModelException;
        }

        void View_OnViewLoaded(object sender, IView e)
        {
            var result = Model.GetUsers(View.SessionToken);

            View.UpdateData(result.ToList().ConvertAll<UserData>(Translate));
        }

        private UserData Translate(User user)
        {
            if (user != null)
            {
                return new UserData()
                {
                    Age       = user.Age,
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

        void Model_HandleModelException(object sender, Common.Exceptions.UserManagementApplicationException e)
        {
            throw new NotImplementedException();
        }


    }
}
