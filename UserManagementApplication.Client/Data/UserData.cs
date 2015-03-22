using System;
using UserManagementApplication.Common.Enumerations;

namespace UserManagementApplication.Client.ViewData
{
    public class UserData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public int Age { get; set; }
        public int UserId { get; set; }
        public RoleType RoleType { get; set; }
        public int BadLogins { get; set; }
    }
}
