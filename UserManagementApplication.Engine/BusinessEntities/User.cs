using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UserManagementApplication.Common;
using UserManagementApplication.Common.Enumerations;
using UserManagementApplication.Common.Exceptions;
using UserManagementApplication.Data.Contracts;
using UserManagementApplication.Data.Contracts.Interfaces;
using UserManagementApplication.Data.Services;
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
        public RoleType RoleType { get; set; }
        public int BadLogins { get; set; }

        protected IDateProvider DateProvider { get; set; }
        protected IUserDataService UserDataService { get; set; }

        #endregion

        #region Constructor

        public User()
            : this(new DefaultDateProvider(), new UserDataServices())
        { }

        public User(IDateProvider dateProvider, IUserDataService userDataService)
        {
            DateProvider = dateProvider;
            UserDataService = userDataService;

            RoleType = RoleType.User;
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

        public User GetUser(string username)
        {
            return Translate(UserDataService.GetUser(username));
        }

        public User Create( string username,
                            string password,
                            string firstName, 
                            string lastName, 
                            DateTime birthDate,
                            RoleType roleType = RoleType.User)
        {
            UserInformation user = new UserInformation()
            {
                Username  = username,
                Password  = password,
                FirstName = firstName,
                LastName  = lastName,
                Birthdate = birthDate,
                DataState = DataState.New,
                RoleType  = roleType
            };

            if (user.Birthdate > DateProvider.NOW())
            {
                throw new ValidationException("Invalid birthdate.");
            }

            if (!Regex.IsMatch(user.Username, RegexPatterns.LETTERS_ONLY))
            {
                throw new ValidationException("Invalid first name.");
            }

            if (!Regex.IsMatch(user.LastName, RegexPatterns.LETTERS_ONLY))
            {
                throw new ValidationException("Invalid last name.");
            }

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
                userInfo = new User(DateProvider, UserDataService)
                {
                    Username  = user.Username,
                    Password  = user.Password,
                    FirstName = user.FirstName,
                    LastName  = user.LastName,
                    Birthdate = user.Birthdate,
                    UserId    = user.UserId,
                    BadLogins = user.BadLogins,
                    RoleType  = user.RoleType
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
                    DataState = DataState.Clean,
                    BadLogins = userInfo.BadLogins,
                    RoleType  = userInfo.RoleType
                };
            }

            return user;
        }
        
        #endregion
    }
}
