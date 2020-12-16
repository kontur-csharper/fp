using System.Collections.Generic;
using System.Linq;
using HomeExercise.Settings;
using ResultOf;

namespace HomeExercise
{
    public class WordsProcessor : IWordsProcessor
    {
        private readonly IFileProcessor fileProcessor;
        private readonly WordSettings settings;
        
        public WordsProcessor(IFileProcessor fileProcessor, WordSettings settings)
        {
            this.fileProcessor = fileProcessor;
            this.settings = settings;
        }

        public Result<List<IWord>> HandleWords()
        {
            var resultWords = fileProcessor.GetWords();

            return !resultWords.IsSuccess 
                ? Result.Fail<List<IWord>>(resultWords.Error) 
                : Result.Of(() => resultWords.Value.Select(w => WordHandle(w.Key, w.Value)).ToList());
        }
   
        private IWord WordHandle(string text, int frequency)
        {
            return new Word(text, frequency, settings.Font, frequency*settings.Coefficient);
        }
    }
}