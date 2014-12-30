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
        /// Månedstekst for statusdatoen.
        /// </summary>
        string StatusDatoAsMonthText
        {
            get;
        }

        /// <summary>
        /// Månedstekst for forrige måned i forhold til statusdato.
        /// </summary>
        string StatusDatoAsLastMonthText
        {
            get;
        }

        /// <summary>
        /// Tekst for år til dato i forhold til statusdato.
        /// </summary>
        string StatusDatoAsYearToDateText
        {
            get;
        }

        /// <summary>
        /// Tekst for sidste år i forhold til statusdato.
        /// </summary>
        string StatusDatoAsLastYearText
        {
            get;
        }

        /// <summary>
        /// Konti.
        /// </summary>
        IEnumerable<IKontoViewModel> Konti
        {
            get;
        }

        /// <summary>
        /// Konti fordelt på kontogrupper.
        /// </summary>
        IEnumerable<KeyValuePair<IKontogruppeViewModel, IEnumerable<IKontoViewModel>>> KontiGrouped
        {
            get;
        }

        /// <summary>
        /// Topbenyttede konti.
        /// </summary>
        IEnumerable<IKontoViewModel> KontiTop
        {
            get;
        }

        /// <summary>
        /// Topbenyttede konti fordelt på kontogrupper.
        /// </summary>
        IEnumerable<KeyValuePair<IKontogruppeViewModel, IEnumerable<IKontoViewModel>>> KontiTopGrouped
        {
            get;
        }

        /// <summary>
        /// Overskrift til konti.
        /// </summary>
        string KontiHeader
        {
            get;
        }

        /// <summary>
        /// Kolonneoverskrifter til konti.
        /// </summary>
        IEnumerable<string> KontiColumns
        {
            get;
        }

        /// <summary>
        /// Budgetkonti.
        /// </summary>
        IEnumerable<IBudgetkontoViewModel> Budgetkonti
        {
            get;
        }

        /// <summary>
        /// Budgetkonti fordelt på kontogrupper til budetkonti.
        /// </summary>
        IEnumerable<KeyValuePair<IBudgetkontogruppeViewModel, IEnumerable<IBudgetkontoViewModel>>> BudgetkontiGrouped
        {
            get;
        }

        /// <summary>
        /// Topbenyttede budgetkonti.
        /// </summary>
        IEnumerable<IBudgetkontoViewModel> BudgetkontiTop
        {
            get;
        }

        /// <summary>
        /// Topbenyttede budgetkonti fordelt på kontogrupper til budetkonti.
        /// </summary>
        IEnumerable<KeyValuePair<IBudgetkontogruppeViewModel, IEnumerable<IBudgetkontoViewModel>>> BudgetkontiTopGrouped
        {
            get;
        }

        /// <summary>
        /// Overskrift til budgetkonti.
        /// </summary>
        string BudgetkontiHeader
        {
            get;
        }

        /// <summary>
        /// Kolonneoverskrifter til budgetkonti.
        /// </summary>
        IEnumerable<string> BudgetkontiColumns
        {
            get;
        }

        /// <summary>
        /// Bogføringslinjer.
        /// </summary>
        IEnumerable<IReadOnlyBogføringslinjeViewModel> Bogføringslinjer
        {
            get;
        }

        /// <summary>
        /// Overskrift til bogføringslinjer.
        /// </summary>
        string BogføringslinjerHeader
        {
            get;
        }

        /// <summary>
        /// Kolonneoverskrifter til bogføringslinjer.
        /// </summary>
        IEnumerable<string> BogføringslinjerColumns
        {
            get;
        }

        /// <summary>
        /// ViewModel, hvorfra der kan bogføres.
        /// </summary>
        IBogføringViewModel Bogføring
        {
            get; 
        }

        /// <summary>
        /// Overskrift til en ViewModel, hvorfra der kan bogføres.
        /// </summary>
        string BogføringHeader
        {
            get;
        }

        /// <summary>
        /// Kommando, der kan sætte en ny ViewModel, hvorfra der kan bogføres.
        /// </summary>
        ITaskableCommand BogføringSetCommand
        {
            get;
        }

        /// <summary>
        /// Bogføringsadvarsler.
        /// </summary>
        IEnumerable<IBogføringsadvarselViewModel> Bogføringsadvarsler
        {
            get;
        }

        /// <summary>
        /// Overskrift til bogføringsadvarsler.
        /// </summary>
        string BogføringsadvarslerHeader
        {
            get;
        }

        /// <summary>
        /// Linjer, der indgår i årsopgørelsen.
        /// </summary>
        IEnumerable<IOpgørelseViewModel> Opgørelseslinjer
        {
            get;
        }

        /// <summary>
        /// Overskrift til linjer, der indgår i årsopgørelsen.
        /// </summary>
        string OpgørelseslinjerHeader
        {
            get;
        }

        /// <summary>
        /// Kolonneoverskrifter til linjer, der indgår i årsopgørelsen.
        /// </summary>
        IEnumerable<string> OpgørelseslinjerColumns
        {
            get;
        }

        /// <summary>
        /// Budgetteret beløb fra årsopgørelsen.
        /// </summary>
        decimal Budget
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af budgetteret beløb fra årsopgørelsen.
        /// </summary>
        string BudgetAsText
        {
            get;
        }

        /// <summary>
        /// Label til budgetteret beløb fra årsopgørelsen.
        /// </summary>
        string BudgetLabel
        {
            get;
        }

        /// <summary>
        /// Budgetteret beløb for sidste måned fra årsopgørelsen.
        /// </summary>
        decimal BudgetSidsteMåned
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af budgetteret beløb for sidste måned fra årsopgørelsen.
        /// </summary>
        string BudgetSidsteMånedAsText
        {
            get;
        }

        /// <summary>
        /// Label til budgetteret beløb for sidste måned fra årsopgørelsen.
        /// </summary>
        string BudgetSidsteMånedLabel
        {
            get;
        }

        /// <summary>
        /// Overskrift til balancen.
        /// </summary>
        string BalanceHeader
        {
            get;
        }

        /// <summary>
        /// Linjer, der indgår i balancens aktiver.
        /// </summary>
        IEnumerable<IBalanceViewModel> Aktiver
        {
            get;
        }

        /// <summary>
        /// Overskrift til linjer, der indgår i balancens aktiver.
        /// </summary>
        string AktiverHeader
        {
            get;
        }

        /// <summary>
        /// Kolonneoverskrifter til linjer, der indgår i balancens aktiver.
        /// </summary>
        IEnumerable<string> AktiverColumns
        {
            get;
        }

        /// <summary>
        /// Aktiver i alt.
        /// </summary>
        decimal AktiverIAlt
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af aktiver i alt.
        /// </summary>
        string AktiverIAltAsText
        {
            get;
        }

        /// <summary>
        /// Label til aktiver i alt.
        /// </summary>
        string AktiverIAltLabel
        {
            get;
        }

        /// <summary>
        /// Linjer, der indgår i balancens passiver.
        /// </summary>
        IEnumerable<IBalanceViewModel> Passiver
        {
            get;
        }

        /// <summary>
        /// Overskrift til linjer, der indgår i balancens passiver.
        /// </summary>
        string PassiverHeader
        {
            get;
        }

        /// <summary>
        /// Kolonneoverskrifter til linjer, der indgår i balancens aktiver.
        /// </summary>
        IEnumerable<string> PassiverColumns
        {
            get;
        }

        /// <summary>
        /// Passiver i alt.
        /// </summary>
        decimal PassiverIAlt
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af passiver i alt.
        /// </summary>
        string PassiverIAltAsText
        {
            get;
        }

        /// <summary>
        /// Label til passiver i alt.
        /// </summary>
        string PassiverIAltLabel
        {
            get;
        }

        /// <summary>
        /// Debitorer.
        /// </summary>
        IEnumerable<IAdressekontoViewModel> Debitorer
        {
            get;
        }

        /// <summary>
        /// Overskrift til debitorer.
        /// </summary>
        string DebitorerHeader
        {
            get;
        }

        /// <summary>
        /// Kreditorer.
        /// </summary>
        IEnumerable<IAdressekontoViewModel> Kreditorer
        {
            get;
        }

        /// <summary>
        /// Overskrift til kreditorer.
        /// </summary>
        string KreditorerHeader
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
        /// Overskrift til nyheder.
        /// </summary>
        string NyhederHeader
        {
            get;
        }

        /// <summary>
        /// Kontogrupper.
        /// </summary>
        IEnumerable<IKontogruppeViewModel> Kontogrupper
        {
            get;
        }

        /// <summary>
        /// Kontogrupper til budgetkonti.
        /// </summary>
        IEnumerable<IBudgetkontogruppeViewModel> Budgetkontogrupper
        {
            get;
        }

        /// <summary>
        /// Tilføjer en konto til regnskabet.
        /// </summary>
        /// <param name="kontoViewModel">ViewModel for kontoen, der skal tilføjes regnskabet.</param>
        void KontoAdd(IKontoViewModel kontoViewModel);

        /// <summary>
        /// Tilføjer en budgetkonto til regnskabet.
        /// </summary>
        /// <param name="budgetkontoViewModel">ViewModel for budgetkontoen, der skal tilføjes regnskabet.</param>
        void BudgetkontoAdd(IBudgetkontoViewModel budgetkontoViewModel);

        /// <summary>
        /// Tilføjer en bogføringslinje til regnskabet.
        /// </summary>
        /// <param name="bogføringslinjeViewModel">ViewModel for bogføringslinjen, der skal tilføjes regnskabet.</param>
        void BogføringslinjeAdd(IReadOnlyBogføringslinjeViewModel bogføringslinjeViewModel);

        /// <summary>
        /// Sætter en ny ViewModel, hvorfra der kan bogføres.
        /// </summary>
        /// <param name="bogføringViewModel">Ny ViewModel, hvorfra der kan bogføres.</param>
        void BogføringSet(IBogføringViewModel bogføringViewModel);

        /// <summary>
        /// Tilføjer en bogføringsadvarsel til regnskabet.
        /// </summary>
        /// <param name="bogføringsadvarselViewModel">ViewModel for bogføringsadvarslen, der skal tilføjes regnskabet.</param>
        void BogføringsadvarselAdd(IBogføringsadvarselViewModel bogføringsadvarselViewModel);

        /// <summary>
        /// Fjerner en bogføringsadvarsel fra regnskabet.
        /// </summary>
        /// <param name="bogføringsadvarselViewModel">ViewModel for bogføringsadvarslen, der skal fjernes fra regnskabet.</param>
        void BogføringsadvarselRemove(IBogføringsadvarselViewModel bogføringsadvarselViewModel);

        /// <summary>
        /// Tilføjerer en debitor til regnskabet.
        /// </summary>
        /// <param name="adressekontoViewModel">ViewModel for adressekontoen, der skal tilføjes som debitor.</param>
        void DebitorAdd(IAdressekontoViewModel adressekontoViewModel);

        /// <summary>
        /// Tilføjerer en kreditor til regnskabet.
        /// </summary>
        /// <param name="adressekontoViewModel">ViewModel for adressekontoen, der skal tilføjes som kreditor.</param>
        void KreditorAdd(IAdressekontoViewModel adressekontoViewModel);

        /// <summary>
        /// Tilføjer en nyhed til regnskabet.
        /// </summary>
        /// <param name="nyhedViewModel">ViewModel for nyheden, der skal tilføjes regnskabet.</param>
        void NyhedAdd(INyhedViewModel nyhedViewModel);

        /// <summary>
        /// Tilføjer en kontogruppe til regnskabet.
        /// </summary>
        /// <param name="kontogruppeViewModel">ViewModel for kontogruppen, der skal tilføjes.</param>
        void KontogruppeAdd(IKontogruppeViewModel kontogruppeViewModel);

        /// <summary>
        /// Tilføjer en kontogruppe for budgetkonti til regnskabet.
        /// </summary>
        /// <param name="budgetkontogruppeViewModel">ViewModel for budgetkontogruppen, der skal tilføjes.</param>
        void BudgetkontogruppeAdd(IBudgetkontogruppeViewModel budgetkontogruppeViewModel);
    }
}
