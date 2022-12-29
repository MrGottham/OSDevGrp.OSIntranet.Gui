namespace OSDevGrp.OSIntranet.Gui.App.Core
{
    public class AppShellViewModel : ViewModelBase
    {
        #region Private variables

        private bool _systemIsOffline;

        #endregion

        #region Properties

        public bool SystemIsOffline
        {
            get => _systemIsOffline;
            set
            {
                if (_systemIsOffline == value)
                {
                    return;
                }

                _systemIsOffline = value;

                RaisePropertyChanged();
            }
        }

        #endregion
    }
}