namespace OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core
{
    /// <summary>
    /// Interface til en model, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m.
    /// </summary>
    public interface ITabelModelBase : IModel
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
