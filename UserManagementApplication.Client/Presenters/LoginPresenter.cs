using System;
using UserManagementApplication.Client.Models;
using UserManagementApplication.Client.Translators;
using UserManagementApplication.Client.ViewDefinitions;
using UserManagementApplication.Common.Exceptions;

namespace UserManagementApplication.Client.Presenters
{
    public class LoginPresenter
    {
        #region Properties

        protected ILoginView View { get; set; }
        protected SessionModel Model { get; set; } 

        #endregion

        #region Constructors
        
        public LoginPresenter(ILoginView loginView)
        {
            View = loginView;
            Model = new SessionModel();
            Model.HandleModelException += Model_OnClientModelException;
            View.OnViewLoaded += View_OnViewLoaded;
        } 

        #endregion

        #region Methods

        public void Login()
        {
            var loginResult = Model.Login(View.Username, View.Password);

            if (loginResult != null)
            {
                View.SessionToken = new SessionDataTranslator().Translate(loginResult);
                View.HandleSuccessfulLogin();
            }
        } 

        #endregion

        #region Functions

        private void View_OnViewLoaded(object sender, IView e)
        {
            View.Username = String.Empty;
            View.Password = String.Empty;
        }

        private void Model_OnClientModelException(object sender, UserManagementApplicationException e)
        {
            View.HandleException(e.Message);
        } 

        #endregion
    }
}
