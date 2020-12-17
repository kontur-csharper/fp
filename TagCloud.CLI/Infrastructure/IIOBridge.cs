namespace TagCloud.CLI.Infrastructure
{
    public interface IIOBridge
    {
        public void Write(string line);
        public void WriteLine(string line);
        public string Read();
        public void Next();
    }
}