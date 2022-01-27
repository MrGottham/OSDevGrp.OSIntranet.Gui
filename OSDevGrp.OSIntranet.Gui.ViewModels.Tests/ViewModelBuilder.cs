using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using Moq;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests
{
    internal static class ViewModelBuilder
    {
        internal static IRegnskabViewModel BuildRegnskabViewModel(this ISpecimenBuilder fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return BuildRegnskabViewModelMock(fixture).Object;
        }

        internal static Mock<IRegnskabViewModel> BuildRegnskabViewModelMock(this ISpecimenBuilder fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return new Mock<IRegnskabViewModel>();
        }

        internal static IReadOnlyBogføringslinjeViewModel BuildReadOnlyBogføringslinjeViewModel(this ISpecimenBuilder fixture, IRegnskabViewModel regnskabViewModel = null, int? løbenummer = null, DateTime? dato = null, bool hasBilag = true, string bilag = null, string kontonummer = null, string tekst = null, bool hasBudgetkontonummer = true, string budgetkontonummer = null, decimal? debit = null, decimal? kredit = null, bool hasAdressekonto = true, int? adressekonto = null, string displayName = null, byte[] image = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return BuildReadOnlyBogføringslinjeViewModelMock(fixture, regnskabViewModel, løbenummer, dato, hasBilag, bilag, kontonummer, tekst, hasBudgetkontonummer, budgetkontonummer, debit, kredit, hasAdressekonto, adressekonto, displayName, image).Object;
        }

        internal static Mock<IReadOnlyBogføringslinjeViewModel> BuildReadOnlyBogføringslinjeViewModelMock(this ISpecimenBuilder fixture, IRegnskabViewModel regnskabViewModel = null, int? løbenummer = null, DateTime? dato = null, bool hasBilag = true, string bilag = null, string kontonummer = null, string tekst = null, bool hasBudgetkontonummer = true, string budgetkontonummer = null, decimal? debit = null, decimal? kredit = null, bool hasAdressekonto = true, int? adressekonto = null, string displayName = null, byte[] image = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Random random = new Random(fixture.Create<int>());

            if (debit.HasValue == false)
            {
                debit = kredit.HasValue ? null : random.Next(100) > 50 ? Math.Abs(fixture.Create<decimal>()) : (decimal?)null;
            }

            if (kredit.HasValue == false)
            {
                kredit = debit.HasValue ? null : (decimal?)Math.Abs(fixture.Create<decimal>());
            }

            Mock<IReadOnlyBogføringslinjeViewModel> mock = new Mock<IReadOnlyBogføringslinjeViewModel>();
            mock.Setup(m => m.Regnskab)
                .Returns(regnskabViewModel ?? fixture.BuildRegnskabViewModel());
            mock.Setup(m => m.Løbenummer)
                .Returns(løbenummer ?? fixture.Create<int>());
            mock.Setup(m => m.Dato)
                .Returns((dato ?? DateTime.Today.AddDays(random.Next(0, 365) * -1)).Date);
            mock.Setup(m => m.Bilag)
                .Returns(hasBilag ? bilag ?? fixture.Create<string>() : null);
            mock.Setup(m => m.Kontonummer)
                .Returns(kontonummer ?? fixture.Create<string>());
            mock.Setup(m => m.Tekst)
                .Returns(tekst ?? fixture.Create<string>());
            mock.Setup(m => m.Budgetkontonummer)
                .Returns(budgetkontonummer ?? fixture.Create<string>());
            mock.Setup(m => m.Debit)
                .Returns(debit ?? 0M);
            mock.Setup(m => m.DebitAsText)
                .Returns((debit ?? 0M).ToString("C"));
            mock.Setup(m => m.Kredit)
                .Returns(kredit ?? 0M);
            mock.Setup(m => m.KreditAsText)
                .Returns((kredit ?? 0M).ToString("C"));
            mock.Setup(m => m.Bogført)
                .Returns((debit ?? 0M) - (kredit ?? 0M));
            mock.Setup(m => m.BogførtAsText)
                .Returns(((debit ?? 0M) - (kredit ?? 0M)).ToString("C"));
            mock.Setup(m => m.Adressekonto)
                .Returns(hasAdressekonto ? adressekonto ?? fixture.Create<int>() : 0);
            mock.Setup(m => m.DisplayName)
                .Returns(displayName ?? fixture.Create<string>());
            mock.Setup(m => m.Image)
                .Returns(image ?? fixture.CreateMany<byte>(random.Next(256, 512)).ToArray());
            return mock;
        }

        internal static IExceptionHandlerViewModel BuildExceptionHandlerViewModel(this ISpecimenBuilder fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return BuildExceptionHandlerViewModelMock(fixture).Object;
        }

        internal static Mock<IExceptionHandlerViewModel> BuildExceptionHandlerViewModelMock(this ISpecimenBuilder fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return new Mock<IExceptionHandlerViewModel>();
        }
    }
}