using System;
using TagCloud.Drawers;
using TagCloud.TextReaders;
using TagCloud.WordsAnalyzer;
using TagCloud.ImageSavers;

namespace TagCloud
{
    public class TagCloud
    {
        private readonly ITagDrawer drawer;
        private readonly ITextReader textReader;
        private readonly IWordsAnalyzer wordsAnalyzer;
        private readonly IImageSaver imageSaver;
        
        public TagCloud(ITagDrawer drawer, ITextReader textReader, IWordsAnalyzer wordsAnalyzer, IImageSaver imageSaver)
        {
            this.drawer = drawer;
            this.textReader = textReader;
            this.wordsAnalyzer = wordsAnalyzer;
            this.imageSaver = imageSaver;
        }

        public void MakeTagCloud()
        {
            textReader.ReadWords()
                .Then(words => wordsAnalyzer.GetTags(words))
                .Then(tags => drawer.DrawTagCloud(tags))
                .Then(bitmap => imageSaver.Save(bitmap))
                .OnFail(Console.WriteLine);
        }
    }
}