using System;
using System.Globalization;
using OSDevGrp.OSIntranet.Gui.Models.Core;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;

namespace OSDevGrp.OSIntranet.Gui.Models.Finansstyring
{
    /// <summary>
    /// Model, der indeholder de grundlæggende oplysninger for en kontogruppe.
    /// </summary>
    public abstract class KontogruppeModelBase : TabelModelBase, IKontogruppeModelBase
    {
        #region Constructor

        /// <summary>
        /// Danner en model, der indeholder de grundlæggende oplysninger for en kontogruppe.
        /// </summary>
        /// <param name="nummer">Unik identifikation af kontogruppen.</param>
        /// <param name="tekst">Teksten, der beskriver kontogruppen.</param>
        protected KontogruppeModelBase(int nummer, string tekst)
            : base(nummer.ToString(CultureInfo.InvariantCulture), tekst)
        {
            if (nummer <= 0)
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "nummer", nummer), "nummer");
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Unik identifikation af kontogruppen.
        /// </summary>
        public virtual int Nummer
        {
            get
            {
                return Convert.ToInt32(Id);
            }
        }

        #endregion
    }
}
