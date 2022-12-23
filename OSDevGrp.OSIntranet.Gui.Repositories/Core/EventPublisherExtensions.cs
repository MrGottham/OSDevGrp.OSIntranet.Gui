using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.Repositories.Core.Events;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Core
{
    internal static class EventPublisherExtensions
    {
        #region Methods

        internal static async Task PublishSystemWentOfflineEventAsync(this IEventPublisher eventPublisher)
        {
            NullGuard.NotNull(eventPublisher, nameof(eventPublisher));

            await eventPublisher.PublishAsync(new SystemWentOfflineEvent());
        }

        #endregion
    }
}