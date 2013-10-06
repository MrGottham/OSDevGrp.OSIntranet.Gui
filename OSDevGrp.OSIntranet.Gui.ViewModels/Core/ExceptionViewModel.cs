using System;
using System.Text;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Core
{
    /// <summary>
    /// ViewModel for en præsenterbar exception.
    /// </summary>
    public class ExceptionViewModel : ViewModelBase, IExceptionViewModel
    {
        #region Private variables

        private readonly Exception _exception;

        #endregion
        
        #region Constructor

        /// <summary>
        /// Danner ViewModel for en præsenterbar exception.
        /// </summary>
        /// <param name="exception">Præsenterbar exception.</param>
        public ExceptionViewModel(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            _exception = exception;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Fejlbesked.
        /// </summary>
        public virtual string Message
        {
            get
            {
                return _exception.Message;
            }
        }

        /// <summary>
        /// Detaljeret fejlbesked.
        /// </summary>
        public virtual string Details
        {
            get
            {
                var detailsBuilder = new StringBuilder(_exception.Message);
                if (string.IsNullOrEmpty(_exception.StackTrace) == false)
                {
                    detailsBuilder.AppendLine();
                    detailsBuilder.AppendLine(_exception.StackTrace);
                }
                var innerException = _exception.InnerException;
                while (innerException != null)
                {
                    detailsBuilder.AppendLine();
                    detailsBuilder.AppendLine(innerException.Message);
                    if (string.IsNullOrEmpty(innerException.StackTrace) == false)
                    {
                        detailsBuilder.AppendLine(innerException.StackTrace);
                    }
                    innerException = innerException.InnerException;
                }
                return detailsBuilder.ToString();
            }
        }

        /// <summary>
        ///  Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return Resource.GetText(Text.Exception);
            }
        }

        #endregion
    }
}
