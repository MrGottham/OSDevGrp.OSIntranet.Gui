using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Events;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Models;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Accounting.Events
{
    internal class AccountingCollectionReceivedEvent : IAccountingCollectionReceivedEvent
    {
        #region Constructor

        public AccountingCollectionReceivedEvent(IEnumerable<IAccountingModel> accountings)
        {
            NullGuard.NotNull(accountings, nameof(accountings));

            Accountings = accountings;
        }

        #endregion

        #region Properties

        public IEnumerable<IAccountingModel> Accountings { get; }

        #endregion
    }
}