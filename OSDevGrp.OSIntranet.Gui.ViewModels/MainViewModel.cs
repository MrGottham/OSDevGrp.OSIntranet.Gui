using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;
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
        private readonly List<object> _eventSubscribers = new List<object>();

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
                if (_exceptionHandlerViewModel == null)
                {
                    _exceptionHandlerViewModel = new ExceptionHandlerViewModel();
                    _exceptionHandlerViewModel.OnHandleException += HandleExceptionEventHandler;
                }
                return _exceptionHandlerViewModel;
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

        /// <summary>
        /// Tilmelder en subscriber, der kan vente på, at events publiseres.
        /// </summary>
        /// <typeparam name="TEventArgs">Typen af argumenter til eventet, der skal subscribes på.</typeparam>
        /// <param name="eventSubscriber">Subscriber, der skal tilmeldes.</param>
        public virtual void Subscribe<TEventArgs>(IEventSubscriber<TEventArgs> eventSubscriber) where TEventArgs : IIntranetGuiEventArgs
        {
            if (eventSubscriber == null)
            {
                throw new ArgumentNullException("eventSubscriber");
            }
            _eventSubscribers.Add(eventSubscriber);
        }

        /// <summary>
        /// Framelder en subscriber, der venter på, at events publiseres.
        /// </summary>
        /// <typeparam name="TEventArgs">Typen af argumenter til eventet, der skal subscribes på.</typeparam>
        /// <param name="eventSubscriber">Subscriber, der skal frameldes.</param>
        public virtual void Unsubscribe<TEventArgs>(IEventSubscriber<TEventArgs> eventSubscriber) where TEventArgs : IIntranetGuiEventArgs
        {
            if (eventSubscriber == null)
            {
                throw new ArgumentNullException("eventSubscriber");
            }
            while (_eventSubscribers.Contains(eventSubscriber))
            {
                _eventSubscribers.Remove(eventSubscriber);
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når en exception skal håndteres.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void HandleExceptionEventHandler(object sender, IHandleExceptionEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }
            PublishEvent(eventArgs);
        }

        /// <summary>
        /// Publiserer events til subscribers.
        /// </summary>
        /// <typeparam name="TEventArgs">Type på argumenter til eventet, der skal publiseres.</typeparam>
        /// <param name="eventArgs">Argumenter til eventet, der skal publiseres.</param>
        private void PublishEvent<TEventArgs>(TEventArgs eventArgs) where TEventArgs : IIntranetGuiEventArgs
        {
            if (Equals(eventArgs, null))
            {
                throw new ArgumentNullException("eventArgs");
            }
            var subscriberType = typeof (IEventSubscriber<>);
            var genericSubscriberType = subscriberType.MakeGenericType(new[] {typeof (TEventArgs)});
            var eventSubscribers = _eventSubscribers.Where(m => m.GetType().GetTypeInfo().ImplementedInterfaces.Contains(genericSubscriberType)).ToArray();
            foreach (var eventSubscriber in eventSubscribers)
            {
                var eventSubscriberInterface = eventSubscriber.GetType().GetTypeInfo().ImplementedInterfaces.SingleOrDefault(genericSubscriberType.Equals);
                if (eventSubscriberInterface == null)
                {
                    continue;
                }
                var method = eventSubscriberInterface.GetRuntimeMethod("OnEvent", new[] { typeof(TEventArgs) });
                if (method == null)
                {
                    continue;
                }
                try
                {
                    method.Invoke(eventSubscriber, new object[] {eventArgs});
                    var handleExceptionEventArgs = eventArgs as IHandleExceptionEventArgs;
                    if (handleExceptionEventArgs != null && handleExceptionEventArgs.IsHandled)
                    {
                        return;
                    }
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException ?? ex;
                }
            }
        }

        #endregion
    }
}
