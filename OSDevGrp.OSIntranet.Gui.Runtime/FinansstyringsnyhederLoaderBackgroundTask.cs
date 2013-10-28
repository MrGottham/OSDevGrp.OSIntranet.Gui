﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace OSDevGrp.OSIntranet.Gui.Runtime
{
    /// <summary>
    /// Baggrundstask, der kan loaded nyheder til finansstyring.
    /// </summary>
    public sealed class FinansstyringsnyhederLoaderBackgroundTask : IBackgroundTask
    {
        #region Methods

        /// <summary>
        /// Starter baggrundstask, der kan loaded nyheder til finansstyring.
        /// </summary>
        /// <param name="taskInstance">Instans af baggrundstasken.</param>
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            try
            {
                var configurationProvider = new ConfigurationProvider();

                var finansstyringKonfigurationRepository = new FinansstyringKonfigurationRepository();
                finansstyringKonfigurationRepository.KonfigurationerAdd(configurationProvider.Settings);

                var finansstyringRepository = new FinansstyringRepository(finansstyringKonfigurationRepository);

                var finansstyringsnyheder = await FinansstyringsnyhederGetAsync(finansstyringRepository);
                UpdateTile(finansstyringsnyheder);
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

        /// <summary>
        /// Henter alle nyheder til finansstyring.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <returns>Nyheder til finansstyring.</returns>
        private static async Task<IEnumerable<INyhedModel>> FinansstyringsnyhederGetAsync(IFinansstyringRepository finansstyringRepository)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            var nyheder = new List<INyhedModel>();
            try
            {
                var statusDato = DateTime.Now;
                var konfiguration = finansstyringRepository.Konfiguration;
                foreach (var regnskab in await finansstyringRepository.RegnskabslisteGetAsync())
                {
                    nyheder.AddRange((await finansstyringRepository.BogføringslinjerGetAsync(regnskab.Nummer, statusDato, konfiguration.AntalBogføringslinjer)).Where(m => m.Nyhedsudgivelsestidspunkt.Date.CompareTo(statusDato.AddDays(konfiguration.DageForNyheder*-1).Date) >= 0 && m.Nyhedsudgivelsestidspunkt.Date.CompareTo(statusDato.Date) <= 0).ToList());
                }
                return nyheder;
            }
            catch (IntranetGuiRepositoryException)
            {
                return nyheder;
            }
        }

        /// <summary>
        /// Opdaterer applikationstile med nyheder til finansstyring.
        /// </summary>
        /// <param name="nyheder">Nyheder til finansstyring.</param>
        private static void UpdateTile(IEnumerable<INyhedModel> nyheder)
        {
            if (nyheder == null)
            {
                throw new ArgumentNullException("nyheder");
            }
            var tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
            tileUpdater.EnableNotificationQueue(true);
            tileUpdater.Clear();
            foreach (var nyhedsgruppe in nyheder.Where(m => string.IsNullOrEmpty(m.Nyhedsinformation) == false).OrderByDescending(m => m.Nyhedsudgivelsestidspunkt.Date).Take(5).GroupBy(m => m.Nyhedsudgivelsestidspunkt.Date))
            {
                foreach (var nyhed in nyhedsgruppe.OrderByDescending(m => m.Nyhedsaktualitet))
                {
                    tileUpdater.Update(UpdateTile(nyhed, TileTemplateType.TileSquareText04));
                    tileUpdater.Update(UpdateTile(nyhed, TileTemplateType.TileWideBlockAndText02));
                }
            }
        }

        /// <summary>
        /// Opdaterer applikationstile med en given nyhed på en given templatetype.
        /// </summary>
        /// <param name="nyhed">Nyhed, som applikationstile skal opdateres med.</param>
        /// <param name="type">Templatetypen, som skal præsentere nyheden.</param>
        /// <returns>Notifikation, som applikationstile skal benytte.</returns>
        private static TileNotification UpdateTile(INyhedModel nyhed, TileTemplateType type)
        {
            if (nyhed == null)
            {
                throw new ArgumentNullException("nyhed");
            }
            var tileXml = TileUpdateManager.GetTemplateContent(type);
            tileXml.GetElementsByTagName("text").First().InnerText = nyhed.Nyhedsinformation;
            return new TileNotification(tileXml);
            
        }

        #endregion
    }
}