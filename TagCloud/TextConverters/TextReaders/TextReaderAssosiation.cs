using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ResultOf;

namespace TagCloud.TextConverters.TextReaders
{
    public static class TextReaderAssosiation
    {
        private static readonly Dictionary<string, ITextReader> textReaders =
            new Dictionary<string, ITextReader>
            {
                [".txt"] = new TextReaderTxt()
            };

        private static readonly HashSet<string> extensions = textReaders.Keys.ToHashSet();

        public static Result<ITextReader> GetTextReader(string path) 
        {
            if (!File.Exists(path))
            {
                return new Result<ITextReader>("file doesnt exist");
            }
            var extension = Path.GetExtension(path);
            if (!textReaders.ContainsKey(extension))
            {
                return new Result<ITextReader>($"can't read file with extension {extension} \n" +
                    $"list of extensions to read: \n {string.Join('\n', extension)}");
            }
            return new Result<ITextReader>(null, textReaders[extension]);
        }
    }
}
