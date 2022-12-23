using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Options;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Tests.Core.OfflineDataProvider
{
    [TestFixture]
    public class GetSyncRootTests : TestBase
    {
        #region Private variables

        private Mock<IOptions<OfflineOptions>> _offlineOptionsMock;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _offlineOptionsMock = new Mock<IOptions<OfflineOptions>>();
        }

        [Test]
        [Category("UnitTest")]
        public void GetSyncRoot_WhenCalled_ReturnsNotNull()
        {
            IOfflineDataProvider sut = CreateSut();

            object result = sut.GetSyncRoot();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetSyncRoot_WhenCalled_ReturnsObject()
        {
            IOfflineDataProvider sut = CreateSut();

            object result = sut.GetSyncRoot();

            Assert.That(result, Is.TypeOf<object>());
        }

        [Test]
        [Category("UnitTest")]
        public void GetSyncRoot_WhenCalledMultipleTimes_ReturnsSameObject()
        {
            IOfflineDataProvider sut = CreateSut();

            object result = sut.GetSyncRoot();

            Assert.That(sut.GetSyncRoot(), Is.SameAs(result));
            Assert.That(sut.GetSyncRoot(), Is.SameAs(result));
            Assert.That(sut.GetSyncRoot(), Is.SameAs(result));
        }

        private IOfflineDataProvider CreateSut()
        {
            _offlineOptionsMock.Setup(m => m.Value)
                .Returns(BuildOfflineOptions());

            return new Repositories.Core.OfflineDataProvider(_offlineOptionsMock.Object);
        }

        private static OfflineOptions BuildOfflineOptions()
        {
            return new OfflineOptions
            {
                OfflineDataDocument = OfflineDataDocumentFactory.Build()
            };
        }
    }
}