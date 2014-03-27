using System;
using System.ComponentModel;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Ploeh.AutoFixture;
using Rhino.Mocks;
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
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));
            fixture.Customize<IBogføringsadvarselModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBogføringsadvarselModel>();
                    mock.Expect(m => m.Advarsel)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.Kontonummer)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.Kontonavn)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.Beløb)
                        .Return(fixture.Create<decimal>()*-1)
                        .Repeat.Any();
                    return mock;
                }));

            var regnskabViewModelMock = fixture.Create<IRegnskabViewModel>();
            var bogføringslinjeViewModelMock = fixture.Create<IReadOnlyBogføringslinjeViewModel>();
            var bogføringsadvarselModelMock = fixture.Create<IBogføringsadvarselModel>();
            var tidspunkt = fixture.Create<DateTime>();
            var bogføringsadvarselViewModel = new BogføringsadvarselViewModel(regnskabViewModelMock, bogføringslinjeViewModelMock, bogføringsadvarselModelMock, tidspunkt);
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
            Assert.That(bogføringsadvarselViewModel.Advarsel, Is.EqualTo(bogføringsadvarselModelMock.Advarsel));
            Assert.That(bogføringsadvarselViewModel.Kontonummer, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.Kontonummer, Is.Not.Empty);
            Assert.That(bogføringsadvarselViewModel.Kontonummer, Is.EqualTo(bogføringsadvarselModelMock.Kontonummer));
            Assert.That(bogføringsadvarselViewModel.Kontonavn, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.Kontonavn, Is.Not.Empty);
            Assert.That(bogføringsadvarselViewModel.Kontonavn, Is.EqualTo(bogføringsadvarselModelMock.Kontonavn));
            Assert.That(bogføringsadvarselViewModel.Beløb, Is.GreaterThan(0M));
            Assert.That(bogføringsadvarselViewModel.Beløb, Is.EqualTo(Math.Abs(bogføringsadvarselModelMock.Beløb)));
            Assert.That(bogføringsadvarselViewModel.BeløbAsText, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.BeløbAsText, Is.Not.Empty);
            Assert.That(bogføringsadvarselViewModel.BeløbAsText, Is.EqualTo(Math.Abs(bogføringsadvarselModelMock.Beløb).ToString("C")));
            Assert.That(bogføringsadvarselViewModel.Information, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.Information, Is.Not.Empty);
            Assert.That(bogføringsadvarselViewModel.RemoveCommand, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.RemoveCommand, Is.TypeOf<RelayCommand>());
            Assert.That(bogføringsadvarselViewModel.DisplayName, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.DisplayName, Is.Not.Empty);
            Assert.That(bogføringsadvarselViewModel.DisplayName, Is.EqualTo(Resource.GetText(Text.PostingWarning)));
            Assert.That(bogføringsadvarselViewModel.Image, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.Image, Is.Not.Empty);
            Assert.That(bogføringsadvarselViewModel.Image, Is.EqualTo(Resource.GetEmbeddedResource("Images.Bogføringslinje.png")));

            bogføringsadvarselModelMock.AssertWasCalled(m => m.Advarsel);
            bogføringsadvarselModelMock.AssertWasCalled(m => m.Kontonummer);
            bogføringsadvarselModelMock.AssertWasCalled(m => m.Kontonavn);
            bogføringsadvarselModelMock.AssertWasCalled(m => m.Beløb);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for regnskabet er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisRegnskabViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));
            fixture.Customize<IBogføringsadvarselModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringsadvarselModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BogføringsadvarselViewModel(null, fixture.Create<IReadOnlyBogføringslinjeViewModel>(), fixture.Create<IBogføringsadvarselModel>(), fixture.Create<DateTime>()));
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
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IBogføringsadvarselModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringsadvarselModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new BogføringsadvarselViewModel(fixture.Create<IRegnskabViewModel>(), null, fixture.Create<IBogføringsadvarselModel>(), fixture.Create<DateTime>()));
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
            var fixture = new Fixture();
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var exception = Assert.Throws<ArgumentNullException>(() => new BogføringsadvarselViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IReadOnlyBogføringslinjeViewModel>(), null, fixture.Create<DateTime>()));
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
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>();
                    mock.Expect(m => m.Tekst)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.BogførtAsText)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    return mock;
                }));
            fixture.Customize<IBogføringsadvarselModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBogføringsadvarselModel>();
                    mock.Expect(m => m.Kontonavn)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.Beløb)
                        .Return(fixture.Create<decimal>()*-1)
                        .Repeat.Any();
                    return mock;
                }));

            var bogføringslinjeViewModelMock = fixture.Create<IReadOnlyBogføringslinjeViewModel>();
            var bogføringsadvarselModelMock = fixture.Create<IBogføringsadvarselModel>();
            var tidspunkt = fixture.Create<DateTime>();
            var bogføringsadvarselViewModel = new BogføringsadvarselViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeViewModelMock, bogføringsadvarselModelMock, tidspunkt);
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.Tidspunkt, Is.EqualTo(tidspunkt));
            Assert.That(bogføringsadvarselViewModel.Beløb, Is.GreaterThan(0M));
            Assert.That(bogføringsadvarselViewModel.Information, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.Information, Is.Not.Empty);
            Assert.That(bogføringsadvarselViewModel.Information, Is.EqualTo(string.Format("{0} {1} {2}\r\n\r\n{3}: {4} {5}", tidspunkt.ToShortDateString(), tidspunkt.ToShortTimeString(), Resource.GetText(Text.AccountOverdrawnedWithValue, bogføringsadvarselModelMock.Kontonavn, Math.Abs(bogføringsadvarselModelMock.Beløb).ToString("C")), Resource.GetText(Text.Cause), bogføringslinjeViewModelMock.Tekst, bogføringslinjeViewModelMock.BogførtAsText)));

            bogføringsadvarselModelMock.AssertWasCalled(m => m.Kontonavn);
            bogføringsadvarselModelMock.AssertWasCalled(m => m.Beløb);
            bogføringslinjeViewModelMock.AssertWasCalled(m => m.Tekst);
            bogføringslinjeViewModelMock.AssertWasCalled(m => m.BogførtAsText);
        }

        /// <summary>
        /// Tester, at getteren til Information returnerer information for advarlsen, hvor beløbet er 0.
        /// </summary>
        [Test]
        public void TestAtInformationGetterReturnererInformationHvorBeløbErNul()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>();
                    mock.Expect(m => m.Tekst)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.BogførtAsText)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    return mock;
                }));
            fixture.Customize<IBogføringsadvarselModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IBogføringsadvarselModel>();
                    mock.Expect(m => m.Kontonavn)
                        .Return(fixture.Create<string>())
                        .Repeat.Any();
                    mock.Expect(m => m.Beløb)
                        .Return(0M)
                        .Repeat.Any();
                    return mock;
                }));

            var bogføringslinjeViewModelMock = fixture.Create<IReadOnlyBogføringslinjeViewModel>();
            var bogføringsadvarselModelMock = fixture.Create<IBogføringsadvarselModel>();
            var tidspunkt = fixture.Create<DateTime>();
            var bogføringsadvarselViewModel = new BogføringsadvarselViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeViewModelMock, bogføringsadvarselModelMock, tidspunkt);
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.Tidspunkt, Is.EqualTo(tidspunkt));
            Assert.That(bogføringsadvarselViewModel.Beløb, Is.EqualTo(0M));
            Assert.That(bogføringsadvarselViewModel.Information, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.Information, Is.Not.Empty);
            Assert.That(bogføringsadvarselViewModel.Information, Is.EqualTo(string.Format("{0} {1} {2}\r\n\r\n{3}: {4} {5}", tidspunkt.ToShortDateString(), tidspunkt.ToShortTimeString(), Resource.GetText(Text.AccountOverdrawnedWithoutValue, bogføringsadvarselModelMock.Kontonavn), Resource.GetText(Text.Cause), bogføringslinjeViewModelMock.Tekst, bogføringslinjeViewModelMock.BogførtAsText)));

            bogføringsadvarselModelMock.AssertWasCalled(m => m.Kontonavn);
            bogføringsadvarselModelMock.AssertWasCalled(m => m.Beløb);
            bogføringslinjeViewModelMock.AssertWasCalled(m => m.Tekst);
            bogføringslinjeViewModelMock.AssertWasCalled(m => m.BogførtAsText);
        }

        /// <summary>
        /// Tester, at getteren til RemoveCommand returnerer en kommando, der kan fjerne advarslen fra et regnskab.
        /// </summary>
        [Test]
        public void TestAtRemoveCommandGetterReturnererCommand()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));
            fixture.Customize<IBogføringsadvarselModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringsadvarselModel>()));

            var bogføringsadvarselViewModel = new BogføringsadvarselViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IReadOnlyBogføringslinjeViewModel>(), fixture.Create<IBogføringsadvarselModel>(), fixture.Create<DateTime>());
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);

            var removeCommand = bogføringsadvarselViewModel.RemoveCommand;
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
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));
            fixture.Customize<IBogføringsadvarselModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringsadvarselModel>()));

            var bogføringsadvarselViewModel = new BogføringsadvarselViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IReadOnlyBogføringslinjeViewModel>(), fixture.Create<IBogføringsadvarselModel>(), fixture.Create<DateTime>());
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);

            var removeCommand = bogføringsadvarselViewModel.RemoveCommand;
            Assert.That(removeCommand, Is.Not.Null);

            var result = removeCommand.CanExecute(parameter);
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
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));
            fixture.Customize<IBogføringsadvarselModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringsadvarselModel>()));

            var bogføringsadvarselViewModel = new BogføringsadvarselViewModel(MockRepository.GenerateMock<IRegnskabViewModel>(), fixture.Create<IReadOnlyBogføringslinjeViewModel>(), fixture.Create<IBogføringsadvarselModel>(), fixture.Create<DateTime>());
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);

            var removeCommand = bogføringsadvarselViewModel.RemoveCommand;
            Assert.That(removeCommand, Is.Not.Null);

            removeCommand.Execute(parameter);
        }

        /// <summary>
        /// Tester, at Execute på RemoveCommand fjeren bogføringsadvarslen fra regnskabet.
        /// </summary>
        [Test]
        public void TestAtExecuteOnRemoveCommandFjernerBogføringsadvarselViewModelFraRegnskab()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));
            fixture.Customize<IBogføringsadvarselModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringsadvarselModel>()));

            var regnskabViewModelMock = fixture.Create<IRegnskabViewModel>();
            var bogføringsadvarselViewModel = new BogføringsadvarselViewModel(regnskabViewModelMock, fixture.Create<IReadOnlyBogføringslinjeViewModel>(), fixture.Create<IBogføringsadvarselModel>(), fixture.Create<DateTime>());
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);

            var removeCommand = bogføringsadvarselViewModel.RemoveCommand;
            Assert.That(removeCommand, Is.Not.Null);

            removeCommand.Execute(null);

            regnskabViewModelMock.AssertWasCalled(m => m.BogføringsadvarselRemove(Arg<IBogføringsadvarselViewModel>.Is.Equal(bogføringsadvarselViewModel)));
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
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));
            fixture.Customize<IBogføringsadvarselModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringsadvarselModel>()));

            var bogføringslinjeViewModelMock = fixture.Create<IReadOnlyBogføringslinjeViewModel>();
            var bogføringsadvarselViewModel = new BogføringsadvarselViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeViewModelMock, fixture.Create<IBogføringsadvarselModel>(), fixture.Create<DateTime>());
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);

            var eventCalled = false;
            bogføringsadvarselViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, expectPropertyName, StringComparison.Ordinal) == 0)
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
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));
            fixture.Customize<IBogføringsadvarselModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringsadvarselModel>()));

            var bogføringslinjeViewModelMock = fixture.Create<IReadOnlyBogføringslinjeViewModel>();
            var bogføringsadvarselViewModel = new BogføringsadvarselViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeViewModelMock, fixture.Create<IBogføringsadvarselModel>(), fixture.Create<DateTime>());
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => bogføringslinjeViewModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
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
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));
            fixture.Customize<IBogføringsadvarselModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringsadvarselModel>()));

            var bogføringslinjeViewModelMock = fixture.Create<IReadOnlyBogføringslinjeViewModel>();
            var bogføringsadvarselViewModel = new BogføringsadvarselViewModel(fixture.Create<IRegnskabViewModel>(), bogføringslinjeViewModelMock, fixture.Create<IBogføringsadvarselModel>(), fixture.Create<DateTime>());
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => bogføringslinjeViewModelMock.Raise(m => m.PropertyChanged += null, bogføringslinjeViewModelMock, null));
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
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));
            fixture.Customize<IBogføringsadvarselModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringsadvarselModel>()));

            var bogføringsadvarselModelMock = fixture.Create<IBogføringsadvarselModel>();
            var bogføringsadvarselViewModel = new BogføringsadvarselViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IReadOnlyBogføringslinjeViewModel>(), bogføringsadvarselModelMock, fixture.Create<DateTime>());
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);

            var eventCalled = false;
            bogføringsadvarselViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, expectPropertyName, StringComparison.Ordinal) == 0)
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
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));
            fixture.Customize<IBogføringsadvarselModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringsadvarselModel>()));

            var bogføringsadvarselModelMock = fixture.Create<IBogføringsadvarselModel>();
            var bogføringsadvarselViewModel = new BogføringsadvarselViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IReadOnlyBogføringslinjeViewModel>(), bogføringsadvarselModelMock, fixture.Create<DateTime>());
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => bogføringsadvarselModelMock.Raise(m => m.PropertyChanged += null, null, fixture.Create<PropertyChangedEventArgs>()));
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
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));
            fixture.Customize<IBogføringsadvarselModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IBogføringsadvarselModel>()));

            var bogføringsadvarselModelMock = fixture.Create<IBogføringsadvarselModel>();
            var bogføringsadvarselViewModel = new BogføringsadvarselViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IReadOnlyBogføringslinjeViewModel>(), bogføringsadvarselModelMock, fixture.Create<DateTime>());
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => bogføringsadvarselModelMock.Raise(m => m.PropertyChanged += null, bogføringsadvarselModelMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("eventArgs"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
