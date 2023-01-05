using OSDevGrp.OSIntranet.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace OSDevGrp.OSIntranet.Gui.App.Core
{
    internal class AsyncRelayCommand : AsyncRelayCommand<object>
    {
        #region Constructors

        internal AsyncRelayCommand(Func<Task> executionTaskGetter, Func<Exception, Task> asyncExceptionHandler)
            : base(argument => executionTaskGetter(), asyncExceptionHandler)
        {
        }

        internal AsyncRelayCommand(Func<CancellationToken, Task> executionTaskGetter, Func<Exception, Task> asyncExceptionHandler)
            : base((argument, cancellationToken) => executionTaskGetter(cancellationToken), asyncExceptionHandler)
        {
        }

        internal AsyncRelayCommand(Func<Task> executionTaskGetter, Func<bool> canExecute, Func<Exception, Task> asyncExceptionHandler)
            : base(argument => executionTaskGetter(), argument => canExecute(), asyncExceptionHandler)
        {
        }

        internal AsyncRelayCommand(Func<CancellationToken, Task> executionTaskGetter, Func<bool> canExecute, Func<Exception, Task> asyncExceptionHandler)
            : base((argument, cancellationToken) => executionTaskGetter(cancellationToken), argument => canExecute(), asyncExceptionHandler)
        {
        }

        #endregion
    }

    internal class AsyncRelayCommand<TArgument> : IAsyncRelayCommand where TArgument : class
    {
        #region Private variables

        private bool _isRunning;
        private Task _executionTask;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly bool _canBeCancelled;
        private readonly Func<TArgument, CancellationToken, Task> _executionTaskGetter;
        private readonly Predicate<TArgument> _canExecute;
        private readonly Func<Exception, Task> _asyncExceptionHandler;
        private readonly ICommand _command;

        #endregion

        #region Constructors

        internal AsyncRelayCommand(Func<TArgument, Task> executionTaskGetter, Func<Exception, Task> asyncExceptionHandler)
            : this((argument, cancellationToken) => executionTaskGetter(argument), argument => true, asyncExceptionHandler, false)
        {
        }

        internal AsyncRelayCommand(Func<TArgument, CancellationToken, Task> executionTaskGetter, Func<Exception, Task> asyncExceptionHandler)
            : this(executionTaskGetter, argument => true, asyncExceptionHandler, true)
        {
        }

        internal AsyncRelayCommand(Func<TArgument, Task> executionTaskGetter, Predicate<TArgument> canExecute, Func<Exception, Task> asyncExceptionHandler)
            : this((argument, cancellationToken) => executionTaskGetter(argument), canExecute, asyncExceptionHandler, false)
        {
        }

        internal AsyncRelayCommand(Func<TArgument, CancellationToken, Task> executionTaskGetter, Predicate<TArgument> canExecute, Func<Exception, Task> asyncExceptionHandler)
            : this(executionTaskGetter, canExecute, asyncExceptionHandler, true)
        {
        }

        private AsyncRelayCommand(Func<TArgument, CancellationToken, Task> executionTaskGetter, Predicate<TArgument> canExecute, Func<Exception, Task> asyncExceptionHandler, bool canBeCancelled)
        {
            NullGuard.NotNull(executionTaskGetter, nameof(executionTaskGetter))
                .NotNull(canExecute, nameof(canExecute))
                .NotNull(asyncExceptionHandler, nameof(asyncExceptionHandler));

            _executionTaskGetter = executionTaskGetter;
            _canExecute = canExecute;
            _asyncExceptionHandler = asyncExceptionHandler;

            _command = new Command(LocalExecute, LocalCanExecute);
            _command.CanExecuteChanged += LocalCanExecuteChanged;

            CanBeCancelled = canBeCancelled;
            IsRunning = false;
            ExecutionTask = null;
            CancellationTokenSource = null;
        }

        #endregion

        #region Properties

        public bool CanBeCancelled
        {
            get => _canBeCancelled;
            private init
            {
                if (_canBeCancelled == value)
                {
                    return;
                }

                _canBeCancelled = value;

                RaisePropertyChanged();
            }
        }

        public bool IsRunning
        {
            get => _isRunning;
            private set
            {
                if (_isRunning == value)
                {
                    return;
                }

                _isRunning = value;

                RaisePropertyChanged();
                RaiseCanExecuteChanged();
            }
        }

        public Task ExecutionTask
        {
            get => _executionTask;
            private set
            {
                if (_executionTask == value)
                {
                    return;
                }

                _executionTask = value;

                RaisePropertyChanged();
            }
        }

        public bool IsCancellationRequested => _cancellationTokenSource?.IsCancellationRequested ?? false;

        private CancellationTokenSource CancellationTokenSource
        {
            get => _cancellationTokenSource;
            set
            {
                if (_cancellationTokenSource == value)
                {
                    return;
                }

                _cancellationTokenSource = value;

                RaisePropertyChanged(nameof(IsCancellationRequested));
            }
        }

        #endregion;

        #region Events

        public event EventHandler CanExecuteChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        public void Dispose()
        {
            if (CancellationTokenSource == null)
            {
                return;
            }

            CancellationTokenSource.Dispose();
            CancellationTokenSource = null;
        }

        public bool CanExecute(object argument)
        {
            return _command.CanExecute(argument);
        }

        public void Execute(object argument)
        {
            _command.Execute(argument);
        }

        public void Cancel()
        {
            if (CanBeCancelled == false || IsRunning == false || CancellationTokenSource == null)
            {
                return;
            }

            CancellationTokenSource.Cancel();

            RaisePropertyChanged(nameof(IsCancellationRequested));
        }

        private async Task Execute(TArgument argument, CancellationToken cancellationToken)
        {
            try
            {
                ExecutionTask = _executionTaskGetter(argument, cancellationToken);
                await ExecutionTask;
            }
            catch (Exception ex)
            {
                await _asyncExceptionHandler(ex);
            }
            finally
            {
                ExecutionTask = null;
            }
        }

        private bool LocalCanExecute(object argument)
        {
            return _canExecute(argument as TArgument) && IsRunning == false;
        }

        private async void LocalExecute(object argument)
        {
            if (IsRunning)
            {
                return;
            }

            IsRunning = true;
            try
            {
                CancellationTokenSource = new CancellationTokenSource();

                await Execute(argument as TArgument, CancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                await _asyncExceptionHandler(ex);
            }
            finally
            {
                if (CancellationTokenSource != null)
                {
                    CancellationTokenSource.Dispose();
                    CancellationTokenSource = null;
                }
                IsRunning = false;
            }
        }

        private void LocalCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            RaiseCanExecuteChanged(eventArgs);
        }

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            NullGuard.NotNullOrWhiteSpace(propertyName, nameof(propertyName));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RaiseCanExecuteChanged(EventArgs e = null)
        {
            CanExecuteChanged?.Invoke(this, e ?? EventArgs.Empty);
        }

        #endregion
    }
}