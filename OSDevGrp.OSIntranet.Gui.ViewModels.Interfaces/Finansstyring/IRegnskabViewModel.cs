namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en ViewModel for et regnskab.
    /// </summary>
    public interface IRegnskabViewModel : IViewModel
    {
        /// <summary>
        /// Regnskabsnummer.
        /// </summary>
        int Nummer
        {
            get;
        }

        /// <summary>
        /// Navn.
        /// </summary>
        string Navn
        {
            get; 
            set;
        }
    }
}
