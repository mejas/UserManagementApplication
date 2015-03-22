using System.Collections.Generic;
using UserManagementApplication.Client.ViewData;

namespace UserManagementApplication.Client.ViewDefinitions
{
    public interface IUserManagementView : IView
    {
        UserData CurrentUserData { get; }

        void UpdateData(IList<UserData> userData);

        void HandleLogout();

        void EnableAdd(bool value);
        void EnableEdit(bool value);
        void EnableDelete(bool value);
        void EnableUnlock(bool value);

        void OnAddItem(UserData userData);
        void OnEditItem(UserData userData);
    }
}
