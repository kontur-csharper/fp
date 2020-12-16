using System;
using System.Drawing;
using System.IO;
using ResultOf;

namespace TagCloud
{
    public class ArgumentParser
    {
        private static IPathCreator pathCreator = new PathCreator();
        
        public static Result<Background> GetBackground(string background)
        {
            switch (background)
            {
                case "empty":
                    return Background.Empty;
                case "circle":
                    return Background.Circle;
                case "rectangles":
                    return Background.Rectangles;
                default:
                    return Result.Fail<Background>("Unknown background type");
            }
        }
        
        public static Result<Size> GetSize(string size)
        {
            var arr = size.Split(',');
            if (arr.Length == 2 && int.TryParse(arr[0], out var width) && int.TryParse(arr[1], out var height))
            {
                if (width <= 0 || height <= 0)
                {
                    return Result.Fail<Size>("Width and height must be more then zero");
                }
                return new Size(width, height);
            }
            
            return Result.Fail<Size>("Incorrect size argument");
        }

        public static Result<string> CheckFileName(string fileName)
        {
            if (!File.Exists(pathCreator.GetPathToFile(fileName)))
            {
                return Result.Fail<string>("Input file not found");
            }
            if (!fileName.EndsWith(".txt") && !fileName.EndsWith(".docx"))
            {
                return Result.Fail<string>(@"Not supported format. Expected: .txt\.docx");
            }
            return fileName;
        }

        public static Result<FontFamily> GetFont(string font)
        {
            try
            {
                return new FontFamily(font);
            }
            catch (ArgumentException)
            {
                return Result.Fail<FontFamily>("Unknown FontFamily");
            }
        }
        
        public static Result<Color> ParseColor(string colorInRGB)
        {
            var arr = colorInRGB.Split(',');
            if (arr.Length == 3
                && TryGetColorComponent(arr[0], out var red)
                && TryGetColorComponent(arr[1], out var green)
                && TryGetColorComponent(arr[2], out var blue))
            {
                return Color.FromArgb(red, green, blue);
            }
            return Result.Fail<Color>($"Incorrect color format. Given: {colorInRGB}. Expected: 0-255,0-255,0-255");
        }
        
        private static bool TryGetColorComponent(string colorComponent, out int value)
        {
            if (int.TryParse(colorComponent, out var intColorComponent))
            {
                value = intColorComponent;
                return intColorComponent >= 0 && intColorComponent <= 255;
            }

            value = 0;
            return false;
        }
    }
}