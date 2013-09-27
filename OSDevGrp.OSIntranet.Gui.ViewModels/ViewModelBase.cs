using System;
using System.ComponentModel;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;

namespace OSDevGrp.OSIntranet.Gui.ViewModels
{
    /// <summary>
    /// Basisfunktionalitet for en ViewModel i OS Intranet.
    /// </summary>
    public abstract class ViewModelBase : IViewModel
    {
        #region Events

        public virtual event PropertyChangedEventHandler PropertyChanged;

        #endregion
        
        #region Properties
        
        /// <summary>
        /// Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.
        /// </summary>
        public abstract string DisplayName
        {
            get;
        }
        
        #endregion
        
        #region Methods
        
        /// <summary>
        /// Rejser eventet, der notifiserer, at en property er blevet ændret.
        /// </summary>
        /// <param name="propertyName">Navnet på propertyen, der er blevet ændret.</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
        #endregion
    }
}
