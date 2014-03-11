namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core
{
    /// <summary>
    /// Interface til en ViewModel, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m.
    /// </summary>
    public interface ITabelViewModelBase : IViewModel
    {
        /// <summary>
        /// Unik identifikation af de grundlæggende tabeloplysningerne i denne model.
        /// </summary>
        string Id
        {
            get;
        }

        /// <summary>
        /// Teksten der beskriver de grundlæggende tabeloplysninger i denne model.
        /// </summary>
        string Tekst
        {
            get; 
            set;
        }
    }
}
