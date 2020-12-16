namespace ConwaysGameOfLife
{
    public interface IGameUi
    {
        void UpdateAll(IReadonlyField field);
        void UpdateCell(int x, int y, bool alive);
    }
}