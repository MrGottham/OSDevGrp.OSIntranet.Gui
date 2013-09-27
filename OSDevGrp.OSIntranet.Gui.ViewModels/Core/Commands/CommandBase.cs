using System;
using System.Windows.Input;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands
{
    /// <summary>
    /// Basisfunktionalitet til en kommando.
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        #region Events
        
        /// <summary>
        /// Event, der rejses, når der sker en ændring af, om kommandoen kan udføres,
        /// </summary>
        public virtual event EventHandler CanExecuteChanged
        {
            add
            {
                //CommandManager.RequerySuggested += value;
            }
            remove
            {
                //CommandManager.RequerySuggested -= value;
            }
        }
        
        #endregion
        
        #region Methods

        /// <summary>
        /// Returnerer angivelse af, om kommandoen kan udføres.
        /// </summary>
        /// <param name="parameter">Parameter til kommandoen.</param>
        /// <returns>Angivelse af, om kommandoen kan udføres.</returns>
        public abstract bool CanExecute(object parameter);

        /// <summary>
        /// Udfører kommandoen.
        /// </summary>
        /// <param name="parameter">Parameter til kommandoen.</param>
        public abstract void Execute(object parameter);

        #endregion
    }
}
