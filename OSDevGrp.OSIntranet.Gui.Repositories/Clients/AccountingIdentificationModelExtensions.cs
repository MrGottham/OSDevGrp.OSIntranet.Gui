using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Models;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Clients
{
    internal static class AccountingIdentificationModelExtensions
    {
        #region Methods

        internal static IAccountingIdentificationModel AsInterface(this AccountingIdentificationModel accountingIdentificationModel)
        {
            NullGuard.NotNull(accountingIdentificationModel, nameof(accountingIdentificationModel));

            return new Accounting.Models.AccountingIdentificationModel(accountingIdentificationModel.Number, accountingIdentificationModel.Name);
        }

        #endregion
    }
}