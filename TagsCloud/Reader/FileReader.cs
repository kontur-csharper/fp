using System;
using System.IO;
using ResultPattern;
using TagsCloud.Settings.SettingsForStorage;

namespace TagsCloud.Reader
{
    public class FileReader : IFileReader
    {
        private readonly IStorageSettings _storageSettings;

        public FileReader(IStorageSettings storageSettings)
        {
            _storageSettings = storageSettings;
        }

        public Result<string> GetTextFromFile()
        {
            try
            {
                using var reader = new StreamReader(_storageSettings.PathToCustomText);
                return new Result<string>(null, reader.ReadToEnd());
            }
            catch (Exception)
            {
                return new Result<string>("Can't read the file");
            }
        }
    }
}