using System;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events
{
    /// <summary>
    /// Interface for argumenter til et event, der håndtere en exception i OS Intranet.
    /// </summary>
    public interface IHandleExceptionEventArgs : IIntranetGuiEventArgs
    {
        /// <summary>
        /// Exception, der skal håndteres.
        /// </summary>
        Exception Error
        {
            get;
        }

        /// <summary>
        /// Angivelse af, om exception er blevet håndteret.
        /// </summary>
        bool IsHandled
        {
            get; 
            set; 
        }
    }
}
