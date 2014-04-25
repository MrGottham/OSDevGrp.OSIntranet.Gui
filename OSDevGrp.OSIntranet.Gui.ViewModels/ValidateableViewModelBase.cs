using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces;

namespace OSDevGrp.OSIntranet.Gui.ViewModels
{
    /// <summary>
    /// Basisfunktionalitet for en validérbar ViewModel i OS Intranet.
    /// </summary>
    public abstract class ValidateableViewModelBase : ViewModelBase, IValidateableViewModel
    {
        #region Private variables

        private readonly IDictionary<string, string> _validationErrors = new Dictionary<string, string>();

        #endregion

        #region Methods

        /// <summary>
        /// Nulstiller alle valideringsfejl.
        /// </summary>
        public virtual void ClearValidationErrors()
        {
            while (_validationErrors.Count > 0)
            {
                _validationErrors.Clear();
            }
        }

        /// <summary>
        /// Returnerer valideringsfejlen til en given property.
        /// </summary>
        /// <param name="propertyName">Navn på property, hvortil valideringsfejlen skal returneres.</param>
        /// <returns>Valideringsfejl til en given property.</returns>
        protected virtual string GetValidationError(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }
            return _validationErrors.ContainsKey(propertyName) ? _validationErrors[propertyName] : string.Empty;
        }

        /// <summary>
        /// Sætter valideringsfejlen til en given property.
        /// </summary>
        /// <param name="propertyName">Navn på property, hvorpå valideringsfejl skal sættes.</param>
        /// <param name="validationError">Valideringsfejl.</param>
        /// <param name="raisePropertyName">Navn på den property, som returnerer valideringsfejlen.</param>
        protected virtual void SetValidationError(string propertyName, string validationError, string raisePropertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }
            if (string.IsNullOrWhiteSpace(raisePropertyName))
            {
                throw new ArgumentNullException("raisePropertyName");
            }
            try
            {
                if (string.IsNullOrEmpty(validationError))
                {
                    while (_validationErrors.ContainsKey(propertyName))
                    {
                        _validationErrors.Remove(propertyName);
                    }
                    return;
                }
                if (_validationErrors.ContainsKey(propertyName))
                {
                    _validationErrors[propertyName] = validationError;
                    return;
                }
                _validationErrors.Add(propertyName, validationError);
            }
            finally
            {
                RaisePropertyChanged(raisePropertyName);
            }
        }

        #endregion
    }
}
