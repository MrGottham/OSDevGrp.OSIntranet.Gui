using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Kernel;
using Moq;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests
{
    internal static class RepositoryBuilder
    {
        internal static IFinansstyringRepository BuildFinansstyringRepository(this ISpecimenBuilder fixture, IEnumerable<IRegnskabModel> regnskabModelCollection = null, IEnumerable<IKontoModel> kontoModelCollection = null, IKontoModel kontoModel = null, IEnumerable<IBudgetkontoModel> budgetkontoModelCollection = null, IBudgetkontoModel budgetkontoModel = null, IEnumerable<IAdressekontoModel> debitorModelCollection = null, IEnumerable<IAdressekontoModel> kreditorModelCollection = null, IEnumerable<IAdressekontoModel> adressekontoModelCollection = null, IAdressekontoModel adressekontoModel = null, IEnumerable<IBogføringslinjeModel> bogføringslinjeModelCollection = null, IBogføringslinjeModel nyBogføringslinjeModel = null, IEnumerable<IBogføringsresultatModel> bogføringsresultatModelCollection = null, IEnumerable<IKontogruppeModel> kontogruppeModelCollection = null, IEnumerable<IBudgetkontogruppeModel> budgetkontogruppeModelCollection = null, Action<Mock<IFinansstyringRepository>> setupCallback = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.BuildFinansstyringRepositoryMock(regnskabModelCollection, kontoModelCollection, kontoModel, budgetkontoModelCollection, budgetkontoModel, debitorModelCollection, kreditorModelCollection, adressekontoModelCollection, adressekontoModel, bogføringslinjeModelCollection, nyBogføringslinjeModel, bogføringsresultatModelCollection, kontogruppeModelCollection, budgetkontogruppeModelCollection, setupCallback).Object;
        }

        internal static Mock<IFinansstyringRepository> BuildFinansstyringRepositoryMock(this ISpecimenBuilder fixture, IEnumerable<IRegnskabModel> regnskabModelCollection = null, IEnumerable<IKontoModel> kontoModelCollection = null, IKontoModel kontoModel = null, IEnumerable<IBudgetkontoModel> budgetkontoModelCollection = null, IBudgetkontoModel budgetkontoModel = null, IEnumerable<IAdressekontoModel> debitorModelCollection = null, IEnumerable<IAdressekontoModel> kreditorModelCollection = null, IEnumerable<IAdressekontoModel> adressekontoModelCollection = null, IAdressekontoModel adressekontoModel = null, IEnumerable<IBogføringslinjeModel> bogføringslinjeModelCollection = null, IBogføringslinjeModel nyBogføringslinjeModel = null, IEnumerable<IBogføringsresultatModel> bogføringsresultatModelCollection = null, IEnumerable<IKontogruppeModel> kontogruppeModelCollection = null, IEnumerable<IBudgetkontogruppeModel> budgetkontogruppeModelCollection = null, Action<Mock<IFinansstyringRepository>> setupCallback = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Mock<IFinansstyringRepository> mock = new Mock<IFinansstyringRepository>();
            mock.Setup(m => m.RegnskabslisteGetAsync())
                .Returns(Task.FromResult(regnskabModelCollection ?? fixture.BuildRegnskabModelCollection()));
            mock.Setup(m => m.KontoplanGetAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(kontoModelCollection ?? fixture.BuildKontoModelCollection()));
            mock.Setup(m => m.KontoGetAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(kontoModel ?? fixture.BuildKontoModel()));
            mock.Setup(m => m.BudgetkontoplanGetAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(budgetkontoModelCollection ?? fixture.BuildBudgetkontoModelCollection()));
            mock.Setup(m => m.BudgetkontoGetAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(budgetkontoModel ?? fixture.BuildBudgetkontoModel()));
            mock.Setup(m => m.DebitorlisteGetAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(debitorModelCollection ?? fixture.BuildAdressekontoModelCollection()));
            mock.Setup(m => m.KreditorlisteGetAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(kreditorModelCollection ?? fixture.BuildAdressekontoModelCollection()));
            mock.Setup(m => m.AdressekontolisteGetAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(adressekontoModelCollection ?? fixture.BuildAdressekontoModelCollection()));
            mock.Setup(m => m.AdressekontoGetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(adressekontoModel ?? fixture.BuildAdressekontoModel()));
            mock.Setup(m => m.BogføringslinjerGetAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(Task.FromResult(bogføringslinjeModelCollection ?? fixture.BuildBogføringslinjeModelCollection()));
            mock.Setup(m => m.BogføringslinjeCreateNewAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .Returns(Task.FromResult(nyBogføringslinjeModel ?? fixture.BuildBogføringslinjeModel()));
            mock.Setup(m => m.BogførAsync(It.IsAny<IBogføringslinjeModel[]>()))
                .Returns(Task.FromResult(bogføringsresultatModelCollection ?? fixture.BuildBogføringsresultatModelCollection()));
            mock.Setup(m => m.KontogruppelisteGetAsync())
                .Returns(Task.FromResult(kontogruppeModelCollection ?? fixture.BuildKontogruppeModelCollection()));
            mock.Setup(m => m.BudgetkontogruppelisteGetAsync())
                .Returns(Task.FromResult(budgetkontogruppeModelCollection ?? fixture.BuildBudgetkontogruppeModelCollection()));

            if (setupCallback == null)
            {
                return mock;
            }

            setupCallback(mock);

            return mock;
        }
    }
}