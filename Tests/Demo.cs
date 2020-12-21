using System;
using System.Drawing;
using System.IO;
using Autofac;
using Autofac.Core;
using NUnit.Framework;
using WordCloudGenerator;

namespace Tests
{
    [TestFixture]
    public class Demo
    {
        [Test]
        public void Demo1()
        {
            var textPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "text.txt");
            var text = Reader.ReadFile(textPath);
            var preparer = new Preparer(new[] {"что", "если", "это", "как"}, word => word.Length > 3);
            var prepared = preparer.CreateWordFreqList(text, 200);
            var algorithm = AlgorithmFabric.Create(AlgorithmType.Exponential);
            var graphicalWords = algorithm(prepared);
            var painter = new Painter(FontFamily.GenericSansSerif, 
                new RandomChoicePalette(new[]
                    {Color.Peru, Color.Pink, Color.Green, Color.Red, Color.Blue, Color.Black}, Color.White),
                size => new CircularLayouter(size));

            var img = painter.Paint(graphicalWords);

            var path = Path.Combine(Directory.GetCurrentDirectory(), "demo1.jpg");
            Saver.SaveImage(img, path);
            
            Console.WriteLine($"Изображение сохранено в {path}");
        }
    }
}