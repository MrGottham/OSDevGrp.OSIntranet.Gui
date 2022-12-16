using Microsoft.Extensions.DependencyInjection;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Tests
{
    internal class TestEventHandler<TEvent> : IEventHandler<TEvent> where TEvent : class, IEvent
    {
        #region Private variabels

        private readonly IEventPublisher _eventPublisher;
        private readonly IList<TEvent> _handledEvents = new List<TEvent>();
        private readonly object _syncRoot = new();

        #endregion

        #region Constructors

        public TestEventHandler(IServiceProvider serviceProvider)
            : this(serviceProvider.GetRequiredService<IEventPublisher>())
        {
        }

        public TestEventHandler(IEventPublisher eventPublisher)
        {
            NullGuard.NotNull(eventPublisher, nameof(eventPublisher));

            _eventPublisher = eventPublisher;
            _eventPublisher.AddSubscriber(this);
        }

        #endregion

        #region Properties

        public IEnumerable<TEvent> HandledEvents
        {
            get
            {
                lock (_syncRoot)
                {
                    return _handledEvents;
                }
            }
        }

        public int NumberOfHandledEvents => HandledEvents.Count();

        #endregion

        #region Methods

        public void Dispose()
        {
            _eventPublisher.RemoveSubscriber(this);

            lock (_syncRoot)
            {
                _handledEvents.Clear();
            }
        }

        public Task HandleAsync(TEvent e)
        {
            NullGuard.NotNull(e, nameof(e));

            lock (_syncRoot)
            {
                _handledEvents.Add(e);
            }

            return Task.CompletedTask;
        }

        #endregion
    }
}