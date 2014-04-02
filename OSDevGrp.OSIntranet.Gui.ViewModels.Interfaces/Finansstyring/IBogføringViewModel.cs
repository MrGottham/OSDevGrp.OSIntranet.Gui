using System;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en ViewModel, hvorfra der kan bogføres.
    /// </summary>
    public interface IBogføringViewModel : IViewModel
    {
        /// <summary>
        /// Regnskabet, som bogføringslinjen kan bogføres på.
        /// </summary>
        IRegnskabViewModel Regnskab
        {
            get;
        }

        /// <summary>
        /// Bogføringstidspunkt.
        /// </summary>
        DateTime Dato
        {
            get; 
        }

        /// <summary>
        /// Tekstangivelse af bogføringstidspunkt.
        /// </summary>
        string DatoAsText
        {
            get; 
            set;
        }

        /// <summary>
        /// Label til bogføringstidspunkt.
        /// </summary>
        string DatoLabel
        {
            get;
        }

        /// <summary>
        /// Bilagsnummer.
        /// </summary>
        string Bilag 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Label til bilagsnummer.
        /// </summary>
        string BilagLabel
        {
            get; 
        }

        /// <summary>
        /// Kontonummer, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string Kontonummer
        {
            get; 
            set; 
        }

        /// <summary>
        /// Label til kontonummeret, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string KontonummerLabel
        {
            get; 
        }

        /// <summary>
        /// Navn på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string Kontonavn
        {
            get;
        }

        /// <summary>
        /// Label til navnet på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string KontonavnLabel
        {
            get;
        }

        /// <summary>
        /// Saldo på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        decimal KontoSaldo
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af saldo på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string KontoSaldoAsText
        {
            get;
        }

        /// <summary>
        /// Label til saldoen på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string KontoSaldoLabel
        {
            get;
        }

        /// <summary>
        /// Disponibel beløb på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        decimal KontoDisponibel
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse af disponibel beløb på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string KontoDisponibelAsText
        {
            get;
        }

        /// <summary>
        /// Label til disponibel beløb på kontoen, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string KontoDisponibelLabel
        {
            get;
        }
    }
}
