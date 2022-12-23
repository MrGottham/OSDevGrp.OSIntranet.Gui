using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Common.Models;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Models
{
    public interface IAccountingModel : IAccountingIdentificationModel
    {
        ILetterHeadIdentificationModel LetterHead { get; }

        BalanceBelowZeroType BalanceBelowZero { get; }

        int BackDating { get; }
    }
}