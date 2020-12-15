using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ResultOf;
using TagsCloudContainer.TagsCloudVisualization.Interfaces;

namespace TagsCloudContainer.TagsCloudVisualization
{
    public class BitmapSaver : IBitmapSaver
    {
        public void SaveBitmapToDirectory(Bitmap imageBitmap, string savePath)
        {
            Result.Ok(savePath)
                .Then(ValidateDirectoryPath)
                .Then(ValidateFileName)
                .OnFail(e => throw new ArgumentException(e, nameof(savePath)));

            Result.Ok(imageBitmap)
                .Then(ValidateBitmap)
                .OnFail(e => throw new ArgumentException(e, nameof(imageBitmap)))
                .Then(x =>
                {
                    using (x)
                    {
                        x.Save(savePath, ImageFormat.Png);
                    }
                });
        }

        private Result<Bitmap> ValidateBitmap(Bitmap bitmap)
        {
            return Validate(bitmap, x => x == null, "Bitmap is null");
        }

        private Result<string> ValidateDirectoryPath(string path)
        {
            var directoryName = Path.GetDirectoryName(path);
            return Validate(path, x => !Directory.Exists(directoryName), $"Directory {directoryName} doesnt exists");
        }

        private Result<string> ValidateFileName(string path)
        {
            var fileName = Path.GetFileName(path);
            var extension = Path.GetExtension(fileName);
            return Validate(path, x => extension == "", $"Wrong file name: {fileName}");
        }

        private Result<T> Validate<T>(T obj, Func<T, bool> predicate, string exception)
        {
            return predicate(obj)
                ? Result.Fail<T>(exception)
                : Result.Ok(obj);
        }
    }
}