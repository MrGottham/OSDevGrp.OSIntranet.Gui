using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;
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

            // TODO: RemoveCommand må ikke være null.
            // Assert.That(bogføringsadvarselViewModel.RemoveCommand, Is.Not.Null);

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
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));
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

            var bogføringsadvarselModelMock = fixture.Create<IBogføringsadvarselModel>();
            var tidspunkt = fixture.Create<DateTime>();
            var bogføringsadvarselViewModel = new BogføringsadvarselViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IReadOnlyBogføringslinjeViewModel>(), bogføringsadvarselModelMock, tidspunkt);
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.Tidspunkt, Is.EqualTo(tidspunkt));
            Assert.That(bogføringsadvarselViewModel.Beløb, Is.GreaterThan(0M));
            Assert.That(bogføringsadvarselViewModel.Information, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.Information, Is.Not.Empty);
            Assert.That(bogføringsadvarselViewModel.Information, Is.EqualTo(string.Format("{0} {1}\r\n{2}", tidspunkt.ToShortDateString(), tidspunkt.ToShortTimeString(), Resource.GetText(Text.AccountOverdrawned, bogføringsadvarselModelMock.Kontonavn))));

            bogføringsadvarselModelMock.AssertWasCalled(m => m.Kontonavn);
            bogføringsadvarselModelMock.AssertWasCalled(m => m.Beløb);
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
            fixture.Customize<IReadOnlyBogføringslinjeViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IReadOnlyBogføringslinjeViewModel>()));
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

            var bogføringsadvarselModelMock = fixture.Create<IBogføringsadvarselModel>();
            var tidspunkt = fixture.Create<DateTime>();
            var bogføringsadvarselViewModel = new BogføringsadvarselViewModel(fixture.Create<IRegnskabViewModel>(), fixture.Create<IReadOnlyBogføringslinjeViewModel>(), bogføringsadvarselModelMock, tidspunkt);
            Assert.That(bogføringsadvarselViewModel, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.Tidspunkt, Is.EqualTo(tidspunkt));
            Assert.That(bogføringsadvarselViewModel.Beløb, Is.EqualTo(0M));
            Assert.That(bogføringsadvarselViewModel.Information, Is.Not.Null);
            Assert.That(bogføringsadvarselViewModel.Information, Is.Not.Empty);
            Assert.That(bogføringsadvarselViewModel.Information, Is.EqualTo(string.Format("{0} {1}\r\n{2}", tidspunkt.ToShortDateString(), tidspunkt.ToShortTimeString(), Resource.GetText(Text.AccountOverdrawned, bogføringsadvarselModelMock.Kontonavn))));

            bogføringsadvarselModelMock.AssertWasCalled(m => m.Kontonavn);
            bogføringsadvarselModelMock.AssertWasCalled(m => m.Beløb);
        }
    }
}
