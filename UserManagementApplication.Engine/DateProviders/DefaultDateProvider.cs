using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementApplication.Engine.Providers;

namespace UserManagementApplication.Engine.DateProviders
{
    public class DefaultDateProvider : IDateProvider
    {
        public DateTime NOW()
        {
            return DateTime.Now;
        }
    }
}
