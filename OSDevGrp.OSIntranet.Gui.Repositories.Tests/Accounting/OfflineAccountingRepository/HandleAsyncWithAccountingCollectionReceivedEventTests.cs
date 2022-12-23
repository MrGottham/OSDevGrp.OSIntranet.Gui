using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.Repositories.Accounting.Events;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Events;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Models;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Options;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Tests.Accounting.OfflineAccountingRepository
{
    [TestFixture]
    public class HandleAsyncWithAccountingCollectionReceivedEventTests : AccountingTestBase
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
        public void HandleAsync_WhenAccountingCollectionReceivedEventIsNull_ThrowsArgumentNullException()
        {
            using IOfflineAccountingRepository sut = CreateSut();

            ArgumentNullException? result = Assert.ThrowsAsync<ArgumentNullException>(async () => await ((IEventHandler<IAccountingCollectionReceivedEvent>)sut).HandleAsync(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("accountingCollectionReceivedEvent"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task HandleAsync_WhenCalled_AssertPushAsyncWasCalledOnOfflineDataCommitterForEachAccountingModelInAccountingCollectionReceivedEvent()
        {
            using IOfflineAccountingRepository sut = CreateSut();

            IAccountingModel[] accountingModelCollection =
            {
                _fixture.BuildAccountingModelMock().Object,
                _fixture.BuildAccountingModelMock().Object,
                _fixture.BuildAccountingModelMock().Object
            };
            await ((IEventHandler<IAccountingCollectionReceivedEvent>)sut).HandleAsync(new AccountingCollectionReceivedEvent(accountingModelCollection));

            foreach (IAccountingModel accountingModel in accountingModelCollection)
            {
                _offlineDataCommitterMock.Verify(m => m.PushAsync(It.Is<IAccountingModel>(value => value != null && value == accountingModel)), Times.Once);
            }
        }

        private IOfflineAccountingRepository CreateSut()
        {
            _offlineDataCommitterMock.Setup(m => m.PushAsync(It.IsAny<IAccountingModel>()))
                .Returns(Task.CompletedTask);

            return new Repositories.Accounting.OfflineAccountingRepository(_offlineOptionsMock.Object, _offlineDataProviderMock.Object, _offlineDataCommitterMock.Object, _eventPublisherMock.Object);
        }
    }
}