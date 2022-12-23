using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Events;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Options;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Tests.Accounting.OfflineAccountingRepository
{
    [TestFixture]
    public class DisposeTests : AccountingTestBase
    {
        #region Private variables

        private Mock<IOptions<OfflineOptions>> _offlineOptionsMock;
        private Mock<IOfflineDataProvider> _offlineDataProviderMock;
        private Mock<IOfflineDataCommitter> _offlineDataCommitterMock;
        private Mock<IEventPublisher> _eventPublisherMock;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _offlineOptionsMock = new Mock<IOptions<OfflineOptions>>();
            _offlineDataProviderMock = new Mock<IOfflineDataProvider>();
            _offlineDataCommitterMock = new Mock<IOfflineDataCommitter>();
            _eventPublisherMock = new Mock<IEventPublisher>();
        }

        [Test]
        [Category("UnitTest")]
        public void Dispose_WhenCalled_AssertRemoveSubscriberWasCalledOnEventPublisherWithEventHandlerForAccountingCollectionReceivedEvent()
        {
            using IOfflineAccountingRepository sut = CreateSut();

            sut.Dispose();

            _eventPublisherMock.Verify(m => m.RemoveSubscriber(It.Is<IEventHandler<IAccountingCollectionReceivedEvent>>(value => value != null && value == sut)), Times.Once);
        }

        private IOfflineAccountingRepository CreateSut()
        {
            return new Repositories.Accounting.OfflineAccountingRepository(_offlineOptionsMock.Object, _offlineDataProviderMock.Object, _offlineDataCommitterMock.Object, _eventPublisherMock.Object);
        }
    }
}