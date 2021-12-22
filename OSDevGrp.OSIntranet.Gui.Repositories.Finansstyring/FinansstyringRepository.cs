using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring
{
    /// <summary>
    /// Repository, der supporterer finansstyring.
    /// </summary>
    public class FinansstyringRepository : IFinansstyringRepository
    {
        #region Constructor

        /// <summary>
        /// Danner repository, der supporterer finansstyring.
        /// </summary>
        /// <param name="finansstyringKonfigurationRepository">Implementering af konfigurationsrepository, der supporterer finansstyring.</param>
        public FinansstyringRepository(IFinansstyringKonfigurationRepository finansstyringKonfigurationRepository)
        {
            if (finansstyringKonfigurationRepository == null)
            {
                throw new ArgumentNullException(nameof(finansstyringKonfigurationRepository));
            }

            Konfiguration = finansstyringKonfigurationRepository;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returnerer konfigurationsrepositoryet, der supporterer finansstyring.
        /// </summary>
        public virtual IFinansstyringKonfigurationRepository Konfiguration { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Henter en liste af regnskaber.
        /// </summary>
        /// <returns>Liste af regnskaber.</returns>
        public virtual Task<IEnumerable<IRegnskabModel>> RegnskabslisteGetAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Henter kontoplanen til et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvortil kontoplanen skal hentes.</param>
        /// <param name="statusDato">Statusdato for kontoplanen.</param>
        /// <returns>Kontoplan.</returns>
        public virtual Task<IEnumerable<IKontoModel>> KontoplanGetAsync(int regnskabsnummer, DateTime statusDato)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Henter en given konto i et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvorfra kontoen skal hentes.</param>
        /// <param name="kontonummer">Kontonummer på kontoen, der skal hentes.</param>
        /// <param name="statusDato">Statusdato for kontoen.</param>
        /// <returns>Konto.</returns>
        public virtual Task<IKontoModel> KontoGetAsync(int regnskabsnummer, string kontonummer, DateTime statusDato)
        {
            if (string.IsNullOrWhiteSpace(kontonummer))
            {
                throw new ArgumentNullException(nameof(kontonummer));
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Henter budgetkontoplanen til et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvortil budgetkontoplanen skal hentes.</param>
        /// <param name="statusDato">Statusdato for budgetkontoplanen.</param>
        /// <returns>Budgetkontoplan.</returns>
        public virtual Task<IEnumerable<IBudgetkontoModel>> BudgetkontoplanGetAsync(int regnskabsnummer, DateTime statusDato)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Henter en given budgetkonto i et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvorfra budgetkontoen skal hentes.</param>
        /// <param name="budgetkontonummer">Kontonummer på budgetkontoen, der skal hentes.</param>
        /// <param name="statusDato">Statusdato for budgetkontoen.</param>
        /// <returns>Budgetkonto.</returns>
        public virtual Task<IBudgetkontoModel> BudgetkontoGetAsync(int regnskabsnummer, string budgetkontonummer, DateTime statusDato)
        {
            if (string.IsNullOrWhiteSpace(budgetkontonummer))
            {
                throw new ArgumentNullException(nameof(budgetkontonummer));
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Henter et givent antal bogføringslinjer til et regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvortil bogføringslinjer skal hentes.</param>
        /// <param name="statusDato">Dato, hvorfra bogføringslinjer skal hentes.</param>
        /// <param name="antalBogføringslinjer">Antal bogføringslinjer, der skal hentes.</param>
        /// <returns>Bogføringslinjer til regnskabet.</returns>
        public virtual Task<IEnumerable<IBogføringslinjeModel>> BogføringslinjerGetAsync(int regnskabsnummer, DateTime statusDato, int antalBogføringslinjer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Danner og returnerer en ny bogføringslinje, der efterfølgende kan bogføres.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, som den nye bogføringslinje skal være tilknyttet.</param>
        /// <param name="dato">Bogføringsdato, som den nye bogføringslinje skal initieres med.</param>
        /// <param name="kontonummer">Kontonummer, som den nye bogføringslinje skal initieres med.</param>
        /// <returns>Ny bogføringslinje, der efterfølgende kan bogføres.</returns>
        public virtual Task<IBogføringslinjeModel> BogføringslinjeCreateNewAsync(int regnskabsnummer, DateTime dato, string kontonummer)
        {
            try
            {
                return Task.FromResult(BogføringslinjeModel.CreateNew(regnskabsnummer, dato, kontonummer));
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (IntranetGuiOfflineRepositoryException)
            {
                throw;
            }
            catch (IntranetGuiRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "BogføringslinjeCreateNewAsync", ex.Message), ex);
            }
        }

        /// <summary>
        /// Bogfører én til flere bogføringslinjer.
        /// </summary>
        /// <param name="bogføringslinjer">De bogføringslinjer, der skal bogføres.</param>
        /// <returns>Bogføringsresultater for de enkelte bogførte bogføringslinjer.</returns>
        public virtual Task<IEnumerable<IBogføringsresultatModel>> BogførAsync(params IBogføringslinjeModel[] bogføringslinjer)
        {
            if (bogføringslinjer == null)
            {
                throw new ArgumentNullException(nameof(bogføringslinjer));
            }

            if (bogføringslinjer.Any(m => m.Regnskabsnummer <= 0))
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "Regnskabsnummer", bogføringslinjer.First(m => m.Regnskabsnummer <= 0).Regnskabsnummer), nameof(bogføringslinjer));
            }

            if (bogføringslinjer.Any(m => m.Dato > DateTime.Now))
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "Dato", bogføringslinjer.First(m => m.Dato > DateTime.Now).Dato), nameof(bogføringslinjer));
            }

            if (bogføringslinjer.Any(m => string.IsNullOrWhiteSpace(m.Kontonummer)))
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "Kontonummer", bogføringslinjer.First(m => string.IsNullOrWhiteSpace(m.Kontonummer)).Kontonummer), nameof(bogføringslinjer));
            }

            if (bogføringslinjer.Any(m => string.IsNullOrWhiteSpace(m.Tekst)))
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "Tekst", bogføringslinjer.First(m => string.IsNullOrWhiteSpace(m.Tekst)).Tekst), nameof(bogføringslinjer));
            }

            if (bogføringslinjer.Any(m => m.Debit < 0M))
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "Debit", bogføringslinjer.First(m => m.Debit < 0M).Debit), nameof(bogføringslinjer));
            }

            if (bogføringslinjer.Any(m => m.Kredit < 0M))
            {
                throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "Kredit", bogføringslinjer.First(m => m.Kredit < 0M).Kredit), nameof(bogføringslinjer));
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Henter listen af debitorer til et regnskab.
        /// </summary>
        /// <param name="regnskabsummer">Regnskabsnummer, hvortil listen af debitorer skal hentes.</param>
        /// <param name="statusDato">Statusdato for listen af debitorer.</param>
        /// <returns>Debitorer i regnskabet.</returns>
        public virtual Task<IEnumerable<IAdressekontoModel>> DebitorlisteGetAsync(int regnskabsummer, DateTime statusDato)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Henter listen af kreditorer til et regnskab.
        /// </summary>
        /// <param name="regnskabsummer">Regnskabsnummer, hvortil listen af kreditorer skal hentes.</param>
        /// <param name="statusDato">Statusdato for listen af kreditorer.</param>
        /// <returns>Kreditorer i regnskabet.</returns>
        public virtual Task<IEnumerable<IAdressekontoModel>> KreditorlisteGetAsync(int regnskabsummer, DateTime statusDato)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Henter listen af adressekonti til en regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvortil listen af adressekonti skal hentes.</param>
        /// <param name="statusDato">Statusdato for listen af adressekonti.</param>
        /// <returns>Adressekonti til regnskabet.</returns>
        public virtual Task<IEnumerable<IAdressekontoModel>> AdressekontolisteGetAsync(int regnskabsnummer, DateTime statusDato)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Henter en given adressekonto til et regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer, hvortil adressekontoen skal hentes.</param>
        /// <param name="nummer">Unik identifikation af adressekontoen.</param>
        /// <param name="statusDato">Statusdato, hvorpå adressekontoen skal hentes.</param>
        /// <returns>Adressekonto.</returns>
        public virtual Task<IAdressekontoModel> AdressekontoGetAsync(int regnskabsnummer, int nummer, DateTime statusDato)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Henter listen af kontogrupper.
        /// </summary>
        /// <returns>Kontogrupper.</returns>
        public virtual Task<IEnumerable<IKontogruppeModel>> KontogruppelisteGetAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Henter listen af grupper til budgetkonti.
        /// </summary>
        /// <returns>Grupper til budgetkonti.</returns>
        public virtual Task<IEnumerable<IBudgetkontogruppeModel>> BudgetkontogruppelisteGetAsync()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}