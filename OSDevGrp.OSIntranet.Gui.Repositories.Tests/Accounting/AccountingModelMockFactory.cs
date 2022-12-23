using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Models;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Common.Models;
using OSDevGrp.OSIntranet.Gui.Repositories.Tests.Common;
using System;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Tests.Accounting
{
    internal static class AccountingModelMockFactory
    {
        #region Methods

        internal static Mock<IAccountingIdentificationModel> BuildAccountingIdentificationModelMock(this Fixture fixture, int? number = null, string? name = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Random random = new Random(fixture.Create<int>());

            Mock<IAccountingIdentificationModel> accountingIdentificationModelMock = new Mock<IAccountingIdentificationModel>();
            accountingIdentificationModelMock.Setup(m => m.Number)
                .Returns(number ?? random.Next(1, 99));
            accountingIdentificationModelMock.Setup(m => m.Name)
                .Returns(name ?? fixture.Create<string>());
            return accountingIdentificationModelMock;
        }

        internal static Mock<IAccountingModel> BuildAccountingModelMock(this Fixture fixture, int? number = null, string? name = null, ILetterHeadIdentificationModel? letterHeadIdentificationModel = null, BalanceBelowZeroType? balanceBelowZero = null, int? backDating = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Random random = new Random(fixture.Create<int>());

            Mock<IAccountingModel> accountingModelMock = new Mock<IAccountingModel>();
            accountingModelMock.Setup(m => m.Number)
                .Returns(number ?? random.Next(1, 99));
            accountingModelMock.Setup(m => m.Name)
                .Returns(name ?? fixture.Create<string>());
            accountingModelMock.Setup(m => m.LetterHead)
                .Returns(letterHeadIdentificationModel ?? fixture.BuildLetterHeadIdentificationModelMock().Object);
            accountingModelMock.Setup(m => m.BalanceBelowZero)
                .Returns(balanceBelowZero ?? fixture.Create<BalanceBelowZeroType>());
            accountingModelMock.Setup(m => m.BalanceBelowZero)
                .Returns(balanceBelowZero ?? fixture.Create<BalanceBelowZeroType>());
            accountingModelMock.Setup(m => m.BackDating)
                .Returns(backDating ?? random.Next(0, 365));
            return accountingModelMock;
        }

        #endregion
    }
}