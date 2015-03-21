using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using UserManagementApplication.Data.DataEntities;
using Moq;
using UserManagementApplication.Data.StorageProviders.Interfaces;

namespace UserManagementApplication.Data.Tests
{
    public class SessionTests
    {
        public class SessionTestsProviders
        {
            private Dictionary<string, Session> _sessions = new Dictionary<string, Session>();

            public ISessionDataStorageProvider GetSessionDataStorageProvider()
            {
                var storageProvider = new Mock<ISessionDataStorageProvider>();

                storageProvider
                    .Setup(d => d.CreateSession(It.IsAny<string>(), It.IsAny<Session>()))
                    .Returns((string sessionToken, Session user) =>
                        {
                            _sessions[sessionToken] = user;

                            return user;
                        });

                storageProvider
                    .Setup(d=>d.GetSession(It.IsAny<string>()))
                    .Returns((string token) => 
                    {
                        Session userInfo;
                        _sessions.TryGetValue(token, out userInfo);
                        
                        return userInfo;
                    });

                storageProvider
                    .Setup(d => d.RemoveSession(It.IsAny<string>()))
                    .Callback((string session) => _sessions.Remove(session));

                return storageProvider.Object;
            }
        }

        public class SessionTestsBase
        {
            private SessionTestsProviders _providers = new SessionTestsProviders();

            protected ISessionDataStorageProvider StorageProvider
            {
                get
                {
                    return _providers.GetSessionDataStorageProvider();
                }
            }
        }

        [Trait("Trait", "SessionData")]
        public class SessionConstructorTests : SessionTestsBase
        {
            [Fact]
            public void InstanceShouldNotBeNull()
            {
                var subject = new Session(StorageProvider);

                subject.Should().NotBeNull();
            }

            [Fact]
            public void SessionTokenShouldBeEmpty()
            {
                var subject = new Session(StorageProvider);

                subject.SessionToken.Should().BeNullOrEmpty();
            }

            [Fact]
            public void UserDataShouldBeNull()
            {
                var subject = new Session(StorageProvider);

                subject.UserData.Should().BeNull();
            }
        }

        [Trait("Trait", "SessionData")]
        public class CreateSessionTests : SessionTestsBase
        {
            private User USER = new User()
            {
                Username = "sample",
                Password = "user"
            };

            [Fact]
            public void ResultShouldNotBeNull()
            {
                var session = new Session(StorageProvider);

                var subject = session.CreateSession("abc", USER);

                subject.Should().NotBeNull();
            }

            [Fact]
            public void SessionTokenShouldNotBeEmpty()
            {
                var session = new Session(StorageProvider);

                var subject = session.CreateSession("abc", USER);

                subject.SessionToken.Should().NotBeNullOrEmpty();
            }

            [Fact]
            public void UserDataShouldNotBeNull()
            {
                var session = new Session(StorageProvider);

                var subject = session.CreateSession("abc", USER);

                subject.UserData.Should().NotBeNull();
            }
        }

        [Trait("Trait", "SessionData")]
        public class GetSessionTests : SessionTestsBase
        {
            private User USER = new User()
            {
                Username = "sample",
                Password = "user"
            };

            private string TOKEN = "token";

            [Fact]
            public void ResultShouldNotBeNull()
            {
                var session = new Session(StorageProvider);

                session.CreateSession(TOKEN, USER);

                var subject = session.GetSession(TOKEN);

                subject.Should().NotBeNull();
            }

            [Fact]
            public void ResultShouldBeNull()
            {
                var session = new Session(StorageProvider);

                session.CreateSession(TOKEN, USER);

                var subject = session.GetSession("abc");

                subject.Should().BeNull();
            }
        }

        [Trait("Trait", "SessionData")]
        public class RemoveSessionTests : SessionTestsBase
        {
            private User USER = new User()
            {
                Username = "sample",
                Password = "user"
            };

            private string TOKEN = "token";

            [Fact]
            public void ResultShouldBeNull()
            {
                var session = new Session(StorageProvider);

                session.CreateSession(TOKEN, USER);

                session.RemoveSession(TOKEN);

                var subject = session.GetSession(TOKEN);

                subject.Should().BeNull();
            }
        }


    }
}
