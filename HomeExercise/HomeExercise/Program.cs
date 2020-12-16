using System;
using Autofac;
using ResultOf;

namespace HomeExercise
{
    class Program
    {
        static void Main(string[] args)
        {
            var wordsPath = @"aaa.txt";
            var boringWordsPath = @"bw.txt";

            //args = new[] {"options", "--words", wordsPath, "--format", "bp", "--color", "7000", "-c", "24", "--imageName", "aa"};
            args = new[] {"options", "--words", wordsPath, "--font", "Comic Sans MS", "-h", "-2222", "-w", "-2000"};
            //args = new[] {"options", "--words", wordsPath, "--boring", boringWordsPath, "--format", "bmpjjj", "--color", "70", "-c", "29", "--imageName", "ff"};
            //args = new[] {"options", "--words", wordsPath};

            var builder = new ContainerBuilder();
            Result.Of(BuildConsole).Then(c => c.HandleSettingsFromConsole(args, builder)).OnFail(Console.WriteLine);
            Result.Of(() => BuildPainter(builder)).Then(p => p.DrawFigures()).OnFail(Console.WriteLine);
        }

        private static IConsoleCloudClient BuildConsole()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleCloudClient>().As<IConsoleCloudClient>();
            var consoleContainer = builder.Build();
            return consoleContainer.Resolve<IConsoleCloudClient>();
        }
        
        private static IPainter BuildPainter(ContainerBuilder builder)
        {
            builder.RegisterType<WordsProcessor>().As<IWordsProcessor>();
            builder.RegisterType<Spiral>().As<ISpiral>();
            builder.RegisterType<Word>().As<IWord>();
            builder.RegisterType<WordCloud>().As<IWordCloud>();
            builder.RegisterType<CircularCloudLayouter>().As<ICircularCloudLayouter>();
            builder.RegisterType<WordCloudPainter>().As<IPainter>();
            var container = builder.Build();
            return container.Resolve<IPainter>();
        }
    }
}