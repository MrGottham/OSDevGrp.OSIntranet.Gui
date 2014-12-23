using System;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en ViewModel indeholdende grundlæggende kontooplysninger.
    /// </summary>
    /// <typeparam name="TKontogruppeViewModel">Typen på den ViewModel, der indeholder kontogruppen.</typeparam>
    public interface IKontoViewModelBase<TKontogruppeViewModel> : IViewModel, IRefreshable where TKontogruppeViewModel : IKontogruppeViewModelBase
    {
        /// <summary>
        /// Regnskabet, som kontoen er tilknyttet.
        /// </summary>
        IRegnskabViewModel Regnskab
        {
            get;
        }

        /// <summary>
        /// Kontonummer.
        /// </summary>
        string Kontonummer
        {
            get;
        }

        /// <summary>
        /// Label til kontonummer.
        /// </summary>
        string KontonummerLabel
        {
            get;
        }

        /// <summary>
        /// Kontonavn.
        /// </summary>
        string Kontonavn
        {
            get; 
            set;
        }

        /// <summary>
        /// Label til kontonavn.
        /// </summary>
        string KontonavnLabel
        {
            get;
        }

        /// <summary>
        /// Beskrivelse.
        /// </summary>
        string Beskrivelse
        {
            get; 
            set; 
        }

        /// <summary>
        /// Notat.
        /// </summary>
        string Notat
        {
            get; 
            set;
        }

        /// <summary>
        /// Kontogruppe.
        /// </summary>
        TKontogruppeViewModel Kontogruppe
        {
            get; 
            set; 
        }

        /// <summary>
        /// Statusdato for opgørelse af kontoen.
        /// </summary>
        DateTime StatusDato
        {
            get; 
            set; 
        }

        /// <summary>
        /// Kontoens værdi pr. opgørelsestidspunktet.
        /// </summary>
        decimal Kontoværdi
        {
            get;
        }

        /// <summary>
        /// Billede, der illustrerer en kontoen.
        /// </summary>
        byte[] Image
        {
            get;
        }

        /// <summary>
        /// Angivelse af, om kontoen er registreret i forhold til opgørelsen og/eller balancen.
        /// </summary>
        bool ErRegistreret
        {
            get; 
            set;
        }
    }
}
