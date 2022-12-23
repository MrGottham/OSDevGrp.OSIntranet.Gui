using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Tests
{
    [TestFixture]
    public class ConfigurationTests : TestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithApiEndpoint_ReturnsApiEndpoint()
        {
            IConfiguration sut = CreateTestConfiguration();

            string? apiEndpoint = sut[ConfigurationKeys.ApiEndpointKey];

            Assert.That(string.IsNullOrWhiteSpace(apiEndpoint), Is.False);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithApiEndpoint_AssertApiEndpointIsAbsoluteUri()
        {
            IConfiguration sut = CreateTestConfiguration();

            bool result = Uri.TryCreate(sut[ConfigurationKeys.ApiEndpointKey], UriKind.Absolute, out var apiEndpoint);

            Assert.That(result && apiEndpoint != null, Is.True);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityClientId_ReturnsClientId()
        {
            IConfiguration sut = CreateTestConfiguration();

            string? clientId = sut[ConfigurationKeys.SecurityClientIdKey];

            Assert.That(string.IsNullOrWhiteSpace(clientId), Is.False);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityClientSecret_ReturnsClientSecret()
        {
            IConfiguration sut = CreateTestConfiguration();

            string? clientSecret = sut[ConfigurationKeys.SecurityClientSecretKey];

            Assert.That(string.IsNullOrWhiteSpace(clientSecret), Is.False);
        }
    }
}