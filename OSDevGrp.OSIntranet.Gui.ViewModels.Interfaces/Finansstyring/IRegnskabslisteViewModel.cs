using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en ViewModel for en liste indeholdende regnskaber.
    /// </summary>
    public interface IRegnskabslisteViewModel : IViewModel, IRefreshable
    {
        /// <summary>
        /// Statusdato for listen af regnskaber.
        /// </summary>
        DateTime StatusDato
        {
            get; 
            set;
        }

        /// <summary>
        /// Regnskaber.
        /// </summary>
        IEnumerable<IRegnskabViewModel> Regnskaber
        {
            get;
        }

        /// <summary>
        /// Tilføjer et regnskab til listen af regnskaber.
        /// </summary>
        /// <param name="regnskab">ViewModel for regnskabet, der skal tilføjes listen af regnskaber.</param>
        void RegnskabAdd(IRegnskabViewModel regnskab);
    }
}
