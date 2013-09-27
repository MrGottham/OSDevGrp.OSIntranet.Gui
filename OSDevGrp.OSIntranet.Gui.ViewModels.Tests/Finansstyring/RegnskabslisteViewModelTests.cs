﻿using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring.Commands;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Ploeh.AutoFixture;
using Rhino.Mocks;
using Text = OSDevGrp.OSIntranet.Gui.Resources.Text;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring
{
    /// <summary>
    /// Tester ViewModel for en liste indeholdende regnskaber.
    /// </summary>
    [TestFixture]
    public class RegnskabslisteViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer ViewModel for en liste indeholdende regnskaber.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererRegnskabslisteViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var regnskabslisteViewModel = new RegnskabslisteViewModel(fixture.Create<IFinansstyringRepository>());
            Assert.That(regnskabslisteViewModel, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.DisplayName, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.DisplayName, Is.Not.Empty);
            Assert.That(regnskabslisteViewModel.DisplayName, Is.EqualTo(Resource.GetText(Text.Accountings)));
            Assert.That(regnskabslisteViewModel.StatusDato, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Empty);
            Assert.That(regnskabslisteViewModel.RefreshCommand, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.RefreshCommand, Is.TypeOf<RegnskabslisteRefreshCommand>());
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en RegnskabAdd, hvis repositoryet til finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RegnskabslisteViewModel(null));
        }

        /// <summary>
        /// Tester, at sætteren til StatusDato opdaterer statusdatoen for regnskabslisten.
        /// </summary>
        [Test]
        public void TestAtStatusDatoSetterOpdatererStatusDato()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var regnskabslisteViewModel = new RegnskabslisteViewModel(fixture.Create<IFinansstyringRepository>());
            Assert.That(regnskabslisteViewModel, Is.Not.Null);

            var statusDato = new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0, 0);
            regnskabslisteViewModel.StatusDato = statusDato;
            Assert.That(regnskabslisteViewModel.StatusDato, Is.EqualTo(statusDato));
        }

        /// <summary>
        /// Tester, at sætteren til StatusDato rejser PropertyChanged ved ændring af statusdatoen.
        /// </summary>
        [Test]
        public void TestAtStatusDatoSetterRejserPropertyChangedVedVedOpdateringAfStatusDato()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var regnskabslisteViewModel = new RegnskabslisteViewModel(fixture.Create<IFinansstyringRepository>());
            Assert.That(regnskabslisteViewModel, Is.Not.Null);

            var eventCalled = false;
            regnskabslisteViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    Assert.That(e.PropertyName, Is.EqualTo("StatusDato"));
                    eventCalled = true;
                };

            Assert.That(eventCalled, Is.False);
            regnskabslisteViewModel.StatusDato = regnskabslisteViewModel.StatusDato;
            Assert.That(eventCalled, Is.False);
            regnskabslisteViewModel.StatusDato = regnskabslisteViewModel.StatusDato.AddHours(-1);
            Assert.That(eventCalled, Is.True);
        }

        /// <summary>
        /// Tester, at RegnskabAdd kaster en ArgumentNullException, hvis regnskabet, der forsøges tilføjet, er null.
        /// </summary>
        [Test]
        public void TestAtRegnskabAddKasterArgumentNullExceptionHvisRegnskabViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));

            var regnskabslisteViewModel = new RegnskabslisteViewModel(fixture.Create<IFinansstyringRepository>());
            Assert.That(regnskabslisteViewModel, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => regnskabslisteViewModel.RegnskabAdd(null));
        }

        /// <summary>
        /// Tester, at RegnskabAdd tilføjer et regnskab til listen af regnskaber.
        /// </summary>
        [Test]
        public void TestAtRegnskabAddAdderRegnskabViewModel()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));

            var regnskabslisteViewModel = new RegnskabslisteViewModel(fixture.Create<IFinansstyringRepository>());
            Assert.That(regnskabslisteViewModel, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Empty);

            var regnskabViewModelMock = MockRepository.GenerateMock<IRegnskabViewModel>();
            regnskabslisteViewModel.RegnskabAdd(regnskabViewModelMock);
            Assert.That(regnskabslisteViewModel, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Null);
            Assert.That(regnskabslisteViewModel.Regnskaber, Is.Not.Empty);
        }

        /// <summary>
        /// Tester, at RegnskabAdd rejser PropertyChanged ved tilføjelse af et regnskab til listen af regnskaber.
        /// </summary>
        [Test]
        public void TestAtRegnskabAddRejserPropertyChangedVedAddAfRegnskab()
        {
            var fixture = new Fixture();
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IRegnskabViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IRegnskabViewModel>()));

            var regnskabslisteViewModel = new RegnskabslisteViewModel(fixture.Create<IFinansstyringRepository>());
            Assert.That(regnskabslisteViewModel, Is.Not.Null);

            var eventCalledForRegnskaber = false;
            regnskabslisteViewModel.PropertyChanged += (s, e) =>
                {
                    Assert.That(s, Is.Not.Null);
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Null);
                    Assert.That(e.PropertyName, Is.Not.Empty);
                    if (string.Compare(e.PropertyName, "Regnskaber", StringComparison.Ordinal) == 0)
                    {
                        eventCalledForRegnskaber = true;
                    }
                };

            Assert.That(eventCalledForRegnskaber, Is.False);
            regnskabslisteViewModel.RegnskabAdd(fixture.Create<IRegnskabViewModel>());
            Assert.That(eventCalledForRegnskaber, Is.True);
        }
    }
}
