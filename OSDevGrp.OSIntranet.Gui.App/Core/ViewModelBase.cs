using OSDevGrp.OSIntranet.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OSDevGrp.OSIntranet.Gui.App.Core
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            NullGuard.NotNullOrWhiteSpace(propertyName, nameof(propertyName));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}