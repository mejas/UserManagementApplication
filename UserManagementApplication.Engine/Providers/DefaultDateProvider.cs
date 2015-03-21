using System;
using UserManagementApplication.Engine.Providers;

namespace UserManagementApplication.Engine.Providers
{
    public class DefaultDateProvider : IDateProvider
    {
        public DateTime NOW()
        {
            return DateTime.Now;
        }
    }
}
