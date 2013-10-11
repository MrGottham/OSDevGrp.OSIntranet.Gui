using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core
{
    /// <summary>
    /// Interface til en ViewModel for en exceptionhandler.
    /// </summary>
    public interface IExceptionHandlerViewModel : IViewModel
    {
        /// <summary>
        /// Seneste håndteret exception.
        /// </summary>
        IExceptionViewModel Last
        {
            get;
        }

        /// <summary>
        /// Angivelse af, om seneste håndterede exception skal vises.
        /// </summary>
        bool ShowLast
        {
            get; 
            set;
        }

        /// <summary>
        /// Exceptions, der er håndteret af exceptionhandleren.
        /// </summary>
        IEnumerable<IExceptionViewModel> Exceptions
        {
            get;
        }

        /// <summary>
        /// Tekstangivelse til kommandoen, der kan skjule seneste håndterede exception.
        /// </summary>
        string HideCommandText
        {
            get;
        }

        /// <summary>
        /// Kommando, der kan skjule seneste håndterede exception.
        /// </summary>
        ICommand HideCommand
        {
            get;
        }

        /// <summary>
        /// Håndterer en exception.
        /// </summary>
        /// <param name="exception">Exception, der skal håndteres.</param>
        void HandleException(Exception exception);
    }
}
