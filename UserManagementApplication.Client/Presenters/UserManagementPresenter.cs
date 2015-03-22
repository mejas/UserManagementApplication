using System.Linq;
using UserManagementApplication.Client.Models;
using UserManagementApplication.Client.ViewData;
using UserManagementApplication.Client.ViewDefinitions;
using UserManagementApplication.Common.Enumerations;
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
            FindAllUsers();
        }

        public void Logout()
        {
            LoginModel.Logout(View.SessionToken);
            View.HandleLogout();
        }

        public void FindAllUsers()
        {
            var result = UserManagementModel.GetUsers(View.SessionToken);

            View.UpdateData(result.ToList().ConvertAll<UserData>(Translate));
        }

        private void Model_HandleModelException(object sender, UserManagementApplicationException e)
        {
            View.HandleException(e.Message);
        }

        public void SecureControls(int selectedIndex)
        {
            bool hasItem = selectedIndex != -1;

            View.EnableDelete(hasItem);
            View.EnableEdit(hasItem);
            View.EnableUnlock(hasItem);
        }

        public void EditItem()
        {
            if (View.CurrentUserData != null)
            {
                View.OnEditUser(View.CurrentUserData);
            }
        }

        public void AddUser()
        {
            View.OnAddUser(new UserData());
            FindAllUsers();
        }

        public void DeleteUser()
        {
            if (View.CurrentUserData != null)
            {
                var itemToDelete = Translate(View.CurrentUserData);

                itemToDelete.MessageState = MessageState.Deleted;

                UserManagementModel.DeleteUser(View.SessionToken, itemToDelete);
                FindAllUsers();
            }
        }

        public void UnlockUser()
        {
            if (View.CurrentUserData != null)
            {
                var user = Translate(View.CurrentUserData);

                user.BadLogins = 0;
                user.MessageState = MessageState.Modified;
                
                UserManagementModel.UnlockUser(View.SessionToken, user);
                FindAllUsers();
            }
        }

        public void FindUsers()
        {
            var result = UserManagementModel.FindUsers(View.SessionToken, View.FirstName, View.LastName);

            View.UpdateData(result.ToList().ConvertAll<UserData>(Translate));
        }

        private User Translate(UserData userData)
        {
            if (userData != null)
            {
                return new User()
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
    }
}
