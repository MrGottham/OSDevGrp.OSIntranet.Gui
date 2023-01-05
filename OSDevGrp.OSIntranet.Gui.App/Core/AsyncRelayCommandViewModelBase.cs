using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.Gui.App.Core
{
    public abstract class AsyncRelayCommandViewModelBase : ViewModelBase
    {
        #region Private variables

        private Exception _commandError;

        #endregion

        #region Properties

        public Exception CommandError
        {
            get => _commandError;
            protected set
            {
                NullGuard.NotNull(value, nameof(value));

                if (_commandError == value)
                {
                    return;
                }

                _commandError = value;

                RaisePropertyChanged();
            }
        }

        #endregion

        #region Methods

        protected Task HandleCommandErrorAsync(Exception exception)
        {
            NullGuard.NotNull(exception, nameof(exception));

            CommandError = exception;

            return Task.CompletedTask;
        }

        #endregion
    }
}