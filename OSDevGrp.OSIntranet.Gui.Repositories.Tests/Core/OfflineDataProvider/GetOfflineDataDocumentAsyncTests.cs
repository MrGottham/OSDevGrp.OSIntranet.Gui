using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Gui.Repositories.Exceptions;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces.Core.Options;
using System;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace OSDevGrp.OSIntranet.Gui.Repositories.Tests.Core.OfflineDataProvider
{
    [TestFixture]
    public class GetOfflineDataDocumentAsyncTests : TestBase
    {
        #region Private variables

        private Mock<IOptions<OfflineOptions>> _offlineOptionsMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _offlineOptionsMock = new Mock<IOptions<OfflineOptions>>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetOfflineDataDocument_WhenCalled_AssertValueWasCalledOnOfflineOptions()
        {
            IOfflineDataProvider sut = CreateSut();

            await sut.GetOfflineDataDocumentAsync();

            _offlineOptionsMock.Verify(m => m.Value);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetOfflineDataDocument_WhenOfflineDataDocumentIsValid_ReturnsNotNull()
        {
            IOfflineDataProvider sut = CreateSut();

            XmlDocument result = await sut.GetOfflineDataDocumentAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetOfflineDataDocument_WhenOfflineDataDocumentIsValid_ReturnsOfflineDataDocumentFromOfflineOptions()
        {
            XmlDocument offlineDataDocument = OfflineDataDocumentFactory.Build();
            IOfflineDataProvider sut = CreateSut(offlineDataDocument);

            XmlDocument result = await sut.GetOfflineDataDocumentAsync();

            Assert.That(result, Is.SameAs(offlineDataDocument));
        }

        [Test]
        [Category("UnitTest")]
        public void GetOfflineDataDocument_WhenOfflineDataDocumentIsInvalid_ThrowsIntranetGuiUserFriendlyException()
        {
            IOfflineDataProvider sut = CreateSut(BuildInvalidOfflineDataDocument());

            IntranetGuiUserFriendlyException? result = Assert.ThrowsAsync<IntranetGuiUserFriendlyException>(sut.GetOfflineDataDocumentAsync);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetOfflineDataDocument_WhenOfflineDataDocumentIsInvalid_ThrowsIntranetGuiUserFriendlyExceptionWhereInnerExceptionIsNotNull()
        {
            IOfflineDataProvider sut = CreateSut(BuildInvalidOfflineDataDocument());

            IntranetGuiUserFriendlyException? result = Assert.ThrowsAsync<IntranetGuiUserFriendlyException>(sut.GetOfflineDataDocumentAsync);

            Assert.That(result?.InnerException, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetOfflineDataDocument_WhenOfflineDataDocumentIsInvalid_ThrowsIntranetGuiUserFriendlyExceptionWhereInnerExceptionIsXmlSchemaValidationException()
        {
            IOfflineDataProvider sut = CreateSut(BuildInvalidOfflineDataDocument());

            IntranetGuiUserFriendlyException? result = Assert.ThrowsAsync<IntranetGuiUserFriendlyException>(sut.GetOfflineDataDocumentAsync);

            Assert.That(result?.InnerException, Is.TypeOf<XmlSchemaValidationException>());
        }

        [Test]
        [Category("UnitTest")]
        public void GetOfflineDataDocument_WhenXmlExceptionHasBeenThrown_ThrowsIntranetGuiSystemException()
        {
            XmlException exception = _fixture.Create<XmlException>();
            IOfflineDataProvider sut = CreateSut(exception: exception);

            IntranetGuiSystemException? result = Assert.ThrowsAsync<IntranetGuiSystemException>(sut.GetOfflineDataDocumentAsync);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetOfflineDataDocument_WhenXmlExceptionHasBeenThrown_ThrowsIntranetGuiSystemExceptionWhereInnerExceptionIsNotNull()
        {
            XmlException exception = _fixture.Create<XmlException>();
            IOfflineDataProvider sut = CreateSut(exception: exception);

            IntranetGuiSystemException? result = Assert.ThrowsAsync<IntranetGuiSystemException>(sut.GetOfflineDataDocumentAsync);

            Assert.That(result?.InnerException, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetOfflineDataDocument_WhenXmlExceptionHasBeenThrown_ThrowsIntranetGuiSystemExceptionWhereInnerExceptionIsXmlElement()
        {
            XmlException exception = _fixture.Create<XmlException>();
            IOfflineDataProvider sut = CreateSut(exception: exception);

            IntranetGuiSystemException? result = Assert.ThrowsAsync<IntranetGuiSystemException>(sut.GetOfflineDataDocumentAsync);

            Assert.That(result?.InnerException, Is.TypeOf<XmlException>());
        }

        [Test]
        [Category("UnitTest")]
        public void GetOfflineDataDocument_WhenXmlExceptionHasBeenThrown_ThrowsIntranetGuiSystemExceptionWhereInnerExceptionIsEqualToThrownXmlException()
        {
            XmlException exception = _fixture.Create<XmlException>();
            IOfflineDataProvider sut = CreateSut(exception: exception);

            IntranetGuiSystemException? result = Assert.ThrowsAsync<IntranetGuiSystemException>(sut.GetOfflineDataDocumentAsync);

            Assert.That(result?.InnerException, Is.EqualTo(exception));
        }

        private IOfflineDataProvider CreateSut(XmlDocument? offlineDataDocument = null, Exception? exception = null)
        {
            if (exception != null)
            {
                _offlineOptionsMock.Setup(m => m.Value)
                    .Throws(exception);
            }
            else
            {
                _offlineOptionsMock.Setup(m => m.Value)
                    .Returns(BuildOfflineOptions(offlineDataDocument));
            }

            return new Repositories.Core.OfflineDataProvider(_offlineOptionsMock.Object);
        }

        private static OfflineOptions BuildOfflineOptions(XmlDocument? offlineDataDocument = null)
        {
            return new OfflineOptions
            {
                OfflineDataDocument = offlineDataDocument ?? OfflineDataDocumentFactory.Build()
            };
        }

        private static XmlDocument BuildInvalidOfflineDataDocument()
        {
            XmlDocument document = OfflineDataDocumentFactory.Build();
            if (document.DocumentElement == null)
            {
                throw new XmlException("No root element was added to the XML document.");
            }

            XmlElement invalidElement = document.CreateElement("InvalidNode", document.DocumentElement.NamespaceURI);
            document.DocumentElement.AppendChild(invalidElement);

            return document;
        }
    }
}