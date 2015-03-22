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

        public User(IUserDataService userDataService) : this(new DefaultDateProvider(), userDataService)
        {
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

        public User GetUser(string username)
        {
            return Translate(UserDataService.GetUser(username));
        }

        public User Create( UserSession userSession,
                            string username,
                            string password,
                            string firstName, 
                            string lastName, 
                            DateTime birthDate,
                            RoleType roleType = RoleType.User)
        {
            if (isRoleClearanceValid(userSession, roleType))
            {
                UserInformation user = new UserInformation()
                {
                    Username = username,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName,
                    Birthdate = birthDate,
                    DataState = DataState.New,
                    RoleType = roleType
                };

                if (String.IsNullOrEmpty(user.Username) ||
                    String.IsNullOrEmpty(user.Password))
                {
                    throw new ValidationException("Username and Password should not be blank.");
                }

                if (user.Birthdate.Date > DateProvider.NOW())
                {
                    throw new ValidationException("Invalid birthdate.");
                }

                if (!Regex.IsMatch(user.FirstName, RegexPatterns.LETTERS_ONLY))
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

            return null;
        }

        public User Update(UserSession session, User modifiedData)
        {
            if (validateUserSession(session, modifiedData))
            {

                User originalData = Translate(UserDataService.GetUser(modifiedData.UserId));

                if (originalData == null)
                {
                    throw new ErrorException("User does not exist.");
                }

                if (modifiedData.BadLogins != originalData.BadLogins)
                {
                    isRoleClearanceValid(session, RoleType.Admin);
                }
                
                var userInfo = Translate(modifiedData);
                userInfo.DataState = DataState.Modified;

                userInfo = UserDataService.Commit(userInfo);

                return Translate(userInfo);
            }

            return null;
        }

        public void Remove(UserSession session, User user)
        {
            if (validateUserSession(session, user) 
                && isRoleClearanceValid(session, user.RoleType))
            {
                var userInfo = Translate(user);

                userInfo.DataState = DataState.Deleted;

                UserDataService.Commit(userInfo);
            }
        }

        #endregion

        #region Functions

        private bool isRoleClearanceValid(UserSession userSession, Common.Enumerations.RoleType roleType)
        {
            if (!userSession.IsClearedForRole(userSession, roleType))
            {
                throw new UnauthorizedOperationException("The user is not allowed to execute this operation.");
            }

            return true;
        }

        private bool validateUserSession(UserSession session, User user)
        {
            if (session.User.UserId == user.UserId)
            {
                throw new UnauthorizedOperationException("The user is not allowed to execute this operation.");
            }

            return true;
        }

        public User Translate(UserInformation user)
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

        public UserInformation Translate(User userInfo)
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
