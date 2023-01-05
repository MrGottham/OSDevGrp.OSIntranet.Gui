using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Gui.App.Core;

namespace OSDevGrp.OSIntranet.Gui.App.Features
{
    internal abstract class EventHandlerFeatureBase<TEvent> : BackgroundFeatureBase, IEventHandler<TEvent> where TEvent : IEvent
    {
        #region Private variables

        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<EventHandlerFeatureBase<TEvent>> _logger;
        private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

        #endregion

        #region Constructor

        protected EventHandlerFeatureBase(IEventPublisher eventPublisher, ILogger<EventHandlerFeatureBase<TEvent>> logger)
        {
            NullGuard.NotNull(eventPublisher, nameof(eventPublisher))
                .NotNull(logger, nameof(logger));

            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        #endregion

        #region Methods

        public async Task HandleAsync(TEvent e)
        {
            NullGuard.NotNull(e, nameof(e));

            await _semaphoreSlim.WaitAsync();
            try
            {
                await OnHandleAsync(e);
            }
            catch (Exception ex)
            {
                await ex.HandleAsync(_logger);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public sealed override Task StartAsync()
        {
            _eventPublisher.AddSubscriber(this);

            return Task.CompletedTask;
        }

        public sealed override Task StopAsync()
        {
            _eventPublisher.RemoveSubscriber(this);

            return Task.CompletedTask;
        }

        protected abstract Task OnHandleAsync(TEvent e);

        protected override void OnDispose()
        {
            _eventPublisher.RemoveSubscriber(this);

            _semaphoreSlim.Dispose();
        }

        #endregion
    }
}