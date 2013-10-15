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
            var xDate = x.Nyhedsudgivelsestidspunkt.Date;
            var yDate = y.Nyhedsudgivelsestidspunkt.Date;
            var result = xDate.CompareTo(yDate);
            if (result != 0)
            {
                return result < 0 ? Math.Abs(result) : result*-1;
            }
            result = x.Nyhedsaktualitet.CompareTo(y.Nyhedsaktualitet);
            if (result != 0)
            {
                return result < 0 ? Math.Abs(result) : result*-1;
            }
            var xTimeOfDay = x.Nyhedsudgivelsestidspunkt.TimeOfDay;
            var yTimeOfDay = y.Nyhedsudgivelsestidspunkt.TimeOfDay;
            result = xTimeOfDay.CompareTo(yTimeOfDay);
            return result <= 0 ? Math.Abs(result) : result*-1;
        }

        #endregion
    }
}
