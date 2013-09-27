using OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels
{
    /// <summary>
    /// ViewModel til binding mod Views.
    /// </summary>
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        #region Private variables

        private IRegnskabslisteViewModel _regnskabslisteViewModel;

        #endregion
        
        #region Properties

        /// <summary>
        /// Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return GetType().Name;
            }
        }

        /// <summary>
        /// ViewModel for en liste af regnskaber.
        /// </summary>
        public virtual IRegnskabslisteViewModel Regnskabsliste
        {
            get
            {
                return _regnskabslisteViewModel ?? (_regnskabslisteViewModel = new RegnskabslisteViewModel(new FinansstyringRepository()));
            }
        }

        #endregion
    }
}
