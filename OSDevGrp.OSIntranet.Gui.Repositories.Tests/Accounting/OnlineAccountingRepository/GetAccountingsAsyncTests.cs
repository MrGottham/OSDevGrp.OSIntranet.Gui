using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Events;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Tests.Accounting.OnlineAccountingRepository
{
    [TestFixture]
    public class GetAccountingsAsyncTests : AccountingTestBase
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
        public async Task GetAccountingsAsync_WhenCalled_AssertIAccountingCollectionReceivedEventWasPublishedOnEventPublisher()
        {
            using TestEventHandler<IAccountingCollectionReceivedEvent> testEventHandler = CreateTestEventHandler<IAccountingCollectionReceivedEvent>(_serviceScope.ServiceProvider);

            using IOnlineAccountingRepository sut = CreateTestOnlineAccountingRepository(_serviceScope.ServiceProvider);

            await sut.GetAccountingsAsync();

            Assert.That(testEventHandler.NumberOfHandledEvents, Is.EqualTo(1));
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetAccountingsAsync_WhenCalled_ReturnsNotNull()
        {
            using IOnlineAccountingRepository sut = CreateTestOnlineAccountingRepository(_serviceScope.ServiceProvider);

            IEnumerable<IAccountingModel> result = await sut.GetAccountingsAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetAccountingsAsync_WhenCalled_ReturnsNotEmpty()
        {
            using IOnlineAccountingRepository sut = CreateTestOnlineAccountingRepository(_serviceScope.ServiceProvider);

            IEnumerable<IAccountingModel> result = await sut.GetAccountingsAsync();

            Assert.That(result, Is.Not.Empty);
        }
    }
}