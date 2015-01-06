using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Data;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.Finansstyring.Core.Converters
{
    /// <summary>
    /// Converter, der kan konvertere et objekt til en ViewModel for et regnskab.
    /// </summary>
    public class ObjectToRegnskabViewModelConverter : IValueConverter
    {
        /// <summary>
        /// Konverterer et objekt til en ViewModel for et regnskab.
        /// </summary>
        /// <param name="value">Værdi, der skal konverteres.</param>
        /// <param name="targetType">Typen, der skal konverteres til.</param>
        /// <param name="parameter">Parameter til konverteringen.</param>
        /// <param name="language">Sprog.</param>
        /// <returns>Konverteret værdi.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return null;
            }
            var regnskabViewModel = value as IRegnskabModel;
            if (regnskabViewModel != null)
            {
                return regnskabViewModel;
            }
            var regnskabViewModelCollection = value as IEnumerable<IRegnskabViewModel>;
            if (regnskabViewModelCollection != null)
            {
                var regnskabViewModelArray = regnskabViewModelCollection.ToArray();
                return regnskabViewModelArray.Any() ? regnskabViewModelArray.First() : null;
            }
            var kontoViewModel = value as IKontoViewModel;
            if (kontoViewModel != null)
            {
                return kontoViewModel;
            }
            var kontoViewModelCollection = value as IEnumerable<IKontoViewModel>;
            if (kontoViewModelCollection != null)
            {
                var kontoViewModelArray = kontoViewModelCollection.ToArray();
                return kontoViewModelArray.Any() ? kontoViewModelArray.First().Regnskab : null;
            }
            var budgetkontoViewModel = value as IBudgetkontoModel;
            if (budgetkontoViewModel != null)
            {
                return budgetkontoViewModel;
            }
            var budgetkontoViewModelCollection = value as IEnumerable<IBudgetkontoViewModel>;
            if (budgetkontoViewModelCollection != null)
            {
                var budgetkontoViewModelArray = budgetkontoViewModelCollection.ToArray();
                return budgetkontoViewModelArray.Any() ? budgetkontoViewModelArray.First().Regnskab : null;
            }
            var balanceViewModel = value as IBalanceViewModel;
            if (balanceViewModel != null)
            {
                return balanceViewModel.Regnskab;
            }
            var balanceModelCollection = value as IEnumerable<IBalanceViewModel>;
            if (balanceModelCollection != null)
            {
                var balanceViewModelArray = balanceModelCollection.ToArray();
                return balanceViewModelArray.Any() ? balanceViewModelArray.First().Regnskab : null;
            }
            var opgørelseViewModel = value as IOpgørelseViewModel;
            if (opgørelseViewModel != null)
            {
                return opgørelseViewModel.Regnskab;
            }
            var opgørelseModelCollection = value as IEnumerable<IOpgørelseViewModel>;
            if (opgørelseModelCollection != null)
            {
                var opgørelseViewModelArray = opgørelseModelCollection.ToArray();
                return opgørelseViewModelArray.Any() ? opgørelseViewModelArray.First().Regnskab : null;
            }
            return null;
        }

        /// <summary>
        /// Konverterer en ViewModel for et regnskab til et objekt.
        /// </summary>
        /// <param name="value">Værdi, der skal konverteres.</param>
        /// <param name="targetType">Typen, der skal konverteres til.</param>
        /// <param name="parameter">Parameter til konverteringen.</param>
        /// <param name="language">Sprog.</param>
        /// <returns>Konverteret værdi.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
