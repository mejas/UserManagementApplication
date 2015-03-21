using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using UserManagementApplication.Common.Enumerations;
using UserManagementApplication.Common.Exceptions;
using UserManagementApplication.Engine.BusinessEntities;
using UserManagementApplication.Engine.Providers;
using UserManagementApplication.Engine.Providers.Interfaces;
using Xunit;

namespace UserManagementApplication.Engine.Tests
{
    public class UserSessionTests
    {
        public class UserSessionTestsServices
        {
            private List<User> _userInfos = new List<User>()
            {
                new User(null, null)
                {
                    Username = "admin",
                    Password = "admin",
                    FirstName = "Yuuki",
                    LastName = "Yuuna",
                    RoleType = RoleType.Admin,
                    Birthdate = new DateTime(2001, 3, 9) //oh wow. i'm old ;_;
                },
                new User(null, null)
                {
                    Username = "gyuuki",
                    Password = "user",
                    FirstName = "Gyuuki",
                    LastName = "Gyuuna",
                    RoleType = RoleType.User,
                    Birthdate = new DateTime(2001, 3, 10)
                }
            };

            private Dictionary<string, User> _userSessions = new Dictionary<string, User>();

            public IAuthenticationProvider GetAuthenticationProvider()
            {
                var sessionDataService = new Mock<IAuthenticationProvider>();

                sessionDataService
                    .Setup(d => d.CreateSession(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns((string username, string password) => mockAuthenticationLogic(username, password));

                sessionDataService
                    .Setup(d => d.HasPermission(It.IsAny<UserSession>(), It.IsAny<RoleType>()))
                    .Returns((UserSession session, RoleType roleType) =>
                    {
                        var currentRole = _userSessions[session.SessionToken].RoleType;

                        if (currentRole == roleType)
                        {
                            return true;
                        }
                        else
                        {
                            return roleType == (currentRole | roleType);
                        }
                    }
                    );

                sessionDataService
                    .Setup(d => d.TerminateSession(It.IsAny<UserSession>()))
                    .Callback((UserSession session) =>
                    {
                        _userSessions.Remove(session.SessionToken);
                    });

                return sessionDataService.Object;
            }

            private UserSession mockAuthenticationLogic(string username, string password)
            {
                User userInformation = _userInfos.Find(item => item.Username == username);

                if (userInformation == null || 
                    userInformation.Password != password)
                {
                    throw new WarningException("Invalid logon credentials.");
                }

                UserSession userSession = new UserSession(null)
                {
                    SessionToken = "token"
                };

                _userSessions.Add(userSession.SessionToken, userInformation);

                return userSession;
            }
        }

        public class UserSessionTestsBase
        {
            protected IAuthenticationProvider AuthenticationProvider
            {
                get
                {
                    return new UserSessionTestsServices().GetAuthenticationProvider();
                }
            }
        }

        [Trait("Trait", "UserSession")]
        public class UserSessionConstructor : UserSessionTestsBase
        {
            [Fact]
            public void ShouldHaveInstance()
            {
                var subject = new UserSession(AuthenticationProvider);

                subject.Should().NotBeNull();
            }

            [Fact]
            public void SessionTokenShouldBeEmpty()
            {
                var subject = new UserSession(AuthenticationProvider);

                subject.SessionToken.Should().BeNullOrEmpty();
            }
        }

        [Trait("Trait", "UserSession")]
        public class UserAuthSuccess : UserSessionTestsBase
        {
            [Fact]
            public void UserAuthenticationSuccessful()
            {
                var userSession = new UserSession(AuthenticationProvider);

                var subject = userSession.AuthenticateUser("admin", "admin");

                subject.Should().NotBeNull();
            }

            [Fact]
            public void UserTokenShouldNotBeEmpty()
            {
                var userSession = new UserSession(AuthenticationProvider);

                var subject = userSession.AuthenticateUser("admin", "admin");

                subject.SessionToken.Should().NotBeNullOrEmpty();
            }
        }

        [Trait("Trait", "UserSession")]
        public class UserAuthFail : UserSessionTestsBase
        {
            [Fact]
            public void UserAuthenticationUnsuccessful()
            {
                var userSession = new UserSession(AuthenticationProvider);

                Assert.Throws<WarningException>(() => userSession.AuthenticateUser("admin", "yuunaxtougou"));
            }
        }

        [Trait("Trait", "UserSession")]
        public class UserElevationTest : UserSessionTestsBase
        {
            [Fact]
            public void AdminRoleShouldPermitAdmin()
            {
                var userSession = new UserSession(AuthenticationProvider);

                var subject = userSession.AuthenticateUser("admin", "admin");

                userSession.IsPermitted(subject, RoleType.Admin).Should().BeTrue();
            }

            [Fact]
            public void AdminRoleShouldPermitUser()
            {
                var userSession = new UserSession(AuthenticationProvider);

                var subject = userSession.AuthenticateUser("admin", "admin");

                userSession.IsPermitted(subject, RoleType.User).Should().BeTrue();
            }

            [Fact]
            public void UserRoleShouldDenyAdmin()
            {
                var userSession = new UserSession(AuthenticationProvider);

                var subject = userSession.AuthenticateUser("gyuuki", "user");

                userSession.IsPermitted(subject, RoleType.Admin).Should().BeFalse();
            }

            [Fact]
            public void UserRoleShouldPermitUser()
            {
                var userSession = new UserSession(AuthenticationProvider);

                var subject = userSession.AuthenticateUser("gyuuki", "user");

                userSession.IsPermitted(subject, RoleType.User).Should().BeTrue();
            }
        }

        [Trait("Trait", "UserSession")]
        public class UserSessionDestructionTest : UserSessionTestsBase
        {
            [Fact]
            public void UserSessionShouldBeDestroyed()
            {
                var userSession = new UserSession(AuthenticationProvider);

                var subject = userSession.AuthenticateUser("gyuuki", "user");

                userSession.TerminateSession(subject);

                Assert.Throws<KeyNotFoundException>(() => userSession.IsPermitted(subject, RoleType.User));
            }
        }
    }
}
