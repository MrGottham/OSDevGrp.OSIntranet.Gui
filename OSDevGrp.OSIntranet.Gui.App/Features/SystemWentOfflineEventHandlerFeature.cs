using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.App.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Events;

namespace OSDevGrp.OSIntranet.Gui.App.Features
{
    internal sealed class SystemWentOfflineEventHandlerFeature : EventHandlerFeatureBase<ISystemWentOfflineEvent>
    {
        #region Private variables

        private readonly AppShellViewModel _appShellViewModel;

        #endregion

        #region Constructor

        public SystemWentOfflineEventHandlerFeature(AppShellViewModel appShellViewModel, IEventPublisher eventPublisher, ILogger<SystemWentOfflineEventHandlerFeature> logger)
            : base(eventPublisher, logger)
        {
            NullGuard.NotNull(appShellViewModel, nameof(appShellViewModel));

            _appShellViewModel = appShellViewModel;
        }

        #endregion

        #region Methods

        protected override Task OnHandleAsync(ISystemWentOfflineEvent systemWentOfflineEvent)
        {
            NullGuard.NotNull(systemWentOfflineEvent, nameof(systemWentOfflineEvent));

            _appShellViewModel.SystemIsOffline = true;

            return Task.CompletedTask;
        }

        #endregion
    }
}