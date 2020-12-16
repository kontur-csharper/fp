using System.Collections.Generic;
using ResultOf;

namespace HomeExercise
{
    public interface IFileProcessor
    {
        Result<Dictionary<string, int>> GetWords();
    }
}