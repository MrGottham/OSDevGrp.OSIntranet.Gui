using System;
using System.Windows.Input;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Gui.Resources;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Interfaces.Finansstyring;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests.Finansstyring
{
    /// <summary>
    /// Tester ViewModel indeholdende grundlæggende kontooplysninger.
    /// </summary>
    [TestFixture]
    public class KontoViewModelBaseTests
    {
        /// <summary>
        /// Egen klasse til test af ViewModel indeholdende grundlæggende kontooplysninger.
        /// </summary>
        private class MyKontoViewModel : KontoViewModelBase<IKontoModelBase, IKontogruppeViewModelBase>
        {
            #region Constructor

            /// <summary>
            /// Danner egen klasse til test af ViewModel indeholdende grundlæggende kontooplysninger.
            /// </summary>
            /// <param name="regnskabViewModel">ViewModel for regnskabet, som kontoen er tilknyttet.</param>
            /// <param name="kontoModel">Model indeholdende grundlæggende kontooplysninger.</param>
            /// <param name="kontogruppeViewModel">ViewModel for kontogruppen.</param>
            /// <param name="displayName">Navn for ViewModel, som kan benyttes til visning i brugergrænsefladen.</param>
            /// <param name="image">Billede, der illustrerer en kontoen.</param>
            /// <param name="finansstyringRepository">Implementering af repositoryet til finansstyring.</param>
            /// <param name="exceptionHandlerViewModel">Implementering af ViewModel for exceptionhandleren.</param>
            public MyKontoViewModel(IRegnskabViewModel regnskabViewModel, IKontoModelBase kontoModel, IKontogruppeViewModelBase kontogruppeViewModel, string displayName, byte[] image, IFinansstyringRepository finansstyringRepository, IExceptionHandlerViewModel exceptionHandlerViewModel)
                : base(regnskabViewModel, kontoModel, kontogruppeViewModel, displayName, image, finansstyringRepository, exceptionHandlerViewModel)
            {
            }

            #endregion

            #region Properties

            /// <summary>
            /// Kommando til genindlæsning og opdatering.
            /// </summary>
            public override ICommand RefreshCommand
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            #endregion
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ViewModel for regnskabet er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisRegnskabViewModelErNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IKontoModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontoModel>()));
            fixture.Customize<IKontogruppeViewModelBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IKontogruppeViewModelBase>()));
            fixture.Customize<IFinansstyringRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<IFinansstyringRepository>()));
            fixture.Customize<IExceptionHandlerViewModel>(e => e.FromFactory(() => MockRepository.GenerateMock<IExceptionHandlerViewModel>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new MyKontoViewModel(null, fixture.Create<IKontoModel>(), fixture.Create<IKontogruppeViewModelBase>(), fixture.Create<string>(), Resource.GetEmbeddedResource("Images.Konto.png"), fixture.Create<IFinansstyringRepository>(), fixture.Create<IExceptionHandlerViewModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("regnskabViewModel"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
