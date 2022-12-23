using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.Repositories.Clients;
using OSDevGrp.OSIntranet.Gui.Repositories.Exceptions;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Events;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Models;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Options;
using OSDevGrp.OSIntranet.Gui.Repositories.Security.Events;
using System.Net.Http;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Security
{
    internal class AccessTokenProvider : IAccessTokenProvider
    {
        #region Private variables

        private readonly IOptions<SecurityOptions> _securityOptions;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Constructor

        public AccessTokenProvider(IOptions<SecurityOptions> securityOptions, IEventPublisher eventPublisher)
        {
            NullGuard.NotNull(securityOptions, nameof(securityOptions))
                .NotNull(eventPublisher, nameof(eventPublisher));

            _securityOptions = securityOptions;
            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        public async Task<IAccessTokenModel> GetAccessTokenAsync()
        {
            try
            {
                using HttpClient httpClient = new HttpClient();

                SecurityClient securityClient = new SecurityClient(_securityOptions.Value.ApiEndpoint, httpClient, _securityOptions.Value.ClientId, _securityOptions.Value.ClientSecret);
                IAccessTokenModel accessTokenModel =
                    (await securityClient.TokenAsync("client_credentials")).AsInterface();

                await _eventPublisher.PublishAsync<IAccessTokenAcquiredEvent>(new AccessTokenAcquiredEvent(accessTokenModel));

                return accessTokenModel;
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