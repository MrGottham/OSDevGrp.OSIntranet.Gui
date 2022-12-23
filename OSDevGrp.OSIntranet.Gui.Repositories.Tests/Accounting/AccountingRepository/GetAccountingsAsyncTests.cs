using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.Repositories.Exceptions;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Models;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Tests.Accounting.AccountingRepository
{
    [TestFixture]
    public class GetAccountingsAsyncTests : AccountingTestBase
    {
        #region Private variables

        private Mock<IOnlineAccountingRepository> _onlineAccountingRepositoryMock;
        private Mock<IOfflineAccountingRepository> _offlineAccountingRepositoryMock;
        private Mock<IEventPublisher> _eventPublisherMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _onlineAccountingRepositoryMock = new Mock<IOnlineAccountingRepository>();
            _offlineAccountingRepositoryMock = new Mock<IOfflineAccountingRepository>();
            _eventPublisherMock = new Mock<IEventPublisher>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccountingsAsync_WhenFirstCalled_AssertGetAccountingsAsyncWasCalledOnOnlineAccountingRepository()
        {
            using IAccountingRepository sut = CreateSut();

            await sut.GetAccountingsAsync();

            _onlineAccountingRepositoryMock.Verify(m => m.GetAccountingsAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccountingsAsync_WhenOnlineAccountingRepositoryIsOnline_AssertGetAccountingsAsyncWasNotCalledOnOfflineAccountingRepository()
        {
            using IAccountingRepository sut = CreateSut();

            await sut.GetAccountingsAsync();

            _offlineAccountingRepositoryMock.Verify(m => m.GetAccountingsAsync(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccountingsAsync_WhenOnlineAccountingRepositoryWentOnline_AssertPublishAsyncWasCalledOnEventPublisherWithSystemWentOfflineEvent()
        {
            IntranetGuiOfflineException exception = new IntranetGuiOfflineException(_fixture.Create<string>());
            using IAccountingRepository sut = CreateSut(exception: exception);

            await sut.GetAccountingsAsync();

            _eventPublisherMock.Verify(m => m.PublishAsync(It.IsNotNull<ISystemWentOfflineEvent>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccountingsAsync_WhenOnlineAccountingRepositoryWentOnline_AssertGetAccountingsAsyncWasCalledOnOfflineAccountingRepository()
        {
            IntranetGuiOfflineException exception = new IntranetGuiOfflineException(_fixture.Create<string>());
            using IAccountingRepository sut = CreateSut(exception: exception);

            await sut.GetAccountingsAsync();

            _offlineAccountingRepositoryMock.Verify(m => m.GetAccountingsAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccountingsAsync_WhenOnlineAccountingRepositoryIsOffline_AssertGetAccountingsAsyncWasNotCalledOnOnlineAccountingRepository()
        {
            using IAccountingRepository sut = CreateSut(isOffline: true);

            await sut.GetAccountingsAsync();

            _onlineAccountingRepositoryMock.Verify(m => m.GetAccountingsAsync(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccountingsAsync_WhenOnlineAccountingRepositoryIsOffline_AssertPublishAsyncWasNotCalledOnEventPublisherWithSystemWentOfflineEvent()
        {
            using IAccountingRepository sut = CreateSut(isOffline: true);

            await sut.GetAccountingsAsync();

            _eventPublisherMock.Verify(m => m.PublishAsync(It.IsAny<ISystemWentOfflineEvent>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccountingsAsync_WhenOnlineAccountingRepositoryIsOffline_AssertGetAccountingsAsyncWasCalledOnOfflineAccountingRepository()
        {
            using IAccountingRepository sut = CreateSut(isOffline: true);

            await sut.GetAccountingsAsync();

            _offlineAccountingRepositoryMock.Verify(m => m.GetAccountingsAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccountingsAsync_WhenOnlineAccountingRepositoryIsOnline_ReturnsNotNull()
        {
            using IAccountingRepository sut = CreateSut();

            IEnumerable<IAccountingModel> result = await sut.GetAccountingsAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccountingsAsync_WhenOnlineAccountingRepositoryIsOnline_ReturnsNotEmpty()
        {
            using IAccountingRepository sut = CreateSut();

            IEnumerable<IAccountingModel> result = await sut.GetAccountingsAsync();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccountingsAsync_WhenOnlineAccountingRepositoryIsOnline_ReturnsAccountingsFromOnlineAccountingRepository()
        {
            IAccountingModel[] accountingModels =
            {
                _fixture.BuildAccountingModelMock().Object,
                _fixture.BuildAccountingModelMock().Object,
                _fixture.BuildAccountingModelMock().Object
            };
            using IAccountingRepository sut = CreateSut(accountingModels);

            IEnumerable<IAccountingModel> result = await sut.GetAccountingsAsync();

            Assert.That(result, Is.EqualTo(accountingModels));
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccountingsAsync_WhenOnlineAccountingRepositoryWentOnline_ReturnsNotNull()
        {
            IntranetGuiOfflineException exception = new IntranetGuiOfflineException(_fixture.Create<string>());
            using IAccountingRepository sut = CreateSut(exception: exception);

            IEnumerable<IAccountingModel> result = await sut.GetAccountingsAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccountingsAsync_WhenOnlineAccountingRepositoryWentOnline_ReturnsNotEmpty()
        {
            IntranetGuiOfflineException exception = new IntranetGuiOfflineException(_fixture.Create<string>());
            using IAccountingRepository sut = CreateSut(exception: exception);

            IEnumerable<IAccountingModel> result = await sut.GetAccountingsAsync();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccountingsAsync_WhenOnlineAccountingRepositoryWentOnline_ReturnsAccountingsFromOfflineAccountingRepository()
        {
            IAccountingModel[] accountingModels =
            {
                _fixture.BuildAccountingModelMock().Object,
                _fixture.BuildAccountingModelMock().Object,
                _fixture.BuildAccountingModelMock().Object
            };
            IntranetGuiOfflineException exception = new IntranetGuiOfflineException(_fixture.Create<string>());
            using IAccountingRepository sut = CreateSut(accountingModels, exception);

            IEnumerable<IAccountingModel> result = await sut.GetAccountingsAsync();

            Assert.That(result, Is.EqualTo(accountingModels));
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccountingsAsync_WhenOnlineAccountingRepositoryIsOffline_ReturnsNotNull()
        {
            using IAccountingRepository sut = CreateSut(isOffline: true);

            IEnumerable<IAccountingModel> result = await sut.GetAccountingsAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccountingsAsync_WhenOnlineAccountingRepositoryIsOffline_ReturnsNotEmpty()
        {
            using IAccountingRepository sut = CreateSut(isOffline: true);

            IEnumerable<IAccountingModel> result = await sut.GetAccountingsAsync();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccountingsAsync_WhenOnlineAccountingRepositoryIsOffline_ReturnsAccountingsFromOfflineAccountingRepository()
        {
            IAccountingModel[] accountingModels =
            {
                _fixture.BuildAccountingModelMock().Object,
                _fixture.BuildAccountingModelMock().Object,
                _fixture.BuildAccountingModelMock().Object
            };
            using IAccountingRepository sut = CreateSut(accountingModels, isOffline: true);

            IEnumerable<IAccountingModel> result = await sut.GetAccountingsAsync();

            Assert.That(result, Is.EqualTo(accountingModels));
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetAccountingsAsync_WhenCalled_AssertOfflineDataUpdatedEventHasBeenPublishedFromEventPublisher()
        {
            using IServiceScope serviceScope = CreateTestServiceScope();
            using IAccountingRepository sut = CreateTestAccountingRepository(serviceScope.ServiceProvider);
            using TestEventHandler<IOfflineDataUpdatedEvent> testEventHandler = CreateTestEventHandler<IOfflineDataUpdatedEvent>(serviceScope.ServiceProvider);

            IOfflineDataUpdatedEvent? result = await testEventHandler.WaitForEventAsync(sut.GetAccountingsAsync(), new TimeSpan(0, 0, 1, 0));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetAccountingsAsync_WhenCalled_ReturnsNotNull()
        {
            using IServiceScope serviceScope = CreateTestServiceScope();
            using IAccountingRepository sut = CreateTestAccountingRepository(serviceScope.ServiceProvider);

            IEnumerable<IAccountingModel> result = await sut.GetAccountingsAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetAccountingsAsync_WhenCalled_ReturnsNotEmpty()
        {
            using IServiceScope serviceScope = CreateTestServiceScope();
            using IAccountingRepository sut = CreateTestAccountingRepository(serviceScope.ServiceProvider);

            IEnumerable<IAccountingModel> result = await sut.GetAccountingsAsync();

            Assert.That(result, Is.Not.Empty);
        }

        private IAccountingRepository CreateSut(IEnumerable<IAccountingModel>? accountingModels = null, Exception? exception = null, bool isOffline = false)
        {
            if (exception != null)
            {
                _onlineAccountingRepositoryMock.Setup(m => m.GetAccountingsAsync())
                    .Throws(exception);
            }
            else
            {
                _onlineAccountingRepositoryMock.Setup(m => m.GetAccountingsAsync())
                    .Returns(Task.FromResult(accountingModels ?? new[]
                    {
                        _fixture.BuildAccountingModelMock().Object,
                        _fixture.BuildAccountingModelMock().Object,
                        _fixture.BuildAccountingModelMock().Object
                    }));
            }

            _offlineAccountingRepositoryMock.Setup(m => m.GetAccountingsAsync())
                .Returns(Task.FromResult(accountingModels ?? new[]
                {
                    _fixture.BuildAccountingModelMock().Object,
                    _fixture.BuildAccountingModelMock().Object,
                    _fixture.BuildAccountingModelMock().Object
                }));

            _eventPublisherMock.Setup(m => m.PublishAsync(It.IsAny<IEvent>()))
                .Returns(Task.CompletedTask);

            return new Repositories.Accounting.AccountingRepository(_onlineAccountingRepositoryMock.Object, _offlineAccountingRepositoryMock.Object, _eventPublisherMock.Object, isOffline);
        }
    }
}