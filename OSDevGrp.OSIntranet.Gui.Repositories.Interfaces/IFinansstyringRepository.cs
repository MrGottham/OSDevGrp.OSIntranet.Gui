using System.Collections.Generic;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Interfaces
{
    /// <summary>
    /// Interface for et repository, der supporterer finansstyring.
    /// </summary>
    public interface IFinansstyringRepository
    {
        /// <summary>
        /// Henter en liste af regnskaber.
        /// </summary>
        /// <returns>Liste af regnskaber.</returns>
        Task<IEnumerable<IRegnskabModel>> RegnskabslisteGetAsync();
    }
}
