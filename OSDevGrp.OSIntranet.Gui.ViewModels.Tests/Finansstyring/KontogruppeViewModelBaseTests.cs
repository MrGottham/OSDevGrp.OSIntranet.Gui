using System;
using System.ComponentModel;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring
{
    /// <summary>
    /// Tester ViewModel, der indeholder de grundlæggende oplysninger for en kontogruppe.
    /// </summary>
    [TestFixture]
    public class KontogruppeViewModelBaseTests
    {
        /// <summary>
        /// Egen klasse til test af ViewModel, der indeholder de grundlæggende oplysninger for en kontogruppe.
        /// </summary>
        private class MyKontogruppeViewModel : KontogruppeViewModelBase<IKontogruppeModelBase>
        {
            #region Constructor

            /// <summary>
            /// Danner egen klasse til test af ViewModel, der indeholder de grundlæggende oplysninger for en kontogruppe.
            /// </summary>
            /// <param name="kontogruppeModel">Modellen, der indeholder de grundlæggende oplysninger for en kontogruppe.</param>
            /// <param name="exceptionHandlerViewModel">Implementering af ViewModel for en exceptionhandler.</param>
            public MyKontogruppeViewModel(IKontogruppeModelBase kontogruppeModel, IExceptionHandlerViewModel exceptionHandlerViewModel)
                : base(kontogruppeModel, exceptionHandlerViewModel)
            {
            }

            #endregion
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en ViewModel, der indeholder de grundlæggende oplysninger for en kontogruppe.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererKontogruppeViewModelBase()
        {
            var fixture = new Fixture();

            var kontogruppeModelMock = MockRepository.GenerateMock<IKontogruppeModelBase>();
            kontogruppeModelMock.Expect(m => m.Id)
                                .Return(fixture.Create<string>())
                                .Repeat.Any();
            kontogruppeModelMock.Expect(m => m.Nummer)
                                .Return(fixture.Create<int>())
                                .Repeat.Any();
            kontogruppeModelMock.Expect(m => m.Tekst)
                                .Return(fixture.Create<string>())
                                .Repeat.Any();

            var exceptionHandleViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var kontogruppeViewModel = new MyKontogruppeViewModel(kontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(kontogruppeViewModel, Is.Not.Null);
            Assert.That(kontogruppeViewModel.Id, Is.Not.Null);
            Assert.That(kontogruppeViewModel.Id, Is.Not.Empty);
            Assert.That(kontogruppeViewModel.Id, Is.EqualTo(kontogruppeModelMock.Id));
            Assert.That(kontogruppeViewModel.Nummer, Is.EqualTo(kontogruppeModelMock.Nummer));
            Assert.That(kontogruppeViewModel.Tekst, Is.Not.Null);
            Assert.That(kontogruppeViewModel.Tekst, Is.Not.Empty);
            Assert.That(kontogruppeViewModel.Tekst, Is.EqualTo(kontogruppeModelMock.Tekst));
            Assert.That(kontogruppeViewModel.DisplayName, Is.Not.Null);
            Assert.That(kontogruppeViewModel.DisplayName, Is.Not.Empty);
            Assert.That(kontogruppeViewModel.DisplayName, Is.EqualTo(kontogruppeModelMock.Tekst));

            kontogruppeModelMock.AssertWasCalled(m => m.Id);
            kontogruppeModelMock.AssertWasCalled(m => m.Nummer);
            kontogruppeModelMock.AssertWasCalled(m => m.Tekst);
            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis modellen, , der indeholder de grundlæggende oplysninger for en kontogruppe, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisKontogruppeModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new MyKontogruppeViewModel(null, fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("tabelModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for exceptionhandleren er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisExceptionHandleViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IKontogruppeModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeModelBase>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new MyKontogruppeViewModel(fixture.Create<IKontogruppeModelBase>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionHandlerViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at PropertyChangedOnTabelModelEventHandler rejser PropertyChanged, når modellen, der indeholder de grundlæggende oplysninger for en kontogruppe, opdateres.
        /// </summary>
        [Test]
        [TestCase("Id", "Id")]
        [TestCase("Nummer", "Nummer")]
        [TestCase("Tekst", "Tekst")]
        [TestCase("Tekst", "DisplayName")]
        public void TestAtPropertyChangedOnTabelModelEventHandlerRejserPropertyChangedOnKontogruppeModelUpdate(string propertyNameToRaise, string expectPropertyName)
        {
            var fixture = new Fixture();
            fixture.Customize<IKontogruppeModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeModelBase>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var kontogruppeModelMock = fixture.Create<IKontogruppeModelBase>();
            var exceptionHandleViewModelMock = fixture.Create<IExceptionHandlerViewModel>();

            var kontogruppeViewModel = new MyKontogruppeViewModel(kontogruppeModelMock, exceptionHandleViewModelMock);
            Assert.That(kontogruppeViewModel, Is.Not.Null);

            var eventCalled = false;
            kontogruppeViewModel.PropertyChanged += (s, e) =>
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
            kontogruppeModelMock.Raise(m => m.PropertyChanged += null, kontogruppeModelMock, new PropertyChangedEventArgs(propertyNameToRaise));
            Assert.That(eventCalled, Is.True);

            exceptionHandleViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }
    }
}
