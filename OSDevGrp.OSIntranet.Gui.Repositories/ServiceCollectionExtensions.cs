using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.Repositories.Accounting;
using OSDevGrp.OSIntranet.Gui.Repositories.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Options;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Options;
using OSDevGrp.OSIntranet.Gui.Repositories.Security;
using System;

namespace OSDevGrp.OSIntranet.Gui.Repositories
{
    public static class ServiceCollectionExtensions
    {
        #region Methods

        public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection, Action<OnlineOptions> configureOnlineOptions, Action<OfflineOptions> configureOfflineOptions, Action<SecurityOptions> configureSecurityOptions)
        {
            NullGuard.NotNull(serviceCollection, nameof(serviceCollection))
                .NotNull(configureOnlineOptions, nameof(configureOnlineOptions))
                .NotNull(configureOfflineOptions, nameof(configureOfflineOptions))
                .NotNull(configureSecurityOptions, nameof(configureSecurityOptions));

            serviceCollection.Configure(configureOnlineOptions);
            serviceCollection.Configure(configureOfflineOptions);
            serviceCollection.Configure(configureSecurityOptions);

            serviceCollection.AddEventPublisher();

            serviceCollection.AddScoped<IAccessTokenProvider>(serviceProvider => new AccessTokenProviderCache(new AccessTokenProvider(serviceProvider.GetRequiredService<IOptions<SecurityOptions>>(), serviceProvider.GetRequiredService<IEventPublisher>()), serviceProvider.GetRequiredService<IEventPublisher>()));
            serviceCollection.AddScoped(serviceProvider => (IAccessTokenSetter)serviceProvider.GetRequiredService<IAccessTokenProvider>());

            serviceCollection.AddSingleton<IOfflineDataProvider, OfflineDataProvider>();
            serviceCollection.AddScoped<IOfflineDataCommitter, OfflineDataCommitter>();

            serviceCollection.AddScoped<IAccountingRepository, AccountingRepository>();
            serviceCollection.AddScoped<IOnlineAccountingRepository, OnlineAccountingRepository>();
            serviceCollection.AddScoped<IOfflineAccountingRepository, OfflineAccountingRepository>();

            return serviceCollection;
        }

        #endregion
    }
}