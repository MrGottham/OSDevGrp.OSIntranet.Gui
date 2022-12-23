using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Models;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Accounting.Models
{
    internal class AccountingIdentificationModel : IAccountingIdentificationModel
    {
        #region Constructor

        public AccountingIdentificationModel(int number, string name)
        {
            NullGuard.NotNullOrWhiteSpace(name, nameof(name));

            Number = number;
            Name = name;
        }

        #endregion

        #region Properties

        public int Number { get; }

        public string Name { get; }

        #endregion
    }
}