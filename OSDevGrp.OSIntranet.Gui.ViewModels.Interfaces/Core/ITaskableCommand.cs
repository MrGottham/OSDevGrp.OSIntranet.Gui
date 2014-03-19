using System.Threading.Tasks;
using System.Windows.Input;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core
{
    /// <summary>
    /// Interface til en kommando, der gør brug af Tasks.
    /// </summary>
    public interface ITaskableCommand : ICommand
    {
        /// <summary>
        /// Task, som kommandoen gør brug af.
        /// </summary>
        Task ExecuteTask
        {
            get;
        }
    }
}
