using System.IO;
using System.Linq;

namespace TagsCloudContainer.Common
{
    public class FilesSettings : ISettings
    {
        private static readonly string defaultTextFilePath = @"..\..\text.txt";
        private static readonly string defaultBoringWordsFilePath = @"..\..\boring words.txt";
        private readonly string[] availableFileExtensions = {".doc", ".docx", ".txt"};
        public string TextFilePath { get; set; } = defaultTextFilePath;
        public string BoringWordsFilePath { get; set; } = defaultBoringWordsFilePath;

        public Result<ISettings> CheckSettings()
        {
            if (!File.Exists(TextFilePath))
            {
                var badPath = TextFilePath;
                TextFilePath = defaultTextFilePath;
                return new Result<ISettings>($"Файла {badPath} не существует");
            }

            if (!availableFileExtensions.Contains(Path.GetExtension(TextFilePath)))
            {
                TextFilePath = defaultTextFilePath;
                return new Result<ISettings>("Данный формат текстового файла не поддерживается");
            }

            if (!File.Exists(BoringWordsFilePath))
            {
                var badPath = BoringWordsFilePath;
                BoringWordsFilePath = defaultBoringWordsFilePath;
                return new Result<ISettings>($"Файла {badPath} не существует");
            }

            if (!availableFileExtensions.Contains(Path.GetExtension(BoringWordsFilePath)))
            {
                BoringWordsFilePath = defaultBoringWordsFilePath;
                return new Result<ISettings>("Данный формат текстового файла не поддерживается");
            }

            return new Result<ISettings>(null, this);
        }
    }
}