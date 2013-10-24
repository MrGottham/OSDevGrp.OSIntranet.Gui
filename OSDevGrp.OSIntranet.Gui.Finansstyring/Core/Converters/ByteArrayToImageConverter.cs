using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring.Core.Converters
{
    /// <summary>
    /// Converter, der kan konvertere et array af bytes til et billede.
    /// </summary>
    public class ByteArrayToImageConverter : IValueConverter
    {
        #region Methods

        /// <summary>
        /// Konverterer et array af bytes til et billede.
        /// </summary>
        /// <param name="value">Værdi, der skal konverteres.</param>
        /// <param name="targetType">Typen, der skal konverteres til.</param>
        /// <param name="parameter">Parameter til konverteringen.</param>
        /// <param name="language">Sprog.</param>
        /// <returns>Konverteret værdi.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return null;
            }
            var task = ToImageAsync(value as byte[]);
            task.ToString();
            return null;
        }

        /// <summary>
        /// Konverterer et billede til et array af bytes.
        /// </summary>
        /// <param name="value">Værdi, der skal konverteres.</param>
        /// <param name="targetType">Typen, der skal konverteres til.</param>
        /// <param name="parameter">Parameter til konverteringen.</param>
        /// <param name="language">Sprog.</param>
        /// <returns>Konverteret værdi.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Konverteres et array af bytes til et billede.
        /// </summary>
        /// <param name="bytes">Array af bytes, der skal konverteres til et billede.</param>
        /// <returns>Billede.</returns>
        private static async Task<BitmapImage> ToImageAsync(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            using (var memoryStream = new InMemoryRandomAccessStream())
            {
                await memoryStream.WriteAsync(bytes.AsBuffer());
                memoryStream.Seek(0);
                var image = new BitmapImage();
                await image.SetSourceAsync(memoryStream);
                return image;
            }
        }

        #endregion
    }
}
