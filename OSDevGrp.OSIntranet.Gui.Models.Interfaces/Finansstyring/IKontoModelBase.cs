using System;

namespace OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en model indeholdende grundlæggende kontooplysninger.
    /// </summary>
    public interface IKontoModelBase : IModel
    {
        /// <summary>
        /// Regnskabsnummer, som kontoen er tilknyttet.
        /// </summary>
        int Regnskabsnummer
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
        /// Kontonavn.
        /// </summary>
        string Kontonavn
        {
            get; 
            set; 
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
        /// Statusdato for opgørelse af kontoen.
        /// </summary>
        DateTime StatusDato
        {
            get; 
            set; 
        }
    }
}
