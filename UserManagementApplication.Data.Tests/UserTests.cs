using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using UserManagementApplication.Common.Enumerations;
using UserManagementApplication.Data.DataEntities;
using UserManagementApplication.Data.Providers.Interfaces;
using Xunit;

namespace UserManagementApplication.Data.Tests
{
    public class UserTests
    {
        public class UserTestsProviders
        {
            private List<User> _users = new List<User>();

            public IUserDataStorageProvider GetStorageProvider()
            {
                var storageProvider = new Mock<IUserDataStorageProvider>();

                storageProvider
                    .Setup(d => d.GetUsers())
                    .Returns(() => mockGetAllLogic());

                storageProvider
                    .Setup(d => d.AddUser(It.IsAny<User>()))
                    .Returns((User user) => mockAddLogic(user));

                storageProvider
                    .Setup(d => d.UpdateUser(It.IsAny<User>()))
                    .Returns((User user) => mockUpdateLogic(user));

                storageProvider
                    .Setup(d => d.DeleteUser(It.IsAny<User>()))
                    .Callback((User user) => mockDeleteLogic(user));

                storageProvider
                    .Setup(d => d.GetUsers(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns((string firstName, string lastName) => mockFindUsersLogic(firstName, lastName));

                storageProvider
                    .Setup(d => d.GetUser(It.IsAny<string>()))
                    .Returns((string username) => mockGetUserByUsernameLogic(username));

                storageProvider
                    .Setup(d => d.GetUser(It.IsAny<int>()))
                    .Returns((int id) => mockGetLogic(id));

                return storageProvider.Object;
            }

            private User mockGetLogic(int id)
            {
                return _users.Find(user => user.UserId == id);
            }

            private User mockGetUserByUsernameLogic(string username)
            {
                return _users.Find(d => d.Username == username);
            }

            private IList<User> mockFindUsersLogic(string firstName, string lastName)
            {
                return _users.FindAll(user =>
                    String.IsNullOrEmpty(firstName) && user.LastName == lastName ||
                    String.IsNullOrEmpty(lastName) && user.FirstName == firstName ||
                    user.FirstName == firstName && user.LastName == lastName);
            }

            private IList<User> mockGetAllLogic()
            {
                return _users;
            }

            private User mockAddLogic(User user)
            {
                user.UserId = _users.Count + 1;

                var userToadd = new User()
                    {
                        UserId = _users.Count + 1,
                        BadLogins= user.BadLogins,
                        Birthdate = user.Birthdate,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Password = user.Password,
                        RoleType = user.RoleType,
                        Salt = user.Salt,
                        Username = user.Username
                    };

                _users.Add(userToadd);

                return user;
            }

            private User mockUpdateLogic(User user)
            {
                var userToUpdate = _users[user.UserId - 1];

                userToUpdate.UserId    = user.UserId;
                userToUpdate.Username  = user.Username;
                userToUpdate.Password  = user.Password;
                userToUpdate.FirstName = user.FirstName;
                userToUpdate.LastName  = user.LastName;
                userToUpdate.Birthdate = user.Birthdate;
                userToUpdate.RoleType  = user.RoleType;

                return userToUpdate;
            }

            private void mockDeleteLogic(User user)
            {
                _users.RemoveAt(user.UserId - 1);
            }

            public IDataSecurityProvider GetSecurityProvider()
            {
                var securityService = new Mock<IDataSecurityProvider>();

                securityService
                    .Setup(d => d.GenerateHash(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns((string s, string k) => s);

                securityService
                    .Setup(d => d.GenerateSalt())
                    .Returns(() => "salt");

                return securityService.Object;
            }
        }

        public class UserTestsBase
        {
            private UserTestsProviders _serviceInstance = new UserTestsProviders();
            protected IUserDataStorageProvider StorageProvider
            {
                get
                {
                    return _serviceInstance.GetStorageProvider();
                }
            }

            protected IDataSecurityProvider DataSecurityProvider
            {
                get
                {
                    return _serviceInstance.GetSecurityProvider();
                }
            }
        }

        [Trait("Trait", "UserData")]
        public class UserConstructorTests : UserTestsBase
        {
            [Fact]
            public void InstanceShouldNotBeNull()
            {
                var subject = new User(StorageProvider, DataSecurityProvider);

                subject.Should().NotBeNull();
            }

            [Fact]
            public void UserIdShouldBeZero()
            {
                var subject = new User(StorageProvider, DataSecurityProvider);

                subject.UserId.Should().Be(0);
            }

            [Fact]
            public void UsernameShouldBeEmpty()
            {
                var subject = new User(StorageProvider, DataSecurityProvider);

                subject.Username.Should().BeNullOrEmpty();
            }

            [Fact]
            public void PasswordShouldBeEmpty()
            {
                var subject = new User(StorageProvider, DataSecurityProvider);

                subject.Password.Should().BeNullOrEmpty();
            }

            [Fact]
            public void FirstNameShouldBeEmpty()
            {
                var subject = new User(StorageProvider, DataSecurityProvider);

                subject.FirstName.Should().BeNullOrEmpty();
            }

            [Fact]
            public void LastNameShouldBeEmpty()
            {
                var subject = new User(StorageProvider, DataSecurityProvider);

                subject.LastName.Should().BeNullOrEmpty();
            }

            [Fact]
            public void BirthdateShouldBeDefault()
            {
                var subject = new User(StorageProvider, DataSecurityProvider);

                subject.Birthdate.Should().Be(DateTime.MinValue);
            }

            [Fact]
            public void RoleTypeShouldBeUser()
            {
                var subject = new User(StorageProvider, DataSecurityProvider);

                subject.RoleType.Should().Be(RoleType.User);
            }

            [Fact]
            public void BadLoginsShouldBeZero()
            {
                var subject = new User(StorageProvider, DataSecurityProvider);

                subject.BadLogins.Should().Be(0);
            }

            [Fact]
            public void SaltShouldBeEmpty()
            {
                var subject = new User(StorageProvider, DataSecurityProvider);

                subject.Salt.Should().BeNullOrEmpty();
            }
        }

        [Trait("Trait", "UserData")]
        public class GetAllUserTests : UserTestsBase
        {
            [Fact]
            public void ResultShouldNotBeNull()
            {
                var user = new User(StorageProvider, DataSecurityProvider);

                var subject = user.GetAll();

                subject.Should().NotBeNull();
            }
        }

        [Trait("Trait", "UserData")]
        public class CreateUserTests : UserTestsBase
        {
            private string USERNAME = "username";
            private string PASSWORD = "password";
            private string FIRST_NAME = "Sample";
            private string LAST_NAME = "Data";
            private DateTime BIRTH_DATE = new DateTime(1992, 11, 9);

            [Fact]
            public void InstanceShouldNotBeNull()
            {
                var user = new User(StorageProvider, DataSecurityProvider);

                var subject = user.Create(USERNAME, PASSWORD, FIRST_NAME, LAST_NAME, BIRTH_DATE);

                subject.Should().NotBeNull();
            }

            [Fact]
            public void UsernameShouldHaveValue()
            {
                var user = new User(StorageProvider, DataSecurityProvider);

                var subject = user.Create(USERNAME, PASSWORD, FIRST_NAME, LAST_NAME, BIRTH_DATE);

                subject.FirstName.Should().Be(FIRST_NAME);
            }

            [Fact]
            public void PasswordShouldHaveValue()
            {
                var user = new User(StorageProvider, DataSecurityProvider);

                var subject = user.Create(USERNAME, PASSWORD, FIRST_NAME, LAST_NAME, BIRTH_DATE);

                subject.Password.Should().Be(PASSWORD);
            }

            [Fact]
            public void FirstNameShouldHaveValue()
            {
                var user = new User(StorageProvider, DataSecurityProvider);

                var subject = user.Create(USERNAME, PASSWORD, FIRST_NAME, LAST_NAME, BIRTH_DATE);

                subject.FirstName.Should().Be(FIRST_NAME);
            }

            [Fact]
            public void LastNameShouldHaveValue()
            {
                var user = new User(StorageProvider, DataSecurityProvider);

                var subject = user.Create(USERNAME, PASSWORD, FIRST_NAME, LAST_NAME, BIRTH_DATE);

                subject.LastName.Should().Be(LAST_NAME);
            }

            [Fact]
            public void BirthdateShouldHaveValue()
            {
                var user = new User(StorageProvider, DataSecurityProvider);

                var subject = user.Create(USERNAME, PASSWORD, FIRST_NAME, LAST_NAME, BIRTH_DATE);

                subject.Birthdate.Should().Be(BIRTH_DATE);
            }

            [Fact]
            public void SaltShouldHaveValue()
            {
                var user = new User(StorageProvider, DataSecurityProvider);

                var subject = user.Create(USERNAME, PASSWORD, FIRST_NAME, LAST_NAME, BIRTH_DATE);

                subject.Salt.Should().Be("salt");
            }
        }

        [Trait("Trait", "UserData")]
        public class GetUserTests : UserTestsBase
        {
            [Fact]
            public void InstanceShouldNotBeNull()
            {
                var user = new User(StorageProvider, DataSecurityProvider);

                user.Create("admin", "admin", "yuuna", "yuuki", new DateTime(2001, 3, 4));
                user.Create("gyuuki", "admin", "yuuna", "gelato", new DateTime(2001, 3, 4));

                var subject = user.GetUsers("yuuna", String.Empty);

                subject.Should().NotBeNull();
            }

            [Fact]
            public void ResultShouldHaveCount()
            {
                var user = new User(StorageProvider, DataSecurityProvider);

                user.Create("admin", "admin", "yuuna", "yuuki", new DateTime(2001, 3, 4));
                user.Create("gyuuki", "admin", "yuuna", "gelato", new DateTime(2001, 3, 4));

                var subject = user.GetUsers("yuuna", String.Empty);

                subject.Count.Should().Be(2);
            }

            [Fact]
            public void ResultShouldBeUnique()
            {
                var user = new User(StorageProvider, DataSecurityProvider);

                user.Create("admin", "admin", "yuuna", "yuuki", new DateTime(2001, 3, 4));
                user.Create("gyuuki", "admin", "yuuna", "gelato", new DateTime(2001, 3, 4));

                var subject = user.GetUsers("yuuna", "gelato");

                subject.Count.Should().Be(1);
            }
        }

        [Trait("Trait", "UserData")]
        public class GetByUsernameTests : UserTestsBase
        {
            [Fact]
            public void ResultShouldNotBeNull()
            {
                var user = new User(StorageProvider, DataSecurityProvider);

                user.Create("admin", "admin", "yuuna", "yuuki", new DateTime(2001, 3, 4));
                user.Create("gyuuki", "admin", "yuuna", "gelato", new DateTime(2001, 3, 4));

                var subject = user.GetUserByUserName("admin");

                subject.Should().NotBeNull();
            }

            [Fact]
            public void ResultShouldBeNull()
            {
                var user = new User(StorageProvider, DataSecurityProvider);

                user.Create("admin", "admin", "yuuna", "yuuki", new DateTime(2001, 3, 4));
                user.Create("gyuuki", "admin", "yuuna", "gelato", new DateTime(2001, 3, 4));

                var subject = user.GetUserByUserName("username");

                subject.Should().BeNull();
            }
        }

        [Trait("Trait", "UserData")]
        public class UpdateUserTests : UserTestsBase
        {
            private string USERNAME = "gyuuki";
            private string PASSWORD = "gyuuna";
            private string FIRST_NAME = "Gyuuki";
            private string LAST_NAME = "Gyuuna";
            private DateTime BIRTH_DATE = new DateTime(1981, 11, 9);

            [Fact]
            public void InstanceShouldNotBeNull()
            {
                var user = new User(StorageProvider, DataSecurityProvider);

                var subject = user.Create("yuuki", "admin", "yuuna", "yuuki", new DateTime(2001, 3, 4));
                
                subject.FirstName = FIRST_NAME;
                subject.LastName  = LAST_NAME;
                subject.Username  = USERNAME;
                subject.Password  = PASSWORD;
                subject.Birthdate = BIRTH_DATE;
                subject.RoleType  = RoleType.Admin;

                subject = user.Update(subject);

                subject.Should().NotBeNull();
            }

            [Fact]
            public void UsernameShouldHaveValue()
            {
                var user = new User(StorageProvider, DataSecurityProvider);

                var subject = user.Create("yuuki", "admin", "yuuna", "yuuki", new DateTime(2001, 3, 4));

                subject.FirstName = FIRST_NAME;
                subject.LastName  = LAST_NAME;
                subject.Username  = USERNAME;
                subject.Password  = PASSWORD;
                subject.Birthdate = BIRTH_DATE;
                subject.RoleType  = RoleType.Admin;

                subject = user.Update(subject);

                subject.FirstName.Should().Be(FIRST_NAME);
            }

            [Fact]
            public void PasswordShouldHaveValue()
            {
                var user = new User(StorageProvider, DataSecurityProvider);

                var subject = user.Create("yuuki", "admin", "yuuna", "yuuki", new DateTime(2001, 3, 4));

                subject.FirstName = FIRST_NAME;
                subject.LastName  = LAST_NAME;
                subject.Username  = USERNAME;
                subject.Password  = PASSWORD;
                subject.Birthdate = BIRTH_DATE;
                subject.RoleType = RoleType.Admin;

                subject = user.Update(subject);

                subject.Password.Should().Be(PASSWORD);
            }

            [Fact]
            public void FirstNameShouldHaveValue()
            {
                var user = new User(StorageProvider, DataSecurityProvider);

                var subject = user.Create("yuuki", "admin", "yuuna", "yuuki", new DateTime(2001, 3, 4));

                subject.FirstName = FIRST_NAME;
                subject.LastName  = LAST_NAME;
                subject.Username  = USERNAME;
                subject.Password  = PASSWORD;
                subject.Birthdate = BIRTH_DATE;
                subject.RoleType = RoleType.Admin;

                subject = user.Update(subject);

                subject.FirstName.Should().Be(FIRST_NAME);
            }

            [Fact]
            public void LastNameShouldHaveValue()
            {
                var user = new User(StorageProvider, DataSecurityProvider);

                var subject = user.Create("yuuki", "admin", "yuuna", "yuuki", new DateTime(2001, 3, 4));

                subject.FirstName = FIRST_NAME;
                subject.LastName  = LAST_NAME;
                subject.Username  = USERNAME;
                subject.Password  = PASSWORD;
                subject.Birthdate = BIRTH_DATE;
                subject.RoleType = RoleType.Admin;

                subject = user.Update(subject);

                subject.LastName.Should().Be(LAST_NAME);
            }

            [Fact]
            public void BirthdateShouldHaveValue()
            {
                var user = new User(StorageProvider, DataSecurityProvider);

                var subject = user.Create("yuuki", "admin", "yuuna", "yuuki", new DateTime(2001, 3, 4));

                subject.FirstName = FIRST_NAME;
                subject.LastName  = LAST_NAME;
                subject.Username  = USERNAME;
                subject.Password  = PASSWORD;
                subject.Birthdate = BIRTH_DATE;
                subject.RoleType = RoleType.Admin;

                subject = user.Update(subject);

                subject.Birthdate.Should().Be(BIRTH_DATE);
            }

            [Fact]
            public void RoleTypeShouldBeAdmin()
            {
                var user = new User(StorageProvider, DataSecurityProvider);

                var subject = user.Create("yuuki", "admin", "yuuna", "yuuki", new DateTime(2001, 3, 4));

                subject.FirstName = FIRST_NAME;
                subject.LastName  = LAST_NAME;
                subject.Username  = USERNAME;
                subject.Password  = PASSWORD;
                subject.Birthdate = BIRTH_DATE;
                subject.RoleType = RoleType.Admin;

                subject = user.Update(subject);

                subject.RoleType.Should().Be(RoleType.Admin);
            }

            [Fact]
            public void SaltShouldNotChange()
            {
                var user = new User(StorageProvider, DataSecurityProvider);

                var subject = user.Create("yuuki", "admin", "yuuna", "yuuki", new DateTime(2001, 3, 4));

                subject.FirstName = FIRST_NAME;
                subject.LastName  = LAST_NAME;
                subject.Username  = USERNAME;
                subject.Password  = PASSWORD;
                subject.Birthdate = BIRTH_DATE;
                subject.RoleType  = RoleType.Admin;
                subject.Salt      = "ASDASD";

                subject = user.Update(subject);

                subject.Salt.Should().Be("salt");
            }
        }

        [Trait("Trait", "UserData")]
        public class DeleteUserTests : UserTestsBase
        {
            [Fact]
            public void UserShouldBeDeleted()
            {
                var user = new User(StorageProvider, DataSecurityProvider);

                var itemToDelete = user.Create("yuuki", "admin", "yuuna", "yuuki", new DateTime(2001, 3, 4));

                user.Delete(itemToDelete);

                var subject = user.GetAll();

                subject.Count.Should().Be(0);
            }
        }
    }
}
