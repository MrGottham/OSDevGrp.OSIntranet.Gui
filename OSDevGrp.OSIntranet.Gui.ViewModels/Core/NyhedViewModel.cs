using System;
using System.ComponentModel;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Core
{
    /// <summary>
    /// ViewModel for en nyhed.
    /// </summary>
    public class NyhedViewModel : ViewModelBase, INyhedViewModel
    {
        #region Private variables

        private readonly INyhedModel _nyhedModel;
        private readonly byte[] _image;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner ViewModel for en nyhed.
        /// </summary>
        /// <param name="nyhedModel">Model for nyheden.</param>
        /// <param name="image">Billede, der illustrerer nyheden.</param>
        public NyhedViewModel(INyhedModel nyhedModel, byte[] image)
        {
            if (nyhedModel == null)
            {
                throw new ArgumentNullException("nyhedModel");
            }
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }
            _nyhedModel = nyhedModel;
            _nyhedModel.PropertyChanged += PropertyChangedOnNyhedModelEventHandler;
            _image = image;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Nyhedsaktualitet.
        /// </summary>
        public virtual Nyhedsaktualitet Nyhedsaktualitet
        {
            get
            {
                return _nyhedModel.Nyhedsaktualitet;
            }
        }

        /// <summary>
        /// Udgivelsestidspunkt for nyheden.
        /// </summary>
        public virtual DateTime Nyhedsudgivelsestidspunkt
        {
            get
            {
                return _nyhedModel.Nyhedsudgivelsestidspunkt;
            }
        }

        /// <summary>
        /// Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return Resource.GetText(Text.News);
            }
        }

        /// <summary>
        /// Billede, der illustrerer nyheden.
        /// </summary>
        public virtual byte[] Image
        {
            get
            {
                return _image;
            }
        }

        /// <summary>
        /// Detaljeret nyhedsinformation.
        /// </summary>
        public virtual string Nyhedsinformation
        {
            get
            {
                return _nyhedModel.Nyhedsinformation;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Eventhandler, der kaldes, når en property ændres på modellen for nyheden.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void PropertyChangedOnNyhedModelEventHandler(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }
            RaisePropertyChanged(eventArgs.PropertyName);
        }

        #endregion
    }
}
