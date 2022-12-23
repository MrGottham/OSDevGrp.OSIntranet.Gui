using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Options;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Core
{
    internal abstract class OnlineRepositoryBase : RepositoryBase, IOnlineRepository
    {
        #region Constructor

        protected OnlineRepositoryBase(IOptions<OnlineOptions> onlineOptions, IAccessTokenProvider accessTokenProvider, IEventPublisher eventPublisher)
            : base(eventPublisher)
        {
            NullGuard.NotNull(onlineOptions, nameof(onlineOptions))
                .NotNull(accessTokenProvider, nameof(accessTokenProvider));

            OnlineOptions = onlineOptions;
            AccessTokenProvider = accessTokenProvider;
        }

        #endregion

        #region Properties

        protected IOptions<OnlineOptions> OnlineOptions { get; }

        protected Uri ApiEndpoint => OnlineOptions.Value.ApiEndpoint;

        protected IAccessTokenProvider AccessTokenProvider { get; }

        #endregion
    }
}