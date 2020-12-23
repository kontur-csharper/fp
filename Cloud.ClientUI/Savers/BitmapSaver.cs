using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using TagsCloudVisualization;

namespace Cloud.ClientUI
{
    public class BitmapSaver : ISaver
    {
        public Result<None> SaveImage(Bitmap bitmap, string fileName)
        {
            return CheckValidFileName(fileName)
                .Then(x => Save(bitmap, x));
        }

        private static Result<None> Save(Bitmap bitmap, string fileName)
        {
            var path = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}{fileName}.bmp";
            bitmap.Save(path, ImageFormat.Bmp);
            return Result.Ok();
        }

        private static Result<string> CheckValidFileName(string name)
        {
            var invalidSpecialCharacters = "/\\:*?\"<>|".ToCharArray();
            return name.Any(letter => invalidSpecialCharacters.Contains(letter))
                ? Result.Fail<string>("File name contains invalid characters")
                : Result.Ok(name);
        }
    }
}