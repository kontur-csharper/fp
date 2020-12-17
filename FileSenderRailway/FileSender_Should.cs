using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace FileSenderRailway
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    public class FileSender_Should
    {
        [SetUp]
        public void SetUp()
        {
            cryptographer = A.Fake<ICryptographer>();
            sender = A.Fake<ISender>();
            recognizer = A.Fake<IRecognizer>();
            fileSender = new FileSender(cryptographer, sender, recognizer, () => now);
        }

        private FileSender fileSender;
        private ICryptographer cryptographer;
        private ISender sender;
        private IRecognizer recognizer;

        private readonly FileContent file = new FileContent(Guid.NewGuid().ToString("N"), Guid.NewGuid().ToByteArray());
        private readonly DateTime now = new DateTime(2000, 01, 01);
        private readonly X509Certificate certificate = new X509Certificate();

        private void PrepareDocument(FileContent content, byte[] signedContent, DateTime created, string format)
        {
            var document = new Document(content.Name, content.Content, created, format);
            A.CallTo(() => recognizer.Recognize(content)).Returns(document);
            A.CallTo(() => cryptographer.Sign(content.Content, certificate)).Returns(signedContent);
        }

        private void VerifyErrorOnPrepareFile(FileContent fileContent, X509Certificate x509Certificate)
        {
            var res = fileSender
                .SendFiles(new[] {fileContent}, x509Certificate)
                .Single();
            res.IsSuccess.Should().BeFalse();
            Approvals.Verify(res.Error);
        }

        private static byte[] SomeByteArray()
        {
            return Guid.NewGuid().ToByteArray();
        }

        [Test]
        public void BeOk_WhenGoodFormat(
            [Values("4.0", "3.1")] string format,
            [Values(0, 30)] int daysBeforeNow)
        {
            var signed = SomeByteArray();
            PrepareDocument(file, signed, now.AddDays(-daysBeforeNow), format);

            fileSender.SendFiles(new[] {file}, certificate)
                .Should().BeEquivalentTo(new FileSendResult(file));
            A.CallTo(() => sender.Send(A<Document>.That.Matches(d => d.Content == signed)))
                .MustHaveHappened();
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
            {
                VerifyErrorOnPrepareFile(file, certificate);
            }
        }


        [Test]
        public void Fail_WhenNotRecognized()
        {
            A.CallTo(() => recognizer.Recognize(file))
                .Throws(new FormatException("Can't recognize"));

            VerifyErrorOnPrepareFile(file, certificate);
        }
    }
}