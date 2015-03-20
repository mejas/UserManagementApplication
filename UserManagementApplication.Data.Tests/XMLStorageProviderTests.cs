using FluentAssertions;
using System;
using System.IO;
using UserManagementApplication.Data.DataEntities;
using UserManagementApplication.Data.StorageProviders;
using Xunit;

namespace UserManagementApplication.Data.Tests
{
    public class XMLStorageProviderTests
    {
        public class XMLStorageProviderTestsBase : IDisposable
        {
            protected string FILENAME { get; private set; }

            public XMLStorageProviderTestsBase(string fileName)
            {
                FILENAME = fileName;
            }

            public void Dispose()
            {
                FileInfo fileInfo = new FileInfo(FILENAME);

                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
            }
        }

        [Trait("Trait", "XMLStorageProviderTests")]
        public class XMLStorageProviderInitTests : XMLStorageProviderTestsBase
        {
            public XMLStorageProviderInitTests() : base("xmlFileTest1.xml") { }

            [Fact]
            public void InstanceShouldNotBeNull()
            {
                XMLStorageProvider subject = new XMLStorageProvider(FILENAME);
                subject.Should().NotBeNull();
            }

            [Fact]
            public void XMLFileShouldBeInitialized()
            {
                XMLStorageProvider storageProvider = new XMLStorageProvider(FILENAME);

                bool subject = File.Exists(storageProvider.XmlFile);
                
                subject.Should().BeTrue();
            }
        }

        [Trait("Trait", "XMLStorageProviderTests")]
        public class XMLStorageProviderAddUserTests : XMLStorageProviderTestsBase
        {
            public XMLStorageProviderAddUserTests() : base("xmlFileTest2.xml") { }

            [Fact]
            public void UserShouldBeAdded()
            {
                XMLStorageProvider storageProvider = new XMLStorageProvider(FILENAME);

                var userService = new User(storageProvider);

                userService.Create("admin", "admin", "Administrator", "Administrator", DateTime.Today, Contracts.DbRoleType.Administrator);
                userService.Create("yuckyTuna", "123456", "Yucky", "Tuna", DateTime.Today);

                var subject = userService.GetUsers("Administrator", String.Empty);

                subject.Count.Should().Be(1);
            }
        }
    }
}
