using System;
using System.IO;
using Windows.Storage;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Resources;

namespace OSDevGrp.OSIntranet.Gui.Runtime
{
    /// <summary>
    /// Hjælpeklasse til det lokale datalager.
    /// </summary>
    public static class LocaleDataStorageHelper
    {
        /// <summary>
        /// Eventhandler, der kan afgøre, om der findes et lokalt datalager indeholdende data.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        public static void HasLocaleDataEventHandler(object sender, object eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }

            var handleEvaluationEventArgs = eventArgs as IHandleEvaluationEventArgs;
            if (handleEvaluationEventArgs == null)
            {
                throw new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "eventArgs", eventArgs.GetType()));
            }
        }

        /// <summary>
        /// Eventhandler, der danner en læsestream til det lokale datalager.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        public static void CreateReaderStreamEventHandler(object sender, object eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }

            var handleEvaluationEventArgs = eventArgs as IHandleEvaluationEventArgs;
            if (handleEvaluationEventArgs == null)
            {
                throw new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "eventArgs", eventArgs.GetType()));
            }
        }

        /// <summary>
        /// Eventhandler, der danner en skrivestream til det lokale datalager.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        public static void CreateWriterStreamEventHandler(object sender, object eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }

            var handleEvaluationEventArgs = eventArgs as IHandleEvaluationEventArgs;
            if (handleEvaluationEventArgs == null)
            {
                throw new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "eventArgs", eventArgs.GetType()));
            }
        }

        public static async void Test()
        {
            var sf = await ApplicationData.Current.RoamingFolder.GetFileAsync("XYZ").AsTask().ConfigureAwait(false);
            var s = await sf.OpenReadAsync().AsTask().ConfigureAwait(false);

            s.AsStream();
        }
    }
}
