using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using ResultOf;

namespace FileSenderRailway.Solved
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    public class FileSender_Should
    {
        private FileSender fileSender;
        private ICryptographer cryptographer;
        private IRecognizer recognizer;

        [SetUp]
        public void SetUp()
        {
            Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);
            cryptographer = A.Fake<ICryptographer>();
            recognizer = A.Fake<IRecognizer>();
            fileSender = new FileSender(cryptographer, A.Fake<ISender>(), recognizer, () => now);
        }

        private readonly FileContent file = new FileContent(Guid.NewGuid().ToString("N"), Guid.NewGuid().ToByteArray());
        private readonly DateTime now = new DateTime(2000, 01, 01);
        private readonly X509Certificate certificate = new X509Certificate();

        [Test]
        public void BeOk_WhenGoodFormat(
            [Values("4.0", "3.1")]string format,
            [Values(0, 30)]int daysBeforeNow)
        {
            var signedContent = SomeByteArray();
            var document = PrepareDocument(file, signedContent, now.AddDays(-daysBeforeNow), format);
            var expectedDocument = document.WithContent(signedContent);
            var result = fileSender.PrepareFileToSend(file, certificate);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(expectedDocument);
        }

        [Test]
        public void Fail_WhenNotRecognized()
        {
            A.CallTo(() => recognizer.Recognize(file))
                .Returns(Result.Fail<Document>("Can't recognize"));

            VerifyErrorOnPrepareFile(file, certificate);
        }

        [TestCase("1.0", 0)]
        [TestCase("4.0", 32)]
        [TestCase("3.1", 32)]
        [TestCase("wrong", 32)]
        [Test]
        public void Fail_WhenBadFormatOrTimestamp(string format, int daysBeforeNow)
        {
            PrepareDocument(file, null, now.AddDays(-daysBeforeNow), format);
            using (ApprovalResults.ForScenario(format, daysBeforeNow))
                VerifyErrorOnPrepareFile(file, certificate);
        }

        private Document PrepareDocument(FileContent fileToPrepare, byte[] signed, DateTime created, string format)
        {
            var document = new Document(fileToPrepare.Name, fileToPrepare.Content, created, format);
            A.CallTo(() => recognizer.Recognize(fileToPrepare)).Returns(Result.Ok(document));
            A.CallTo(() => cryptographer.Sign(fileToPrepare.Content, certificate)).Returns(signed);
            return document;
        }

        private void VerifyErrorOnPrepareFile(FileContent fileContent, X509Certificate x509Certificate)
        {
            var res = fileSender.PrepareFileToSend(fileContent, x509Certificate);
            res.IsSuccess.Should().BeFalse();
            Approvals.Verify(res.Error);
        }

        private static byte[] SomeByteArray()
        {
            return Guid.NewGuid().ToByteArray();
        }
    }
}