using System;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en ViewModel for en adressekonto.
    /// </summary>
    public interface IAdressekontoViewModel : IViewModel, IRefreshable
    {
        /// <summary>
        /// Regnskabet, som adressekontoen er tilknyttet.
        /// </summary>
        IRegnskabViewModel Regnskab
        {
            get;
        }

        /// <summary>
        /// Unik identifikation af adressekontoen.
        /// </summary>
        int Nummer
        {
            get;
        }

        /// <summary>
        /// Navn for adressekontoen.
        /// </summary>
        string Navn
        {
            get; 
            set; 
        }

        /// <summary>
        /// Primær telefonnummer.
        /// </summary>
        string PrimærTelefon
        {
            get; 
            set;
        }

        /// <summary>
        /// Label til primær telefonnummer.
        /// </summary>
        string PrimærTelefonLabel
        {
            get;
        }

        /// <summary>
        /// Sekundær telefonnummer.
        /// </summary>
        string SekundærTelefon
        {
            get; 
            set;
        }

        /// <summary>
        /// Label til sekundær telefonummer.
        /// </summary>
        string SekundærTelefonLabel
        {
            get;
        }

        /// <summary>
        /// Statusdato for opgørelsen.
        /// </summary>
        DateTime StatusDato
        {
            get; 
            set;
        }

        /// <summary>
        /// Saldo pr. statusdato.
        /// </summary>
        decimal Saldo
        {
            get; 
            set; 
        }

        /// <summary>
        /// Tekstangivelse af saldo pr. statusdato
        /// </summary>
        string SaldoAsText
        {
            get;
        }

        /// <summary>
        /// Label til saldo pr. statusdato.
        /// </summary>
        string SaldoLabel
        {
            get;
        }

        /// <summary>
        /// Billede, der illustrerer en adressekontoen.
        /// </summary>
        byte[] Image
        {
            get;
        }
    }
}
