using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Comparers
{
    /// <summary>
    /// Funktionalitet til sammenligning af to ViewModels for adressekonti.
    /// </summary>
    public class AdressekontoViewModelComparer : IComparer<IAdressekontoViewModel>
    {
        #region Methods

        /// <summary>
        /// Sammenligner to ViewModels for adressekonti.
        /// </summary>
        /// <param name="x">ViewModel for adressekonto.</param>
        /// <param name="y">ViewModel for adressekonto.</param>
        /// <returns>Sammenligningsresultat.</returns>
        public virtual int Compare(IAdressekontoViewModel x, IAdressekontoViewModel y)
        {
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            var result = x.StatusDato.Date.CompareTo(y.StatusDato.Date);
            if (result != 0)
            {
                return result <= 0 ? Math.Abs(result) : result*-1;
            }
            result = string.Compare(x.Navn, y.Navn, StringComparison.CurrentCultureIgnoreCase);
            if (result != 0)
            {
                return result;
            }
            result = x.Saldo.CompareTo(y.Saldo);
            return result <= 0 ? Math.Abs(result) : result*-1;
        }

        #endregion
    }
}
