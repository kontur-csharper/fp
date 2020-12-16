using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Autofac;
using Autofac.Core;
using CommandLine;
using HomeExercise.Helpers;
using HomeExercise.Settings;
using ResultOf;

namespace HomeExercise
{
    public class ConsoleCloudClient : IConsoleCloudClient
    {
        public void HandleSettingsFromConsole(string[] args, ContainerBuilder builder)
        {
            Result
                .Ok(Parser.Default)
                .Then(p => p.ParseArguments<Options.Options>(args))
                .Then(p => p.WithParsed(options =>
                {
                    HandleFilesOption(options.WordsPath, options.BoringPath, builder);
                    HandleWordOption(options.Font, options.Coefficient, builder);
                    HandleSpiralOption(options.CenterX, options.CenterY, builder);
                    HandlePainterOption(options.Wight, options.Height, options.ImageName, options.Format, options.Color,
                        builder);
                }))
                .OnFail(Console.WriteLine);
        }

        private void HandleFilesOption(string wordPath, string boringWordPath, ContainerBuilder builder)
        {
            Result.Ok(wordPath).Then(CheckWordsPathCorrectly).OnFail(Console.WriteLine);
            var boringWordPathResult = Result.Ok(boringWordPath).Then(CheckBoringWordsPathCorrectly).OnFail(Console.WriteLine);
            if (!boringWordPathResult.IsSuccess)
                boringWordPath = Environment.CurrentDirectory + @"\defaultBoringWords.txt";
            
            builder.RegisterType<FileProcessor>().As<IFileProcessor>()
                .WithParameters(new Parameter[] 
                {new NamedParameter("pathWords", wordPath), 
                    new NamedParameter("pathBoringWords", boringWordPath) });
        }

        private Result<None> CheckWordsPathCorrectly(string wordPath)
        {
            if (string.IsNullOrWhiteSpace(wordPath))
                return Result.Fail<None>($"Null or WhiteSpace path - wordPath:{wordPath}");
            if(string.IsNullOrEmpty(wordPath))
                return Result.Fail<None>($"Null or Empty path - wordPath:{wordPath}");
            return Result.Ok();
        }
        private Result<None> CheckBoringWordsPathCorrectly(string boringWordPath)
        {
            if(string.IsNullOrWhiteSpace(boringWordPath))
                return Result.Fail<None>($"Null or WhiteSpace path - boringWordPath:{boringWordPath}"
                                         + Environment.NewLine +"Boring words will be used by default");
            if(string.IsNullOrEmpty(boringWordPath))
                return Result.Fail<None>($"Null or WhiteSpace path - boringWordPath:{boringWordPath}"
                                         + Environment.NewLine +"Boring words will be used by default");
            return Result.Ok();
        }

        private void HandleWordOption(string fontText, int coefficient, ContainerBuilder builder)
        {
            var resultFont = Result
                .Of(() => FontFamilyHelper.GetFontFamily(fontText))
                .RefineError($"{ToString()}.HandleWordOption")
                .OnFail(Console.WriteLine);

            if (!resultFont.IsSuccess) 
            {
                Console.WriteLine("The incorrect font has been replaced with the default font (Microsoft Sans Serif)");
                resultFont = new Result<FontFamily>(null, new FontFamily("Microsoft Sans Serif"));
            }
                
            var wordSettings = new WordSettings(resultFont.Value, coefficient);
            builder.RegisterInstance(wordSettings).As<WordSettings>();
        }
        
        private void HandleSpiralOption(int x, int y, ContainerBuilder builder)
        {
            var center = new Point(x,y);
            var spiralSettings = new SpiralSettings(center);
            builder.RegisterInstance(spiralSettings).As<SpiralSettings>();
        }

        private void HandlePainterOption(int width, int height, string fileName, string format, int colorNumber, ContainerBuilder builder)
        {
            var size = GetSize(width, height);
            var font = GetFont(format);
            var color = GetColor(colorNumber);

            var painterSettings = new PainterSettings(size,fileName, font, color);
            builder.RegisterInstance(painterSettings).As<PainterSettings>();
        }

        private Size GetSize(int width, int height)
        {
            var sizeResult = CheckSize(width, height).OnFail(Console.WriteLine);
            if (!sizeResult.IsSuccess)
            {
                Console.WriteLine("The incorrect size has been replaced with the default size (3000,3000)");
                sizeResult = new Result<Size>(null, new Size(3000, 3000));
            }

            return sizeResult.Value;
        }
        
        private Result<Size> CheckSize(int width, int height)
        {
            if (width <= 0 || height <= 0)
                return Result.Fail<Size>("The incorrect image size");

            return new Result<Size>(null,new Size(width, height));
        }

        private ImageFormat GetFont(string format)
        {
            var formatResult = GetImageFormat(format).OnFail(Console.WriteLine);
            if (!formatResult.IsSuccess)
            {
                Console.WriteLine("The incorrect format has been replaced with the default format (Png)");
                formatResult = new Result<ImageFormat>(null, ImageFormat.Png);
            }

            return formatResult.Value;
        }

        private Color GetColor(int colorNumber)
        {
            var names = (KnownColor[]) Enum.GetValues(typeof(KnownColor));
            
            var colorResult = Result.Of(() => names[colorNumber]).Then(Color.FromKnownColor).OnFail(Console.WriteLine);
            
            if (!colorResult.IsSuccess)
            {
                Console.WriteLine("The incorrect color has been replaced with the default color (Color.DeepPink)");
                colorResult = new Result<Color>(null, Color.DeepPink);
            }

            return colorResult.Value;
        }
        
        private static Result<ImageFormat> GetImageFormat(string extension)  
        {
            var prop = typeof(ImageFormat)
                .GetProperties()
                .FirstOrDefault(p => p.Name.Equals(extension, StringComparison.InvariantCultureIgnoreCase));

            if(prop == null)
                return Result.Fail<ImageFormat>("The incorrect image format");
            
            var result = Result.Of(() =>prop.GetValue(prop) as ImageFormat);
    
            return result;  
        }
    }
}