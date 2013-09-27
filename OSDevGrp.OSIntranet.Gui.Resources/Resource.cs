using System;
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

        private static readonly ResourceManager ExceptionMessages = new ResourceManager("OSDevGrp.OSIntranet.Gui.Resources.ExceptionMessages", Assembly.GetExecutingAssembly());
        private static readonly ResourceManager Texts = new ResourceManager("OSDevGrp.OSIntranet.Gui.Resources.Texts", Assembly.GetExecutingAssembly());

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
                var message = ExceptionMessages.GetString(exceptionMessage.ToString());
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
                var txt = Texts.GetString(text.ToString());
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

        #endregion
    }
}
