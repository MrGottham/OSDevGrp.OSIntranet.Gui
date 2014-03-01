using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Models.Finansstyring;
using OSDevGrp.OSIntranet.Gui.Resources;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Gui.Models.Tests.Finansstyring
{
    /// <summary>
    /// Tester modellen indeholdende grundlæggende kontooplysninger.
    /// </summary>
    [TestFixture]
    public class KontoModelBaseTests
    {
        /// <summary>
        /// Egen klasse til test af modellen indeholdende grundlæggende kontooplysninger.
        /// </summary>
        private class MyKontoModel : KontoModelBase
        {
            #region Constructor

            /// <summary>
            /// Danner egen klasse til test af modellen indeholdende grundlæggende kontooplysninger.
            /// </summary>
            /// <param name="regnskabsnummer">Regnskabsnummer, som kontoen er tilknyttet.</param>
            /// <param name="kontonummer">Kontonummer.</param>
            /// <param name="kontonavn">Kontonavn.</param>
            /// <param name="statusDato">Statusdato for opgørelse af kontoen.</param>
            public MyKontoModel(int regnskabsnummer, string kontonummer, string kontonavn, DateTime statusDato)
                : base(regnskabsnummer, kontonummer, kontonavn, statusDato)
            {
            }

            #endregion
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentException ved illegale regnskabsnumre.
        /// </summary>
        [Test]
        [TestCase(-1024)]
        [TestCase(-1)]
        [TestCase(0)]
        public void TestAtConstructorKasterArgumentExceptionVedIllegalRegnskabsnummer(int illegalValue)
        {
            var fixture = new Fixture();
            fixture.Customize<DateTime>(e => e.FromFactory(() => DateTime.Now));

            var exception = Assert.Throws<ArgumentException>(() => new MyKontoModel(illegalValue, fixture.Create<string>(), fixture.Create<string>(), fixture.Create<DateTime>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.StringStarting(Resource.GetExceptionMessage(ExceptionMessage.IllegalArgumentValue, "regnskabsnummer", illegalValue)));
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("regnskabsnummer"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
