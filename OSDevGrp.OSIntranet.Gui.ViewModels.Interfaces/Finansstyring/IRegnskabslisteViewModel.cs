using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en ViewModel for en liste indeholdende regnskaber.
    /// </summary>
    public interface IRegnskabslisteViewModel : IViewModel, IRefreshable
    {
        /// <summary>
        /// Statusdato for listen af regnskaber.
        /// </summary>
        DateTime StatusDato
        {
            get; 
            set;
        }

        /// <summary>
        /// Regnskaber.
        /// </summary>
        IEnumerable<IRegnskabViewModel> Regnskaber
        {
            get;
        }

        /// <summary>
        /// Tilføjer et regnskab til listen af regnskaber.
        /// </summary>
        /// <param name="regnskab">ViewModel for regnskabet, der skal tilføjes listen af regnskaber.</param>
        void RegnskabAdd(IRegnskabViewModel regnskab);

        /// <summary>
        /// Henter og returnerer en ViewModel til et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummeret, hvortil ViewModel for regnskabet skal returneres.</param>
        /// <returns>ViewModel for det givne regnskab.</returns>
        Task<IRegnskabViewModel> RegnskabGetAsync(int regnskabsnummer);
    }
}
