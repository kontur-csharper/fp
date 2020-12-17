using System;
using System.IO;
using TagCloud.Infrastructure.Settings.SettingsProviders;
using TagCloud.Infrastructure.Settings.UISettingsManagers.Interfaces;

namespace TagCloud.Infrastructure.Settings.UISettingsManagers
{
    public class FileSettingManager : IInputManager
    {
        private readonly Func<IFileSettingsProvider> fileSettingsProvider;

        public FileSettingManager(Func<IFileSettingsProvider> fileSettingsProvider)
        {
            this.fileSettingsProvider = fileSettingsProvider;
        }

        public string Title => "Input file";
        public string Help => "Type path to file to analyze";

        public Result<string> TrySet(string path)
        {
            if (!File.Exists(path)) return Result.Fail<string>($"{path} not found");

            fileSettingsProvider().Path = path;
            return Result.Ok("");
        }

        public string Get()
        {
            return fileSettingsProvider().Path;
        }
    }
}