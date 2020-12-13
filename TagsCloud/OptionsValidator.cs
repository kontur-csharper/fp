using System;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text.RegularExpressions;
using TagsCloud.Options;
using TagsCloud.ResultOf;

namespace TagsCloud
{
    public static class OptionsValidator
    {
        private static readonly Regex ColorParser =
            new Regex(@"argb\((?<alpha>\d{1,3}),(?<red>\d{1,3}),(?<green>\d{1,3}),(?<blue>\d{1,3})\)");

        public static Result<IImageOptions> ValidateImageOptions(CommandLineOptions options)
        {
            int.TryParse(options.Width, out var width);
            int.TryParse(options.Height, out var height);

            return width > 0 && height > 0
                ? Result.Ok((IImageOptions)new ImageOptions(width, height, options.OutputDirectory, options.OutputFileName,
                    options.OutputFileExtension))
                : Result.Fail<IImageOptions>("Incorrect image size.");
        }

        public static Result<IFontOptions> ValidateFontOptions(CommandLineOptions options)
        {
            var color = StringToArgbColor(options.FontColor);
            var family = ValidateFontFamily(options.FontFamily);

            return color.IsSuccess && family.IsSuccess
                ? Result.Ok((IFontOptions)new FontOptions(family.Value, color.Value))
                : Result.Fail<IFontOptions>(!color.IsSuccess ? color.Error : family.Error);
        }

        public static Result<IFilterOptions> ValidateFilterOptions(CommandLineOptions options)
            => new FilterOptions(options.MystemLocation, options.BoringWords);

        private static Result<Color> StringToArgbColor(string s)
        {
            var random = new Random();
            if (s == "random")
                return Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));

            return Result.Of(() => ColorParser.Match(s).Groups)
                .Then(groups => Color.FromArgb(int.Parse(groups["alpha"].Value), int.Parse(groups["red"].Value),
                    int.Parse(groups["green"].Value), int.Parse(groups["blue"].Value)))
                .ReplaceError(e => "Incorrect font color.");
        }

        private static Result<string> ValidateFontFamily(string fontName)
        {
            return new InstalledFontCollection().Families.Any(family => family.Name == fontName)
                ? Result.Ok(fontName)
                : Result.Fail<string>($"Font family {fontName} doesn't exist.");
        }
    }
}