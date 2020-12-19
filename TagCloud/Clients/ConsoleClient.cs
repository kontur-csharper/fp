using System;
using TagCloud.TextConverters.TextReaders;
using TagCloud.TextConverters.TextProcessors;
using TagCloud.WordsMetrics;
using TagCloud.Visualization;
using TagCloud.Visualization.WordsColorings;
using TagCloud.CloudLayoters;
using TagCloud.PointGetters;
using ResultOf;

namespace TagCloud.Clients
{
    internal class ConsoleClient : IClient
    {
        private ITextProcessor processor;
        private IWordsMetric metric;
        private ICloudLayoter layoter;
        private IPointGetter pointGetter;

        private VisualizationInfo vizInfo;

        public void Run()
        {
            Console.WriteLine("Hello, I'm your personal visualization client");
            while (true)
            {
                Console.WriteLine("Write path to file with text or \"exit\" to exit");
                var answear = Console.ReadLine();
                if (answear == "exit")
                    break;
                if (!ReadFileIsSuccess(answear, out var text))
                    continue;
                if (!ReadTagInfoIsSuccess())
                    continue;
                if (!ReadVisualizationInfoIsSuccess())
                    continue;
                Console.WriteLine("write path file to save");
                answear = Console.ReadLine();
                Visualizate(text, answear);
                Console.WriteLine("file save");
            }
        }

        private bool ReadFileIsSuccess(string path, out string text)
        {
            text = null;
            if (!TryGetValueOrWriteError(TextReaderAssosiation.GetTextReader(path), out var reader))
                return false;
            var textResut = reader.ReadText(path);
            if (!textResut.IsSuccess)
            {
                Console.WriteLine(textResut.Error);
                return false;
            }
            text = textResut.GetValueOrThrow();
            if (text == null)
            {
                Console.WriteLine("something was wrong, please, try again");
                return false;
            }
            return true;
        }

        private bool ReadTagInfoIsSuccess()
        {
            Console.WriteLine("write params cloud configuration");
            Console.WriteLine($"type of text processor, write {TextProcessorAssosiation.paragraph} or {TextProcessorAssosiation.words}");
            if (!TryGetValueOrWriteError(TextProcessorAssosiation.GetProcessor(Console.ReadLine()), out processor))
                return false;
            Console.WriteLine("Name of metric. Write \"count\"");
            if (!TryGetValueOrWriteError(WordsMetricAssosiation.GetMetric(Console.ReadLine()), out metric))
                return false;
            Console.WriteLine($"type of point getter, write {PointGetterAssosiation.circle} or {PointGetterAssosiation.spiral}");
            if (!TryGetValueOrWriteError(PointGetterAssosiation.GetPointGetter(Console.ReadLine()), out pointGetter))
                return false;
            Console.WriteLine($"type of layoter, write {CloudLayoterAssosiation.density} or {CloudLayoterAssosiation.identity}");
            if (!TryGetValueOrWriteError(CloudLayoterAssosiation.GetCloudLayoter(Console.ReadLine()), out layoter))
                return false;
            layoter.SetPointGetterIfNull(pointGetter);
            return true;
        }

        private bool ReadVisualizationInfoIsSuccess()
        {
            Console.WriteLine("write Visualizate configuration (3 parametrs - size, coloring and font)");
            SetVisualizateInfo();
            return vizInfo != null;
        }

        private void SetVisualizateInfo()
        {
            vizInfo = null;
            Console.WriteLine("size to cut picture, write two numbers or \"dynamic\" for dynamic size");
            if (!TryGetValueOrWriteError(VisualizationInfo.ReadSize(Console.ReadLine()), out var size))
                return;
            Console.WriteLine("coloring text, write red, geen, blue, black, random, multi, line red, line green, line blue, line random");
            if (!TryGetValueOrWriteError(WordsColoringAssosiation.GetColoring(Console.ReadLine()), out var coloring))
                return;
            Console.WriteLine("font of words, write Arial, Calibri, ...");
            var font = Console.ReadLine();
            if (!VisualizationInfo.FontIsCorrect(font))
            {
                Console.WriteLine("Not correct font");
                return;
            }
            vizInfo = new VisualizationInfo(coloring, size, font);
        }

        private bool TryGetValueOrWriteError<T>(Result<T> result, out T value)
        {
            if (!result.IsSuccess)
            {
                Console.WriteLine(result.Error);
                value = default;
            }
            else
                value = result.GetValueOrThrow();
            return result.IsSuccess;
        }

        public void Visualizate(string text, string picturePath)
        {
            var tagCloud = AlgorithmTagCloud.GetTagCloud(text, layoter, processor, metric);
            if(vizInfo.TryGetSize(out var size) && (size.Width < tagCloud.Width || size.Height < tagCloud.Height))
            {
                Console.WriteLine($"so small size, width and height should be more or equals: {tagCloud.Width}, {tagCloud.Height}");
                return;
            }
            TagCloudVisualization.Visualize(tagCloud, picturePath, vizInfo);
        }
    }
}
