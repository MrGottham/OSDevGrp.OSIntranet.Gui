namespace OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core
{
    /// <summary>
    /// Interface til en model for en nyhed.
    /// </summary>
    public interface INyhedModel : IModel
    {
        /// <summary>
        /// Nyhedsaktualitet.
        /// </summary>
        Nyhedsaktualitet Nyhedsaktualitet
        {
            get;
        }
    }
}
