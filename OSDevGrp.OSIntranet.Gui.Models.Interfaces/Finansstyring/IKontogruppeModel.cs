namespace OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en model til en kontogruppe.
    /// </summary>
    public interface IKontogruppeModel : IKontogruppeModelBase
    {
        /// <summary>
        /// Angivelse af siden i balancen, hvor kontogruppen er placeret.
        /// </summary>
        Balancetype Balancetype
        {
            get; 
            set; 
        }
    }
}
