using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Interfaces
{
    /// <summary>
    /// Interface for et repository, der supporterer finansstyring.
    /// </summary>
    public interface IFinansstyringRepository
    {
        /// <summary>
        /// Returnerer konfigurationsrepositoryet, der supporterer finansstyring.
        /// </summary>
        IFinansstyringKonfigurationRepository Konfiguration
        {
            get;
        }

        /// <summary>
        /// Henter en liste af regnskaber.
        /// </summary>
        /// <returns>Liste af regnskaber.</returns>
        Task<IEnumerable<IRegnskabModel>> RegnskabslisteGetAsync();

        /// <summary>
        /// Henter et givent antal bogføringslinjer til et regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvortil bogføringslinjer skal hentes.</param>
        /// <param name="statusDato">Dato, hvorfra bogføringslinjer skal hentes.</param>
        /// <param name="antalBogføringslinjer">Antal bogføringslinjer, der skal hentes.</param>
        /// <returns>Bogføringslinjer til regnskabet.</returns>
        Task<IEnumerable<IBogføringslinjeModel>> BogføringslinjerGetAsync(int regnskabsnummer, DateTime statusDato, int antalBogføringslinjer);

        /// <summary>
        /// Henter listen af debitorer til et regnskab.
        /// </summary>
        /// <param name="regnskabsummer">Regnskabsnummer, hvortil listen af debitorer skal hentes.</param>
        /// <param name="statusDato">Statusdato for listen af debitorer.</param>
        /// <returns>Debitorer i regnskabet.</returns>
        Task<IEnumerable<IAdressekontoModel>> DebitorlisteGetAsync(int regnskabsummer, DateTime statusDato);

        /// <summary>
        /// Henter listen af kreditorer til et regnskab.
        /// </summary>
        /// <param name="regnskabsummer">Regnskabsnummer, hvortil listen af kreditorer skal hentes.</param>
        /// <param name="statusDato">Statusdato for listen af kreditorer.</param>
        /// <returns>Kreditorer i regnskabet.</returns>
        Task<IEnumerable<IAdressekontoModel>> KreditorlisteGetAsync(int regnskabsummer, DateTime statusDato);

        /// <summary>
        /// Henter en given adressekonto til et regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvortil adressekontoen skal hentes.</param>
        /// <param name="nummer">Unik identifikation af adressekontoen.</param>
        /// <param name="statusDato">Statusdato, hvorpå adressekontoen skal hentes.</param>
        /// <returns>Adressekonto.</returns>
        Task<IAdressekontoModel> AdressekontoGetAsync(int regnskabsnummer, int nummer, DateTime statusDato);
    }
}
