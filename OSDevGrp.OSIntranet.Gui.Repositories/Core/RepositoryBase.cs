using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Core
{
    internal abstract class RepositoryBase : IRepository
    {
        #region Constructor

        protected RepositoryBase(IEventPublisher eventPublisher)
        {
            NullGuard.NotNull(eventPublisher, nameof(eventPublisher));

            EventPublisher = eventPublisher;
        }

        #endregion

        #region Properties

        protected IEventPublisher EventPublisher { get; }

        #endregion

        #region Methods

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        #endregion
    }
}