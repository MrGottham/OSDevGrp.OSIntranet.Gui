using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Models;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security
{
    public interface IAccessTokenProvider
    {
        Task<IAccessTokenModel> GetAccessTokenAsync();
    }
}