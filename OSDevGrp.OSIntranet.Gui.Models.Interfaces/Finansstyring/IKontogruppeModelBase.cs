using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en model, der indeholder de grundlæggende oplysninger for en kontogruppe.
    /// </summary>
    public interface IKontogruppeModelBase : ITabelModelBase
    {
        /// <summary>
        /// Unik identifikation af kontogruppen.
        /// </summary>
        int Nummer
        {
            get;
        }
    }
}
