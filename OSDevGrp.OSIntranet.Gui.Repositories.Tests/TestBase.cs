using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Options;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Options;
using System;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Tests
{
    public abstract class TestBase
    {
        #region Methods

        internal static TestEventHandler<TEvent> CreateTestEventHandler<TEvent>(IServiceProvider serviceProvider) where TEvent : class, IEvent
        {
            NullGuard.NotNull(serviceProvider, nameof(serviceProvider));

            return new TestEventHandler<TEvent>(serviceProvider);
        }

        protected static IConfiguration CreateTestConfiguration()
        {
            return new ConfigurationBuilder()
                .AddUserSecrets<TestBase>()
                .Build();
        }

        protected static IServiceScope CreateTestServiceScope()
        {
            return CreateTestServiceProvider().CreateScope();
        }

        private static IServiceProvider CreateTestServiceProvider()
        {
            return CreateTestServiceCollection().BuildServiceProvider();
        }

        private static IServiceCollection CreateTestServiceCollection()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            return serviceCollection.AddRepositories(ConfigureTestOnlineOptions, ConfigureTestOfflineOptions, ConfigureTestSecurityOptions);
        }

        private static void ConfigureTestOnlineOptions(OnlineOptions onlineOptions)
        {
            NullGuard.NotNull(onlineOptions, nameof(onlineOptions));

            ConfigureTestOnlineOptions(onlineOptions, CreateTestConfiguration());
        }

        private static void ConfigureTestOnlineOptions(OnlineOptions onlineOptions, IConfiguration configuration)
        {
            NullGuard.NotNull(onlineOptions, nameof(onlineOptions))
                .NotNull(configuration, nameof(configuration));

            onlineOptions.ApiEndpoint = GetTestApiEndpoint(configuration);
        }

        private static void ConfigureTestOfflineOptions(OfflineOptions offlineOptions)
        {
            NullGuard.NotNull(offlineOptions, nameof(offlineOptions));

            ConfigureTestOfflineOptions(offlineOptions, CreateTestConfiguration());
        }

        private static void ConfigureTestOfflineOptions(OfflineOptions offlineOptions, IConfiguration configuration)
        {
            NullGuard.NotNull(offlineOptions, nameof(offlineOptions))
                .NotNull(configuration, nameof(configuration));

            offlineOptions.OfflineDataDocument = OfflineDataDocumentFactory.Build();
        }

        private static void ConfigureTestSecurityOptions(SecurityOptions securityOptions)
        {
            NullGuard.NotNull(securityOptions, nameof(securityOptions));

            ConfigureTestSecurityOptions(securityOptions, CreateTestConfiguration());
        }

        private static void ConfigureTestSecurityOptions(SecurityOptions securityOptions, IConfiguration configuration)
        {
            NullGuard.NotNull(securityOptions, nameof(securityOptions))
                .NotNull(configuration, nameof(configuration));

            securityOptions.ApiEndpoint = GetTestApiEndpoint(configuration);
            securityOptions.ClientId = GetTestClientId(configuration);
            securityOptions.ClientSecret = GetTestClientSecret(configuration);
        }

        protected static IOfflineDataCommitter CreateTestOfflineDataCommitter(IServiceProvider serviceProvider)
        {
            NullGuard.NotNull(serviceProvider, nameof(serviceProvider));

            return serviceProvider.GetRequiredService<IOfflineDataCommitter>();
        }

        private static Uri GetTestApiEndpoint(IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            if (Uri.TryCreate(configuration[ConfigurationKeys.ApiEndpointKey], UriKind.Absolute, out Uri? testApiEndpoint))
            {
                return testApiEndpoint;
            }

            throw new Exception("Unable to get the Test API Endpoint from the configuration.");
        }

        private static string GetTestClientId(IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            string? clientId = configuration[ConfigurationKeys.SecurityClientIdKey];
            if (string.IsNullOrWhiteSpace(clientId) == false)
            {
                return clientId;
            }

            throw new Exception("Unable to get the Client Id from the configuration.");
        }

        private static string GetTestClientSecret(IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            string? clientSecret = configuration[ConfigurationKeys.SecurityClientSecretKey];
            if (string.IsNullOrWhiteSpace(clientSecret) == false)
            {
                return clientSecret;
            }

            throw new Exception("Unable to get the Client Secret from the configuration.");
        }

        #endregion;
    }
}