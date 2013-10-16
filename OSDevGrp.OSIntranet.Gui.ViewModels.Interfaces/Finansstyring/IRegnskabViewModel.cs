using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en ViewModel for et regnskab.
    /// </summary>
    public interface IRegnskabViewModel : IViewModel, IRefreshable
    {
        /// <summary>
        /// Regnskabsnummer.
        /// </summary>
        int Nummer
        {
            get;
        }

        /// <summary>
        /// Navn.
        /// </summary>
        string Navn
        {
            get; 
            set;
        }

        /// <summary>
        /// Statusdato for regnskabet.
        /// </summary>
        DateTime StatusDato
        { 
            get; 
            set;
        }

        /// <summary>
        /// Bogføringslinjer.
        /// </summary>
        IEnumerable<IReadOnlyBogføringslinjeViewModel> Bogføringslinjer
        {
            get;
        }

        /// <summary>
        /// Nyheder.
        /// </summary>
        IEnumerable<INyhedViewModel> Nyheder
        {
            get;
        }

        /// <summary>
        /// Tilføjer en bogføringslinje til regnskabet.
        /// </summary>
        /// <param name="bogføringslinjeViewModel">ViewModel for bogføringslinjen, der skal tilføjes regnskabet.</param>
        void BogføringslinjeAdd(IReadOnlyBogføringslinjeViewModel bogføringslinjeViewModel);

        /// <summary>
        /// Tilføjer en nyhed til regnskabet.
        /// </summary>
        /// <param name="nyhedViewModel">ViewModel for nyheden, er skal tilføjes regnskabet.</param>
        void NyhedAdd(INyhedViewModel nyhedViewModel);
    }
}
