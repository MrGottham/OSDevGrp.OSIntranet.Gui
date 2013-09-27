using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Intrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring.Commands
{
    /// <summary>
    /// Tester kommando til genindlæsning af regnskabslisten.
    /// </summary>
    [TestFixture]
    public class RegnskabslisteRefreshCommandTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en kommando til genindlæsning af regnskabslisten.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererRegnskabslisteRefreshCommand()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var command = new RegnskabslisteRefreshCommand(fixture.Create<IFinansstyringRepository>());
            Assert.That(command, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repositoryet til finansstyring er null..
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RegnskabslisteRefreshCommand(null));
        }

        /// <summary>
        /// Tester, at CanExecute returnerer true.
        /// </summary>
        [Test]
        public void TestAtCanExecuteReturnererTrue()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IRegnskabslisteViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabslisteViewModel>()));

            var command = new RegnskabslisteRefreshCommand(fixture.Create<IFinansstyringRepository>());
            Assert.That(command, Is.Not.Null);

            var result = command.CanExecute(fixture.Create<IRegnskabslisteViewModel>());
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tester at Execute tilføjer nye regnskaber listen af regnskaber.
        /// </summary>
        [Test]
        public void TestAtExecuteAdderNyeRegnskaberTilRegnskabslisteViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() =>
                {
                    var regnskabModelMock = MockRepository.GenerateMock<IRegnskabModel>();
                    regnskabModelMock.Expect(m => m.Nummer)
                                     .Return(fixture.Create<int>())
                                     .Repeat.Any();
                    return regnskabModelMock;
                }));

            var regnskabCollection = fixture.CreateMany<IRegnskabModel>(25).ToList();
            Func<IEnumerable<IRegnskabModel>> regnskabCollectionGetter = () => regnskabCollection;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabslisteGetAsync())
                                       .Return(Task.Run(regnskabCollectionGetter))
                                       .Repeat.Any();

            var regnskabslisteViewModelMock = MockRepository.GenerateMock<IRegnskabslisteViewModel>();
            regnskabslisteViewModelMock.Expect(m => m.Regnskaber)
                                       .Return(new ObservableCollection<IRegnskabViewModel>(new List<IRegnskabViewModel>()))
                                       .Repeat.Any();

            var command = new RegnskabslisteRefreshCommand(finansstyringRepositoryMock);
            Assert.That(command, Is.Not.Null);

            command.Execute(regnskabslisteViewModelMock);

            finansstyringRepositoryMock.AssertWasCalled(m => m.RegnskabslisteGetAsync());
            regnskabslisteViewModelMock.AssertWasCalled(m => m.RegnskabAdd(Arg<IRegnskabViewModel>.Is.NotNull), opt => opt.Repeat.Times(regnskabCollection.Count));
        }

        /// <summary>
        /// Tester, at Execute opdaterer eksisterede regnskab i listen af regnskaber.
        /// </summary>
        [Test]
        public void TestAtExecuteOpdatererEksisteredeRegnskabIRegnskabslisteViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabModel>(e => e.FromFactory(() =>
                {
                    var regnskabModelMock = MockRepository.GenerateMock<IRegnskabModel>();
                    regnskabModelMock.Expect(m => m.Nummer)
                                     .Return(fixture.Create<int>())
                                     .Repeat.Any();
                    return regnskabModelMock;
                }));

            var regnskabCollection = fixture.CreateMany<IRegnskabModel>(1).ToList();
            Func<IEnumerable<IRegnskabModel>> regnskabCollectionGetter = () => regnskabCollection;
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabslisteGetAsync())
                                       .Return(Task.Run(regnskabCollectionGetter))
                                       .Repeat.Any();

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabViewModelMock.Expect(m => m.Nummer)
                                 .Return(regnskabCollection.ElementAt(0).Nummer)
                                 .Repeat.Any();
            var regnskabslisteViewModelMock = MockRepository.GenerateMock<IRegnskabslisteViewModel>();
            regnskabslisteViewModelMock.Expect(m => m.Regnskaber)
                                       .Return(new ObservableCollection<IRegnskabViewModel>(new List<IRegnskabViewModel> {regnskabViewModelMock}))
                                       .Repeat.Any();

            var command = new RegnskabslisteRefreshCommand(finansstyringRepositoryMock);
            Assert.That(command, Is.Not.Null);

            command.Execute(regnskabslisteViewModelMock);

            finansstyringRepositoryMock.AssertWasCalled(m => m.RegnskabslisteGetAsync());
            regnskabViewModelMock.AssertWasCalled(m => m.Navn = Arg<string>.Is.Equal(regnskabCollection.ElementAt(0).Navn));
            regnskabslisteViewModelMock.AssertWasNotCalled(m => m.RegnskabAdd(Arg<IRegnskabViewModel>.Is.Anything));
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetGuiRepositoryException ved IntranetGuiRepositoryException.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetGuiRepositoryExceptionVedIntranetGuiRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Customize<IRegnskabslisteViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabslisteViewModel>()));

            Func<IEnumerable<IRegnskabModel>> regnskabCollectionGetter = () =>
                {
                    throw fixture.Create<IntranetGuiRepositoryException>();
                };
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabslisteGetAsync())
                                       .Return(Task.Run(regnskabCollectionGetter))
                                       .Repeat.Any();

            var command = new RegnskabslisteRefreshCommand(finansstyringRepositoryMock);
            Assert.That(command, Is.Not.Null);

            using (var waitEvent = new AutoResetEvent(false))
            {
                var we = waitEvent;
                command.OnException += (s, e) =>
                    {
                        try
                        {
                            Assert.That(s, Is.Not.Null);
                            Assert.That(e, Is.Not.Null);
                            Assert.That(e.Error, Is.Not.Null);
                            Assert.That(e.Error, Is.TypeOf<IntranetGuiRepositoryException>());
                        }
                        finally
                        {
                            we.Set();
                        }
                    };
                command.Execute(fixture.Create<IRegnskabslisteViewModel>());
                waitEvent.WaitOne();
            }

            finansstyringRepositoryMock.AssertWasCalled(m => m.RegnskabslisteGetAsync());
        }
    }
}
