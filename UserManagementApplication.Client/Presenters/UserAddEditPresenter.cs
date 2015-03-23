using System;
using System.Text.RegularExpressions;
using UserManagementApplication.Client.Data;
using UserManagementApplication.Client.Enumerations;
using UserManagementApplication.Client.Models;
using UserManagementApplication.Client.Translators;
using UserManagementApplication.Client.ViewData;
using UserManagementApplication.Client.ViewDefinitions;
using UserManagementApplication.Common;
using UserManagementApplication.Common.Enumerations;
using UserManagementApplication.Common.Security;

namespace UserManagementApplication.Client.Presenters
{
    public class UserAddEditPresenter
    {
        #region Properties
        protected IUserAddEditView View { get; set; }
        protected UserAddEditModel Model { get; set; } 
        #endregion

        #region Constructors
        public UserAddEditPresenter(IUserAddEditView view)
        {
            View = view;
            View.OnViewLoaded += View_OnViewLoaded;

            Model = new UserAddEditModel();
            Model.HandleModelException += Model_HandleModelException;
        } 
        #endregion

        #region Methods
        public void Save()
        {
            var validationResults = validateFields();

            if (validationResults.HasErrors)
            {
                View.HandleValidationResults(validationResults);
            }
            else
            {
                var itemToCommit = new UserDataTranslator().Translate(getUserFromView());

                itemToCommit.MessageState = getMessageState();

                var result = Model.Commit(new SessionDataTranslator().Translate(View.SessionToken),
                                            itemToCommit);

                if (result != null)
                {
                    View.HandleCommitSuccess();
                }
            }
        } 
        #endregion

        #region Functions
        private UserData getUserFromView()
        {
            var data = new UserData()
            {
                Username = View.Username,
                FirstName = View.FirstName,
                LastName = View.LastName,
                Birthdate = View.Birthdate,
                UserId = View.UserData != null ? View.UserData.UserId : 0,
                RoleType = View.ViewOperation == ViewOperation.Edit ? View.UserData.RoleType : RoleType.User
            };

            if (View.UserData.Password != View.Password)
            {
                data.Password = new HashGenerator().GenerateHash(View.Password);
            }

            return data;
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
                View.ViewTitle = "Add User";
                View.Username = String.Empty;
                View.Password = String.Empty;
                View.FirstName = String.Empty;
                View.LastName = String.Empty;
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
                View.Username = View.UserData.Username;
                View.Password = View.UserData.Password;
                View.FirstName = View.UserData.FirstName;
                View.LastName = View.UserData.LastName;
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
        #endregion
    }
}
