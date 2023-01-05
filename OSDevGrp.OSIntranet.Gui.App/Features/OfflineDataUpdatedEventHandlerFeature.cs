using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.App.Settings;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Events;

namespace OSDevGrp.OSIntranet.Gui.App.Features
{
    internal sealed class OfflineDataUpdatedEventHandlerFeature : EventHandlerFeatureBase<IOfflineDataUpdatedEvent>
    {
        #region Private variables

        private DateTime? _latestUpdate;
        private readonly IApplicationDataProvider _applicationDataProvider;

        #endregion

        #region Constructor

        public OfflineDataUpdatedEventHandlerFeature(IApplicationDataProvider applicationDataProvider, IEventPublisher eventPublisher, ILogger<OfflineDataUpdatedEventHandlerFeature> logger)
            : base(eventPublisher, logger)
        {
            NullGuard.NotNull(applicationDataProvider, nameof(applicationDataProvider));

            _applicationDataProvider = applicationDataProvider;
        }

        #endregion

        #region Methods

        protected override async Task OnHandleAsync(IOfflineDataUpdatedEvent offlineDataUpdatedEvent)
        {
            NullGuard.NotNull(offlineDataUpdatedEvent, nameof(offlineDataUpdatedEvent));

            if (_latestUpdate == null)
            {
                FileInfo offlineDataFile = _applicationDataProvider.OfflineDataFile;
                offlineDataFile.Refresh();

                if (offlineDataFile.Exists)
                {
                    _latestUpdate = offlineDataFile.LastWriteTime;
                }
            }

            if (_latestUpdate != null && _latestUpdate.Value > offlineDataUpdatedEvent.Updated)
            {
                return;
            }

            await _applicationDataProvider.SetOfflineDataDocumentAsync(offlineDataUpdatedEvent.OfflineDataDocument);

            _latestUpdate = offlineDataUpdatedEvent.Updated;
        }

        #endregion
    }
}