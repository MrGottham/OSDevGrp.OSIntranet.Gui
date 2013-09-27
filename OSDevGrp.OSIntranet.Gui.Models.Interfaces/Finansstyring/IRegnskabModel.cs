namespace OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til modellen for et regnskab.
    /// </summary>
    public interface IRegnskabModel : IModel
    {
        /// <summary>
        /// Regnskabsnummer.
        /// </summary>
        int Nummer
        {
            get;
        }

        /// <summary>
        /// Navnet på regnskabet.
        /// </summary>
        string Navn
        {
            get; 
            set; 
        }
    }
}
