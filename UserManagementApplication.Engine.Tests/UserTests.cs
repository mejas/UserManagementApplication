using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using UserManagementApplication.Common.Diagnostics.Interfaces;
using UserManagementApplication.Common.Enumerations;
using UserManagementApplication.Common.Exceptions;
using UserManagementApplication.Data.Contracts;
using UserManagementApplication.Data.Contracts.Interfaces;
using UserManagementApplication.Engine.BusinessEntities;
using UserManagementApplication.Engine.Providers;
using UserManagementApplication.Engine.Providers.Interfaces;
using Xunit;

namespace UserManagementApplication.Engine.Tests
{
    public class UserTests
    {
        public class UserInfoTestsProviders
        {
            public IDateProvider GetDateProvider()
            {
                var dateProvider = new Mock<IDateProvider>();

                dateProvider
                    .Setup(d => d.NOW())
                    .Returns(DateTime.Now);

                return dateProvider.Object;
            }

            public IAuthenticationProvider GetAuthenticationProvider()
            {
                return new UserManagementApplication.Engine.Tests.UserSessionTests.UserSessionTestsServices().AuthenticationProvider;
            }

            internal ILogProvider GetLogProvider()
            {
                var logProvider = new Mock<ILogProvider>();

                logProvider
                    .Setup(d => d.LogMessage(It.IsAny<string>()));

                return logProvider.Object;
            }
        }

        public class UserInfoTestsMockServices
        {
            private List<UserInformation> _users = new List<UserInformation>();

            public IUserDataService GetUserDataService()
            {
                var userDataService = new Mock<IUserDataService>();

                userDataService
                    .Setup(d => d.GetUsers())
                    .Returns(()=>_users);

                userDataService
                    .Setup(d => d.GetUsers(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns((string firstName, string lastName)=>findLogic(firstName, lastName));

                userDataService
                    .Setup(d => d.Commit(It.IsAny<UserInformation>()))
                    .Returns((UserInformation user)=>commitLogic(user));

                userDataService
                    .Setup(d => d.GetUser(It.IsAny<string>()))
                    .Returns((string username) => getLogic(username));

                userDataService
                    .Setup(d => d.GetUser(It.IsAny<int>()))
                    .Returns((int id) => getLogic(id));

                return userDataService.Object;
            }

            private UserInformation getLogic(int id)
            {
                return _users.Find(d => d.UserId == id);
            }

            private UserInformation getLogic(string username)
            {
                return _users.Find(d => d.Username == username);
            }

            private IList<UserInformation> findLogic(string firstName, string lastName)
            {
                return _users.FindAll(user =>
                    String.IsNullOrEmpty(firstName) && user.LastName == lastName ||
                    String.IsNullOrEmpty(lastName) && user.FirstName == firstName ||
                    user.FirstName == firstName && user.LastName == lastName);
            }


            private UserInformation commitLogic(UserInformation user)
            {
                switch (user.DataState)
                {
                    case DataState.New:
                        {
                            user.UserId = _users.Count + 1;
                            user.DataState = DataState.Clean;
                            _users.Add(user);
                        }
                        break;

                    case DataState.Modified:
                        {
                            var originalUser = _users.Find(d=>d.UserId == user.UserId);

                            if (originalUser != null)
                            {
                                originalUser.Username = user.Username;
                                originalUser.Password = user.Password;
                                originalUser.FirstName = user.FirstName;
                                originalUser.LastName = user.LastName;
                                originalUser.Birthdate = user.Birthdate;
                                originalUser.BadLogins = user.BadLogins;

                                originalUser.DataState = DataState.Clean;

                                user = originalUser;
                            }
                            else
                            {
                                return null;
                            }
                        }
                        break;

                    case DataState.Deleted:
                        {
                            _users.RemoveAt(user.UserId - 1);
                            user = null;
                        }
                        break;

                    default:
                        {
                            user = _users[user.UserId - 1];
                        }
                        break;
                }

                return user;
            }
        }

        public abstract class UserInfoTestsBase
        {
            private UserInfoTestsProviders _providers = new UserInfoTestsProviders();
            private UserInfoTestsMockServices _mockServices = new UserInfoTestsMockServices();

            protected IAuthenticationProvider AuthenticationProvider
            {
                get
                {
                    return _providers.GetAuthenticationProvider();
                }
            }

            protected IDateProvider DateProvider
            {
                get
                {
                    return _providers.GetDateProvider();
                }
            }

            protected IUserDataService UserDataService
            {
                get
                {
                    return _mockServices.GetUserDataService();
                }
            }

            protected ILogProvider LogProvider
            {
                get
                {
                    return _providers.GetLogProvider();
                }
            }
        }

        [Trait("Trait", "UserInfo")]
        public class UserInfoConstructor : UserInfoTestsBase
        {
            [Fact]
            public void ShouldHaveInstance()
            {
                var subject = new User(DateProvider, UserDataService, LogProvider);

                subject.Should().NotBeNull();
            }

            [Fact]
            public void UsernameShouldBeEmpty()
            {
                var subject = new User(DateProvider, UserDataService, LogProvider);

                subject.Username.Should().BeNullOrEmpty();
            }

            [Fact]
            public void PasswordShouldBeEmpty()
            {
                var subject = new User(DateProvider, UserDataService, LogProvider);

                subject.Password.Should().BeNullOrEmpty();
            }

            [Fact]
            public void FirstNameShouldBeEmpty()
            {
                var subject = new User(DateProvider, UserDataService, LogProvider);

                subject.FirstName.Should().BeNullOrEmpty();
            }

            [Fact]
            public void LastNameShouldBeEmpty()
            {
                var subject = new User(DateProvider, UserDataService, LogProvider);

                subject.LastName.Should().BeNullOrEmpty();
            }

            [Fact]
            public void BirthdateShouldBeDefault()
            {
                var subject = new User(DateProvider, UserDataService, LogProvider);

                subject.Age.Should().Be(0);
            }

            [Fact]
            public void UserIDShouldBeZero()
            {
                var subject = new User(DateProvider, UserDataService, LogProvider);

                subject.UserId.Should().Be(0);
            }

            [Fact]
            public void RoleTypeShouldBeUser()
            {
                var subject = new User(DateProvider, UserDataService, LogProvider);

                subject.RoleType.Should().Be(RoleType.User);
            }

            [Fact]
            public void BadLoginsShouldBeZero()
            {
                var subject = new User(DateProvider, UserDataService, LogProvider);

                subject.BadLogins.Should().Be(0);
            }
        }

        [Trait("Trait", "UserInfo")]
        public class UserInfoFindUserInfo : UserInfoTestsBase
        {
            [Fact]
            public void ResultsShouldNotBeNull()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider);
                userSession = userSession.AuthenticateUser("admin", "admin");

                var userInfo = new User(DateProvider, UserDataService, LogProvider);

                var subject = userInfo.Find(userSession);

                subject.Should().NotBeNull();
            }

            [Fact]
            public void ResultsShouldBeEmpty()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider);
                userSession = userSession.AuthenticateUser("admin", "admin");

                var userInfo = new User(DateProvider, UserDataService, LogProvider);

                var subject = userInfo.Find(userSession);

                subject.Should().BeEmpty();
            }
        
            [Fact]
            public void ResultShouldNotBeNull()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider);
                userSession = userSession.AuthenticateUser("admin", "admin");

                var userInfo = new User(DateProvider, UserDataService, LogProvider);
                userInfo.Create(userSession, "sampleman", "sampleman", "Sample", "Data", new DateTime(1992, 11, 9));
                userInfo.Create(userSession, "yuuna", "tougoudidnothingwrong", "Sample", "Information", new DateTime(1994, 8, 1));

                var subject = userInfo.Find(userSession, "Sample", String.Empty);

                subject.Should().NotBeNull();
            }

            [Fact]
            public void ResultShouldNotBeEmpty()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider);
                userSession = userSession.AuthenticateUser("admin", "admin");

                var userInfo = new User(DateProvider, UserDataService, LogProvider);
                userInfo.Create(userSession, "sampleman", "sampleman", "Sample", "Data", new DateTime(1992, 11, 9));
                userInfo.Create(userSession, "yuuna", "tougoudidnothingwrong", "Sample", "Information", new DateTime(1994, 8, 1));

                var subject = userInfo.Find(userSession, "Sample", String.Empty);

                subject.Should().HaveCount(2);
            }
        }

        [Trait("Trait", "UserInfo")]
        public class UserInfoGetUser : UserInfoTestsBase
        {
            [Fact]
            public void ResultShouldNotBeNull()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider);
                userSession = userSession.AuthenticateUser("admin", "admin");

                var userInfo = new User(DateProvider, UserDataService, LogProvider);
                userInfo.Create(userSession, "sampleman", "sampleman", "Sample", "Data", new DateTime(1992, 11, 9));
                userInfo.Create(userSession, "yuuna", "tougoudidnothingwrong", "Sample", "Information", new DateTime(1994, 8, 1));

                var subject = userInfo.GetUser("yuuna");

                subject.Should().NotBeNull();
            }

            [Fact]
            public void ResultShouldBeNull()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider);
                userSession = userSession.AuthenticateUser("admin", "admin");

                var userInfo = new User(DateProvider, UserDataService, LogProvider);
                userInfo.Create(userSession, "sampleman", "sampleman", "Sample", "Data", new DateTime(1992, 11, 9));
                userInfo.Create(userSession, "yuuna", "tougoudidnothingwrong", "Sample", "Information", new DateTime(1994, 8, 1));

                var subject = userInfo.GetUser("fuu");

                subject.Should().BeNull();
            }
        }

        [Trait("Trait", "UserInfo")]
        public class UserInfoCreateUserValidation : UserInfoTestsBase
        {
            [Fact]
            public void FirstNameInvalid()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider);
                userSession = userSession.AuthenticateUser("admin", "admin");

                var user = new User(DateProvider, UserDataService, LogProvider);

                Assert.Throws<ValidationException>(()=>user.Create(userSession, "username", "password", "f1r$tnaMe", "lastname", DateTime.Now));
            }

            [Fact]
            public void LastNameIsInvalid()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider);
                userSession = userSession.AuthenticateUser("admin", "admin");

                var user = new User(DateProvider, UserDataService, LogProvider);

                Assert.Throws<ValidationException>(() => user.Create(userSession, "username", "password", "firstname", "1@$tName", DateTime.Now));
            }

            [Fact]
            public void AdminCreationShouldBeDenied()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider); ;
                userSession = userSession.AuthenticateUser("gyuuki", "user");

                var user = new User(DateProvider, UserDataService, LogProvider);

                Assert.Throws<ValidationException>(() => user.Create(userSession, "username", "password", "firstname", "1@$tName", DateTime.Now, RoleType.Admin));
            }
        }

        [Trait("Trait", "UserInfo")]
        public class UserInfoCreateUser : UserInfoTestsBase
        {
            private string USERNAME = "username";
            private string PASSWORD = "password";
            private string FIRST_NAME = "Sample";
            private string LAST_NAME = "Data";
            private DateTime BIRTH_DATE = new DateTime(1992, 11, 9);

            [Fact]
            public void InstanceShouldNotBeNull()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider);
                userSession = userSession.AuthenticateUser("admin", "admin");

                var user = new User(DateProvider, UserDataService, LogProvider);

                var subject = user.Create(userSession, USERNAME, PASSWORD, FIRST_NAME, LAST_NAME, BIRTH_DATE);

                subject.Should().NotBeNull();
            }

            [Fact]
            public void UsernameShouldHaveValue()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider);
                userSession = userSession.AuthenticateUser("admin", "admin");

                var user = new User(DateProvider, UserDataService, LogProvider);

                var subject = user.Create(userSession, USERNAME, PASSWORD, FIRST_NAME, LAST_NAME, BIRTH_DATE);

                subject.Username.Should().Be(USERNAME);
            }

            [Fact]
            public void PasswordShouldHaveValue()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider);
                userSession = userSession.AuthenticateUser("admin", "admin");

                var user = new User(DateProvider, UserDataService, LogProvider);

                var subject = user.Create(userSession, USERNAME, PASSWORD, FIRST_NAME, LAST_NAME, BIRTH_DATE);

                subject.Password.Should().Be(PASSWORD);
            }

            [Fact]
            public void FirstNameShouldHaveValue()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider);
                userSession = userSession.AuthenticateUser("admin", "admin");

                var user = new User(DateProvider, UserDataService, LogProvider);

                var subject = user.Create(userSession, USERNAME, PASSWORD, FIRST_NAME, LAST_NAME, BIRTH_DATE);

                subject.FirstName.Should().Be(FIRST_NAME);
            }

            [Fact]
            public void LastNameShouldHaveValue()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider);
                userSession = userSession.AuthenticateUser("admin", "admin");

                var user = new User(DateProvider, UserDataService, LogProvider);

                var subject = user.Create(userSession, USERNAME, PASSWORD, FIRST_NAME, LAST_NAME, BIRTH_DATE);

                subject.LastName.Should().Be(LAST_NAME);
            }

            [Fact]
            public void BirthdateShouldHaveValue()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider);
                userSession = userSession.AuthenticateUser("admin", "admin");

                var user = new User(DateProvider, UserDataService, LogProvider);

                var subject = user.Create(userSession, USERNAME, PASSWORD, FIRST_NAME, LAST_NAME, BIRTH_DATE);

                subject.Birthdate.Should().Be(BIRTH_DATE);
            }

            [Fact]
            public void UserIdShouldHaveValue()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider);
                userSession = userSession.AuthenticateUser("admin", "admin");

                var user = new User(DateProvider, UserDataService, LogProvider);

                var subject = user.Create(userSession, USERNAME, PASSWORD, FIRST_NAME, LAST_NAME, BIRTH_DATE);

                subject.UserId.Should().Be(1);
            }

            [Fact]
            public void RoleTypeShouldBeUser()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider);
                userSession = userSession.AuthenticateUser("admin", "admin");

                var user = new User(DateProvider, UserDataService, LogProvider);

                var subject = user.Create(userSession, USERNAME, PASSWORD, FIRST_NAME, LAST_NAME, BIRTH_DATE);

                subject.RoleType.Should().Be(RoleType.User);
            }

            [Fact]
            public void BadLoginsShouldBeZero()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider);
                userSession = userSession.AuthenticateUser("admin", "admin");

                var user = new User(DateProvider, UserDataService, LogProvider);

                var subject = user.Create(userSession, USERNAME, PASSWORD, FIRST_NAME, LAST_NAME, BIRTH_DATE);

                subject.BadLogins.Should().Be(0);
            }
        }

        [Trait("Trait", "UserInfo")]
        public class UserInfoUpdateUser : UserInfoTestsBase
        {
            private string USERNAME     = "eurobeat";
            private string PASSWORD     = "yurobeat";
            private string FIRST_NAME   = "Dave";
            private string LAST_NAME    = "Rodgers";
            private DateTime BIRTH_DATE = new DateTime(1992, 12, 9);

            [Fact]
            public void InstanceShouldNotBeNull()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider);
                userSession = userSession.AuthenticateUser("admin", "admin");

                var userInfo = new User(DateProvider, UserDataService, LogProvider);

                var subject = userInfo.Create(userSession, "sampleman", "sampleman", "Sample", "Data", new DateTime(1992, 11, 9));

                subject.Username  = USERNAME;
                subject.Password  = PASSWORD;
                subject.FirstName = FIRST_NAME;
                subject.LastName  = LAST_NAME;
                subject.Birthdate = BIRTH_DATE;
                subject.BadLogins = 1;

                subject = userInfo.Update(userSession, subject);

                subject.Should().NotBeNull();
            }

            [Fact]
            public void UsernameShouldBeUpdated()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider);
                userSession = userSession.AuthenticateUser("admin", "admin");

                var userInfo = new User(DateProvider, UserDataService, LogProvider);

                var subject = userInfo.Create(userSession, "sampleman", "sampleman", "Sample", "Data", new DateTime(1992, 11, 9));

                subject.Username = USERNAME;
                subject.Password = PASSWORD;
                subject.FirstName = FIRST_NAME;
                subject.LastName = LAST_NAME;
                subject.Birthdate = BIRTH_DATE;
                subject.BadLogins = 1;

                subject = userInfo.Update(userSession, subject);

                subject.Username.Should().Be(USERNAME);
            }

            [Fact]
            public void PasswordShouldBeUpdated()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider);
                userSession = userSession.AuthenticateUser("admin", "admin");

                var userInfo = new User(DateProvider, UserDataService, LogProvider);

                var subject = userInfo.Create(userSession, "sampleman", "sampleman", "Sample", "Data", new DateTime(1992, 11, 9));

                subject.Username = USERNAME;
                subject.Password = PASSWORD;
                subject.FirstName = FIRST_NAME;
                subject.LastName = LAST_NAME;
                subject.Birthdate = BIRTH_DATE;
                subject.BadLogins = 1;

                subject = userInfo.Update(userSession, subject);

                subject.Password.Should().Be(PASSWORD);
            }

            [Fact]
            public void FirstNameShouldHaveValue()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider);
                userSession = userSession.AuthenticateUser("admin", "admin");

                var userInfo = new User(DateProvider, UserDataService, LogProvider);

                var subject = userInfo.Create(userSession, "sampleman", "sampleman", "Sample", "Data", new DateTime(1992, 11, 9));

                subject.Username = USERNAME;
                subject.Password = PASSWORD;
                subject.FirstName = FIRST_NAME;
                subject.LastName = LAST_NAME;
                subject.Birthdate = BIRTH_DATE;
                subject.BadLogins = 1;

                subject = userInfo.Update(userSession, subject);

                subject.FirstName.Should().Be(FIRST_NAME);
            }

            [Fact]
            public void LastNameShouldHaveValue()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider);
                userSession = userSession.AuthenticateUser("admin", "admin");

                var userInfo = new User(DateProvider, UserDataService, LogProvider);

                var subject = userInfo.Create(userSession, "sampleman", "sampleman", "Sample", "Data", new DateTime(1992, 11, 9));

                subject.Username = USERNAME;
                subject.Password = PASSWORD;
                subject.FirstName = FIRST_NAME;
                subject.LastName = LAST_NAME;
                subject.Birthdate = BIRTH_DATE;
                subject.BadLogins = 1;

                subject = userInfo.Update(userSession, subject);

                subject.LastName.Should().Be(LAST_NAME);
            }

            [Fact]
            public void BirthdateShouldHaveValue()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider);
                userSession = userSession.AuthenticateUser("admin", "admin");

                var userInfo = new User(DateProvider, UserDataService, LogProvider);

                var subject = userInfo.Create(userSession, "sampleman", "sampleman", "Sample", "Data", new DateTime(1992, 11, 9));

                subject.Username = USERNAME;
                subject.Password = PASSWORD;
                subject.FirstName = FIRST_NAME;
                subject.LastName = LAST_NAME;
                subject.Birthdate = BIRTH_DATE;
                subject.BadLogins = 1;

                subject = userInfo.Update(userSession, subject);

                subject.Birthdate.Should().Be(BIRTH_DATE);
            }

            [Fact]
            public void BadLoginsSHouldHaveValue()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider);
                userSession = userSession.AuthenticateUser("admin", "admin");

                var userInfo = new User(DateProvider, UserDataService, LogProvider);

                var subject = userInfo.Create(userSession, "sampleman", "sampleman", "Sample", "Data", new DateTime(1992, 11, 9));

                subject.Username = USERNAME;
                subject.Password = PASSWORD;
                subject.FirstName = FIRST_NAME;
                subject.LastName = LAST_NAME;
                subject.Birthdate = BIRTH_DATE;
                subject.BadLogins = 1;

                subject = userInfo.Update(userSession, subject);

                subject.BadLogins.Should().Be(1);
            }
        }

        [Trait("Trait", "UserInfo")]
        public class UserInfoDeleteUser : UserInfoTestsBase
        {
            [Fact]
            public void UserInfoShouldNotExist()
            {
                var userSession = new UserSession(AuthenticationProvider, LogProvider);
                userSession = userSession.AuthenticateUser("admin", "admin");

                var userInfo = new User(DateProvider, UserDataService, LogProvider);

                var itemToRemove = userInfo.Create(userSession, "sampleman", "sampleman", "Sample", "Data", new DateTime(1992, 11, 9));

                userInfo.Remove(userSession, itemToRemove);

                var subject = userInfo.Find(userSession);

                subject.Should().BeEmpty();
            }
        }
    }
}
