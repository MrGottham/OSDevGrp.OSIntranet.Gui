using System;
using System.Windows.Input;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en ViewModel, der indeholder en advarsel ved en bogføring.
    /// </summary>
    public interface IBogføringsadvarselViewModel : IViewModel
    {
        /// <summary>
        /// Regnskabet, som adressekontoen er tilknyttet.
        /// </summary>
        IRegnskabViewModel Regnskab
        {
            get;
        }

        /// <summary>
        /// Bogføringslinjen, der har medført advarslen.
        /// </summary>
        IReadOnlyBogføringslinjeViewModel Bogføringslinje
        {
            get;
        }

        /// <summary>
        /// Tidspunkt for advarslen, som er opstået ved bogføring.
        /// </summary>
        DateTime Tidspunkt
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af tidspunktet for advarslen, som er opstået ved bogføring.
        /// </summary>
        string TidspunktAsText
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af advarslen, som er opstået ved bogføring.
        /// </summary>
        string Advarsel
        {
            get;
        }

        /// <summary>
        /// Kontonummer på kontoen, hvorpå advarslen er opstået ved bogføring.
        /// </summary>
        string Kontonummer
        {
            get;
        }

        /// <summary>
        /// Kontonavnet på kontoen, hvorpå advarslen er opstået ved bogføring.
        /// </summary>
        string Kontonavn
        {
            get;
        }

        /// <summary>
        /// Beløbet, der har medført advarslen.
        /// </summary>
        decimal Beløb
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af beløbet, der har medført advarslen.
        /// </summary>
        string BeløbAsText
        {
            get;
        }

        /// <summary>
        /// Samlet information for advarslen.
        /// </summary>
        string Information
        {
            get;
        }

        /// <summary>
        /// Kommando, der fjerner advarslen fra regnskabet, hvorpå den er tilknyttet.
        /// </summary>
        ICommand RemoveCommand
        {
            get;
        }

        /// <summary>
        /// Label til kommandoen, der fjerner advarslen fra regnskabet, hvorpå den er tilknyttet.
        /// </summary>
        string RemoveCommandLabel
        {
            get;
        }

        /// <summary>
        /// Billede, der illustrerer en advarsel ved en bogføring.
        /// </summary>
        byte[] Image
        {
            get;
        }
    }
}
