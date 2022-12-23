using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Models;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting
{
    public interface IAccountingRepository : IRepository
    {
        Task<IEnumerable<IAccountingModel>> GetAccountingsAsync();
    }
}