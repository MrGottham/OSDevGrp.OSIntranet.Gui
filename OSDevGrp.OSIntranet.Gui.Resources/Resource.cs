using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Resources.Images;
using OSDevGrp.OSIntranet.Gui.Resources.Schemas;

namespace OSDevGrp.OSIntranet.Gui.Resources
{
    /// <summary>
    /// Klasse, der kan tilgå ressourcer.
    /// </summary>
    public class Resource
    {
        #region Private variables

        private static readonly IReadOnlyDictionary<string, string> DefaultExceptionMessages = GetDefaultMessageDictionary(typeof(ExceptionMessage));
        private static readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> GlobalizedExceptionMessages = GetGlobalizedMessageDictionary(typeof(ExceptionMessage));
        private static readonly IReadOnlyDictionary<string, string> DefaultTexts = GetDefaultMessageDictionary(typeof(Text));
        private static readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> GlobalizedTexts = GetGlobalizedMessageDictionary(typeof(Text));
        private static readonly IReadOnlyDictionary<string, EmbeddedResourceBase> EmbeddedResources = GetEmbeddedResourceDictionary(
            new AccountImage(),
            new BudgetAccountImage(),
            new AddressAccountImage(),
            new PostingLineImage(),
            new FinansstyringRepositoryLocaleSchema());

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
            return ResolveMessage(exceptionMessage.ToString(), GlobalizedExceptionMessages, DefaultExceptionMessages, CultureInfo.CurrentUICulture, args);
        }

        /// <summary>
        /// Returnerer tekst til en given tekstangivelse.
        /// </summary>
        /// <param name="text">Tekstangivelse, hvortil tekst skal returneres.</param>
        /// <param name="args">Argumenter til teksten.</param>
        /// <returns>Tekst.</returns>
        public static string GetText(Text text, params object[] args)
        {
            return ResolveMessage(text.ToString(), GlobalizedTexts, DefaultTexts, CultureInfo.CurrentUICulture, args);
        }

        /// <summary>
        /// Loader og returnerer en embedded resource.
        /// </summary>
        /// <param name="resourceName">Navn på resources, der skal loades.</param>
        /// <returns>Bytes for den angivne resource.</returns>
        public static byte[] GetEmbeddedResource(string resourceName)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            try
            {
                EmbeddedResourceBase embeddedResource;
                if (EmbeddedResources.TryGetValue(resourceName, out embeddedResource))
                {
                    return embeddedResource.Value;
                }

                throw new IntranetGuiSystemException(GetExceptionMessage(ExceptionMessage.UnableToLoadResource, $"OSDevGrp.OSIntranet.Gui.Resources.{resourceName}"));
            }
            catch (IntranetGuiSystemException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetGuiSystemException(GetExceptionMessage(ExceptionMessage.UnableToLoadResource, $"OSDevGrp.OSIntranet.Gui.Resources.{resourceName}"), ex);
            }
        }

        private static string ResolveMessage(string fieldName, IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> globalizedMessages, IReadOnlyDictionary<string, string> defaultMessages, CultureInfo cultureInfo, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                throw new ArgumentNullException(nameof(fieldName));
            }

            if (globalizedMessages == null)
            {
                throw new ArgumentNullException(nameof(globalizedMessages));
            }

            if (defaultMessages == null)
            {
                throw new ArgumentNullException(nameof(defaultMessages));
            }

            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }

            try
            {
                string message;

                IReadOnlyDictionary<string, string> messageDictionary;
                if (globalizedMessages.TryGetValue(fieldName, out messageDictionary))
                {
                    if (messageDictionary.TryGetValue(cultureInfo.Name, out message))
                    {
                        return string.Format(message, args);
                    }
                }

                if (defaultMessages.TryGetValue(fieldName, out message))
                {
                    return string.Format(message, args);
                }

                throw new IntranetGuiSystemException($"Could not get resource string named '{fieldName}' using culture {cultureInfo.Name}.");
            }
            catch (IntranetGuiSystemException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetGuiSystemException($"Could not get resource string named '{fieldName}' using culture {cultureInfo.Name}.", ex);
            }
        }

        private static IReadOnlyDictionary<string, string> GetDefaultMessageDictionary(Type enumType)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException(nameof(enumType));
            }

            return new ConcurrentDictionary<string, string>(enumType.GetRuntimeFields()
                .Where(fieldInfo => fieldInfo.GetCustomAttributes<DefaultMessageAttribute>().SingleOrDefault(defaultMessageAttribute => defaultMessageAttribute.GetType() == typeof(DefaultMessageAttribute)) != null)
                .ToDictionary(fieldInfo => fieldInfo.Name, fieldInfo => fieldInfo.GetCustomAttributes<DefaultMessageAttribute>().Single(defaultMessageAttribute => defaultMessageAttribute.GetType() == typeof(DefaultMessageAttribute)).Message));
        }

        private static IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> GetGlobalizedMessageDictionary(Type enumType)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException(nameof(enumType));
            }

            return new ConcurrentDictionary<string, IReadOnlyDictionary<string, string>>(enumType.GetRuntimeFields()
                .Where(fieldInfo => fieldInfo.GetCustomAttributes<GlobalizedMessageAttribute>().Any())
                .ToDictionary(fieldInfo => fieldInfo.Name, fieldInfo => (IReadOnlyDictionary<string, string>)new ConcurrentDictionary<string, string>(fieldInfo.GetCustomAttributes<GlobalizedMessageAttribute>().ToDictionary(globalizedMessageAttribute => globalizedMessageAttribute.CultureInfo.Name, globalizedMessageAttribute => globalizedMessageAttribute.Message))));
        }

        private static IReadOnlyDictionary<string, EmbeddedResourceBase> GetEmbeddedResourceDictionary(params EmbeddedResourceBase[] embeddedResourceCollection)
        {
            if (embeddedResourceCollection == null)
            {
                throw new ArgumentNullException(nameof(embeddedResourceCollection));
            }

            return new ConcurrentDictionary<string, EmbeddedResourceBase>(embeddedResourceCollection.ToDictionary(embeddedResource => embeddedResource.Name, embeddedResource => embeddedResource));
        }

        #endregion
    }
}