﻿using System;
using UserManagementApplication.Common.Enumerations;

namespace UserManagementApplication.Data.Contracts
{
    public class UserInformation
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public int UserId { get; set; }
        public DataState DataState { get; set; }
        public RoleType RoleType { get; set; }
        public int BadLogins { get; set; }
    }
}
