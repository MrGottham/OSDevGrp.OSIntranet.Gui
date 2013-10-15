using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Comparers
{
    /// <summary>
    /// Funktionalitet til sammenligning af to ViewModels for bogføringslinjer.
    /// </summary>
    public class BogføringslinjeViewModelComparer : IComparer<IReadOnlyBogføringslinjeViewModel>
    {
        #region Methods
        
        /// <summary>
        /// Sammenligninger to ViewModels for bogføringslinjer.
        /// </summary>
        /// <param name="x">ViewModel for en bogføringslinje.</param>
        /// <param name="y">ViewModel for en bogføringslinje.</param>
        /// <returns>Sammenligningsresultat.</returns>
        public int Compare(IReadOnlyBogføringslinjeViewModel x, IReadOnlyBogføringslinjeViewModel y)
        {
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            var result = x.Dato.CompareTo(y.Dato);
            if (result != 0)
            {
                return result < 0 ? Math.Abs(result) : result*-1;
            }
            result = x.Løbenummer.CompareTo(y.Løbenummer);
            return result <= 0 ? Math.Abs(result) : result*-1;
        }

        #endregion
    }
}
