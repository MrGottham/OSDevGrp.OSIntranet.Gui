using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
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

        private readonly IFinansstyringKonfigurationRepository _finansstyringKonfigurationRepository;
        private readonly IExceptionHandlerViewModel _exceptionHandlerViewModel;

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
            _finansstyringKonfigurationRepository = finansstyringKonfigurationRepository;
            _exceptionHandlerViewModel = exceptionHandlerViewModel;
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

        /// <summary>
        /// Label til uri for servicen, der supporterer finansstyring.
        /// </summary>
        public virtual string FinansstyringServiceUriLabel
        {
            get
            {
                return Resource.GetText(Text.SupportingServiceUri);
            }
        }

        /// <summary>
        /// Uri til servicen, der supporterer finansstyring.
        /// </summary>
        public virtual string FinansstyringServiceUri
        {
            get
            {
                try
                {
                    return _finansstyringKonfigurationRepository.FinansstyringServiceUri.ToString();
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                    return string.Empty;
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileGettingPropertyValue, "FinansstyringServiceUri", ex.Message), ex));
                    return string.Empty;
                }
            }
            set
            {
                try
                {
                    var uri = new Uri(value);
                    if (_finansstyringKonfigurationRepository.FinansstyringServiceUri == uri)
                    {
                        return;
                    }
                    _finansstyringKonfigurationRepository.KonfigurationerAdd(new Dictionary<string, object> {{"FinansstyringServiceUri", uri}});
                    RaisePropertyChanged("FinansstyringServiceUri");
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "FinansstyringServiceUri", ex.Message), ex));
                }
            }
        }

        /// <summary>
        /// Label til antal bogføringslinjer, der skal hentes.
        /// </summary>
        public virtual string AntalBogføringslinjerLabel
        {
            get
            {
                return Resource.GetText(Text.NumberOfAccountingLinesToGet);
            }
        }

        /// <summary>
        /// Antal bogføringslinjer, der skal hentes.
        /// </summary>
        public virtual int AntalBogføringslinjer
        {
            get
            {
                try
                {
                    return _finansstyringKonfigurationRepository.AntalBogføringslinjer;
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                    return 0;
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileGettingPropertyValue, "AntalBogføringslinjer", ex.Message), ex));
                    return 0;
                }
            }
            set
            {
                try
                {
                    if (_finansstyringKonfigurationRepository.AntalBogføringslinjer == value)
                    {
                        return;
                    }
                    _finansstyringKonfigurationRepository.KonfigurationerAdd(new Dictionary<string, object> {{"AntalBogføringslinjer", value}});
                    RaisePropertyChanged("AntalBogføringslinjer");
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "AntalBogføringslinjer", ex.Message), ex));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string DageForNyhederLabel
        {
            get
            {
                return Resource.GetText(Text.DaysForNews);
            }
        }

        /// <summary>
        /// Antal dage, som nyheder er gældende.
        /// </summary>
        public virtual int DageForNyheder
        {
            get
            {
                try
                {
                    return _finansstyringKonfigurationRepository.DageForNyheder;
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                    return 0;
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileGettingPropertyValue, "DageForNyheder", ex.Message), ex));
                    return 0;
                }
            }
            set
            {
                try
                {
                    if (_finansstyringKonfigurationRepository.DageForNyheder == value)
                    {
                        return;
                    }
                    _finansstyringKonfigurationRepository.KonfigurationerAdd(new Dictionary<string, object> {{"DageForNyheder", value}});
                    RaisePropertyChanged("DageForNyheder");
                }
                catch (IntranetGuiExceptionBase ex)
                {
                    _exceptionHandlerViewModel.HandleException(ex);
                }
                catch (Exception ex)
                {
                    _exceptionHandlerViewModel.HandleException(new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorWhileSettingPropertyValue, "DageForNyheder", ex.Message), ex));
                }
            }
        }

        #endregion
    }
}
