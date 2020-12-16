using System.Drawing;

namespace HomeExercise
{
    public interface ISpiral
    {
        Point Center { get; }
        Point GetNextPoint();
    }
}