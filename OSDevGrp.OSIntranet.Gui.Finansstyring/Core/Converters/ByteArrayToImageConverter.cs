using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring.Core.Converters
{
    /// <summary>
    /// Converter, der kan konvertere et array af bytes til et billede.
    /// </summary>
    public class ByteArrayToImageConverter : IValueConverter
    {
        #region Private variables

        private readonly SynchronizationContext _synchronizationContext = SynchronizationContext.Current;

        #endregion

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
            var result = new BitmapImage();
            Task.Run(() => LoadImageAsync(value as byte[], result));
            return result;
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
        /// Loader et array af bytes ind i et billede.
        /// </summary>
        /// <param name="bytes">Array af bytes, der skal loades ind i billedet.</param>
        /// <param name="image">Billede, hvori arrayet af bytes skal loades ind i.</param>
        private async Task LoadImageAsync(byte[] bytes, BitmapSource image)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }
            using (var memoryStream = new InMemoryRandomAccessStream())
            {
                await memoryStream.WriteAsync(bytes.AsBuffer());
                memoryStream.Seek(0);
                if (_synchronizationContext == null)
                {
                    await image.SetSourceAsync(memoryStream);
                    return;
                }
                _synchronizationContext.Post(async (obj) => await image.SetSourceAsync((IRandomAccessStream) obj), memoryStream);
            }
        }

        #endregion
    }
}
