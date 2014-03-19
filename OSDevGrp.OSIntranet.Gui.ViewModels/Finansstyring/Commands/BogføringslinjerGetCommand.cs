using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands
{
    /// <summary>
    /// Kommando, der kan hente bogføringslinjer til et regnskab.
    /// </summary>
    public class BogføringslinjerGetCommand : ViewModelCommandBase<IRegnskabViewModel>, ITaskableCommand
    {
        #region Private variables

        private bool _isBusy;
        private readonly IFinansstyringRepository _finansstyringRepository;

        #endregion
        
        #region Constructor

        /// <summary>
        /// Danner kommando, der kan hente bogføringslinjer til et regnskab.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af en ViewModel til en exceptionhandler.</param>
        public BogføringslinjerGetCommand(IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
            : base(exceptionHandlerViewModel)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            _finansstyringRepository = finansstyringRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returnerer angivelse af, om kommandoen kan udføres på den givne ViewModel.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel, hvorpå kommandoen skal udføres.</param>
        /// <returns>Angivelse af, om kommandoen kan udføres på den givne ViewModel.</returns>
        protected override bool CanExecute(IRegnskabViewModel regnskabViewModel)
        {
            return _isBusy == false;
        }

        /// <summary>
        /// Henter og indsætter bogføringslinjer til regnskabet.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for et regnskabet, hvortil bogføringslinjer skal hentes og indsætter.</param>
        protected override void Execute(IRegnskabViewModel regnskabViewModel)
        {
            _isBusy = true;
            var konfiguration = _finansstyringRepository.Konfiguration;
            var task = _finansstyringRepository.BogføringslinjerGetAsync(regnskabViewModel.Nummer, regnskabViewModel.StatusDato, konfiguration.AntalBogføringslinjer);
            ExecuteTask = task.ContinueWith(t =>
                {
                    try
                    {
                        var dageForNyheder = konfiguration.DageForNyheder;
                        HandleResultFromTask(t, regnskabViewModel, dageForNyheder, HandleResult);
                    }
                    finally
                    {
                        _isBusy = false;
                    }
                });
        }

        /// <summary>
        /// Opdaterer ViewModel for regnskabet med bogføringslinjer.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, der skal opdateres.</param>
        /// <param name="bogføringslinjeModels">Bogføringslinjer, som ViewModel for regnskabet skal opdateres med.</param>
        /// <param name="dageForNyheder">Antallet af dage, som nyheder er gældende.</param>
        private static void HandleResult(IRegnskabViewModel regnskabViewModel, IEnumerable<IBogføringslinjeModel> bogføringslinjeModels, int dageForNyheder)
        {
            if (regnskabViewModel == null)
            {
                throw new ArgumentNullException("regnskabViewModel");
            }
            if (bogføringslinjeModels == null)
            {
                throw new ArgumentNullException("bogføringslinjeModels");
            }
            foreach (var bogføringslinjeModel in bogføringslinjeModels)
            {
                if (regnskabViewModel.Bogføringslinjer.Any(m => m.Løbenummer == bogføringslinjeModel.Løbenummer))
                {
                    return;
                }
                var bogføringslinjeViewModel = new BogføringslinjeViewModel(regnskabViewModel, bogføringslinjeModel);
                regnskabViewModel.BogføringslinjeAdd(bogføringslinjeViewModel);
                if (bogføringslinjeModel.Dato.Date.CompareTo(regnskabViewModel.StatusDato.AddDays(dageForNyheder*-1).Date) >= 0 && bogføringslinjeModel.Dato.Date.CompareTo(regnskabViewModel.StatusDato.Date) <= 0)
                {
                    regnskabViewModel.NyhedAdd(new NyhedViewModel(bogføringslinjeModel, bogføringslinjeViewModel.Image));
                }
            }
        }

        #endregion
    }
}
