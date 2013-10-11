﻿using System;
using System.Linq;
using System.Threading;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands
{
    /// <summary>
    /// Kommando til genindlæsning af regnskabslisten.
    /// </summary>
    public class RegnskabslisteRefreshCommand : ViewModelCommandBase<IRegnskabslisteViewModel>
    {
        #region Private variables

        private bool _isBusy;
        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly SynchronizationContext _synchronizationContext;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Danner kommando til genindlæsning af regnskabslisten.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af en ViewModel til en exceptionhandler.</param>
        public RegnskabslisteRefreshCommand(IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
            : base(exceptionHandlerViewModel)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            _finansstyringRepository = finansstyringRepository;
            _synchronizationContext = SynchronizationContext.Current;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returnerer angivelse af, om kommandoen kan udføres på den givne ViewModel.
        /// </summary>
        /// <param name="regnskabslisteViewModel">ViewModel, hvorpå kommandoen skal udføres.</param>
        /// <returns>Angivelse af, om kommandoen kan udføres på den givne ViewModel.</returns>
        protected override bool CanExecute(IRegnskabslisteViewModel regnskabslisteViewModel)
        {
            return _isBusy == false;
        }

        /// <summary>
        /// Genindlæser regnskaber i regnskabslisten.
        /// </summary>
        /// <param name="regnskabslisteViewModel">ViewModel for en liste af regnskaber.</param>
        protected override void Execute(IRegnskabslisteViewModel regnskabslisteViewModel)
        {
            _isBusy = true;
            var task = _finansstyringRepository.RegnskabslisteGetAsync();
            task.ContinueWith(t =>
                {
                    try
                    {
                        if (t.IsCanceled || t.IsFaulted)
                        {
                            if (t.Exception != null)
                            {
                                t.Exception.Handle(exception =>
                                    {
                                        HandleException(exception);
                                        return true;
                                    });
                            }
                            return;
                        }
                        foreach (var regnskabModel in t.Result)
                        {
                            var regnskabViewModel = regnskabslisteViewModel.Regnskaber.SingleOrDefault(m => m.Nummer == regnskabModel.Nummer);
                            if (regnskabViewModel != null)
                            {
                                regnskabViewModel.Navn = regnskabModel.Navn;
                                continue;
                            }
                            if (_synchronizationContext == null)
                            {
                                regnskabslisteViewModel.RegnskabAdd(new RegnskabViewModel(regnskabModel));
                                continue;
                            }
                            _synchronizationContext.Post(obj => regnskabslisteViewModel.RegnskabAdd(new RegnskabViewModel(obj as IRegnskabModel)), regnskabModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        HandleException(ex);
                    }
                    finally
                    {
                        _isBusy = false;
                    }
                });
        }

        #endregion
    }
}
