using System;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring
{
    /// <summary>
    ///  Interface til modellen for en adressekonto. 
    /// </summary>
    public interface IAdressekontoModel : INyhedModel
    {
        /// <summary>
        /// Regnskabsnummer, som adressekontoen er tilknyttet.
        /// </summary>
        int Regnskabsnummer
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
        /// Sekundær telefonnummer.
        /// </summary>
        string SekundærTelefon
        { 
            get; 
            set; 
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
        /// Opdaterer nyhedsaktualiteten for adressekontoen.
        /// </summary>
        /// <param name="nyhedsaktualitet">Nyhedsaktualitet, som adressekontoen skal opdateres med.</param>
        void SetNyhedsaktualitet(Nyhedsaktualitet nyhedsaktualitet);
    }
}
