using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UserManagementApplication.Client.Data;
using UserManagementApplication.Client.Enumerations;
using UserManagementApplication.Client.Models;
using UserManagementApplication.Client.ViewDefinitions;
using UserManagementApplication.Common;
using UserManagementApplication.Common.Enumerations;
using UserManagementApplication.Remoting.Data;

namespace UserManagementApplication.Client.Presenters
{
    public class UserAddEditPresenter
    {
        protected IUserAddEditView View { get; set; }
        protected UserAddEditModel Model { get; set; }

        public UserAddEditPresenter(IUserAddEditView view)
        {
            View = view;
            View.OnViewLoaded += View_OnViewLoaded;

            Model = new UserAddEditModel();
            Model.HandleModelException += Model_HandleModelException;
        }

        public void Save()
        {
            var validationResults = validateFields();

            if (validationResults.HasErrors)
            {
                View.HandleValidationResults(validationResults);
            }
            else
            {
                var result = Model.Commit(new UserSession() { SessionToken = View.SessionToken }, getUserFromView());

                if (result != null)
                {
                    View.HandleCommitSuccess();
                }
            }
        }

        private User getUserFromView()
        {
            return new User()
            {
                Username  = View.Username,
                Password  = View.Password,
                FirstName = View.FirstName,
                LastName  = View.LastName,
                Birthdate = View.Birthdate,
                UserId    = View.UserData != null ? View.UserData.UserId : 0,
                RoleType  = View.ViewOperation == ViewOperation.Edit ? View.UserData.RoleType : RoleType.User,
                MessageState = getMessageState()
            };
        }

        private MessageState getMessageState()
        {
            if (View.ViewOperation == ViewOperation.Add)
            {
                return MessageState.New;
            }
            else if (View.ViewOperation == ViewOperation.Edit)
            {
                return MessageState.Modified;
            }

            return MessageState.Clean;   
        }

        private void View_OnViewLoaded(object sender, IView e)
        {
            if (View.ViewOperation == ViewOperation.Add)
            {
                View.ViewTitle     = "Add User";
                View.Username  = String.Empty;
                View.Password  = String.Empty;
                View.FirstName = String.Empty;
                View.LastName  = String.Empty;
                View.Birthdate = DateTime.Today;
            }
            else if (View.ViewOperation == ViewOperation.Edit)
            {
                View.ViewTitle = "Edit User";
                displayData();
            }
        }

        private void displayData()
        {
            if (View.UserData != null)
            {
                View.Username  = View.UserData.Username;
                View.Password  = View.UserData.Password;
                View.FirstName = View.UserData.FirstName;
                View.LastName  = View.UserData.LastName;
                View.Birthdate = View.UserData.Birthdate;
            }
        }

        private void Model_HandleModelException(object sender, Common.Exceptions.UserManagementApplicationException e)
        {
            View.HandleException(e.Message);
        }

        private ValidationResults validateFields()
        {
            var validationResult = new ValidationResults();

            if (View.Username == String.Empty)
            {
                validationResult.AddEntry("Username", "Username is required.");
            }

            if (View.Password == String.Empty)
            {
                validationResult.AddEntry("Password", "Password is required.");
            }

            if (View.FirstName == String.Empty)
            {
                validationResult.AddEntry("FirstName", "First Name is required.");
            }

            if (!Regex.IsMatch(View.FirstName, RegexPatterns.LETTERS_ONLY))
            {
                validationResult.AddEntry("FirstName", "Invalid first name.");
            }

            if (View.LastName == String.Empty)
            {
                validationResult.AddEntry("LastName", "Last Name is required.");
            }

            if (!Regex.IsMatch(View.LastName, RegexPatterns.LETTERS_ONLY))
            {
                validationResult.AddEntry("LastName", "Invalid last name.");
            }

            if (View.Birthdate > DateTime.Today)
            {
                validationResult.AddEntry("Username", "You haven't been born yet!");
            }

            return validationResult;
        }
    }
}
