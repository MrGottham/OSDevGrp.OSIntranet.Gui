using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.Repositories.Exceptions;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Events;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Models;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Tests.Security.AccessTokenProviderCache
{
    [TestFixture]
    public class GetAccessTokenAsyncTests : SecurityTestBase
    {
        #region Private variables

        private Mock<IAccessTokenProvider> _accessTokenProviderMock;
        private Mock<IEventPublisher> _eventPublisherMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _accessTokenProviderMock = new Mock<IAccessTokenProvider>();
            _eventPublisherMock = new Mock<IEventPublisher>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccessTokenAsync_WhenAccessTokenModelHasNotBeenSet_AssertGetAccessTokenAsyncWasCalledOnAccessTokenProvider()
        {
            IAccessTokenProvider sut = CreateSut();

            await sut.GetAccessTokenAsync();

            _accessTokenProviderMock.Verify(m => m.GetAccessTokenAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccessTokenAsync_WhenNonExpiredAccessTokenModelHasBeenSet_AssertExpiresWasCalledOnNonExpiredAccessTokenModel()
        {
            IAccessTokenProvider sut = CreateSut();

            Mock<IAccessTokenModel> nonExpiredAccessTokenModelMock = CreateNonExpiredAccessTokenModelMock();
            await ((IAccessTokenSetter)sut).SetAccessTokenAsync(nonExpiredAccessTokenModelMock.Object);

            await sut.GetAccessTokenAsync();

            nonExpiredAccessTokenModelMock.Verify(m => m.Expires, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccessTokenAsync_WhenNonExpiredAccessTokenModelHasBeenSet_AssertGetAccessTokenAsyncWasNotCalledOnAccessTokenProvider()
        {
            IAccessTokenProvider sut = CreateSut();

            await ((IAccessTokenSetter)sut).SetAccessTokenAsync(CreateNonExpiredAccessTokenModelMock().Object);

            await sut.GetAccessTokenAsync();

            _accessTokenProviderMock.Verify(m => m.GetAccessTokenAsync(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccessTokenAsync_WhenExpiredAccessTokenModelHasBeenSet_AssertExpiresWasCalledOnExpiredAccessTokenModel()
        {
            IAccessTokenProvider sut = CreateSut();

            Mock<IAccessTokenModel> expiredAccessTokenModelMock = CreateExpiredAccessTokenModelMock();
            await ((IAccessTokenSetter)sut).SetAccessTokenAsync(expiredAccessTokenModelMock.Object);

            await sut.GetAccessTokenAsync();

            expiredAccessTokenModelMock.Verify(m => m.Expires, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccessTokenAsync_WhenExpiredAccessTokenModelHasBeenSet_AssertGetAccessTokenAsyncWasCalledOnAccessTokenProvider()
        {
            IAccessTokenProvider sut = CreateSut();

            await ((IAccessTokenSetter)sut).SetAccessTokenAsync(CreateExpiredAccessTokenModelMock().Object);

            await sut.GetAccessTokenAsync();

            _accessTokenProviderMock.Verify(m => m.GetAccessTokenAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccessTokenAsync_WhenCalledMultipleTimes_AssertGetAccessTokenAsyncWasCalledOnlyOnceOnAccessTokenProvider()
        {
            IAccessTokenProvider sut = CreateSut();

            await sut.GetAccessTokenAsync();
            await sut.GetAccessTokenAsync();
            await sut.GetAccessTokenAsync();

            _accessTokenProviderMock.Verify(m => m.GetAccessTokenAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccessTokenAsync_WhenAccessTokenModelHasNotBeenSet_ReturnsNotNull()
        {
            IAccessTokenProvider sut = CreateSut();

            IAccessTokenModel result = await sut.GetAccessTokenAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccessTokenAsync_WhenAccessTokenModelHasNotBeenSet_ReturnsAccessTokenModelFromAccessTokenProvider()
        {
            IAccessTokenModel accessTokenModel = _fixture.BuildAccessTokenModelMock().Object;
            IAccessTokenProvider sut = CreateSut(accessTokenModel);

            IAccessTokenModel result = await sut.GetAccessTokenAsync();

            Assert.That(result, Is.EqualTo(accessTokenModel));
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccessTokenAsync_WhenNonExpiredAccessTokenModelHasBeenSet_ReturnsNotNull()
        {
            IAccessTokenProvider sut = CreateSut();

            await ((IAccessTokenSetter)sut).SetAccessTokenAsync(CreateNonExpiredAccessTokenModelMock().Object);

            IAccessTokenModel result = await sut.GetAccessTokenAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccessTokenAsync_WhenNonExpiredAccessTokenModelHasBeenSet_ReturnsNonExpiredAccessTokenModel()
        {
            IAccessTokenProvider sut = CreateSut();

            IAccessTokenModel nonExpiredAccessTokenModel = CreateNonExpiredAccessTokenModelMock().Object;
            await ((IAccessTokenSetter)sut).SetAccessTokenAsync(nonExpiredAccessTokenModel);

            IAccessTokenModel result = await sut.GetAccessTokenAsync();

            Assert.That(result, Is.EqualTo(nonExpiredAccessTokenModel));
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccessTokenAsync_WhenExpiredAccessTokenModelHasBeenSet_ReturnsNotNull()
        {
            IAccessTokenProvider sut = CreateSut();

            await ((IAccessTokenSetter)sut).SetAccessTokenAsync(CreateExpiredAccessTokenModelMock().Object);

            IAccessTokenModel result = await sut.GetAccessTokenAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccessTokenAsync_WhenExpiredAccessTokenModelHasBeenSet_ReturnsAccessTokenModelFromAccessTokenProvider()
        {
            IAccessTokenModel accessTokenModel = _fixture.BuildAccessTokenModelMock().Object;
            IAccessTokenProvider sut = CreateSut(accessTokenModel);

            await ((IAccessTokenSetter)sut).SetAccessTokenAsync(CreateExpiredAccessTokenModelMock().Object);

            IAccessTokenModel result = await sut.GetAccessTokenAsync();

            Assert.That(result, Is.EqualTo(accessTokenModel));
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccessTokenAsync_WhenCalledMultipleTimes_ReturnsNotNull()
        {
            IAccessTokenProvider sut = CreateSut();

            IAccessTokenModel[] accessTokenModelCollection =
            {
                await sut.GetAccessTokenAsync(),
                await sut.GetAccessTokenAsync(),
                await sut.GetAccessTokenAsync()
            };

            foreach (IAccessTokenModel accessTokenModel in accessTokenModelCollection)
            {
                Assert.That(accessTokenModel, Is.Not.Null);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetAccessTokenAsync_WhenCalledMultipleTimes_ReturnsSameAccessTokenModelFromAccessTokenProvider()
        {
            IAccessTokenProvider sut = CreateSut();

            IAccessTokenModel result = await sut.GetAccessTokenAsync();
            IAccessTokenModel[] accessTokenModelCollection =
            {
                await sut.GetAccessTokenAsync(),
                await sut.GetAccessTokenAsync(),
                await sut.GetAccessTokenAsync()
            };

            foreach (IAccessTokenModel accessTokenModel in accessTokenModelCollection)
            {
                Assert.That(accessTokenModel, Is.EqualTo(result));
            }
        }

        [Test]
        [Category("UnitTest")]
        public void GetAccessTokenAsync_WhenIntranetGuiOfflineExceptionWasThrown_AssertPublishAsyncWasCalledOnEventPublisherWithSystemWentOfflineEvent()
        {
            IAccessTokenProvider sut = CreateSut(exception: new IntranetGuiOfflineException(_fixture.Create<string>()));

            Assert.ThrowsAsync<IntranetGuiOfflineException>(sut.GetAccessTokenAsync);

            _eventPublisherMock.Verify(m => m.PublishAsync(It.IsAny<ISystemWentOfflineEvent>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void GetAccessTokenAsync_WhenIntranetGuiOfflineExceptionWasThrown_TrowsIntranetGuiOfflineException()
        {
            IntranetGuiOfflineException exception = new IntranetGuiOfflineException(_fixture.Create<string>());
            IAccessTokenProvider sut = CreateSut(exception: exception);

            IntranetGuiOfflineException? result = Assert.ThrowsAsync<IntranetGuiOfflineException>(sut.GetAccessTokenAsync);

            Assert.That(result, Is.EqualTo(exception));
        }

        private IAccessTokenProvider CreateSut(IAccessTokenModel? accessTokenModel = null, Exception? exception = null)
        {
            if (exception != null)
            {
                _accessTokenProviderMock.Setup(m => m.GetAccessTokenAsync())
                    .Throws(exception);
            }
            else
            {
                _accessTokenProviderMock.Setup(m => m.GetAccessTokenAsync())
                    .Returns(Task.FromResult(accessTokenModel ?? _fixture.BuildAccessTokenModelMock().Object));
            }

            _eventPublisherMock.Setup(m => m.PublishAsync(It.IsAny<IEvent>()))
                .Returns(Task.CompletedTask);

            return new Repositories.Security.AccessTokenProviderCache(_accessTokenProviderMock.Object, _eventPublisherMock.Object);
        }

        private Mock<IAccessTokenModel> CreateNonExpiredAccessTokenModelMock()
        {
            return _fixture.BuildAccessTokenModelMock(expires: DateTime.Now.AddMinutes(_random.Next(15, 60)));
        }

        private Mock<IAccessTokenModel> CreateExpiredAccessTokenModelMock()
        {
            return _fixture.BuildAccessTokenModelMock(expires: DateTime.Now.AddMinutes(_random.Next(0, 15) * -1));
        }
    }
}