using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring
{
    /// <summary>
    /// ViewModel, der indeholder en advarsel ved en bogføring.
    /// </summary>
    public class BogføringsadvarselViewModel : ViewModelBase, IBogføringsadvarselViewModel
    {
        #region Private variables

        private ICommand _removeCommand;
        private readonly IRegnskabViewModel _regnskabViewModel;
        private readonly IReadOnlyBogføringslinjeViewModel _bogføringslinjeViewModel;
        private readonly IBogføringsadvarselModel _bogføringsadvarselModel;
        private readonly DateTime _tidspunkt;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en ViewModel, der indeholder en advarsel ved en bogføring.
        /// </summary>
        /// <param name="regnskabViewModel">ViewModel for regnskabet, som advarslen skal være tilknyttet.</param>
        /// <param name="bogføringslinjeViewModel">ViewModel for bogføringslinjen, der har medført advarslen.</param>
        /// <param name="bogføringsadvarselModel">Model for advarslen, som er opstået ved bogføring.</param>
        /// <param name="tidspunkt">Tidspunkt for advarslen, som er opstået ved bogføring.</param>
        public BogføringsadvarselViewModel(IRegnskabViewModel regnskabViewModel, IReadOnlyBogføringslinjeViewModel bogføringslinjeViewModel, IBogføringsadvarselModel bogføringsadvarselModel, DateTime tidspunkt)
        {
            if (regnskabViewModel == null)
            {
                throw new ArgumentNullException("regnskabViewModel");
            }
            if (bogføringslinjeViewModel == null)
            {
                throw new ArgumentNullException("bogføringslinjeViewModel");
            }
            if (bogføringsadvarselModel == null)
            {
                throw new ArgumentNullException("bogføringsadvarselModel");
            }
            _regnskabViewModel = regnskabViewModel;
            _bogføringslinjeViewModel = bogføringslinjeViewModel;
            _bogføringslinjeViewModel.PropertyChanged += PropertyChangedOnBogføringslinjeViewModelEventHandler;
            _bogføringsadvarselModel = bogføringsadvarselModel;
            _bogføringsadvarselModel.PropertyChanged += PropertyChangedOnBogføringsadvarselModelEventHandler;
            _tidspunkt = tidspunkt;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Regnskabet, som adressekontoen er tilknyttet.
        /// </summary>
        public virtual IRegnskabViewModel Regnskab
        {
            get
            {
                return _regnskabViewModel;
            }
        }

        /// <summary>
        /// Bogføringslinjen, der har medført advarslen.
        /// </summary>
        public virtual IReadOnlyBogføringslinjeViewModel Bogføringslinje
        {
            get
            {
                return _bogføringslinjeViewModel;
            }
        }

        /// <summary>
        /// Tidspunkt for advarslen, som er opstået ved bogføring.
        /// </summary>
        public virtual DateTime Tidspunkt
        {
            get
            {
                return _tidspunkt;
            }
        }

        /// <summary>
        /// Tekstangivelse af tidspunktet for advarslen, som er opstået ved bogføring.
        /// </summary>
        public virtual string TidspunktAsText
        {
            get
            {
                return string.Format("{0} {1}", Tidspunkt.ToString("d"), Tidspunkt.ToString("t"));
            }
        }

        /// <summary>
        /// Tekstangivelse af advarslen, som er opstået ved bogføring.
        /// </summary>
        public virtual string Advarsel
        {
            get
            {
                return _bogføringsadvarselModel.Advarsel;
            }
        }

        /// <summary>
        /// Kontonummer på kontoen, hvorpå advarslen er opstået ved bogføring.
        /// </summary>
        public virtual string Kontonummer
        {
            get
            {
                return _bogføringsadvarselModel.Kontonummer;
            }
        }

        /// <summary>
        /// Kontonavnet på kontoen, hvorpå advarslen er opstået ved bogføring.
        /// </summary>
        public virtual string Kontonavn
        {
            get
            {
                return _bogføringsadvarselModel.Kontonavn;
            }
        }

        /// <summary>
        /// Beløbet, der har medført advarslen.
        /// </summary>
        public virtual decimal Beløb
        {
            get
            {
                return Math.Abs(_bogføringsadvarselModel.Beløb);
            }
        }

        /// <summary>
        /// Tekstangivelse af beløbet, der har medført advarslen.
        /// </summary>
        public virtual string BeløbAsText
        {
            get
            {
                return Beløb.ToString("C");
            }
        }

        /// <summary>
        /// Samlet information for advarslen.
        /// </summary>
        public virtual string Information
        {
            get
            {
                var informationBuilder = new StringBuilder(TidspunktAsText);
                informationBuilder.AppendFormat(" {0}", Beløb == 0M ? Resource.GetText(Text.AccountOverdrawnedWithoutValue, Kontonavn) : Resource.GetText(Text.AccountOverdrawnedWithValue, Kontonavn, BeløbAsText));
                informationBuilder.AppendLine();
                informationBuilder.AppendLine();
                informationBuilder.AppendFormat("{0}: {1} {2}", Resource.GetText(Text.Cause), _bogføringslinjeViewModel.Tekst, _bogføringslinjeViewModel.BogførtAsText);
                return informationBuilder.ToString();
            }
        }

        /// <summary>
        /// Kommando, der fjerner advarslen fra regnskabet, hvorpå den er tilknyttet.
        /// </summary>
        public virtual ICommand RemoveCommand
        {
            get
            {
                if (_removeCommand == null)
                {
                    Action<object> action = obj =>
                        {
                            _bogføringslinjeViewModel.PropertyChanged -= PropertyChangedOnBogføringslinjeViewModelEventHandler;
                            _bogføringsadvarselModel.PropertyChanged -= PropertyChangedOnBogføringsadvarselModelEventHandler;
                        };
                    _removeCommand = new RelayCommand(action);
                }
                return _removeCommand;
            }
        }

        /// <summary>
        /// Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return Resource.GetText(Text.PostingWarning);
            }
        }

        /// <summary>
        /// Billede, der illustrerer en advarsel ved en bogføring.
        /// </summary>
        public virtual byte[] Image
        {
            get
            {
                return Resource.GetEmbeddedResource("Images.Bogføringslinje.png");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Eventhandler, der kaldes, når en property ændres på ViewModel for bogføringslinjen, der har medført advarslen.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void PropertyChangedOnBogføringslinjeViewModelEventHandler(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }
            switch (eventArgs.PropertyName)
            {
                case "Tekst":
                    RaisePropertyChanged("Bogføringslinje");
                    RaisePropertyChanged("Information");
                    break;

                case "BogførtAsText":
                    RaisePropertyChanged("Bogføringslinje");
                    RaisePropertyChanged("Information");
                    break;

                default:
                    RaisePropertyChanged("Bogføringslinje");
                    break;
            }
        }

        /// <summary>
        /// Eventhandler, der kaldes, når en property ændres på modellen for advarslen, som er opstået ved bogføring.
        /// </summary>
        /// <param name="sender">Objekt, der rejser eventet.</param>
        /// <param name="eventArgs">Argumenter til eventet.</param>
        private void PropertyChangedOnBogføringsadvarselModelEventHandler(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }
            switch (eventArgs.PropertyName)
            {
                case "Kontonavn":
                    RaisePropertyChanged(eventArgs.PropertyName);
                    RaisePropertyChanged("Information");
                    break;

                case "Beløb":
                    RaisePropertyChanged(eventArgs.PropertyName);
                    RaisePropertyChanged("BeløbAsText");
                    RaisePropertyChanged("Information");
                    break;

                default:
                    RaisePropertyChanged(eventArgs.PropertyName);
                    break;
            }
        }

        #endregion
    }
}
