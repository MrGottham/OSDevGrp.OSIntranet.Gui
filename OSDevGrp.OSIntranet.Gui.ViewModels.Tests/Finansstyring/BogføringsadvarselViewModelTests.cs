using System;
using System.ComponentModel;
using System.Windows.Input;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Text = OSDevGrp.OSIntranet.Gui.Resources.Text;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring
{
    /// <summary>
    /// Tester ViewModel, der indeholder en advarsel ved en bogføring.
    /// </summary>
    [TestFixture]
    public class BogføringsadvarselViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel, der indeholder en advarsel ved en bogføring.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBogføringsadvarselViewModel()
        {
            Fixture fixture = new Fixture();

            IRegnskabViewModel regnskabViewModelMock = fixture.BuildRegnskabViewModel();
            IReadOnlyBogføringslinjeViewModel bogføringslinjeViewModelMock = fixture.BuildReadOnlyBogføringslinjeViewModel();
            string advarsel = fixture.Create<string>();
            string kontonummer = fixture.Create<string>();
            string kontonavn = fixture.Create<string>();
            decimal beløb = fixture.Create<decimal>() * 1;
            Mock<IBogføringsadvarselModel> bogføringsadvarselModelMock = fixture.BuildBogføringsadvarselModelMock(advarsel, kontonummer, kontonavn, beløb);
            DateTime tidspunkt = DateTime.Now;
            IBogføringsadvarselViewModel bogføringsadvarselViewModel = new BogføringsadvarselViewModel(regnskabViewModelMock, bogføringslinjeViewModelMock, bogføringsadvarselModelMock.Object, tidspunkt);
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.Regnskab, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.Regnskab, Is.EqualTo(regnskabViewModelMock));
            Assert.That(bogføringsadvarselViewModel.Bogføringslinje, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.Bogføringslinje, Is.EqualTo(bogføringslinjeViewModelMock));
            Assert.That(bogføringsadvarselViewModel.Tidspunkt, Is.EqualTo(tidspunkt));
            Assert.That(bogføringsadvarselViewModel.TidspunktAsText, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.TidspunktAsText, Is.Not.Empty);
            Assert.That(bogføringsadvarselViewModel.TidspunktAsText, Is.EqualTo(string.Format("{0} {1}", tidspunkt.ToShortDateString(), tidspunkt.ToShortTimeString())));
            Assert.That(bogføringsadvarselViewModel.Advarsel, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.Advarsel, Is.Not.Empty);
            Assert.That(bogføringsadvarselViewModel.Advarsel, Is.EqualTo(advarsel));
            Assert.That(bogføringsadvarselViewModel.Kontonummer, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.Kontonummer, Is.Not.Empty);
            Assert.That(bogføringsadvarselViewModel.Kontonummer, Is.EqualTo(kontonummer));
            Assert.That(bogføringsadvarselViewModel.Kontonavn, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.Kontonavn, Is.Not.Empty);
            Assert.That(bogføringsadvarselViewModel.Kontonavn, Is.EqualTo(kontonavn));
            Assert.That(bogføringsadvarselViewModel.Beløb, Is.GreaterThan(0M));
            Assert.That(bogføringsadvarselViewModel.Beløb, Is.EqualTo(Math.Abs(beløb)));
            Assert.That(bogføringsadvarselViewModel.BeløbAsText, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.BeløbAsText, Is.Not.Empty);
            Assert.That(bogføringsadvarselViewModel.BeløbAsText, Is.EqualTo(Math.Abs(beløb).ToString("C")));
            Assert.That(bogføringsadvarselViewModel.Information, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.Information, Is.Not.Empty);
            Assert.That(bogføringsadvarselViewModel.RemoveCommand, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.RemoveCommand, Is.TypeOf<RelayCommand>());
            Assert.That(bogføringsadvarselViewModel.RemoveCommandLabel, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.RemoveCommandLabel, Is.Not.Empty);
            Assert.That(bogføringsadvarselViewModel.RemoveCommandLabel, Is.EqualTo(Resource.GetText(Text.Ignore)));
            Assert.That(bogføringsadvarselViewModel.DisplayName, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.DisplayName, Is.Not.Empty);
            Assert.That(bogføringsadvarselViewModel.DisplayName, Is.EqualTo(Resource.GetText(Text.PostingWarning)));
            Assert.That(bogføringsadvarselViewModel.Image, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.Image, Is.Not.Empty);
            Assert.That(bogføringsadvarselViewModel.Image, Is.EqualTo(Resource.GetEmbeddedResource("Images.Bogføringslinje.png")));

            bogføringsadvarselModelMock.Verify(m => m.Advarsel, Times.Once);
            bogføringsadvarselModelMock.Verify(m => m.Kontonummer, Times.Once);
            bogføringsadvarselModelMock.Verify(m => m.Kontonavn, Times.Once);
            bogføringsadvarselModelMock.Verify(m => m.Beløb, Times.Once);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for regnskabet er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisRegnskabViewModelErNull()
        {
            Fixture fixture = new Fixture();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new BogføringsadvarselViewModel(null, fixture.BuildReadOnlyBogføringslinjeViewModel(), fixture.BuildBogføringsadvarselModel(), DateTime.Now));
            // ReSharper restore ObjectCreationAsStatement
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("regnskabViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel  for bogføringslinjen, der har medført advarslen, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisBogføringslinjeViewModelErNull()
        {
            Fixture fixture = new Fixture();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new BogføringsadvarselViewModel(fixture.BuildRegnskabViewModel(), null, fixture.BuildBogføringsadvarselModel(), DateTime.Now));
            // ReSharper restore ObjectCreationAsStatement
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("bogføringslinjeViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis modellen for advarslen, der er opstået ved bogføring, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisBogføringsadvarselModelErNull()
        {
            Fixture fixture = new Fixture();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new BogføringsadvarselViewModel(fixture.BuildRegnskabViewModel(), fixture.BuildReadOnlyBogføringslinjeViewModel(), null, DateTime.Now));
            // ReSharper restore ObjectCreationAsStatement
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("bogføringsadvarselModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at getteren til Information returnerer information for advarlsen, hvor beløbet ikke er 0.
        /// </summary>
        [Test]
        public void TestAtInformationGetterReturnererInformationHvorBeløbIkkeErNul()
        {
            Fixture fixture = new Fixture();

            string tekst = fixture.Create<string>();
            decimal debit = Math.Abs(fixture.Create<decimal>());
            decimal kredit = Math.Abs(fixture.Create<decimal>());
            Mock<IReadOnlyBogføringslinjeViewModel> bogføringslinjeViewModelMock = fixture.BuildReadOnlyBogføringslinjeViewModelMock(tekst: tekst, debit: debit, kredit: kredit);
            string kontonavn = fixture.Create<string>();
            decimal beløb = Math.Abs(fixture.Create<decimal>()) * -1;
            Mock<IBogføringsadvarselModel> bogføringsadvarselModelMock = fixture.BuildBogføringsadvarselModelMock(kontonavn: kontonavn, beløb: beløb);
            DateTime tidspunkt = DateTime.Now;
            IBogføringsadvarselViewModel bogføringsadvarselViewModel = new BogføringsadvarselViewModel(fixture.BuildRegnskabViewModel(), bogføringslinjeViewModelMock.Object, bogføringsadvarselModelMock.Object, tidspunkt);
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.Tidspunkt, Is.EqualTo(tidspunkt));
            Assert.That(bogføringsadvarselViewModel.Beløb, Is.GreaterThan(0M));
            Assert.That(bogføringsadvarselViewModel.Information, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.Information, Is.Not.Empty);
            Assert.That(bogføringsadvarselViewModel.Information, Is.EqualTo($"{tidspunkt.ToShortDateString()} {tidspunkt.ToShortTimeString()} {Resource.GetText(Text.AccountOverdrawnWithValue, kontonavn, Math.Abs(beløb).ToString("C"))}\r\n\r\n{Resource.GetText(Text.Cause)}: {tekst} {debit - kredit:C}"));

            bogføringsadvarselModelMock.Verify(m => m.Kontonavn, Times.Once);
            bogføringsadvarselModelMock.Verify(m => m.Beløb, Times.Once);
            bogføringslinjeViewModelMock.Verify(m => m.Tekst, Times.Once);
            bogføringslinjeViewModelMock.Verify(m => m.BogførtAsText, Times.Once);
        }

        /// <summary>
        /// Tester, at getteren til Information returnerer information for advarlsen, hvor beløbet er 0.
        /// </summary>
        [Test]
        public void TestAtInformationGetterReturnererInformationHvorBeløbErNul()
        {
            Fixture fixture = new Fixture();

            string tekst = fixture.Create<string>();
            decimal debit = Math.Abs(fixture.Create<decimal>());
            decimal kredit = Math.Abs(fixture.Create<decimal>());
            Mock<IReadOnlyBogføringslinjeViewModel> bogføringslinjeViewModelMock = fixture.BuildReadOnlyBogføringslinjeViewModelMock(tekst: tekst, debit: debit, kredit: kredit);
            string kontonavn = fixture.Create<string>();
            Mock<IBogføringsadvarselModel> bogføringsadvarselModelMock = fixture.BuildBogføringsadvarselModelMock(kontonavn: kontonavn, beløb: 0M);
            DateTime tidspunkt = DateTime.Now;
            IBogføringsadvarselViewModel bogføringsadvarselViewModel = new BogføringsadvarselViewModel(fixture.BuildRegnskabViewModel(), bogføringslinjeViewModelMock.Object, bogføringsadvarselModelMock.Object, tidspunkt);
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.Tidspunkt, Is.EqualTo(tidspunkt));
            Assert.That(bogføringsadvarselViewModel.Beløb, Is.EqualTo(0M));
            Assert.That(bogføringsadvarselViewModel.Information, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.Information, Is.Not.Empty);
            Assert.That(bogføringsadvarselViewModel.Information, Is.EqualTo($"{tidspunkt.ToShortDateString()} {tidspunkt.ToShortTimeString()} {Resource.GetText(Text.AccountOverdrawnWithoutValue, kontonavn)}\r\n\r\n{Resource.GetText(Text.Cause)}: {tekst} {debit-kredit:C}"));

            bogføringsadvarselModelMock.Verify(m => m.Kontonavn, Times.Once);
            bogføringsadvarselModelMock.Verify(m => m.Beløb, Times.Once);
            bogføringslinjeViewModelMock.Verify(m => m.Tekst, Times.Once);
            bogføringslinjeViewModelMock.Verify(m => m.BogførtAsText, Times.Once);
        }

        /// <summary>
        /// Tester, at getteren til RemoveCommand returnerer en kommando, der kan fjerne advarslen fra et regnskab.
        /// </summary>
        [Test]
        public void TestAtRemoveCommandGetterReturnererCommand()
        {
            Fixture fixture = new Fixture();

            IBogføringsadvarselViewModel bogføringsadvarselViewModel = new BogføringsadvarselViewModel(fixture.BuildRegnskabViewModel(), fixture.BuildReadOnlyBogføringslinjeViewModel(), fixture.BuildBogføringsadvarselModel(), DateTime.Now);
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);

            ICommand removeCommand = bogføringsadvarselViewModel.RemoveCommand;
            Assert.That(removeCommand, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at CanExecute på RemoveCommand returerer værdi, der angiver, om kommandoen kan udføres.
        /// </summary>
        [Test]
        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase("XYZ", true)]
        [TestCase(0, true)]
        [TestCase(1, true)]
        public void TestAtCanExecuteOnRemoveCommandReturnererValue(object parameter, bool expectedResult)
        {
            Fixture fixture = new Fixture();

            IBogføringsadvarselViewModel bogføringsadvarselViewModel = new BogføringsadvarselViewModel(fixture.BuildRegnskabViewModel(), fixture.BuildReadOnlyBogføringslinjeViewModel(), fixture.BuildBogføringsadvarselModel(), DateTime.Now);
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);

            ICommand removeCommand = bogføringsadvarselViewModel.RemoveCommand;
            Assert.That(removeCommand, Is.Not.Null);

            bool result = removeCommand.CanExecute(parameter);
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Tester, at Execute på RemoveCommand udfører kommandoen, der fjerner advarslen fra et regnskab.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("XYZ")]
        [TestCase(0)]
        [TestCase(1)]
        public void TestAtExecuteOnRemoveCommandExecutes(object parameter)
        {
            Fixture fixture = new Fixture();

            IBogføringsadvarselViewModel bogføringsadvarselViewModel = new BogføringsadvarselViewModel(fixture.BuildRegnskabViewModel(), fixture.BuildReadOnlyBogføringslinjeViewModel(), fixture.BuildBogføringsadvarselModel(), DateTime.Now);
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);

            ICommand removeCommand = bogføringsadvarselViewModel.RemoveCommand;
            Assert.That(removeCommand, Is.Not.Null);

            removeCommand.Execute(parameter);
        }

        /// <summary>
        /// Tester, at Execute på RemoveCommand fjeren bogføringsadvarslen fra regnskabet.
        /// </summary>
        [Test]
        public void TestAtExecuteOnRemoveCommandFjernerBogføringsadvarselViewModelFraRegnskab()
        {
            Fixture fixture = new Fixture();

            Mock<IRegnskabViewModel> regnskabViewModelMock = fixture.BuildRegnskabViewModelMock();
            IBogføringsadvarselViewModel bogføringsadvarselViewModel = new BogføringsadvarselViewModel(regnskabViewModelMock.Object, fixture.BuildReadOnlyBogføringslinjeViewModel(), fixture.BuildBogføringsadvarselModel(), DateTime.Now);
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);

            ICommand removeCommand = bogføringsadvarselViewModel.RemoveCommand;
            Assert.That(removeCommand, Is.Not.Null);

            removeCommand.Execute(null);

            regnskabViewModelMock.Verify(m => m.BogføringsadvarselRemove(It.Is<IBogføringsadvarselViewModel>(value => value != null && value == bogføringsadvarselViewModel)), Times.Once);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringslinjeViewModelEventHandler rejser PropertyChanged, når ViewModel for bogføringslinjen, der har medført advarslen, opdateres.
        /// </summary>
        [Test]
        [TestCase("Regnskab", "Bogføringslinje")]
        [TestCase("Løbenummer", "Bogføringslinje")]
        [TestCase("Dato", "Bogføringslinje")]
        [TestCase("Bilag", "Bogføringslinje")]
        [TestCase("Kontonummer", "Bogføringslinje")]
        [TestCase("Tekst", "Bogføringslinje")]
        [TestCase("Tekst", "Information")]
        [TestCase("Budgetkontonummer", "Bogføringslinje")]
        [TestCase("Debit", "Bogføringslinje")]
        [TestCase("DebitAsText", "Bogføringslinje")]
        [TestCase("Kredit", "Bogføringslinje")]
        [TestCase("KreditAsText", "Bogføringslinje")]
        [TestCase("Bogført", "Bogføringslinje")]
        [TestCase("BogførtAsText", "Bogføringslinje")]
        [TestCase("BogførtAsText", "Information")]
        [TestCase("Adressekonto", "Bogføringslinje")]
        [TestCase("DisplayName", "Bogføringslinje")]
        [TestCase("Image", "Bogføringslinje")]
        public void TestAtPropertyChangedOnBogføringslinjeViewModelEventHandlerRejserPropertyChangedOnBogføringslinjeViewModelUpdate(string propertyNameToRaise, string expectPropertyName)
        {
            Fixture fixture = new Fixture();

            Mock<IReadOnlyBogføringslinjeViewModel> bogføringslinjeViewModelMock = fixture.BuildReadOnlyBogføringslinjeViewModelMock();
            IBogføringsadvarselViewModel bogføringsadvarselViewModel = new BogføringsadvarselViewModel(fixture.BuildRegnskabViewModel(), bogføringslinjeViewModelMock.Object, fixture.BuildBogføringsadvarselModel(), DateTime.Now);
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);

            bool eventCalled = false;
            bogføringsadvarselViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, expectPropertyName) == 0)
                {
                    eventCalled = true;
                }
            };

            Assert.That(eventCalled, Is.False);
            bogføringslinjeViewModelMock.Raise(m => m.PropertyChanged += null, bogføringslinjeViewModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringslinjeViewModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnBogføringslinjeViewModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            Fixture fixture = new Fixture();

            Mock<IReadOnlyBogføringslinjeViewModel> bogføringslinjeViewModelMock = fixture.BuildReadOnlyBogføringslinjeViewModelMock();
            IBogføringsadvarselViewModel bogføringsadvarselViewModel = new BogføringsadvarselViewModel(fixture.BuildRegnskabViewModel(), bogføringslinjeViewModelMock.Object, fixture.BuildBogføringsadvarselModel(), DateTime.Now);
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => bogføringslinjeViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringslinjeViewModelEventHandler kaster en ArgumentNullException, hvis argumenter eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnBogføringslinjeViewModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            Fixture fixture = new Fixture();

            Mock<IReadOnlyBogføringslinjeViewModel> bogføringslinjeViewModelMock = fixture.BuildReadOnlyBogføringslinjeViewModelMock();
            IBogføringsadvarselViewModel bogføringsadvarselViewModel = new BogføringsadvarselViewModel(fixture.BuildRegnskabViewModel(), bogføringslinjeViewModelMock.Object, fixture.BuildBogføringsadvarselModel(), DateTime.Now);
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => bogføringslinjeViewModelMock.Raise(m => m.PropertyChanged += null, bogføringslinjeViewModelMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("eventArgs"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringsadvarselModelEventHandler  rejser PropertyChanged, når modellen for advarslen, som er opstået ved bogføring, opdateres.
        /// </summary>
        [Test]
        [TestCase("Advarsel", "Advarsel")]
        [TestCase("Kontonummer", "Kontonummer")]
        [TestCase("Kontonavn", "Kontonavn")]
        [TestCase("Kontonavn", "Information")]
        [TestCase("Beløb", "Beløb")]
        [TestCase("Beløb", "BeløbAsText")]
        [TestCase("Beløb", "Information")]
        public void TestAtPropertyChangedOnBogføringsadvarselModelEventHandlerRejserPropertyChangedOnBogføringsadvarselModelUpdate(string propertyNameToRaise, string expectPropertyName)
        {
            Fixture fixture = new Fixture();

            Mock<IBogføringsadvarselModel> bogføringsadvarselModelMock = fixture.BuildBogføringsadvarselModelMock();
            IBogføringsadvarselViewModel bogføringsadvarselViewModel = new BogføringsadvarselViewModel(fixture.BuildRegnskabViewModel(), fixture.BuildReadOnlyBogføringslinjeViewModel(), bogføringsadvarselModelMock.Object, DateTime.Now);
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);

            bool eventCalled = false;
            bogføringsadvarselViewModel.PropertyChanged += (s, e) =>
            {
                Assert.That(s, Is.Not.Null);
                Assert.That(e, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Null);
                Assert.That(e.PropertyName, Is.Not.Empty);
                if (string.CompareOrdinal(e.PropertyName, expectPropertyName) == 0)
                {
                    eventCalled = true;
                }
            };

            Assert.That(eventCalled, Is.False);
            bogføringsadvarselModelMock.Raise(m => m.PropertyChanged += null, bogføringsadvarselModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringsadvarselModelEventHandler kaster en ArgumentNullException, hvis objektet, der rejser eventet, er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnBogføringsadvarselModelEventHandlerKasterArgumentNullExceptionHvisSenderErNull()
        {
            Fixture fixture = new Fixture();

            Mock<IBogføringsadvarselModel> bogføringsadvarselModelMock = fixture.BuildBogføringsadvarselModelMock();
            IBogføringsadvarselViewModel bogføringsadvarselViewModel = new BogføringsadvarselViewModel(fixture.BuildRegnskabViewModel(), fixture.BuildReadOnlyBogføringslinjeViewModel(), bogføringsadvarselModelMock.Object, DateTime.Now);
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => bogføringsadvarselModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("sender"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnBogføringsadvarselModelEventHandler kaster en ArgumentNullException, hvis argumenter eventet er null.
        /// </summary>
        [Test]
        public void TestAtPropertyChangedOnBogføringsadvarselModelEventHandlerKasterArgumentNullExceptionHvisEventArgsErNull()
        {
            Fixture fixture = new Fixture();

            Mock<IBogføringsadvarselModel> bogføringsadvarselModelMock = fixture.BuildBogføringsadvarselModelMock();
            IBogføringsadvarselViewModel bogføringsadvarselViewModel = new BogføringsadvarselViewModel(fixture.BuildRegnskabViewModel(), fixture.BuildReadOnlyBogføringslinjeViewModel(), bogføringsadvarselModelMock.Object, DateTime.Now);
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => bogføringsadvarselModelMock.Raise(m => m.PropertyChanged += null, bogføringsadvarselModelMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("eventArgs"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}