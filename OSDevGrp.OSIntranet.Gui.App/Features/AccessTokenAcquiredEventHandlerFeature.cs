using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.App.Settings;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Events;

namespace OSDevGrp.OSIntranet.Gui.App.Features
{
    internal sealed class AccessTokenAcquiredEventHandlerFeature : EventHandlerFeatureBase<IAccessTokenAcquiredEvent>
    {
        #region Private variables

        private readonly IApplicationDataProvider _applicationDataProvider;

        #endregion

        #region Constructor

        public AccessTokenAcquiredEventHandlerFeature(IApplicationDataProvider applicationDataProvider, IEventPublisher eventPublisher, ILogger<AccessTokenAcquiredEventHandlerFeature> logger)
            : base(eventPublisher, logger)
        {
            NullGuard.NotNull(applicationDataProvider, nameof(applicationDataProvider));

            _applicationDataProvider = applicationDataProvider;
        }

        #endregion

        #region Methods

        protected override async Task OnHandleAsync(IAccessTokenAcquiredEvent accessTokenAcquiredEvent)
        {
            NullGuard.NotNull(accessTokenAcquiredEvent, nameof(accessTokenAcquiredEvent));

            await _applicationDataProvider.SetAccessTokenAsync(accessTokenAcquiredEvent.AccessToken);
        }

        #endregion
    }
}