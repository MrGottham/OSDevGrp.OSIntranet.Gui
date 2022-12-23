using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.Repositories.Accounting.Events;
using OSDevGrp.OSIntranet.Gui.Repositories.Clients;
using OSDevGrp.OSIntranet.Gui.Repositories.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Exceptions;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Events;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting.Models;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Options;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Accounting
{
    internal class OnlineAccountingRepository : OnlineRepositoryBase, IOnlineAccountingRepository
    {
        #region Constructor

        public OnlineAccountingRepository(IOptions<OnlineOptions> onlineOptions, IAccessTokenProvider accessTokenProvider, IEventPublisher eventPublisher) 
            : base(onlineOptions, accessTokenProvider, eventPublisher)
        {
        }

        #endregion

        #region Methdos

        public async Task<IEnumerable<IAccountingModel>> GetAccountingsAsync()
        {
            try
            {
                IAccessTokenModel accessTokenModel = await AccessTokenProvider.GetAccessTokenAsync();

                using HttpClient httpClient = new HttpClient();
                AccountingClient accountingClient = new AccountingClient(ApiEndpoint, httpClient, accessTokenModel);

                IAccountingModel[] accountingModels = (await accountingClient.AccountingsAsync())
                    .Select(a => a.AsInterface())
                    .ToArray();

                await EventPublisher.PublishAsync<IAccountingCollectionReceivedEvent>(new AccountingCollectionReceivedEvent(accountingModels));

                return accountingModels;
            }
            catch (ApiException<ErrorModel> ex)
            {
                throw ex.ToException();
            }
            catch (ApiException ex)
            {
                throw ex.ToException();
            }
            catch (HttpRequestException ex)
            {
                throw ex.ToException();
            }
        }

        #endregion
    }
}