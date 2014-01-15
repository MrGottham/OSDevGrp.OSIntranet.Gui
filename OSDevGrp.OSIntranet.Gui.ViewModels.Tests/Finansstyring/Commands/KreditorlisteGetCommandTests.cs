using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring.Commands
{
    /// <summary>
    /// Tester kommando, der kan hente og opdatere kreditorlisten til et regnskab.
    /// </summary>
    [TestFixture]
    public class KreditorlisteGetCommandTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en kommando, der kan hente og opdatere kreditorlisten til et regnskab.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererKreditorlisteGetCommand()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var command = new KreditorlisteGetCommand(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(command, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repositoryet til finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new KreditorlisteGetCommand(null, fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("finansstyringRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for exceptionhandleren er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisExceptionHandlerViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new KreditorlisteGetCommand(fixture.Create<IFinansstyringRepository>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionHandlerViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at CanExecute returnerer true.
        /// </summary>
        [Test]
        public void TestAtCanExecuteReturnererTrue()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));

            var command = new KreditorlisteGetCommand(fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>());
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(fixture.Create<IRegnskabViewModel>());
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tester, at Execute tilføjer nye kreditorer til regnskabet.
        /// </summary>
        [Test]
        public void TestAtExecuteAddsNyeKreditorerTilRegnskabViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));
            fixture.Customize<IAdressekontoModel>(e => e.FromFactory(() =>
                {
                    var mock = MockRepository.GenerateMock<IAdressekontoModel>();
                    mock.Expect(m => m.Nummer)
                        .Return(fixture.Create<int>())
                        .Repeat.Any();
                    return mock;
                }));

            var finansstyringKonfigurationRepositoryMock = MockRepository.GenerateMock<IFinansstyringKonfigurationRepository>();
            finansstyringKonfigurationRepositoryMock.Expect(m => m.DageForNyheder)
                                                    .Return(7)
                                                    .Repeat.Any();

            var adressekontoModelCollection = fixture.CreateMany<IAdressekontoModel>(250).ToList();
            Func<IEnumerable<IAdressekontoModel>> getter = () => adressekontoModelCollection;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.Konfiguration)
                                       .Return(finansstyringKonfigurationRepositoryMock)
                                       .Repeat.Any();
            finansstyringRepositoryMock.Expect(m => m.KreditorlisteGetAsync(Arg<int>.Is.GreaterThan(0), Arg<DateTime>.Is.GreaterThan(DateTime.MinValue)))
                                       .Return(Task.Run(getter))
                                       .Repeat.Any();

            var exceptionHandlerViewModelMock = MockRepository.GenerateMock<IExceptionHandlerViewModel>();

            var count = adressekontoModelCollection.Count;
            using (var waitEvent = new AutoResetEvent(false))
            {
                var we = waitEvent;
                var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
                regnskabViewModelMock.Expect(m => m.Nummer)
                                     .Return(fixture.Create<int>())
                                     .Repeat.Any();
                regnskabViewModelMock.Expect(m => m.StatusDato)
                                     .Return(fixture.Create<DateTime>())
                                     .Repeat.Any();
                regnskabViewModelMock.Expect(m => m.Kreditorer)
                                     .Return(new List<IAdressekontoViewModel>(0))
                                     .Repeat.Any();
                regnskabViewModelMock.Expect(m => m.KreditorAdd(Arg<IAdressekontoViewModel>.Is.NotNull))
                                     .WhenCalled(e =>
                                         {
                                             count--;
                                             if (count == 0)
                                             {
                                                 we.Set();
                                             }
                                         })
                                     .Repeat.Any();

                var command = new KreditorlisteGetCommand(finansstyringRepositoryMock, exceptionHandlerViewModelMock);
                Assert.That(command, Is.Not.Null);

                command.Execute(regnskabViewModelMock);
                waitEvent.WaitOne(3000);

                finansstyringRepositoryMock.AssertWasCalled(m => m.Konfiguration);
                finansstyringRepositoryMock.AssertWasCalled(m => m.KreditorlisteGetAsync(Arg<int>.Is.Equal(regnskabViewModelMock.Nummer), Arg<DateTime>.Is.Equal(regnskabViewModelMock.StatusDato)));
                finansstyringKonfigurationRepositoryMock.AssertWasCalled(m => m.DageForNyheder);
                regnskabViewModelMock.AssertWasCalled(m => m.Kreditorer);
                regnskabViewModelMock.AssertWasCalled(m => m.KreditorAdd(Arg<IAdressekontoViewModel>.Is.NotNull), opt => opt.Repeat.Times(adressekontoModelCollection.Count));
            }

            exceptionHandlerViewModelMock.AssertWasNotCalled(m => m.HandleException(Arg<Exception>.Is.Anything));
        }
    }
}
