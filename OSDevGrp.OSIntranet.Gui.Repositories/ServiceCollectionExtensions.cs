using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Options;
using OSDevGrp.OSIntranet.Gui.Repositories.Security;
using System;

namespace OSDevGrp.OSIntranet.Gui.Repositories
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection, Action<SecurityOptions> configureSecurityOptions)
        {
            NullGuard.NotNull(serviceCollection, nameof(serviceCollection))
                .NotNull(configureSecurityOptions, nameof(configureSecurityOptions));

            serviceCollection.Configure(configureSecurityOptions);

            serviceCollection.AddEventPublisher();
            serviceCollection.AddScoped<IAccessTokenProvider>(serviceProvider => new AccessTokenProviderCache(new AccessTokenProvider(serviceProvider.GetRequiredService<IOptions<SecurityOptions>>(), serviceProvider.GetRequiredService<IEventPublisher>())));
            serviceCollection.AddScoped(serviceProvider => (IAccessTokenSetter)serviceProvider.GetRequiredService<IAccessTokenProvider>());

            return serviceCollection;
        }
    }
}