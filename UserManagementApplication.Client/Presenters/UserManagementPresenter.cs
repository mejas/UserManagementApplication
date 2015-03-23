using System;
using UserManagementApplication.Client.Models;
using UserManagementApplication.Client.Translators;
using UserManagementApplication.Client.ViewData;
using UserManagementApplication.Client.ViewDefinitions;
using UserManagementApplication.Common.Enumerations;
using UserManagementApplication.Common.Exceptions;

namespace UserManagementApplication.Client.Presenters
{
    public class UserManagementPresenter
    {
        #region Properties
        protected IUserManagementView View { get; set; }
        protected UserManagementModel UserManagementModel { get; set; }
        protected SessionModel SessionModel { get; set; } 
        #endregion

        #region Constructors
        public UserManagementPresenter(IUserManagementView view)
        {
            View = view;
            View.OnViewLoaded += View_OnViewLoaded;

            UserManagementModel = new UserManagementModel();
            UserManagementModel.HandleModelException += Model_HandleModelException;

            SessionModel = new SessionModel();
            SessionModel.HandleModelException += Model_HandleModelException;
        } 
        #endregion

        #region Methods
        public void Logout()
        {
            SessionModel.Logout(new SessionDataTranslator().Translate(View.SessionToken));
            View.HandleLogout();
        }

        public void FindAllUsers()
        {
            var result = UserManagementModel.GetUsers(new SessionDataTranslator().Translate(View.SessionToken));

            View.UpdateData(new UserDataTranslator().Translate(result));
        }

        public void SecureControls()
        {
            bool hasItem = View.CurrentUserData != null;
            bool isAdminRole = View.SessionToken.UserData.RoleType == RoleType.Admin;

            View.EnableAdd(true);
            View.EnableDelete(hasItem);
            View.EnableEdit(hasItem);
            View.EnableUnlock(hasItem && isAdminRole);
        }

        public void EditUser()
        {
            if (View.CurrentUserData != null)
            {
                View.OnEditUser(View.CurrentUserData);
                FindAllUsers();
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
                var itemToDelete = new UserDataTranslator().Translate(View.CurrentUserData);

                itemToDelete.MessageState = MessageState.Deleted;

                UserManagementModel.DeleteUser(new SessionDataTranslator().Translate(View.SessionToken), itemToDelete);
                FindAllUsers();
            }
        }

        public void UnlockUser()
        {
            if (View.CurrentUserData != null)
            {
                var user = new UserDataTranslator().Translate(View.CurrentUserData);

                user.BadLogins = 0;
                user.MessageState = MessageState.Modified;

                UserManagementModel.UnlockUser(new SessionDataTranslator().Translate(View.SessionToken), user);
                FindAllUsers();
            }
        }

        public void FindUsers()
        {
            var result = UserManagementModel.FindUsers(new SessionDataTranslator().Translate(View.SessionToken), View.FirstName, View.LastName);

            View.UpdateData(new UserDataTranslator().Translate(result));
        } 
        #endregion

        #region Functions
        private void View_OnViewLoaded(object sender, IView e)
        {
            View.ViewTitle = String.Format("{0} - [{1}] - {2}", View.ViewTitle, View.SessionToken.UserData.RoleType, View.SessionToken.UserData.Username);
            FindAllUsers();
            SecureControls();
        }

        private void Model_HandleModelException(object sender, UserManagementApplicationException e)
        {
            View.HandleException(e.Message);
        } 
        #endregion
    }
}
