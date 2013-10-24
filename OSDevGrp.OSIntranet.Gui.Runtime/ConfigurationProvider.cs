using System;
using System.Collections.Generic;
using Windows.Storage;

namespace OSDevGrp.OSIntranet.Gui.Runtime
{
    /// <summary>
    /// Leverandør af konfigurationer.
    /// </summary>
    public sealed class ConfigurationProvider
    {
        #region Private variables

        private readonly ApplicationDataContainer _roamingSettings = ApplicationData.Current.RoamingSettings;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner leverandør af konfigurationer.
        /// </summary>
        public ConfigurationProvider()
        {
            if (_roamingSettings.Values.ContainsKey("FinansstyringServiceUri") == false)
            {
                _roamingSettings.Values.Add("FinansstyringServiceUri", "http://mother/osintranet/finansstyringservice.svc/mobile");
            }
            if (_roamingSettings.Values.ContainsKey("AntalBogføringslinjer") == false)
            {
                _roamingSettings.Values.Add("AntalBogføringslinjer", 50);
            }
            if (_roamingSettings.Values.ContainsKey("DageForNyheder") == false)
            {
                _roamingSettings.Values.Add("DageForNyheder", 7);
            }
            _roamingSettings.Values["DageForNyheder"] = 30;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Dictionary indeholdende navngivne konfigurationer.
        /// </summary>
        public IDictionary<string, object> Settings
        {
            get
            {
                return _roamingSettings.Values;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Opdaterer værdien på en navngiven konfiguration.
        /// </summary>
        /// <param name="settingName">Navn på konfigurationen, der skal opdateres.</param>
        /// <param name="value">Værdi, som konfigurationen skal opdateres med.</param>
        public void SetValue(string settingName, object value)
        {
            if (string.IsNullOrEmpty(settingName))
            {
                throw new ArgumentNullException("settingName");
            }
            if (_roamingSettings.Values.ContainsKey(settingName))
            {
                _roamingSettings.Values[settingName] = value;
                return;
            }
            _roamingSettings.Values.Add(settingName, value);
        }

        #endregion
    }
}
