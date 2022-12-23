using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.Repositories.Exceptions;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Models;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Common.Models;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Events;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Options;
using OSDevGrp.OSIntranet.Gui.Repositories.Tests.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Tests.Accounting.OfflineAccountingRepository
{
    [TestFixture]
    public class GetAccountingsAsyncTests : AccountingTestBase
    {
        #region Private variables

        private Mock<IOptions<OfflineOptions>> _offlineOptionsMock;
        private Mock<IOfflineDataProvider> _offlineDataProviderMock;
        private Mock<IOfflineDataCommitter> _offlineDataCommitterMock;
        private Mock<IEventPublisher> _eventPublisherMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _offlineOptionsMock = new Mock<IOptions<OfflineOptions>>();
            _offlineDataProviderMock = new Mock<IOfflineDataProvider>();
            _offlineDataCommitterMock = new Mock<IOfflineDataCommitter>();
            _eventPublisherMock = new Mock<IEventPublisher>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccountingsAsync_WhenCalled_AssertGetSyncRootWasCalledOnOfflineDataProvider()
        {
            using IServiceScope serviceScope = CreateTestServiceScope();
            using IOfflineAccountingRepository sut = CreateSut(serviceScope.ServiceProvider);

            await sut.GetAccountingsAsync();

            _offlineDataProviderMock.Verify(m => m.GetSyncRoot(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccountingsAsync_WhenCalled_AssertGetOfflineDataDocumentAsyncWasCalledOnOfflineDataProvider()
        {
            using IServiceScope serviceScope = CreateTestServiceScope();
            using IOfflineAccountingRepository sut = CreateSut(serviceScope.ServiceProvider);

            await sut.GetAccountingsAsync();

            _offlineDataProviderMock.Verify(m => m.GetOfflineDataDocumentAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccountingsAsync_WhenCalled_ReturnsNotNull()
        {
            using IServiceScope serviceScope = CreateTestServiceScope();
            using IOfflineAccountingRepository sut = CreateSut(serviceScope.ServiceProvider);

            IEnumerable<IAccountingModel> result = await sut.GetAccountingsAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccountingsAsync_WhenCalled_ReturnsNotEmpty()
        {
            using IServiceScope serviceScope = CreateTestServiceScope();
            using IOfflineAccountingRepository sut = CreateSut(serviceScope.ServiceProvider);

            IEnumerable<IAccountingModel> result = await sut.GetAccountingsAsync();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void GetAccountingsAsync_WhenXmlExceptionHasBeenThrown_ThrowsIntranetGuiSystemException()
        {
            XmlException exception = _fixture.Create<XmlException>();
            using IServiceScope serviceScope = CreateTestServiceScope();
            using IOfflineAccountingRepository sut = CreateSut(serviceScope.ServiceProvider, exception: exception);

            IntranetGuiSystemException? result = Assert.ThrowsAsync<IntranetGuiSystemException>(sut.GetAccountingsAsync);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetAccountingsAsync_WhenXmlExceptionHasBeenThrown_ThrowsIntranetGuiSystemExceptionWhereInnerExceptionIsNotNull()
        {
            XmlException exception = _fixture.Create<XmlException>();
            using IServiceScope serviceScope = CreateTestServiceScope();
            using IOfflineAccountingRepository sut = CreateSut(serviceScope.ServiceProvider, exception: exception);

            IntranetGuiSystemException? result = Assert.ThrowsAsync<IntranetGuiSystemException>(sut.GetAccountingsAsync);

            Assert.That(result?.InnerException, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetAccountingsAsync_WhenXmlExceptionHasBeenThrown_ThrowsIntranetGuiSystemExceptionWhereInnerExceptionIsXmlException()
        {
            XmlException exception = _fixture.Create<XmlException>();
            using IServiceScope serviceScope = CreateTestServiceScope();
            using IOfflineAccountingRepository sut = CreateSut(serviceScope.ServiceProvider, exception: exception);

            IntranetGuiSystemException? result = Assert.ThrowsAsync<IntranetGuiSystemException>(sut.GetAccountingsAsync);

            Assert.That(result?.InnerException, Is.TypeOf<XmlException>());
        }

        [Test]
        [Category("UnitTest")]
        public void GetAccountingsAsync_WhenXmlExceptionHasBeenThrown_ThrowsIntranetGuiSystemExceptionWhereInnerExceptionIsEqualToThrownXmlException()
        {
            XmlException exception = _fixture.Create<XmlException>();
            using IServiceScope serviceScope = CreateTestServiceScope();
            using IOfflineAccountingRepository sut = CreateSut(serviceScope.ServiceProvider, exception: exception);

            IntranetGuiSystemException? result = Assert.ThrowsAsync<IntranetGuiSystemException>(sut.GetAccountingsAsync);

            Assert.That(result?.InnerException, Is.EqualTo(exception));
        }

        private IOfflineAccountingRepository CreateSut(IServiceProvider serviceProvider, XmlDocument? offlineDataDocument = null, Exception? exception = null)
        {
            NullGuard.NotNull(serviceProvider, nameof(serviceProvider));

            object syncRoot = new object();

            _offlineDataProviderMock.Setup(m => m.GetSyncRoot())
                .Returns(syncRoot);

            if (exception != null)
            {
                _offlineDataProviderMock.Setup(m => m.GetOfflineDataDocumentAsync())
                    .Throws(exception);
            }
            else
            {
                _offlineDataProviderMock.Setup(m => m.GetOfflineDataDocumentAsync())
                    .Returns(Task.FromResult(offlineDataDocument ?? BuildOfflineDataDocument(serviceProvider).GetAwaiter().GetResult()));
            }

            return new Repositories.Accounting.OfflineAccountingRepository(_offlineOptionsMock.Object, _offlineDataProviderMock.Object, _offlineDataCommitterMock.Object, _eventPublisherMock.Object);
        }

        private async Task<XmlDocument> BuildOfflineDataDocument(IServiceProvider serviceProvider)
        {
            NullGuard.NotNull(serviceProvider, nameof(serviceProvider));

            ILetterHeadIdentificationModel letterHeadIdentificationModel = _fixture.BuildLetterHeadIdentificationModelMock(1).Object;
            IAccountingModel[] accountingModels =
            {
                _fixture.BuildAccountingModelMock(1, letterHeadIdentificationModel: letterHeadIdentificationModel).Object,
                _fixture.BuildAccountingModelMock(2, letterHeadIdentificationModel: letterHeadIdentificationModel).Object,
                _fixture.BuildAccountingModelMock(3, letterHeadIdentificationModel: letterHeadIdentificationModel).Object
            };

            using IOfflineDataCommitter offlineDataCommitter = CreateTestOfflineDataCommitter(serviceProvider);
            using TestEventHandler<IOfflineDataUpdatedEvent> testEventHandler = CreateTestEventHandler<IOfflineDataUpdatedEvent>(serviceProvider);

            XmlDocument offlineDataDocument = OfflineDataDocumentFactory.Build();
            foreach (IAccountingModel accountingModel in accountingModels)
            {
                IOfflineDataUpdatedEvent? offlineDataUpdatedEvent = await testEventHandler.WaitForEventAsync(offlineDataCommitter.PushAsync(accountingModel));
                if (offlineDataUpdatedEvent == null)
                {
                    throw new Exception("An OfflineDataUpdatedEvent was not published.");
                }

                offlineDataDocument = offlineDataUpdatedEvent.OfflineDataDocument;
            }

            return offlineDataDocument;
        }
    }
}