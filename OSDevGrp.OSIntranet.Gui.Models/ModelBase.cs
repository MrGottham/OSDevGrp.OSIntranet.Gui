using System;
using System.ComponentModel;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces;

namespace OSDevGrp.OSIntranet.Gui.Models
{
    /// <summary>
    /// Basisfunktionalitet for en model i OS Intranet.
    /// </summary>
    public abstract class ModelBase : IModel
    {
        #region Events

        public virtual event PropertyChangedEventHandler PropertyChanged;

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
