using System;

namespace UserManagementApplication.Client.ViewDefinitions
{
    public interface IView
    {
        event EventHandler<IView> OnViewLoaded;
        string SessionToken { get; set; }
        void HandleException(string message);
    }
}
