using System;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core
{
    /// <summary>
    /// Interface til en ViewModel for en nyhed.
    /// </summary>
    public interface INyhedViewModel : IViewModel
    {
        /// <summary>
        /// Nyhedsaktualitet.
        /// </summary>
        Nyhedsaktualitet Nyhedsaktualitet
        {
            get;
        }

        /// <summary>
        /// Udgivelsestidspunkt for nyheden.
        /// </summary>
        DateTime Nyhedsudgivelsestidspunkt
        {
            get;
        }

        /// <summary>
        /// Billede, der illustrerer nyheden.
        /// </summary>
        byte[] Image
        {
            get;
        }

        /// <summary>
        /// Detaljeret nyhedsinformation.
        /// </summary>
        string Nyhedsinformation
        {
            get;
        }
    }
}
