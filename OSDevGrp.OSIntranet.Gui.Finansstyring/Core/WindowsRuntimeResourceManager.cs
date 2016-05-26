using Windows.ApplicationModel.Resources;
using OSDevGrp.OSIntranet.Gui.Resources;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring.Core
{
    public static class WindowsRuntimeResourceManager
    {
        private static readonly ResourceLoader ExceptionMessages = ResourceLoader.GetForViewIndependentUse("ExceptionMessages");
        private static readonly ResourceLoader Texts = ResourceLoader.GetForViewIndependentUse("Texts");

        public static void PatchResourceManagers()
        {
            Resource.PatchResourceManagerForExceptionMessages(ExceptionMessages.GetString);
            Resource.PatchResourceManagerForTexts(Texts.GetString);
        }
    }
}
