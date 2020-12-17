using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security;
using ResultOf;

namespace TagsCloud.BitmapSaver
{
    public static class BitmapSaver
    {
        private static readonly Dictionary<string, ImageFormat> _supportedFormats =
            new Dictionary<string, ImageFormat>(StringComparer.CurrentCultureIgnoreCase)
            {
                {"png", ImageFormat.Png},
                {"jpeg", ImageFormat.Jpeg},
                {"bmp", ImageFormat.Bmp},
                {"jpg", ImageFormat.Jpeg}
            };

        public static Result<string> Save(Bitmap bitmap, string format, string directoryPath = ".")
        {
            if (!_supportedFormats.ContainsKey(format))
                return Result.Fail<string>("This file format is not supported.");

            if (!Directory.Exists(directoryPath))
                return Result.Fail<string>("This directory does not exist.");

            var imageFormat = _supportedFormats[format];
            var filePath = Path.GetFullPath(CreateFilePath(directoryPath, format));

            bitmap.Save(filePath, imageFormat);

            return Result.Ok(filePath);
        }

        private static string CreateFilePath(string directoryPath, string format)
        {
            var fileName = DateTime.Now.ToString("yyyyMMddhhmmss");
            return Path.Join(directoryPath, $"{fileName}.{format.ToLower()}");
        }
    }
}
