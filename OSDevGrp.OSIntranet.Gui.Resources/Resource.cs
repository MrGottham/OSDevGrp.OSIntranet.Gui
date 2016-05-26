using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Threading;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.Gui.Resources
{
    /// <summary>
    /// Klasse, der kan tilgå ressourcer.
    /// </summary>
    public class Resource
    {
        #region Private variables

        private static Func<string, string> _exceptionMessageGetter;
        private static Func<string, string> _textGetter;
        private static readonly ResourceManager ExceptionMessages = new ResourceManager(typeof(ExceptionMessages).FullName, Assembly.GetExecutingAssembly());
        private static readonly ResourceManager Texts = new ResourceManager(typeof(Texts).FullName, Assembly.GetExecutingAssembly());
        private static readonly IDictionary<string, byte[]> ResourceCache = new Dictionary<string, byte[]>();
        private static readonly object SyncRoot = new object();

        #endregion
        
        #region Methods

        /// <summary>
        /// Returnerer fejlbesked for en given exception message.
        /// </summary>
        /// <param name="exceptionMessage">Exception message, hvortil fejlbesked skal returneres.</param>
        /// <param name="args">Argumenter til fejlbeskeden.</param>
        /// <returns>Fejlbesked.</returns>
        public static string GetExceptionMessage(ExceptionMessage exceptionMessage, params object[] args)
        {
            try
            {
                var message = _exceptionMessageGetter == null ? ExceptionMessages.GetString(exceptionMessage.ToString()) : _exceptionMessageGetter.Invoke(exceptionMessage.ToString());
                if (message == null)
                {
                    throw new IntranetGuiSystemException("Null returned.");
                }
                return args == null ? message : string.Format(message, args);
            }
            catch (Exception ex)
            {
                throw new IntranetGuiSystemException(string.Format("Could not get resource string named '{0}' using culture {1}.", exceptionMessage, Thread.CurrentThread.CurrentUICulture.Name), ex);
            }
        }

        /// <summary>
        /// Returnerer tekst til en given tekstangivelse.
        /// </summary>
        /// <param name="text">Tekstangivelse, hvortil tekst skal returneres.</param>
        /// <param name="args">Argumenter til teksten.</param>
        /// <returns>Tekst.</returns>
        public static string GetText(Text text, params object[] args)
        {
            try
            {
                var txt = _textGetter == null ? Texts.GetString(text.ToString()) : _textGetter.Invoke(text.ToString());
                if (txt == null)
                {
                    throw new IntranetGuiSystemException("Null returned.");
                }
                return args == null ? txt : string.Format(txt, args);
            }
            catch (Exception ex)
            {
                throw new IntranetGuiSystemException(string.Format("Could not get resource string named '{0}' using culture {1}.", text, Thread.CurrentThread.CurrentUICulture.Name), ex);
            }
        }

        /// <summary>
        /// Loader og returnerer en embedded resource.
        /// </summary>
        /// <param name="resourceName">Navn på resources, der skal loades.</param>
        /// <returns>Bytes for den angivne resource.</returns>
        public static byte[] GetEmbeddedResource(string resourceName)
        {
            if (string.IsNullOrEmpty(resourceName))
            {
                throw new ArgumentNullException("resourceName");
            }
            lock (SyncRoot)
            {
                if (ResourceCache.ContainsKey(resourceName))
                {
                    return ResourceCache[resourceName];
                }
                var resource = new Resource();
                var assembly = resource.GetType().Assembly;
                using (var resourceStream = assembly.GetManifestResourceStream(string.Format("OSDevGrp.OSIntranet.Gui.Resources.{0}", resourceName)))
                {
                    if (resourceStream == null)
                    {
                        throw new IntranetGuiSystemException(GetExceptionMessage(ExceptionMessage.UnableToLoadResource, string.Format("OSDevGrp.OSIntranet.Gui.Resources.{0}", resourceName)));
                    }
                    var bytes = new byte[resourceStream.Length];
                    resourceStream.Read(bytes, 0, bytes.Length);
                    ResourceCache.Add(resourceName, bytes);
                    return ResourceCache[resourceName];
                }
            }
        }

        /// <summary>
        /// Patcher den ResourceManager, der kan hente exception messages.
        /// </summary>
        /// <param name="exceptionMessageGetter">Funktion, der kan hente exception messages.</param>
        public static void PatchResourceManagerForExceptionMessages(Func<string, string> exceptionMessageGetter)
        {
            if (exceptionMessageGetter == null)
            {
                throw new ArgumentNullException("exceptionMessageGetter");
            }
            _exceptionMessageGetter = exceptionMessageGetter;
        }

        /// <summary>
        /// Patcher den ResourceManager, der kan hente tekstangivelser.
        /// </summary>
        /// <param name="textGetter">Funktion, der kan hente tekstangivelser.</param>
        public static void PatchResourceManagerForTexts(Func<string, string> textGetter)
        {
            if (textGetter == null)
            {
                throw new ArgumentNullException("textGetter");
            }
            _textGetter = textGetter;
        }

        #endregion
    }
}
