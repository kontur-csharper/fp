using System.IO;

namespace TagCloud.CLI.Infrastructure
{
    public class TextBridge : IIOBridge
    {
        public delegate void BridgeClearer();

        private readonly BridgeClearer clear;
        private readonly TextReader reader;
        private readonly TextWriter writer;

        public TextBridge(TextWriter writer, TextReader reader, BridgeClearer clear)
        {
            this.writer = writer;
            this.reader = reader;
            this.clear = clear;
        }

        public void Write(string line)
        {
            writer.Write(line);
        }

        public void WriteLine(string line)
        {
            writer.WriteLine(line);
        }

        public string Read()
        {
            return reader.ReadLine();
        }

        public void Next()
        {
            clear();
        }
    }
}