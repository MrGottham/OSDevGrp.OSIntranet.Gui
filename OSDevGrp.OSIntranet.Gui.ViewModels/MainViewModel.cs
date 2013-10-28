using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
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
        private IExceptionHandlerViewModel _exceptionHandlerViewModel;
        private readonly List<IConfigurationViewModel> _configurationViewModels = new List<IConfigurationViewModel>();

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
                return _regnskabslisteViewModel ?? (_regnskabslisteViewModel = new RegnskabslisteViewModel(new FinansstyringRepository(FinansstyringKonfigurationRepository), ExceptionHandler));
            }
        }

        /// <summary>
        /// ViewModel indeholdende konfiguration til finansstyring.
        /// </summary>
        public virtual IFinansstyringKonfigurationViewModel FinansstyringKonfiguration
        {
            get
            {
                var finansstyringKonfigurationViewModel = _configurationViewModels.OfType<IFinansstyringKonfigurationViewModel>().FirstOrDefault();
                if (finansstyringKonfigurationViewModel == null)
                {
                    finansstyringKonfigurationViewModel = new FinansstyringKonfigurationViewModel(FinansstyringKonfigurationRepository, ExceptionHandler);
                    _configurationViewModels.Add(finansstyringKonfigurationViewModel);
                }
                return finansstyringKonfigurationViewModel;
            }
        }

        /// <summary>
        /// ViewModel for en exceptionhandler.
        /// </summary>
        public virtual IExceptionHandlerViewModel ExceptionHandler
        {
            get
            {
                return _exceptionHandlerViewModel ?? (_exceptionHandlerViewModel = new ExceptionHandlerViewModel());
            }
        }

        /// <summary>
        /// Returnerer instans af konfigurationsrepositoryet, der supporterer finansstyring.
        /// </summary>
        private static IFinansstyringKonfigurationRepository FinansstyringKonfigurationRepository
        {
            get
            {
                return Repositories.Finansstyring.FinansstyringKonfigurationRepository.Instance;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Tilføjer konfiguration.
        /// </summary>
        /// <param name="configurationSettings">Dictionary indeholdende konfiguration.</param>
        public virtual void ApplyConfiguration(IDictionary<string, object> configurationSettings)
        {
            if (configurationSettings == null)
            {
                throw new ArgumentNullException("configurationSettings");
            }
            var finansstyringConfiguration = configurationSettings.Where(configuration => Repositories.Finansstyring.FinansstyringKonfigurationRepository.Keys.Contains(configuration.Key))
                                                                  .ToDictionary(m => m.Key, m => m.Value);
            FinansstyringKonfigurationRepository.KonfigurationerAdd(finansstyringConfiguration);
        }

        #endregion
    }
}
