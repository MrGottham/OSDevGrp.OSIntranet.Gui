using System;
using System.ComponentModel;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring
{
    /// <summary>
    /// ViewModel for et regnskab.
    /// </summary>
    public class RegnskabViewModel : ViewModelBase, IRegnskabViewModel
    {
        #region Private variables

        private readonly IRegnskabModel _regnskabModel;

        #endregion

        #region Constructor
        
        /// <summary>
        /// Danner ViewModel for et regnskab.
        /// </summary>
        /// <param name="regnskabModel">Model for et regnskab.</param>
        public RegnskabViewModel(IRegnskabModel regnskabModel)
        {
            if (regnskabModel == null)
            {
                throw new ArgumentNullException("regnskabModel");
            }
            _regnskabModel = regnskabModel;
            _regnskabModel.PropertyChanged += PropertyChangedOnRegnskabModelEventHandler;
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// Regnskabsnummer.
        /// </summary>
        public virtual int Nummer
        {
            get
            {
                return _regnskabModel.Nummer;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Navn
        {
            get
            {
                return _regnskabModel.Navn;
            }
            set
            {
                _regnskabModel.Navn = value;
            }
        }

        /// <summary>
        /// Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return Navn;
            }
        }

        #endregion
        
        #region Methods

        /// <summary>
        /// Eventhandler, der kaldes, når en property ændres på modellen for regnskabet.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="e">Argumenter til eventet.</param>
        private void PropertyChangedOnRegnskabModelEventHandler(object sender, PropertyChangedEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
            RaisePropertyChanged(e.PropertyName);
        }

        #endregion
    }
}
