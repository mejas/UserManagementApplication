using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using UserManagementApplication.Data.DataEntities;
using UserManagementApplication.Data.Providers.Interfaces;
using Xunit;

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
                    .Setup(d => d.RemoveSessionByToken(It.IsAny<string>()))
                    .Callback((string session) => _sessions.Remove(session));

                storageProvider
                    .Setup(d => d.RemoveSessionByUsername(It.IsAny<string>()))
                    .Callback((string username) =>
                    {
                        string sessionKey = _sessions.Where(item => item.Value.UserData.Username == username).Select(p => p.Key).FirstOrDefault();

                        _sessions.Remove(sessionKey);
                    });

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
            public void SessionTokenDeletionResultShouldBeNull()
            {
                var session = new Session(StorageProvider);

                session.CreateSession(TOKEN, USER);

                session.RemoveSession(new Session(StorageProvider) { SessionToken = TOKEN });

                var subject = session.GetSession(TOKEN);

                subject.Should().BeNull();
            }

            [Fact]
            public void UsernameDeletionResultShouldBeNull()
            {
                var session = new Session(StorageProvider);

                session.CreateSession(TOKEN, USER);

                session.RemoveSession(new Session(StorageProvider) { UserData = USER });

                var subject = session.GetSession(TOKEN);

                subject.Should().BeNull();
            }
        }


    }
}
