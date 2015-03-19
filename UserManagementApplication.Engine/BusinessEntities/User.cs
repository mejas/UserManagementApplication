﻿using System;
using System.Collections.Generic;
using System.Linq;
using UserManagementApplication.Data.Contracts;
using UserManagementApplication.Data.Contracts.Interfaces;
using UserManagementApplication.Engine.Providers;

namespace UserManagementApplication.Engine.BusinessEntities
{
    public class User
    {
        #region Properties

        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public int Age
        {
            get
            {
                if (Birthdate != DateTime.MinValue)
                {
                    return DateProvider.NOW().Year - Birthdate.Year;
                }

                return 0;
            }
        }
        public int UserId { get; set; }
        public Enumerations.RoleType RoleType { get; set; }

        protected IDateProvider DateProvider { get; set; }
        protected IUserDataService UserDataService { get; set; }

        #endregion

        #region Constructor

        public User()
        {

        }

        public User(IDateProvider dateProvider, IUserDataService userDataService)
        {
            DateProvider = dateProvider;
            UserDataService = userDataService;
        }

        #endregion

        #region Methods

        public IList<User> Find()
        {
            var users = UserDataService.GetUsers();

            return users.ToList().ConvertAll(Translate);
        }

        public IList<User> Find(string firstName, string lastName)
        {
            var users = UserDataService.GetUsers(firstName, lastName);

            return users.ToList().ConvertAll(Translate);
        }

        public User Create( string username,
                            string password,
                            string firstName, 
                            string lastName, 
                            DateTime birthDate,
                            Enumerations.RoleType roleType = Enumerations.RoleType.User)
        {
            UserInformation user = new UserInformation()
            {
                Username  = username,
                Password  = password,
                FirstName = firstName,
                LastName  = lastName,
                Birthdate = birthDate,
                DataState = DataState.New,
                RoleType  = Translate(roleType)
            };

            user = UserDataService.Commit(user);

            return Translate(user);
        }

        public User Update(User userInfo)
        {
            var user = Translate(userInfo);

            user.DataState = DataState.Modified;

            user = UserDataService.Commit(user);

            return Translate(user);
        }

        public void Remove(User userInfo)
        {
            var user = Translate(userInfo);

            user.DataState = DataState.Deleted;

            UserDataService.Commit(user);
        }

        #endregion

        #region Functions

        protected User Translate(UserInformation user)
        {
            User userInfo = null;

            if (user != null)
            {
                userInfo = new User()
                {
                    Username  = user.Username,
                    Password  = user.Password,
                    FirstName = user.FirstName,
                    LastName  = user.LastName,
                    Birthdate = user.Birthdate,
                    UserId    = user.UserId
                };
            }

            return userInfo;
        }

        protected UserInformation Translate(User userInfo)
        {
            UserInformation user = null;

            if (userInfo != null)
            {
                user = new UserInformation()
                {
                    Username  = userInfo.Username,
                    Password  = userInfo.Password,
                    FirstName = userInfo.FirstName,
                    LastName  = userInfo.LastName,
                    Birthdate = userInfo.Birthdate,
                    UserId    = userInfo.UserId,
                    DataState = DataState.Clean
                };
            }

            return user;
        }

        protected Enumerations.RoleType Translate(Data.Contracts.DbRoleType roleType)
        {
            switch (roleType)
            {
                case Data.Contracts.DbRoleType.Administrator:
                    return Enumerations.RoleType.Admin;
                case Data.Contracts.DbRoleType.User:
                    return Enumerations.RoleType.User;
                default:
                    throw new ArgumentException("Invalid Data Contract RoleType", "roleType");
            }
        }

        protected Data.Contracts.DbRoleType Translate(Enumerations.RoleType roleType)
        {
            switch (roleType)
            {
                case Enumerations.RoleType.Admin:
                    return Data.Contracts.DbRoleType.Administrator;
                case Enumerations.RoleType.User:
                    return Data.Contracts.DbRoleType.User;
                default:
                    throw new ArgumentException("Invalid BusinessEngine RoleType.", "roleType");
            }
        }
        
        #endregion
    }
}
