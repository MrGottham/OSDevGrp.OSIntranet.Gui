using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Common.Models;
using System;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Tests.Common
{
    internal static class CommonModelMockFactory
    {
        #region Methods

        internal static Mock<ILetterHeadIdentificationModel> BuildLetterHeadIdentificationModelMock(this Fixture fixture, int? number = null, string? name = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Random random = new Random(fixture.Create<int>());

            Mock<ILetterHeadIdentificationModel> letterHeadIdentificationModelMock = new Mock<ILetterHeadIdentificationModel>();
            letterHeadIdentificationModelMock.Setup(m => m.Number)
                .Returns(number ?? random.Next(1, 99));
            letterHeadIdentificationModelMock.Setup(m => m.Name)
                .Returns(name ?? fixture.Create<string>());
            return letterHeadIdentificationModelMock;
        }

        #endregion
    }
}