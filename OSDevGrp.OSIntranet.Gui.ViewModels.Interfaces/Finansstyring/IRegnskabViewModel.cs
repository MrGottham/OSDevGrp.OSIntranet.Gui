﻿using System;
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
        /// Tilføjer en bogføringslinje til regnskabet.
        /// </summary>
        /// <param name="bogføringslinjeViewModel">ViewModel for bogføringslinjen, der skal tilføjes regnskabet.</param>
        void BogføringslinjeAdd(IReadOnlyBogføringslinjeViewModel bogføringslinjeViewModel);

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
