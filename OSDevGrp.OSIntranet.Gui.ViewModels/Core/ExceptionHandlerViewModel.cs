using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Core
{
    /// <summary>
    /// ViewModel for en exceptionhandler.
    /// </summary>
    public class ExceptionHandlerViewModel : ViewModelBase, IExceptionHandlerViewModel
    {
        #region Private variables

        private bool _showLast;
        private readonly ObservableCollection<IExceptionViewModel> _exceptions = new ObservableCollection<IExceptionViewModel>();
        private readonly SynchronizationContext _synchronizationContext;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner ViewModel for en exceptionhandler.
        /// </summary>
        public ExceptionHandlerViewModel()
        {
            _exceptions.CollectionChanged += ExceptionCollectionChangedEventHandler;
            _synchronizationContext = SynchronizationContext.Current;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Seneste håndteret exception.
        /// </summary>
        public virtual IExceptionViewModel Last
        {
            get
            {
                return Exceptions.LastOrDefault();
            }
        }

        /// <summary>
        /// Angivelse af, om seneste håndterede exception skal vises.
        /// </summary>
        public virtual bool ShowLast
        {
            get
            {
                if (Last == null)
                {
                    return false;
                }
                throw new NotImplementedException();
            }
            set
            {
                if (value == _showLast)
                {
                    return;
                }
                _showLast = value;
                RaisePropertyChanged("ShowLast");
            }
        }

        /// <summary>
        /// Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return Resource.GetText(Text.ExceptionHandler);
            }
        }

        /// <summary>
        /// Exceptions, der er håndteret af exceptionhandleren.
        /// </summary>
        public virtual IEnumerable<IExceptionViewModel> Exceptions
        {
            get
            {
                return _exceptions;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Håndterer en exception.
        /// </summary>
        /// <param name="exception">Exception, der skal håndteres.</param>
        public virtual void HandleException(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            if (_synchronizationContext == null)
            {
                _exceptions.Add(new ExceptionViewModel(exception));
                return;
            }
            _synchronizationContext.Post(ex => _exceptions.Add(new ExceptionViewModel(ex as Exception)), exception);
        }

        /// <summary>
        /// Eventhandler, der rejses, når listen af håndterede collections ændres.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void ExceptionCollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    RaisePropertyChanged("Exceptions");
                    RaisePropertyChanged("Last");
                    ShowLast = true;
                    break;
            }
        }

        #endregion
    }
}
