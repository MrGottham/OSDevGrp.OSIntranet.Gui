using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Events;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Models;
using OSDevGrp.OSIntranet.Gui.Repositories.Security.Models;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Tests.Security.AccessTokenProvider
{
    [TestFixture]
    public class GetAccessTokenAsyncTests : SecurityTestBase
    {
        #region Private variables

        private IServiceScope _serviceScope;

        #endregion

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _serviceScope = CreateTestServiceScope();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _serviceScope.Dispose();
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetAccessTokenAsync_WhenCalled_AssertAccessTokenAcquiredEventWasPublishedOnEventPublisher()
        {
            using TestEventHandler<IAccessTokenAcquiredEvent> testEventHandler = CreateTestEventHandler<IAccessTokenAcquiredEvent>(_serviceScope.ServiceProvider);

            IAccessTokenProvider sut = CreateTestAccessTokenProvider(_serviceScope.ServiceProvider);

            await sut.GetAccessTokenAsync();

            Assert.That(testEventHandler.NumberOfHandledEvents, Is.EqualTo(1));
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetAccessTokenAsync_WhenCalled_ReturnNotNull()
        {
            IAccessTokenProvider sut = CreateTestAccessTokenProvider(_serviceScope.ServiceProvider);

            IAccessTokenModel result = await sut.GetAccessTokenAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetAccessTokenAsync_WhenCalled_ReturnAccessTokenModel()
        {
            IAccessTokenProvider sut = CreateTestAccessTokenProvider(_serviceScope.ServiceProvider);

            IAccessTokenModel result = await sut.GetAccessTokenAsync();

            Assert.That(result, Is.TypeOf<AccessTokenModel>());
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetAccessTokenAsync_WhenCalled_ReturnAccessTokenModelWhereTokenTypeIsNotNull()
        {
            IAccessTokenProvider sut = CreateTestAccessTokenProvider(_serviceScope.ServiceProvider);

            IAccessTokenModel result = await sut.GetAccessTokenAsync();

            Assert.That(result.TokenType, Is.Not.Null);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetAccessTokenAsync_WhenCalled_ReturnAccessTokenModelWhereTokenTypeIsNotEmpty()
        {
            IAccessTokenProvider sut = CreateTestAccessTokenProvider(_serviceScope.ServiceProvider);

            IAccessTokenModel result = await sut.GetAccessTokenAsync();

            Assert.That(result.TokenType, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetAccessTokenAsync_WhenCalled_ReturnAccessTokenModelWhereTokenValueIsNotNull()
        {
            IAccessTokenProvider sut = CreateTestAccessTokenProvider(_serviceScope.ServiceProvider);

            IAccessTokenModel result = await sut.GetAccessTokenAsync();

            Assert.That(result.TokenValue, Is.Not.Null);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetAccessTokenAsync_WhenCalled_ReturnAccessTokenModelWhereTokenValueIsNotEmpty()
        {
            IAccessTokenProvider sut = CreateTestAccessTokenProvider(_serviceScope.ServiceProvider);

            IAccessTokenModel result = await sut.GetAccessTokenAsync();

            Assert.That(result.TokenValue, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetAccessTokenAsync_WhenCalled_ReturnAccessTokenModelWhereExpiresIsGreaterThanAcquiredTime()
        {
            IAccessTokenProvider sut = CreateTestAccessTokenProvider(_serviceScope.ServiceProvider);

            DateTime acquiredTime = DateTime.Now;
            IAccessTokenModel result = await sut.GetAccessTokenAsync();

            Assert.That(result.Expires, Is.GreaterThan(acquiredTime));
        }
    }
}