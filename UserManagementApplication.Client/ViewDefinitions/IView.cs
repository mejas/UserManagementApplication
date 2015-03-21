using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementApplication.Client.ViewDefinitions
{
    public interface IView
    {
        string SessionToken { get; set; }
    }
}
