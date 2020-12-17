using System;
using System.Drawing;
using System.IO;
using System.Security;

namespace TagCloud.Infrastructure.Graphics
{
    public class ImageSaver
    {
        private readonly Func<Settings.Settings> settingsFactory;

        public ImageSaver(Func<Settings.Settings> settingsFactory)
        {
            this.settingsFactory = settingsFactory;
        }

        public Result<string> Save(Image image)
        {
            var path = "";
            try
            {
                var imagePath = settingsFactory().ImagePath;
                var withExtension = SetExtension(imagePath, settingsFactory().Format.ToString().ToLowerInvariant());
                path = Path.GetFullPath(withExtension);
                image.Save(path, settingsFactory().Format);
            }
            catch (SecurityException)
            {
                return Result.Fail<string>($"It is not possible to write file {path}");
            }
            catch (NotSupportedException)
            {
                return Result.Fail<string>("Colon (:) in path is not allowed ");
            }
            catch (PathTooLongException)
            {
                return Result.Fail<string>("Path is too long");
            }
            catch
            {
                // todo Logger.fatal();
                return Result.Fail<string>("Something went wrong");
            }

            return $"Image saved into {path}";
        }

        private static string SetExtension(string filename, string extension)
        {
            return Path.ChangeExtension(filename, extension);
        }
    }
}