using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementApplication.Client.Models;
using UserManagementApplication.Client.ViewDefinitions;
using UserManagementApplication.Common.Exceptions;

namespace UserManagementApplication.Client.Presenters
{
    public class LoginPresenter
    {
        protected ILoginView View { get; set; }
        protected LoginModel Model { get; set; }

        public LoginPresenter(ILoginView loginView)
        {
            View = loginView;
            Model = new LoginModel();
            Model.HandleModelException += Model_OnClientModelException;
        }

        public void Login()
        {
            if (Model.Login(View.Username, View.Password))
            {
                View.SessionToken = Model.GetSessionToken();
                View.HandleSuccessfulLogin();
            }
        }

        void Model_OnClientModelException(object sender, UserManagementApplicationException e)
        {
            View.HandleLoginMessage(e.Message);
        }
    }
}
