using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en model, der indeholder resultatet af en bogføring.
    /// </summary>
    public interface IBogføringsresultatModel : IModel
    {
        /// <summary>
        /// Bogført linje.
        /// </summary>
        IBogføringslinjeModel Bogføringslinje
        {
            get;
        }

        /// <summary>
        /// Bogføringsadvarsler, som den bogførte linje har medført.
        /// </summary>
        IEnumerable<IBogføringsadvarselModel> Bogføringsadvarsler
        {
            get;
        }
    }
}
