using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring.Core.Behaviors
{
    /// <summary>
    /// Dependency object, der kan markere valideringsfejl.
    /// </summary>
    public static class HighlightOnErrorBehavior
    {
        #region Public variables

        public readonly static DependencyProperty PropertyErrorProperty = DependencyProperty.RegisterAttached("PropertyError", typeof (string), typeof (HighlightOnErrorBehavior), new PropertyMetadata(string.Empty, OnPropertyErrorChanged));

        #endregion

        #region Methods

        /// <summary>
        /// Returnerer værdien for PropertyError på et dependency objekt.
        /// </summary>
        /// <param name="dependencyObject">Dependency objekt, hvorfra PropertyError skal returneres.</param>
        /// <returns>Værdi for PropertyError.</returns>
        public static string GetPropertyError(DependencyObject dependencyObject)
        {
            if (dependencyObject == null)
            {
                return string.Empty;
            }
            return (string) dependencyObject.GetValue(PropertyErrorProperty);
        }

        /// <summary>
        /// Opdaterer værdien for PropertyError på et dependency objekt.
        /// </summary>
        /// <param name="dependencyObject">Dependency objekt, hvor PropertyError skal opdateres.</param>
        /// <param name="value">Værdi, hvormed PropertyError skal opdateres med.</param>
        public static void SetPropertyError(DependencyObject dependencyObject, string value)
        {
            if (dependencyObject == null)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(value))
            {
                dependencyObject.SetValue(PropertyErrorProperty, string.Empty);
                return;
            }
            dependencyObject.SetValue(PropertyErrorProperty, value);
        }

        /// <summary>
        /// Eventhandler, der rejses, når værdien for PropertyError ændres på et dependency objekt.
        /// </summary>
        /// <param name="dependencyObject">Dependency objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private static void OnPropertyErrorChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (eventArgs == null)
            {
                return;
            }
            if (eventArgs.NewValue == eventArgs.OldValue)
            {
                return;
            }
            var textBox = dependencyObject as TextBox;
            if (textBox == null)
            {
                return;
            }
            var errorMessage = eventArgs.NewValue as string;
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                dependencyObject.ClearValue(FrameworkElement.StyleProperty);
                return;
            }
            dependencyObject.SetValue(FrameworkElement.StyleProperty, Application.Current.Resources["RedBorderedTextBoxStyle"]);

            /*

            var textBoxStyle = string.IsNullOrWhiteSpace(errorMessage) ? null : (Style) Application.Current.Resources["RedBorderedTextBoxStyle"];
            if (textBoxStyle == textBox.Style)
            {
                return;
            }
            textBox.Style = textBoxStyle;
            textBox.UpdateLayout();
            */
        }

        #endregion
    }
}
