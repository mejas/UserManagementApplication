using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using UserManagementApplication.Common.Enumerations;
using UserManagementApplication.Common.Exceptions;
using UserManagementApplication.Data.Contracts;
using UserManagementApplication.Data.Contracts.Interfaces;
using UserManagementApplication.Engine.Providers;
using Xunit;

namespace UserManagementApplication.Engine.Tests
{
    public class DefaultAuthenticationProviderTests
    {
        public class DefaultAuthenticationProviderServices
        {
            private List<UserInformation> _users = new List<UserInformation>() 
            {
                new UserInformation()
                {
                    Username  = "admin",
                    Password  = "admin",
                    FirstName = "yucky",
                    LastName  = "tuna",
                    DataState = DataState.Clean,
                    RoleType  = RoleType.Admin,
                    UserId    = 1
                },

                new UserInformation()
                {
                    Username  = "user",
                    Password  = "user",
                    FirstName = "default",
                    LastName  = "user",
                    DataState = DataState.Clean,
                    RoleType  = RoleType.User,
                    UserId    = 2
                }
            };

            private Dictionary<string, UserSessionInformation> _sessions = new Dictionary<string, UserSessionInformation>();

            public IAuthenticationDataService GetAuthenticationDataService()
            {
                var authenticationDataService = new Mock<IAuthenticationDataService>();

                authenticationDataService
                    .Setup(d => d.StoreSession(It.IsAny<UserSessionInformation>()))
                    .Callback((UserSessionInformation userSessionInfo) => { _sessions[userSessionInfo.SessionToken] = userSessionInfo; });

                authenticationDataService
                    .Setup(d => d.GetUserSession(It.IsAny<string>()))
                    .Returns(
                    (string sessionToken) => 
                    {
                        UserSessionInformation userInfo;

                        _sessions.TryGetValue(sessionToken, out userInfo);

                        return userInfo;
                    });

                authenticationDataService
                    .Setup(d => d.RemoveSession(It.IsAny<string>()))
                    .Callback((string sessionToken) => _sessions.Remove(sessionToken));

                authenticationDataService
                    .Setup(d => d.Authenticate(It.IsAny<UserInformation>(), It.IsAny<string>()))
                    .Returns((UserInformation info, string pass) => mockAuthLogic(info, pass));
                
                return authenticationDataService.Object;
            }

            private bool mockAuthLogic(UserInformation info, string pass)
            {
                return info.Password == pass;
            }

            public IUserDataService GetUserDataService()
            {
                var userDataService = new Mock<IUserDataService>();

                userDataService
                    .Setup(d => d.GetUsers())
                    .Returns(() => _users);

                userDataService
                    .Setup(d => d.GetUsers(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns((string firstName, string lastName) => findLogic(firstName, lastName));

                userDataService
                    .Setup(d => d.Commit(It.IsAny<UserInformation>()))
                    .Returns((UserInformation user) => commitLogic(user));

                userDataService
                    .Setup(d => d.GetUser(It.IsAny<string>()))
                    .Returns((string username) => getLogic(username));

                return userDataService.Object;
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
                            var originalUser = _users[user.UserId - 1];

                            originalUser.Username = user.Username;
                            originalUser.Password = user.Password;
                            originalUser.FirstName = user.FirstName;
                            originalUser.LastName = user.LastName;
                            originalUser.Birthdate = user.Birthdate;

                            originalUser.DataState = DataState.Clean;

                            user = originalUser;
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

        public class DefaultAuthenticationProviderTestsBase
        {
            private DefaultAuthenticationProviderServices _services = new DefaultAuthenticationProviderServices();

            protected IUserDataService UserDataService
            {
                get
                {
                    return _services.GetUserDataService();
                }
            }

            protected IAuthenticationDataService AuthenticationDataService
            {
                get
                {
                    return _services.GetAuthenticationDataService();
                }
            }
        }

        [Trait("Trait","AuthenticationProviderTests")]
        public class AuthenticationProviderConstructorTests : DefaultAuthenticationProviderTestsBase
        {
            [Fact]
            public void InstanceShouldNotBeNull()
            {
                var subject = new DefaultAuthenticationProvider(UserDataService, AuthenticationDataService);

                subject.Should().NotBeNull();
            }
        }

        [Trait("Trait", "AuthenticationProviderTests")]
        public class AuthenticateSuccessTests : DefaultAuthenticationProviderTestsBase
        {
            [Fact]
            public void ResultShouldNotBeNull()
            {
                var authenticationProvider = new DefaultAuthenticationProvider(UserDataService, AuthenticationDataService);

                var subject = authenticationProvider.CreateSession("admin", "admin");

                subject.Should().NotBeNull();
            }

            [Fact]
            public void ResultSessionTokenShouldNotBeEmpty()
            {
                var authenticationProvider = new DefaultAuthenticationProvider(UserDataService, AuthenticationDataService);

                var subject = authenticationProvider.CreateSession("admin", "admin");

                subject.SessionToken.Should().NotBeNullOrEmpty();
            }
        }

        [Trait("Trait", "AuthenticationProviderTests")]
        public class AuthenticateFailureTests : DefaultAuthenticationProviderTestsBase
        {
            [Fact]
            public void ResultShouldNotBeNull()
            {
                var subject = new DefaultAuthenticationProvider(UserDataService, AuthenticationDataService);

                Assert.Throws<ErrorException>(() => subject.CreateSession("admin", "badpass"));
            }
        }

        [Trait("Trait", "AuthenticationProviderTests")]
        public class VerifyUserPermissionTests : DefaultAuthenticationProviderTestsBase
        {
            [Fact]
            public void VerifyAdminPermissionAgainstAdmin()
            {
                var authenticationProvider = new DefaultAuthenticationProvider(UserDataService, AuthenticationDataService);

                var session = authenticationProvider.CreateSession("admin", "admin");

                bool subject = authenticationProvider.HasPermission(session, RoleType.Admin);

                subject.Should().BeTrue();
            }

            [Fact]
            public void VerifyAdminPermissionAgainstUser()
            {
                var authenticationProvider = new DefaultAuthenticationProvider(UserDataService, AuthenticationDataService);

                var session = authenticationProvider.CreateSession("admin", "admin");

                bool subject = authenticationProvider.HasPermission(session, RoleType.User);

                subject.Should().BeTrue();
            }

            [Fact]
            public void VerifyUserPermissionAgainstUser()
            {
                var authenticationProvider = new DefaultAuthenticationProvider(UserDataService, AuthenticationDataService);

                var session = authenticationProvider.CreateSession("user", "user");

                bool subject = authenticationProvider.HasPermission(session, RoleType.User);

                subject.Should().BeTrue();
            }

            [Fact]
            public void VerifyUserPermissionAgainstAdmin()
            {
                var authenticationProvider = new DefaultAuthenticationProvider(UserDataService, AuthenticationDataService);

                var session = authenticationProvider.CreateSession("user", "user");

                bool subject = authenticationProvider.HasPermission(session, RoleType.Admin);

                subject.Should().BeFalse();
            }
        }

        [Trait("Trait", "AuthenticationProviderTests")]
        public class TerminateUserSessionTests : DefaultAuthenticationProviderTestsBase
        {
            [Fact]
            public void TerminateSessionTest()
            {
                var authenticationProvider = new DefaultAuthenticationProvider(UserDataService, AuthenticationDataService);

                var session = authenticationProvider.CreateSession("admin", "admin");

                authenticationProvider.TerminateSession(session);

                Assert.Throws<InvalidSessionException>(() => authenticationProvider.HasPermission(session, RoleType.Admin));
            }
        }
    }
}
