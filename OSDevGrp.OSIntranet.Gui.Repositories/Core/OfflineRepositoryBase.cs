using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Options;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Core
{
    internal abstract class OfflineRepositoryBase : RepositoryBase, IOfflineRepository
    {
        #region Constructor

        protected OfflineRepositoryBase(IOptions<OfflineOptions> offlineOptions, IOfflineDataProvider offlineDataProvider, IOfflineDataCommitter offlineDataCommitter, IEventPublisher eventPublisher)
            : base(eventPublisher)
        {
            NullGuard.NotNull(offlineOptions, nameof(offlineOptions))
                .NotNull(offlineDataProvider, nameof(offlineDataProvider))
                .NotNull(offlineDataCommitter, nameof(offlineDataCommitter));

            OfflineOptions = offlineOptions;
            OfflineDataProvider = offlineDataProvider;
            OfflineDataCommitter = offlineDataCommitter;
        }

        #endregion

        #region Properties

        protected IOptions<OfflineOptions> OfflineOptions { get; }

        protected IOfflineDataProvider OfflineDataProvider { get; }

        protected IOfflineDataCommitter OfflineDataCommitter { get; }

        #endregion
    }
}