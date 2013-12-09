using System;
using System.Collections.Generic;
using System.ComponentModel;
using OSDevGrp.OSIntranet.Gui.Finansstyring.Core;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Runtime;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring
{
    /// <summary>
    /// User control til konfiguration.
    /// </summary>
    public sealed partial class ConfigurationUserControl : IDisposable, IEventSubscriber<IHandleExceptionEventArgs>
    {
        #region Private variables

        private bool _disposed;
        private readonly ConfigurationProvider _configurationProvider;
        private readonly IMainViewModel _mainViewModel;
        private readonly static DependencyProperty ValidationErrorsProperty = DependencyProperty.RegisterAttached("ValidationErrors", typeof (IDictionary<string, string>), typeof (ConfigurationUserControl), null);

        #endregion

        #region Constructor

        /// <summary>
        /// Danner user control til konfiguration.
        /// </summary>
        public ConfigurationUserControl()
        {
            InitializeComponent();

            ValidationErrors = new Dictionary<string, string>();

            _configurationProvider = ConfigurationProvider.Instance;

            _mainViewModel = (IMainViewModel) Resources["MainViewModel"];
            _mainViewModel.ApplyConfiguration(_configurationProvider.Settings);
            _mainViewModel.Subscribe(this);

            var finansstyringKonfiguration = (IFinansstyringKonfigurationViewModel) ((Grid) Content).DataContext;
            finansstyringKonfiguration.PropertyChanged += FinansstyringKonfigurationPropertyChangedEventHandler;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Dictionary indeholdende valideringsfejl.
        /// </summary>
        public IDictionary<string, string> ValidationErrors
        {
            get
            {
                return (IDictionary<string, string>) GetValue(ValidationErrorsProperty);
            }
            private set
            {
                SetValue(ValidationErrorsProperty, value);
            }
        }

        #endregion

        #region IDisposable members

        /// <summary>
        /// Destructor.
        /// </summary>
        ~ConfigurationUserControl()
        {
            Dispose(false);
        }

        /// <summary>
        /// Frigørelse af ressourcer.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Frigørelse af ressourcer.
        /// </summary>
        /// <param name="disposing">Angivelse af, om managed ressourcer også skal frigøres.</param>
        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            _mainViewModel.Unsubscribe(this);
            if (disposing)
            {
            }
            _disposed = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Eventhandler, der rejses, når eventet til håndtering af exceptions publiceres.
        /// </summary>
        /// <param name="handleExceptionEventArgs">Argumenter fra eventet, der publiceres.</param>
        public void OnEvent(IHandleExceptionEventArgs handleExceptionEventArgs)
        {
            if (handleExceptionEventArgs == null)
            {
                throw new ArgumentNullException("handleExceptionEventArgs");
            }
            var validationException = handleExceptionEventArgs.Error as IntranetGuiValidationException;
            if (validationException == null)
            {
                return;
            }
            var validationContext = validationException.ValidationContext as IFinansstyringKonfigurationViewModel;
            if (validationContext == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(validationException.PropertyName) || string.IsNullOrEmpty(validationException.Message))
            {
                return;
            }
            try
            {
                if (ValidationErrors.ContainsKey(validationException.PropertyName))
                {
                    ValidationErrors[validationException.PropertyName] = validationException.Message;
                    return;
                }
                ValidationErrors.Add(validationException.PropertyName, validationException.Message);
                ValidationErrors = new Dictionary<string, string>();
            }
            finally
            {
                handleExceptionEventArgs.IsHandled = true;
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når properties ændres på ViewModel for konfiguration til finansstyring.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void FinansstyringKonfigurationPropertyChangedEventHandler(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("sender");
            }
            var finansstyringKonfiguration = sender as IFinansstyringKonfigurationViewModel;
            if (finansstyringKonfiguration == null)
            {
                return;
            }
            if (!ViewModelValidator.Validate(finansstyringKonfiguration, eventArgs.PropertyName))
            {
                return;
            }
            switch (eventArgs.PropertyName)
            {
                case "FinansstyringServiceUri": 
                    _configurationProvider.SetValue("FinansstyringServiceUri", finansstyringKonfiguration.FinansstyringServiceUri);
                    break;
            }
        }

        #endregion

    }
}
