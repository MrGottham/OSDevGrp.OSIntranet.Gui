using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;

namespace OSDevGrp.OSIntranet.Gui.Runtime
{
    /// <summary>
    /// Baggrundstask, der kan loaded nyheder til finansstyring.
    /// </summary>
    public sealed class FinansstyringsnyhederShowerBackgroundTask : IBackgroundTask
    {
        #region Methods

        /// <summary>
        /// Starter baggrundstask, der kan loaded nyheder til finansstyring.
        /// </summary>
        /// <param name="taskInstance">Instans af baggrundstasken.</param>
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            try
            {

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                deferral.Complete();
            }
        }

        #endregion
    }
}
