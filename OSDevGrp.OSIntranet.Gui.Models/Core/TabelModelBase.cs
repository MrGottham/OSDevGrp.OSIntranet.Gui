using System;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Gui.Models.Core
{
    /// <summary>
    /// Model, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m.
    /// </summary>
    public abstract class TabelModelBase : ModelBase, ITabelModelBase
    {
        #region Private variables

        private readonly string _id;
        private string _tekst;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en model, der indeholder grundlæggende tabeloplysninger, såsom typer, grupper m.m.
        /// </summary>
        /// <param name="id">Unik identifikation af de grundlæggende tabeloplysningerne i denne model.</param>
        /// <param name="tekst">Teksten der beskriver de grundlæggende tabeloplysninger i denne model.</param>
        protected TabelModelBase(string id, string tekst)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }
            if (string.IsNullOrEmpty(tekst))
            {
                throw new ArgumentNullException("tekst");
            }
            _id = id;
            _tekst = tekst;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Unik identifikation af de grundlæggende tabeloplysningerne i denne model.
        /// </summary>
        public virtual string Id
        {
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// Teksten der beskriver de grundlæggende tabeloplysninger i denne model.
        /// </summary>
        public virtual string Tekst
        {
            get
            {
                return _tekst;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
                if (_tekst == value)
                {
                    return;
                }
                _tekst = value;
                RaisePropertyChanged("Tekst");
            }
        }

        #endregion
    }
}
