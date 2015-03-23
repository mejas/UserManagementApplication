using System;
using UserManagementApplication.Client.Data;

namespace UserManagementApplication.Client.ViewDefinitions
{
    public interface IView
    {
        SessionData SessionToken { get; set; }
        
        event EventHandler<IView> OnViewLoaded;
        void HandleException(string message);
    }
}
