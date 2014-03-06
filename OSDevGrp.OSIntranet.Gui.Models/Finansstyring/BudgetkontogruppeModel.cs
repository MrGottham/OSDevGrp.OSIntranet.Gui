using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.Models.Finansstyring
{
    /// <summary>
    /// Model for en budgetkontogruppe.
    /// </summary>
    public class BudgetkontogruppeModel : KontogruppeModelBase, IBudgetkontogruppeModel
    {
        #region Constructor

        /// <summary>
        /// Danner modellen til en budgetkontogruppe.
        /// </summary>
        /// <param name="nummer">Unik identifikation af budgetkontogruppen.</param>
        /// <param name="tekst">Teksten, der beskriver budgetkontogruppen.</param>
        public BudgetkontogruppeModel(int nummer, string tekst)
            : base(nummer, tekst)
        {
        }

        #endregion
    }
}
