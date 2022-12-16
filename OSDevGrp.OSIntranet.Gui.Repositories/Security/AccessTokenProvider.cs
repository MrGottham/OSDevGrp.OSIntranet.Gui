using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.Repositories.Clients;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Events;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Models;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Options;
using OSDevGrp.OSIntranet.Gui.Repositories.Security.Events;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Security
{
    internal class AccessTokenProvider : IAccessTokenProvider
    {
        #region Private variables

        private readonly Uri _apiEndpoint;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Constructor

        public AccessTokenProvider(IOptions<SecurityOptions> securityOptions, IEventPublisher eventPublisher)
        {
            NullGuard.NotNull(securityOptions, nameof(securityOptions))
                .NotNull(eventPublisher, nameof(eventPublisher));

            _apiEndpoint = securityOptions.Value.ApiEndpoint;
            _clientId = securityOptions.Value.ClientId;
            _clientSecret = securityOptions.Value.ClientSecret;
            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        public async Task<IAccessTokenModel> GetAccessTokenAsync()
        {
            try
            {
                using HttpClient httpClient = new HttpClient();

                SecurityClient securityClient = new SecurityClient(_apiEndpoint, httpClient, _clientId, _clientSecret);
                IAccessTokenModel accessTokenModel = (await securityClient.TokenAsync("client_credentials")).AsInterface();

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
        }

        #endregion
    }
}