using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands
{
    /// <summary>
    /// Kommando, der kan udføre en til flere kommandoer.
    /// </summary>
    public class CommandCollectionExecuterCommand : CommandBase
    {
        #region Private variables

        private readonly IEnumerable<ICommand> _commands;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner kommando, der kan udføre en til flere kommandoer.
        /// </summary>
        /// <param name="commands">Kommandoer, skal udføres, når denne kommando udføres.</param>
        public CommandCollectionExecuterCommand(IEnumerable<ICommand> commands)
        {
            if (commands == null)
            {
                throw new ArgumentNullException("commands");
            }
            _commands = commands;
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
            var commands = _commands.ToList();
            return commands.Count != 0 && commands.Any(m => m.CanExecute(parameter));
        }

        /// <summary>
        /// Udfører kommandoen.
        /// </summary>
        /// <param name="parameter">Parameter til kommandoen.</param>
        public override void Execute(object parameter)
        {
            foreach (var command in _commands.Where(m => m.CanExecute(parameter)))
            {
                command.Execute(parameter);
            }
        }

        #endregion
    }
}
