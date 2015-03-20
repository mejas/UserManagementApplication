using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using UserManagementApplication.Engine.AuthenticationProviders;

namespace UserManagementApplication.Engine.Tests
{
    public class DefaultAuthenticationProviderTests
    {
        [Trait("Trait","AuthenticationProviderTests")]
        public class AuthenticationProviderConstructorTests
        {
            [Fact]
            public void InstanceShouldNotBeNull()
            {
                var subject = new DefaultAuthenticationProvider();

                subject.Should().NotBeNull();
            }
        }

        [Trait("Trait", "AuthenticationProviderTests")]
        public class AuthenticateSuccessTests
        {
            [Fact]
            public void ResultShouldNotBeNull()
            {
                var authenticationProvider = new DefaultAuthenticationProvider();

                var subject = authenticationProvider.CreateSession("admin", "admin");

                subject.Should().NotBeNull();
            }
        }
    }
}
