using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementApplication.Client.ViewDefinitions
{
    public interface ILoginView : IView
    {
        string Username { get; set; }
        string Password { get; set; }

        void HandleLoginMessage(string message);

        void HandleSuccessfulLogin();
    }
}
