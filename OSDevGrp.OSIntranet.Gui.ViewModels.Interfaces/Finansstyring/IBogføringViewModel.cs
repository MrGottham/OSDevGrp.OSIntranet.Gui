using System;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en ViewModel, hvorfra der kan bogføres.
    /// </summary>
    public interface IBogføringViewModel : IViewModel
    {
        /// <summary>
        /// Regnskabet, som bogføringslinjen kan bogføres på.
        /// </summary>
        IRegnskabViewModel Regnskab
        {
            get;
        }

        /// <summary>
        /// Bogføringstidspunkt.
        /// </summary>
        DateTime Dato
        {
            get; 
        }

        /// <summary>
        /// Tekstangivelse af bogføringstidspunkt.
        /// </summary>
        string DatoAsText
        {
            get; 
            set;
        }

        /// <summary>
        /// Label til bogføringstidspunkt.
        /// </summary>
        string DatoLabel
        {
            get;
        }
    }
}
