using System;
using System.Windows.Input;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring
{
    /// <summary>
    /// ViewModel til en konto.
    /// </summary>
    public class KontoViewModel : KontoViewModelBase<IKontoModel, IKontogruppeViewModel>, IKontoViewModel
    {
        #region Private variables

        private ICommand _refreshCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en ViewModel til en konto.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, som kontoen er tilknyttet.</param>
        /// <param name="kontoModel">Model for kontoen.</param>
        /// <param name="kontogruppeViewModel">ViewModel for kontogruppen.</param>
        /// <param name="finansstyringRepository">Implementering af repositoryet til finansstyring.</param>
        /// <param name="exceptionHandlerViewModel">Implementering af ViewModel for exceptionhandleren.</param>
        public KontoViewModel(IRegnskabViewModel regnskabViewModel, IKontoModel kontoModel, IKontogruppeViewModel kontogruppeViewModel, IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
            : base(regnskabViewModel, kontoModel, kontogruppeViewModel, Resource.GetText(Text.Account), Resource.GetEmbeddedResource("Images.Konto.png"), finansstyringRepository, exceptionHandlerViewModel)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Kreditbeløb.
        /// </summary>
        public virtual decimal Kredit
        {
            get
            {
                return Model.Kredit;
            }
            set
            {
                try
                {
                    Model.Kredit = value;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Tekstangivelse af kreditbeløb.
        /// </summary>
        public virtual string KreditAsText
        {
            get
            {
                return Kredit.ToString("C");
            }
        }

        /// <summary>
        /// Label til kreditbeløb.
        /// </summary>
        public virtual string KreditLabel
        {
            get
            {
                return Resource.GetText(Text.Credit);
            }
        }

        /// <summary>
        /// Saldo.
        /// </summary>
        public virtual decimal Saldo
        {
            get
            {
                return Model.Saldo;
            }
            set
            {
                try
                {
                    Model.Saldo = value;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Tekstangivelse af saldo.
        /// </summary>
        public virtual string SaldoAsText
        {
            get
            {
                return Saldo.ToString("C");
            }
        }

        /// <summary>
        /// Label til saldo.
        /// </summary>
        public virtual string SaldoLabel
        {
            get
            {
                return Resource.GetText(Text.Balance);
            }
        }

        /// <summary>
        /// Disponibel beløb.
        /// </summary>
        public virtual decimal Disponibel
        {
            get
            {
                return Model.Disponibel;
            }
        }

        /// <summary>
        /// Tekstangivelse af disponibel beløb.
        /// </summary>
        public virtual string DisponibelAsText
        {
            get
            {
                return Disponibel.ToString("C");
            }
        }

        /// <summary>
        /// Label til disponibel beløb.
        /// </summary>
        public virtual string DisponibelLabel
        {
            get
            {
                return Resource.GetText(Text.Available);
            }
        }

        /// <summary>
        /// Kontoens værdi pr. opgørelsestidspunktet.
        /// </summary>
        public override decimal Kontoværdi
        {
            get
            {
                return Disponibel;
            }
        }

        /// <summary>
        /// Kommando til genindlæsning og opdatering af kontoen.
        /// </summary>
        public override ICommand RefreshCommand
        {
            get
            {
                return _refreshCommand ?? (_refreshCommand = new KontoGetCommand(null, FinansstyringRepository, ExceptionHandler));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Metode, der kaldes, når en property på modellen for kontoen ændres.
        /// </summary>
        /// <param name="propertyName">Navn på den ændrede property.</param>
        protected override void ModelChanged(string propertyName)
        {
            switch (propertyName)
            {
                case "Kredit":
                    RaisePropertyChanged("KreditAsText");
                    break;

                case "Saldo":
                    RaisePropertyChanged("SaldoAsText");
                    break;

                case "Disponibel":
                    RaisePropertyChanged("DisponibelAsText");
                    RaisePropertyChanged("Kontoværdi");
                    break;
            }
        }

        #endregion
    }
}
