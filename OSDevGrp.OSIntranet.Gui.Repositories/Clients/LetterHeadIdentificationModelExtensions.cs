using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Common.Models;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Clients
{
    internal static class LetterHeadIdentificationModelExtensions
    {
        #region Methods

        internal static ILetterHeadIdentificationModel AsInterface(this LetterHeadIdentificationModel letterHeadIdentificationModel)
        {
            NullGuard.NotNull(letterHeadIdentificationModel, nameof(letterHeadIdentificationModel));

            return new Common.Models.LetterHeadIdentificationModel(letterHeadIdentificationModel.Number, letterHeadIdentificationModel.Name);
        }

        #endregion
    }
}