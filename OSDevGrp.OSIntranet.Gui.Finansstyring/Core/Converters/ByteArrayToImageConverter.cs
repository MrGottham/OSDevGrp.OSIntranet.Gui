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
            Task.Run(() => LoadImageAsync(value as byte[], result, _synchronizationContext));
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
            throw new NotSupportedException();
        }

        /// <summary>
        /// Loader et array af bytes ind i et billede.
        /// </summary>
        /// <param name="bytes">Array af bytes, der skal loades ind i billedet.</param>
        /// <param name="image">Billede, hvori arrayet af bytes skal loades ind i.</param>
        /// <param name="synchronizationContext">Synkroniseringskontekst.</param>
        private static async Task LoadImageAsync(byte[] bytes, BitmapSource image, SynchronizationContext synchronizationContext)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }
            if (synchronizationContext == null)
            {
                using (var memoryStream = new InMemoryRandomAccessStream())
                {
                    await memoryStream.WriteAsync(bytes.AsBuffer());
                    memoryStream.Seek(0);
                    await image.SetSourceAsync(memoryStream);
                }
                return;
            }
            var arguments = new Tuple<byte[], BitmapSource>(bytes, image);
            synchronizationContext.Post(async obj =>
                {
                    var tuple = (Tuple<byte[], BitmapSource>) obj;
                    await LoadImageAsync(tuple.Item1, tuple.Item2, null);
                }, arguments);
        }

        #endregion
    }
}
