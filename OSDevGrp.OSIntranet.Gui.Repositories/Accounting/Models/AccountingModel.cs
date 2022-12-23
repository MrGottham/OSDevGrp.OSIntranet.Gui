using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Models;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Common.Models;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Accounting.Models
{
    internal class AccountingModel : AccountingIdentificationModel, IAccountingModel
    {
        #region Constructor

        public AccountingModel(int number, string name, ILetterHeadIdentificationModel letterHeadIdentificationModel, BalanceBelowZeroType balanceBelowZero, int backDating)
            : base(number, name)
        {
            NullGuard.NotNull(letterHeadIdentificationModel, nameof(letterHeadIdentificationModel));

            LetterHead = letterHeadIdentificationModel;
            BalanceBelowZero = balanceBelowZero;
            BackDating = backDating;
        }

        #endregion

        #region Properties

        public ILetterHeadIdentificationModel LetterHead { get; }

        public BalanceBelowZeroType BalanceBelowZero { get; }

        public int BackDating { get; }

        #endregion
    }
}