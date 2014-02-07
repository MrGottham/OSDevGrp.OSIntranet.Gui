using System;
namespace OSDevGrp.OSIntranet.Gui.Finansstyring.Core
{
    public class SuspensionManagerException : Exception
    {
        public SuspensionManagerException()
        {
        }

        public SuspensionManagerException(Exception e)
            : base("SuspensionManager failed", e)
        {
        }
    }
}
