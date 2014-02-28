using System;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en uredigerbar ViewModel for en bogføringslinje.
    /// </summary>
    public interface IReadOnlyBogføringslinjeViewModel : IBogføringslinjeViewModel
    {
        /// <summary>
        /// Bogføringstidspunkt.
        /// </summary>
        DateTime Dato
        {
            get;
        }

        /// <summary>
        /// Bilagsnummer.
        /// </summary>
        string Bilag
        {
            get;
        }

        /// <summary>
        /// Kontonummer, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string Kontonummer
        {
            get;
        }

        /// <summary>
        /// Tekst.
        /// </summary>
        string Tekst
        {
            get;
        }

        /// <summary>
        /// Budgetkontonummer, hvortil bogføringslinjen er tilknyttet.
        /// </summary>
        string Budgetkontonummer
        {
            get;
        }

        /// <summary>
        /// Debitbeløb.
        /// </summary>
        decimal Debit
        {
            get;
        }

        /// <summary>
        /// Kreditbeløb.
        /// </summary>
        decimal Kredit
        {
            get;
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
        }
    }
}
