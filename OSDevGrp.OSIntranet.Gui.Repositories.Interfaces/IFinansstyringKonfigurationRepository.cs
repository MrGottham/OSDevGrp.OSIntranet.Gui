using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Interfaces
{
    /// <summary>
    /// Interface for et konfigurationsrepository, der supporterer finansstyring.
    /// </summary>
    public interface IFinansstyringKonfigurationRepository
    {
        /// <summary>
        /// Uri til servicen, der supporterer finansstyring.
        /// </summary>
        Uri FinansstyringServiceUri
        {
            get; 
        }

        /// <summary>
        /// Antal bogføringslinjer, der skal hentes.
        /// </summary>
        int AntalBogføringslinjer
        {
            get;
        }

        /// <summary>
        /// Antal dage, som nyheder er gældende.
        /// </summary>
        int DageForNyheder
        {
            get;
        }

        /// <summary>
        /// Tilføjer konfigurationer til repositoryet.
        /// </summary>
        /// <param name="konfigurationer">Konfigurationer, der skal tilføjes.</param>
        void KonfigurationerAdd(IDictionary<string, object> konfigurationer);
    }
}
