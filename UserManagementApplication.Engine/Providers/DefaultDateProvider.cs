using System;

namespace UserManagementApplication.Engine.Providers
{
    public class DefaultDateProvider : IDateProvider
    {
        public DateTime NOW()
        {
            return DateTime.Now.Date;
        }
    }
}
