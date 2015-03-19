using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using UserManagementApplication.Engine.BusinessEntities;
using UserManagementApplication.Engine.Enumerations;
using UserManagementApplication.Data.Contracts.Interfaces;
using Moq;
using UserManagementApplication.Data.Contracts;
using UserManagementApplication.Common.Exceptions;

namespace UserManagementApplication.Engine.Tests
{
    public class UserSessionTests
    {
        public class UserSessionTestsServices
        {
            private List<UserInformation> _userInfos = new List<UserInformation>()
            {
                new UserInformation()
                {
                    Username = "admin",
                    Password = "admin",
                    FirstName = "Yuuki",
                    LastName = "Yuuna",
                    RoleType = DbRoleType.Administrator,
                    Birthdate = new DateTime(2001, 3, 9) //oh wow. i'm old ;_;
                },
                new UserInformation()
                {
                    Username = "gyuuki",
                    Password = "user",
                    FirstName = "Gyuuki",
                    LastName = "Gyuuna",
                    RoleType = DbRoleType.User,
                    Birthdate = new DateTime(2001, 3, 10)
                }
            };

            private Dictionary<string, UserInformation> _userSessions = new Dictionary<string, UserInformation>();

            public ISessionDataService GetSessionDataService()
            {
                var sessionDataService = new Mock<ISessionDataService>();

                sessionDataService
                    .Setup(d => d.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns((string username, string password) => mockAuthenticationLogic(username, password));

                sessionDataService
                    .Setup(d => d.GetSessionRoleType(It.IsAny<Session>()))
                    .Returns((Session session) =>
                    {
                        return _userSessions[session.SessionToken].RoleType;
                    });

                sessionDataService
                    .Setup(d => d.TerminateSession(It.IsAny<Session>()))
                    .Callback((Session session) =>
                    {
                        _userSessions.Remove(session.SessionToken);
                    });

                return sessionDataService.Object;
            }

            private Data.Contracts.Session mockAuthenticationLogic(string username, string password)
            {
                UserInformation userInformation = _userInfos.Find(item => item.Username == username);

                Session session = null;

                if (userInformation == null || 
                    userInformation.Password != password)
                {
                    throw new WarningException("Invalid logon credentials.");
                }

                session = new Session()
                {
                    SessionToken = "token"
                };

                _userSessions.Add(session.SessionToken, userInformation);

                return session;
            }
        }

        public class UserSessionTestsBase
        {
            protected ISessionDataService SessionDataService
            {
                get
                {
                    return new UserSessionTestsServices().GetSessionDataService();
                }
            }
        }

        [Trait("Trait", "UserSession")]
        public class UserSessionConstructor : UserSessionTestsBase
        {
            [Fact]
            public void ShouldHaveInstance()
            {
                var subject = new UserSession(SessionDataService);

                subject.Should().NotBeNull();
            }

            [Fact]
            public void SessionTokenShouldBeEmpty()
            {
                var subject = new UserSession(SessionDataService);

                subject.SessionToken.Should().BeNullOrEmpty();
            }
        }

        [Trait("Trait", "UserSession")]
        public class UserAuthSuccess : UserSessionTestsBase
        {
            [Fact]
            public void UserAuthenticationSuccessful()
            {
                var userSession = new UserSession(SessionDataService);

                var subject = userSession.AuthenticateUser("admin", "admin");

                subject.Should().NotBeNull();
            }

            [Fact]
            public void UserTokenShouldNotBeEmpty()
            {
                var userSession = new UserSession(SessionDataService);

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
                var userSession = new UserSession(SessionDataService);

                Assert.Throws<WarningException>(() => userSession.AuthenticateUser("admin", "yuunaxtougou"));
            }
        }

        [Trait("Trait", "UserSession")]
        public class UserElevationTest : UserSessionTestsBase
        {
            [Fact]
            public void AdminRoleShouldPermitAdmin()
            {
                var userSession = new UserSession(SessionDataService);

                var subject = userSession.AuthenticateUser("admin", "admin");

                userSession.IsPermitted(subject, RoleType.Admin).Should().BeTrue();
            }

            [Fact]
            public void AdminRoleShouldPermitUser()
            {
                var userSession = new UserSession(SessionDataService);

                var subject = userSession.AuthenticateUser("admin", "admin");

                userSession.IsPermitted(subject, RoleType.Admin).Should().BeTrue();
            }

            [Fact]
            public void UserRoleShouldDenyAdmin()
            {
                var userSession = new UserSession(SessionDataService);

                var subject = userSession.AuthenticateUser("gyuuki", "user");

                userSession.IsPermitted(subject, RoleType.Admin).Should().BeFalse();
            }

            [Fact]
            public void UserRoleShouldPermitUser()
            {
                var userSession = new UserSession(SessionDataService);

                var subject = userSession.AuthenticateUser("gyuuki", "user");

                userSession.IsPermitted(subject, RoleType.User).Should().BeTrue();
            }
        }

        public class UserSessionDestructionTest : UserSessionTestsBase
        {
            [Fact]
            public void UserSessionShouldBeDestroyed()
            {
                var userSession = new UserSession(SessionDataService);

                var subject = userSession.AuthenticateUser("gyuuki", "user");

                userSession.TerminateSession(subject);

                Assert.Throws<KeyNotFoundException>(() => userSession.IsPermitted(subject, RoleType.User));
            }
        }
    }
}
