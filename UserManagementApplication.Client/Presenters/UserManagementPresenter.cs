using System.Linq;
using UserManagementApplication.Client.Models;
using UserManagementApplication.Client.ViewData;
using UserManagementApplication.Client.ViewDefinitions;
using UserManagementApplication.Common.Exceptions;
using UserManagementApplication.Remoting.Data;

namespace UserManagementApplication.Client.Presenters
{
    public class UserManagementPresenter
    {
        protected IUserManagementView View { get; set; }
        protected UserManagementModel UserManagementModel { get; set; }
        protected SessionModel LoginModel { get; set; }

        public UserManagementPresenter(IUserManagementView view)
        {
            View = view;
            View.OnViewLoaded += View_OnViewLoaded;

            UserManagementModel = new UserManagementModel();
            UserManagementModel.HandleModelException += Model_HandleModelException;
            
            LoginModel = new SessionModel();
            LoginModel.HandleModelException += Model_HandleModelException;
        }

        void View_OnViewLoaded(object sender, IView e)
        {
            RefreshData();
        }

        public void Logout()
        {
            LoginModel.Logout(View.SessionToken);
            View.HandleLogout();
        }

        public void RefreshData()
        {
            var result = UserManagementModel.GetUsers(View.SessionToken);

            View.UpdateData(result.ToList().ConvertAll<UserData>(Translate));
        }

        private void Model_HandleModelException(object sender, UserManagementApplicationException e)
        {
            View.HandleException(e.Message);
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
                    Username  = user.Username,
                    BadLogins = user.BadLogins
                };
            }

            return null;
        }

        public void SecureControls(int selectedIndex)
        {
            bool hasItem = selectedIndex != -1;

            View.EnableDelete(hasItem);
            View.EnableEdit(hasItem);
            View.EnableUnlock(hasItem);
        }
    }
}
