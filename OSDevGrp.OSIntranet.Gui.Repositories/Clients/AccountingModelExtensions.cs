using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Models;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Clients
{
    internal static class AccountingModelExtensions
    {
        #region Methods

        internal static IAccountingModel AsInterface(this AccountingModel accountingModel)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel));

            return new Accounting.Models.AccountingModel(
                accountingModel.Number,
                accountingModel.Name,
                accountingModel.LetterHead.AsInterface(),
                accountingModel.BalanceBelowZero.Convert<BalanceBelowZeroType, Interfaces.Accounting.Enums.BalanceBelowZeroType>(),
                accountingModel.BackDating);
        }

        #endregion
    }
}