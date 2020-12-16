using System.Collections.Generic;
using ResultOf;

namespace HomeExercise
{
    public interface IWordsProcessor
    {
        Result<List<IWord>> HandleWords(); 
    }
}