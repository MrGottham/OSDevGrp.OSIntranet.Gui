using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Models;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Common.Models;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Events;
using OSDevGrp.OSIntranet.Gui.Repositories.Tests.Accounting;
using OSDevGrp.OSIntranet.Gui.Repositories.Tests.Common;
using System;
using System.Threading.Tasks;
using System.Xml;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Tests.Core.OfflineDataCommitter
{
    [TestFixture]
    public class PushAsyncWithAccountingModelTests : TestBase
    {
        #region Private variables

        private Mock<IOfflineDataProvider> _offlineDataProviderMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _offlineDataProviderMock = new Mock<IOfflineDataProvider>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void PushAsync_WhenAccountingModelIsNull_ThrowsArgumentNullException()
        {
            using IServiceScope serviceScope = CreateTestServiceScope();
            using IOfflineDataCommitter sut = CreateSut(serviceScope);

            ArgumentNullException? result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.PushAsync(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result?.ParamName, Is.EqualTo("accountingModel"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task PushAsync_WhenCalled_AssertNumberWasCalledOnAccountingModel()
        {
            using IServiceScope serviceScope = CreateTestServiceScope();
            using TestEventHandler<IOfflineDataUpdatedEvent> testEventHandler = CreateTestEventHandler<IOfflineDataUpdatedEvent>(serviceScope.ServiceProvider);
            using IOfflineDataCommitter sut = CreateSut(serviceScope);

            Mock<IAccountingModel> accountingModelMock = _fixture.BuildAccountingModelMock();
            await testEventHandler.WaitForEventAsync(sut.PushAsync(accountingModelMock.Object));

            accountingModelMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PushAsync_WhenCalled_AssertNameWasCalledOnAccountingModel()
        {
            using IServiceScope serviceScope = CreateTestServiceScope();
            using TestEventHandler<IOfflineDataUpdatedEvent> testEventHandler = CreateTestEventHandler<IOfflineDataUpdatedEvent>(serviceScope.ServiceProvider);
            using IOfflineDataCommitter sut = CreateSut(serviceScope);

            Mock<IAccountingModel> accountingModelMock = _fixture.BuildAccountingModelMock();
            await testEventHandler.WaitForEventAsync(sut.PushAsync(accountingModelMock.Object));

            accountingModelMock.Verify(m => m.Name, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PushAsync_WhenCalled_AssertLetterHeadWasCalledTwiceOnAccountingModel()
        {
            using IServiceScope serviceScope = CreateTestServiceScope();
            using TestEventHandler<IOfflineDataUpdatedEvent> testEventHandler = CreateTestEventHandler<IOfflineDataUpdatedEvent>(serviceScope.ServiceProvider);
            using IOfflineDataCommitter sut = CreateSut(serviceScope);

            Mock<IAccountingModel> accountingModelMock = _fixture.BuildAccountingModelMock();
            await testEventHandler.WaitForEventAsync(sut.PushAsync(accountingModelMock.Object));

            accountingModelMock.Verify(m => m.LetterHead, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task PushAsync_WhenCalled_AssertNumberWasCalledOnLetterHeadIdentificationModelFromAccountingModel()
        {
            using IServiceScope serviceScope = CreateTestServiceScope();
            using TestEventHandler<IOfflineDataUpdatedEvent> testEventHandler = CreateTestEventHandler<IOfflineDataUpdatedEvent>(serviceScope.ServiceProvider);
            using IOfflineDataCommitter sut = CreateSut(serviceScope);

            Mock<ILetterHeadIdentificationModel> letterHeadIdentificationModelMock = _fixture.BuildLetterHeadIdentificationModelMock();
            IAccountingModel accountingModel = _fixture.BuildAccountingModelMock(letterHeadIdentificationModel: letterHeadIdentificationModelMock.Object).Object;
            await testEventHandler.WaitForEventAsync(sut.PushAsync(accountingModel));

            letterHeadIdentificationModelMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PushAsync_WhenCalled_AssertNameWasCalledOnLetterHeadIdentificationModelFromAccountingModel()
        {
            using IServiceScope serviceScope = CreateTestServiceScope();
            using TestEventHandler<IOfflineDataUpdatedEvent> testEventHandler = CreateTestEventHandler<IOfflineDataUpdatedEvent>(serviceScope.ServiceProvider);
            using IOfflineDataCommitter sut = CreateSut(serviceScope);

            Mock<ILetterHeadIdentificationModel> letterHeadIdentificationModelMock = _fixture.BuildLetterHeadIdentificationModelMock();
            IAccountingModel accountingModel = _fixture.BuildAccountingModelMock(letterHeadIdentificationModel: letterHeadIdentificationModelMock.Object).Object;
            await testEventHandler.WaitForEventAsync(sut.PushAsync(accountingModel));

            letterHeadIdentificationModelMock.Verify(m => m.Name, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PushAsync_WhenCalled_AssertBalanceBelowZeroWasCalledOnAccountingModel()
        {
            using IServiceScope serviceScope = CreateTestServiceScope();
            using TestEventHandler<IOfflineDataUpdatedEvent> testEventHandler = CreateTestEventHandler<IOfflineDataUpdatedEvent>(serviceScope.ServiceProvider);
            using IOfflineDataCommitter sut = CreateSut(serviceScope);

            Mock<IAccountingModel> accountingModelMock = _fixture.BuildAccountingModelMock();
            await testEventHandler.WaitForEventAsync(sut.PushAsync(accountingModelMock.Object));

            accountingModelMock.Verify(m => m.BalanceBelowZero, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PushAsync_WhenCalled_AssertBackDatingWasCalledOnAccountingModel()
        {
            using IServiceScope serviceScope = CreateTestServiceScope();
            using TestEventHandler<IOfflineDataUpdatedEvent> testEventHandler = CreateTestEventHandler<IOfflineDataUpdatedEvent>(serviceScope.ServiceProvider);
            using IOfflineDataCommitter sut = CreateSut(serviceScope);

            Mock<IAccountingModel> accountingModelMock = _fixture.BuildAccountingModelMock();
            await testEventHandler.WaitForEventAsync(sut.PushAsync(accountingModelMock.Object));

            accountingModelMock.Verify(m => m.BackDating, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PushAsync_WhenCalled_AssertGetSyncRootWasCalledTwiceOnOfflineDataProvider()
        {
            using IServiceScope serviceScope = CreateTestServiceScope();
            using TestEventHandler<IOfflineDataUpdatedEvent> testEventHandler = CreateTestEventHandler<IOfflineDataUpdatedEvent>(serviceScope.ServiceProvider);
            using IOfflineDataCommitter sut = CreateSut(serviceScope);

            await testEventHandler.WaitForEventAsync(sut.PushAsync(_fixture.BuildAccountingModelMock().Object));

            _offlineDataProviderMock.Verify(m => m.GetSyncRoot(), Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task PushAsync_WhenCalled_AssertGetOfflineDataDocumentAsyncWasCalledTwiceOnOfflineDataProvider()
        {
            using IServiceScope serviceScope = CreateTestServiceScope();
            using TestEventHandler<IOfflineDataUpdatedEvent> testEventHandler = CreateTestEventHandler<IOfflineDataUpdatedEvent>(serviceScope.ServiceProvider);
            using IOfflineDataCommitter sut = CreateSut(serviceScope);

            await testEventHandler.WaitForEventAsync(sut.PushAsync(_fixture.BuildAccountingModelMock().Object));

            _offlineDataProviderMock.Verify(m => m.GetOfflineDataDocumentAsync(), Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task PushAsync_WhenCalled_AssertPublishAsyncWasCalledOnEventPublisherWithOfflineDataUpdatedEvent()
        {
            using IServiceScope serviceScope = CreateTestServiceScope();
            using TestEventHandler<IOfflineDataUpdatedEvent> testEventHandler = CreateTestEventHandler<IOfflineDataUpdatedEvent>(serviceScope.ServiceProvider);
            using IOfflineDataCommitter sut = CreateSut(serviceScope);

            IOfflineDataUpdatedEvent? result = await testEventHandler.WaitForEventAsync(sut.PushAsync(_fixture.BuildAccountingModelMock().Object));

            Assert.That(result, Is.Not.Null);
        }

        private IOfflineDataCommitter CreateSut(IServiceScope serviceScope, XmlDocument? offlineDataDocument = null)
        {
            NullGuard.NotNull(serviceScope, nameof(serviceScope));

            object syncRoot = new object();

            _offlineDataProviderMock.Setup(m => m.GetOfflineDataDocumentAsync())
                .Returns(Task.FromResult(offlineDataDocument ?? OfflineDataDocumentFactory.Build()));
            _offlineDataProviderMock.Setup(m => m.GetSyncRoot())
                .Returns(syncRoot);

            return new Repositories.Core.OfflineDataCommitter(_offlineDataProviderMock.Object, serviceScope.ServiceProvider.GetRequiredService<IEventPublisher>());
        }
    }
}