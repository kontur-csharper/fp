using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace FileSenderRailway
{
    public class FileSender
    {
        private readonly ICryptographer cryptographer;
        private readonly IRecognizer recognizer;
        private readonly Func<DateTime> now;
        private readonly ISender sender;

        public FileSender(
            ICryptographer cryptographer,
            ISender sender,
            IRecognizer recognizer,
            Func<DateTime> now)
        {
            this.cryptographer = cryptographer;
            this.sender = sender;
            this.recognizer = recognizer;
            this.now = now;
        }

        public IEnumerable<FileSendResult> SendFiles(FileContent[] files, X509Certificate certificate)
        {
            foreach (var file in files)
            {
                string errorMessage = null;
                try
                {
                    Document doc = recognizer.Recognize(file);
                    if (!IsValidFormatVersion(doc))
                        throw new FormatException("Invalid format version");
                    if (!IsValidTimestamp(doc))
                        throw new FormatException("Too old document");
                    doc.Content = cryptographer.Sign(doc.Content, certificate);
                    sender.Send(doc);
                }
                catch (FormatException e)
                {
                    errorMessage = "Can't prepare file to send. " + e.Message;
                }
                catch (InvalidOperationException e)
                {
                    errorMessage = "Can't send. " + e.Message;
                }
                yield return new FileSendResult(file, errorMessage);
            }
        }

        private bool IsValidFormatVersion(Document doc)
        {
            return doc.Format == "4.0" || doc.Format == "3.1";
        }

        private bool IsValidTimestamp(Document doc)
        {
            var oneMonthBefore = now().AddMonths(-1);
            return doc.Created > oneMonthBefore;
        }
    }
}