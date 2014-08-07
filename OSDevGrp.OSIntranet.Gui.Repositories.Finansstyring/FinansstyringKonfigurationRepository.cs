using System;
using System.Collections.Generic;
using System.IO;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring
{
    /// <summary>
    /// Konfigurationsrepository, der supporterer finansstyring.
    /// </summary>
    public class FinansstyringKonfigurationRepository : IFinansstyringKonfigurationRepository
    {
        #region Private variables

        private readonly IDictionary<string, object> _konfigurationer = new Dictionary<string, object>();
        private static IFinansstyringKonfigurationRepository _finansstyringKonfigurationRepository;
        private static readonly object SyncRoot = new object();

        #endregion

        #region Properties
        
        /// <summary>
        /// Uri til servicen, der supporterer finansstyring.
        /// </summary>
        public virtual Uri FinansstyringServiceUri
        {
            get
            {
                lock (SyncRoot)
                {
                    const string konfigurationNavn = "FinansstyringServiceUri";
                    if (_konfigurationer.ContainsKey(konfigurationNavn) == false)
                    {
                        throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.MissingConfigurationSetting, konfigurationNavn));
                    }
                    try
                    {
                        return new Uri(Convert.ToString(_konfigurationer[konfigurationNavn]));
                    }
                    catch (Exception ex)
                    {
                        throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.InvalidConfigurationSettingValue, konfigurationNavn), ex);
                    }
                }
            }
        }

        /// <summary>
        /// Filnavn til det lokale synkroniseringslager.
        /// </summary>
        public virtual string LokalDataFil
        {
            get
            {
                lock (SyncRoot)
                {
                    const string konfigurationNavn = "LokalDataFil";
                    if (_konfigurationer.ContainsKey(konfigurationNavn) == false)
                    {
                        throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.MissingConfigurationSetting, konfigurationNavn));
                    }
                    var value = Convert.ToString(_konfigurationer[konfigurationNavn]);
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.InvalidConfigurationSettingValue, konfigurationNavn));
                    }
                    try
                    {
                        return Path.GetFileName(value);
                    }
                    catch (Exception ex)
                    {
                        throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.InvalidConfigurationSettingValue, konfigurationNavn), ex);
                    }
                }
            }
        }

        /// <summary>
        /// Filnavn til det lokale synkroniseringslager.
        /// </summary>
        public virtual string SynkroniseringDataFil
        {
            get
            {
                lock (SyncRoot)
                {
                    const string konfigurationNavn = "SynkroniseringDataFil";
                    if (_konfigurationer.ContainsKey(konfigurationNavn) == false)
                    {
                        throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.MissingConfigurationSetting, konfigurationNavn));
                    }
                    var value = Convert.ToString(_konfigurationer[konfigurationNavn]);
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.InvalidConfigurationSettingValue, konfigurationNavn));
                    }
                    try
                    {
                        return Path.GetFileName(value);
                    }
                    catch (Exception ex)
                    {
                        throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.InvalidConfigurationSettingValue, konfigurationNavn), ex);
                    }
                }
            }
        }

        /// <summary>
        /// Antal bogføringslinjer, der skal hentes.
        /// </summary>
        public virtual int AntalBogføringslinjer
        {
            get
            {
                lock (SyncRoot)
                {
                    const string konfigurationNavn = "AntalBogføringslinjer";
                    if (_konfigurationer.ContainsKey(konfigurationNavn) == false)
                    {
                        throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.MissingConfigurationSetting, konfigurationNavn));
                    }
                    try
                    {
                        return Convert.ToInt32(_konfigurationer[konfigurationNavn]);
                    }
                    catch (Exception ex)
                    {
                        throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.InvalidConfigurationSettingValue, konfigurationNavn), ex);
                    }
                }
            }
        }

        /// <summary>
        /// Antal dage, som nyheder er gældende.
        /// </summary>
        public virtual int DageForNyheder
        {
            get
            {
                lock (SyncRoot)
                {
                    const string konfigurationNavn = "DageForNyheder";
                    if (_konfigurationer.ContainsKey(konfigurationNavn) == false)
                    {
                        throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.MissingConfigurationSetting, konfigurationNavn));
                    }
                    try
                    {
                        return Convert.ToInt32(_konfigurationer[konfigurationNavn]);
                    }
                    catch (Exception ex)
                    {
                        throw new IntranetGuiRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.InvalidConfigurationSettingValue, konfigurationNavn), ex);
                    }
                }
            }
        }

        /// <summary>
        /// Returnerer en instans af konfigurationsrepositoryet, der supporterer finansstyring.
        /// </summary>
        public static IFinansstyringKonfigurationRepository Instance
        {
            get
            {
                lock (SyncRoot)
                {
                    return _finansstyringKonfigurationRepository ?? (_finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository());
                }
            }
        }

        /// <summary>
        /// Collection indeholdende navne for de enkelte konfigurationsværdier.
        /// </summary>
        public static IEnumerable<string> Keys
        {
            get
            {
                return new List<string>(3)
                    {
                        "FinansstyringServiceUri",
                        "LokalDataFil",
                        "SynkroniseringDataFil",
                        "AntalBogføringslinjer",
                        "DageForNyheder"
                    };
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Tilføjer konfigurationer til repositoryet.
        /// </summary>
        /// <param name="konfigurationer">Konfigurationer, der skal tilføjes.</param>
        public void KonfigurationerAdd(IDictionary<string, object> konfigurationer)
        {
            if (konfigurationer == null)
            {
                throw new ArgumentNullException("konfigurationer");
            }
            lock (SyncRoot)
            {
                foreach (var konfiguration in konfigurationer)
                {
                    if (_konfigurationer.ContainsKey(konfiguration.Key))
                    {
                        _konfigurationer[konfiguration.Key] = konfiguration.Value;
                        continue;
                    }
                    _konfigurationer.Add(konfiguration.Key, konfiguration.Value);
                }
            }
        }

        #endregion
    }
}
