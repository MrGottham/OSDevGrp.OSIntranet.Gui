using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Gui.App.Core;
using OSDevGrp.OSIntranet.Gui.App.Features;
using OSDevGrp.OSIntranet.Gui.App.Settings;
using OSDevGrp.OSIntranet.Gui.Repositories;

namespace OSDevGrp.OSIntranet.Gui.App;

public static class MauiProgram
{
    #region Methods

    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder()
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Logging.AddEventSourceLogger();
        builder.Logging.AddFilter(typeof(MauiApp).Namespace, logLevel => logLevel >= LogLevel.Information);

        return builder
            .RegisterAppServices(ApplicationDataProvider.Create(() => SecureStorage.Default, () => FileSystem.Current))
            .RegisterViewModels()
            .RegisterViews()
            .Build();
    }

    private static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder, IApplicationDataProvider applicationDataProvider)
    {
        NullGuard.NotNull(mauiAppBuilder, nameof(mauiAppBuilder))
            .NotNull(applicationDataProvider, nameof(applicationDataProvider));

        mauiAppBuilder.Services
            .Configure<AppOptions>(appOptions => { })
            .AddRepositories(onlineOptions => { },
                offlineOptions => { },
                securityOptions => { })
            .AddSingleton(applicationDataProvider)
            .AddScoped<IBackgroundFeature, EventListenerFeature>()
            .AddScoped<IBackgroundFeature, SystemWentOfflineEventHandlerFeature>()
            .AddScoped<IBackgroundFeature, OfflineDataUpdatedEventHandlerFeature>()
            .AddScoped<IBackgroundFeature, AccessTokenAcquiredEventHandlerFeature>();

        return mauiAppBuilder;
    }

    private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        NullGuard.NotNull(mauiAppBuilder, nameof(mauiAppBuilder));

        mauiAppBuilder.Services
            .AddScoped<StartupViewModel>()
            .AddScoped<AppShellViewModel>()
            .AddScoped<SettingsViewModel>()
            .AddScoped(serviceProvider => new Func<SettingsViewModel>(serviceProvider.GetRequiredService<SettingsViewModel>));

        return mauiAppBuilder;
    }

    private static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        NullGuard.NotNull(mauiAppBuilder, nameof(mauiAppBuilder));

        mauiAppBuilder.Services
            .AddScoped<StartupPage>()
            .AddScoped(serviceProvider => new Func<StartupPage>(serviceProvider.GetRequiredService<StartupPage>))
            .AddScoped<AppShell>()
            .AddScoped(serviceProvider => new Func<AppShell>(serviceProvider.GetRequiredService<AppShell>))
            .AddScoped<MainPage>()
            .AddScoped<SettingsPage>()
            .AddScoped(serviceProvider => new Func<SettingsPage>(serviceProvider.GetRequiredService<SettingsPage>));

        return mauiAppBuilder;
    }

    #endregion
}