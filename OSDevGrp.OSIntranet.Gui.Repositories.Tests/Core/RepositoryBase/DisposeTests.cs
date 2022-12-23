using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Tests.Core.RepositoryBase
{
    [TestFixture]
    public class DisposeTests : TestBase
    {
        #region Private variables

        private Mock<IEventPublisher> _eventPublisherMock;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _eventPublisherMock = new Mock<IEventPublisher>();
        }

        [Test]
        [Category("UnitTest")]
        public void Dispose_WhenCalled_AssertVirtualDisposeWasCalledOnSut()
        {
            MyRepository sut = CreateSut();

            sut.Dispose();

            Assert.That(sut.DisposeCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Dispose_WhenCalled_AssertVirtualDisposeWasCalledOnSutWithDisposingEqualToTrue()
        {
            MyRepository sut = CreateSut();

            sut.Dispose();

            Assert.That(sut.DisposeCalledWithDisposing, Is.True);
        }

        private MyRepository CreateSut()
        {
            return new MyRepository(_eventPublisherMock.Object);
        }

        private class MyRepository : Repositories.Core.RepositoryBase
        {
            #region Constructor

            public MyRepository(IEventPublisher eventPublisher)
                : base(eventPublisher)
            {
            }

            #endregion

            #region Properties

            public bool DisposeCalled { get; private set; }

            public bool DisposeCalledWithDisposing { get; private set; }

            #endregion

            #region Methods

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                DisposeCalled = true;
                DisposeCalledWithDisposing = disposing;
            }

            #endregion
        }
    }
}