using Windows.UI.Xaml;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring.Core.Behaviors
{
    /// <summary>
    /// Dependency object, der kan markere valideringsfejl.
    /// </summary>
    public static class HighlightOnErrorBehavior
    {
        #region Public variables

        public static DependencyProperty PropertyErrorProperty = DependencyProperty.RegisterAttached("PropertyError", typeof (string), typeof (HighlightOnErrorBehavior), new PropertyMetadata(null, OnPropertyErrorChanged));

        #endregion

        #region Methods

        /// <summary>
        /// Eventhandler, der rejses, når værdien for PropertyError ændres på et dependency objekt.
        /// </summary>
        /// <param name="dependencyObject">Dependency objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private static void OnPropertyErrorChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (eventArgs == null || eventArgs.NewValue == null)
            {
                return;
            }
        }

        #endregion
    }
}
