using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Models;
using System;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Tests.Security
{
    internal static class SecurityModelMockFactory
    {
        #region Methods

        internal static Mock<IAccessTokenModel> BuildAccessTokenModelMock(this Fixture fixture, string? tokenType = null, string? tokenValue = null, DateTime? expires = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Random random = new Random(fixture.Create<int>());

            Mock<IAccessTokenModel> accessTokenModelMock = new Mock<IAccessTokenModel>();
            accessTokenModelMock.Setup(m => m.TokenType)
                .Returns(tokenType ?? fixture.Create<string>());
            accessTokenModelMock.Setup(m => m.TokenValue)
                .Returns(tokenValue ?? fixture.Create<string>());
            accessTokenModelMock.Setup(m => m.Expires)
                .Returns(expires ?? DateTime.Now.AddMinutes(random.Next(15, 69)));
            return accessTokenModelMock;
        }

        #endregion
    }
}