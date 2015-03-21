using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementApplication.Client.ViewData;

namespace UserManagementApplication.Client.ViewDefinitions
{
    public interface IUserManagementView : IView
    {
        void UpdateData(IList<UserData> userData);
    }
}
