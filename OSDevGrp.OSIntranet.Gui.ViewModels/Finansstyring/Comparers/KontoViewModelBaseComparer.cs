using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Comparers
{
    /// <summary>
    /// Funktionalitet til sammenligning af to ViewModels indeholdende grundlæggende kontooplysninger.
    /// </summary>
    /// <typeparam name="TKontoViewModel">Typen på ViewModels for de konti, der skal sammenlignes.</typeparam>
    /// <typeparam name="TKontogruppeViewModel">Typen på ViewModel for de kontogrupper, der er benyttet på de konti, der skal sammenlignes.</typeparam>
    public class KontoViewModelBaseComparer<TKontoViewModel, TKontogruppeViewModel> : IComparer<TKontoViewModel> where TKontoViewModel : IKontoViewModelBase<TKontogruppeViewModel> where TKontogruppeViewModel : IKontogruppeViewModelBase
    {
        #region Private variables

        private readonly bool _sortByKontoværdi;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner funktionalitet til sammenligning af to ViewModels indeholdende grundlæggende kontooplysninger.
        /// </summary>
        /// <param name="sortByKontoværdi">Angivelse af, om der skal sorteres efter kontoens værdi.</param>
        public KontoViewModelBaseComparer(bool sortByKontoværdi)
        {
            _sortByKontoværdi = sortByKontoværdi;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sammenligner to ViewModels indeholdende grundlæggende kontooplysninger.
        /// </summary>
        /// <param name="x">ViewModel for konto, indeholdende kontooplysninger.</param>
        /// <param name="y">ViewModel for konto, indeholdende kontooplysninger.</param>
        /// <returns>Sammenligningsresultat.</returns>
        public virtual int Compare(TKontoViewModel x, TKontoViewModel y)
        {
            if (Equals(x, null))
            {
                throw new ArgumentNullException("x");
            }
            if (Equals(y, null))
            {
                throw new ArgumentNullException("y");
            }
            var result = x.Kontogruppe.Nummer.CompareTo(y.Kontogruppe.Nummer);
            if (result != 0)
            {
                return result;
            }
            if (_sortByKontoværdi)
            {
                result = x.Kontoværdi.CompareTo(y.Kontoværdi);
                if (result != 0)
                {
                    return result;
                }
            }
            return string.Compare(x.Kontonummer, y.Kontonummer, StringComparison.CurrentCulture);
        }

        #endregion
    }
}
