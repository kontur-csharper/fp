using System;
using System.Collections.Generic;
using System.Linq;
using ResultOf;

namespace TagCloud.TextConverters.TextProcessors
{
    public static class TextProcessorAssosiation
    {
        public const string paragraph = "paragraph";
        public const string words = "words";
        private static readonly Dictionary<string, ITextProcessor> processors =
            new Dictionary<string, ITextProcessor>
            {
                [paragraph] = new ParagraphTextProcessor(),
                [words] = new WordsTextProcessor()
            };
        private static readonly HashSet<string> names = processors.Keys.ToHashSet();

        public static Result<ITextProcessor> GetProcessor(string name)
        {
            if (!processors.ContainsKey(name))
            {
                return new Result<ITextProcessor>($"doesn't have processor with name {name}\n" +
                    $"List of text processor names:\n{string.Join('\n', names)}");
            }
            return new Result<ITextProcessor>(null, processors[name]);
        }
    }
}
