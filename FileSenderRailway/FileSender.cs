using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ResultOf;

namespace FileSenderRailway
{
    public class FileSender
    {
        private readonly Func<byte[], X509Certificate, byte[]> cryptographer;
        private readonly Func<FileContent, Document> recognize;
        private readonly Func<DateTime> now;
        private readonly Action<Document> sendFile;

        public FileSender(
            Func<byte[], X509Certificate, byte[]> cryptographer,
            Action<Document> sendFile,
            Func<FileContent, Document> recognize,
            Func<DateTime> now)
        {
            this.cryptographer = cryptographer;
            this.sendFile = sendFile;
            this.recognize = recognize;
            this.now = now;
        }

        public IEnumerable<FileSendResult> SendFiles(FileContent[] files, X509Certificate certificate) =>
            files.Select(file => new FileSendResult(file, ParseDocument(file, certificate).Then(SendFile).Error));

        private Result<Document> ParseDocument(FileContent file, X509Certificate certificate) => 
            Result.Of(() => recognize(file))
                .Then(ValidateFormatVersion)
                .Then(ValidateTimestamp)
                .Then(d => SignDocContent(d, certificate))
                .RefineError("Can't prepare file to send");

        private Result<Document> SignDocContent(Document document, X509Certificate certificate) =>
            Result.Of(() => document.SetContent(cryptographer.Invoke(document.Content, certificate)));

        private Result<None> SendFile(Document doc) => 
            Result.OfAction(() => sendFile(doc)).RefineError("Can't send");
        
        private Result<Document> ValidateFormatVersion(Document doc)
        {
            if (doc.Format != "4.0" && doc.Format != "3.1")
                return Result.Fail<Document>($"Invalid format version");
            return doc.AsResult();
        }

        private Result<Document> ValidateTimestamp(Document doc)
        {
            var oneMonthBefore = now().AddMonths(-1);
            if (doc.Created <= oneMonthBefore)
                return Result.Fail<Document>($"Too old document");
            return doc.AsResult();
        }
    }
}