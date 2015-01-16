using Windows.UI.Xaml;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring
{
    /// <summary>
    /// User control for Privacy Policy.
    /// </summary>
    public sealed partial class PrivacyPolicyUserControl
    {
        #region Private variables

        private static readonly DependencyProperty MainViewModelProperty = DependencyProperty.Register("MainViewModel", typeof(IMainViewModel), typeof(PrivacyPolicyUserControl), new PropertyMetadata(null));

        #endregion

        #region Constructor

        /// <summary>
        /// Creates the user control for Privacy Policy.
        /// </summary>
        public PrivacyPolicyUserControl()
        {
            InitializeComponent();

            MainViewModel = (IMainViewModel) Application.Current.Resources["MainViewModel"];

            DataContext = MainViewModel;
        }

        #endregion

        #region Properties

        /// <summary>
        /// MainViewModel, der skal benyttes til konfiguration.
        /// </summary>
        public IMainViewModel MainViewModel
        {
            get
            {
                return GetValue(MainViewModelProperty) as IMainViewModel;
            }
            private set
            {
                SetValue(MainViewModelProperty, value);
            }
        }

        #endregion
    }
}
