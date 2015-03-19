﻿using System.Collections.Generic;

namespace UserManagementApplication.Data.Contracts.Interfaces
{
    public interface IUserDataService
    {
        IList<UserInformation> GetUsers();
        IList<UserInformation> GetUsers(string firstName, string lastName);
        UserInformation Commit(UserInformation user);
    }
}
