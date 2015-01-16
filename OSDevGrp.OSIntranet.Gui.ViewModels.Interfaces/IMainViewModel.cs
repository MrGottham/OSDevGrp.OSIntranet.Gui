using System.Collections.Generic;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces
{
    /// <summary>
    /// Interface for ViewModel til binding mod Views.
    /// </summary>
    public interface IMainViewModel : IViewModel
    {
        /// <summary>
        /// ViewModel for en liste af regnskaber.
        /// </summary>
        IRegnskabslisteViewModel Regnskabsliste
        {
            get;
        }

        /// <summary>
        /// ViewModel indeholdende konfiguration til finansstyring.
        /// </summary>
        IFinansstyringKonfigurationViewModel FinansstyringKonfiguration
        {
            get;
        }

        /// <summary>
        /// ViewModel for en exceptionhandler.
        /// </summary>
        IExceptionHandlerViewModel ExceptionHandler
        {
            get;
        }

        /// <summary>
        /// ViewModel for Privacy Policy.
        /// </summary>
        IPrivacyPolicyViewModel PrivacyPolicy
        {
            get;
        }

        /// <summary>
        /// Tilføjer konfiguration.
        /// </summary>
        /// <param name="configurationSettings">Dictionary indeholdende konfiguration.</param>
        void ApplyConfiguration(IDictionary<string, object> configurationSettings);

        /// <summary>
        /// Nulstiller denne ViewModel og skifter til det lokale datalager.
        /// </summary>
        /// <param name="finansstyringRepositoryLocale">Implementering af et lokale datalager til finansstyring.</param>
        void SwitchToLocaleDataStorage(IFinansstyringRepository finansstyringRepositoryLocale);

        /// <summary>
        /// Tilmelder en subscriber, der kan vente på, at events publiseres.
        /// </summary>
        /// <typeparam name="TEventArgs">Typen af argumenter til eventet, der skal subscribes på.</typeparam>
        /// <param name="eventSubscriber">Subscriber, der skal tilmeldes.</param>
        void Subscribe<TEventArgs>(IEventSubscriber<TEventArgs> eventSubscriber) where TEventArgs : IIntranetGuiEventArgs;

        /// <summary>
        /// Framelder en subscriber, der venter på, at events publiseres.
        /// </summary>
        /// <typeparam name="TEventArgs">Typen af argumenter til eventet, der skal subscribes på.</typeparam>
        /// <param name="eventSubscriber">Subscriber, der skal frameldes.</param>
        void Unsubscribe<TEventArgs>(IEventSubscriber<TEventArgs> eventSubscriber) where TEventArgs : IIntranetGuiEventArgs;
    }
}
