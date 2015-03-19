﻿using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using UserManagementApplication.Data.DataEntities;
using UserManagementApplication.Data.Enumerations;
using UserManagementApplication.Data.Providers;
using Xunit;

namespace UserManagementApplication.Data.Tests
{
    public class UserTests
    {
        public class UserTestsProviders
        {
            private List<User> _users = new List<User>();

            public IStorageProvider GetStorageProvider()
            {
                var storageProvider = new Mock<IStorageProvider>();

                storageProvider
                    .Setup(d => d.GetAllUsers())
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

                return storageProvider.Object;
            }

            private IList<User> mockGetAllLogic()
            {
                return _users;
            }

            private User mockAddLogic(User user)
            {
                user.UserId = _users.Count + 1;
                _users.Add(user);

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
        }

        public class UserTestsBase
        {
            protected IStorageProvider StorageProvider
            {
                get
                {
                    return new UserTestsProviders().GetStorageProvider();
                }
            }
        }

        [Trait("Trait", "UserDataTests")]
        public class UserConstructorTests : UserTestsBase
        {
            [Fact]
            public void InstanceShouldNotBeNull()
            {
                var subject = new User(StorageProvider);

                subject.Should().NotBeNull();
            }

            [Fact]
            public void UserIdShouldBeZero()
            {
                var subject = new User(StorageProvider);

                subject.UserId.Should().Be(0);
            }

            [Fact]
            public void UsernameShouldBeEmpty()
            {
                var subject = new User(StorageProvider);

                subject.Username.Should().BeNullOrEmpty();
            }

            [Fact]
            public void PasswordShouldBeEmpty()
            {
                var subject = new User(StorageProvider);

                subject.Password.Should().BeNullOrEmpty();
            }

            [Fact]
            public void FirstNameShouldBeEmpty()
            {
                var subject = new User(StorageProvider);

                subject.FirstName.Should().BeNullOrEmpty();
            }

            [Fact]
            public void LastNameShouldBeEmpty()
            {
                var subject = new User(StorageProvider);

                subject.LastName.Should().BeNullOrEmpty();
            }

            [Fact]
            public void BirthdateShouldBeDefault()
            {
                var subject = new User(StorageProvider);

                subject.Birthdate.Should().Be(DateTime.MinValue);
            }

            [Fact]
            public void RoleTypeShouldBeUser()
            {
                var subject = new User(StorageProvider);

                subject.RoleType.Should().Be(RoleType.User);
            }
        }

        [Trait("Trait", "UserDataTests")]
        public class GetAllUserTests : UserTestsBase
        {
            [Fact]
            public void ResultShouldNotBeNull()
            {
                var user = new User(StorageProvider);

                var subject = user.GetAll();

                subject.Should().NotBeNull();
            }
        }

        [Trait("Trait", "UserDataTests")]
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
                var user = new User(StorageProvider);

                var subject = user.Create(USERNAME, PASSWORD, FIRST_NAME, LAST_NAME, BIRTH_DATE);

                subject.Should().NotBeNull();
            }

            [Fact]
            public void UsernameShouldHaveValue()
            {
                var user = new User(StorageProvider);

                var subject = user.Create(USERNAME, PASSWORD, FIRST_NAME, LAST_NAME, BIRTH_DATE);

                subject.FirstName.Should().Be(FIRST_NAME);
            }

            [Fact]
            public void PasswordShouldHaveValue()
            {
                var user = new User(StorageProvider);

                var subject = user.Create(USERNAME, PASSWORD, FIRST_NAME, LAST_NAME, BIRTH_DATE);

                subject.Password.Should().Be(PASSWORD);
            }

            [Fact]
            public void FirstNameShouldHaveValue()
            {
                var user = new User(StorageProvider);

                var subject = user.Create(USERNAME, PASSWORD, FIRST_NAME, LAST_NAME, BIRTH_DATE);

                subject.FirstName.Should().Be(FIRST_NAME);
            }

            [Fact]
            public void LastNameShouldHaveValue()
            {
                var user = new User(StorageProvider);

                var subject = user.Create(USERNAME, PASSWORD, FIRST_NAME, LAST_NAME, BIRTH_DATE);

                subject.LastName.Should().Be(LAST_NAME);
            }

            [Fact]
            public void BirthdateShouldHaveValue()
            {
                var user = new User(StorageProvider);

                var subject = user.Create(USERNAME, PASSWORD, FIRST_NAME, LAST_NAME, BIRTH_DATE);

                subject.Birthdate.Should().Be(BIRTH_DATE);
            }
        }

        [Trait("Trait", "UserDataTests")]
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
                var user = new User(StorageProvider);

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
                var user = new User(StorageProvider);

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
                var user = new User(StorageProvider);

                var subject = user.Create("yuuki", "admin", "yuuna", "yuuki", new DateTime(2001, 3, 4));

                subject.FirstName = FIRST_NAME;
                subject.LastName  = LAST_NAME;
                subject.Username  = USERNAME;
                subject.Password  = PASSWORD;
                subject.Birthdate = BIRTH_DATE;
                subject.RoleType  = RoleType.Admin;

                subject = user.Update(subject);

                subject.Password.Should().Be(PASSWORD);
            }

            [Fact]
            public void FirstNameShouldHaveValue()
            {
                var user = new User(StorageProvider);

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
            public void LastNameShouldHaveValue()
            {
                var user = new User(StorageProvider);

                var subject = user.Create("yuuki", "admin", "yuuna", "yuuki", new DateTime(2001, 3, 4));

                subject.FirstName = FIRST_NAME;
                subject.LastName  = LAST_NAME;
                subject.Username  = USERNAME;
                subject.Password  = PASSWORD;
                subject.Birthdate = BIRTH_DATE;
                subject.RoleType  = RoleType.Admin;

                subject = user.Update(subject);

                subject.LastName.Should().Be(LAST_NAME);
            }

            [Fact]
            public void BirthdateShouldHaveValue()
            {
                var user = new User(StorageProvider);

                var subject = user.Create("yuuki", "admin", "yuuna", "yuuki", new DateTime(2001, 3, 4));

                subject.FirstName = FIRST_NAME;
                subject.LastName  = LAST_NAME;
                subject.Username  = USERNAME;
                subject.Password  = PASSWORD;
                subject.Birthdate = BIRTH_DATE;
                subject.RoleType  = RoleType.Admin;

                subject = user.Update(subject);

                subject.Birthdate.Should().Be(BIRTH_DATE);
            }

            [Fact]
            public void RoleTypeShouldBeAdmin()
            {
                var user = new User(StorageProvider);

                var subject = user.Create("yuuki", "admin", "yuuna", "yuuki", new DateTime(2001, 3, 4));

                subject.FirstName = FIRST_NAME;
                subject.LastName  = LAST_NAME;
                subject.Username  = USERNAME;
                subject.Password  = PASSWORD;
                subject.Birthdate = BIRTH_DATE;
                subject.RoleType  = RoleType.Admin;

                subject = user.Update(subject);

                subject.RoleType.Should().Be(RoleType.Admin);
            }
        }

        [Trait("Trait", "UserDataTests")]
        public class DeleteUserTests : UserTestsBase
        {
            [Fact]
            public void UserShouldBeDeleted()
            {
                var user = new User(StorageProvider);

                var itemToDelete = user.Create("yuuki", "admin", "yuuna", "yuuki", new DateTime(2001, 3, 4));

                user.Delete(itemToDelete);

                var subject = user.GetAll();

                subject.Count.Should().Be(0);
            }
        }
    }
}