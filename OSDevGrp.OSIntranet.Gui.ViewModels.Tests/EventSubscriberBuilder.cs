using System;
using AutoFixture.Kernel;
using Moq;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Events;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests
{
    internal static class EventSubscriberBuilder
    {
        internal static IEventSubscriber<TEventArgs> BuildEventSubscriber<TEventArgs>(this ISpecimenBuilder fixture, Action<TEventArgs> onEventCallback = null) where TEventArgs : IIntranetGuiEventArgs
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildEventSubscriberMock(onEventCallback).Object;
        }

        internal static Mock<IEventSubscriber<TEventArgs>> BuildEventSubscriberMock<TEventArgs>(this ISpecimenBuilder fixture, Action<TEventArgs> onEventCallback = null) where TEventArgs : IIntranetGuiEventArgs
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Mock<IEventSubscriber<TEventArgs>> mock = new Mock<IEventSubscriber<TEventArgs>>();
            mock.Setup(m => m.OnEvent(It.IsAny<TEventArgs>()))
                .Callback<TEventArgs>(eventArgs =>
                {
                    if (onEventCallback == null)
                    {
                        return;
                    }

                    onEventCallback(eventArgs);
                });
            return mock;
        }
    }
}