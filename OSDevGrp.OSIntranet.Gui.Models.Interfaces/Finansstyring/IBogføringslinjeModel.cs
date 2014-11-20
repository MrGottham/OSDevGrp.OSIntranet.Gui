using System;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til modellen for en bogføringslinje.
    /// </summary>
    public interface IBogføringslinjeModel : INyhedModel
    {
        /// <summary>
        /// Regnskabsnummer, som bogføringslinjen er tilknyttet.
        /// </summary>
        int Regnskabsnummer
        {
            get;
        }

        /// <summary>
        /// Unik identifikation af bogføringslinjen inden for regnskabet.
        /// </summary>
        int Løbenummer
        {
            get;
        }

        /// <summary>
        /// Bogføringstidspunkt.
        /// </summary>
        DateTime Dato
        {
            get; 
            set; 
        }

        /// <summary>
        /// Bilag.
        /// </summary>
        string Bilag
        { 
            get; 
            set; 
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
        /// Tekst.
        /// </summary>
        string Tekst
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Budgetkontonummer, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string Budgetkontonummer
        {
            get; 
            set;
        }

        /// <summary>
        /// Debitbeløb.
        /// </summary>
        decimal Debit
        {
            get; 
            set;
        }

        /// <summary>
        /// Kreditbeløb.
        /// </summary>
        decimal Kredit
        {
            get; 
            set;
        }

        /// <summary>
        /// Bogføringsbeløb.
        /// </summary>
        decimal Bogført
        {
            get;
        }

        /// <summary>
        /// Adressekonto, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        int Adressekonto
        {
            get; 
            set;
        }
    }
}
