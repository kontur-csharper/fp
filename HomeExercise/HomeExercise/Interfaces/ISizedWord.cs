using System.Drawing;

namespace HomeExercise
{
    public interface ISizedWord
    {
        string Text { get; }
        FontFamily Font { get; }
        Rectangle Rectangle { get; }
        int Size { get; }
    }
}