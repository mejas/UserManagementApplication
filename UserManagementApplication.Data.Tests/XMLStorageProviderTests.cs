using FluentAssertions;
using System;
using System.IO;
using UserManagementApplication.Common.Enumerations;
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

        [Trait("Trait", "XMLStorageProvider")]
        public class XMLStorageProviderInitTests : XMLStorageProviderTestsBase
        {
            public XMLStorageProviderInitTests() : base("xmlFileTest1.xml") { }

            [Fact]
            public void InstanceShouldNotBeNull()
            {
                UserDataXmlStorageProvider subject = new UserDataXmlStorageProvider(FILENAME);
                subject.Should().NotBeNull();
            }

            [Fact]
            public void XMLFileShouldBeInitialized()
            {
                UserDataXmlStorageProvider storageProvider = new UserDataXmlStorageProvider(FILENAME);

                bool subject = File.Exists(storageProvider.XmlFile);
                
                subject.Should().BeTrue();
            }
        }

        [Trait("Trait", "XMLStorageProvider")]
        public class XMLStorageProviderAddUserTests : XMLStorageProviderTestsBase
        {
            public XMLStorageProviderAddUserTests() : base("xmlFileTest2.xml") { }

            [Fact]
            public void UserShouldBeAdded()
            {
                UserDataXmlStorageProvider storageProvider = new UserDataXmlStorageProvider(FILENAME);

                var userService = new User(storageProvider);

                userService.Create("yuckyTuna", "123456", "Yucky", "Tuna", DateTime.Today);

                var subject = userService.GetUsers("Administrator", String.Empty);

                subject.Count.Should().Be(1);
            }
        }
    }
}
