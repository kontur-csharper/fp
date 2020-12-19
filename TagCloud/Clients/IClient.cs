using TagCloud.Visualization;

namespace TagCloud.Clients
{
    internal interface IClient
    {
        public void Run();

        public void Visualizate(string text, string picturePath);
    }
}
