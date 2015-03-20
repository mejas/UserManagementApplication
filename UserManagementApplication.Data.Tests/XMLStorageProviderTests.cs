using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using UserManagementApplication.Data.StorageProviders;
using System.IO;
using UserManagementApplication.Data.DataEntities;

namespace UserManagementApplication.Data.Tests
{
    public class XMLStorageProviderTests
    {
        [Trait("Trait", "XMLStorageProviderTests")]
        public class XMLStorageProviderInitTests
        {
            [Fact]
            public void InstanceShouldNotBeNull()
            {
                var subject = new XMLStorageProvider();

                subject.Should().NotBeNull();
            }

            [Fact]
            public void XMLFileShouldBeInitialized()
            {
                var xmlProvider = new XMLStorageProvider();

                Assert.True(File.Exists(xmlProvider.XmlFile));
            }
        }

        [Trait("Trait", "XMLStorageProviderTests")]
        public class XMLStorageProviderAddUserTests
        {
            [Fact]
            public void UserShouldBeAdded()
            {
                var storageProvider = new XMLStorageProvider();

                var userService = new User(storageProvider);

                userService.Create("admin", "admin", "Administrator", "Administrator", DateTime.Today, Contracts.DbRoleType.Administrator);
                userService.Create("yuckyTuna", "123456", "Yucky", "Tuna", DateTime.Today);

                var subject = userService.GetUsers("Administrator", String.Empty);

                subject.Count.Should().Be(1);
            }
        }
    }
}
