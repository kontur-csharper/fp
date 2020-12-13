using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TagsCloudContainer
{
    public class AppSettingsValidator
    {
        public AppSettings Settings { get; }

        public AppSettingsValidator(AppSettings settings)
        {
            Settings = settings;
        }

        public AppSettingsValidator ValidateTextFilePath(string[] supportedTextFormats)
        {
            if (!File.Exists(Settings.PathToFile))
                throw new ArgumentException($"The specified file does not exist: {Settings.PathToFile}");

            CheckForItem(Path.GetExtension(Settings.PathToFile), supportedTextFormats,
                $"Format of this file isn't supported: {Settings.PathToFile}\n" +
                $"Supported formats: {string.Join(", ", supportedTextFormats)}");

            return this;
        }

        public AppSettingsValidator ValidatePartsOfSpeech()
        {
            var partsOfSpeech = Enum.GetNames(typeof(PartOfSpeech));
            foreach (var name in Settings.ExcludedPartsOfSpeechNames)
                CheckForItemIgnoreCase(name, partsOfSpeech, $"Incorrect name of part of speech: {name}");

            return this;
        }

        public AppSettingsValidator ValidateColors()
        {
            var colors = Enum.GetNames(typeof(KnownColor));

            CheckForItemIgnoreCase(Settings.BackgroundColorName, colors,
                $"Incorrect background color: {Settings.BackgroundColorName}");

            CheckForItemIgnoreCase(Settings.FontColorName, colors,
                $"Incorrect font color: {Settings.FontColorName}");

            return this;
        }

        public AppSettingsValidator ValidateFont()
        {
            var fontNames = FontFamily.Families.Select(family => family.Name);
            CheckForItemIgnoreCase(Settings.FontName, fontNames, $"Incorrect font: {Settings.FontName}");

            return this;
        }

        public AppSettingsValidator ValidateImagePath(string[] validExtensions)
        {
            var ext = Path.GetExtension(Settings.ImagePath);
            CheckForItem(ext, validExtensions, $"Incorrect image format: {ext}");

            return this;
        }

        private static void CheckForItem(string item, IEnumerable<string> allItems, string errorMessage)
        {
            if (!allItems.Contains(item))
                throw new ArgumentException(errorMessage);
        }
        
        private static void CheckForItemIgnoreCase(string item, IEnumerable<string> allItems, string errorMessage)
        {
            if (!allItems.Contains(item, StringComparer.OrdinalIgnoreCase))
                throw new ArgumentException(errorMessage);
        }
    }
}