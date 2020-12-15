using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ResultOf;
using TagsCloudContainer.TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.TagsCloudContainer
{
    public class TextParser : ITextParser
    {
        private readonly IWordValidator wordValidator;

        public TextParser(IWordValidator wordValidator)
        {
            this.wordValidator = wordValidator;
        }

        public List<string> GetAllWords(string text)
        {
            Result.Ok(text)
                .Then(ValidateText)
                .OnFail(e => throw new ArgumentException(e, nameof(text)));

            var matches = Regex.Matches(text, @"\b\w+\b");
            var wordsEntry = matches
                .Select(x => x.Value.ToLower())
                .Where(x => wordValidator.IsValidWord(x))
                .ToList();

            return wordsEntry;
        }

        private Result<string> ValidateText(string text)
        {
            return text is null
                ? Result.Fail<string>("Text is null")
                : Result.Ok(text);
        }
    }
}