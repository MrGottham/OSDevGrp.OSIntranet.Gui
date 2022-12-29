using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.App.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Events;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Security.Events;

namespace OSDevGrp.OSIntranet.Gui.App;

public partial class AppShell : IEventHandler<ISystemWentOfflineEvent>, IEventHandler<IOfflineDataUpdatedEvent>, IEventHandler<IAccessTokenAcquiredEvent>
{
	#region Private variables

    private bool _disposed;
    private readonly AppShellViewModel _appShellViewModel;
    private readonly IEventPublisher _eventPublisher;

    #endregion

	#region Constructor

	public AppShell(AppShellViewModel appShellViewModel, IEventPublisher eventPublisher)
    {
        NullGuard.NotNull(appShellViewModel, nameof(appShellViewModel))
            .NotNull(eventPublisher, nameof(eventPublisher));

        _appShellViewModel = appShellViewModel;

        _eventPublisher = eventPublisher;
        _eventPublisher.AddSubscriber(this);

        InitializeComponent();
    }

    #endregion

    #region Methods

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public Task HandleAsync(ISystemWentOfflineEvent systemWentOfflineEvent)
    {
        NullGuard.NotNull(systemWentOfflineEvent, nameof(systemWentOfflineEvent));

        _appShellViewModel.SystemIsOffline = true;

        return Task.CompletedTask;
    }

    public Task HandleAsync(IOfflineDataUpdatedEvent offlineDataUpdatedEvent)
    {
        NullGuard.NotNull(offlineDataUpdatedEvent, nameof(offlineDataUpdatedEvent));

        // TODO: Handle the event.

        return Task.CompletedTask;
    }

    public Task HandleAsync(IAccessTokenAcquiredEvent accessTokenAcquiredEvent)
    {
        NullGuard.NotNull(accessTokenAcquiredEvent, nameof(accessTokenAcquiredEvent));

        // TODO: Handle the event.

        return Task.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        _eventPublisher.RemoveSubscriber(this);

        _disposed = true;
    }

    #endregion
}