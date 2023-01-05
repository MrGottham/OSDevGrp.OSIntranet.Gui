using System.ComponentModel;
using System.Windows.Input;

namespace OSDevGrp.OSIntranet.Gui.App.Core
{
    public interface IAsyncRelayCommand : ICommand, INotifyPropertyChanged, IDisposable
    {
        bool CanBeCancelled { get; }

        bool IsRunning { get; }

        Task ExecutionTask { get; }

        bool IsCancellationRequested { get; }

        void Cancel();
    }
}