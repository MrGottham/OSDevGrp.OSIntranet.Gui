using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Models;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Events
{
    public interface IAccessTokenAcquiredEvent : IEvent
    {
        IAccessTokenModel AccessToken { get; }
    }
}