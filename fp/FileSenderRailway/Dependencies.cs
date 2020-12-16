using System;
using System.Security.Cryptography.X509Certificates;

namespace FileSenderRailway
{
    public interface ICryptographer
    {
        byte[] Sign(byte[] content, X509Certificate certificate);
    }

    public interface IRecognizer
    {
        /// <exception cref="FormatException">Not recognized</exception>
        Document Recognize(FileContent file);
    }

    public interface ISender
    {
        /// <exception cref="InvalidOperationException">Can't send</exception>
        void Send(Document document);
    }

    public class Document
    {
        public Document(string name, byte[] content, DateTime created, string format)
        {
            Name = name;
            Created = created;
            Format = format;
            Content = content;
        }

        public string Name { get; set; }
        public DateTime Created { get; set; }
        public string Format { get; set; }
        public byte[] Content { get; set; }
    }

    public class FileContent
    {
        public FileContent(string name, byte[] content)
        {
            Name = name;
            Content = content;
        }

        public string Name { get; }
        public byte[] Content { get; }
    }

    public class FileSendResult
    {
        public FileSendResult(FileContent file, string error = null)
        {
            File = file;
            Error = error;
        }

        public FileContent File { get; }
        public string Error { get; }
        public bool IsSuccess => Error == null;
    }
}