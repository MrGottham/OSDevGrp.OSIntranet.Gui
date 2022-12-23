using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Common.Models;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Common.Models
{
    internal class LetterHeadIdentificationModel : ILetterHeadIdentificationModel
    {
        #region Constructor

        public LetterHeadIdentificationModel(int number, string name)
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