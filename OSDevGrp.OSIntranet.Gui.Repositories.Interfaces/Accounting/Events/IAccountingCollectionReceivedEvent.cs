using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Models;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Events
{
    public interface IAccountingCollectionReceivedEvent : IEvent
    {
        IEnumerable<IAccountingModel> Accountings { get; }
    }
}