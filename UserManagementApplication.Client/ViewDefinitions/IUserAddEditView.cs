using System;
using UserManagementApplication.Client.Data;
using UserManagementApplication.Client.Enumerations;
using UserManagementApplication.Client.ViewData;

namespace UserManagementApplication.Client.ViewDefinitions
{
    public interface IUserAddEditView : IView
    {
        ViewOperation ViewOperation { get; set; }
        string ViewTitle { get; set; }
        UserData UserData { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        DateTime Birthdate { get; set; }

        void HandleValidationResults(ValidationResults validationResults);
        void HandleCommitSuccess();
    }
}
