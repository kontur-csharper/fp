using System.IO;
using System.Linq;
using TagsCloud.Factory;
using TagsCloud.ResultOf;
using TagsCloud.TextProcessing.WordsConfig;

namespace TagsCloud.TextProcessing.TextReaders
{
    public class ReadersFactory : ServiceFactory<IWordsReader>
    {
        private readonly IWordConfig wordsConfig;

        public ReadersFactory(IWordConfig wordsConfig)
        {
            this.wordsConfig = wordsConfig;
        }

        public override Result<IWordsReader> Create()
        {
            var reader = services.FirstOrDefault(pair => pair.Value().CanRead(wordsConfig.Path)).Value;
            if (!File.Exists(wordsConfig.Path))
                return Result.Fail<IWordsReader>($"Path: {wordsConfig.Path} don't exists");

            return Result.Of(reader, $"This file type {Path.GetExtension(wordsConfig.Path)} is not supported");
        }
    }
}
