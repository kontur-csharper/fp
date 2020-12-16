using System.Drawing;

namespace HomeExercise
{
    public interface IWord
    {
        string Text { get; }
        FontFamily Font { get; }
        int Size { get; }
    }
}