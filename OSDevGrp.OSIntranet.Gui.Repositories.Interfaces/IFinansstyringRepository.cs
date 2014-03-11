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
        /// Henter kontoplanen til et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvortil kontoplanen skal hentes.</param>
        /// <param name="statusDato">Statusdato for kontoplanen.</param>
        /// <returns>Kontoplan.</returns>
        Task<IEnumerable<IKontoModel>> KontoplanGetAsync(int regnskabsnummer, DateTime statusDato);

        /// <summary>
        /// Henter en given konto i et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvorfra kontoen skal hentes.</param>
        /// <param name="kontonummer">Kontonummer på kontoen, der skal hentes.</param>
        /// <param name="statusDato">Statusdato for kontoen.</param>
        /// <returns>Konto.</returns>
        Task<IKontoModel> KontoGetAsync(int regnskabsnummer, string kontonummer, DateTime statusDato);

        /// <summary>
        /// Henter budgetkontoplanen til et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvortil budgetkontoplanen skal hentes.</param>
        /// <param name="statusDato">Statusdato for budgetkontoplanen.</param>
        /// <returns>Budgetkontoplan.</returns>
        Task<IEnumerable<IBudgetkontoModel>> BudgetkontoplanGetAsync(int regnskabsnummer, DateTime statusDato);

        /// <summary>
        /// Henter en given budgetkonto i et givet regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvorfra budgetkontoen skal hentes.</param>
        /// <param name="budgetkontonummer">Kontonummer på budgetkontoen, der skal hentes.</param>
        /// <param name="statusDato">Statusdato for budgetkontoen.</param>
        /// <returns>Budgetkonto.</returns>
        Task<IBudgetkontoModel> BudgetkontoGetAsync(int regnskabsnummer, string budgetkontonummer, DateTime statusDato);

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

        /// <summary>
        /// Henter listen af kontogrupper.
        /// </summary>
        /// <returns>Kontogrupper.</returns>
        Task<IEnumerable<IKontogruppeModel>> KontogruppelisteGetAsync();

        /// <summary>
        /// Henter listen af grupper til budgetkonti.
        /// </summary>
        /// <returns>Grupper til budgetkonti.</returns>
        Task<IEnumerable<IBudgetkontogruppeModel>> BudgetkontogruppelisteGetAsync();
    }
}
