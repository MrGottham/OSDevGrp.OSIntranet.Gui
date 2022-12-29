namespace OSDevGrp.OSIntranet.Gui.App.Settings
{
    public class AppOptions
    {
        #region Properties

        public bool ShouldOpenSettingsOnStartup { get; set; } = true;

        public FileInfo OfflineDataFile { get; set; }

        #endregion
    }
}