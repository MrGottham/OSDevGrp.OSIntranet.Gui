using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
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
        private ICommand _hideCommand;
        private readonly int _mainThreadId;
        private readonly SynchronizationContext _synchronizationContext;
        private readonly ObservableCollection<IExceptionViewModel> _exceptions = new ObservableCollection<IExceptionViewModel>();

        #endregion

        #region Constructor

        /// <summary>
        /// Danner ViewModel for en exceptionhandler.
        /// </summary>
        public ExceptionHandlerViewModel()
        {
            _exceptions.CollectionChanged += ExceptionCollectionChangedEventHandler;
            _mainThreadId = Environment.CurrentManagedThreadId;
            _synchronizationContext = SynchronizationContext.Current;
        }

        #endregion

        #region Events

        /// <summary>
        /// Event, der rejses, når en exception håndteres.
        /// </summary>
        public virtual event IntranetGuiEventHandler<IHandleExceptionEventArgs> OnHandleException;

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
                return Last != null && _showLast;
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

        /// <summary>
        /// Tekstangivelse til kommandoen, der kan skjule seneste håndterede exception.
        /// </summary>
        public virtual string HideCommandText
        {
            get
            {
                return Resource.GetText(Text.Hide);
            }
        }
       
        /// <summary>
        /// Kommando, der kan skjule seneste håndterede exception.
        /// </summary>
        public virtual ICommand HideCommand
        {
            get
            {
                return _hideCommand ?? (_hideCommand = new RelayCommand(obj => ShowLast = false));
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
            if (_synchronizationContext == null || _mainThreadId == Environment.CurrentManagedThreadId)
            {
                PulishException(exception);
                return;
            }
            using (var waitEvent = new AutoResetEvent(false))
            {
                var we = waitEvent;
                _synchronizationContext.Post(obj =>
                    {
                        try
                        {
                            PulishException(exception);
                        }
                        finally
                        {
                            we.Set();
                        }
                    }, exception);
                waitEvent.WaitOne();
            }
        }

        /// <summary>
        /// Publiserer og håndterer en exception.
        /// </summary>
        /// <param name="exception">Exception, der skal håndteres.</param>
        private void PulishException(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            if (OnHandleException == null)
            {
                _exceptions.Add(new ExceptionViewModel(exception));
                return;
            }
            var eventArgs = new HandleExceptionEventArgs(exception)
                {
                    IsHandled = false
                };
            OnHandleException.Invoke(this, eventArgs);
            if (!eventArgs.IsHandled)
            {
                _exceptions.Add(new ExceptionViewModel(exception));
            }
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
