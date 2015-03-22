
namespace UserManagementApplication.Client.ViewDefinitions
{
    public interface ILoginView : IView
    {
        string Username { get; set; }
        string Password { get; set; }

        void HandleSuccessfulLogin();
    }
}
