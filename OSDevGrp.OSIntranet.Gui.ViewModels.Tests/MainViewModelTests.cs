using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.ViewModels.Core;
using OSDevGrp.OSIntranet.Gui.ViewModels.Finansstyring;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests
{
    /// <summary>
    /// Tester ViewModel til binding mod Views.
    /// </summary>
    [TestFixture]
    public class MainViewModelTests
    {
        /// <summary>
        /// Tester, at konstruktøren initerer en ViewModel til binding mod Views.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererMainViewModel()
        {
            var mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);
            Assert.That(mainViewModel.DisplayName, Is.Not.Null);
            Assert.That(mainViewModel.DisplayName, Is.Not.Empty);
            Assert.That(mainViewModel.DisplayName, Is.EqualTo(mainViewModel.GetType().Name));
        }

        /// <summary>
        /// Tester, at getteren til Regnskabsliste returnerer ViewModel for en liste af regnskaber.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetterReturnererRegnskabslisteViewModel()
        {
            var mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            var regnskabslisteViewModel = mainViewModel.Regnskabsliste;
            Assert.That(regnskabslisteViewModel, Is.Not.Null);
            Assert.That(regnskabslisteViewModel, Is.TypeOf<RegnskabslisteViewModel>());
        }

        /// <summary>
        /// Tester, at getteren til ExceptionHandler returnerer ViewModel for en exceptionhandler.
        /// </summary>
        [Test]
        public void TestAtExceptionHandlerGetterReturnererExceptionHandlerViewModel()
        {
            var mainViewModel = new MainViewModel();
            Assert.That(mainViewModel, Is.Not.Null);

            var exceptionHandlerViewModel = mainViewModel.ExceptionHandler;
            Assert.That(exceptionHandlerViewModel, Is.Not.Null);
            Assert.That(exceptionHandlerViewModel, Is.TypeOf<ExceptionHandlerViewModel>());
        }
    }
}
