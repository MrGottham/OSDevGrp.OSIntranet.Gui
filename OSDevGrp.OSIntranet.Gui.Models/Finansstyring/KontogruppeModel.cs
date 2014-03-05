using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.Models.Finansstyring
{
    /// <summary>
    /// Model for en kontogruppe.
    /// </summary>
    public class KontogruppeModel : KontogruppeModelBase, IKontogruppeModel
    {
        #region Private variables

        private Balancetype _balancetype;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner modellen til en kontogruppe.
        /// </summary>
        /// <param name="nummer">Unik identifikation af kontogruppen.</param>
        /// <param name="tekst">Teksten, der beskriver kontogruppen.</param>
        /// <param name="balancetype">Angivelse af siden i balance, hvor kontogruppen er placeret.</param>
        public KontogruppeModel(int nummer, string tekst, Balancetype balancetype)
            : base(nummer, tekst)
        {
            _balancetype = balancetype;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Angivelse af siden i balancen, hvor kontogruppen er placeret.
        /// </summary>
        public virtual Balancetype Balancetype
        {
            get
            {
                return _balancetype;
            }
            set
            {
                if (_balancetype == value)
                {
                    return;
                }
                _balancetype = value;
                RaisePropertyChanged("Balancetype");
            }
        }

        #endregion
    }
}
