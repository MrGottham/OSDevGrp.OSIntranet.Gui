using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Models;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core
{
    public interface IOfflineDataCommitter : IDisposable
    {
        Task PushAsync(IAccountingModel accountingModel);
    }
}