namespace OSDevGrp.OSIntranet.Gui.App.Features
{
    internal abstract class BackgroundFeatureBase : IBackgroundFeature
    {
        #region Private variables

        private bool _disposed;

        #endregion

        #region Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual Task StartAsync() => Task.CompletedTask;

        public virtual Task StopAsync() => Task.CompletedTask;

        protected void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                OnDispose();
            }

            _disposed = true;
        }

        protected virtual void OnDispose()
        {
        }

        #endregion
    }
}