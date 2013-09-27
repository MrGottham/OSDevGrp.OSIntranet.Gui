using System;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands
{
    /// <summary>
    /// Relay kommando.
    /// </summary>
    public class RelayCommand : CommandBase
    {
        #region Private variables

        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        #endregion
        
        #region Constructors

        /// <summary>
        /// Danner en relay kommando.
        /// </summary>
        /// <param name="execute">Handling, der skal udføres ved udførelse af kommandoen.</param>
        public RelayCommand(Action<object> execute) 
            : this(execute, null)
        {
        }

        /// <summary>
        /// Danner en relay kommando.
        /// </summary>
        /// <param name="execute">Handling, der skal udføres ved udførelse af kommandoen.</param>
        /// <param name="canExecute">Udtryk, der angiver, om kommandoen kan udføres.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returnerer angivelse af, om kommandoen kan udføres.
        /// </summary>
        /// <param name="parameter">Parameter til kommandoen.</param>
        /// <returns>Angivelse af, om kommandoen kan udføres.</returns>
        public override bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute.Invoke(parameter);
        }

        /// <summary>
        /// Udfører kommandoen.
        /// </summary>
        /// <param name="parameter">Parameter til kommandoen.</param>
        public override void Execute(object parameter)
        {
            _execute.Invoke(parameter);
        }
        
        #endregion
    }
}
