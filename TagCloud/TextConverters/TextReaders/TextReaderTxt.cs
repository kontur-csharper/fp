using System;
using System.IO;
using ResultOf;

namespace TagCloud.TextConverters.TextReaders
{
    public class TextReaderTxt : ITextReader
    {
       public string Extension { get => ".txt"; }

        public Result<string> ReadText(string path)
        {
            try
            {
                using var sr = File.OpenText(path);
                return new Result<string>(null, sr.ReadToEnd());
            }
            catch(Exception e)
            {
                return new Result<string>($"Can't read file, error: {e.Message}");
            }
        }
    }
}
