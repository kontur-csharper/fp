namespace ConwaysGameOfLife
{
    public interface IReadonlyField
    {
        int Width { get; }
        int Height { get; }
        bool IsAlive(int x, int y);
    }
}