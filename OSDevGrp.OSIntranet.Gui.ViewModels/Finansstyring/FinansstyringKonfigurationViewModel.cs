using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring
{
    /// <summary>
    /// ViewModel indeholdende konfiguration til finansstyring.
    /// </summary>
    public class FinansstyringKonfigurationViewModel : ViewModelBase, IFinansstyringKonfigurationViewModel
    {
        #region Private variables



        #endregion

        #region Constructor

        /// <summary>
        /// Danner en ViewModel indeholdende konfiguration til finansstyring.
        /// </summary>
        /// <param name="finansstyringKonfigurationRepository">Implementering af konfigurationsrepository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewMdoel til en exceptionhandler.</param>
        public FinansstyringKonfigurationViewModel(IFinansstyringKonfigurationRepository finansstyringKonfigurationRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
        {
            if (finansstyringKonfigurationRepository == null)
            {
                throw new ArgumentNullException("finansstyringKonfigurationRepository");
            }
            if (exceptionHandlerViewModel == null)
            {
                throw new ArgumentNullException("exceptionHandlerViewModel");
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Unikt navn for konfigurationen.
        /// </summary>
        public virtual string Configuration
        {
            get
            {
                return "OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.FinansstyringKonfigurationViewModel";
            }
        }

        /// <summary>
        /// Collection indeholdende navne for de enkelte konfigurationsværdier.
        /// </summary>
        public virtual IEnumerable<string> Keys
        {
            get
            {
                return FinansstyringKonfigurationRepository.Keys;
            }
        }

        /// <summary>
        /// Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return Resource.GetText(Text.Configuration);
            }
        }

        #endregion
    }
}
