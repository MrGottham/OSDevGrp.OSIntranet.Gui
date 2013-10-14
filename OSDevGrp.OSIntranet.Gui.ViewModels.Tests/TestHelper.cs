using System;
using System.Reflection;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Resources;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests
{
    /// <summary>
    /// Hjælpefunktionalitet til tests.
    /// </summary>
    public static class TestHelper
    {
        /// <summary>
        /// Loader og returnerer en embedded resouce fra et givent assembly.
        /// </summary>
        /// <param name="assembly">Assembly, hvorfra resource skal loades.</param>
        /// <param name="resourceName">Navnet på resource, der skal loades.</param>
        /// <returns>Bytes for den angivne resource.</returns>
        public static byte[] GetEmbeddedResource(Assembly assembly, string resourceName)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            if (string.IsNullOrEmpty(resourceName))
            {
                throw new ArgumentNullException("resourceName");
            }
                using (var resourceStream = assembly.GetManifestResourceStream(string.Format("{0}.{1}", assembly.GetName().Name, resourceName)))
                {
                    if (resourceStream == null)
                    {
                        throw new IntranetGuiSystemException(Resource.GetExceptionMessage(ExceptionMessage.UnableToLoadResource, string.Format("{0}.{1}", assembly.GetName().Name, resourceName)));
                    }
                    var bytes = new byte[resourceStream.Length];
                    resourceStream.Read(bytes, 0, bytes.Length);
                    return bytes;
                }
        }
    }
}
