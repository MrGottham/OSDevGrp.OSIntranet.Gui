using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.Models.Finansstyring
{
    /// <summary>
    /// Model, der indeholder resultatet af en bogføring.
    /// </summary>
    public class BogføringsresultatModel : ModelBase, IBogføringsresultatModel
    {
        #region Private variables

        private readonly IBogføringslinjeModel _bogføringslinjeModel;
        private readonly IEnumerable<IBogføringsadvarselModel> _bogføringsadvarselModels;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en model, der indeholder resultatet af en bogføring.
        /// </summary>
        /// <param name="bogføringslinje">Bogført linje.</param>
        /// <param name="bogføringsadvarsler">Bogføringsadvarsler, som den bogførte linje har medført.</param>
        public BogføringsresultatModel(IBogføringslinjeModel bogføringslinje, IEnumerable<IBogføringsadvarselModel> bogføringsadvarsler)
        {
            if (bogføringslinje == null)
            {
                throw new ArgumentNullException("bogføringslinje");
            }
            if (bogføringsadvarsler == null)
            {
                throw new ArgumentNullException("bogføringsadvarsler");
            }
            _bogføringslinjeModel = bogføringslinje;
            _bogføringsadvarselModels = bogføringsadvarsler;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Bogført linje.
        /// </summary>
        public virtual IBogføringslinjeModel Bogføringslinje
        {
            get
            {
                return _bogføringslinjeModel;
            }
        }

        /// <summary>
        /// Bogføringsadvarsler, som den bogførte linje har medført.
        /// </summary>
        public virtual IEnumerable<IBogføringsadvarselModel> Bogføringsadvarsler
        {
            get
            {
                return _bogføringsadvarselModels;
            }
        }

        #endregion
    }
}
