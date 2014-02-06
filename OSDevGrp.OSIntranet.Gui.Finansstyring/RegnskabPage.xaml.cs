using System;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring
{
    /// <summary>
    /// Page til et regnskab.
    /// </summary>
    public sealed partial class RegnskabPage
    {
        #region Private variables

        private static readonly DependencyProperty ExceptionHandlerProperty = DependencyProperty.Register("ExceptionHandler", typeof (IExceptionHandlerViewModel), typeof (RegnskabPage), new PropertyMetadata(null));
        private static readonly DependencyProperty RegnskabProperty = DependencyProperty.Register("Regnskab", typeof (IRegnskabViewModel), typeof (RegnskabPage), new PropertyMetadata(null));

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en Page til et regnskab.
        /// </summary>
        public RegnskabPage()
        {
            InitializeComponent();

            SizeChanged += PageSizeChangedEventHandler;
        }

        #endregion

        #region Properties

        /// <summary>
        /// ViewModel for exceptionhandleren.
        /// </summary>
        public IExceptionHandlerViewModel ExceptionHandler
        {
            get
            {
                return GetValue(ExceptionHandlerProperty) as IExceptionHandlerViewModel;
            }
            set
            {
                SetValue(ExceptionHandlerProperty, value);
            }
        }

        /// <summary>
        /// ViewModel for regnskabet.
        /// </summary>
        public IRegnskabViewModel Regnskab
        {
            get
            {
                return GetValue(RegnskabProperty) as IRegnskabViewModel;
            }
            set
            {
                SetValue(RegnskabProperty, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
            
            var tuple = e.Parameter as Tuple<IRegnskabViewModel, IExceptionHandlerViewModel>;
            if (tuple == null)
            {
                return;
            }
            Regnskab = tuple.Item1;
            ExceptionHandler = tuple.Item2;

            DefaultNavigation.DataContext = Regnskab;
            MinimalNavigation.DataContext = Regnskab;
            ExceptionHandlerAppBar.DataContext = ExceptionHandler;
        }

        /// <summary>
        /// Eventhandler, der håndterer åbning af applikationens TopAppBar.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void TopAppBarOpenedEventHandler(object sender, object eventArgs)
        {
            if (TopAppBar == null)
            {
                return;
            }
            TopAppBar.IsOpen = ExceptionHandler.ShowLast;
        }

        /// <summary>
        /// Eventhandler, der håndterer ændring af størrelse på siden.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void PageSizeChangedEventHandler(object sender, SizeChangedEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }
            if (eventArgs.NewSize.Width < 500)
            {
                VisualStateManager.GoToState(this, "MinimalLayout", true);
                return;
            }
            VisualStateManager.GoToState(this, "DefaultLayout", true);
        }

        #endregion
    }
}
