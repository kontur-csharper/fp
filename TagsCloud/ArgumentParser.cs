using System;
using System.IO;
using System.Drawing;
using ResultOf;

namespace TagsCloud
{
    public static class ArgumentParser
    {
        public static Result<string> CheckFilePath(string filePath)
        {
            if (!File.Exists(filePath))
                return Result.Fail<string>(
                    "ArgumentException: не удалось найти данный файл. Для подробностей --help");

            if (!filePath.EndsWith(".txt") && !filePath.EndsWith("docx"))
                return Result.Fail<string>(
                    "ArgumentException: расширения данного файла не поддерживаются. Для подробностей --help");

            return Result.Ok(filePath);
        }

        public static Result<Color> GetColor(string colorName)
        {
            var color = Color.FromName(colorName);
            return color.IsKnownColor
                ? Result.Ok(color)
                : Result.Fail<Color>($"ArgumentException: неизвестный цвет {colorName}");
        }

        public static Result<FontFamily> GetFontFamily(string fontFamilyName)
        {
            try
            {
                var value = new FontFamily(fontFamilyName);
                return Result.Ok(value);
            }
            catch (Exception)
            {
                return Result.Fail<FontFamily>(
                    "ArgumentException. Аргумент -f: введенный вами шрифт не поддерживается. Для подробностей --help");
            }
        }

        public static Result<bool> IsCorrectFormat(string format)
        {
            if (format == "png" || format == "jpg" || format == "jpeg" || format == "bmp")
                return Result.Ok(true);

            return Result.Fail<bool>(
                "ArgumentException. Аргумент -r: введенный вами формат не поддерживается. Для подробностей --help");
        }

        public static Result<Size> GetSize(string size)
        {
            var widthAndHeight = size.Split('x', 'х');

            Size result;
            if (int.TryParse(widthAndHeight[0], out var width) &&
                int.TryParse(widthAndHeight[1], out var height) &&
                (width > 0 && height > 0))
                result = new Size(width, height);
            else
                return Result.Fail<Size>(
                    "ArgumentException. Аргумент -s: введенный вами размер некоректен. Для подробностей --help");
            return result;
        }
    }
}
