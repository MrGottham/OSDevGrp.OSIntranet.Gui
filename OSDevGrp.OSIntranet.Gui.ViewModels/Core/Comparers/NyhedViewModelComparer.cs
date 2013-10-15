using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Core.Comparers
{
    /// <summary>
    /// Funktionalitet til sammenligning af to ViewModels for nyheder.
    /// </summary>
    public class NyhedViewModelComparer : IComparer<INyhedViewModel>
    {
        #region Methods
        
        /// <summary>
        /// Sammenligninger to ViewModels for nyheder.
        /// </summary>
        /// <param name="x">ViewModel for en nyhed.</param>
        /// <param name="y">ViewModel for en nyhed.</param>
        /// <returns>Sammenligningsresultat.</returns>
        public int Compare(INyhedViewModel x, INyhedViewModel y)
        {
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            var result = x.Nyhedsudgivelsestidspunkt.CompareTo(y.Nyhedsudgivelsestidspunkt);
            if (result != 0)
            {
                return result < 0 ? Math.Abs(result) : result*-1;
            }
            result = x.Nyhedsaktualitet.CompareTo(y.Nyhedsaktualitet);
            return result <= 0 ? Math.Abs(result) : result*-1;
        }

        #endregion
    }
}
