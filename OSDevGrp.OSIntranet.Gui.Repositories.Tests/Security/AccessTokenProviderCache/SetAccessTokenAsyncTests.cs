using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Models;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Tests.Security.AccessTokenProviderCache
{
    [TestFixture]
    public class SetAccessTokenAsyncTests : SecurityTestBase
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
        public void SetAccessTokenAsync_WhenAccessTokenModelIsNull_ThrowsArgumentNullException()
        {
            IAccessTokenSetter sut = CreateSut();

            ArgumentNullException? result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.SetAccessTokenAsync(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result?.ParamName, Is.EqualTo("accessTokenModel"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task SetAccessTokenAsync_WhenAccessTokenModelIsNotNull_SetsAccessTokenModelWhichWouldBeReturnedFromGetAccessTokenAsync()
        {
            IAccessTokenSetter sut = CreateSut();

            IAccessTokenModel accessTokenModel = _fixture.BuildAccessTokenModelMock(expires: DateTime.Now.AddMinutes(_random.Next(15, 60))).Object;
            await sut.SetAccessTokenAsync(accessTokenModel);

            IAccessTokenModel result = await ((IAccessTokenProvider)sut).GetAccessTokenAsync();

            Assert.That(result, Is.EqualTo(accessTokenModel));
        }

        private IAccessTokenSetter CreateSut()
        {
            return new Repositories.Security.AccessTokenProviderCache(_accessTokenProviderMock.Object, _eventPublisherMock.Object);
        }
    }
}