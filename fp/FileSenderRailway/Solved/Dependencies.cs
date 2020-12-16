using System;
using System.Security.Cryptography.X509Certificates;
using ResultOf;

namespace FileSenderRailway.Solved
{
    public class Document
    {
        public Document(string name, byte[] content, DateTime created, string format)
        {
            Name = name;
            Created = created;
            Format = format;
            Content = content;
        }

        public string Name { get; }
        public DateTime Created { get; }
        public string Format { get; }
        public byte[] Content { get; }
        public Document WithContent(byte[] newContent) 
            => new Document(Name, newContent, Created, Format);
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
        public FileSendResult(FileContent file, string error)
        {
            File = file;
            Error = error;
        }

        public FileContent File { get; }
        public string Error { get; }
    }

    public interface ICryptographer
    {
        byte[] Sign(byte[] content, X509Certificate certificate);
    }

    public interface IRecognizer
    {
        Result<Document> Recognize(FileContent file);
    }

    public interface ISender
    {
        Result<None> Send(Document content);
    }
}