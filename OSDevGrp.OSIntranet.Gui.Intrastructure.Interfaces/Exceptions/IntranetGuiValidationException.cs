﻿using System;

namespace OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions
{
    /// <summary>
    /// Valideringsexception fra OS Intranet.
    /// </summary>
    public class IntranetGuiValidationException : IntranetGuiBusinessException
    {
        #region Private variables

        private readonly object _validationContext;
        private readonly string _propertyName;
        private readonly object _value;

        #endregion

        #region Constructors

        /// <summary>
        /// Danner en valideringsexception fra OS Intranet.
        /// </summary>
        /// <param name="message">Fejlbesked.</param>
        /// <param name="validationContext">Instans af objektet, hvorpå validering er fejlet.</param>
        /// <param name="propertyName">Navn på property, hvor validering er fejlet.</param>
        /// <param name="value">Værdi, hvormed validering er fejlet.</param>
        public IntranetGuiValidationException(string message, object validationContext, string propertyName, object value)
            : this(message, validationContext, propertyName, value, null)
        {
        }

        /// <summary>
        /// Danner en valideringsexception fra OS Intranet.
        /// </summary>
        /// <param name="message">Fejlbesked.</param>
        /// <param name="validationContext">Instans af objektet, hvorpå validering er fejlet.</param>
        /// <param name="propertyName">Navn på property, hvor validering er fejlet.</param>
        /// <param name="value">Værdi, hvormed validering er fejlet.</param>
        /// <param name="innerException">Inner exception.</param>
        public IntranetGuiValidationException(string message, object validationContext, string propertyName, object value, Exception innerException)
            : base(message, innerException)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException("validationContext");
            }
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }
            _validationContext = validationContext;
            _propertyName = propertyName;
            _value = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returnerer instans af objektet, hvorpå validering er fejlet.
        /// </summary>
        public virtual object ValidationContext
        {
            get
            {
                return _validationContext;
            }
        }

        /// <summary>
        /// Returnerer navn på den property, hvor validering er fejlet.
        /// </summary>
        public virtual string PropertyName
        {
            get
            {
                return _propertyName;
            }
        }

        /// <summary>
        /// Returnerer værdi, hvormed validering er fejlet.
        /// </summary>
        public virtual object Value
        {
            get
            {
                return _value;
            }
        }

        #endregion
    }
}
