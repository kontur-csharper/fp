using ResultOf;
using TagsCloudContainer.Infrastructure.TextAnalyzer;
using YandexMystem.Wrapper;

namespace TagsCloudContainer.App.TextAnalyzer
{
    public class ToInitialFormNormalizer : IWordNormalizer
    {
        private readonly Mysteam mysteam;

        public ToInitialFormNormalizer(Mysteam mysteam)
        {
            this.mysteam = mysteam;
        }

        public Result<string> NormalizeWord(string word)
        {
            return Result
                .Of(() => mysteam.GetWords(word))
                .Then(words => words[0].Lexems[0].Lexeme)
                .ReplaceError(str => $"Can't normalize word {word} to initial form");
        }
    }
}
