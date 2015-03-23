﻿using System.Linq;
using UserManagementApplication.Client.Models;
using UserManagementApplication.Client.Translators;
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
        protected SessionModel SessionModel { get; set; }

        public UserManagementPresenter(IUserManagementView view)
        {
            View = view;
            View.OnViewLoaded += View_OnViewLoaded;

            UserManagementModel = new UserManagementModel();
            UserManagementModel.HandleModelException += Model_HandleModelException;
            
            SessionModel = new SessionModel();
            SessionModel.HandleModelException += Model_HandleModelException;
        }

        void View_OnViewLoaded(object sender, IView e)
        {
            FindAllUsers();
            SecureControls();
        }

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

            View.EnableAdd(isAdminRole);

            bool enableOp = hasItem && isAdminRole;

            View.EnableDelete(enableOp);
            View.EnableEdit(enableOp);
            View.EnableUnlock(enableOp);
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

            View.UpdateData(result.ToList().ConvertAll<UserData>(new UserDataTranslator().Translate));
        }

        private void Model_HandleModelException(object sender, UserManagementApplicationException e)
        {
            View.HandleException(e.Message);
        }
    }
}
